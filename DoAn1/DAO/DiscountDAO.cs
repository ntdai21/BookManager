using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Discount? FindDiscountByCode(string code)
        {
            return _db.Discounts.FirstOrDefault(c => c.Code == code);
        }

        public List<Discount> GetDiscounts() 
        {
            return _db.Discounts.ToList();
        }

        public List<Discount> GetDiscountsWithPagination(int page, int pageSize, string keyword = "", string sortBy = "")
        {
            return _db.Discounts
                   .Where(discount => string.IsNullOrEmpty(keyword) || discount.Code.Contains(keyword))
                   .OrderBy(discount => discount.Id)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
        }

        public int CountTotalDiscounts(string keyword = "")
        {
            return _db.Discounts.Count(discount => string.IsNullOrEmpty(keyword) || discount.Code.Contains(keyword));
        }

        public void DeleteDiscountById(int id)
        {
            Discount discount = FindDiscountById(id);

            if (discount != null)
            {
                _db.Discounts.Remove(discount);
                _db.SaveChanges();
            }
        }

        public void UpdateDiscount(Discount discount)
        {
            Discount? existingDiscount = _db.Discounts.Find(discount.Id);

            if (existingDiscount != null)
            {
                existingDiscount = discount;
                _db.SaveChanges();
            }
        }

        public void AddDiscount(Discount discount)
        {
            _db.Discounts.Add(discount);
            _db.SaveChanges();
        }
    }
}
