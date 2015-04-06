using SubtitleReader.Subtitles;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SubtitleTimer : INotifyPropertyChanged
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
             set 
            { 
                currentTime = value; 
                OnPropertyChanged("CurrentTime"); 
            }
        }

        /// <summary>
        ///     The current segment in time
        /// </summary>
        private Segment currentSegment;

        public Segment CurrentSegment
        {
            get { return currentSegment;}
            private set 
            { 
                currentSegment = value;
                RaisePropertyChanged("CurrentSegment");
            }
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

            CurrentSegment = new Segment();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     The timer event, computes and adds time ellapsed
        ///     to CurrentTime, and selects next segment
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            long timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long delta = timeNow - timePassed;
            timePassed = timeNow;

            lock (CurrentTime)
            {
                CurrentTime.Add(delta);
                OnPropertyChanged("CurrentTime");
                fetchCurrentSegment();
            }
        }

        /// <summary>
        ///     Looks for next appropriate segment
        /// </summary>
        private void fetchCurrentSegment()
        {
            if (subtitle.IsNextSegmentReady(CurrentTime))
            {
                CurrentSegment = subtitle.GetCurrentSegment();
                Console.Write(CurrentSegment.ToString() + "\n");
            }

            // If the time interval has passed, set the 
            // current segment to empty one
            if (CurrentSegment.Finished(CurrentTime))
            {
                CurrentSegment = new Segment();
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

            bool wasStoped = timer.Enabled;

            Stop();
            CurrentSegment = new Segment();
            subtitle.Seek(time);
            lock(CurrentTime)
            {
                CurrentTime = time;
            }
            if(wasStoped)
                Start();

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
