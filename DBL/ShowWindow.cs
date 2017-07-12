using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DBL
{
    public partial class ShowWindow : Form
    {
        Form1 parent;
        public ShowWindow(Form1 parent)
        {
            this.parent = parent;
            InitializeComponent();
        }
        private const uint WS_EX_LAYERED = 0x80000;
        //private const uint WS_EX_LAYERED = 0xff0000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0x2;
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(
        IntPtr hwnd,
        int nIndex,
        uint dwNewLong
        );
        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(
        IntPtr hwnd,
        int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(
        IntPtr hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
        );

        /// <summary>
        /// 使窗口有鼠标穿透功能
        /// </summary>
        public void CanPenetrate()
        {
            uint intExTemp = GetWindowLong(this.Handle, GWL_EXSTYLE);
            uint oldGWLEx = SetWindowLong(this.Handle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            //SetLayeredWindowAttributes(this.Handle, 0, 100, LWA_ALPHA);
        }

        private void ShowWindow_Load(object sender, EventArgs e)
        {
            //要时窗体恢复正常，只要执行以下语句：  
            this.FormBorderStyle = FormBorderStyle.None;
            CanPenetrate();
            richTextBox1.Enabled = false;
            trackBar1.Visible = false;
        }


        internal void EndEdit()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            CanPenetrate();
        }

        internal void ShowEdit(FormConfig formconfig)
        {
            this.formconfig = formconfig;
            //SetLayeredWindowAttributes(this.Handle, 0, 255, LWA_ALPHA);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Show();
            trackBar1.Focus();
        }
        FormConfig formconfig;
        private void ShowWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formconfig == null) return;
            formconfig.location = this.Location;
            formconfig.size = this.Size;
            formconfig.text = this.richTextBox1.Text;
            formconfig.alpha = this.trackBar1.Value / 100.0;
            Bll.SaveAllConfig(parent.list);
            Opacity = formconfig.alpha;
            this.EndEdit();
            formconfig = null;
            richTextBox1.Enabled = false;
            trackBar1.Visible = false;
            e.Cancel = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Opacity = this.trackBar1.Value / 100.0;
        }


    }
}
