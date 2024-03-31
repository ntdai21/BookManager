using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAn1
{
    public class OrderDAO
    {
        readonly private MyShopContext _db;

        //Singleton implementation
        private static OrderDAO instance;
        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }

                return instance;
            }
        }

        private OrderDAO()
        {
            _db = new MyShopContext();
        }

        public List<Order> GetOrders()
        {
            return _db.Orders.ToList();
        }

        public Order? FindById(int orderId)
        {
            Order? order = _db.Orders.Find(orderId);
            return order;
        }

        public int AddOrder(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();

            return order.Id;
        }


        public bool DeleteOrderById(int orderId)
        {
            Order? order = _db.Orders.Find(orderId);

            if (order != null)
            {
                // Retrieve all order_book records associated with the order
                var orderBooks = _db.OrderBooks.Where(ob => ob.OrderId == orderId).ToList();

                // Remove each order_book record
                _db.OrderBooks.RemoveRange(orderBooks);

                // Remove the order itself
                _db.Orders.Remove(order);

                // Save changes
                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public List<Order> GetOrdersWithPagination(int page, int pageSize, string keyword = "", string sortBy = "", string dateCreated = "")
        {
            var query = _db.Orders
                           .Where(order => string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword) || order.ShippingAddress.Contains(keyword));

            if (!string.IsNullOrEmpty(dateCreated) && DateTime.TryParseExact(dateCreated, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime searchDate))
            {
                query = query.Where(order => order.CreatedAt.HasValue && order.CreatedAt.Value.Date == searchDate.Date);
            }

            if (sortBy == "Latest")
            {
                query = query.OrderByDescending(order => order.CreatedAt);
            }
            else
            {
                query = query.OrderBy(order => order.CreatedAt);
            }

            return query.Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int CountTotalOrders(string keyword = "", string dateCreated = "")
        {
            string dateFormat = "MM/dd/yyyy";
            DateTime dateTime;
            bool success = DateTime.TryParseExact(dateCreated, dateFormat, null, System.Globalization.DateTimeStyles.None, out dateTime);
            if (success == true)
            {
                DateTime searchDate = dateTime.Date;
                return _db.Orders.Count(order => (string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword) || order.ShippingAddress.Contains(keyword)) && order.CreatedAt.Value.Date == searchDate);
            }
            else return _db.Orders.Count(order => string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword) || order.ShippingAddress.Contains(keyword));
        }
        public decimal CalculateDailyRevenueAndProfit(DateTime date)
        {
            var orders = _db.Orders.Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == date.Date).ToList();
            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = (decimal) orders.Sum(o => o.OrderBooks.Sum(ob => ob.Book.CostPrice * ob.NumOfBook));
            decimal totalProfit = totalRevenue - totalCost;
            return totalProfit;
        }

        public decimal CalculateMonthlyRevenueAndProfit(int year, int month)
        {
            var orders = _db.Orders.Where(o => o.CreatedAt.Value.Year == year && o.CreatedAt.Value.Month == month).ToList();
            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = (decimal)orders.Sum(o => o.OrderBooks.Sum(ob => ob.Book.CostPrice * ob.NumOfBook));
            decimal totalProfit = totalRevenue - totalCost;
            return totalProfit;
        }

        public decimal CalculateYearlyRevenueAndProfit(int year)
        {
            var orders = _db.Orders.Where(o => o.CreatedAt.Value.Year == year).ToList();
            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = (decimal)orders.Sum(o => o.OrderBooks.Sum(ob => ob.Book.CostPrice * ob.NumOfBook));
            decimal totalProfit = totalRevenue - totalCost;
            return totalProfit;
        }
    }
}
