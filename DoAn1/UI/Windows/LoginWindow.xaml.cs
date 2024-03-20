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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginPanelToSettingsPanel(object sender, RoutedEventArgs e)
        {
            databaseSettingsPanel.Visibility = Visibility.Visible;
            loginPanel.Visibility = Visibility.Collapsed;
        }

        private void SettingsPanelToLoginPanel(object sender, RoutedEventArgs e)
        {
            loginPanel.Visibility = Visibility.Visible;
            databaseSettingsPanel.Visibility = Visibility.Collapsed;
        }

        private void ExpirationPanelToLoginPanel(object sender, RoutedEventArgs e)
        { 
            if (MessageBox.Show("Tài khoản của bạn sẽ bị đăng xuất, bạn có chắc muốn tiếp tục", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                loginPanel.Visibility = Visibility.Visible;
                expirationDuePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            expirationDuePanel.Visibility = Visibility.Visible;
            loginPanel.Visibility= Visibility.Collapsed;
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();
            this.Close();
        }
    }
}
