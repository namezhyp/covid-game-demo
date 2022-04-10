using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int money_set = 0;
        private int id_set = 0; //身份
        private int bornSpace_set = 0; //出身地
        private int store_set = 0;  //初始物资
        private int job_set = 0;   //职业
        private int time_set = 0;  //持续时间
        private int difficulty_set = 0;  //严重程度
        private int spread_set = 0;  //传播效率
        private int ideology_set = 0;  //社会风气
        private int supply_set = 0;  //物资供应
        //以上是允许玩家设置的参数

        //set类都是用int,底下的都是实际参与运算的值，不一定要int

        //以下参数不直接显示                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        private int money = 0;
        private int id = 0; //身份
        private int bornSpace = 0; //出身地
        private int meat_store = 0;  //初始肉
        private int vege_store = 0;  //初始菜
        private int fastfood_store = 0;  //初始方便食品
        private int basefood_store = 0; // 初始主食
        private int job = 0;   //职业
        private int time = 0;  //持续时间
        private int difficulty = 0;  //严重程度
        private int spread = 0;  //传播效率
        private int ideology = 0;  //社会风气
        private int supply = 0;  //物资供应

        private int now_day = 1;//当前天数
        string[] now_time = new string[] { "0", "00" };  //当前时间 注意声明方式
        private int death = 0;
        private int infected = 0;//感染者数
        private int non_symptom = 0; //无症状者数
        private int mental = 100;//精神，0-100  四个均允许溢出
        private int health = 100;//健康，0-100
        private int hungry = 100;//饥饿度，0-100
        private int tire = 100;//劳累度，0-100

        bool worktime = false; //判断是否工作时间 8:00-21:00
        bool license = false;//外出证明
        struct rna_about
        {
            public bool report ;//是否有核酸证明
            public bool tested; //是否在早9点做了核酸
            public int day ;
            public int hour ;  //核酸报告的最后有效时间
            public bool infect;  //是否感染
        }  //核酸相关 不写public其他函数无法访问
        rna_about rna;

        
        int worked_time = 0;//七天内已工作次数
        int one_day_work = 0;//单日已工作次数
        bool sleeping = false;//当前是否处于睡觉状态
        bool event_choose = false; //随机事件的选择结果



        int which_event = 0;//无法解决线程问题 用这个去区分事件
        //0无效 1 核酸检测

        int gameMode = 0; //游戏模式 0肉鸽 1普通 2无尽
        public string chuanzhi = "";//用于跨窗口传值
        SoundPlayer bgm;

        public Form1()
        {
            InitializeComponent();
            ComboBox_init();
            string path = Environment.CurrentDirectory + "\\test1.wav";
            showBGM(path);
        }

        private void showBGM(string path)//播放BGM
        {
            bgm = new SoundPlayer(path);
            bgm.Play();
        }

        private void ComboBox_init()  //所有comboBox的初始化
        {
            comboBox1.Items.Add("外国人");
            comboBox1.Items.Add("本地居民");
            comboBox1.Items.Add("外来居民");
            //comboBox1.Items.Add("工作人员");
            comboBox6.Items.Add("A国");
            comboBox6.Items.Add("B国");
            comboBox5.Items.Add("极其丰富");
            comboBox5.Items.Add("十分充足");
            comboBox5.Items.Add("常见储备");
            comboBox5.Items.Add("少有存粮");
            comboBox5.Items.Add("缺粮少油");
            comboBox4.Items.Add("公司高层");
            comboBox4.Items.Add("程序员");
            comboBox4.Items.Add("厨师");
            comboBox4.Items.Add("制造业工人");
            comboBox4.Items.Add("物流快递业");
            comboBox4.Items.Add("行政人员");
            comboBox4.Items.Add("学生");
            comboBox4.Items.Add("无业");
            comboBox7.Items.Add("较短");
            comboBox7.Items.Add("一般");
            comboBox7.Items.Add("较长");
            comboBox7.Items.Add("漫长");
            comboBox3.Items.Add("just a flu");
            comboBox3.Items.Add("一般");
            comboBox3.Items.Add("'just a flu'");
            comboBox8.Items.Add("难以传播");
            comboBox8.Items.Add("普通传播速率");
            comboBox8.Items.Add("极易传播");
            comboBox2.Items.Add("秩序");
            comboBox2.Items.Add("无序");
            comboBox9.Items.Add("供应充足");
            comboBox9.Items.Add("稍有限制");
            comboBox9.Items.Add("供应紧缺");
            comboBox9.Items.Add("急缺物资");
        }

        private void mainGame()  //主函数 但实质主函数是时间推进
        {

            textBox2.Text += "\r\n新游戏开始";
            label6.Text = money.ToString();
            checkState();
            display_now_store();
        }

        private void 停止当前游戏ToolStripMenuItem_Click(object sender, EventArgs e) //重开
        {
            panel1.Visible = false;
            panel3.Visible = false;
            data_init();//初始化隐藏参数
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        protected override void OnClosing(CancelEventArgs e)  //退出提醒
        {
            DialogResult result = MessageBox.Show("是否确认关闭？\n关闭后游戏记录将丢失", "退出提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            e.Cancel = result != DialogResult.Yes;
            base.OnClosing(e);
        }

        private void aBoutToolStripMenuItem_Click(object sender, EventArgs e)//关于页面
        {
            Form about = new Form2();
            about.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        } //无用

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) //无用
        {

        }

        private void Form1_Load(object sender, EventArgs e) //加载函数，重要
        {

        }

        private void button3_Click(object sender, EventArgs e)//自定义按钮
        {
            label18.Visible = true;
            comboBox7.Visible = true;//允许修改时间
            comboBox7.Text = "";  //删除无尽按钮的影响
            panel1.Visible = true;
            gameMode = 1;
        }

        private void button4_Click(object sender, EventArgs e)//无尽按钮
        {
            panel1.Visible = true;
            label18.Visible = false;    //进入参数页面
            comboBox7.Visible = false;  //禁止修改时间
            comboBox7.Text = "无尽";
            gameMode = 2;
        }

        private void button2_Click(object sender, EventArgs e)  //rouge按钮
        {
            //肉鸽不经过参数设置，所以需要自己随机参数
            data_init();//参数初始化

            Random ra = new Random();
            money = ra.Next(0, 15000);
            id = ra.Next(0, 2);
            bornSpace = ra.Next(0, 1);
            meat_store = ra.Next(1, 13);
            vege_store = ra.Next(4, 16);
            fastfood_store = ra.Next(1, 10);
            basefood_store = ra.Next(8, 25);
            difficulty = ra.Next(0, 2);
            spread = ra.Next(0, 2);
            ideology = ra.Next(0, 1);
            job = ra.Next(0, 7);
            time = ra.Next(10, 50);    //玩家参数准备

            hungry = ra.Next(40, 100);    //隐藏参数准备
            mental = ra.Next(50, 100);
            tire = ra.Next(20, 95);
            health = ra.Next(50, 100);

            textBox2.Text = "rouge开始";
            label6.Text = money.ToString();
            panel3.Visible = true;
            gameMode = 0;
            mainGame();
        }

        private void button8_Click(object sender, EventArgs e) //开始游戏按钮
        {
            //按下以后，首先应确保所有值都填好了
            //这个按钮出现在选普通和无尽模式时
            if (comboBox1.Text == "" || comboBox2.Text == ""
                || comboBox3.Text == "" || comboBox4.Text == ""
                || comboBox5.Text == "" || comboBox6.Text == ""
                || comboBox7.Text == "" || comboBox8.Text == ""
                || comboBox9.Text == "" || textBox1.Text == "")  //注意不能用Null判断
            {
                label20.Text = "有参数未设置";
            }
            else  //设置好参数，开始游戏
            {
                Random ra = new Random();
                money = money_set;
                id = id_set; //身份
                bornSpace = bornSpace_set; //出身地
                if (store_set == 0)  //初始储备
                {
                    meat_store = ra.Next(12, 15);
                    vege_store = ra.Next(12, 20);
                    fastfood_store = ra.Next(8, 10);
                    basefood_store = ra.Next(15, 20);
                }
                if (store_set == 1)
                {
                    meat_store = ra.Next(7, 10);
                    vege_store = ra.Next(9, 14);
                    fastfood_store = ra.Next(4, 5);
                    basefood_store = ra.Next(12, 16);
                }
                if (store_set == 2)
                {
                    meat_store = ra.Next(2, 6);
                    vege_store = ra.Next(5, 8);
                    fastfood_store = ra.Next(0, 2);
                    basefood_store = ra.Next(10, 12);
                }
                job = job_set;   //职业

                if (time_set == 0) time = ra.Next(8, 12); //时间设置
                if (time_set == 1) time = ra.Next(13, 20);
                if (time_set == 2) time = ra.Next(20, 27);
                if (time_set == 3) time = ra.Next(27, 40);
                if (time_set == 4) time = 9999;
                difficulty = difficulty_set;  //严重程度
                spread = spread_set;  //传播效率
                ideology = ideology_set;  //社会风气

                panel1.Visible = false;
                panel3.Visible = true;
                textBox2.Text = "开始游戏";

                data_init();//参数初始化
                mainGame();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//初始金钱限制只能输入数字
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)//初始金钱限制最大值
        {
            int iMax = 10000000;//首先设置上限值
            if (textBox1.Text != null && textBox1.Text != "")//判断TextBox的内容不为空，如果不判断会导致后面的非数字对比异常
            {
                if (int.Parse(textBox1.Text) > iMax)//num就是传进来的值,如果大于上限（输入的值），那就强制为上限-1
                {
                    textBox1.Text = (iMax - 1).ToString();
                }
            }
        }

        private void label9_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "开局时的身份信息";
        } //以下为提示信息

        private void label17_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "病毒的传播效率";
        }

        private void label18_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "完整疫情从开始到结束的时间";
        }

        private void label15_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "开局时的国家位置，与现实无关";
        }

        private void comboBox5_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "初始持有的物资，包括肉、菜、主食和方便食品";
        }

        private void comboBox9_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "物资供应的紧缺程度";
        }

        private void label11_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "不同职业会有不同的开局信息与加成，甚至减损";
        }

        private void label13_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "得病后的严重程度";
        }

        private void label24_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "物资供应的紧缺程度";
        }

        private void label10_MouseMove(object sender, MouseEventArgs e)
        {
            label23.Text = "开局时所能使用的金钱";
        }

        private void label16_MouseMove(object sender, MouseEventArgs e)//社会风气提示
        {
            label23.Text = "整个社会对疫情处理的态度";
        }
        //以上为提示信息
        private void comboBox1_TextChanged(object sender, EventArgs e) //身份下拉
        {
            if (comboBox1.Text == "外国人") id_set = 0;
            if (comboBox1.Text == "本地居民") id_set = 1;
            if (comboBox1.Text == "外来居民") id_set = 2;
            //if (comboBox1.Text == "外国人") id_set = 0;
        }

        private void comboBox4_TextChanged(object sender, EventArgs e) //职业下拉
        {
            if (comboBox4.Text == "公司高层") job_set = 0;
            if (comboBox4.Text == "程序员") job_set = 1;
            if (comboBox4.Text == "厨师") job_set = 2;
            if (comboBox4.Text == "制造业工人") job_set = 3;
            if (comboBox4.Text == "物流快递业") job_set = 4;
            if (comboBox4.Text == "行政人员") job_set = 5;
            if (comboBox4.Text == "学生") job_set = 6;
            if (comboBox4.Text == "无业") job_set = 7;
        }

        private void comboBox3_TextChanged(object sender, EventArgs e) //严重程度下拉
        {
            if (comboBox3.Text == "just a flu") difficulty_set = 0;
            if (comboBox3.Text == "一般") difficulty_set = 1;
            if (comboBox3.Text == "'just a flu'") difficulty_set = 2;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e) //社会风气下拉
        {
            if (comboBox2.Text == "秩序") ideology_set = 0;
            if (comboBox2.Text == "无序") ideology_set = 1;
        }

        private void comboBox6_TextChanged(object sender, EventArgs e)//国家下拉
        {
            if (comboBox6.Text == "A国") bornSpace_set = 0;
            if (comboBox6.Text == "B国") bornSpace_set = 1;
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)//初始物资下拉
        {
            if (comboBox5.Text == "极其丰富") store_set = 0;
            if (comboBox5.Text == "十分充足") store_set = 1;
            if (comboBox5.Text == "常见储备") store_set = 2;
            if (comboBox5.Text == "少有存粮") store_set = 3;
            if (comboBox5.Text == "缺粮少油") store_set = 4;
        }

        private void comboBox7_TextChanged(object sender, EventArgs e) //持续时间下拉
        {
            if (comboBox7.Text == "较短") time_set = 0;
            if (comboBox7.Text == "一般") time_set = 1;
            if (comboBox7.Text == "较长") time_set = 2;
            if (comboBox7.Text == "漫长") time_set = 3;
            if (comboBox7.Text == "无尽") time_set = 4;
        }

        private void comboBox8_TextChanged(object sender, EventArgs e) //传播速率下拉
        {
            if (comboBox8.Text == "难以传播") spread_set = 0;
            if (comboBox8.Text == "普通传播速率") spread_set = 1;
            if (comboBox8.Text == "极易传播") spread_set = 2;
        }

        private void comboBox9_TextChanged(object sender, EventArgs e) //物资供应下拉
        {
            if (comboBox9.Text == "供应充足") supply_set = 0;
            if (comboBox9.Text == "稍有限制") supply_set = 1;
            if (comboBox9.Text == "供应紧缺") supply_set = 2;
            if (comboBox9.Text == "急缺物资") supply_set = 3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)  //金钱设置
        {
            money_set = Convert.ToInt32(textBox1.Text);
        }

        private void display_now_store()  //显示所有信息  仅供调试用
        {
            string nowState = "";
            nowState = "\r\n" + "金钱:" + money.ToString() + "身份:" + id.ToString() + "出身地:" + bornSpace.ToString() + "肉类储备:" + meat_store.ToString()
                + "\r\n蔬菜储备:" + vege_store.ToString() + "速食储备:" + fastfood_store.ToString() + "主食储备" + basefood_store.ToString() + "职业:" + job.ToString() +
                "严重程度:" + difficulty.ToString() + "\r\n传播速率:" + spread.ToString() + "社会风气:" + ideology.ToString() + "物资供应:" + supply.ToString()
                + "\r\n饥饿:" + hungry.ToString() + "劳累:" + tire.ToString()
                + "心情:" + mental.ToString() + "健康:" + health.ToString()
            + "\r\n感染数:" + infected.ToString();
            textBox2.Text += nowState;
        }

        private void data_init()  //参数初始化
        {
            //所有不能玩家设置的参数都要初始化
            now_day = 1;
            infected = 0; death = 0; non_symptom = 0;

            health = 100; mental = 100;
            hungry = 100; tire = 100;   //肉鸽模式属性随机，其他模式不是
            now_time[0] = "0";           //所以要在肉鸽随机前就使用函数
            now_time[1] = "00";
            worktime = false;   //判断是否工作时间 8:00-21:00
            license = false;    //外出证明
            rna.report = false;    //核酸证明
            rna.tested= false; //是否做了核酸
            rna.infect = false;
            worked_time = 0;    //已工作次数
            one_day_work = 0;   //单日已工作次数

            label25.Text = ""; //提示框要清空
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)  //吃饭提示
        {
            label25.Text = "消耗半小时做饭，半小时吃饭，回复饥饿、健康和精神。" +
                "\r\n优先做饭，无法做饭时会使用方便食品";
        }

        private void button5_MouseMove(object sender, MouseEventArgs e)  //睡觉提示
        {
            label25.Text = "消耗7小时睡觉，回复体力值和少许健康";
        }

        private void button2_MouseMove(object sender, MouseEventArgs e) //刷手机提示
        {
            label25.Text = "每次消耗半小时刷手机，精神回复\r\n也许能看到点消息";
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)  //玩电脑提示
        {
            label25.Text = "每次消耗一小时玩电脑，精神会回复，会很累";
        }

        private void button4_MouseMove(object sender, MouseEventArgs e)  //办公提醒
        {
            label25.Text = "消耗四小时居家工作，只能在白天进行，每天最多两次";
        }

        private void button9_MouseMove(object sender, MouseEventArgs e) //外出提醒
        {
            label25.Text = "外出闲逛，需要有出门证和48小时内核算报告" +
                "\r\n花费三小时在附近逛逛，或许看到什么";
        }

        private void change_nowMoney(int target) //修改当前钱数
        {
            money = target;
            label6.Text = money.ToString();
        }

        private void changeTime(int deltaTime)  //时间推进 只准此函数改时间
        {
            //只允许这个函数推进时间
            while (deltaTime > 0)  //以半小时为单位时间推进
            {
                deltaTime -= 30;
                if (now_time[1] == "00")
                {
                    now_time[1] = "30";
                    autoDecrease();
                }
                else if (now_time[1] == "30")
                {
                    now_time[1] = "00";
                    now_time[0] = (Convert.ToInt32(now_time[0]) + 1).ToString();
                    autoDecrease();
                }
                if (Convert.ToInt32(now_time[0]) > 24) //已经过了一天
                {
                    now_day++;
                    now_time[0] = (Convert.ToInt32(now_time[0]) - 24).ToString();
                }
                label26.Text = "当前时间:第" + now_day.ToString() + "天" + now_time[0].ToString()
                    + ":" + now_time[1].ToString();

                //以上是必有的时间推进功能
                //以下是各种事件


                if (time == now_day)
                {
                    finish();
                }//时间到了，到结局

                if (now_day % 7 == 0)
                {
                    give_money();
                }//结算工资

                if (now_time[0] == "9" && now_time[1]=="00")
                {
                    checkDna();
                }//每天9点核酸检测
                //做过核酸的话14点拿到结果
                if(Convert.ToInt32(now_time[0]) == 14 && rna.tested==true)
                {
                    textBox2.AppendText("\r\n核酸报告已经拿到了，结果是阴性");
                    rna.infect = false;
                    rna.report = true;
                    rna.day = now_day+2;
                    rna.hour = Convert.ToInt32(now_time[0]);
                }  
            }
        }

        private void autoDecrease() //随着时间自动扣饥饿和睡眠
        {
            //不同模式扣除不同
            if (gameMode == 0 || gameMode == 2)
            {
                hungry -= 3;    //每过半小时饥饿-3 睡眠-1
                tire -= 1;
            }
            else
            {
                hungry -= 4;    //自设模式-3 -2
                tire -= 3;
            }
        }

        private void checkState()  //更新玩家五属性 发生动作时就应调用
        {
            if (hungry > 80) label4.Text = "饱足";
            if (hungry <= 80 && hungry > 60) label4.Text = "一般";
            if (hungry <= 60 && hungry > 35) label4.Text = "饥饿";
            if (hungry <= 35) label4.Text = "营养不足";

            if (tire > 80) label21.Text = "精神充沛";
            if (tire <= 80 && tire > 55) label21.Text = "一般";
            if (tire <= 55 && tire > 35) label21.Text = "稍有疲倦";
            if (tire <= 35 && tire > 20) label21.Text = "十分疲倦";
            if (tire <= 20) label21.Text = "缺乏睡眠";

            if (mental > 80) label3.Text = "愉悦";
            if (mental <= 80 && tire > 45) label3.Text = "一般";
            if (mental <= 45 && tire > 25) label3.Text = "心情沉重";
            if (mental <= 25) label3.Text = "接近崩溃";

            if (health > 80) label28.Text = "十分健康";
            if (health <= 80 && tire > 55) label28.Text = "健康";
            if (health <= 55 && tire > 25) label28.Text = "身体不适";
            if (health <= 25) label28.Text = "需要治疗";

            label6.Text = money.ToString();
        }

        private void give_money()  //薪资计算
        {
            textBox2.AppendText("\r\n发放工资");
            money += 500 * worked_time;
            worked_time = 0;
            one_day_work = 0;
        }

        private void 导出当前记录ToolStripMenuItem_Click(object sender, EventArgs e) //导出记录
        {
            textBox2.AppendText("\r\n游戏记录已经导出到游戏文件夹下");
            string path = Environment.CurrentDirectory + "\\record.txt";
            using (StreamWriter se = File.CreateText(path))
            {
                se.Write(textBox2.Text);
                se.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)  //吃饭按钮  
        {
            //做饭必须有主食/方便食品
            //主食必须配肉/菜   方便食品可以不配 
            //优先做正餐
            //吃饭减劳累 加饥饿心情 但快餐扣心情
            //做饭吃一小时  方便食品半小时
            if (hungry >= 100) textBox2.AppendText("\r\n你已经很饱了！");
            else
            {
                if (basefood_store > 0 && (meat_store > 0 || vege_store > 0))
                {
                    textBox2.AppendText("\r\n你用一小时做了一顿饭");
                    basefood_store--;
                    hungry += 70;
                    mental += 10;
                    health += 10;
                    if (meat_store > 0) meat_store--;   //决定用什么配菜
                    else vege_store--;
                    changeTime(60);
                }
                else if (fastfood_store > 0)
                {
                    textBox2.AppendText("\r\n你用半小时吃了点方便食品");
                    fastfood_store--;
                    hungry += 50;
                    mental -= 5;
                    health += 5;
                    changeTime(30);
                }
                else
                {
                    textBox2.Text += "\r\n你的食物不足，无法做饭！";
                }
            }
            checkState();
        }

        private void button5_Click(object sender, EventArgs e) //睡觉按钮
        {
            if (tire >= 100) textBox2.AppendText("\r\n精神充沛，无需睡觉");
            else  //睡觉开始前后设置状态
            {
                sleeping = true;
                textBox2.AppendText("\r\n你睡了7小时觉");
                changeTime(420);
                tire += 80;
                mental += 30;
                sleeping = false;
            }
            checkState();
        }

        private void button2_Click_1(object sender, EventArgs e) //刷手机按钮
        {
            textBox2.AppendText("\r\n你刷了半小时手机"); //视觉相关应该首位
            changeTime(30);
            tire -= 5;           
            checkState();
        }

        private void button3_Click_1(object sender, EventArgs e) //玩电脑按钮
        {
            changeTime(60);
            tire -= 10;
            mental += 20;
            textBox2.AppendText("\r\n你刷了一小时电脑");
            checkState();
        }

        private void button4_Click_1(object sender, EventArgs e)  //工作按钮
        {
            //每日最多工作2次
            //每7天结一次钱
            worktime = is_work_time();
            if (worktime == false)
            {
                textBox2.AppendText("\r\n现在还不是工作的时间");
            }
            else
            {
                if (one_day_work >= 2)
                {
                    textBox2.AppendText("\r\n今日已经工作两次了");
                }
                else
                {
                    changeTime(240);
                    textBox2.AppendText("\r\n你工作了四小时");
                    mental -= 15;
                    one_day_work++;
                    checkState();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e) //外出按钮
        {
            if (license == false)
            {
                textBox2.AppendText("\r\n你缺少外出证，还不能出门");
            }
            else if (rna.report == false)
            {
                textBox2.AppendText("\r\n你缺少48小时的核酸证明，还不能出门");
            }
            else
            {
                changeTime(180);
                mental += 15; //外出加心情
            }
        }

        private bool is_work_time()  //判断是不是工作时间 有返回值
        {
            int hour = Convert.ToInt32(now_time[0]);
            //textBox2.AppendText("\r\n当前时间:"+hour.ToString());
            if (hour >= 8 && hour < 21) return true;
            else return false;

        }


        private void finish()  //结局  另开一个窗口
        {
            textBox2.Text = "游戏结束了，你坚持到了最后";
            chuanzhi = textBox2.Text.ToString();
            Form3 f3 = new Form3();
            f3.str = chuanzhi;    //跨窗口传值
            f3.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e) //显示储备按钮
        {
            string nowState = "";
            nowState = "\r\n肉类储备:" + meat_store.ToString()
                + "\r\n蔬菜储备:" + vege_store.ToString()
                + "\r\n速食储备:" + fastfood_store.ToString()
                + "\r\n主食储备:" + basefood_store.ToString();
            textBox2.AppendText(nowState);
        }

        private void time_randomEvent()  //随时间的随机事件触发
        {
            //这个函数应该在时间推进中一小时触发一次
            Random ra = new Random();
            int num = 0;
            worktime = is_work_time();
            if (ra.NextDouble() > 0.5 && worktime)  //晚上就不触发事件了
            {
                num = ra.Next(0, 10);
                switch (num)
                {
                    case 1: { textBox2.AppendText("\r\n事件1"); break; }
                    case 2: { textBox2.AppendText("\r\n事件2"); break; }
                    case 3: { textBox2.AppendText("\r\n事件3"); break; }
                    case 4: { textBox2.AppendText("\r\n事件4"); break; }
                    case 5: { textBox2.AppendText("\r\n事件5"); break; }
                    case 6: { textBox2.AppendText("\r\n事件6"); break; }
                    case 7: { textBox2.AppendText("\r\n事件7"); break; }
                    case 8: { textBox2.AppendText("\r\n事件8"); break; }
                    case 9: { textBox2.AppendText("\r\n事件9"); break; }
                    default: { textBox2.AppendText("这个小时无事发生"); break; }
                }
            }

        }

        private void checkDna() //核酸检测事件上 挂在时间推进里
        {
            if (sleeping == true)
            {
                textBox2.AppendText("\r\n你错过了早上9点的核酸检测" +
                    "\r\n大白敲门你没听见");
            }//睡觉则不发生
            else
            {
                unable_buttons();
                which_event = 1;
                button10.Visible = true;
                button11.Visible = true;
                textBox2.AppendText("\r\n现在是早上9点，要下楼去做核酸吗？");
                label25.Text = "\r\n现在是早上9点，要下楼去做核酸吗？";
            }
        }

        private void unable_buttons() //禁用六个按钮
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button9.Enabled = false;
        }

        private void enable_buttons() //启用六个按钮
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button9.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e) //"是"按钮
        {
            event_choose = true;
            textBox2.AppendText("\r\n调用了一次是按钮");
            deal_event(); //转到事件处理函数
            enable_buttons(); //恢复6个按钮使用
            button10.Visible = false;
            button11.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)//"否"按钮
        {
            event_choose = false;
            textBox2.AppendText("\r\n调用了一次否按钮");
            deal_event();
            enable_buttons();
            button10.Visible = false;
            button11.Visible = false;
        }

        private void deal_event()  //所有需要点选事件的后半段
        {
            switch(which_event)
            {
                case 1:  //核酸检测
                    {
                        which_event = 0;
                        if (event_choose)
                        {
                            rna.tested = true;
                            textBox2.AppendText("\r\n你已经做了核酸检测，五小时后出报告");
                        }
                        else
                        {
                            rna.tested = false;
                            textBox2.AppendText("\r\n你没有参加核酸检测，希望没人注意到");
                        }
                        break;
                    }
            }
        }

        private void button12_Click(object sender, EventArgs e) //手机买菜按钮
        {
            Form4 f4 = new Form4(money);            
            f4.Show();
        }
    }
}
