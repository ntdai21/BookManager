using DoAn1.UI.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAn1.BUS
{
    public class CategoryBUS
    {
        private static CategoryBUS instance;
        public static CategoryBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryBUS();
                }

                return instance;
            }
        }

        private CategoryBUS() { }

        public void addDataToTable(ObservableCollection<Category>table)
        {
            List<Category> list = CategoryDAO.Instance.GetCategories();

            foreach (Category category in list)
            {
                table.Add(category);
            }
        }

        public void HandleAddCategory(ObservableCollection<Category> categories)
        {
            var addScreen = new AddCategory();
            addScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (addScreen.ShowDialog() == true)
            {
                // Add to database
                Category newCategory = new Category();
                newCategory.Name = addScreen.categoryName;
                CategoryDAO.Instance.AddCategory(newCategory);

                // Add to Render list
                categories.Add(newCategory);
                MessageBox.Show("Add successfully");
            }
        }

        public void HandleUpdateCategory(ObservableCollection<Category> categories, Category? selected)
        {
            if (selected != null)
            {
                var addScreen = new AddCategory(selected.Name);
                addScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                if (addScreen.ShowDialog() == true)
                {
                    // Update to database
                    selected.Name = addScreen.categoryName;
                    CategoryDAO.Instance.UpdateCategory(selected);
                    MessageBox.Show("Updated success");

                }
            }
        }

        public void HandleDeleteCategory(Category? selected, ObservableCollection<Category> categories)
        {

            if(selected != null)
            {
                MessageBoxResult choice = MessageBox.Show("Do you want to delete this item?", "Warning", MessageBoxButton.OKCancel);

                if (choice == MessageBoxResult.OK)
                {
                    // Delete in database
                    CategoryDAO.Instance.DeleteCategoryById(selected.Id);
                    MessageBox.Show("Delete complete");

                    // Delete in render list
                    categories.Remove(selected);
                }
            }

        }
    }
}
