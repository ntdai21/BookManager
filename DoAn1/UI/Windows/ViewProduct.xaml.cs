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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

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

        public ViewProduct()
        {
            InitializeComponent();

            //Get all book
            _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            bookListView.ItemsSource = _books;

            //Get all category
            _categories = CategoryBUS.Instance.LoadCategory(_categories);
            _categories = CategoryBUS.Instance.InsertToList(_categories, "All", 0);
            categoryComboBox.ItemsSource = _categories;
        }



        private void maxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _maxPrice = (double)e.NewValue;
            _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
        }

        private void minSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _minPrice = (double)e.NewValue;
            _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
        }

        private void NameSort_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            if (selected != null)
            {
                string sortBy = selected.Content.ToString();

                _filters = BookBUS.Instance.ModifySortCondition(_filters, book => book.Name, sortBy);
                _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            }
        }


        private void PriceSort_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            if (selected != null)
            {
                string sortBy = selected.Content.ToString();

                _filters = BookBUS.Instance.ModifySortCondition(_filters, book => book.Price, sortBy);
                _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            }

        }

        private void searchTextbox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter && searchTextbox.Text != null)
            {
                _currentSearchTerm = searchTextbox.Text;
                _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);
            }

        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Category)(sender as ComboBox).SelectedItem;

            if (selectedItem.Name == "All")
            {
                _selectedCategory = null;
            }
            else
            {
                _selectedCategory = selectedItem;
            }

            _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);



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
                _books = BookBUS.Instance.LoadBook(_books, 1, 10, _selectedCategory, _minPrice, _maxPrice, _currentSearchTerm, _filters);

            }
        }

        private void Screen_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProduct();

            if(screen.ShowDialog()==true)
            {
                Book newBook = screen._book;
                MessageBox.Show("Add successfully");
                _books.Insert(0,newBook);
            }
            else
            {
                MessageBox.Show("Add failed");
            }
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
