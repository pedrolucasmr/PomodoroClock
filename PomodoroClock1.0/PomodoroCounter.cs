using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.IO;

namespace PomodoroClock
{
    public class PomodoroCounter:INotifyPropertyChanged
    {
        public TimeSpan StartingWorkingTime { get; set; }
        public TimeSpan StartingBreakTime { get; set; }
        public TimeSpan currentTime;
        private int State=0; //0-> Stopped, 1-> Working, 2-> Break
        private bool IsPlaying { get; set; } = false;
        Timer workingTimeTimer;
        public string currentText="Estamos Parados!";
        public TimeSpan CurrentTime
        {
            get { return currentTime; }
            set 
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }
        public string CurrentText
        {
            get => currentText;
            set 
            { 
                currentText=value; 
                OnPropertyChanged("CurrentText");
            }
        }
        public bool IsSuper { get; set; }
        public int SuperCounter { get; set; } = 0;
        public PomodoroCounter()
        {
            this.StartingWorkingTime= new TimeSpan(0,0,5);
            this.CurrentTime = StartingWorkingTime;
            this.StartingBreakTime = new TimeSpan(0, 0, 5);
            this.IsSuper = false;
        }
        public PomodoroCounter(int workingHour, int workingMinute, int workingSecond, int breakHour, int breakMinute, int breakSecond,bool super)
        {
            this.StartingWorkingTime =new TimeSpan(workingHour, workingMinute, workingSecond);
            this.CurrentTime = StartingWorkingTime;
            this.StartingBreakTime = new TimeSpan(breakHour, breakMinute, breakSecond);
            this.IsSuper = super;
        }
        public PomodoroCounter(TimeSpan workingTime, TimeSpan breakTime, bool super)
        {
            this.StartingWorkingTime = workingTime;
            this.CurrentTime = StartingWorkingTime;
            this.StartingBreakTime = breakTime;
            this.IsSuper = super;
        }
        public void Play()
        {
            if (IsPlaying == false && this.State==0)
            {

                IsPlaying = true;
                workingTimeTimer = new Timer(TimerCallback, null, 0, 1000);
                ChangeState();
            }
            else if (IsPlaying==false && this.State!=0)
            {
                IsPlaying = true;
                workingTimeTimer = new Timer(TimerCallback, null, 0, 1000);
                CurrentText = ChangeText();
            }
            else
            {
                MessageBox.Show("O cronômetro já está correndo!");
            }
        }
        public void Stop()
        {
            if (this.State != 0)
            {
                workingTimeTimer.Dispose();
                this.State = 0;
                CurrentTime = StartingWorkingTime;
                IsPlaying = false;
                CurrentText = "Estamos parados!";
            }
            else
            {
                MessageBox.Show("Já estamos parados!");
            }
        }
        public void Pause()
        {
            if (IsPlaying == true)
            {
                IsPlaying = false;
                workingTimeTimer.Dispose();
                CurrentText = PauseText();
            }
        }
        public void WorkingTime()
        {
            try
            {
                CurrentTime = StartingWorkingTime;
                CurrentText=ChangeText();
            }
            catch
            {
                MessageBox.Show("Um erro Ocorreu");
            }
        }
        public void BreakTime()
        {
            try
            {
                if ((this.IsSuper == true)&&(this.SuperCounter>=3))
                {
                        CurrentTime = StartingBreakTime*2;
                        SuperCounter = 0;
                }
                else if ((this.IsSuper == true) && (this.SuperCounter <= 2))
                {
                    CurrentTime = StartingBreakTime;
                    SuperCounter++;
                }
                else
                {
                    CurrentTime = StartingBreakTime;
                }
                CurrentText = ChangeText();
            }
            catch
            {
                MessageBox.Show("Um erro Ocorreu");
            }            
        }
        private void ChangeState()
        {
            if (this.State == 0)
            {
                this.State = 1;
                WorkingTime();
            }
            else if (this.State == 1)
            {
                this.State = 2;
                BreakTime();
                Play();
            }
            else if(this.State==2)
            {
                this.State = 1;
                WorkingTime();
                Play();
            }
        }
        private void TimerCallback(object o)
        {
            if (CurrentTime.TotalMilliseconds > 0)
            {
                CurrentTime=CurrentTime.Subtract(new TimeSpan(0, 0, 1));
                GC.Collect();
            }
            else
            {
                workingTimeTimer.Dispose();
                IsPlaying = false;
                /*System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                string path = @"..\..\..\assets\alarmsound.wav";
                player.SoundLocation = path;
                player.Play();*/
                Thread.Sleep(2500);
                ChangeState();
            }
        }
        private string ChangeText()
        {
            return this.State == 1 ? "Está na hora de trabalhar!" : "Está na hora de descançar!";
        }
        private string PauseText()
        {
            return this.State == 1 ? "Pausa no trabalho!" : "Pausa no descanço!";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
