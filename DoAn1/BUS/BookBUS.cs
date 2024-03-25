using DoAn1.BUS;
using DoAn1.UI.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DoAn1
{
    public class BookBUS
    {
        private static BookBUS instance;
        public static BookBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookBUS();
                }

                return instance;
            }
        }

        private BookBUS() { }

        public BindingList<Book> LoadBook(BindingList<Book>? books, int page, int pageSize,
            Category? category,
            double minPrice, double maxPrice,
            string searchTerm,
            List<(Expression<Func<Book, object>>, bool)> orderByExpressions)
        {
            if (books == null)
            {
                books = new BindingList<Book>();
            }

            List<Book> bookList = BookDAO.Instance.GetBooksPaginatedSorted(page, pageSize, category, minPrice, maxPrice, searchTerm, orderByExpressions.ToArray());
            books.Clear();

            foreach (Book book in bookList)
            {
                books.Add(book);
            }

            return books;

        }

        public List<(Expression<Func<Book, object>>, bool)> ModifySortCondition(List<(Expression<Func<Book, object>>, bool)> current, Expression<Func<Book, object>> criteriaExpression, string sortBy)
        {
            switch (sortBy)
            {
                case "ASC":
                    current = ExpressionBUS.Instance.ModifyExpressionList(current, criteriaExpression, true);
                    break;
                case "DES":
                    current = ExpressionBUS.Instance.ModifyExpressionList(current, criteriaExpression, false);
                    break;
                case "None":
                    current = ExpressionBUS.Instance.RemoveFromExpressionList(current, criteriaExpression);
                    break;
            }

            return current;
        }

        public bool HandleAddBook(Book book, Grid productGrid)
        {
            if (AreTextBoxesFilled(productGrid) && book.Cover!=null && book.CategoryId!=null)
            {
                string imagePath = SaveImageToFolder(book.Cover, "Resources/BookCovers");
                book.Cover = imagePath;
                BookDAO.Instance.AddBook(book);
                return  true;
            }
            else
            {
                return false;
            }
        }

        public bool HandleUpdateBook(Book book, Grid productGrid, string clientImagePath, Category selectedCategory)
        {
            if (AreTextBoxesFilled(productGrid))
            {

                if(clientImagePath!=null)
                {
                    string imagePath = SaveImageToFolder(clientImagePath, "Resources/BookCovers");
                    string oldPath = book.Cover;
                    book.Cover = imagePath;

                    try
                    {
                        if (File.Exists(oldPath))
                        {
                            File.Delete(oldPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa ảnh: " + ex.Message);
                    }

                }
                book.CategoryId = selectedCategory.Id;
                BookDAO.Instance.UpdateBook(book);

                return true;


            }
            else
            {
                return false;

            }

        }

        private bool AreTextBoxesFilled(Grid grid)
        {
            foreach (var child in grid.Children)
            {
                if (child is System.Windows.Controls.TextBox textBox)
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string? SaveImageToFolder(string sourceImagePath, string destinationFolder)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(sourceImagePath);
                string destinationPath = System.IO.Path.Combine(destinationFolder, fileName);

                if (System.IO.File.Exists(destinationPath))
                {
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    string fileExtension = System.IO.Path.GetExtension(fileName);
                    int counter = 1;

                    do
                    {
                        string newFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                        destinationPath = System.IO.Path.Combine(destinationFolder, newFileName);
                        counter++;
                    }
                    while (System.IO.File.Exists(destinationPath));
                }

                System.IO.File.Copy(sourceImagePath, destinationPath, true);
                return destinationPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu ảnh: " + ex.Message);
                return null;
            }
        }

        public string HandlePickImage(Image coverImage)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.webp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.webp";
            
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                var bitmap = new BitmapImage(
                    new Uri($"{imagePath}", UriKind.Absolute));
                coverImage.Source = bitmap;

                return imagePath;

                //_book.Cover = imagePath;
            }

            return null;
        }

    }
}
