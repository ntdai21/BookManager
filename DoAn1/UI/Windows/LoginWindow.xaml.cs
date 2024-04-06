using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using DoAn1.BUS;

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

        MyShopContext database = new MyShopContext();

        public LoginWindow()
        {
            InitializeComponent();

            loginPanel.DataContext = this;
        }

        async private void Login()
        {
            if (AccountEmail == string.Empty || AccountPassword == string.Empty) {
                loginResult.Text = "Enter your Email and password!";
                return;
            }

            loginProgressBar.Visibility = Visibility.Visible;
            loginBtn.IsEnabled = false;

            var result = await FirebaseBUS.Instance.Login(AccountEmail, AccountPassword);

            if (result.Result == FirebaseBUS.LoginResult.LoginResultType.Failed)
            {
                loginResult.Text = "Failed to log in!";
            }
            else
            {
                if (result.Result == FirebaseBUS.LoginResult.LoginResultType.Activated)
                {
                    if (!(await openLastWindow())) Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
                }
                
                else
                {
                    daysRemainingTxt.Text = result.DayRemain.ToString();

                    if (result.DayRemain == 0) skipBtn.Visibility = Visibility.Collapsed;

                    Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 1));
                }

                loginResult.Text = string.Empty;
            }

            loginProgressBar.Visibility = Visibility.Hidden;
            loginBtn.IsEnabled = true;
        }

        async private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async Task<bool> openLastWindow()
        {

            if (!DoAn1.Properties.Settings.Default.OpenLastWindow) return false;

            if (!(await ConnectDatabase())) return false;

            var lastWindow = DoAn1.Properties.Settings.Default.LastWindow;

            if (string.IsNullOrEmpty(lastWindow) || lastWindow == "Dashboard")
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

        private async void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(await openLastWindow())) Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 2));
        }

        private void returnToLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Your account will be logged out. Countinue?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Dispatcher.BeginInvoke((Action)(() => myTabControl.SelectedIndex = 0));
            }
        }

        async private Task<bool> ConnectDatabase()
        {
            database.UpdateConnectionString();

            if (string.IsNullOrWhiteSpace(database._Server) || string.IsNullOrWhiteSpace(database._Database)) return false;

            connectionProgressBar.Visibility = Visibility.Visible;
            connectionDatabaseBtn.IsEnabled = false;
            fromDatabaseToLoginBtn.IsEnabled = false;

            var canConnect = await database.Database.CanConnectAsync();

            connectionProgressBar.Visibility = Visibility.Hidden;

            return canConnect;
        }

        async private void connectDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = await ConnectDatabase();

            if (result)
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
        }

        async private void activateKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            activationProgressBar.Visibility = Visibility.Visible;
            activateKeyBtn.IsEnabled = false;

            var result = await FirebaseBUS.Instance.ActivateAccount(ActivationKey);

            if (!result)
            {
                activationResult.Text = "Invalid Key!";
            }
            else
            {
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
            if (DoAn1.Properties.Settings.Default.AutoLogin)
            {
                AccountEmail = DoAn1.Properties.Settings.Default.Email;
                AccountPassword = DoAn1.Properties.Settings.Default.Password;
                rememberMeCheckBox.IsChecked = true;
                Login();
            }
        }

        private void loginPanel_Loaded(object sender, RoutedEventArgs e)
        {if (DoAn1.Properties.Settings.Default.AutoLogin)
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

            DoAn1.Properties.Settings.Default.AutoLogin = (rememberMeCheckBox.IsChecked == true);

            DoAn1.Properties.Settings.Default.Save();
        }

        private void activationPanel_Loaded(object sender, RoutedEventArgs e)
        {
            activationResult.Text = "";
        }
    }
}
