using DoAn1.UI.Windows;
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

namespace DoAn1.UI.UserControls
{
    /// <summary>
    /// Interaction logic for NavigationUC.xaml
    /// </summary>
    public partial class NavigationUC : UserControl
    {
        public int UC_Width { get; set; } = 200;
        Window parentWindow;

        public NavigationUC()
        {
            InitializeComponent();
        }

        private void switchMenuMode(object sender, RoutedEventArgs e)
        {
            var size = DoAn1.Properties.Settings.Default.ButtonSize + 2;
            if (menuPanel.Width > size)
            {
                menuPanel.Width = size;
            }
            else
            {
                menuPanel.Width = UC_Width;
            }
        }

        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = parentWindow;
            if (configurationWindow.ShowDialog() == true)
            {
                var lastWindow = DoAn1.Properties.Settings.Default.LastWindow;

                if (string.IsNullOrEmpty(lastWindow) || lastWindow == "Dashboard")
                {
                    var window = new DashboardWindow();
                    window.Show();
                    parentWindow.Close();
                }
                else if (lastWindow == "Book Management")
                {
                    var window = new ViewProduct();
                    window.Show();
                    parentWindow.Close();
                }
                else if (lastWindow == "Invoice Management")
                {
                    var window = new ViewOrder();
                    window.Show();
                    parentWindow.Close();
                }
                else if (lastWindow == "Coupon Management")
                {
                    var window = new CouponManagement();
                    window.Show();
                    parentWindow.Close();
                }
                else if (lastWindow == "Statistical Reporting")
                {
                    var window = new StatisticsWindow();
                    window.Show();
                    parentWindow.Close();
                }
            }
        }

        private void swithToDashboardWindow(object sender, RoutedEventArgs e)
        {
            var window = new DashboardWindow();
            window.Show();
            parentWindow.Close();
        }

        private void switchToCategoryManagementWindow(object sender, RoutedEventArgs e)
        {
            var window = new ViewCategory();
            window.Show();
            parentWindow.Close();
        }

        private void swithToBookManagementWindow(object sender, RoutedEventArgs e)
        {
            var window = new ViewProduct();
            window.Show();
            parentWindow.Close();
        }

        private void swithToInvoiceManagementWindow(object sender, RoutedEventArgs e)
        {
            var window = new ViewOrder();
            window.Show();
            parentWindow.Close();
        }

        private void swithToCouponManagementWindow(object sender, RoutedEventArgs e)
        {
            var window = new CouponManagement();
            window.Show();
            parentWindow.Close();
        }

        private void swithToStatisticalReportingWindow(object sender, RoutedEventArgs e)
        {
            var window = new StatisticsWindow();
            window.Show();
            parentWindow.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
        }
    }
}
