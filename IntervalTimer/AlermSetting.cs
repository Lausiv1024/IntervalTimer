using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTimer
{
    public class AlermSetting
    {
        public bool IsFile;
        public string FilePath;

        public AlermSetting(bool isFile, string filePath)
        {
            IsFile = isFile;
            FilePath = filePath;
        }

        public AlermSetting()
        {
            IsFile = false;
            FilePath = string.Empty;
        }
    }
}
