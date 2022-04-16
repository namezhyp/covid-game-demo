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
        public static int money = 0;
        private int id = 0; //身份
        private int bornSpace = 0; //出身地
        private int meat_store = 0;  //初始肉
        private int vege_store = 0;  //初始菜
        private int fastfood_store = 0;  //初始方便食品
        private int basefood_store = 0; // 初始主食
        private int medicine = 0; //初始药物
        private int job = 0;   //职业
        private int time = 0;  //持续时间
        private float difficulty = 0;  //严重程度
        private float spread = 0;  //传播效率
        private float ideology = 0;  //社会风气
        private float supply = 0;  //物资供应

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
            public bool infect_check; //是否已经被确认了感染
        }  //核酸相关 不写public其他函数无法访问
        rna_about rna;

        
        int worked_time = 0;//七天内已工作次数
        int one_day_work = 0;//单日已工作次数

        int deltaTime = 0; //当前事务剩余时间
        bool busy = false; //当前时间推进函数是否繁忙
        string main_thing ;  //记录当前玩家行为状态
        bool event_choose = false; //需要选择的随机事件的选择结果

        bool get_food;//抢到了菜
        public struct res 
        {
            public int meat_num;
            public int vege_num;
            public int basefood_num;
            public int fastfood_num;
            public int medicine_num;
            public int hour;//快递运达要多久
        };   //抢到菜以后要用的结构体
        public res r1 = new res(); //注意这里也要public

        int which_event = 0;//无法解决线程问题 用这个去区分事件 用于手动点选的随机事件
        //0无效 1 核酸检测

        int[][] exchange = new int[2][];//用于交换物资事件


        int gameMode = 0; //游戏模式 0肉鸽 1普通 2无尽
        public string chuanzhi = "";//用于跨窗口传值
        private int[] linshi= { 0,0,0,-1}; //用于向买菜窗口传新闻，确保每天新闻不会变
        SoundPlayer bgm;


        public Form1()
        {
            InitializeComponent();
            ComboBox_init();           
            showBGM();
        }

        private void showBGM()//播放BGM
        {
            //combine函数可以拼接路径
            //soundplayer 有个问题 无法获取是否已经播完了
            Random ra = new Random();
            string str = "test"+ra.Next(1, 3).ToString()+".wav";
            string path =Path.Combine(Environment.CurrentDirectory,"sounds",str);
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
            Form5 f5 = new Form5("\r\n由于疫情，你所在的城市暂时封锁了" +
                "\r\n游戏按照时间推进"+
                "\r\n用已有的物资生活到疫情结束吧");
            f5.Text = "提示";
            f5.Show();
            //textBox2.Text += "\r\n新游戏开始";
            if (job == 4) button4.Text = "外出工作"; //快递的工作是室外
            label6.Text = money.ToString();
            checkState();
            label26.Text = "当前时间：第1天0:00";
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
            money = ra.Next(0, 20000);
            id = ra.Next(0, 2);
            bornSpace = ra.Next(0, 1);
            meat_store = ra.Next(1, 13);
            vege_store = ra.Next(4, 16);
            fastfood_store = ra.Next(1, 10);
            basefood_store = ra.Next(8, 25);
            medicine = ra.Next(0, 5);
            difficulty = (float)Convert.ToDouble(ra.Next(0, 80))/100;
            spread = (float)Convert.ToDouble(ra.Next(5,70)) / 100;
            ideology = (float)Convert.ToDouble(ra.Next(5,70)) / 100;
            supply = (float)Convert.ToDouble(ra.Next(5, 80)) / 100;
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
                if (store_set == 0)  
                {
                    meat_store = ra.Next(12, 15);
                    vege_store = ra.Next(12, 20);
                    fastfood_store = ra.Next(8, 10);
                    basefood_store = ra.Next(15, 20);
                    medicine = ra.Next(3, 5);
                }//初始储备
                if (store_set == 1)
                {
                    meat_store = ra.Next(7, 10);
                    vege_store = ra.Next(9, 14);
                    fastfood_store = ra.Next(4, 5);
                    basefood_store = ra.Next(12, 16);
                    medicine = ra.Next(2,4);
                }
                if (store_set == 2)
                {
                    meat_store = ra.Next(2, 6);
                    vege_store = ra.Next(5, 8);
                    fastfood_store = ra.Next(0, 2);
                    basefood_store = ra.Next(10, 12);
                    medicine = ra.Next(0,2);
                }
                job = job_set;   //职业

                if (time_set == 0) time = ra.Next(8, 12); //时间设置
                if (time_set == 1) time = ra.Next(13, 20);
                if (time_set == 2) time = ra.Next(20, 27);
                if (time_set == 3) time = ra.Next(27, 40);
                if (time_set == 4) time = 9999;

                if (difficulty_set == 0) difficulty = ra.Next(10,30)/100;
                if (difficulty_set == 1) difficulty = ra.Next(40,65) / 100;
                if (difficulty_set == 2) difficulty = ra.Next(70,95) / 100;

                if (spread_set == 0) spread = ra.Next(10, 25) / 100;
                if (spread_set == 1) spread = ra.Next(30, 45) / 100;
                if (spread_set == 2) spread = ra.Next(50, 70) / 100;

                if (ideology_set == 0) ideology = ra.Next(0, 35) / 100;
                if (ideology_set == 1) ideology = ra.Next(25, 70) / 100;

                if (supply_set == 0) supply = ra.Next(0, 10) / 100;
                if (supply_set == 1) supply = ra.Next(10, 25) / 100;
                if (supply_set == 2) supply = ra.Next(30, 45) / 100;
                if (supply_set == 3) supply = ra.Next(40, 65) / 100;

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
           /* string nowState = "";
            nowState = "\r\n" + "金钱:" + money.ToString() + "身份:" + id.ToString() + "出身地:" + bornSpace.ToString() + "肉类储备:" + meat_store.ToString()
                + "\r\n蔬菜储备:" + vege_store.ToString() + "速食储备:" + fastfood_store.ToString() + "主食储备" + basefood_store.ToString() 
                +"药物:"+medicine.ToString()+ "\r\n职业:" + job.ToString() +
                "严重程度:" + difficulty.ToString() + "\r\n传播速率:" + spread.ToString() + "社会风气:" + ideology.ToString() + "物资供应:" + supply.ToString()
                + "\r\n饥饿:" + hungry.ToString() + "劳累:" + tire.ToString()
                + "心情:" + mental.ToString() + "健康:" + health.ToString()
            + "\r\n感染数:" + infected.ToString();
            nowState += "\r\n当前时间:" + now_day + "日" + now_time[0] + ":" + now_time[1];
            textBox2.AppendText(nowState) ;
            nowState = "\r\n本次总计:" + time.ToString() + "天";
            textBox2.AppendText(nowState);*/
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
            rna.infect_check = false;
            worked_time = 0;    //已工作次数
            one_day_work = 0;   //单日已工作次数

            label25.Text = ""; //提示框要清空
            enable_buttons(); //6个按钮要恢复
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)  //吃饭提示
        {
            label25.Text = "消耗半小时做饭，半小时吃饭，回复饥饿、健康和精神。" +
                "\r\n优先消耗主食和食材做饭\r\n无法做饭时会使用方便食品";
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
            label25.Text = "每次消耗一小时玩电脑，回复心情，消耗精力";
        }

        private void button4_MouseMove(object sender, MouseEventArgs e)  //工作提醒
        {
            if(job==4)
                label25.Text = "消耗四小时外出送货，只能在白天进行\r\n每天最多两次" +
                    "\r\n警告:有概率感染\r\n需要有有效核酸证明和外出证才能外出";
            else
            label25.Text = "消耗四小时居家工作，只能在白天进行\r\n每天最多两次";
        }

        private void button9_MouseMove(object sender, MouseEventArgs e) //外出提醒
        {
            label25.Text = "外出闲逛，需要有出门证和48小时内核酸报告" +
                "\r\n花费三小时在附近逛逛，或许看到什么";
        }

        /*private void change_nowMoney(int target) //修改当前钱数
        {
            money = target;
            label6.Text = money.ToString();
        }*/

        private void changeTime(int need_time,string thing)  //时间推进函数 
        {
            //只允许这个函数推进时间
            //原有函数只会机械执行时间推进，一旦遇到早上核酸事件就会出问题
            //现在函数接收时间和事件申请，不忙的话就会一直执行到完
            //如果有别的事件依赖时间，在底下while中继续加if进入事件函数就行
            //记得要在事件函数中设置busy，结束后还要在事件处理函数deal_event()中取消busy

            deltaTime = need_time;   
            main_thing = thing;//标记玩家处于什么状态        
                       
            while (deltaTime > 0)  //以半小时为单位时间推进
            {
                if (busy) break;
                else
                {
                    deltaTime -= 30;
                    if (now_time[1] == "00")
                    {
                        now_time[1] = "30";
                        autoDecrease();
                    }
                    else
                    {
                        now_time[1] = "00";
                        now_time[0] = (Convert.ToInt32(now_time[0]) + 1).ToString();
                        autoDecrease();
                    }
                    if (Convert.ToInt32(now_time[0]) > 24) 
                    {
                        now_day++;
                        now_time[0] = (Convert.ToInt32(now_time[0]) - 24).ToString();
                        mental -= 8;//每过一天心情-8
                        cacu_infect();//计算今日感染情况
                        covid_news(); //播报今日疫情
                    }//过了一天
                    
                    label26.Text = "当前时间:第" + now_day.ToString() + "天" + now_time[0].ToString()
                        + ":" + now_time[1].ToString();

                    //以上是必有的时间推进功能
                    //以下是各种已经规定好时间的定时事件
                    checkState();//更新一下状态

                    if(Convert.ToInt32(now_time[0])%2==0 &&now_time[1]=="00")
                    {
                        time_randomEvent();
                    } //两小时触发一次时间推进的随机事件

                    if(now_day%3==0 && now_time[0]=="8")
                    {
                        Random ra = new Random();
                        double raf = ra.NextDouble();
                        if(raf>0.9)
                        {
                            string str = "\r\n援助物资抵达了，小区向住户发放了物资";
                            Form5 f5 = new Form5(str);
                            textBox2.AppendText(str) ;
                            vege_store += ra.Next(2, 4);
                            meat_store += ra.Next(0, 1);
                            basefood_store += 5;
                            fastfood_store += ra.Next(0, 4);
                        }
                    } //隔三天 早8点有10%概率送菜

                    if (now_day % 7 == 0)
                    {
                        give_money();
                    }//结算工资

                    if (now_time[0] == "9" && now_time[1] == "00")
                    {
                        checkDna();
                    }//9点核酸检测

                    if(get_food && r1.hour.ToString()==now_time[0] && now_time[1]=="00")
                    {
                        get_food = false;
                        basefood_store += r1.basefood_num;
                        meat_store += r1.meat_num;
                        vege_store += r1.vege_num;
                        fastfood_store += r1.fastfood_num;
                        medicine += r1.medicine_num;

                        r1.basefood_num = 0;
                        r1.meat_num = 0;
                        r1.vege_num = 0;
                        r1.fastfood_num = 0;
                        r1.medicine_num = 0;

                        textBox2.AppendText("\r\n你订购的货物已经送达了！");
                    }//收快递

                    if (Convert.ToInt32(now_time[0]) == 14 && rna.tested)
                    {
                        if (rna.infect = false)
                        {
                            textBox2.AppendText("\r\n核酸报告已经拿到了，结果是阴性");
                            rna.infect_check = false; //检查结果也是阴性
                            rna.report = true; //拿到报告
                            rna.tested = false; //等待下次捅喉咙
                            rna.day = now_day + 2;
                            rna.hour = Convert.ToInt32(now_time[0]); //有效期截止两天后的现在
                        }
                        else
                        {
                            rna.infect_check = true; //确诊阳性了
                        }
                    }//核酸14点拿结果  
                }
            } 
            //以下是不能在时间推进中反复调用的函数

            if (deltaTime == 0)
            {
                end_thing_tip(); 
                main_thing = "";            
            }//任务结束后弹提示、清痕迹

            if (time == now_day)
            {
                finish();
            }//时间到了就跳结局A-顺利度过

            if (now_day % 4 == 0) showBGM();
        }

        private void autoDecrease() //自动扣饥饿和睡眠  条件扣/回复心情和健康 
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
            if(hungry>=75 && tire>=70)
            {
                mental += 2;
                health += 1;
            } //饱腹时加心情健康
            if(hungry<=15 ||tire<=10)
            {
                mental -= 1;
                health -= 1;
            }
            if (rna.infect) health -= 2; //一旦感染就开始掉健康
        }

        private void checkState()  //更新玩家五属性 发生动作时就应调用 含结局B D
        {
            if (hungry > 75) label4.Text = "饱足";
            if (hungry <= 75 && hungry > 60) label4.Text = "一般";
            if (hungry <= 60 && hungry > 40) label4.Text = "饥饿";
            if (hungry <= 40 && hungry > 20) label4.Text = "十分饥饿";
            if (hungry <= 20) label4.Text = "营养不足";

            if (tire > 75) label21.Text = "精神充沛";
            if (tire <= 75 && tire > 55) label21.Text = "一般";
            if (tire <= 55 && tire > 35) label21.Text = "稍有疲倦";
            if (tire <= 35 && tire > 20) label21.Text = "十分疲倦";
            if (tire <= 20) label21.Text = "缺乏睡眠";

            if (mental > 80) label3.Text = "愉悦";
            if (mental <= 80 && tire > 45) label3.Text = "一般";
            if (mental <= 45 && tire > 23) label3.Text = "心情沉重";
            if (mental <= 23 && tire > 0) label3.Text = "接近崩溃";
            if (mental < 0) label3.Text = "崩溃边缘";

            if (health > 80) label28.Text = "十分健康";
            if (health <= 80 && tire > 55) label28.Text = "健康";
            if (health <= 55 && tire > 25) label28.Text = "身体不适";
            if (health <= 25) label28.Text = "需要治疗";

            label6.Text = money.ToString();
            if(money<1000)
            {
                textBox2.AppendText("\r\n你的存款不多了，最好想点办法");
            }   //欠款提醒

            if (health <= -10) finishB(); //健康为-10，结局B

            if (mental <= -20) finishD(); //精神崩溃，结局D
        }

        private void cacu_infect() //每日0点更新感染者人数
        {
            //不设置死亡人数
            if (infected == 0) infected = 1;
            else                
            {
                Random ra = new Random();
                infected =(int) ( infected * (1 + spread * 12 + ideology * 0.8+difficulty*0.4+ra.NextDouble()*10 )
                    +non_symptom*(1+spread*17+ideology*6+ra.NextDouble()*10 ) );
                non_symptom = (int)((infected * (1 + spread * 3 + ra.NextDouble()*10))
                    + (non_symptom * (2 + spread * 20 + ideology * 12+ ra.NextDouble()*10)));
            }
            //这个函数内需要用到社会风气 严重程度 传播速度
        }

        private void covid_news() //每日疫情信息播报
        {
            string str = "今天是封城第" + now_day.ToString() + "天\r\n今日疫情播报:\r\n 感染人数:"
                + infected.ToString() + "人 无症状者" + non_symptom.ToString()+"人";
            Form5 f5 = new Form5(str);
            textBox2.AppendText(str);
            f5.Show();
        }
        //疫情信息考虑挪到手机窗口去 

        private void give_money()  //7天发一次薪水
        {
            //无业人员 没薪水  行政人员干多少都一样  厨师和工人只有底薪
            //快递要想想怎么处理
            textBox2.AppendText("\r\n发放工资");
            if (job == 0) money += 1000 * worked_time;
            else if (job == 1) money += 500 * worked_time; //程序员
            else if (job == 2 || job == 3) money += 1000; //厨师 工人
            else if (job == 5) money += 5000;//行政人员
            else if (job == 4) money += 1500 * worked_time; //快递员
            else money += 0;

            if(job==6)
            {
                textBox2.AppendText("\r\n你的父母担心生活费不够，给你打了2000元");
                money += 2000;
            }

            worked_time = 0; //7天内工作次数和单日次数清零
            one_day_work = 0;
        }

        private void rent_house()//7天收一次房租
        {
            if (job == 0||job==6) ; //高管有房 学生住校
            else if (job >= 1 && job <= 5)
            {
                textBox2.AppendText("还房贷时间已到，银行已经自动从你的卡中划走了本次还款4500元");
                money -= 4500;
                checkState();
            }
            else 
            {
                textBox2.AppendText("还房贷时间已到，银行已经自动从你的卡中划走了钱");
                if (money < 2000)
                {
                    finishC();
                } //结局C，没钱租房 流浪
                else
                {
                    money -= 3000;
                    checkState();
                }
            }//无业只能租房 到时没交就结局C
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
            //主食配肉/菜    方便食品可以不配 
            //优先做正餐
            //吃饭减劳累 加饥饿心情 但快餐扣心情
            //做饭吃一小时  方便食品半小时
            if (hungry >= 100) textBox2.AppendText("\r\n你已经很饱了！");
            else
            {
                if (basefood_store > 0 && (meat_store > 0 || vege_store > 0))
                {
                    basefood_store--;
                    if (job == 2)
                    {
                        hungry += 90;
                        mental += 10;
                    } //厨师的额外加成
                    else { hungry += 75; }

                    mental += 10;
                    health += 8;
                    if (meat_store > 0)
                    {
                        meat_store--;   //决定用什么配菜
                        mental += 5; //吃肉的额外加成
                    }
                    else 
                    { vege_store--; }

                    changeTime(60,"food");
                    //textBox2.AppendText("\r\n你用一小时做了一顿饭");
                }//正餐
                else if (fastfood_store > 0)
                {
                    textBox2.AppendText("\r\n你用半小时吃了点方便食品");
                    fastfood_store--;
                    hungry += 45;
                    mental -= 5;
                    health += 5;
                    changeTime(30,"food");
                }//零食
                else
                {
                    textBox2.Text += "\r\n你的食物不足，无法做饭！";
                } //没食材
            }
            checkState();
        }

        private void button5_Click(object sender, EventArgs e) //睡觉按钮
        {
            if (tire >= 100) textBox2.AppendText("\r\n精神充沛，无需睡觉");
            else  //睡觉开始前后设置状态
            {
                textBox2.AppendText("\r\n你睡了7小时觉");
                changeTime(420,"sleep");
                tire += 80;
                mental += 30;

            }
            checkState();
        }

        private void button2_Click_1(object sender, EventArgs e) //刷手机按钮
        {
            textBox2.AppendText("\r\n你刷了半小时手机"); 
            changeTime(30,"phone");
            tire -= 5;           
            checkState();
            Random ra = new Random();
            if(ra.NextDouble()>0.9)
            {
                phone_randomEvent();  //概率触发事件
            }
        }

        private void button3_Click_1(object sender, EventArgs e) //玩电脑按钮
        {
            changeTime(60,"computer");
            tire -= 10;
            mental += 20;
            textBox2.AppendText("\r\n你刷了一小时电脑");
            checkState();
        }

        private void button4_Click_1(object sender, EventArgs e)  //工作按钮
        {
            //每日最多工作2次
            //每7天结一次钱
            if (job == 2 || job == 3)
            {
                Form5 f5 = new Form5("你的工作无法在家执行！");
                textBox2.AppendText("你的工作无法在家执行！");
                f5.Show();
            } //厨师 工人无法在家干活
            else if (job == 6)
            {
                Form5 f5 = new Form5("学生不需要工作");
                textBox2.AppendText("\r\n学生不需要工作");
                f5.Show();
            } //学生不工作
            else if(job==4)
            {
                //能外出的条件:未被查出阳性 有外出证 
                if(!rna.infect_check && license)
                {   //核酸在有效期内
                    if(rna.day<=now_day || (rna.day==now_day&&rna.hour<=Convert.ToInt32(now_time[0])) )
                    {
                        worktime = is_work_time();
                        Random ra = new Random();
                        if(worktime==false)
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
                                changeTime(240, "work");
                                mental -= 15;
                                one_day_work++;
                                worked_time++;
                                if(ra.NextDouble()<(0.6-spread))
                                {
                                    rna.infect = true;
                                }//有概率得病
                                checkState();
                            }
                        }
                    }
                }
                else
                {
                    textBox2.AppendText("\r\n你不具备外出条件!");
                }
            } 
            //快递员外出工作
            else
            {
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
                        changeTime(240, "work");
                        mental -= 15;
                        one_day_work++;
                        worked_time++;
                        checkState();
                    }
                }
            } //其他职业

            textBox2.AppendText("\r\n最近7天内累计工作:"+worked_time.ToString()+"次");
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
            else if(rna.infect==true)
            {
                textBox2.AppendText("\r\n你确诊了新冠阳性，不能出门");
            }
            else 
            {
                changeTime(180,"hang");
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


        private void finish()  //结局A  活到最后
        {
            textBox2.Text += "\r\n游戏结束了，你坚持到了最后";
            chuanzhi = textBox2.Text.ToString();
            Form3 f3 = new Form3(chuanzhi);
            f3.Controls["button2"].Visible = false;
            f3.ShowDialog();
            unable_buttons(); //禁用按钮，玩家无法继续玩下去了
        }

        private void finishB()//结局B  健康为-10
        {
            textBox2.Text += "你的身体素质没能让你扛过病症";
            chuanzhi = textBox2.Text.ToString();
            Form3 f3 = new Form3(chuanzhi);
            f3.ShowDialog();
            unable_buttons(); //禁用按钮，玩家无法继续玩下去了
        }

        private void finishC()// 结局C 没钱了
        {
            textBox2.Text += "你的存款不足以支持你继续住下去\r\n你消失在了街上...";
            chuanzhi = textBox2.Text.ToString();
            Form3 f3 = new Form3(chuanzhi);
            f3.ShowDialog();
            unable_buttons(); //禁用按钮，玩家无法继续玩下去了
        }

        private void finishD()//结局D 心情掉到-20，崩溃了
        {
            textBox2.AppendText("你的意志力显然不够坚强，打开门一路冲了出去...");
            chuanzhi = textBox2.Text.ToString();
            Form3 f3 = new Form3(chuanzhi);
            f3.Show();
            unable_buttons();
        }

        private void button7_Click(object sender, EventArgs e) //显示储备按钮
        {
            string nowState = "";
            nowState = "\r\n肉类储备:" + meat_store.ToString()
                + "\r\n蔬菜储备:" + vege_store.ToString()
                + "\r\n速食储备:" + fastfood_store.ToString()
                + "\r\n主食储备:" + basefood_store.ToString()
                + "\r\n药物:" + medicine.ToString();
            textBox2.AppendText(nowState);
        }

        private void time_randomEvent()  //时间推进的随机事件触发 两小时触发一次
        {            
            Random ra = new Random();
            int num = 0;
            worktime = is_work_time();
            if (worktime &&main_thing!="sleep")  //晚上就不触发事件了
            {
                //textBox2.AppendText("\r\n触发了一次随机事件");
                num = ra.Next(1, 9);
                switch (num)
                {
                    case 1: 
                        {
                            if (ra.NextDouble() > 0.6)
                            {
                                Form5 f5 = new Form5("\r\n父母给你打了2000元" +
                                    "\r\n感谢他们吧");
                                f5.Show();
                                money += 2000;
                                mental += 3;
                                checkState();                                
                            }
                            break;
                        }  //家里给钱
                    case 2:
                        {
                            if (ra.NextDouble() > 0.8)
                            {
                                Form5 f5 = new Form5("\r\n额外的蔬菜物资送到了");
                                f5.Show();
                                vege_store += ra.Next(2,4);
                                mental += 3;
                                checkState();
                            }
                            break;
                        }//额外蔬菜物资
                    case 3:
                        {
                            if (ra.NextDouble() > 0.92)
                            {
                                Form5 f5 = new Form5("\r\n额外的肉类物资送到了");
                                f5.Show();
                                meat_store += ra.Next(1, 3);
                                mental += 3;
                                checkState();
                            }
                            break;
                        }//额外肉类物资
                    case 4:
                        {
                            if (ra.NextDouble() > 0.95)
                            {
                                Form5 f5 = new Form5("\r\n家里的储备发霉了一点，看样子不能吃了");
                                f5.Show();
                                meat_store -= ra.Next(0, 1);
                                vege_store -= ra.Next(0, 1);
                                basefood_store -= ra.Next(0, 2);
                                mental -= 3;
                                checkState();
                            }
                            break;
                        }//储存发霉
                    case 5:
                        {
                            if (ra.NextDouble() > 0.9)
                            {
                                //肉 菜 主 速   如有需要可以继续拓展
                                exchange[0][0] = ra.Next(0,2);
                                exchange[0][1] = ra.Next(1, 2);
                                exchange[0][2] = ra.Next(1, 3);
                                exchange[0][3] = ra.Next(0, 2);

                                exchange[1][0] = ra.Next(0, 1);
                                exchange[1][1] = ra.Next(0, 3);
                                exchange[1][2] = ra.Next(0, 3);
                                exchange[1][3] = ra.Next(0, 2);

                                string sss = exchange[0][0] + "份肉 " + exchange[0][1] + "份菜 "
                                    + exchange[0][2] + "份主食 和" + exchange[0][3] + "份速食 ";
                                string sss1 = exchange[1][0] + "份肉 " + exchange[1][1] + "份菜 "
                                    + exchange[1][2] + "份主食 和" + exchange[1][3] + "份速食 ";
                                Form5 f5 = new Form5("\r\n邻居隔着门，想和你交换一些物资" +
                                    "\r\n他想用:"+sss+"和你交换:"+sss1+"你想要交换吗？");
                                f5.Owner = this;
                                f5.Show();
                                if (f5.res)
                                {
                                    meat_store = meat_store + exchange[0][0] - exchange[1][0];
                                    vege_store = vege_store + exchange[0][1] - exchange[1][1];
                                    basefood_store = basefood_store + exchange[0][2] - exchange[1][2];
                                    fastfood_store = fastfood_store + exchange[0][3] - exchange[1][3];
                                    mental += 2; ;
                                    textBox2.AppendText("\r\n你和邻居进行了物资交换");
                                } //物资交换
                                else textBox2.AppendText("你拒绝了交换");                              
                                checkState();
                            }
                            break;
                        }//邻居想换物资
                    case 6: { textBox2.AppendText("\r\n没想出来的事件6"); break; }
                    case 7: { textBox2.AppendText("\r\n没想出来的事件7"); break; }
                    case 8: { textBox2.AppendText("\r\n没想出来的事件8"); break; }
                    case 9: { textBox2.AppendText("\r\n没想出来的事件9"); break; }
                    default: { textBox2.AppendText("\r\n这个小时无事发生"); break; }
                }
            }

        }

        private void phone_randomEvent() //刷手机触发的随机事件
        {
            Random ra = new Random();
            switch(ra.Next(0,1))
            {
                case 0:
                    {
                        textBox2.AppendText("\r\n你刷到了一些搞笑meme图，注意力被转移开了");
                        mental += 10;
                        break;
                    } //娱乐
                case 1:
                    {
                        if (ra.NextDouble() > 0.98)
                        {
                            textBox2.AppendText("\r\n你看到传言军队前来支援的消息");
                            supply += (float)0.15;
                        }
                        break;
                    } //军队支援
            }
        }

        private void checkDna() //核酸检测事件上 挂在时间推进里
        {
            if (main_thing=="sleep")
            {
                textBox2.AppendText("\r\n你错过了早上9点的核酸检测" +
                    "\r\n大白敲门你没听见");
            }//睡觉跳过
            else if(main_thing=="hang")
            {
                textBox2.AppendText("\r\n你出门闲逛去了，没有做今天的核酸");
            }//外出跳过
            else if(job==4 && main_thing=="work")
            {
                textBox2.AppendText("\r\n你出去送快递去了，没有做今天的核酸");
            }//在外面送快递
            else
            {
                busy = true; //阻止时间推进 只有事件处理完才能继续
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
            //textBox2.AppendText("\r\n调用了一次是按钮");
            deal_event(); //转到事件处理函数
            enable_buttons(); //恢复6个按钮使用
            button10.Visible = false;
            button11.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)//"否"按钮
        {
            event_choose = false;
            //textBox2.AppendText("\r\n调用了一次否按钮");
            deal_event();
            enable_buttons();
            button10.Visible = false;
            button11.Visible = false;
        }

        private void deal_event()  //所有f1点选事件的后半段
        {
            switch(which_event)
            {
                case 1:  //核酸检测
                    {
                        which_event = 0;
                        if (event_choose)
                        {
                            rna.tested = true;                           
                            textBox2.AppendText("\r\n你消耗半小时做了核酸检测，五小时后出报告");
                            busy = false;
                            changeTime(deltaTime+30,main_thing); //继续之前的任务
                        }//做了核酸
                        else
                        {
                            rna.tested = false;
                            busy = false;
                            textBox2.AppendText("\r\n你没有参加核酸检测，希望没人注意到");
                            changeTime(deltaTime + 30, main_thing); //继续之前的任务
                        }//没做核酸
                        break;
                    }
            }
        }

        private void end_thing_tip()
        {
            switch (main_thing)
            {
                case "work":
                    {
                        textBox2.AppendText("\r\n你工作了四小时");
                        break;
                    }
                case "food":
                    {
                        textBox2.AppendText("\r\n你用一小时自己做了饭");
                        break;
                    }
            }
        } // 工作、外出等任务，由于会被打断，所以提示文本在这里


        private void button12_Click(object sender, EventArgs e) //手机买菜按钮
        {
            //手机买菜牵扯到两个窗口间的数值同步
            //跳转入其他窗口
            //在这里生成好新闻发给窗口4
            Random ra = new Random();
            if (linshi[3] == now_day) {; }//同一天的新闻不要变
            else
            {
                linshi[0] = ra.Next(0, 11);
                linshi[1] = ra.Next(0, 11);
                while (linshi[1] == linshi[0])
                {
                    linshi[1] = ra.Next(0, 11);
                } //循环是为了防止重复
                linshi[2] = ra.Next(0, 11);
                while (linshi[2] == linshi[0] || linshi[2] == linshi[1])
                {
                    linshi[2] = ra.Next(0, 11);
                }
                linshi[3] = Convert.ToInt32(now_day); //新闻数组最后一位存的其实是天数
            } //天数变了就换新闻

            Form4 f4 = new Form4(money,now_day,now_time[0],difficulty,spread,ideology,supply,linshi);
            f4.Owner = this;
            f4.ShowDialog();
            money = f4.money_num;//该语句影响数值回传 必须有
            get_food = f4.food; //判断是否买到了菜
            if (get_food)
            {
                textBox2.AppendText("\r\n你成功抢到了菜!" +
                    "\r\n大约4小时后菜便可运抵");
                r1.meat_num = f4.r1.meat_num;
                r1.vege_num = f4.r1.vege_num;
                r1.basefood_num = f4.r1.basefood_num;
                r1.fastfood_num = f4.r1.fastfood_num;
                r1.medicine_num = f4.r1.medicine_num;
                r1.hour = f4.r1.hour+Convert.ToInt32(now_time[0]);
                //f1窗口的r1.hour记录的是送达时间，和f4里面记录送货用时不同
            } //抢到了东西
            else textBox2.AppendText("\r\n这次抢购你没能抢到菜");
            checkState();
        }

        private void button13_Click(object sender, EventArgs e) //测试按钮 之后无用
        {
            display_now_store();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e) //帮助按钮
        {
            Form7 f7 = new Form7();
            f7.Show();
        }

        private void button14_Click(object sender, EventArgs e) //+1天按钮  仅用于测试
        {
            now_day++;
            checkState();
        }
    }
}


//display_()函数可以打印信息，记得需要时再打开

/*游戏里的随机事件包含时间推进函数里的固定点触发  每两小时随机触发 手机触发三种
 *   如果需要获取用户选择 有两种写法：1.核酸检测的方法，使用f1上的按钮，函数需要拆成两部分
 *   2.用form5窗口 之后取结果  缺点是时间未停止
 * */
