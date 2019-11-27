using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace WebDoAn2.Models
{
    public class Book_CaterogyAction
    {
        public static List<Book_Caterogy> GetAll()
        {
            List<Book_Caterogy> list_product = null;
            using (var db = new DataContext())
            {
                list_product = db.Book_Caterogies.ToList();
                db.Dispose();
            }
            return list_product;
        }
    }
}