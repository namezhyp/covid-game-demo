using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        //public string money_str = "";
        public int money_num = 0;
        private int today = 1;
        private string hour = "";

        private int sum = 0;
        int num_ran = 0, num_meat = 0, num_vege = 0,num_base=0,num_medi=0,num_fast=0,num_buy=0; //记录对应按钮次数
        //int num1=0,num2=0,num3=0,num4=0,num5=0,num6=0;
        public bool food = false;

        private int meat_price=0;
        private int vege_price = 0;
        private int basefood_price = 0;
        private int fastfood_price = 0;
        private int random_price = 0;
        private int medicine_price = 100;

        private float difficulty = 0;  //严重程度
        private float spread = 0;  //传播效率
        private float ideology = 0;  //社会风气
        private float supply = 0;  //物资供应

        private int infect;//感染人数 用于限制价格
        private int non_sym;//无症状人数 用于限制价格
        private int news_num = 0;//新闻的系数 用于影响价格
        int[] news_set;//设置今天是什么新闻

        public struct res //结构体储存要传给主窗口的值
        {
            public int meat_num;
            public int vege_num;
            public int basefood_num;
            public int fastfood_num;
            public int medicine_num;
            public int hour;//快递运达要多久
        };
        public res r1 = new res();
        Random ra = new Random();

        public Form4(int money,int day1,string time1, float a, float b, float c, float d,int[] l)
        {
            //a b c d分别对应严重程度 社会风气等内容
            //x y暂时没用  l是新闻序号
            InitializeComponent(); //这句必须在前
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            pictureBox1.Image = Resource1.饿死了吗;
            pictureBox2.Image = Resource1.京北;
            sum = 0;
            money_num = money;
            difficulty = a;
            spread = b;
            ideology = c;
            supply = d;
            r1.meat_num = 0;r1.vege_num = 0;  //累计买到数目初始化
            r1.basefood_num = 0;r1.fastfood_num = 0;r1.medicine_num = 0;
            news_set = l;

            
            label2.Text = "剩余金钱:" +money_num.ToString();
            today = day1;
            hour = time1;
            label1.Text = "今日新闻   " + "第" + today + "日"+hour+"时";
            show_news();
            if(hour=="10" || hour=="21")
            {
                enable_button();
            }
        }

        private void button2_Click(object sender, EventArgs e) //买外卖按钮
        {
            //点外卖按钮限制最多按12次 能买到多少算自己运气
            if (num_fast < 12)
            {
                if (ra.NextDouble() > 0.7)
                {
                    sum += fastfood_price;
                    label8.Text = "总价:" + sum.ToString();
                    num_fast++;
                    label10.Text = "买到了一份外卖";
                    r1.fastfood_num++;
                }
                else { label10.Text = "手慢了，什么也没抢到"; num_fast++; }
            }
            else
            {
                label10.Text = "能点的几家外卖都下不了单了，看看别的吧";
                button2.Enabled = false;
            }
            
        }

        private void button3_Click(object sender, EventArgs e) //抢菜按钮
        {
            //买菜按钮限制最多按10次 
            if (num_vege < 10)
            {
                if (ra.NextDouble() > 0.5)
                {
                    sum += vege_price;
                    label8.Text = "总价:" + sum.ToString();
                    num_vege++;
                    label10.Text = "买到了一份菜";
                    r1.vege_num++;
                }
                else { label10.Text = "手慢了，什么也没抢到"; num_vege++; }
            }
            else
            {
                label10.Text = "商城放出的库存没了，看看别的吧";
                button3.Enabled = false;
            }
            
        }

        private void button4_Click(object sender, EventArgs e) //抢肉按钮
        {
            //买肉按钮限制最多按8次 
            if (num_meat < 8)
            {
                if (ra.NextDouble() > 0.4)
                {
                    sum += meat_price;
                    label8.Text = "总价:" + sum.ToString();
                    num_meat++;
                    label10.Text = "买到了一份肉";
                    r1.meat_num++;
                }
                else { label10.Text = "手慢了，什么也没抢到"; num_meat++; }
            }
            else
            {
                label10.Text = "商城放出的库存没了，看看别的吧";
                button4.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e) //买主食按钮
        {
            //主食按钮限制最多按15次 
            if (num_base < 8)
            {
                if (ra.NextDouble() > 0.4)
                {
                    sum += basefood_price;
                    label8.Text = "总价:" + sum.ToString();
                    num_base++;
                    label10.Text = "买到了一份主食";
                    r1.basefood_num++;
                }
                else { label10.Text = "手慢了，什么也没抢到"; num_base++; }
            }
            else
            {
                label10.Text = "商城放出的库存没了，看看别的吧";
                button6.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e) //买药按钮
        {          //买药按钮不做次数限制                     
            if (ra.NextDouble() > 0.5)
            {                                     
                 sum += medicine_price;                   
                 label8.Text = "总价:" + sum.ToString();
                label10.Text = "买到了一份药";
                r1.medicine_num++;
            }
            else { label10.Text = "手慢了，没有买到，再试试吧";  }
            
        }

        private void button1_Click(object sender, EventArgs e)  //随机购买按钮
        {          //随机购买按钮限制最多按8次 能买到多少算自己运气
            if (num_ran < 8)
            {
                if (ra.NextDouble() > 0.6)
                {
                    if(today<4) random_price = ra.Next(15, 70); //前4天保护期
                    else random_price = ra.Next(50, 400);
                    sum += random_price;
                    switch(ra.Next(0,4))
                    {
                        case 0: 
                            {
                                r1.meat_num++;
                                label10.Text = "随机按钮购买到了肉";
                                break; 
                            }
                        case 1:
                            {
                                r1.vege_num++;
                                label10.Text = "随机按钮购买到了菜";
                                break;
                            }
                        case 2:
                            {
                                r1.basefood_num++;
                                label10.Text = "随机按钮购买到了主食";
                                break;
                            }
                        case 3:
                            {
                                r1.fastfood_num++;
                                label10.Text = "随机按钮购买到了快速食品";
                                break;
                            }
                        case 4:
                            {
                                r1.medicine_num++;
                                label10.Text = "随机按钮购买到了药";
                                break;
                            }
                    }
                    label8.Text = "总价:" + sum.ToString();
                    num_ran++;
                }
                else { label10.Text = "手慢了，什么也没抢到"; num_ran++; }
            }
            else
            {
                label10.Text = "随机抢购已经被抢空了，看看别的吧";
                button1.Enabled = false;
            }
        }

        private void show_news()
        {
            string[] news = new string[] {"某网络商城大幅裁员，老板声称其员工已毕业"
                ,"来自其他地区的物资支援已经抵达"
            ,"某商城拿出百亿元补贴用户，声称要挤垮对手"
            ,"某某街道被列入高风险地区","外卖员大量离职"
            ,"当地外卖公司奖金鼓励外卖员勇敢支援疫情"
            , "当地仓库得到解禁","其他地区的医疗支援队已经抵达"
            ,"快递已经停运","快递维持正常运转","出现新的疫情爆发点"
            ,"全国各地的医疗专家前来增援"};
            double[] buff = { 0.2,-0.4,-0.2,0.1,0.4,-0.3,-0.2,-0.2
            ,0.6,-0.1,0.2,-0.2};//设置新闻的buff效果，注意正数会涨价 负数才是降价
            
            label3.Text = "";
            label3.Text+= news[news_set[0]]+"\r\n";
            label3.Text += news[news_set[1]] + "\r\n";
            label3.Text += news[news_set[2]] + "\r\n";
            news_num = (int)(buff[news_set[0]] + buff[news_set[1]] + buff[news_set[2]]);

            set_price();//设置商品价格
        } //每日新闻 以及新闻的buff计算

        private void enable_button()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
        }

        private void set_price()//设置价格，这个函数被show_news调用
        {
            //主食和药品价格应该被限制在一个合理范围
            //其他值要考虑能随着天数改变
            //暂时不考虑随着感染人数增加大幅上涨了，参数太多
            //外卖和主食前几天可以降低难度购买
            if (today <= 4)
            {
                meat_price = (int)(40 * (1 + supply * 0.3 + news_num ));
                vege_price = (int)(10 * (1 + supply * 0.2 + news_num));
                fastfood_price = (int)(25 * (1 + supply * 0.5 + news_num * 0.8  + spread * 0.6));
                basefood_price = (int)(15 * (1 + supply * 0.6  + news_num*0.4));
                medicine_price = (int)(80 * (1 + difficulty * 0.5 ));
            } //前4天保护期
            else
            {
                meat_price = (int)(40 * (1 + supply * 0.8 + news_num + ra.Next(0,today) / 8));
                vege_price = (int)(10 * (1 + supply * 0.7 + news_num + ra.Next(0, today) / 8));
                fastfood_price = (int)(25 * (1 + supply * 0.5 + news_num * 1.5 + difficulty + spread * 0.6 + ra.Next(0, today) / 8));
                basefood_price = (int)(15 * (1 + supply * 0.6 + spread * 0.5 + news_num + ra.Next(0, today) / 10));
                medicine_price = (int)(80 * (1 + difficulty * 0.5 + supply * 0.4 + news_num + ra.NextDouble() / 6));
            }

            button1.Text = "代购 价格随机";
            button2.Text = "买外卖 " + fastfood_price.ToString() + "元";
            button3.Text = "抢菜 " + vege_price.ToString() + "元";
            button4.Text = "抢肉 " + meat_price.ToString() + "元";
            button6.Text = "购买主食 " + basefood_price.ToString() + "元";
            button7.Text = "买药 " + medicine_price.ToString() + "元";
        }

        private void button5_Click(object sender, EventArgs e)  //确认下单
        {
            if (num_buy <= 15) //最多允许按16次按钮
            {
                Random ra = new Random();
                if (ra.NextDouble() > 0.98)
                {
                    food = true;
                    money_num -= sum;
                    label2.Text = "剩余金钱:" + money_num.ToString();
                    if (r1.fastfood_num == 0) r1.hour = 5;
                    else r1.hour = 1;       //设置送货时间

                    this.Close();
                    Form5 f5 = new Form5("你成功抢到了菜!");
                    f5.Show();
                }
                else 
                {
                    num_buy++;
                    //label10.Text = num.ToString();
                }
            }
            else
            {
                this.Close();
                Form5 f5 = new Form5("手速太慢了\r\n别人已经把库存抢光了\r\n下次再来吧");
                f5.Show();
            }
            
        }
    }
}


//https://zhuanlan.zhihu.com/p/36587587 窗口间传数值
