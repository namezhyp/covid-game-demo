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
    public partial class Form5 : Form
    {
        public string str;
        public bool res; //用于处理可能的返回值
        public Form5(string str)
        {
            InitializeComponent();
            label1.Text = str;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            res = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            res = false;
            this.Close();
        }
    }
}
