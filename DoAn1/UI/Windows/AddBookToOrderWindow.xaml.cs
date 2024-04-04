using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using DoAn1;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for AddBookToOrder.xaml
    /// </summary>
    public partial class AddBookToOrder : Window
    {
        public Order NewOrders { get; set; } = null;
        public BindingList<OrderBook> OrderBooksBindingList { get; set; } = null;
        BindingList<Book> _books = null;
        public int BookQuantity { get; set; } = 0;

        public delegate void OrderBooksInOrderChangedHandler();
        public event OrderBooksInOrderChangedHandler OrderBooksChanged; // Loaded, Clicked, Moved, Down

        public AddBookToOrder(Order infoOrder, BindingList<OrderBook> orderBooksBindingList)
        {
            InitializeComponent();
            NewOrders = infoOrder;
            OrderBooksBindingList = orderBooksBindingList;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {

        }


        /*private void switchMenuMode(object sender, RoutedEventArgs e)
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
        }*/

        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = this;
            configurationWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _books = new BindingList<Book>(BookDAO.Instance.GetBooks());
            bookDataGrid.ItemsSource = _books;

        }

        private void AddBookToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = (Book) bookDataGrid.SelectedItem;
            if (book == null)
            {
                MessageBox.Show("You have to opt to the book that you want to order", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var screen = new BookQuantityFormWindow(book, BookQuantity);
            if (screen.ShowDialog() == true)
            {
                OrderBook orderBook = NewOrders.OrderBooks.SingleOrDefault(ob => ob.BookId == book.Id);
                if (orderBook != null)
                {
                    orderBook.NumOfBook += screen.BookQuantity;
                }
                else
                {
                    orderBook = new OrderBook() { OrderId = NewOrders.Id, BookId = book.Id, NumOfBook = screen.BookQuantity, Book = book };
                    NewOrders.OrderBooks.Add(orderBook);
                    OrderBooksBindingList.Add(orderBook);
                }
                OrderBooksChanged.Invoke(); 
            }
        }
    }
}
