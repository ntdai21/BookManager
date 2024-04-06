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

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public bool AutoLogin { get; set; } = DoAn1.Properties.Settings.Default.AutoLogin;
        public bool OpenLastWindow { get; set; } = DoAn1.Properties.Settings.Default.OpenLastWindow;
        public int NumOfItemsPerPage { get; set; } = DoAn1.Properties.Settings.Default.ItemsPerPage;

        public ConfigurationWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.AutoLogin = AutoLogin;
            DoAn1.Properties.Settings.Default.OpenLastWindow = OpenLastWindow;
            DoAn1.Properties.Settings.Default.ItemsPerPage = NumOfItemsPerPage;

            DoAn1.Properties.Settings.Default.Save();

            DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
