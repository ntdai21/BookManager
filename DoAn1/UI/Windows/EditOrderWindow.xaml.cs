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
using DocumentFormat.OpenXml.Drawing.Charts;
using DoAn1.DAO;
using DoAn1.BUS;
using static Azure.Core.HttpHeader;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window, INotifyPropertyChanged
    {

        public Order NewOrder { get; set; } = null;
        BindingList<OrderBook> _orderBooks = null;
        public double GrossPrice { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;


        public event PropertyChangedEventHandler? PropertyChanged;

        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            NewOrder = (Order) order.Clone();
            NewOrder.OrderBooks = OrderBookBUS.Instance.GetOrderBooksByOrderIdWithoutPagination(order.Id);
            
            if (NewOrder.DiscountId == null)
            {
                NewOrder.DiscountId = 0;
            }
            BindingList<Discount> discounts = new BindingList<Discount>(DiscountBUS.Instance.GetDiscounts());
            Discount Coupon = discounts.FirstOrDefault(d => d.Id == NewOrder.DiscountId);
            DiscountComboBox.ItemsSource = discounts;
            DiscountComboBox.SelectedItem = Coupon;

            calculateTotalPrice();
            this.DataContext = this;
            _orderBooks = new BindingList<OrderBook>((IList<OrderBook>)NewOrder.OrderBooks);
            booksDataGrid.ItemsSource = _orderBooks;

        }

        private void calculateTotalPrice()
        {
            GrossPrice = 0;
            foreach (OrderBook orderBook in NewOrder.OrderBooks)
            {
                GrossPrice += (double)(orderBook.Book.Price * (double)orderBook.NumOfBook);
            }
            if (NewOrder.DiscountId == null)
            {
                Discount = 0;
            }
            else
            {
                NewOrder.Discount = DiscountDAO.Instance.FindDiscountById((int)NewOrder.DiscountId);
                if (NewOrder.Discount != null)
                {
                    Discount = (double)(GrossPrice * (NewOrder.Discount.DiscountPercent / 100));
                    if (Discount > NewOrder.Discount.MaxDiscount)
                    {
                        Discount = (double)NewOrder.Discount.MaxDiscount;
                    }
                }
                else
                {
                    Discount = 0;
                }
                
            }
            TotalPrice = GrossPrice - Discount;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.CustomerName = CustomerNameInputFieldUC.UC_TextInput;
            NewOrder.ShippingAddress = ShippingAddressInputFieldUC.UC_TextInput;

            if (NewOrder.CustomerName.Trim() == "")
            {
                MessageBox.Show("You have to insert your customer's name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.ShippingAddress.Trim() == "")
            {
                MessageBox.Show("You have to insert shipping address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.OrderBooks.Count() == 0)
            {
                MessageBox.Show("You have to insert products to your order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewOrder.TotalPrice = TotalPrice;
            NewOrder.CreatedAt = DateTime.Now;

            NewOrder.Discount = null;
            foreach (OrderBook orderBook in NewOrder.OrderBooks)
            {
                orderBook.Book = null;
            }
            OrderDAO.Instance.AddOrder(NewOrder);

            MessageBox.Show("Added a order successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = NewOrder.OrderBooks.SingleOrDefault(ob => ob.BookId == ((OrderBook)dataCxtx).BookId);
            _orderBooks.Remove(orderBook);
            calculateTotalPrice();
            MessageBox.Show("Deleted the selected book successfully!");
        }
        private void TextOnlyButtonUC_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookToOrder(NewOrder, _orderBooks);
            if (screen.ShowDialog() == true)
            {
            }

            calculateTotalPrice();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = (OrderBook)dataCxtx;
            bool isExist = OrderBookBUS.Instance.GetOrderBookByOrderIdAndBookId(orderBook.OrderId, orderBook.BookId) != null;
            int oldNumOfBook = 0;
            if (isExist)
            {
                oldNumOfBook = (int)orderBook.NumOfBook;
                BookBUS.Instance.IncreaseQuantity(orderBook.BookId, oldNumOfBook);
            }
            var screen = new BookQuantityFormWindow(orderBook.Book, (int)orderBook.NumOfBook);
            if (screen.ShowDialog() == true)
            {
                orderBook.NumOfBook = screen.BookQuantity;
                calculateTotalPrice();
                MessageBox.Show($"Updated the selected book successfully! {NewOrder.OrderBooks}");
            }
            if (isExist)
            {
                BookBUS.Instance.DescreaseQuantity(orderBook.BookId, oldNumOfBook);
            }
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Discount discount = (Discount)DiscountComboBox.SelectedItem;
            NewOrder.DiscountId = discount.Id;
            calculateTotalPrice();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.CustomerName = CustomerNameInputFieldUC.UC_TextInput;
            NewOrder.ShippingAddress = ShippingAddressInputFieldUC.UC_TextInput;

            if (NewOrder.CustomerName == "")
            {
                MessageBox.Show("You have to insert your customer's name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.ShippingAddress == "")
            {
                MessageBox.Show("You have to insert shipping address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.OrderBooks.Count() == 0)
            {
                MessageBox.Show("You have to insert products to your order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            NewOrder.TotalPrice = TotalPrice;

            NewOrder.Discount = null;
            /*foreach (OrderBook orderBook in NewOrder.OrderBooks)
            {
                orderBook.Book = null;
            }*/
            int result = OrderDAO.Instance.UpdateOrder(NewOrder);
            if (result != -1)
            {
                MessageBox.Show("Updated a order successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}

