using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
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
using System.ComponentModel;
using DoAn1.Models;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        
        Order _orders = null;
        BindingList<OrderBook> _orderBooks = new BindingList<OrderBook>();
        public double GrossPrice { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;

        public string GrossPriceString
        {
            get
            {
                return (string) GrossPrice.ToString();
            }
        }

        public string DiscountString
        {
            get
            {
                return (string)Discount.ToString();
            }
        }

        public string TotalPriceString
        {
            get
            {
                return (string)TotalPrice.ToString();
            }
        }

        public CreateOrderWindow()
        {
            InitializeComponent();
        }

        private void calculateTotalPrice()
        {
            GrossPrice = 0;
            foreach (OrderBook orderBook in _orders.OrderBooks)
            {
                GrossPrice += (double)(orderBook.Book.Price * (double)orderBook.NumOfBook);
            }
            if (_orders.DiscountId == null)
            {
                Discount = 0;
            } 
            else
            {
                Discount = (double)(GrossPrice * (_orders.Discount.DiscountPercent / 100));
            }
            TotalPrice = GrossPrice - Discount;

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Success");
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
                menuPanel.Width = 200;
            }
        }

        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = this;
            configurationWindow.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = _orders.OrderBooks.SingleOrDefault(ob => ob.BookId == ((OrderBook)dataCxtx).BookId);
            _orders.OrderBooks.Remove(orderBook);
            _orderBooks.Remove(orderBook);
            calculateTotalPrice();
            MessageBox.Show("Deleted the selected book successfully!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _orders = new Order();
            booksDataGrid.ItemsSource = _orderBooks;
            MyShopContext db = new MyShopContext();
            DiscountComboBox.ItemsSource = new BindingList<Discount>(db.Discounts.ToList());

            GrossPriceTextFieldUC.DataContext = GrossPriceString;
            DiscountTextFieldUC.DataContext = DiscountString;
            TotalPriceTextFieldUC.DataContext = TotalPriceString;
        }

        private void TextOnlyButtonUC_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookToOrder(_orders, _orderBooks);
            if (screen.ShowDialog() == false)
            {
                //_orderBooks = new BindingList<OrderBook>((List<OrderBook>)screen.NewOrders.OrderBooks);
                //booksDataGrid.ItemsSource = _orderBooks;
            }

            /*Order order = new Order() { CustomerName = "Nguyen Thien Nhan 2" };
            Book book = BookDAO.Instance.GetBooks()[0];
            OrderBook orderBook = new OrderBook() { NumOfBook = 1, BookId = book.Id};
            order.OrderBooks.Add(orderBook);
            OrderDAO.Instance.AddOrder(order);
            MessageBox.Show("Added a order successfully");*/

            calculateTotalPrice();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = (OrderBook)dataCxtx;
            var screen = new BookQuantityFormWindow(orderBook.Book, (int)orderBook.NumOfBook);
            if (screen.ShowDialog() == true)
            {
                orderBook.NumOfBook = screen.BookQuantity;
                calculateTotalPrice();
                MessageBox.Show($"Updated the selected book successfully! {_orders.OrderBooks}");
            } 
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Discount discount = (Discount)DiscountComboBox.SelectedItem;
            _orders.DiscountId = discount.Id;
            _orders.Discount = discount;
            calculateTotalPrice();
        }
    }
}
