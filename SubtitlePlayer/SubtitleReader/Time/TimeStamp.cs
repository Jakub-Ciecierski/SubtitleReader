using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleReader.Time
{
    public class TimeStamp
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/
        private uint hour;
        private uint min;
        private uint sec;
        private uint ms;

        public uint Hour { get { return hour; } private set { hour = value; } }
        public uint Minute { get { return min; } private set { min = value; } }
        public uint Second { get { return sec; } private set { sec = value; } }
        public uint Millisecond { get { return ms; } private set { ms = value; } }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public TimeStamp()
        {
        }

        public TimeStamp(long ms)
        {
            setTimeMs(ms);
        }

        public TimeStamp(uint hour, uint min, uint sec, uint ms)
        {
            this.hour = hour;
            this.min = min;
            this.sec = sec;
            this.ms = ms;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void setTimeMs(long ms)
        {
            uint seconds = (uint)(ms / 1000);
            uint millisconds = (uint)(ms % 1000);

            uint minutes = seconds / 60;
            seconds = seconds % 60;

            uint hours = minutes / 60;
            minutes = minutes % 60;

            Hour = hours;
            Minute = minutes;
            Second = seconds;
            Millisecond = millisconds;
        }
        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Sets the time in TimeStamp, given by milliseconds
        /// </summary>
        /// <param name="ms"></param>
        public void SetTime(long ms)
        {
            setTimeMs(ms);
        }

        /// <summary>
        ///     Return the length of timestamp in milliseconds
        /// </summary>
        /// <returns></returns>
        public long ToMilliSeconds()
        {
            long hourInMs = Hour*60*60*1000;
            long minInMs = Minute*60*1000;
            long secInMs = Second*1000;

            return hourInMs + minInMs + secInMs + Millisecond;
        }

        /// <summary>
        ///     Adds ms milliseconds to current time
        /// </summary>
        /// <param name="ms"></param>
        public void Add(long ms)
        {
            uint seconds = (uint)(ms / 1000);
            uint millisconds = (uint)(ms % 1000);

            uint minutes = seconds / 60;
            seconds = seconds % 60;

            uint hours = minutes / 60;
            minutes = minutes % 60;

            Millisecond += millisconds;
            uint secToAdd = Millisecond / 1000;
            Millisecond = Millisecond % 1000;

            Second += secToAdd + seconds;
            uint minToAdd = Second / 60;
            Second = Second % 60;

            Minute += minToAdd + minutes;
            uint hourToAdd = Minute / 60;
            Minute = Minute % 60;

            Hour += hourToAdd + hours;
        }

        public override string ToString()
        {
            string hourStr = "";
            if(hour < 10)
                hourStr = "0";
            hourStr += hour;

            string minStr = "";
            if(min < 10)
                minStr = "0";
            minStr += min;

            string secStr = "";
            if (sec < 10)
                secStr = "0";
            secStr += sec;

            string msStr = "";
            if(ms < 100)
                msStr = "0";
            if(ms < 10)
                msStr += "0";
            msStr += ms;

            string timeStr = hourStr + ":" + minStr + ":" + secStr + "," + msStr;

            return timeStr;
        }

        public override bool Equals(object obj)
        {
            var time = obj as TimeStamp;
            return (time.Hour == Hour &&
                    time.Minute == Minute &&
                    time.Second == Second &&
                    time.Millisecond == Millisecond);
        }
    }
}
