using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAn2.Models;
using System.Data.Entity;
using System.IO;
using System.Net;


namespace WebDoAn2.Controllers
{
    public class AdminController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Admin
        public ActionResult AdminSite()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        public ActionResult BookList()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = BookAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.Caterogy = CaterogyAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Caterory = CaterogyAction.GetAll();
            ViewBag.Author = AuthorAction.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(string bookName, string description, string author, string[] caterogy, DateTime dateAdded, string status, HttpPostedFileBase img)
        {
            string _path = "";
            if (bookName == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy nhập tên sách</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if (author == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn tên tác giả</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if (caterogy == null)
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn tên thể loại sách</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if (img.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(img.FileName);
                _path = Path.Combine(Server.MapPath("/UploadedFiles"), _FileName);
                img.SaveAs(_path);
                BookAction.Create(bookName, description, "/UploadedFiles/" + _FileName, author, caterogy, dateAdded, status);
            }
            else
            {
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn một hình ảnh cho sách</h3>";
                return View();
            }
            return Redirect("~/Admin/BookList");
        }

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Caterory = CaterogyAction.GetAll();
            ViewBag.Author = AuthorAction.GetAll();
            ViewBag.BookCaterogy = Book_CaterogyAction.GetAll();
            ViewBag.BookId = id;
            var book = db.Books.Find(id);
            ViewBag.Book = book;
            return View();
        }

        [HttpPost]
        public ActionResult EditBook(int id, string bookName, string description, string author, string[] caterogy, DateTime dateAdded, string status, HttpPostedFileBase img)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Caterory = CaterogyAction.GetAll();
            ViewBag.Author = AuthorAction.GetAll();
            string _path = "";
            if (bookName == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy nhập tên sách</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if (author == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn tên tác giả</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if (caterogy == null)
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn tên thể loại sách</h3>";
                ViewBag.Account = AccountAction.GetAll();
                ViewBag.Caterory = CaterogyAction.GetAll();
                ViewBag.Author = AuthorAction.GetAll();
                return View();
            }
            if ((img == null))
            {
                ViewBag.BookId = id;
                var book = db.Books.Find(id);
                string _FileName = book.img;
                BookAction.EditBook(id, bookName, description, _FileName, author, caterogy, dateAdded, status);
            }
            else
            {
                string _FileName = Path.GetFileName(img.FileName);
                _path = Path.Combine(Server.MapPath("/UploadedFiles"), _FileName);
                img.SaveAs(_path);
                BookAction.EditBook(id, bookName, description, "/UploadedFiles/" + _FileName, author, caterogy, dateAdded, status);
            }
            return Redirect("~/Admin/BookList");
        }

        public ActionResult AuthorList()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Author = AuthorAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult AddAuthor()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(string Author)
        {
            if (Author == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy nhập tên tác giả</h3>"; ViewBag.Account = AccountAction.GetAll();
                return View();
            }
            AuthorAction.Create(Author);
            return Redirect("~/Admin/AuthorList");
        }

        [HttpGet]
        public ActionResult EditAuthor(int id)
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.AuthorId = id;
            return View();
        }

        [HttpPost]
        public ActionResult EditAuthor(int id, string authorName)
        {
            ViewBag.Account = AccountAction.GetAll();
            AuthorAction.EditAuthor(id, authorName);
            return Redirect("~/Admin/AuthorList");
        }

        [HttpGet]
        public ActionResult CateroryList()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Caterory = CaterogyAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult AddCaterory()
        {
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult AddCaterory(string caterory)
        {
            if (caterory == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy nhập thể loại mới vào</h3>"; ViewBag.Account = AccountAction.GetAll();
                return View();
            }
            CaterogyAction.Create(caterory);
            return Redirect("~/Admin/CateroryList");
        }

        [HttpGet]
        public ActionResult EditCaterogy(int id)
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditCaterogy()
        {
            return Redirect("~/Admin/CateroryList");
        }

        public ActionResult DangKyTheThuVien()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            List<libraryCard> check = libraryCard.GetAll();
            for (int i = 0; i < check.Count; i++)
            {
                if (check[i].email == account.email)
                {
                    TempData["Alert"] = "Bạn đã có thẻ thư viện";
                    return RedirectToAction("Index", "Home");
                }
            }
            libraryCard paycheck = new libraryCard
            {
                email = account.email,
                name = account.name,
                libraryCardName = account.phoneNumber.Substring(account.phoneNumber.Length -3) + account.name.Substring(account.name.Length - 3),
                Active = false,
            };
            db.libraryCards.Add(paycheck);
            db.SaveChanges();
            account.libraryCardId = paycheck.libraryCardId;
            db.Entry(account).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View();
        }

        public ActionResult TheThuVienList()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LibraryCard = libraryCard.GetAll();
            return View();
        }

        public ActionResult TheThuVienCanActive()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LibraryCard = libraryCard.GetAll();
            return View();
        }

        public ActionResult KichHoatTheThuVien(int id)
        {
            libraryCard libraryCard = db.libraryCards.SingleOrDefault(m => m.libraryCardId == id);
            libraryCard.Active = true;
            db.Entry(libraryCard).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TheThuVienList", "Admin");
        }
    }
}