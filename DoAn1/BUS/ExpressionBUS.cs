using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.BUS
{
    class ExpressionBUS
    {
        private static ExpressionBUS instance;
        public static ExpressionBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExpressionBUS();
                }

                return instance;
            }
        }

        private ExpressionBUS() { }

        public List<(Expression<Func<Book, object>>, bool)> ModifyExpressionList(
            List<(Expression<Func<Book, object>>, bool)> filter,
            Expression<Func<Book, object>> expressionToModify,
            bool newAscending)
        {
            var index = filter.FindIndex(exp => exp.Item1.ToString() == expressionToModify.ToString());

            if (index != -1)
            {
                filter[index] = (expressionToModify, newAscending);
            }
            else
            {
                filter.Add((expressionToModify, newAscending));
            }

            return filter;
        }



        public List<(Expression<Func<Book, object>>, bool)> RemoveFromExpressionList(
            List<(Expression<Func<Book, object>>, bool)> filter,
            Expression<Func<Book, object>> expressionToRemove)
        {
            var itemToRemove = filter.FirstOrDefault(item => item.Item1.ToString() == expressionToRemove.ToString());

            if (itemToRemove.Item1 != null)
            {
                filter.Remove(itemToRemove);
            }

            return filter;
        }
    }
}
