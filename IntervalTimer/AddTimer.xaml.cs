using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntervalTimer
{
    /// <summary>
    /// AddTimer.xaml の相互作用ロジック
    /// </summary>
    public partial class AddTimer : Window
    {
        private readonly string NoName = "名前を指定してください";
        private readonly string NoTime = "必ず数字を入力してください";
        private readonly string NoNameAndTime = "名前と時間を指定してください";
        private readonly string NoInt = "数字を入力してください";
        private readonly string ALL0 = "すべてに0は指定できません";
        public TimerData timerData;
        private DispatcherTimer timer;
        public AddTimer()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timerName.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void showWarning(string a)
        {
            if (timer != null) timer.Stop();
            warning.Text = a;
            warning.Visibility = Visibility.Visible;

            System.Media.SystemSounds.Hand.Play();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += new EventHandler(hideWarning);
            timer.Start();
        }

        private void hideWarning(object sender, EventArgs e)
        {
            warning.Visibility = Visibility.Hidden;
            timer.Stop();
        }

        

        private void confirm()
        {
            int second;
            int minutes;
            int hours;
            if (timerName.Text == string.Empty && Seconds.Text == string.Empty)
            {
                showWarning(NoNameAndTime);
                return;
            }
            if (timerName.Text == string.Empty)
            {
                showWarning(NoName);
                return;
            }
            if (Seconds.Text == string.Empty || Minutes.Text == string.Empty || Hour.Text == string.Empty)
            {
                showWarning(NoTime);
                return;
            }
            if (Minutes.Text == "0" && Seconds.Text == "0" && Hour.Text == "0")
            {
                showWarning(ALL0);
                return;
            }
            if (!int.TryParse(Seconds.Text, out second) || !int.TryParse(Minutes.Text, out minutes) || !int.TryParse(Hour.Text, out hours))
            {
                showWarning(NoInt);
                return;
            }

            var a = new TimeSpan(hours, minutes, second);
            timerData = new TimerData(a, timerName.Text);
            DialogResult = true;
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            confirm();
        }

        private bool nextUIElement()
        {
            if (timerName.IsFocused)
            {
                Hour.Focus();
                return true;
            }else if (Hour.IsFocused)
            {
                Minutes.Focus();
                return true;
            }else if (Minutes.IsFocused)
            {
                Seconds.Focus();
                return true;
            }

            return false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                bool flag = nextUIElement();
                if (!flag)
                {
                    confirm();
                }
            }else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                this.Close();
            }
        }

        private void onTextBoxGetFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                textBox.SelectAll();
                e.Handled = true;
            }
            else
            {
                throw new ArgumentException("senderがTextBoxじゃないです");
            }
        }

        private void onTextBoxGetFocus(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                textBox.SelectAll();
                e.Handled = true;
            } else
            {
                throw new ArgumentException("senderがTextBoxじゃないです");
            }
            
        }
    }
}
