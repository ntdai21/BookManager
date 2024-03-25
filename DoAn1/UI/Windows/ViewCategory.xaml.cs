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
        public ViewCategory()
        {
            InitializeComponent();
            
            _categories =new BindingList<Category>();

            _categories=CategoryBUS.Instance.LoadCategory(_categories);
            categoryDataGrid.ItemsSource = _categories;

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
    }
}
