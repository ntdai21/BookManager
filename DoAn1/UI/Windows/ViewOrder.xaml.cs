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
using System.Collections.ObjectModel;
using DoAn1.BUS;
namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        int currentPage = 1;
        int totalPages = 0;
        int rowsPerPage = 5;
        int totalItems = 0;
        string keyword = "";
        ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ViewOrder()
        {
            InitializeComponent();
            orderDataGrid.ItemsSource = _orders;
            loadAll();
        }

        public async void loadAll()
        {
            (totalPages, totalItems) = OrderBUS.Instance.AddDataToTable(_orders, currentPage, rowsPerPage, keyword);
            showingItemsText.Text = _orders.Count.ToString();
            totalItemsText.Text = totalItems.ToString();
            currentPageText.Text = currentPage.ToString();
            totalPagesText.Text = totalPages.ToString();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editOrderButton_Click(object sender, RoutedEventArgs e)
        {

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
        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1) return;
            currentPage--;
            loadAll();
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == totalPages) return;
            currentPage++;
            loadAll();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                SearchTextBlock.Text = "";
            }
            else SearchTextBlock.Text = "Search here...";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            keyword = txtSearch.Text;
            loadAll();
        }

        private void detailOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
