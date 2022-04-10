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
        public int money = 0;
        public Form4(int money_num)
        {
            InitializeComponent();
            label2.Text = "剩余金钱:" + money_num.ToString();
            show_news();
        }

        private void button1_Click(object sender, EventArgs e)  //购买按钮
        {

        }

        private void show_news()
        {
            Random ra = new Random();
            string[] news = new string[] {"某网络商城大幅裁员，老板声称其员工已毕业","来自其他地区的物资支援已经抵达"
            ,"某商城拿出百亿元补贴用户，声称要挤垮对手","某某街道被列入高风险地区","外卖员大量离职"
            ,"当地外卖公司提高奖金以鼓励外卖员勇敢支援疫情"};
            label3.Text = "";
            label3.Text+= news[ra.Next(0, 5)]+"\r\n";
            label3.Text += news[ra.Next(0, 5)] + "\r\n";
            label3.Text += news[ra.Next(0, 5)] + "\r\n";
        }
    }
}
