using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// 

    public partial class LoginWindow : Window
    {
        DispatcherTimer timer;
        TimeSpan time;

        public string Server { get; set; }
        public string Database {  get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string AccountEmail { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;

        MyShopContext database = new MyShopContext();

        FirebaseAuthClient client;

        public LoginWindow()
        {
            InitializeComponent();

            loginPanel.DataContext = this;

            // Configure...
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyAOpm9dK3MdwQX_rwLux4IOFFtYTcIZ4QM",
                AuthDomain = "bookmanager-fbfe2.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                // Add and configure individual providers
                new EmailProvider()
                    // ...
                },
                // WPF:
                UserRepository = new FileUserRepository("BookManager") // persist data into %AppData%\FirebaseSample
            };

            client = new FirebaseAuthClient(config);
        }

        async private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AccountEmail == string.Empty || AccountPassword == string.Empty) {
                loginResult.Text = "Vui lòng nhập thông tin tài khoản!";
                return;
            }

            loginProgressBar.Visibility = Visibility.Visible;
            loginBtn.IsEnabled = false;

            try
            {
                var userCredential = await Task.Run(() => {
                    // Test khi chạy quá nhanh
                    System.Threading.Thread.Sleep(1000);

                    try
                    {
                        var _userCredential = client.SignInWithEmailAndPasswordAsync(AccountEmail, AccountPassword);
                        return _userCredential;
                    }

                    catch (Exception)
                    {
                        return null;
                    }
                });

                if (userCredential != null)
                {
                    Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 1));
                    loginResult.Text = string.Empty;
                }

            } catch (Exception ex)
            {
                loginResult.Text = "Đăng nhập thất bại!";
            }

            loginProgressBar.Visibility = Visibility.Hidden;
            loginBtn.IsEnabled = true;

            Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 1));
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            database.LoadConnectionPropertiesFromSettings();
            databasePanel.DataContext = database;

            Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
        }

        private void returnToLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất tài khoản?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 0));
            }
        }

        async private void connectDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            database.UpdateConnectionString();

            connectionProgressBar.Visibility = Visibility.Visible;
            connectionDatabaseBtn.IsEnabled = false;
            fromDatabaseToLoginBtn.IsEnabled = false;

            var canConnect = await Task.Run(() => {

                var _canConnect = database.Database.CanConnectAsync();

                // Test khi chạy quá nhanh
                System.Threading.Thread.Sleep(1000);
                return _canConnect;
            });

            if (canConnect)
            {
                DoAn1.Properties.Settings.Default.SQLServer_Server = database._Server;
                DoAn1.Properties.Settings.Default.SQLServer_Database = database._Database;
                DoAn1.Properties.Settings.Default.SQLServer_UserID = database._UserID;
                DoAn1.Properties.Settings.Default.SQLServer_Password = database._Password;
                DoAn1.Properties.Settings.Default.Save();

                time = TimeSpan.FromSeconds(4);

                timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    databaseConnectionResult.Text = "Kết nối CSDL thành công!\nChuyển hướng đến Dashboard trong " + time.Seconds.ToString();
                    if (time == TimeSpan.Zero)
                    {
                        timer.Stop();
                        DashboardWindow dashboardWindow = new DashboardWindow();
                        dashboardWindow.Show();
                        this.Close();
                    }
                    time = time.Add(TimeSpan.FromSeconds(-1));
                }, Application.Current.Dispatcher);
                timer.Start();
            }
            else
            {
                databaseConnectionResult.Text = "Kết nối CSDL thất bại!";
                connectionDatabaseBtn.IsEnabled = true;
                fromDatabaseToLoginBtn.IsEnabled = false;
            }

            connectionProgressBar.Visibility = Visibility.Hidden;
        }
    }
}
