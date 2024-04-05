﻿using DocumentFormat.OpenXml.Wordprocessing;
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
        public List<OrderBook> FindOrderBooksByOrderIdWithoutPagination(int orderId)
        {
            return _db.OrderBooks
                .Where(orderBook =>  orderBook.OrderId == orderId)
                .ToList();
        }

    }
}
