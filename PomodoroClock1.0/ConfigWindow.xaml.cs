using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
namespace PomodoroClock
{
    /// <summary>
    /// Lógica interna para ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            JObject jObj = JObject.Parse(File.ReadAllText(@"../../../custom_configs.json"));
            PomodoroCounter counter = new PomodoroCounter(TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()), TimeSpan.Parse(jObj["StartingBreakTime"].ToString()), bool.Parse(jObj["IsSuper"].ToString()));
            tbWorkingHour.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Hours.ToString();
            tbWorkingMinute.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Minutes.ToString();
            tbWorkingSecond.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Seconds.ToString();
            tbBreakHour.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Hours.ToString();
            tbBreakMinute.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Minutes.ToString();
            tbBreakSecond.Text = TimeSpan.Parse(jObj["StartingWorkingTime"].ToString()).Seconds.ToString();
            cbSuper.IsChecked = bool.Parse(jObj["IsSuper"].ToString());
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int[] times = new int[6];
            times[0] = int.Parse(tbWorkingHour.Text);
            times[1] = int.Parse(tbWorkingMinute.Text);
            times[2] = int.Parse(tbWorkingSecond.Text);
            times[3] = int.Parse(tbBreakHour.Text);
            times[4] = int.Parse(tbBreakMinute.Text);
            times[5] = int.Parse(tbBreakSecond.Text);
            PomodoroCounter customCounter;
            if (times[0] <= 59 && times[1] <= 59 && times[2] <= 59 && times[3] <= 59 && times[4] <= 59 && times[5] <= 59)
            {
                customCounter = new PomodoroCounter(times[0], times[1], times[2], times[3], times[4], times[5], (bool)cbSuper.IsChecked);
                Save(customCounter);
            }
            else
            {
                MessageBox.Show("Todos os valores de tempo não devem exceder 59!");
            }
        }
        public void Save(PomodoroCounter counter)
        {
            try
            {
                PomodoroCounter CounterToBeSaved = counter;
                string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(CounterToBeSaved);
                string path = @"../../../custom_configs.json";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    using (var sw = new StreamWriter(path, true))
                    {
                        try
                        {
                            sw.WriteLine(jsonResult.ToString());
                            sw.Close();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    using (var sw = new StreamWriter(path, true))
                    {
                        try
                        {
                            sw.WriteLine(jsonResult.ToString());
                            sw.Close();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                MessageBoxResult result = MessageBox.Show("Informações armazenadas com sucesso! As mudanças serão aplicadas na próxima vez que o aplicativo for iniciado!", "Configurações salvas");
                this.Close();
            }
            catch
            {
                throw;
            }
        }
        private void NumberValidationTextBox(object sender,TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void btnStandard_Click(object sender, RoutedEventArgs e)
        {
            PomodoroCounter standardCounter= new PomodoroCounter(new TimeSpan(0,25,0),new TimeSpan(0,5,0),false);
            Save(standardCounter);
        }
    }
}
