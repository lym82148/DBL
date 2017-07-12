using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DBL
{
    public static class Bll
    {
        public static string defaultfile = "default.dat";
        public static string allfile = "all.dat";
        public static void LoadDefaultConfig()
        {
            FormConfig result = new FormConfig();
            FileStream fileStream = new FileStream(defaultfile, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();
            result = b.Deserialize(fileStream) as FormConfig;
            fileStream.Close();
            FormConfig.defaultfont = result.font;
            FormConfig.defaultlocation = result.location;
            FormConfig.defaultsize = result.size;
            FormConfig.defaulttext = result.text;
        }
        public static void SaveDefaultConfig()
        {
            FormConfig result = new FormConfig();
            result.font = FormConfig.defaultfont;
            result.location = FormConfig.defaultlocation;
            result.size = FormConfig.defaultsize;
            result.text = FormConfig.defaulttext;
            FileStream fileStream = new FileStream(defaultfile, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, result);
            fileStream.Close();
        }
        public static List<FormConfig> LoadAllConfig()
        {
            List<FormConfig> list = new List<FormConfig>();
            FileStream fileStream = new FileStream(allfile, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();
            list = b.Deserialize(fileStream) as List<FormConfig>;
            fileStream.Close();
            return list;
        }
        public static void SaveAllConfig(List<FormConfig> list)
        {
            FileStream fileStream = new FileStream(allfile, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, list);
            fileStream.Close();
        }


        internal static void CheckDefault()
        {
            if (!File.Exists(defaultfile))
            {
                FormConfig result = new FormConfig();
                result.font = FormConfig.defaultfont;
                result.location = FormConfig.defaultlocation;
                result.size = FormConfig.defaultsize;
                result.text = FormConfig.defaulttext;
                FileStream fileStream = new FileStream(defaultfile, FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, result);
                fileStream.Close();
            }
            if (!File.Exists(allfile))
            {
                List<FormConfig> list = new List<FormConfig>();
                list.Add(new FormConfig());
                SaveAllConfig(list);
            }
        }
    }
}
