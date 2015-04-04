using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleReader
{
    public class SrtLoader
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Path to .srt file
        /// </summary>
        private string filename;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public SrtLoader(string filename)
        {
            this.filename = filename;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/
        private void loadFromFile(List<Segment> segments)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);

            int i = 0;
            while (i < lines.Length)
            {
                try{
                    int id = Convert.ToInt32(lines[i++]);
                    TimeStamp[] timeStamps = parseTimeStamp(lines[i++]);
                    string content = "";
                    while (!lines[i].Equals(""))
                    {
                        content += lines[i++] + "\n";
                    }
                    Segment segment = new Segment(id, timeStamps[0], timeStamps[1], content);
                    segments.Add(segment);
                }
                catch (IndexOutOfRangeException e){Console.Write("Tried to access index ouf of range of srt file\n");}
                i++;
            }
        }

        /// <summary>
        ///     Parses a line with time stamps and 
        ///     return Start and End TimeStamps
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private TimeStamp[] parseTimeStamp(string line)
        {
            string pattern = @"(\d{2}):(\d{2}):(\d{2}),(\d{3})";

            Regex r = new Regex(pattern);

            MatchCollection matches = r.Matches(line);

            // Parse StartTime
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

            TimeStamp startTime = new TimeStamp(hourStart, minStart, secStart, msStart);

            // Parse EndTime
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

            TimeStamp endTime = new TimeStamp(hourEnd, minEnd, secEnd, msEnd);

            TimeStamp[] timeStamps = { startTime, endTime };
            return timeStamps;
        }
        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Computes and returns a segment list
        ///     based on the .srt file
        /// </summary>
        /// <returns></returns>
        public List<Segment> ComputeSegments()
        {
            List<Segment> segments = new List<Segment>();
            loadFromFile(segments);


            return segments;
        }
    }
}
