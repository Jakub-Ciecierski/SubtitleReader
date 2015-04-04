using SubtitleReader.Subtitles;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SubtitleReader.Time
{
    /// <summary>
    ///     Reads current segment of subtitle
    ///     
    /// </summary>
    public class SubtitleTimer
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     In Milliseconds
        /// </summary>
        private const int TIMER_TICK = 10;

        private int currentId;

        public int CurrentID
        {
            get { return currentId; }
            private set { currentId = value; }
        }

        private Subtitle subtitle;

        /// <summary>
        ///     Timer event
        /// </summary>
        private Timer timer;

        /// <summary>
        ///     Current time stamp of the subtitle reader
        /// </summary>
        private TimeStamp currentTime;

        public TimeStamp CurrentTime
        {
            get { return currentTime; }
            private set { currentTime = value; }
        }

        /// <summary>
        ///     The current segment in time
        /// </summary>
        private Segment currentSegment;

        public Segment CurrentSegment
        {
            get { return currentSegment;}
            private set { currentSegment = value;}
        }

        private long timePassed;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public SubtitleTimer(Subtitle subtitle)
        {
            this.subtitle = subtitle;
            CurrentTime = new TimeStamp();

            timer = new Timer(TIMER_TICK);
            timer.Elapsed += onTimedEvent;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            long timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long delta = timeNow - timePassed;
            timePassed = timeNow;

            lock (CurrentTime)
            {
                CurrentTime.Add(delta);

                if(subtitle.IsNextSegmentReady(CurrentTime))
                {
                    CurrentSegment = subtitle.GetCurrentSegment();

                    Console.Write(CurrentSegment.ToString() + "\n");
                }
            }
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Starts the timer
        /// </summary>
        public void Start()
        {
            timePassed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            timer.Enabled = true;
        }

        /// <summary>
        ///     Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Enabled = false;
        }

        /// <summary>
        ///     Seeks to given TimeStamp, changes current segment
        ///     according to that time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Seek(TimeStamp time)
        {
            if (time.ToMilliSeconds() >= subtitle.Length.ToMilliSeconds())
                return false;

            Stop();
            subtitle.Seek(time);
            lock(CurrentTime)
            {
                CurrentTime = time;
            }
            Start();

            return true;
        }
    }
}
