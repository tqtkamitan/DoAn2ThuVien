using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAn2.Models;
using System.IO;
using System.Net;

namespace WebDoAn2.Controllers
{
    public class ReadOnlineController : Controller
    {
        private DataContext db = new DataContext();
        // GET: ReadOnline
        public ActionResult Index(int id)
        {
            return View();
        }

        public ActionResult UpdateChapter(int id)
        {
            if (Session["staff"] == null)
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.BookId = id;
            var book = db.Books.Find(id);
            ViewBag.Book = book;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateChapter(int id, string chapter, HttpPostedFileBase file)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Account = AccountAction.GetAll();
            if (chapter == "")
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy nhập tên chương</h3>";
                ViewBag.Account = AccountAction.GetAll();
                return View();
            }
            else if (file == null)
            {
                ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn file để người dùng đọc</h3>";
                ViewBag.Account = AccountAction.GetAll();
                return View();
            }
            return View();
        }

        public ActionResult ReadChapter(int id)
        {
            return View();
        }
    }
}