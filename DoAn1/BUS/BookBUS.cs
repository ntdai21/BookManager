using DoAn1.BUS;
using DoAn1.DAO;
using DoAn1.UI.Windows;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DoAn1.BUS
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

        public (BindingList<Book>, int) LoadBook(BindingList<Book>? books, int page, int pageSize,
            Category? category,
            double minPrice, double maxPrice,
            string searchTerm,
            List<(Expression<Func<Book, object>>, bool)> orderByExpressions)
        {
            if (books == null)
            {
                books = new BindingList<Book>();
            }

            (List<Book> bookList, int totalPage) = BookDAO.Instance.GetBooksPaginatedSorted(page, pageSize, category, minPrice, maxPrice, searchTerm, orderByExpressions.ToArray());
            books.Clear();

            foreach (Book book in bookList)
            {
                books.Add(book);
            }

            return (books, totalPage);

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
            if (AreTextBoxesFilled(productGrid) && book.Cover != null && book.CategoryId != null)
            {
                string imagePath = SaveImageToFolder(book.Cover, "Resources/BookCovers");
                book.Cover = imagePath;
                BookDAO.Instance.AddBook(book);
                return true;
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

                if (clientImagePath != null)
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
                MessageBox.Show("Error while saving the image: " + ex.Message);
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

        public bool importFromExcel(string filename, string directory, int catRow, int bookRow)
        {
            var document = SpreadsheetDocument.Open(filename, false);
            Hashtable refTable = new Hashtable();

            var wbPart = document.WorkbookPart!;
            var sheets = wbPart.Workbook.Descendants<Sheet>()!;

            var categorySheet = sheets.FirstOrDefault(s => s.Name == "Category");
            var categoryWsPart = (WorksheetPart)(wbPart!.GetPartById(categorySheet.Id!));
            var categoryCells = categoryWsPart.Worksheet.Descendants<Cell>();

            int row = catRow;
            Cell nameCell = categoryCells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
            Cell idCell = categoryCells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;

            while (nameCell != null)
            {
                string stringId = nameCell!.InnerText;
                var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;
                string name = stringTable.SharedStringTable.ElementAt(int.Parse(stringId)).
                InnerText;

                int oldId = int.Parse(idCell!.InnerText);

                Category newCategory = new Category();
                newCategory.Name = name;

                int newId = CategoryDAO.Instance.AddCategory(newCategory);

                refTable.Add(oldId, newId);

                row++;
                nameCell = categoryCells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
                idCell = categoryCells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;
            }

            var bookSheet = sheets.FirstOrDefault(s => s.Name == "Book");
            var bookWsPart = (WorksheetPart)(wbPart!.GetPartById(bookSheet.Id!));
            var bookCells = bookWsPart.Worksheet.Descendants<Cell>();

            var bookColumnInfo = new List<(string columnName, string columnIndex, string type)>()
            {
                ("Name","B","string"),
                ("Price","C","double"),
                ("NumOfPage","D","int"),
                ("PublishingCompany","E","string"),
                ("Author","F","string"),
                ("Cover","G","string"),
                ("CostPrice","H","double"),
                ("Description","I","string"),
                ("CategoryId","J","int"),
                ("Quantity","K","int")
            };

            row = bookRow;
            Cell testCell = bookCells.FirstOrDefault(b => b?.CellReference == $"B{row}")!;

            while (testCell != null)
            {
                Book newBook = new Book();
                foreach (var item in bookColumnInfo)
                {

                    Cell cell = bookCells.FirstOrDefault(b => b?.CellReference == $"{item.columnIndex}{row}");

                    if (cell == null)
                    {
                        MessageBox.Show("Invalid data in excel file");
                        return false;
                    }

                    string stringId = cell!.InnerText;
                    string rawData = stringId;
                    bool isSharedString = cell.DataType != null && cell.DataType.Value == CellValues.SharedString;

                    if (isSharedString == true)
                    {
                        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;
                        rawData = stringTable.SharedStringTable.ElementAt(int.Parse(stringId)).InnerText;
                    }

                    PropertyInfo propertyInfo = typeof(Book).GetProperty(item.columnName);

                    if (propertyInfo != null)
                    {
                        switch (item.type)
                        {
                            case "double":
                                propertyInfo.SetValue(newBook, double.Parse(rawData));
                                break;
                            case "string":
                                if (item.columnName == "Cover")
                                {
                                    int index = rawData.LastIndexOf('/');
                                    string name = rawData.Substring(index + 1);

                                    string fullPath = directory + rawData;
                                    rawData = SaveImageToFolder(fullPath, "Resources/BookCovers");



                                }
                                propertyInfo.SetValue(newBook, (string)rawData);
                                break;
                            case "int":
                                propertyInfo.SetValue(newBook, int.Parse(rawData));
                                break;
                        }
                    }
                }

                newBook.CategoryId = (int?)refTable[newBook.CategoryId];
                BookDAO.Instance.AddBook(newBook);
                row++;
                testCell = bookCells.FirstOrDefault(b => b?.CellReference == $"B{row}")!;
            }

            return true;
        }

        public Book? FindBookById(int id)
        {
            return BookDAO.Instance.FindBookById(id);
        }


        public bool DescreaseQuantity(int bookId, int numOfBook)
        {
            return BookDAO.Instance.DescreaseQuantity(bookId, numOfBook);
        }

        public bool IncreaseQuantity(int bookId, int numOfBook)
        {
            return BookDAO.Instance.IncreaseQuantity(bookId, numOfBook);
        }
    }
}
