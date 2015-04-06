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

        private bool dragCompleted = false;

        public TimeSlider(Slider timeSlider, Subtitle subtitle, SubtitleTimer subtitleTimer)
        {
            this.timeSlider = timeSlider;

            this.subtitle = subtitle;

            this.subtitleTimer = subtitleTimer;

            timeSlider.ValueChanged += valueChangedHandler;

            timeSlider.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(dragCompletedHandler));
            timeSlider.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(dragStartedHandler));



            setSlider();
        }
        private void setSlider()
        {
            timeSlider.Maximum = subtitle.Length.ToMilliSeconds() / 1000;
            timeSlider.TickFrequency = 1;
            timeSlider.IsSnapToTickEnabled = true;
        }

        private void dragStartedHandler(object sender, DragStartedEventArgs e)
        {
            //dragCompleted = true;
            Console.Write("Drag started\n");
        }

        private void dragCompletedHandler(object sender, DragCompletedEventArgs e)
        {
            dragCompleted = true;
            Console.Write("Drag completed\n");
        }

        private void valueChangedHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(dragCompleted)
            {
                Console.Write(timeSlider.Value + "\n");
                subtitleTimer.Seek(new TimeStamp((long)(timeSlider.Value * 1000)));

                dragCompleted = false;
            }
        }
    }
}
