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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SubtitleReader.Util;

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

        bool subtitleLoaded = false;

        public MainWindow()
        {
            InitializeComponent();

            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.ShowTitleBar = true;
            Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#FF000000");
            this.Background = new SolidColorBrush(color);
        }

        /// <summary>
        ///     Starts the actual subtitle app
        /// </summary>
        /// <param name="filename"></param>
        private void startSubtitle(string filename)
        {
            if (!Formats.IsAvaible(filename))
                return;

            subtitle = new Subtitle(filename);

            subtitleTimer = new SubtitleTimer(subtitle);

            setBindings();

            timeSlider = new TimeSlider(timeSliderControl, subtitle, subtitleTimer);

            subtitleLoaded = true;
            setComponentsVisible();
        }

        /// <summary>
        ///     Sets binding for data
        /// </summary>
        private void setBindings()
        {
            this.DataContext = subtitleTimer;
        }


        /// <summary>
        ///     Sets the window visible
        /// </summary>
        private void setComponentsVisible()
        {
            this.ShowTitleBar = true;
            Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#4F000000");
            this.Background = new SolidColorBrush(color);

            openButton.Visibility = Visibility.Visible;
            settingsButton.Visibility = Visibility.Visible;
            playPauseButton.Visibility = Visibility.Visible;

            subtractButton.Visibility = Visibility.Visible;
            addButton.Visibility = Visibility.Visible;

            timeStampTextBox.Visibility = Visibility.Visible;
            timeSliderControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Sets the window invisible
        /// </summary>
        private void setComponentsInvisible()
        {
            this.ShowTitleBar = false;
            Color color = (Color)ColorConverter.ConvertFromString("#00DFD991");
            myBorder.BorderBrush = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString("#01FF0000");
            this.Background = new SolidColorBrush(color);
            openButton.Visibility = Visibility.Collapsed;
            settingsButton.Visibility = Visibility.Collapsed;
            playPauseButton.Visibility = Visibility.Collapsed;

            subtractButton.Visibility = Visibility.Collapsed;
            addButton.Visibility = Visibility.Collapsed;

            timeStampTextBox.Visibility = Visibility.Collapsed;
            timeSliderControl.Visibility = Visibility.Collapsed;
        }

        /********************** EVENT HANDLERS **********************/

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

        /// <summary>
        ///     Open file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                startSubtitle(filename);
            }
        }

        /// <summary>
        ///  Mouse entering the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (subtitleLoaded)
                setComponentsVisible();
        }

        /// <summary>
        /// Mouse leaving the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            if (subtitleLoaded)
                setComponentsInvisible();
        }

        /// <summary>
        ///     Opens settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsButtonClick(object sender, RoutedEventArgs e)
        {

            FontChooser fontChooser = new FontChooser("Change font:", subtitleText.FontSize.ToString());
            if (fontChooser.ShowDialog() == true)
            {
                subtitleText.FontSize = System.Convert.ToInt32(fontChooser.Answer);
            }
                
        }

        /// <summary>
        ///     Handles the drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowDropFileHandler(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            startSubtitle(filenames[0]);
        }

    }
}
