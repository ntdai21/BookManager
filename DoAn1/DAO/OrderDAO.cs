using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public List<Order> GetOrdersWithPagination(int page, int pageSize, string keyword = "", string sortBy="")
        {
            if (sortBy == "Latest")
            {
                return _db.Orders
                   .Where(order => string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword) || order.ShippingAddress.Contains(keyword))
                   .OrderByDescending(order => order.CreatedAt) // Sắp xếp theo thời gian tạo mới nhất
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
            }
            else return _db.Orders
                   .Where(order => string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword) || order.ShippingAddress.Contains(keyword))
                   .OrderBy(order => order.CreatedAt) // Sắp xếp theo thời gian tạo mới nhất
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
        }

    

        public int CountTotalOrders(string keyword = "")
        {
            return _db.Orders.Count(order => string.IsNullOrEmpty(keyword) || order.CustomerName.Contains(keyword));
        }

    }
}
