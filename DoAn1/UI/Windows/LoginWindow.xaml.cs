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
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.ComponentModel;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// 

    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        DispatcherTimer timer;
        TimeSpan time;

        public string AccountEmail { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
        public string ActivationKey {  get; set; } = string.Empty;

        FirebaseAuthClient client;
        UserCredential userCredential;
        FirebaseClient firebase = new FirebaseClient("https://bookmanager-fbfe2-default-rtdb.asia-southeast1.firebasedatabase.app/");

        MyShopContext database = new MyShopContext();

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
            //Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
            //return;

            if (AccountEmail == string.Empty || AccountPassword == string.Empty) {
                loginResult.Text = "Enter your Email and password!";
                return;
            }

            loginProgressBar.Visibility = Visibility.Visible;
            loginBtn.IsEnabled = false;

            try
            {
                userCredential = await Task.Run(() => {
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
                    //Validation Account
                    var obj = await firebase.Child("Accounts").Child(userCredential.User.Uid).Child("ExpirationDate").OnceAsJsonAsync();

                    var expDateStr = obj.ToString().Replace("\"", "").Trim();

                    if (string.IsNullOrEmpty(expDateStr)) {

                        if (!openLastWindow()) Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));

                    } else //Not activated yet
                    {
                        DateTime dt = DateTime.ParseExact(expDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        int daysRemaining = int.Max((dt.Date - DateTime.Now.Date).Days, 0);

                        daysRemainingTxt.Text = daysRemaining.ToString();

                        if (daysRemaining == 0) skipBtn.Visibility = Visibility.Collapsed;

                        activationResult.Text = "";

                        Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 1));
                    }

                    loginResult.Text = string.Empty;
                }

            } catch (Exception ex)
            {
                loginResult.Text = "Failed to log in!";
            }

            loginProgressBar.Visibility = Visibility.Hidden;
            loginBtn.IsEnabled = true;
        }

        private bool openLastWindow()
        {

            if (DoAn1.Properties.Settings.Default.OpenLastWindow == 0) return false;

            var lastWindow = DoAn1.Properties.Settings.Default.LastWindow;

            if (string.IsNullOrEmpty(lastWindow))
            {
                Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
            }
            else if (lastWindow == "Dashboard")
            {
                var window = new DashboardWindow();
                window.Show();
                this.Close();
            }
            else if (lastWindow == "Book Management")
            {
                var window = new ViewProduct();
                window.Show();
                this.Close();
            }
            else if (lastWindow == "Invoice Management")
            {
                var window = new ViewOrder();
                window.Show();
                this.Close();
            }
            else if (lastWindow == "Coupon Management")
            {
                var window = new DashboardWindow();
                window.Show();
                this.Close();
            }
            else if (lastWindow == "Statistical Reporting")
            {
                var window = new StatisticsWindow();
                window.Show();
                this.Close();
            }
            return true;
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!openLastWindow()) Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
        }

        private void returnToLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Your account will be logged out. Countinue?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                    databaseConnectionResult.Text = "Database connected. Redirect to Dashboard in " + time.Seconds.ToString();
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
                databaseConnectionResult.Text = "Failed to connect to database!";
                connectionDatabaseBtn.IsEnabled = true;
                fromDatabaseToLoginBtn.IsEnabled = false;
            }

            connectionProgressBar.Visibility = Visibility.Hidden;
        }

        async private void activateKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            activationProgressBar.Visibility = Visibility.Visible;
            activateKeyBtn.IsEnabled = false;

            var obj = await firebase.Child("ActivationKeys").Child(ActivationKey).OnceAsJsonAsync();

            if (obj != "true")
            {
                activationResult.Text = "Invalid Key!";
            }
            else
            {
                await firebase.Child("Accounts").Child(userCredential.User.Uid).Child("ExpirationDate").PutAsync("\"\"");

                activationResult.Text = "Thank you for purchasing!";
                skipBtn.UC_Text = "Next";
                skipBtn.Visibility = Visibility.Visible;
                activateKeyBtn.Visibility = Visibility.Collapsed;
            }

            activateKeyBtn.IsEnabled = true;
            activationProgressBar.Visibility = Visibility.Hidden;
        }

        private void databasePanel_Loaded(object sender, RoutedEventArgs e)
        {
            database.LoadConnectionPropertiesFromSettings();
            databasePanel.DataContext = database;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void loginPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (DoAn1.Properties.Settings.Default.RememberAccount != 0)
            {
                AccountEmail = DoAn1.Properties.Settings.Default.Email;
                AccountPassword = DoAn1.Properties.Settings.Default.Password;
                rememberMeCheckBox.IsChecked = true;
            }
            else
            {
                AccountEmail = "";
                AccountPassword = "";
                rememberMeCheckBox.IsChecked = false;
            }
        }

        private void loginPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            if (rememberMeCheckBox.IsChecked == true)
            {
                DoAn1.Properties.Settings.Default.Email = AccountEmail;
                DoAn1.Properties.Settings.Default.Password = AccountPassword;
            }

            DoAn1.Properties.Settings.Default.RememberAccount = (rememberMeCheckBox.IsChecked == true) ? 1 : 0;

            DoAn1.Properties.Settings.Default.Save();
        }
    }
}
