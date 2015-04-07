using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubtitleReader;
using SubtitleReader.Subtitles;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnitTestSubtitle
{
    [TestClass]
    public class SubtitleTest
    {
        [TestMethod]
        public void Regex_TimeString_TinmeStamp()
        {
            TimeStamp expectedStartTime = new TimeStamp(0, 0, 0, 667);
            TimeStamp expectedEndTime = new TimeStamp(0, 0, 3, 335);

            string timeStr = "00:00:00,667 --> 00:00:03,335";
            
            //string pattern = @"[\d\d]:[\d\d]:[\d\d],[\d\d\d]";
            string pattern = @"(\d{2}):(\d{2}):(\d{2}),(\d{3})";

            Regex r = new Regex(pattern);

            MatchCollection matches = r.Matches(timeStr);

            string startHourStr = "";
            string startMinStr = "";
            string startSecStr = "";
            string startMsStr = "";

            startHourStr = matches[0].Groups[1].Value;
            startMinStr = matches[0].Groups[2].Value;
            startSecStr = matches[0].Groups[3].Value;
            startMsStr = matches[0].Groups[4].Value;
            
            uint hourStart = Convert.ToUInt32(startHourStr);
            uint minStart = Convert.ToUInt32(startMinStr);
            uint secStart = Convert.ToUInt32(startSecStr);
            uint msStart = Convert.ToUInt32(startMsStr);

            TimeStamp actualStartTime = new TimeStamp(hourStart, minStart, secStart, msStart);


            string endHourStr = "";
            string endMinStr = "";
            string endSecStr = "";
            string endMsStr = "";

            endHourStr = matches[1].Groups[1].Value;
            endMinStr = matches[1].Groups[2].Value;
            endSecStr = matches[1].Groups[3].Value;
            endMsStr = matches[1].Groups[4].Value;

            uint hourEnd = Convert.ToUInt32(endHourStr);
            uint minEnd = Convert.ToUInt32(endMinStr);
            uint secEnd = Convert.ToUInt32(endSecStr);
            uint msEnd = Convert.ToUInt32(endMsStr);

            TimeStamp actualEndTime = new TimeStamp(hourEnd, minEnd, secEnd, msEnd);

            Assert.AreEqual(expectedStartTime, actualStartTime);
            Assert.AreEqual(expectedEndTime, actualEndTime);
        }

        [TestMethod]
        public void TimeStamp_AddMilliseconds_TimeAdded()
        {
            TimeStamp time1 = new TimeStamp(2, 21, 26, 999);
            TimeStamp actualTime = new TimeStamp(1, 15, 20, 490);

            TimeStamp expectedTime = new TimeStamp(3, 36, 47, 489);

            long time1Ms = time1.ToMilliSeconds();

            actualTime.Add(time1Ms);

            Assert.AreEqual(expectedTime, actualTime);
        }

        [TestMethod]
        public void CheckTimeInterval_TimeStamp_FallsInInterval()
        {
            TimeStamp start = new TimeStamp(0, 0, 1, 0);
            TimeStamp end = new TimeStamp(0, 0, 1, 500);

            Segment segment = new Segment(0, start, end, "");

            TimeStamp someTime = new TimeStamp(0, 0, 1, 399);

            bool expectedValue = true;

            Assert.AreEqual(expectedValue, segment.IsInInterval(someTime));
        }

        [TestMethod]
        public void TimeStamp_TimeStampFromHours_TimeStampFromMilliseconds()
        {
            TimeStamp actualTime = new TimeStamp(1, 24, 57, 111);

            TimeStamp expectedTime = new TimeStamp(actualTime.ToMilliSeconds());

            Assert.AreEqual(expectedTime, actualTime);
        }


        [TestMethod]
        public void Regex_Tag_TagRemoved()
        {
            string actualContent = "<format> asd </format>";

            string pattern = @"</?\w+>";
            string rep = "";
            Regex r = new Regex(pattern);

            actualContent = r.Replace(actualContent, rep);

            string expectedContent = " asd ";

            Assert.AreEqual(expectedContent, actualContent);
        }
    }
}
