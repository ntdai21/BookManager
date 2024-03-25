using DoAn1.BUS;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public Book _book;
        BindingList<Category> _categories;
        string _clientImagePath;
        public AddProduct()
        {
            InitializeComponent();
            _book = new Book();
            _categories = CategoryBUS.Instance.LoadCategory(_categories);
            categoryComboBox.ItemsSource = _categories;
            this.DataContext = _book;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = BookBUS.Instance.HandleAddBook(_book, productGrid);
        }

        private void pickImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = BookBUS.Instance.HandlePickImage(coverImage);

            if(imagePath != null)
            {
                _book.Cover=imagePath;
            }

        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = categoryComboBox.SelectedItem as Category;
            _book.CategoryId = category.Id;
        }
    }
}
