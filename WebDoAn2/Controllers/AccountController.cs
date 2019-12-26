using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAn2.Models;
using System.Net;
using System.IO;

namespace WebDoAn2.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();

        // GET: User
        public ActionResult Login([Bind(Include = "email, password")]Account systemUser)
        {
            if (db.Accounts.Find("tqt.kamitan@gmail.com") == null)
            {
                db.libraryCards.Add(new libraryCard { libraryCardName = "1101", email = "tqt.kamitan@gmail.com" , name = "Quang Tân", Active = true });
                db.Accounts.Add(new Account { email = "tqt.kamitan@gmail.com", name = "Quang Tân", password = "kirito1998", role = "Nhân viên", img = "/UploadedFiles/anonymous-profile.jpg", phoneNumber = "0984081735", status = true, libraryCardId = 1});
                db.SaveChanges();
            }
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] != null) return RedirectToAction("Index", "Home");
            string email = systemUser.email;
            string password = systemUser.password;

            using (var db = new DataContext())
            {
                Account user = db.Accounts.Find(email);
                if (user != null)
                {
                    string taikhoan = user.email;
                    string matkhau = user.password;
                    if (email.Equals(taikhoan) && password.Equals(matkhau))
                    {
                        Session.Add("user", user.email);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Noti = "<h3 class='text-danger'>Sai thông tin đăng nhập</h3>";
                        return View();
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] != null) return RedirectToAction("Index", "Home");
            ViewBag.Accounts = AccountAction.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Register(string name, string phoneNumber, string address, string email, string password, string password2)
        {
            if (Session["user"] != null) return RedirectToAction("Index", "Home");
            using (var db = new DataContext())
            {
                Account user = db.Accounts.Find(email);
                if (user != null)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Email này đã được đăng kí</h3>";
                    return View("Register");
                }
                if (password != password2)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Password nhập lại phải trùng Password nhập ban đầu</h3>";
                    return View("Register");
                }
            }
            string img = "/UploadedFiles/anonymous-profile.jpg";
            AccountAction.Create(name, phoneNumber, address, email, password, img);
            Session.Add("user", email);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LamTheThuVien()
        {
            return View();
        }

        public ActionResult EmployeeList() {
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Employee = AccountAction.GetAllEmployee();
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        public ActionResult AccountList()
        {
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        public ActionResult UserDetail()
        {
            if (Session["user"] == null) return RedirectToAction("Index", "Home");
            Account account = db.Accounts.Find(Session["user"].ToString());
            ViewBag.User = account;
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult EditUserInfo()
        {
            if (Session["user"] == null) return RedirectToAction("Index", "Home");
            Account account = db.Accounts.Find(Session["user"].ToString());
            ViewBag.User = account;
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult EditUserInfo(string name, string phoneNumber, string address, string email, string password, string password2, HttpPostedFileBase img)
        {
            if (Session["user"] == null) return RedirectToAction("Index", "Home");
            using (var db = new DataContext())
            {
                Account user = db.Accounts.Find(email);
                if (user != null)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Email này đã được đăng kí</h3>";
                    return View();
                }
                if (!email.Contains("@"))
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Xin hãy nhập email</h3>";
                    return View();
                }
                if (password != password2)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Password nhập lại phải trùng Password nhập ban đầu</h3>";
                    return View();
                }
                if (password.Length <= 8)
                {
                    ViewBag.Noti = "<h3 class='text-danger'>Password phải từ 8 ký tự trở lên</h3>";
                    return View();
                }
            }
            Account account = db.Accounts.Find(Session["user"].ToString());
            string _path = "";
            if (img == null)
            {
                var book = db.Accounts.Find(email);
                string _FileName = book.img;
                AccountAction.EditAccount(email, name, phoneNumber, address, _FileName, password);
            }
            else
            {
                string _FileName = Path.GetFileName(img.FileName);
                _path = Path.Combine(Server.MapPath("/UploadedFiles"), _FileName);
                img.SaveAs(_path);
                AccountAction.EditAccount(email, name, phoneNumber, address, "/UploadedFiles/" + _FileName, password);
            }
            return View();
        }
    }
}