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
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = BookAction.GetAll();
            ViewBag.CountBook = BookAction.CountBook();
            ViewBag.CountAuthor = AuthorAction.CountAuthor();
            return View();
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
            ViewBag.Book = BookAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
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
            var book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
    }
}