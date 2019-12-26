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
            ViewBag.Account = AccountAction.GetAll();
            ViewBag.Book = ReadOnline.getAllBookChapter(id);
            return View();
        }

        public ActionResult UpdateChapter(int id)
        {
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
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
        public ActionResult UpdateChapter(int id, string bookName, int idChapter, string chapter, HttpPostedFileBase[] file)
        {
            for (int i = 0; i < file.Length; i++)
            {
                string _path = "";
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
                else if (file[i] == null)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Hãy chọn file để người dùng đọc</h3>";
                    ViewBag.Account = AccountAction.GetAll();
                    return View();
                }
                else
                {
                    string _FileName = Path.GetFileName(file[i].FileName);
                    _path = Path.Combine(Server.MapPath("/UploadedFiles"), _FileName);
                    file[i].SaveAs(_path);
                    ReadOnline.updateChapter(id, bookName, idChapter, chapter, "/UploadedFiles/" + _FileName);
                }
            }
            return Redirect("~/Admin/BookList");
        }

        public ActionResult ReadChapter([Bind(Include = "idBook, idChapter")]ReadOnline readOnline)
        {
            ViewBag.Account = AccountAction.GetAll();
            int idBook = readOnline.idBook;
            int idChapter = readOnline.idChapter;
            ViewBag.Book = ReadOnline.getChapterFile(idBook, idChapter);
            return View();
        }
    }
}