using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SubtitleReader
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

        long timePassed;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public SubtitleTimer(Subtitle subtitle)
        {
            this.subtitle = subtitle;
            CurrentTime = new TimeStamp();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            long timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long delta = timeNow - timePassed;
            timePassed = timeNow;

            CurrentTime.Add(delta);
            Console.Write(CurrentTime.ToString() + "\n");
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Start()
        {
            timer = new Timer(TIMER_TICK);
            timer.Elapsed += onTimedEvent;
            timePassed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            timer.Enabled = true;
        }
    }
}
