using SubtitleReader.Subtitles;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SubtitlePlayer
{
    public class TimeSlider
    {
        private Slider timeSlider;

        private Subtitle subtitle;

        private SubtitleTimer subtitleTimer;

        public TimeSlider(Slider timeSlider, Subtitle subtitle, SubtitleTimer subtitleTimer)
        {
            this.timeSlider = timeSlider;

            this.subtitle = subtitle;

            this.subtitleTimer = subtitleTimer;

            setSlider();
        }
        private void setSlider()
        {
            timeSlider.Maximum = subtitle.Length.ToMilliSeconds() / 1000;
            timeSlider.TickFrequency = 1;
            timeSlider.IsSnapToTickEnabled = true;
        }
    }
}
