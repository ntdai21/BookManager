﻿using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.DAO
{
    public class OrderBookDAO
    {
        readonly private MyShopContext _db;
        private static OrderBookDAO instance;
        public static OrderBookDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderBookDAO();
                }
                return instance;
            }
        }

        public OrderBookDAO()
        {
            _db = new MyShopContext();
        }

        public List<OrderBook> GetOrderBooks()
        {
            return _db.OrderBooks.ToList();
        }

        public OrderBook GetOrderBookByOrderIdAndBookId(int orderId, int bookId)
        {
            if (orderId == null || bookId == null) return null;
            List<OrderBook> orderBooks = _db.OrderBooks.AsNoTracking().Where(orderBook => orderBook.OrderId == orderId && orderBook.BookId == bookId).ToList();
            return orderBooks.Count == 0 ? null : orderBooks[0];
        }


        public void DeleteOrderBook(OrderBook orderBook)
        {
            _db.OrderBooks.Remove(orderBook);
            _db.SaveChanges();
        }
        public List<OrderBook> FindOrderBooksByOrderIdWithoutPagination(int orderId)
        {
            return _db.OrderBooks.AsNoTracking()
                .Where(orderBook =>  orderBook.OrderId == orderId)
                .ToList();
        }

    }
}
