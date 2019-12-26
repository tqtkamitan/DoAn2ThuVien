using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebDoAn2.Models
{
    public class Borrow_Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idborrow { get; set; }
        [Required]
        public string bookName { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime until { get; set; }
        public string user { get; set; }
        public string borrow { get; set; }

        public static List<Borrow_Book> GetAll()
        {
            List<Borrow_Book> list_pay = null;
            using (var db = new DataContext())
            {
                list_pay = db.Borrow_Books.ToList();
                db.Dispose();
            }
            list_pay.Reverse();
            return list_pay;
        }

        public static List<Borrow_Book> GetYour(string user)
        {
            using (var db = new DataContext())
            {
                var links = from l in db.Borrow_Books // lấy toàn bộ liên kết
                            where l.user.Equals(user)
                            select l;
                List<Borrow_Book> chapter_list = links.ToList();
                return chapter_list;
            }
        }
    }
}