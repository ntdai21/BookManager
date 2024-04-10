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

        public OrderBook GetOrderBookByOrderIdAndBookId(int orderId, int bookId)
        {
            return OrderBookDAO.Instance.GetOrderBookByOrderIdAndBookId(orderId, bookId);
        }

        public List<OrderBook> GetOrderBooksWithoutPagination()
        {
            List<OrderBook> orderBooks = OrderBookDAO.Instance.GetOrderBooks();
            foreach (OrderBook orderBook in orderBooks)
            {
                orderBook.Book = BookDAO.Instance.FindBookById(orderBook.BookId);
            }
            return orderBooks;
        }

        public void DeteleOrderBooksByOrderId(int orderId)
        {
            List<OrderBook> orderBooks = OrderBookDAO.Instance.FindOrderBooksByOrderIdWithoutPagination(orderId);
             foreach (OrderBook orderBook in orderBooks)
            {
                OrderBookDAO.Instance.DeleteOrderBook(orderBook);
            }
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

        public bool isValidNewOrderBooks(List<OrderBook> orderBooks)
        {
            foreach (OrderBook orderBook in orderBooks)
            {
                Book book = BookBUS.Instance.FindBookById(orderBook.BookId);
                if (book == null || orderBook.NumOfBook > book.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isValidUpdateOrderBooks(List<OrderBook> oldOrderBooks, List<OrderBook> newOrderBooks)
        {
            foreach (OrderBook orderBook in newOrderBooks)
            {
                Book book = BookBUS.Instance.FindBookById(orderBook.BookId);
                OrderBook oldOrderBook = oldOrderBooks.FirstOrDefault(oob => (oob.BookId ==  orderBook.BookId && oob.OrderId == orderBook.OrderId));
                if (book == null) return false;
                if (oldOrderBook == null)
                {

                }
                if (book == null || oldOrderBook.NumOfBook + book.Quantity < orderBook.NumOfBook)
                {
                    return false;
                }
            }
            return true;
        }

        public void descreaseBookQuantityInOrderBooks(List<OrderBook> orderBooks)
        {
            foreach (OrderBook orderBook in orderBooks)
            {
                BookBUS.Instance.DescreaseQuantity(orderBook.BookId, (int) orderBook.NumOfBook);
            }
        }

    }
}
