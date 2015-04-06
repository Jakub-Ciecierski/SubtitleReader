using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SubtitlePlayer.Converters
{
    public class TimeStampConverter : IValueConverter
    {
        /// <summary>
        ///     Convertes TimeStamp into seconds passed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            TimeStamp time = value as TimeStamp;

            return time.ToMilliSeconds() / 1000;
        }


        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            long time = System.Convert.ToInt64(value);
            time *= 1000;
            return new TimeStamp(time);
        }
    }
}
