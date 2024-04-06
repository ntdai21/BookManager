using DoAn1.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.BUS
{
    public class DiscountBUS
    {
        private static DiscountBUS instance;
        public static DiscountBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DiscountBUS();
                }

                return instance;
            }
        }

        private DiscountBUS() { }

        public List<Discount> GetDiscountsWithPagination(int page, int pageSize, out int totalItems, string keyword = "", string sortBy = "")
        {
            totalItems = DiscountDAO.Instance.CountTotalDiscounts(keyword);
            return DiscountDAO.Instance.GetDiscountsWithPagination(page, pageSize, keyword, sortBy);
        }

        public Tuple<int, int> LoadDiscounts(BindingList<Discount> _discounts, int currentPage, int rowsPerPage, string keyword, string sortBy)
        {
            int totalItems = 0;
            var list = GetDiscountsWithPagination(currentPage, rowsPerPage, out totalItems, keyword, sortBy);
            _discounts.Clear();
            foreach (var discount in list)
            {
                _discounts.Add(discount);
            }
            return new Tuple<int, int>((int)Math.Ceiling((double)totalItems / rowsPerPage), totalItems);
        }

        public enum DiscountValidationResult
        {
            Valid,
            CodeAlreadyExists,
        }

        public static DiscountValidationResult ValidateDiscount(Discount discount)
        {
            var existingDiscount = DiscountDAO.Instance.FindDiscountByCode(discount.Code);

            if (existingDiscount != null && existingDiscount.Id != discount.Id)
            {
                return DiscountValidationResult.CodeAlreadyExists;
            }

            return DiscountValidationResult.Valid;
        }
    }
}
