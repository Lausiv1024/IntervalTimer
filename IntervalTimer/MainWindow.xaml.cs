using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MetroRadiance.UI;
using System.Windows.Threading;
using System.IO;
using System.IO.Ports;

namespace IntervalTimer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private string winTitleDefault = "インターバルタイマー";

        DispatcherTimer timer = new DispatcherTimer();
        List<TimerData> timerDatas;
        TimeSpan curTime;
        bool isStarting = false;
        int curIndex = 0;
        bool completelyStopped = true;
        public static AlermSetting setting = new AlermSetting();
        SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent(); 
            ChangeTheme();
            changeColor();
            timerDatas = new List<TimerData>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(update);
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                serialPort = new SerialPort();
                serialPort.PortName = ports[0];
                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
            }
        }

        private void ChangeTheme()
        {
            ThemeService.Current.ChangeTheme(Theme.Dark);
        }

        private void changeColor()
        {
            var systemAccent = new SolidColorBrush(MetroRadiance.Platform.ImmersiveColor.GetColorByTypeName("ImmersiveSystemAccent"));
            IntervalList.BorderBrush = systemAccent;
            timerRyoiki.BorderBrush = systemAccent;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTimer();
            if (dialog.ShowDialog() == true)
            {   
                var a = dialog.timerData;
                timerDatas.Add(a);
                IntervalList.Items.Add(a.ToString());
            }
        }

        private void removeFromIntervalList(int index)
        {
            IntervalList.Items.RemoveAt(index);
            timerDatas.RemoveAt(index);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            int index = IntervalList.SelectedIndex;
            if (index != -1 && index < IntervalList.Items.Count && completelyStopped && !isStarting)
            {
                removeFromIntervalList(index);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (timerDatas.Count == 0)
            {
                MessageBox.Show("インターバルが空です",
                    "なにか設定しろ", MessageBoxButton.OK, MessageBoxImage.Warning);return;
            }
            var dialog = new SaveDialog();
            if (dialog.ShowDialog() == true)
            {
                string name = dialog.namee.Text;
                var manager = new ConfigManager(name, timerDatas);
                if (manager.AlreadyExists)
                {
                    var res = MessageBox.Show("すでに設定が存在しますが\r\n上書きしますか?",
                        "設定の保存", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (res == MessageBoxResult.No) return;
                }
                bool flag = manager.Save();
                if (!flag)
                {
                    MessageBox.Show("設定の保存に失敗しました\n[?,/,\\,*,?,\",<,>,|]は名前に使えません",
                        "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            if (IntervalList.Items.Count != 0)
            {
                if (!isStarting)
                {
                    noe.IsEnabled = false;
                    if (completelyStopped)
                    {
                        TimerCounter.Text = timerDatas[curIndex].timerLength.ToString("h\\:m\\:ss");
                        newInterval(curIndex);
                    }
                    startOrStop(true);
                    completelyStopped = false;
                }
                else
                {
                    startOrStop(false);
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            startOrStop(false);
            TimerCounter.Text = "0:0:00";
            Title = winTitleDefault; 
            noe.IsEnabled = true;
            curIndex = 0;
            intervalName.Text = string.Empty;
            completelyStopped = true;
        }

        private void startOrStop(bool mode)
        {
            isStarting = mode;
            if (mode)
            {
                StartStop.Content = "ストップ";
                timer.Start();
                return;
            }
            StartStop.Content = "スタート";
            timer.Stop();
        }

        private void subtractAndUpdate()
        {
            curTime = curTime.Subtract(TimeSpan.FromSeconds(1));
            TimerCounter.Text = curTime.ToString("h\\:m\\:ss");
        }

        private void newInterval(int index)
        {
            var c = timerDatas[index];
            curTime = c.timerLength;
            Title = winTitleDefault + " - " + c.name;
            intervalName.Text = c.name;
            TimerCounter.Text = curTime.ToString("h\\:m\\:ss");
            IntervalList.SelectedIndex = index;
        }

        private void update(object sender, EventArgs e)
        {
            int indexCount = IntervalList.Items.Count;
            subtractAndUpdate();
            if (curTime.Hours == 0 && curTime.Minutes == 0 && curTime.Seconds == 0)
            {
                curIndex++;
                playSound();
                if (serialPort != null)
                {
                    try
                    {
                        serialPort.Open();
                        serialPort.Write("B");
                        serialPort.Close();
                    }
                    catch { }
                }

                if (indexCount == curIndex)
                {
                    curIndex = 0;
                }
                newInterval(curIndex);
            }
        }

        private void playSound()
        {
            if (IntervalTimer.Properties.Settings.Default.IsFile
                && File.Exists(IntervalTimer.Properties.Settings.Default.SoundPath))
            {
                var sPlayer = new System.Media.SoundPlayer(
                    IntervalTimer.Properties.Settings.Default.SoundPath);
                sPlayer.Play();
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConfigLoadDialog();
            if (dialog.ShowDialog() == true)
            {
                ConfigManager a = new ConfigManager(dialog.getConfigNameList()[dialog.ConfigList.SelectedIndex]);
                a.Load();
                var list = a.TimerDatas;
                foreach (TimerData data in list)
                {
                    timerDatas.Add(data);
                    IntervalList.Items.Add(data.ToString());
                }
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Setting();
            dialog.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!completelyStopped)
            {
                var res = MessageBox.Show("タイマーが動作中です。終了しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (res == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
