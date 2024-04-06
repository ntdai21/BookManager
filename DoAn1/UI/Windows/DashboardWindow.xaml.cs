using DoAn1.BUS;
using DoAn1.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window, INotifyPropertyChanged
    {
        public float NumOfBooks {  get; set; }
        public float NumOfOrders { get; set; }
        public float NumOfDiscounts { get; set; }

        public float NewOrdersDay { get; set; }
        public float NewOrdersWeek { get; set; }
        public float NewOrdersMonth { get; set; }

        public float RevenueDay { get; set; }
        public float RevenueWeek { get; set; }
        public float RevenueMonth { get; set; }

        public DashboardWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.LastWindow = "Dashboard";
            DoAn1.Properties.Settings.Default.Save();

            //
            NumOfBooks = BookDAO.Instance.GetBooks().Count;
            NumOfOrders = OrderDAO.Instance.GetOrders().Count;
            NumOfDiscounts = DiscountDAO.Instance.GetDiscounts().Count;

            DateTime _today = DateTime.Now;

            (NewOrdersDay, NewOrdersWeek, NewOrdersMonth) = OrderDAO.Instance.GetOrderCounts(_today);

            RevenueDay = (float)OrderDAO.Instance.CalculateDailyRevenueAndProfit(_today).Item1;
            RevenueWeek = (float)OrderDAO.Instance.CalculateWeeklyRevenueAndProfit(_today).Item1;
            RevenueMonth = (float)OrderDAO.Instance.CalculateMonthlyRevenueAndProfit(_today.Year, _today.Month).Item1;

            //
            List<(Expression<Func<Book, object>>, bool)> _filters = new List<(Expression<Func<Book, object>>, bool)>();
            _filters = BookBUS.Instance.ModifySortCondition(_filters, book => book.Quantity, "ASC");

            BindingList<Book> books = new();
            int totalPage = 0;
            int itemPerPage = 5;

            (books, totalPage) = BookBUS.Instance.LoadBook(books, 1, itemPerPage, null, double.MinValue, double.MaxValue, "", _filters);

            lowStockBookDataGrid.ItemsSource = books;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
