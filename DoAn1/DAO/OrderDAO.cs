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
        public Tuple<decimal, decimal> CalculateDailyRevenueAndProfit(DateTime date)
        {
            var ordersWithBooks = _db.Orders
           .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == date.Date)
           .Include(o => o.OrderBooks)
               .ThenInclude(ob => ob.Book)
           .ToList();

            decimal totalRevenue = (decimal)ordersWithBooks.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = ordersWithBooks
                .SelectMany(o => o.OrderBooks.Select(ob => new { Book = ob.Book, Quantity = ob.NumOfBook }))
                .Where(x => x.Book != null)
                .Sum(x => (decimal)(x.Book.CostPrice * x.Quantity));

            decimal totalProfit = totalRevenue - totalCost;
            return new Tuple<decimal, decimal>(totalRevenue, totalProfit);
        }

        public Tuple<decimal, decimal> CalculateMonthlyRevenueAndProfit(int year, int month)
        {
            var orders = _db.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Year == year && o.CreatedAt.Value.Month == month)
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = orders
                .SelectMany(o => o.OrderBooks.Select(ob => new { Book = ob.Book, Quantity = ob.NumOfBook }))
                .Where(x => x.Book != null)
                .Sum(x => (decimal)(x.Book.CostPrice * x.Quantity));

            decimal totalProfit = totalRevenue - totalCost;
            return new Tuple<decimal, decimal>(totalRevenue, totalProfit);
        }

        public Tuple<decimal, decimal> CalculateYearlyRevenueAndProfit(int year)
        {
            var orders = _db.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Year == year)
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = orders
                .SelectMany(o => o.OrderBooks.Select(ob => new { Book = ob.Book, Quantity = ob.NumOfBook }))
                .Where(x => x.Book != null)
                .Sum(x => (decimal)(x.Book.CostPrice * x.Quantity));

            decimal totalProfit = totalRevenue - totalCost;
            return new Tuple<decimal, decimal>(totalRevenue, totalProfit);
        }
        public Tuple<string, string> CalculateOverallRevenueAndProfit()
        {
            var orders = _db.Orders
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            decimal totalRevenue = (decimal)orders.Sum(o => o.TotalPrice ?? 0);
            decimal totalCost = orders
                .SelectMany(o => o.OrderBooks.Select(ob => new { Book = ob.Book, Quantity = ob.NumOfBook }))
                .Where(x => x.Book != null)
                .Sum(x => (decimal)(x.Book.CostPrice * x.Quantity));

            decimal totalProfit = totalRevenue - totalCost;
            return new Tuple<string, string>(totalRevenue.ToString(), totalProfit.ToString());
        }

        public List<Tuple<Book, int>> GetTopSellingBooksInMonth(int month, int year, int limit = 7)
        {
            // Lấy danh sách tất cả các đơn hàng cùng với các sách đã được đặt hàng trong tháng và năm cụ thể
            var ordersWithBooks = _db.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == month && o.CreatedAt.Value.Year == year)
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            // Tính tổng số lượng của mỗi cuốn sách đã được bán
            var bookSales = ordersWithBooks
                .SelectMany(o => o.OrderBooks)
                .GroupBy(ob => ob.Book)
                .Select(g => new Tuple<Book, int>(g.Key, (int)g.Sum(ob => ob.NumOfBook)))
                .OrderByDescending(tuple => tuple.Item2)
                .Take(limit)
                .ToList();

            return bookSales;
        }

        public List<Tuple<Book, int>> GetTopSellingBooksInYear(int year, int limit = 7)
        {
            // Lấy danh sách tất cả các đơn hàng cùng với các sách đã được đặt hàng trong năm cụ thể
            var ordersWithBooks = _db.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Year == year)
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            // Tính tổng số lượng của mỗi cuốn sách đã được bán
            var bookSales = ordersWithBooks
                .SelectMany(o => o.OrderBooks)
                .GroupBy(ob => ob.Book)
                .Select(g => new Tuple<Book, int>(g.Key, (int)g.Sum(ob => ob.NumOfBook)))
                .OrderByDescending(tuple => tuple.Item2)
                .Take(limit)
                .ToList();

            return bookSales;
        }
    }
}
