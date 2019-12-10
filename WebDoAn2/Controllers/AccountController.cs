using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAn2.Models;

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
                db.Accounts.Add(new Account { email = "tqt.kamitan@gmail.com", name = "Quang Tân", password = "kirito1998", role = "Nhân viên", img = "/UploadedFiles/anonymous-profile.jpg", phoneNumber = "0984081735", status = "Active" });
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
                    //string AccStyle = user.status;
                    if (email.Equals(taikhoan) && password.Equals(matkhau))
                    {
                        Session.Add("user", user.email);
                        if (user.role == "Nhân viên")
                        {
                            Session.Add("staff", user.role);
                        }
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

        public ActionResult EmployeeList() {
            if (Session["staff"] == null)
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
            if (Session["staff"] == null)
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Account = AccountAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult EditAccount(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditAccount(int id, string name)
        {
            return View();
        }
    }
}