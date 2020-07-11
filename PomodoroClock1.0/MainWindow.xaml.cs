using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
namespace PomodoroClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = counter;
            Uri path =new Uri( @"..\..\..\assets\icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(path);
            this.Name = "mainWindow";
        }
        public static JObject jObj = JObject.Parse(File.ReadAllText(@"../../../custom_configs.json"));
        public PomodoroCounter counter = new PomodoroCounter(TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()), TimeSpan.Parse(jObj["StartingBreakTime"].ToString()),bool.Parse(jObj["IsSuper"].ToString()));
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            tbText.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 6, 214, 160));
            tbClock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 6, 214, 160));
            btnPlay.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnPause.IsEnabled = true;
            counter.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            tbText.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 208, 0, 0));
            tbClock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 208, 0, 0));
            btnStop.IsEnabled = false;
            btnPause.IsEnabled = false;
            btnPlay.IsEnabled = true;
            counter.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            tbText.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 254, 228, 64));
            tbClock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 254, 228, 64));
            btnPause.IsEnabled = false;
            btnPlay.IsEnabled = true;
            btnStop.IsEnabled = false;
            counter.Stop();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow=new ConfigWindow();
            tbText.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 208, 0, 0));
            tbClock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 208, 0, 0));
            counter.Pause();
            configWindow.Owner = this;
            configWindow.Show();
        }
    }
}
