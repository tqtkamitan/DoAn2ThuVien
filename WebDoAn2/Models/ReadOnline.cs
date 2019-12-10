using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WebDoAn2.Models
{
    public class ReadOnline
    {
        [Key, Column(Order = 0)]
        public int idBook { get; set; }
        public string book { get; set; }
        [Key, Column(Order = 1)]
        public int idChapter { get; set; }
        public string fileChapter { get; set; }

        public static void updateChapter(int id, string chapter, string file)
        {
            using (var db = new DataContext())
            {
                var book = db.Books.Find(id);
                db.ReadOnlines.Add(new ReadOnline
                {

                });
            }
        }
    }
}