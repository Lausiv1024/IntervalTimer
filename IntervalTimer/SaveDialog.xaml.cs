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

namespace IntervalTimer
{
    /// <summary>
    /// SaveDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class SaveDialog : Window
    {
        public SaveDialog()
        {
            InitializeComponent();
            namee.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            okok();
        }

        private void okok()
        {
            if (namee.Text != string.Empty)
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                okok();
            }else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                this.Close();
            }
        }
    }
}
