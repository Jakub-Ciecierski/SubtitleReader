using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleReader.Util
{
    public class Formats
    {
        public static bool IsAvaible(string filename)
        {
            string ext = filename.Substring(filename.Length - 4);
            return ext.Equals(".srt");
        }
    }
}
