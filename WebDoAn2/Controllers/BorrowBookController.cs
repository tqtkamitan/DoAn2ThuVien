using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAn2.Models;

namespace WebDoAn2.Controllers
{
    public class BorrowBookController : Controller
    {
        private DataContext db = new DataContext();
        // GET: BorrowBook
        public ActionResult Index()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            List<Borrow_Book> borrow = Session["borrow"] as List<Borrow_Book>;
            return View(borrow);
        }

        public RedirectToRouteResult AddToBorrowList(int SanPhamID)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["borrow"] == null)
            {
                Session["borrow"] = new List<Borrow_Book>();
            }

            List<Borrow_Book> giohang = Session["borrow"] as List<Borrow_Book>;

            if (giohang.FirstOrDefault(m => m.idborrow == SanPhamID) == null)
            {
                Book sp = db.Books.Find(SanPhamID);
                if (sp.status == "Hết sách" || sp.status == "")
                {
                    TempData["Alert"] = "Cuốn sách này hiện không thể mượn nữa!";
                    return RedirectToAction("Index", "Home");
                }
                string user = Session["user"].ToString();
                //
                Borrow_Book newItem = null;
                    newItem = new Borrow_Book()
                    {
                        idborrow = SanPhamID,
                        user = user,
                        bookName = sp.bookName,
                        until = DateTime.Now,
                    };

                giohang.Add(newItem);
            }
            else
            {
                TempData["Alert"] = "Bạn đã thêm cuốn sách này!";
                return RedirectToAction("Index", "Home");
                //Borrow_Book cardItem = giohang.FirstOrDefault(m => m.idborrow == SanPhamID);
            }
            return RedirectToAction("Index", "BorrowBook", new { id = SanPhamID });
        }

        public RedirectToRouteResult SuaNgayTra(int SanPhamID, DateTime ngayTraMoi)
        {
            // tìm carditem muon sua
            List<Borrow_Book> giohang = Session["borrow"] as List<Borrow_Book>;
            Borrow_Book itemSua = giohang.FirstOrDefault(m => m.idborrow == SanPhamID);
            if (itemSua != null)
            {
                itemSua.until = ngayTraMoi;
            }
            return RedirectToAction("Index");

        }

        public RedirectToRouteResult xoa(int SanPhamID)
        {
            List<Borrow_Book> giohang = Session["borrow"] as List<Borrow_Book>;
            Borrow_Book itemXoa = giohang.FirstOrDefault(m => m.idborrow == SanPhamID);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        public ActionResult MuonSach()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string user = Session["user"].ToString();
            List<libraryCard> check =  libraryCard.GetAll();
            int count = 0;
            for (int i = 0; i < check.Count; i++)
            {
                if (check[i].email == user)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                TempData["Alert"] = "Bạn cần phải làm thẻ thư viện trước khi mượn sách!";
                Session["borrow"] = null;
                return RedirectToAction("Index", "Home");
            }
            var products = Session["borrow"];
            List<Borrow_Book> giohang = Session["borrow"] as List<Borrow_Book>;
            Borrow_Book[] objects = giohang.ConvertAll<Borrow_Book>(item => (Borrow_Book)item).ToArray();
            DateTime now = DateTime.Now;
            for (int i = 0; i < objects.Length; i++)
            {
                Account account = db.Accounts.Find(objects[i].user);
                Borrow_Book paycheck = new Borrow_Book
                {
                    user = account.email,
                    bookName = objects[i].bookName,
                    until = objects[i].until,
                    borrow = "Chờ được mượn",
                };
                db.Borrow_Books.Add(paycheck);
            }
            Session["borrow"] = null;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SachDangMuon()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            ViewBag.HoaDon = Borrow_Book.GetAll();
            return View();
        }

        public ActionResult TraSach(int id)
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            Borrow_Book hoaDon = db.Borrow_Books.Find(id);
            hoaDon.borrow = "Đã trả";
            db.Entry(hoaDon).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Redirect("~/BorrowBook/TinhTrangMuonSach");
        }

        public ActionResult GiaoSach(int id)
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            Borrow_Book hoaDon = db.Borrow_Books.Find(id);
            hoaDon.borrow = "Đã giao";
            db.Entry(hoaDon).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Redirect("~/BorrowBook/TinhTrangMuonSach");
        }

        public ActionResult TinhTrangMuonSach()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.HoaDon = Borrow_Book.GetAll();
            return View();
        }

        public ActionResult LocSachChuaGiao()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            var links = from l in db.Borrow_Books // lấy toàn bộ liên kết
                        where l.borrow.Equals("Chờ được mượn") where l.until < DateTime.Now
                        select l;
            List<Borrow_Book> borrow_Books = links.ToList();
            ViewBag.HoaDon = borrow_Books;
            return View();
        }

        public ActionResult LocSachChuaTra()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            var links = from l in db.Borrow_Books // lấy toàn bộ liên kết
                        where l.borrow.Equals("Đã giao")
                        select l;
            List<Borrow_Book> borrow_Books = links.ToList();
            ViewBag.HoaDon = borrow_Books;
            return View();
        }

        public ActionResult LocSachDenHangChuaTra()
        {
            ViewBag.Account = AccountAction.GetAll();
            if (Session["user"] == null) return RedirectToAction("Login", "Account");
            Account account = db.Accounts.Find(Session["user"].ToString());
            if (account.role != "Nhân viên")
            {
                TempData["Alert"] = "Bạn không có quyền vào trang";
                return RedirectToAction("Index", "Home");
            }
            var links = from l in db.Borrow_Books // lấy toàn bộ liên kết
                        where l.borrow.Equals("Đã giao") where l.until <DateTime.Now
                        select l;
            List<Borrow_Book> borrow_Books = links.ToList();
            ViewBag.HoaDon = borrow_Books;
            return View();
        }
    }
}