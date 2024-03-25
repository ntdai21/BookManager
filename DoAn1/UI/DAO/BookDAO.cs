using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1
{
    public class BookDAO
    {
        readonly private MyShopContext _db;

        //Singleton implementation
        private static BookDAO instance;
        public static BookDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookDAO();
                }

                return instance;
            }
        }

        private BookDAO()
        {
            _db = new MyShopContext();
        }

        public int AddBook(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();

            return book.Id;
        }
        public List<Book> GetBooks()
        {
            return _db.Books.ToList();
        }

        public void UpdateBook(Book book)
        {
            Book? existingBook = _db.Books.Find(book.Id);

            if (existingBook != null)
            {
                existingBook = book;
                _db.SaveChanges();
            }
        }

        public List<Book> GetBooksPaginatedSorted(int page, int pageSize,
            Category? category =null,
            double minPrice = double.MinValue, double maxPrice = double.MaxValue,
            string searchTerm = "",
            params (Expression<Func<Book, object>> expression, bool ascending)[] orderByExpressions)
        {
            int itemsToSkip = (page - 1) * pageSize;

            IQueryable<Book> query = _db.Books;

            if (category != null)
            {
                query = query.Where(b => b.CategoryId == category.Id); 
            }

            foreach (var orderByExpression in orderByExpressions)
            {
                if (orderByExpression.ascending)
                {
                    query = query.OrderBy(orderByExpression.expression);
                }
                else
                {
                    query = query.OrderByDescending(orderByExpression.expression);
                }
            }

            query = query.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
            query = query.Where(b => b.Name.Contains(searchTerm));

            List<Book> books = query.Skip(itemsToSkip).Take(pageSize).ToList();

            return books;
        }

        public Book? FindBookById(int id)
        {
            return _db.Books.FirstOrDefault(b => b.Id == id);
        }

        public void DeleteBookById(int id)
        {
            Book book= FindBookById(id);

            if(book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

    }
}

