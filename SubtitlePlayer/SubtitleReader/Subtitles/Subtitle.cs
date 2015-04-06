using SubtitleReader.SubLoaders;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleReader.Subtitles
{
    public class Subtitle
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Queue of segments
        /// </summary>
        private List<Segment> segmentQueue;

        public List<Segment> SegmentQueue
        {
            get { return segmentQueue; }
            set { segmentQueue = value; }
        }

        private int currentIndex = -1;

        /// <summary>
        ///     The last subtitle's segment end time stamp.
        ///     In other words, the length of the subtitle
        /// </summary>
        private TimeStamp length;

        public TimeStamp Length
        {
            get { return length; }
            set { length = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public Subtitle(string filename)
        {
            SrtLoader srtLoader = new SrtLoader(filename);
            this.SegmentQueue = srtLoader.ComputeSegments();

            Length = new TimeStamp(segmentQueue[segmentQueue.Count - 1].EndTime.ToMilliSeconds());
        }


        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/
       
        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
        
        /// <summary>
        ///     Seeks for position in the queue
        ///     with specified TimeStamp 
        ///     TODO fix seek
        /// </summary>
        /// <param name="time"></param>
        public bool Seek(TimeStamp time)
        {
            // If time is lower than the first segment, set it to 0
            if (time.ToMilliSeconds() < segmentQueue[0].StartTime.ToMilliSeconds())
            {
                currentIndex = 0;
                return true;
            }

            for (int i = 0; i < segmentQueue.Count - 1; i++)
            {
                Segment seg = segmentQueue[i];
                Segment nextSeg = segmentQueue[i + 1];

                if (time.ToMilliSeconds() > seg.StartTime.ToMilliSeconds() &&
                    time.ToMilliSeconds() < nextSeg.StartTime.ToMilliSeconds())
                {
                    currentIndex = i;
                    return true;
                }
                        
            }
            return false;
        }

        public bool IsNextSegmentReady(TimeStamp time)
        {
            if (currentIndex + 1 >= segmentQueue.Count)
                return false;
            Segment seg = segmentQueue[currentIndex + 1];
            if (seg.IsInInterval(time))
            {
                currentIndex++;
                return true;
            }

            return false;
        }

        public Segment GetCurrentSegment()
        {
            return segmentQueue[currentIndex];
        }
    
    }
}
