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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        public int nx;//横向宽度
        public int ny;//纵向宽度
        public int nb;//雷数
        bool win = false;
        int[] boom;        
        int bl;     //剩下的雷
        int nl;     //剩下的砖
        int ti=0;   //时间
        bool dyc = true; //第一次游戏
        Timer timer1 = new Timer();
        Brick[,] Bricks;//砖块信息
        TextBox nlb;      //剩余雷数显示文本框
        TextBox tib;      //时间显示文本框
        Font newfront;//加粗的字体信息
        bool sb = false;//留名提交按钮按下
        Form input;//留名窗体
        partial void dig();
        partial void find();
        class Brick : Button //定义砖块类
        {
            public Brick(int in_x, int in_y)
            {
                x = in_x;
                y = in_y;
            }
            public int x;//坐标x
            public int y;//坐标y
            public bool boom=false;//是不是雷
            public bool counted = false;//有没有被挖过
        }
        public string InputBox(string Caption, string Hint, string Default) //输入对话框 (标签、标题、默认值)
        {
            //生成窗体
            input = new Form();
            input.Size = new Size(200, 150);
            input.Text = Caption;
            input.MaximizeBox = false;
            input.TopMost = true;
            input.ShowInTaskbar = false;
            input.ShowIcon = false;
            input.MinimizeBox = false;
            //生成文本标签
            Label label1 = new Label();
            label1.Text = Hint;
            label1.Location = new Point(input.Size.Width / 2 - label1.Size.Width  / 2-3, 10);
            input.Controls.Add(label1);
            //生成文本框
            TextBox textbox1 = new TextBox();
            textbox1.Text = Default;
            textbox1.Location = new Point(input.Size.Width / 2 - textbox1.Size.Width / 2-5, label1.Size.Height +15);
            input.Controls.Add(textbox1);
            //生成确定按钮
            Button submit = new Button();
            submit.Text = "确定";
            submit.Location = new Point(input.Size.Width / 2 - submit.Size.Width / 2-5, label1.Size.Height + textbox1.Size.Height + 30);
            submit.MouseUp += submitf;
            //设置窗体
            input.Controls.Add(submit);
            input.AcceptButton = submit;
            input.ShowDialog();


            while (!sb) ;
            return textbox1.Text;
        }

        private void submitf(object sender, MouseEventArgs e)   //按下留名的确定按钮
        {
            sb = true;
            input.Close();
        }

        void startNewgame()
        {
            win = false;
            if (dyc == true)
            {
                nx = 10;
                ny = 10;
                nb = 10;
            }
            else
            {
                nx = conf.nx;
                ny = conf.ny;
                nb = conf.nb;
            }
            if (nx == 0)
            {
                nx = 10;
                ny = 10;
                nb = 10;
            }
            int dx = 32;//砖块的x大小
            int dy = 32;//砖块的y大小
            int UpMargin = 64;//上边距
            int LeftMargin = 32;//左边距
            Bricks = new Brick[nx,ny];
            boom = new int[nb];
////////////////////////////生成雷////////////////////////////////
            for (int i = 0; i < nb; i++)
            {
                Random n = new Random();
                boom[i] = n.Next(nx*ny);
                for (int j = 0; j < i; j++)
                    if (boom[j] == boom[i]) 
                    {
                        i--;
                        break;
                    }
            }
////////////////////////////生成雷结束////////////////////////////

////////////////////////////生成砖块//////////////////////////////           

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Bricks[i,j] = new Brick(i,j);
                    Bricks[i,j].Size = new Size(dx, dy);
                    Bricks[i, j].Location = new Point(LeftMargin + i * dx, UpMargin + j * dy);
                    Bricks[i, j].MouseUp += newButton_MouseUp;
                    Bricks[i, j].BackColor = Color.Gray;
                    if (find(i * 10 + j) == true) Bricks[i, j].boom = true;
                    Controls.Add(Bricks[i, j]);
                }
////////////////////////////生成砖块结束////////////////////////// 
            this.Size = new Size(Bricks[nx - 1, ny - 1]. Location.X + dx+ LeftMargin , Bricks[nx - 1, ny - 1]. Location.Y  +dy+ UpMargin);
////////////////////////////重新开始按钮//////////////////////////
            Button a = new Button();
            a.Text = "New Game";
            a.MouseUp += newgame_Mouseup;
            a.Location = new Point(this.Size.Width/2- a.Size.Width/2, 32);
            Controls.Add(a);
////////////////////////重新开始按钮结束//////////////////////////

            bl = nb;    //初始化雷数
            nl = nx * ny; //初始化砖数
            ti = 0;     //初始化时间

////////////////////////////剩余雷数显示//////////////////////////
            nlb = new TextBox();
            nlb.Text = nb.ToString();
            nlb.Width = 30;
            nlb.Location = new Point(a.Location.X / 2 +a.Width/2 - nlb.Size.Width / 2, 32);            
            Controls.Add(nlb);
////////////////////////剩余雷数显示结束//////////////////////////

////////////////////////////时间显示//////////////////////////
            tib = new TextBox();
            tib.Text = 0.ToString();
            tib.Width = 30;
            tib.Location = new Point(a.Location.X *2 - a.Width / 2 - tib.Size.Width / 2, 32);            
            Controls.Add(tib);
            //计算器设置
            //timer1 = new Timer();
            timer1.Interval = 1000;
            if(dyc)timer1.Tick += timer1_do;//如果是第一次，添加时钟动作
            timer1.Stop();
////////////////////////时间显示结束//////////////////////////
            dyc = false;//你的第一次没有了
        }

        private void timer1_do(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            ti++;
            tib.Text = ti.ToString();
        }
            
        private void newgame_Mouseup(object sender, MouseEventArgs e) //重新开始
        {
            Controls.Clear();
            Controls.Add(menuStrip1);
            startNewgame();
        }

        private void newButton_MouseUp(object sender, MouseEventArgs e) //鼠标按键处理
        {
            if (win) return;
            Brick currbrick = (Brick)sender;
            if (e.Button == MouseButtons.Right) //右键
            {
                if(currbrick.BackColor == Color.White)return; //已经挖过，不做处理
                if (currbrick.Text == "") //标记过，取消标记，并增加剩余雷数
                {
                    bl++;
                    nlb.Text = bl.ToString();
                    currbrick.Text = " ";
                    currbrick.Font = new Font("GB2312", 9);                    
                    currbrick.BackColor = Color.Gray;
                }
                else                        //没有标记过，标记，并减少剩余雷数
                {
                    bl--;
                    nlb.Text = bl.ToString();
                    currbrick.Text = "";
                    currbrick.Font = new Font("wingdings", 14);
                    currbrick.BackColor = Color.Red;
                }
            }
            else if (e.Button == MouseButtons.Left) //左键
            {
                if (currbrick.Text == "") return;     //被标记过，不做处理
                if(nl==nx*ny)timer1.Start();               //第一次挖，开始计时
                while (nl == nx * ny && currbrick.boom == true)                  //人品太差，重新随机
                {
                    for (int i = 0; i < nb; i++)        //重新生成雷
                    {
                        Random n = new Random();
                        boom[i] = n.Next(nx * ny);
                        for (int j = 0; j < i; j++)
                            if (boom[j] == boom[i])
                            {
                                i--;
                                break;
                            }
                    }
                    for (int i = 0; i < nx; i++)        //布雷
                        for (int j = 0; j < ny; j++)
                            if (find(i * 10 + j) == true) Bricks[i, j].boom = true;
                            else Bricks[i, j].boom = false;

                }
                if (currbrick.boom == true)             //是雷，BOOM！！！
                {                            
                    currbrick.Text = "@";
                    findall();
                    currbrick.BackColor = Color.Red;
                    //随机提示语句
                    int i;
                    Random r = new Random();
                    i = r.Next(5);
                    switch (i)
                    {
                        case 0: MessageBox.Show("哈哈！！！你挂了！！！"); break;
                        case 1: MessageBox.Show("还是被炸死了吧？？哈哈"); break;
                        case 2: MessageBox.Show("恭喜你成功人肉排雷一个"); break;
                        case 3: MessageBox.Show("别挣扎了，你是过不了的"); break;
                        case 4: MessageBox.Show("卧槽！谁让你踩坏我雷的"); break;
                    }                          
                }
                else
                {
                    string box_data = "";
                    dig(currbrick.x, currbrick.y);      //不是雷，开挖
                    for (int i = 0; i < ny; i++)
                    {
                        for (int j = 0; j < nx; j++)
                        {
                            if (!Bricks[j, i].counted)
                            {
                                box_data = box_data + "?";
                            }
                            else
                            {
                                box_data = box_data + count(j, i);
                            }                         
                        }
                        if (i < ny - 1)
                        {
                            box_data = box_data + ",";
                        }
                    }
            python(box_data);
        }
            }          
        }

        // 调用Python 处理数据
        private void python(string box_data)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("python2.7 E:\\作业\\人工智能\\z3_mine_sweeping\\Python\\demo.py "+box_data+" &exit");
            //p.StandardInput.AutoFlush = true;
            string strOuput = p.StandardOutput.ReadToEnd();
            string[] words = strOuput.Split('\n');
            //MessageBox.Show(strOuput);
            // 结果计数
            int result_num = 0;

            // 对返回数据进行解析
            foreach (var word in words)
            {
                if (word.Contains("row"))
                {
                    string regex = @"(\d+)\D+(\d+)";
                    Match mstr = Regex.Match(word, regex);

                    // 显示结果
                    int y = Convert.ToInt32(mstr.Groups[1].Value);
                    int x = Convert.ToInt32(mstr.Groups[2].Value);

                    if (Bricks[x-1, y-1].counted == true)
                    {
                        continue;
                    }

                    MessageBox.Show(y + "_" + x);
                    // 挖掘
                    dig(x-1, y-1);
                    result_num++;
                }
            }
            p.WaitForExit();
            p.Close();

            // 如果没有返回结果，退出嵌套循环， 否则继续查找
            if (result_num == 0)
            {
                MessageBox.Show("没有结果");
                return;
            }
            else
            {
                string new_box_data = "";
                for (int i = 0; i < ny; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        if (!Bricks[j, i].counted)
                        {
                            new_box_data = new_box_data + "?";
                        }
                        else
                        {
                            new_box_data = new_box_data + count(j, i);
                        }
                    }
                    if (i < ny - 1)
                    {
                        new_box_data = new_box_data + ",";
                    }
                }
                python(new_box_data);
            }
        }

        static void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (null != e)
            {
                Console.WriteLine(e.Data);
            }
        }

        private void findall()    //Game over 显示全图                  
        {
            int n;  //临时变量，周围的雷数
            win = true;
            timer1.Stop();
            for (int x = 0; x < nx; x++)
                for (int y = 0; y < ny; y++) 
                {
                    if (Bricks[x, y].boom == true) //是雷，显示
                    {
                        Bricks[x, y].Text = "@";
                        Bricks[x, y].Font = new Font("GB2312", 9);
                        newfront = new Font(Bricks[x, y].Font, Bricks[x, y].Font.Style | FontStyle.Bold);
                        Bricks[x, y].Font = newfront;
                        Bricks[x, y].BackColor = Color.White;
                        Bricks[x, y].counted = true;
                    }
                    else                            //不是雷，显示   
                    {
                        n = count(x, y);            //计算周围的雷数
                        if (n != 0)                 //周围有雷，显示数字
                        {
                            Bricks[x, y].Text = n.ToString();
                            Bricks[x, y].Font = new Font("GB2312", 9);
                            newfront = new Font(Bricks[x, y].Font, Bricks[x, y].Font.Style | FontStyle.Bold);
                            Bricks[x, y].Font = newfront;
                            Bricks[x, y].BackColor = Color.White;
                            Bricks[x, y].counted = true;
                            switch (n)  //颜色
                            {
                                case 1: Bricks[x, y].ForeColor = Color.Blue; break;
                                case 2: Bricks[x, y].ForeColor = Color.Green; break;
                                case 3: Bricks[x, y].ForeColor = Color.Red; break;
                                case 4: Bricks[x, y].ForeColor = Color.DarkBlue; break;
                                case 5: Bricks[x, y].ForeColor = Color.Brown; break;
                                case 6: Bricks[x, y].ForeColor = Color.Brown; break;
                                case 7: Bricks[x, y].ForeColor = Color.Brown; break;
                                case 8: Bricks[x, y].ForeColor = Color.DarkRed; break;

                            }
                        }
                        else        //周围没有雷，挖开
                        {
                            Bricks[x, y].BackColor = Color.White;
                            Bricks[x, y].Font = new Font("GB2312", 9);
                            Bricks[x, y].Text = " ";
                            Bricks[x, y].counted = true;
                        }
                    }
                }
        }

        private void dig(int x, int y )   //挖
        {
            int n;      //周围的雷数
            if (Bricks[x, y].boom == true)
            {
                Bricks[x, y].Text = "@";
                findall();
                Bricks[x, y].BackColor = Color.Red;
                int i;
                Random r = new Random();
                i = r.Next(5);
                switch (i)
                {
                    case 0: MessageBox.Show("哈哈！！！你挂了！！！"); break;
                    case 1: MessageBox.Show("还是被炸死了吧？？哈哈"); break;
                    case 2: MessageBox.Show("恭喜你成功人肉排雷一个"); break;
                    case 3: MessageBox.Show("别挣扎了，你是过不了的"); break;
                    case 4: MessageBox.Show("卧槽！谁让你踩坏我雷的"); break;
                }
            }
            if (Bricks[x, y].counted==true ) return;//挖到炸弹或已经挖过，返回
            n = count(x, y);//数周围的雷数

            nl--;
            if (nl ==  nb)//判断胜利
            {                
                findall();
                //随机提示语句
                int i;
                Random r = new Random();
                i = r.Next(5);
                switch(i)
                {
                    case 0: MessageBox.Show("什么？？？你竟然赢了？？！！"); break;
                    case 1: MessageBox.Show("这次就勉强算你侥幸赢了吧。。"); break;
                    case 2: MessageBox.Show("哔哔。。程序出错鸟，这次不算"); break;
                    case 3: MessageBox.Show("谢谢使用扫雷弱智版，恭喜通关"); break;
                    case 4: MessageBox.Show("别急，等我重新去搞点雷来玩玩"); break;
                }
                
///////////////////////////////////保存记录////////////////////////////////////////
                FileStream f;
                if(File.Exists("sl.txt")==false)  //无文件？创建！
                {
                     f= new FileStream("sl.txt",FileMode.Create);
                     StreamWriter nf = new StreamWriter(f); 
                     int s = 999999999;
                     nf.WriteLine(s);
                     nf.WriteLine("匿名");
                     nf.WriteLine("匿名");
                     nf.WriteLine("匿名");
                     nf.Close();
                     f.Close();
                }
                f = new FileStream("sl.txt",FileMode.Open);//读取之前的记录
                StreamReader m=new StreamReader(f);
                string higher, midder, lower;   //擂主
                int high, midd, low;    //高级中级初级
                string s1="";       
                s1=m.ReadLine();
                higher = m.ReadLine();
                midder = m.ReadLine();
                lower = m.ReadLine();
                m.Close(); 
                f.Close();

                high = (s1[0] - '0') * 100 + (s1[1] - '0') * 10 + (s1[2] - '0');//处理数据
                midd = (s1[3] - '0') * 100 + (s1[4] - '0') * 10 + (s1[5] - '0');
                low  = (s1[6] - '0') * 100 + (s1[7] - '0') * 10 + (s1[8] - '0');
                if (nx == 10 && ny == 10 && nb == 10 && low  > ti)
                {
                    low = ti;
                    lower = InputBox("留名", "大侠请留名", "匿名");
                }                   
                if (nx == 16 && ny == 16 && nb == 40 && midd > ti)
                {
                    midd = ti;
                    midder = InputBox("留名", "大侠请留名", "匿名");
                }
                if (nx == 30 && ny == 16 && nb == 99 && high > ti)
                {
                    high = ti;
                    higher = InputBox("留名", "大侠请留名", "匿名");
                }

                int s2 = 0;      //写入记录
                s2 = high * 1000000+midd*1000+low;
                f = new FileStream("sl.txt", FileMode.Create);
                StreamWriter nf1 = new StreamWriter(f);
                nf1.WriteLine(s2);
                nf1.WriteLine(higher);
                nf1.WriteLine(midder);
                nf1.WriteLine(lower);
                nf1.Close();
                f.Close();

                ///////////////////////////////////保存记录结束////////////////////////////////////
                return;
            }
            if (n != 0)     //周围有雷，显示数量
            {
                Bricks[x, y].Text = n.ToString();
                Bricks[x, y].BackColor = Color.White;
                Bricks[x, y].Font = new Font("GB2312", 9);
                newfront = new Font(Bricks[x, y].Font, Bricks[x, y].Font.Style | FontStyle.Bold);
                Bricks[x, y].Font = newfront;
                Bricks[x, y].counted = true;
                switch (n)  //颜色
                {
                    case 1: Bricks[x, y].ForeColor = Color.Blue; break;
                    case 2: Bricks[x, y].ForeColor = Color.Green; break;
                    case 3: Bricks[x, y].ForeColor = Color.Red; break;
                    case 4: Bricks[x, y].ForeColor = Color.DarkBlue; break;
                    case 5: Bricks[x, y].ForeColor = Color.Brown; break;
                    case 6: Bricks[x, y].ForeColor = Color.Brown; break;
                    case 7: Bricks[x, y].ForeColor = Color.Brown; break;
                    case 8: Bricks[x, y].ForeColor = Color.DarkRed; break;

                }
            }
            else    //周围没雷，继续开挖
            {
                Bricks[x, y].BackColor = Color.White;
                Bricks[x, y].Font = new Font("GB2312", 9);
                Bricks[x, y].Text = " ";
                Bricks[x, y].counted = true;
                if (x > 0) dig(x - 1, y);
                if (y > 0) dig(x, y - 1);
                if (x < nx-1) dig(x + 1, y);
                if (y < ny-1) dig(x, y + 1);
                if (x > 0 && y > 0) dig(x-1, y-1);
                if (x < nx - 1 && y > 0) dig(x + 1, y - 1);
                if (x > 0 && y < ny - 1) dig(x - 1, y + 1);
                if (x < nx - 1 && y < ny - 1) dig(x + 1, y + 1);
            }
        }

        private int count(int x, int y) //数周围的雷
        {
            int n = 0;
            if (x > 0 && Bricks[x - 1, y].boom == true) n++;
            if (y > 0 && Bricks[x, y - 1].boom == true) n++;
            if (x < nx - 1 && Bricks[x + 1, y].boom == true) n++;
            if (y < ny - 1 && Bricks[x, y + 1].boom == true) n++;
            if (x > 0 && y > 0 && Bricks[x - 1, y - 1].boom == true) n++;
            if (x < nx - 1 && y > 0 && Bricks[x + 1, y - 1].boom == true) n++;
            if (x > 0 && y < ny - 1 && Bricks[x - 1, y + 1].boom == true) n++;
            if (x < nx - 1 && y < ny - 1 && Bricks[x + 1, y + 1].boom == true) n++;
            return n;
        }
        bool find(int a)    //查询是否重复生成雷
        {
            for (int i = 0; i < nb; i++)
            {
                if (boom[i] == a) return true;
            }
            return false;
        }
        public Form1()  //主函数
        {
            InitializeComponent();
            startNewgame();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void 关于扫雷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("看什么看？！就不给你看！By HXL");
        }

        private void 新游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            Controls.Add(menuStrip1);
            startNewgame();
        }

        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("自行百度去吧。。");
        }

        private void 选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
            Controls.Clear();
            Controls.Add(menuStrip1);
            startNewgame();
        }

        private void 统计信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
