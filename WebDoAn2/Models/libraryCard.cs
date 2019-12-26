using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn2.Models
{
    public class libraryCard
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int libraryCardId { get; set; }
        [Required]
        public string libraryCardName { get; set; }
        [Key, Column(Order = 1)]
        public string email { get; set; }
        public string name { get; set; }
        public bool Active { get; set; }

        public static List<libraryCard> GetAll()
        {
            List<libraryCard> list_caterory = null;
            using (var db = new DataContext())
            {
                list_caterory = db.libraryCards.ToList();
                db.Dispose();
            }
            return list_caterory;
        }
    }
}