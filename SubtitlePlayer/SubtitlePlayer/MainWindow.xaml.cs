using SubtitleReader;
using SubtitleReader.Subtitles;
using SubtitleReader.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps;
using MahApps.Metro.Controls;
namespace SubtitlePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Subtitle subtitle;

        private SubtitleTimer subtitleTimer;

        private TimeSlider timeSlider;

        public MainWindow()
        {
            InitializeComponent();
            //var background = new SolidColorBrush(Colors.White);
           // this.Background = background;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.ShowTitleBar = true;
            Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#FF000000");
            this.Background = new SolidColorBrush(color);
        }

        private void setBindings()
        {
            this.DataContext = subtitleTimer;
        }

        private void addButtonClick(object sender, RoutedEventArgs e)
        {
            if (subtitleTimer != null)
            {
                TimeStamp currentTime = new TimeStamp(subtitleTimer.CurrentTime.ToMilliSeconds() + 1000);

                subtitleTimer.Seek(currentTime);
            }
        }
        private void subtractButtonClick(object sender, RoutedEventArgs e)
        {
            if (subtitleTimer != null) 
            {
                TimeStamp currentTime = new TimeStamp(subtitleTimer.CurrentTime.ToMilliSeconds() - 1000);

                subtitleTimer.Seek(currentTime);
            }
        }

        private void openSubtitleButtonClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Subtitle"; // Default file name
            dlg.DefaultExt = ".srt"; // Default file extension
            dlg.Filter = "Subtitles (*.srt) | *.srt";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                subtitle = new Subtitle(filename);

                subtitleTimer = new SubtitleTimer(subtitle);

                setBindings();

                timeSlider = new TimeSlider(timeSliderControl, subtitle, subtitleTimer);
            }
        }

        /// <summary>
        ///  Mouse entering the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ShowTitleBar = true;
            Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#4F000000");
            this.Background = new SolidColorBrush(color);

            playPauseButton.Visibility = Visibility.Visible;
            openButton.Visibility = Visibility.Visible;
            timeStampTextBox.Visibility = Visibility.Visible;
            timeSliderControl.Visibility = Visibility.Visible;
            subtractButton.Visibility = Visibility.Visible;
            addButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Mouse leaving the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ShowTitleBar = false;
            Color color = (Color)ColorConverter.ConvertFromString("#00DFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#01FF0000");
            this.Background = new SolidColorBrush(color);

            playPauseButton.Visibility = Visibility.Collapsed;
            openButton.Visibility = Visibility.Collapsed;
            timeStampTextBox.Visibility = Visibility.Collapsed;
            timeSliderControl.Visibility = Visibility.Collapsed;
            subtractButton.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;
            
            subtitleText.Visibility = Visibility.Visible;
        }

        private void playPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (subtitleTimer != null)
            {
                if (playPauseButton.Content.Equals("Stop"))
                {
                    playPauseButton.Content = "Play";
                    Console.Write("Start subtitle \n");
                    if (subtitleTimer != null)
                        subtitleTimer.Stop();
                }
                else
                {
                    playPauseButton.Content = "Stop";
                    Console.Write("Stop subtitle \n");
                    if (subtitleTimer != null)
                        subtitleTimer.Start();
                }
            }
        }
    }
}
