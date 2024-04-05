using DoAn1.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.BUS
{
    public class OrderBookBUS
    {
        private static OrderBookBUS instance;
        public static OrderBookBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderBookBUS();
                }
                return instance;
            }
        }

        private OrderBookBUS() { }

        public List<OrderBook> GetOrderBooksWithoutPagination()
        {
            List<OrderBook> orderBooks = OrderBookDAO.Instance.GetOrderBooks();
            foreach (OrderBook orderBook in orderBooks)
            {
                orderBook.Book = BookDAO.Instance.FindBookById(orderBook.BookId);
            }
            return orderBooks;
        }

        public List<OrderBook> GetOrderBooksByOrderIdWithoutPagination(int orderId)
        {
            List<OrderBook> orderBooks = OrderBookDAO.Instance.FindOrderBooksByOrderIdWithoutPagination(orderId);
            foreach (OrderBook orderBook in orderBooks)
            {
                orderBook.Book = BookDAO.Instance.FindBookById(orderBook.BookId);
            }
            return orderBooks;
        }


    }
}
