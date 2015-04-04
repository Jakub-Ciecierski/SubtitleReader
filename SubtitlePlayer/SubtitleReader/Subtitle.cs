using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleReader
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

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public Subtitle(string filename)
        {
            SrtLoader srtLoader = new SrtLoader(filename);
            this.SegmentQueue = srtLoader.ComputeSegments();
        }
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/
       
        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
