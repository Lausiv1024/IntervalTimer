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
using System.IO;

namespace IntervalTimer
{
    /// <summary>
    /// ConfigLoadDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigLoadDialog : Window
    {
        private readonly string ConfigFolderPath;

        private List<string> configNameList = new List<string>();
        public ConfigLoadDialog()
        {
            InitializeComponent();
            ConfigFolderPath = Environment.CurrentDirectory + "\\Intervals";
            loadConfig();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            Button button = sender as Button;
            if (button.Name == "OKButton")
            {
                if (ConfigList.SelectedIndex < 0) return;
                DialogResult = true;
                Close();
                return;
            }
            DialogResult = false;
            Close();
        }

        private void loadConfig()
        {
            var configList = Directory.GetFiles(ConfigFolderPath, "*.ini");
            foreach (string s in configList)
            {
                string coName = System.IO.Path.GetFileNameWithoutExtension(s);
                configNameList.Add(coName);
                ConfigList.Items.Add(coName);
            }
        }

        public List<string> getConfigNameList()
        {
            return configNameList;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                this.Close();
            }
        }
    }
}
