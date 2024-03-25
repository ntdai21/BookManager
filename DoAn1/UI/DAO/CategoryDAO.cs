using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1
{
    public class CategoryDAO
    {
        readonly private MyShopContext _db;
        
        //Singleton implementation
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }

                return instance;
            }
        }

        private CategoryDAO() 
        {
            _db = new MyShopContext();
        }

        public List<Category> GetCategories()
        {
            return _db.Categories.ToList();
        }

        public Category? FindById(int categoryId)
        {
            Category? category = _db.Categories.Find(categoryId);
            return category;

        }

        public int AddCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();

            return category.Id;
        }

        public void UpdateCategory(Category category)
        {
            Category? existingCategory = _db.Categories.Find(category.Id);

            if(existingCategory != null) 
            {
                existingCategory.Name=category.Name;
                _db.SaveChanges();
            }

        }

        public bool DeleteCategoryById(int categoryId)
        {
            Category? category = _db.Categories.Find(categoryId);

            if(category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
