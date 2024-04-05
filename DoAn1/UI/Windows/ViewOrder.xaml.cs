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
        string sortBy = "Latest";
        bool isLoaded = false;
        ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ViewOrder()
        {
            InitializeComponent();
            orderDataGrid.ItemsSource = _orders;
            sortCombobox.SelectedItem = 0;
            loadAll();
        }
        
        public async void loadAll()
        {
            if(!isLoaded) { return; }
            (totalPages, totalItems) = OrderBUS.Instance.AddDataToTable(_orders, currentPage, rowsPerPage, keyword, sortBy);
            showingItemsText.Text = _orders.Count.ToString();
            totalItemsText.Text = totalItems.ToString();
            currentPageText.Text = currentPage.ToString();
            totalPagesText.Text = totalPages.ToString();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CreateOrderWindow();
            if (screen.ShowDialog() == true)
            {

            }
        }

        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Order selected = orderDataGrid.SelectedItem as Order;
            OrderBUS.Instance.HandleDeleteOrder(selected, _orders);
            loadAll();
        }

        private void editOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            Order selectedOrder = (Order)button.DataContext;
            var screen = new EditOrderWindow(selectedOrder);
            if (screen.ShowDialog() == true)
            {

            }
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
            var button = (Button)e.OriginalSource;
            Order selectedOrder = (Order)button.DataContext;
            var screen = new OrderDetailWindow(selectedOrder);
            if (screen.ShowDialog() == true)
            {

            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)sortCombobox.SelectedItem;
            sortBy = selectedItem.Content.ToString();
            loadAll();
        }

        private void ViewOrder_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            loadAll();

            DoAn1.Properties.Settings.Default.LastWindow = "Invoice Management";
            DoAn1.Properties.Settings.Default.Save();
        }
    }
}
