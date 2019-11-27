using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace WebDoAn2.Models
{
    public class BookAction
    {
        public static void Create(string bookName, string description, string img, string author, string[] caterogies, DateTime dateAdded, string status)
        {
            using (var db = new DataContext())
            {
                Book book = new Book()
                {
                    bookName = bookName,
                    author = author,
                    description = description,
                    img = img,
                    dateAdded = dateAdded.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    status = "Mới",
                };
                db.Books.Add(book);
                db.SaveChanges();

                foreach (var idCaterogy in caterogies)
                {
                    var caterogy = db.Caterogies.Find(Int32.Parse(idCaterogy));
                    db.Book_Caterogies.Add(new Book_Caterogy
                    {
                        idBook = book.idBook,
                        book = bookName,
                        idCaterogy = Int32.Parse(idCaterogy),
                        caterogy = caterogy.caterogy,
                    });
                }
                db.SaveChanges();
                db.Dispose();
            }
        }

        public static List<Book> GetAll()
        {
            List<Book> list_product = null;
            using (var db = new DataContext())
            {
                list_product = db.Books.ToList();
                db.Dispose();
            }
            return list_product;
        }

        public static List<Book> SachGioiThieu()
        {
            List<Book> list_product = null;
            Random rnd = new Random();
            int[] random = Enumerable
                .Repeat<int>(0, 4)
                .Select((x, i) => new { i = i, rand = rnd.Next() })
                .OrderBy(x => x.rand)
                .Select(x => x.i)
                .ToArray();
            using (var db = new DataContext())
            {
                var links = from l in db.Books
                                from r in random// lấy toàn bộ liên kết
                                where l.idBook.Equals(r)
                                select l;
                IQueryable<Book> q = links;
                list_product = q.ToList();
            }

            return list_product;
        }

        public static int CountBook()
        {
            List<Book> list_product = null;
            using (var db = new DataContext())
            {
                list_product = db.Books.ToList();
                db.Dispose();
            }
            return list_product.Count();
        }

        public static void EditBook(int id, string bookName, string description, string img, string author, string[] caterogies, DateTime dateAdded, string status)
        {
            using (var db = new DataContext())
            {
                var book = db.Books.Find(id);
                book.bookName = bookName;
                book.author = author;
                book.description = description;
                book.img = img;
                book.dateAdded = dateAdded.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                book.status = "Mới";
                db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                bool hadUpdated = false;
                foreach (var idCaterogy in caterogies)
                {
                    var caterogy = db.Caterogies.Find(Int32.Parse(idCaterogy));
                    if (hadUpdated == false)
                    {
                        db.Book_Caterogies.RemoveRange(db.Book_Caterogies.Where(x => x.idBook == id));
                        db.SaveChanges();
                    }
                    db.Book_Caterogies.Add(new Book_Caterogy
                    {
                        idBook = book.idBook,
                        book = bookName,
                        idCaterogy = Int32.Parse(idCaterogy),
                        caterogy = caterogy.caterogy,
                    });
                    db.SaveChanges();
                    hadUpdated = true;
                }
                
                db.Dispose();
            }
        }
    }
}