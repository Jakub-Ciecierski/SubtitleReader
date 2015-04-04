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

                Subtitle subtitle = new Subtitle(filename);

                SubtitleTimer subtitleTimer = new SubtitleTimer(subtitle);
                subtitleTimer.Start();
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
            timeSlider.Visibility = Visibility.Visible ;
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
           timeSlider.Visibility = Visibility.Collapsed;
            
        }

        private void playPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (playPauseButton.Content == "Stop")
            {
                playPauseButton.Content = "Play";
            }
            else
            {
                playPauseButton.Content = "Stop";
            }
        }
    }
}
