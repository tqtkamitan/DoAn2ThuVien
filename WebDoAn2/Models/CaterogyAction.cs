using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDoAn2.Models
{
    public class CaterogyAction
    {
        public static void Create(string caterory)
        {
            using (var db = new DataContext())
            {
                db.Caterogies.Add(new Caterogy { caterogy = caterory });
                db.SaveChanges();
                db.Dispose();
            }
        }

        public static List<Caterogy> GetAll()
        {
            List<Caterogy> list_caterory = null;
            using (var db = new DataContext())
            {
                list_caterory = db.Caterogies.ToList();
                db.Dispose();
            }
            return list_caterory;
        }
    }
}