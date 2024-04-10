using DoAn1.BUS;
using DoAn1.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            return _db.Orders.AsNoTracking().ToList();
        }

        public Order? FindById(int orderId)
        {
            Order? order = _db.Orders.Find(orderId);
            return order;
        }

        public int AddOrder(Order order)
        {
            _db.Orders.Add(order);
            if (order.DiscountId == 0)
            {
                order.DiscountId = null;
            }
            _db.SaveChanges();

            return order.Id;
        }

        public void UpdateOrderBook(Order order, Book book,int qty)
        {
            Order? selectedOrder = _db.Orders.Find(order.Id);

            if (selectedOrder != null)
            {
                OrderBook? dbBook = (OrderBook)order.OrderBooks.FirstOrDefault(ob => ob.BookId == book.Id);
                bool isExists = dbBook == null ? false : true;

                if (isExists)
                {
                    BookBUS.Instance.IncreaseQuantity(dbBook.BookId, (int)(dbBook.NumOfBook - qty));
                    dbBook.NumOfBook = qty;
                }
                else
                {
                    if (book.Quantity >=  qty)
                    {
                        selectedOrder.OrderBooks.Add(new OrderBook() { BookId = book.Id, NumOfBook = qty});
                        BookBUS.Instance.DescreaseQuantity(book.Id, qty);
                    }
                }
            }
        }

        public int UpdateOrder(Order order)
        {
            Order? selectedOrder = _db.Orders.Include("OrderBooks").Single(o => o.Id == order.Id);

            if (selectedOrder != null)
            {
                selectedOrder.DiscountId = order.DiscountId == 0 ? null : order.DiscountId;
                selectedOrder.CustomerName = order.CustomerName;
                selectedOrder.ShippingAddress = order.ShippingAddress;
                selectedOrder.TotalPrice = order.TotalPrice;

                foreach (OrderBook ob in selectedOrder.OrderBooks.ToList())
                {
                    if (order.OrderBooks.FirstOrDefault(orderBook => orderBook.BookId == ob.BookId) == null)
                    {
                        BookBUS.Instance.IncreaseQuantity(ob.BookId, (int)ob.NumOfBook);
                        selectedOrder.OrderBooks.Remove(ob);
                    }
                }

                foreach (OrderBook ob in order.OrderBooks)
                {
                    UpdateOrderBook(selectedOrder, ob.Book, (int)ob.NumOfBook);
                }

                _db.SaveChanges();

                return order.Id;
            }
            return -1;
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

            return query.AsNoTracking().Skip((page - 1) * pageSize)
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
        public Tuple<int, int> CalculateOverallRevenueAndProfit()
        {
            var orders = _db.Orders
                .Include(o => o.OrderBooks)
                    .ThenInclude(ob => ob.Book)
                .ToList();

            int totalRevenue = (int)orders.Sum(o => o.TotalPrice ?? 0);
            int totalCost = orders
                .SelectMany(o => o.OrderBooks.Select(ob => new { Book = ob.Book, Quantity = ob.NumOfBook }))
                .Where(x => x.Book != null)
                .Sum(x => (int)(x.Book.CostPrice * x.Quantity));

            int totalProfit = totalRevenue - totalCost;
            return new Tuple<int, int>(totalRevenue, totalProfit);
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
        public Tuple<decimal, decimal> CalculateWeeklyRevenueAndProfit(DateTime date)
        {
            // Xác định tuần thứ mấy của năm
            int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Tính ngày đầu tiên và ngày cuối cùng của tuần
            DateTime startDate = FirstDateOfWeekISO8601(date.Year, weekNumber);
            DateTime endDate = startDate.AddDays(6);

            var orders = _db.Orders
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value >= startDate && o.CreatedAt.Value <= endDate)
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

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public Tuple<float, float, float> GetOrderCounts(DateTime today)
        {
            float ordersToday = _db.Orders
                .Count(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today.Date);

            int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateTime startDateOfWeek = FirstDateOfWeekISO8601(today.Year, weekNumber);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            float ordersThisWeek = _db.Orders
                .Count(o => o.CreatedAt.HasValue && o.CreatedAt.Value >= startDateOfWeek && o.CreatedAt.Value <= endDateOfWeek);

            float ordersThisMonth = _db.Orders
                .Count(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Month == today.Month && o.CreatedAt.Value.Year == today.Year);

            return new Tuple<float, float, float>(ordersToday, ordersThisWeek, ordersThisMonth);
        }

    }
}
