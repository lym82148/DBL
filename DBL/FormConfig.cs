using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DBL
{
    [Serializable]
    public class FormConfig
    {
        public static Point defaultlocation=new Point(500,500);
        public static Font defaultfont = new Font("宋体",24);
        public static Size defaultsize = new Size(500,500);
        public static string defaulttext = "test";
        public static double defaultalpha = 0.1;
        public static Color defaultcolor = Color.Black;
        public Point location = FormConfig.defaultlocation;
        public Font font = FormConfig.defaultfont;
        public Size size = FormConfig.defaultsize;
        public string text = FormConfig.defaulttext;
        public double alpha = FormConfig.defaultalpha;
        public Color color = FormConfig.defaultcolor;
    }
}
