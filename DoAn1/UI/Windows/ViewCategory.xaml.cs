using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using DoAn1;
using DoAn1.BUS;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for ViewCategory.xaml
    /// </summary>
    public partial class ViewCategory : Window
    {
        BindingList<Category> _categories;
        int _page = 1;
        int _totalPage = 1;
        int _itemsPerPage=DoAn1.Properties.Settings.Default.ItemsPerPage;

        public ViewCategory()
        {
            InitializeComponent();
            
            _categories =new BindingList<Category>();

            (_categories,_totalPage)=CategoryBUS.Instance.LoadCategory(_categories,_page, _itemsPerPage);
            categoryDataGrid.ItemsSource = _categories;

            currentPageButton.Content = $"{_page} of {_totalPage}";

        }

        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = this;
            configurationWindow.ShowDialog();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryBUS.Instance.HandleAddCategory(_categories);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Category selected = categoryDataGrid.SelectedItem as Category;

            CategoryBUS.Instance.HandleUpdateCategory(_categories,selected);

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Category selected = categoryDataGrid.SelectedItem as Category;
            CategoryBUS.Instance.HandleDeleteCategory(selected,_categories);
        }

        private void firstPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = 1;
            _categories.Clear();
            (_categories, _totalPage) = CategoryBUS.Instance.LoadCategory(_categories, _page, _itemsPerPage);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if(_page>1)
            {
                _page = 1;
            }

            _categories.Clear();
            (_categories, _totalPage) = CategoryBUS.Instance.LoadCategory(_categories, _page, _itemsPerPage);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if(_page<_totalPage)
            {
                _page++;
            }

            _categories.Clear();
            (_categories, _totalPage) = CategoryBUS.Instance.LoadCategory(_categories, _page, _itemsPerPage);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }

        private void lastPageButton_Click(object sender, RoutedEventArgs e)
        {
            _page = _totalPage;

            _categories.Clear();
            (_categories, _totalPage) = CategoryBUS.Instance.LoadCategory(_categories, _page, _itemsPerPage);
            currentPageButton.Content = $"{_page} of {_totalPage}";
        }
    }
}
