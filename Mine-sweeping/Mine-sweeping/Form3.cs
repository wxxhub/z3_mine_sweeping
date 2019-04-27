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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            FileStream f;
            int high, midd, low;//高级中级初级
            string higher, midder, lower;
            if (File.Exists("c://program files/sl.txt") == false)
            {
                high = 999;
                higher = "匿名";
                midd = 999;
                midder = "匿名";
                low = 999;
                lower = "匿名";
            }
            else
            {
                f = new FileStream("c://program files/sl.txt", FileMode.Open);//读取之前的记录
                StreamReader m = new StreamReader(f);
                string s1 = "";
                s1 += m.ReadLine();
                higher = m.ReadLine();
                midder = m.ReadLine();
                lower = m.ReadLine();
                m.Close();
                f.Close();
                high = (s1[0] - '0') * 100 + (s1[1] - '0') * 10 + (s1[2] - '0');
                midd = (s1[3] - '0') * 100 + (s1[4] - '0') * 10 + (s1[5] - '0');
                low = (s1[6] - '0') * 100 + (s1[7] - '0') * 10 + (s1[8] - '0');
            }                       
            label1.Text = "初级：" + low.ToString()  + "秒  By  " + lower;
            label2.Text = "中级：" + midd.ToString() + "秒  By  " + midder;
            label3.Text = "高级：" + high.ToString() + "秒  By  " + higher;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定重置排行榜？", "确定？", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                File.Delete("c://program files/sl.txt");
                this.Close();
            }          
        }
    }
}
