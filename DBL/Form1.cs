using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.IO;

namespace DBL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //int result = 0;
            //for (int i = 2000; i < 2400; i++)
            //{
            //    result = 0;
            //    if (!DateTime.IsLeapYear(i))
            //    {
            //        continue;
            //    }
            //    for (int j = 1; j < 110; j++)
            //    {
            //        if (DateTime.IsLeapYear(i + j))
            //        {
            //            result += j;
            //        }
            //        if (result == 1200)
            //        {
            //            if (!list2.ContainsKey(j))
            //            {
            //                list2.Add(j, i);
            //            }



            //        }
            //    }
            //}

            //long temp = 10000;
            //long res = Calc(temp, ref temp);





            InitializeComponent();
        }

        public long Calc(long n, ref long nt)
        {
            if (n == 1)
            {
                nt = 1;
                return 1;
            }
            long result = Calc(n - 1, ref nt);
            nt = n * nt;
            return result + nt;
        }
        public int f(int n)
        {
            if (n == 1) return 1;
            if (n == 2) return 3;
            else return f(n - 1) + n * f(n - 1) - n * f(n - 2);
        }
        Dictionary<int, int> list2 = new Dictionary<int, int>();

        public List<FormConfig> list = new List<FormConfig>();
        Dictionary<int, ShowWindow> dic = new Dictionary<int, ShowWindow>();
        Dictionary<int, Item> originDic = new Dictionary<int, Item>();
        private void Form1_Load(object sender, EventArgs e)
        {


            //None = 0,
            //Alt = 1,
            //crtl= 2,
            //Shift = 4,
            //Windows = 8
            //调用时
            //handle:窗体handle 888:热键id  2:ctrl  A: a键
            RegisterHotKey(this.Handle, 888, 3, Keys.X);
            Bll.CheckDefault();
            Bll.LoadDefaultConfig();
            list = Bll.LoadAllConfig();
            ShowWindows();



        }


        private void LoadDefaultConfig()
        {

        }

        private void ShowWindows()
        {
            for (int i = 0; i < list.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = (i + 1) + "";
                item.Size = new System.Drawing.Size(152, 22);
                item.Text = "设置窗口" + (i + 1);
                item.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
                contextMenuStrip1.Items.Insert(1, item);
                ShowWindow swindow = new ShowWindow(this);
                swindow.Text = "关闭自动保存";
                swindow.richTextBox1.Text = list[i].text;
                swindow.richTextBox1.Font = list[i].font;
                swindow.richTextBox1.ForeColor = list[i].color;
                swindow.Opacity = list[i].alpha;
                swindow.Size = list[i].size;
                swindow.Location = list[i].location;
                dic.Add(i, swindow);
                swindow.Show();
            }
        }


        // 注册
        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);

        // 注销
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        // 重写方法 热键触发事件
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    if (m.WParam.ToString().Equals("888")) //如果是注册的那个热键
                        MessageBox.Show("你按了ctrl+a");
                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注销时
            UnregisterHotKey(this.Handle, 888);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            FontDialog font = new FontDialog();
            font.ShowColor = true;
            font.ShowEffects = true;
            int i = Convert.ToInt32(item.Name) - 1;
            FormConfig formconfig = list[i];
            font.Font = formconfig.font;
            ShowWindow showwindow = dic[i];
            if (font.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                formconfig.font = font.Font;
                showwindow.richTextBox1.Font = font.Font;
                showwindow.richTextBox1.ForeColor = font.Color;
                formconfig.color = font.Color;
            }
            showwindow.richTextBox1.Enabled = true;
            showwindow.trackBar1.Value = (int)(formconfig.alpha * 100);
            showwindow.trackBar1.Visible = true;
            showwindow.ShowEdit(formconfig);

        }
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            FontDialog font = new FontDialog();
            font.ShowColor = true;
            font.ShowEffects = true;
            int i = Convert.ToInt32(item.Name) - 1;
            FormConfig formconfig = list[i];
            font.Font = formconfig.font;
            font.Color = formconfig.color;
            ShowWindow showwindow = dic[i];
            if (font.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                formconfig.font = font.Font;
                showwindow.richTextBox1.Font = font.Font;
                showwindow.richTextBox1.ForeColor = font.Color;
                formconfig.color = font.Color;
            }
            showwindow.richTextBox1.Enabled = true;
            showwindow.trackBar1.Value = (int)(formconfig.alpha * 100);
            showwindow.trackBar1.Visible = true;
            showwindow.ShowEdit(formconfig);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock ("test")
            {
                var res = Red();
                if (res > 0)
                {
                    ChangeColor(res);
                }
                else
                {
                    RestoreColor();
                }
            }
        }
        public class Item
        {
            public double Opacity;
            public Color Color;
        }
        private void RestoreColor()
        {
            if (originDic.Count == 0)
            {
                return;
            }
            foreach (var item in dic)
            {
                item.Value.Opacity = originDic[item.Key].Opacity;
                item.Value.richTextBox1.ForeColor = originDic[item.Key].Color;
            }
            originDic.Clear();
        }

        private void ChangeColor(double ca)
        {
            var first = originDic.Count == 0;
            foreach (var item in dic)
            {
                if (first)
                {
                    originDic.Add(item.Key, new Item { Opacity = item.Value.Opacity, Color = item.Value.richTextBox1.ForeColor });
                    item.Value.richTextBox1.ForeColor = Color.Red;
                }
                item.Value.Opacity = ca;
            }
        }

        double Red()
        {
            var date = DateTime.Now;
            var flag = 0d;
            var max = 0.65;
            var mid = 0.25;
            var min = 0.15;
            if (date.Hour == 11)
            {
                if (date.Minute >= 30)
                {
                    flag = max;
                }
                else if (date.Minute >= 28)
                {
                    flag = mid;
                }
                else if (date.Minute > 15)
                {
                    flag = min;
                }
            }
            else if (date.Hour == 18 && (date.Minute >= 0 && date.Minute <= 30))
            {
                flag = max;
            }
            else if (date.Hour == 17)
            {
                if (date.Minute >= 58)
                {
                    flag = mid;
                }
                else if (date.Minute >= 50)
                {
                    flag = min;
                }
            }
            return flag;
        }
    }
}
