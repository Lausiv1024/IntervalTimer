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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using MetroRadiance.UI;

namespace IntervalTimer
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
            SoundPath.Text = Properties.Settings.Default.SoundPath;
            if (Properties.Settings.Default.IsFile)
            {
                FileSelection.IsChecked = true;
            }
            else
            {
                Defo.IsChecked = true;
            }
            ThemeSetting.SelectedIndex = Properties.Settings.Default.Theme;
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "音声ファイル(*.wav)|*.wav|すべてのファイル(*.*)|*.*";
            dialog.Title = "音声ファイルを選択(wavのみ)";
            if (dialog.ShowDialog() == true)
            {
                SoundPath.Text = dialog.FileName;
            }
        }

        private void CheckAndApplySetting(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            Button button = (Button)sender;
            if (button.Name == "CancelButton")
            {
                this.Close();
                return;
            }

            if (button.Name == "TestButton")
            {
                playSound();
                return;
            }

            if (!File.Exists(SoundPath.Text) && FileSelection.IsChecked == true)
            {
                MessageBox.Show("ファイルを正しく設定してください", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Properties.Settings.Default.SoundPath = SoundPath.Text;
            Properties.Settings.Default.IsFile = (bool)FileSelection.IsChecked;
            Properties.Settings.Default.Theme = ThemeSetting.SelectedIndex;
            Properties.Settings.Default.Save();
            ThemeService.Current.ChangeTheme(Properties.Settings.Default.Theme == 0 ? Theme.Light : Theme.Dark);
            this.Close();
        }

        private void playSound()
        {
            if (FileSelection.IsChecked == true
                && File.Exists(SoundPath.Text))
            {
                var sPlayer = new System.Media.SoundPlayer(
                  SoundPath.Text);
                sPlayer.Play();
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
