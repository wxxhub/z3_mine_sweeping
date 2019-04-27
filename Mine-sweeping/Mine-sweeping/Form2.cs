/*//////////////////////////GPL开源许可证////////////////////////////////////////////////
    Copyright (C) <2014>  <Xianglong He>
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
	作者：何相龙
	邮箱：qwgg9654@gmail.com
		  568629794@qq.com
	2014年11月9日
*/
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form2 : Form
    {        
        public Form2()
        {
            InitializeComponent();
        }

        

        private void button2_Click(object sender, EventArgs e)//取消
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)//初级
        {
            textBox1.Text = 10.ToString();
            textBox2.Text = 10.ToString();
            textBox3.Text = 10.ToString();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//中级
        {
            textBox1.Text = 16.ToString();
            textBox2.Text = 16.ToString();
            textBox3.Text = 40.ToString();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)//高级
        {
            textBox1.Text = 30.ToString();
            textBox2.Text = 16.ToString();
            textBox3.Text = 99.ToString();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//自定义
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            char[] a;
            int t1=0,t2=0,t3=0;
            a = new char[4];
            a = textBox1.Text.ToCharArray();
            if (a[0] != '\0') t1 = a[0] - '0';
            if (a[1] != '\0') t1 = t1 * 10 + a[1] - '0';
            //if (a[2] != '\0') t1 = t1 * 10 + a[2] - '0';

            a = new char[4];
            a = textBox2.Text.ToCharArray();
            if (a[0] != '\0') t2 = a[0] - '0';
            if (a[1] != '\0') t2 = t2 * 10 + a[1] - '0';
            //if (a[2] != '\0') t2 = t2 * 10 + a[2] - '0';

            a = new char[4];
            a = textBox3.Text.ToCharArray();
            if (a[0] != '\0') t3 = a[0] - '0';
            if (a[1] != '\0') t3 = t3 * 10 + a[1] - '0';
            //if (a[2] != '\0') t3 = t3 * 10 + a[2] - '0';

            conf.setconf(t1, t2, t3);
            this.Close();
        }
    }
}
