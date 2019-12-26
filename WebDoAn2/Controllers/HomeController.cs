using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using WebDoAn2.Models;

namespace WebDoAn2.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewBag.Alert = TempData["Alert"].ToString();
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.CountBook = BookAction.CountBook();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            ViewBag.CountAuthor = AuthorAction.CountAuthor();
            var randomBooks = db.Books.OrderBy(x => Guid.NewGuid()).Take(6);
            ViewBag.Book = randomBooks;
            ViewBag.BookNew = BookAction.GetAll();
            if (Session["user"] != null)
            {
                ViewBag.Borrow = Borrow_Book.GetYour(Session["user"].ToString());
            }
            return View();
        }

        public ActionResult Search(string searchString)
        {
            ViewBag.Account = AccountAction.GetAll();
            db.SaveChanges();
            if (searchString != "")
            {
                var links = from l in db.Books // lấy toàn bộ liên kết
                            where l.bookName.Contains(searchString) || l.author.Contains(searchString)
                            select l;
                List<Book> chapter_list = links.ToList();
                ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
                ViewBag.Caterogy = CaterogyAction.GetAll();
                ViewBag.Book = chapter_list;
                return View();
            }
            else
            {
                var link = from l in db.Books
                           select l;
                List<Book> chapter_list = link.ToList();
                ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
                ViewBag.Caterogy = CaterogyAction.GetAll();
                ViewBag.Book = chapter_list;
                return View();
            }
        }

        public ActionResult BookList()
        {
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = BookAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            return View();
        }

        public ActionResult CaterogiesList()
        {
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            return View();
        }

        public ActionResult CaterogyBook(int id)
        {
            var links = from l in db.Book_Caterogies // lấy toàn bộ liên kết
                        where l.idCaterogy.Equals(id)
                        select l;
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = BookAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            ViewBag.caterogyId = links;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ViewBook(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = BookAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            var randomBooks = db.Books.OrderBy(x => Guid.NewGuid()).Take(4);
            ViewBag.randomBooks = randomBooks;
            var book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
    }
}