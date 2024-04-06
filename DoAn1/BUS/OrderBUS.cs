using DoAn1.UI.Windows;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DoAn1.BUS
{
    public class OrderBUS
    {
        private static OrderBUS instance;
        public static OrderBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderBUS();
                }
                return instance;
            }
        }

        private OrderBUS() { }

        public Tuple<int,int> AddDataToTable(ObservableCollection<Order> table, int currentPage,int rowsPerPage, string keyword, string sortBy, string dateCreated)
        {
            int totalItems = 0;
            var list = OrderBUS.Instance.GetOrdersWithPagination(currentPage, rowsPerPage, out totalItems, keyword, sortBy, dateCreated);
            table.Clear();
            foreach (var order in list)
            {
                table.Add(order);
            }
            return new Tuple<int,int> ((int)Math.Ceiling((double)totalItems / rowsPerPage),totalItems);
        }


        public List<Order> GetOrdersWithPagination(int page, int pageSize, out int totalItems, string keyword = "", string sortBy = "", string dateCreated = "")
        {
            totalItems = OrderDAO.Instance.CountTotalOrders(keyword,dateCreated);
            return OrderDAO.Instance.GetOrdersWithPagination(page, pageSize, keyword, sortBy, dateCreated);
        }

        public void HandleDeleteOrder(Order selected, ObservableCollection<Order> orders)
        {
            if (selected != null)
            {
                MessageBoxResult choice = MessageBox.Show("Do you want to delete this item?", "Warning", MessageBoxButton.OKCancel);

                if (choice == MessageBoxResult.OK)
                {
                    // Delete in database
                    OrderDAO.Instance.DeleteOrderById(selected.Id);
                    MessageBox.Show("Delete complete");

                    // Delete in render list
                    orders.Remove(selected);
                }
            }
        }

    }
}
