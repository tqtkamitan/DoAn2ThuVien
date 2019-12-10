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
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                Borrow_Book cardItem = giohang.FirstOrDefault(m => m.idborrow == SanPhamID);
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
    }
}