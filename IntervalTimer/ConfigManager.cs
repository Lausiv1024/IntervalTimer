using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace IntervalTimer
{
    public class ConfigManager
    {
        public List<TimerData> TimerDatas { get; set; }
        private string worlkngDir;
        public string ConfName { get; private set; }
        private bool loaded = false;
        public bool AlreadyExists = false;

        public ConfigManager(string conf)
        {
            
            TimerDatas = new List<TimerData>();
            worlkngDir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            ConfName = conf;
        }

        public ConfigManager(string conf, List<TimerData> a)
        {
            TimerDatas = a;
            ConfName = conf;
            worlkngDir = Environment.CurrentDirectory;
            var filePath = Path.Combine(worlkngDir, "Intervals", $"{conf}.ini");
            //やっばファイル存在チェックしないと
            AlreadyExists = File.Exists(filePath);
        }

        /// <summary>
        /// 設定を読み込みます
        /// </summary>
        public void Load()
        {
            var filePath = Path.Combine(worlkngDir, "Intervals", $"{ConfName}.ini");
            string data;
            if (File.Exists(filePath))
            {
                try
                {
                    using (var reader = new StreamReader(filePath, Encoding.UTF8))
                    {
                        if (reader.ReadLine() == "[Intervals]")
                        {
                            string setName = reader.ReadLine();
                            if (setName.Substring(0, 5) == "Name=")
                            {
                                ConfName = setName.Substring(5);
                            }

                            while ((data = reader.ReadLine()) != null)
                            {
                                string[] datas = data.Split('-');
                                var name = datas[0];
                                var timeStr = datas[1];
                                var timer = new TimerData(TimeSpan.Parse(timeStr), name);
                                TimerDatas.Add(timer);
                            }
                        }
                    }
                    AlreadyExists = true;
                }
                catch
                {
                    loaded = false;
                    Console.WriteLine("Failed to load file.");
                }
            }
            else
            {
                throw new ArgumentException("Data not found");
            }
        }

        public bool IsLoaded()
        {
            return loaded;
        }

        /// <summary>
        /// 設定を保存します
        /// </summary>
        public bool Save()
        {
            var filePath = Path.Combine(worlkngDir, "Intervals", $"{ConfName}.ini");
            try
            {
                using(var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("[Intervals]");
                    writer.WriteLine("Name=" + ConfName);
                    foreach(var data in TimerDatas)
                    {
                        string dataStr = data.ToString();
                        writer.WriteLine(dataStr);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
