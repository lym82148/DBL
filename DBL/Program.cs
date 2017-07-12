using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DBL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //Form1 Dlg = new Form1();
            //Dlg.Size = new System.Drawing.Size(0, 0);

            //Dlg.Show();
            //Dlg.Visible = false;

            //context = new ApplicationContext();
            //context.ThreadExit += new EventHandler(context_ThreadExit);
            //Application.Run(context);  //应用程序运行的主程序是context而窗口就相当于一个“副”程序了。（个人理解）

        }
        //private static ApplicationContext context;

    }
}
