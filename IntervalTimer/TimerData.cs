using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTimer
{
    public class TimerData
    {
        public TimerData(TimeSpan dateTime, string name)
        {
            this.timerLength = dateTime;
            this.name = name;
        }


        public TimeSpan timerLength;

        public string name;

        public override string ToString()
        {
            return name + "-" +  timerLength.ToString("hh\\:mm\\:ss");
        }
    }
}
