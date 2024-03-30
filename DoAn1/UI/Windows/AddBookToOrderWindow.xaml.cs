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
        Order _newOrders = null;
        BindingList<Book> _books = null;
        MyShopContext _db = null;
        public int BookQuantity { get; set; } = 0;
        public AddBookToOrder(Order infoOrder)
        {
            InitializeComponent();
            _newOrders = infoOrder;
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
            _db = new MyShopContext();
            if (_db.Database.CanConnect())
            {
                _books = new BindingList<Book>(_db.Books.ToList());
                bookDataGrid.ItemsSource = _books;
            }
            
        }

        private void AddBookToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = (Book) bookDataGrid.SelectedItem;
            var screen = new BookQuantityFormWindow(book, BookQuantity);
            if (screen.ShowDialog() == true)
            {
                _newOrders.OrderBooks.Add(new OrderBook() { OrderId = _newOrders.Id, BookId = book.Id, NumOfBook = screen.BookQuantity });
            }
        }
    }
}
