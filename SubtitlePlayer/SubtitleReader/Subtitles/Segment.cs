using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleReader.Subtitles
{
    /// <summary>
    ///     Segment is a single segment of a subtitle
    ///     Discribed by:
    ///     1) Its position in subtitle( unique id)
    ///     2) Time stamp interval - start and finish
    ///     3) Content
    /// </summary>
    public class Segment : INotifyPropertyChanged
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/
        /// <summary>
        ///     The id of the segment
        /// </summary>
        private int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }

        /// <summary>
        ///     When to activate the segment
        /// </summary>
        private TimeStamp startTime;

        public TimeStamp StartTime
        {
            get { return startTime; }
            private set { startTime = value; }
        }

        /// <summary>
        ///     When to deactivate the segment
        /// </summary>
        private TimeStamp endTime;

        public TimeStamp EndTime
        {
            get { return endTime; }
            private set { endTime = value; }
        }

        /// <summary>
        ///     The actual display content
        /// </summary>
        private string content;

        public string Content
        {
            get { return content; }
            private set { content = value; OnPropertyChanged("Content"); }
        }

        /// <summary>
        ///     Duration of segment in milliseconds
        /// </summary>
        private long duration;

        public long Duration
        {
            get 
            { 
                return EndTime.ToMilliSeconds() - StartTime.ToMilliSeconds(); 
            }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Segment()
        {
            Id = -1;
            Content = "";
            EndTime = new TimeStamp();
            StartTime = new TimeStamp();
        }

        public Segment(int id, TimeStamp start, TimeStamp end, string content)
        {
            Id = id;
            StartTime = start;
            EndTime = end;
            Content = content;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Checks if given TimeStamp fits in
        ///     the segment interval
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsInInterval(TimeStamp time)
        {
            return (time.ToMilliSeconds() >= StartTime.ToMilliSeconds() &&
                    time.ToMilliSeconds() <= EndTime.ToMilliSeconds());
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public bool Finished(TimeStamp time)
        {
            if (time.ToMilliSeconds() >= EndTime.ToMilliSeconds())
                return true;
            return false;
        }

        public override string ToString()
        {
            string idStr = "" + Id;
            string time = StartTime + " --> " + EndTime;

            string segment = idStr + "\n" + time + "\n" + content;

            return segment;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
