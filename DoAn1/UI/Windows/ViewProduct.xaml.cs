using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using DoAn1.BUS;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using DoAn1.DAO;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Window
    {
        BindingList<Book> _books;
        BindingList<Category> _categories;

        Category _selectedCategory;
        double _minPrice = double.MinValue, _maxPrice = double.MaxValue;
        List<(Expression<Func<Book, object>>, bool)> _filters = new List<(Expression<Func<Book, object>>, bool)>();
        string _currentSearchTerm = "";

        int _page = 1;
        int _totalPage = 0;
        int rowsPerPage = DoAn1.Properties.Settings.Default.ItemsPerPage;

        public ViewProduct()
        {
            InitializeComponent();

            //Get all book
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            bookListView.ItemsSource = _books;

            //Get all category
            _categories = CategoryBUS.Instance.LoadCategory(_categories);
            _categories = CategoryBUS.Instance.InsertToList(_categories, "All", 0);
            categoryComboBox.ItemsSource = _categories;

            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.LastWindow = "Book Management";
            DoAn1.Properties.Settings.Default.Save();
        }

        private void maxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _maxPrice = (double)e.NewValue;
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            if (currentPageButton != null)
            {
                currentPageButton.Content = $"{_page} of {_totalPage}";
            }
        }

        private void minSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _minPrice = (double)e.NewValue;
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            if (currentPageButton != null)
            {
                currentPageButton.Content = $"{_page} of {_totalPage}";
            }
        }

        private void NameSort_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            if (selected != null)
            {
                string sortBy = selected.Content.ToString();

                _filters = BookBUS.Instance.ModifySortCondition(_filters, book => book.Name, sortBy);
                _page = 1;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }
        }

        private void PriceSort_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            if (selected != null)
            {
                string sortBy = selected.Content.ToString();

                _filters = BookBUS.Instance.ModifySortCondition(_filters, book => book.Price, sortBy);
                _page = 1;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }

        }

        private void searchTextbox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter && searchTextbox.Text != null)
            {
                _minPrice = double.MinValue;
                _maxPrice = double.MaxValue;
                _filters.Clear();
                _page = 1;
                _selectedCategory = null;
                categoryComboBox.SelectedIndex = 0;


                _currentSearchTerm = searchTextbox.Text;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }

        }

        private void IconOnlyButtonUC_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextbox.Text != null)
            {
                _minPrice = double.MinValue;
                _maxPrice = double.MaxValue;
                _filters.Clear();
                _page = 1;
                _selectedCategory = null;
                categoryComboBox.SelectedIndex = 0;


                _currentSearchTerm = searchTextbox.Text;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }
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

                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
                _page = 1;

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }


        }

        private void viewDetailButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            DependencyObject depObj = button;
            while (!(depObj is ListViewItem) && depObj != null)
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }
            ListViewItem listViewItem = depObj as ListViewItem;

            if (listViewItem != null)
            {
                Book selected = (Book)listViewItem.DataContext;
                var screen = new ViewProductDetail(selected);
                screen.Closed += Screen_Closed;
                this.Hide();
                screen.ShowDialog();
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
                currentPageButton.Content = $"{_page} of {_totalPage}";

            }
        }

        private void Screen_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProduct();

            if (screen.ShowDialog() == true)
            {
                Book newBook = screen._book;
                MessageBox.Show("Add successfully");
                _books.Insert(0, newBook);
            }
            else
            {
                MessageBox.Show("Add failed");
            }
        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_page > 1)
            {
                _page--;
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

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
                (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

                if (currentPageButton != null)
                {
                    currentPageButton.Content = $"{_page} of {_totalPage}";
                }
            }
        }

        private void firstPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = 1;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void lastPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = _totalPage;
            (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            if (currentPageButton != null)
            {
                currentPageButton.Content = $"{_page} of {_totalPage}";
            }
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                MessageBoxResult choice = System.Windows.MessageBox.Show($"Do you want to import data from {openFileDialog.FileName}?", "Import data", MessageBoxButton.OKCancel);

                if (choice == MessageBoxResult.OK)
                {
                    bool importResult = BookBUS.Instance.importFromExcel(openFileDialog.FileName,3,2);

                    if (importResult) 
                    { 
                        MessageBox.Show("Import success");

                        _categories.Clear();
                        _categories = CategoryBUS.Instance.LoadCategory(_categories);
                        _categories = CategoryBUS.Instance.InsertToList(_categories, "All", 0);

                        _books.Clear();
                        (_books, _totalPage) = BookBUS.Instance.LoadBook(_books, _page, rowsPerPage, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
                        currentPageButton.Content = $"{_page} of {_totalPage}";
                    }

                }
            }
        }

        private void IconOnlyButtonUC_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private void showSortOption_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanel.Visibility == Visibility.Visible)
                stackPanel.Visibility = Visibility.Collapsed;
            else
                stackPanel.Visibility = Visibility.Visible;
        }
    }
}
