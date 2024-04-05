using DoAn1.BUS;
using DoAn1.DAO;
using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window, INotifyPropertyChanged
    {
        Order _order = null;
        BindingList<OrderBook> _orderBooks = null;
        public Discount Coupon { get; set; }

        public double GrossPrice { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;

        public event PropertyChangedEventHandler? PropertyChanged;



        private void calculateTotalPrice()
        {
            GrossPrice = 0;
            foreach (OrderBook orderBook in _order.OrderBooks)
            {
                GrossPrice += (double)(orderBook.Book.Price * (double)orderBook.NumOfBook);
            }
            if (_order.DiscountId == null)
            {
                Discount = 0;
            }
            else
            {
                Discount discount = DiscountDAO.Instance.FindDiscountById((int)_order.DiscountId);
                if (discount != null)
                {
                    Discount = (double)(GrossPrice * (discount.DiscountPercent / 100));
                }
            }
            TotalPrice = GrossPrice - Discount;
        }

        public OrderDetailWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            _order.OrderBooks = OrderBookBUS.Instance.GetOrderBooksByOrderIdWithoutPagination(order.Id);
            Coupon = DiscountDAO.Instance.FindDiscountById((int)_order.DiscountId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            orderDetailGrid.DataContext = _order;
            calculateTotalPrice();
            PriceStackPanel.DataContext = this;
            DiscountGrid.DataContext = Coupon;
            _orderBooks = new BindingList<OrderBook>((List<OrderBook>)_order.OrderBooks);
            booksDataGrid.ItemsSource = _orderBooks;
        }

    }
}
