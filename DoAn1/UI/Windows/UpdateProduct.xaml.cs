using DoAn1.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using DoAn1.DAO;


namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        public Book _book;
        BindingList<Category> _categories;
        string _clientImagePath;
        Category _selectedCategory;
        public UpdateProduct()
        {
            InitializeComponent();
        }

        public UpdateProduct(Book book)
        {

            _book = book;
            InitializeComponent();
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"{folder}{_book.Cover}";

            if (File.Exists(filePath))
            {
                var bitmap = new BitmapImage(new Uri(filePath, UriKind.Absolute));
                coverImage.Source = bitmap;
            }
            //var bitmap = new BitmapImage(
                //new Uri($"{folder}{_book.Cover}", UriKind.Absolute));

            _categories = CategoryBUS.Instance.LoadCategory(_categories);
            categoryComboBox.ItemsSource = _categories;
            this.DataContext = _book;

            int index = _categories.IndexOf(CategoryDAO.Instance.FindById((int)_book.CategoryId));
            categoryComboBox.SelectedIndex = index;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = BookBUS.Instance.HandleUpdateBook(_book, productGrid, _clientImagePath, _selectedCategory);

            if(result)
            {
                MessageBox.Show("Update successfully");
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Update failed");
                DialogResult = false;
            }
        }


        private void pickImageButton_Click(object sender, RoutedEventArgs e)
        {

            string imagePath = BookBUS.Instance.HandlePickImage(coverImage);

            if(imagePath != null)
            {
                _clientImagePath= imagePath;
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = categoryComboBox.SelectedItem as Category;

            if (category != null)
            {
                _selectedCategory = category;
            }
        }
    }
}
