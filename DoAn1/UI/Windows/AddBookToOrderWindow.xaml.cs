using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
using Azure;
using DoAn1;
using DoAn1.BUS;
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
        public int BookQuantity { get; set; } = 0;

        BindingList<Book> _books = null;
        BindingList<Category> _categories;

        Category _selectedCategory;
        double _minPrice = double.MinValue, _maxPrice = double.MaxValue;
        List<(Expression<Func<Book, object>>, bool)> _filters = new List<(Expression<Func<Book, object>>, bool)>();
        string _currentSearchTerm = "";

        int _page = 1;
        int _totalPage = 0;

        public AddBookToOrder(Order infoOrder, BindingList<OrderBook> orderBooksBindingList)
        {
            InitializeComponent();
            NewOrders = infoOrder;
            OrderBooksBindingList = orderBooksBindingList;

            //Get all book
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            bookDataGrid.ItemsSource = _books;

            //Get all category
            _categories = CategoryBUS.Instance.LoadCategory(_categories);
            _categories = CategoryBUS.Instance.InsertToList(_categories, "All", 0);
            categoryComboBox.ItemsSource = _categories;

            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Category)(sender as ComboBox).SelectedItem;

            if (selectedItem != null)
            {
                if (selectedItem.Name == "All")
                {
                    _selectedCategory = null;
                }
                else
                {
                    _selectedCategory = selectedItem;
                }

                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
                _page = 1;

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }


        }

        private void searchBook()
        {
            _minPrice = double.MinValue;
            _maxPrice = double.MaxValue;
            _filters.Clear();
            _page = 1;
            _selectedCategory = null;
            categoryComboBox.SelectedIndex = 0;


            _currentSearchTerm = searchTextbox.Text;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            if (currentPageButton != null)
            {
                currentPageButton.Content = $"{_page} of {_totalPage}";
            }
        }
        private void searchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && searchTextbox.Text != null)
            {
                searchBook();
            }
            else
            {
                if (searchTextbox.Text != "")
                {
                    SearchTextBlock.Text = "";
                }
                else SearchTextBlock.Text = "Search here...";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextbox.Text != null)
            {
                searchBook();
            }
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
                    //NewOrders.OrderBooks.Add(orderBook);
                    OrderBooksBindingList.Insert(0, orderBook);
                }
            }
        }


        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_page > 1)
            {
                _page--;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_page < _totalPage)
            {
                _page++;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }
        }

        private void firstPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void searchTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBlock.Visibility = Visibility.Hidden;
        }

        private void searchTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBlock.Visibility = Visibility.Visible;
        }

        private void lastPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = _totalPage;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            if (currentPageButton != null)
            {
                currentPageButton.Content = $"{_page} of {_totalPage}";
            }
        }

    }
}
