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

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public string categoryName;
        public AddCategory()
        {
            InitializeComponent();
        }
        public AddCategory(string currentName)
        {
            InitializeComponent();
            categoryTextbox.Text=currentName;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            categoryName = categoryTextbox.Text;
            DialogResult = true;
        }

        private void categoryTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
