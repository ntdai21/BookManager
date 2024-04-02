using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.DAO
{
    internal class DiscountDAO
    {
        readonly private MyShopContext _db;
        private static DiscountDAO instance;
        public static DiscountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DiscountDAO();
                }
                return instance;
            }
        }

        public DiscountDAO() 
        {
            _db = new MyShopContext();
        }

        public Discount? FindDiscountById(int id)
        {
            return _db.Discounts.FirstOrDefault(d => d.Id == id);
        }

    }
}
