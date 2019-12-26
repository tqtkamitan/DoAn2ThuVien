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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idReadOnline { get; set; }
        [Key, Column(Order = 1)]
        public int idBook { get; set; }
        public string book { get; set; }
        public int idChapter { get; set; }
        public string chapterName { get; set; }
        public string fileChapter { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dateAdded { get; set; }

        public static void updateChapter(int id, string bookName, int idChapter, string chapter, string file)
        {
            using (var db = new DataContext())
            {
                var book = db.Books.Find(id);
                DateTime now = DateTime.Now;
                db.ReadOnlines.Add(new ReadOnline
                {
                    idBook = id,
                    book = bookName,
                    idChapter = idChapter,
                    chapterName = chapter,
                    fileChapter = file,
                    dateAdded = now,
                });
                db.SaveChanges();
                db.Dispose();
            }
        }

        public static List<ReadOnline> getAllBookChapter(int id)
        {
            using (var db = new DataContext())
            {
                var links = from l in db.ReadOnlines // lấy toàn bộ liên kết
                            where l.idBook.Equals(id)
                            select l;
                List<ReadOnline> chapter_list = links.ToList();
                return chapter_list;
            }
        }

        public static List<ReadOnline> getChapterFile(int id, int idChapter)
        {
            using (var db = new DataContext())
            {
                var links = from l in db.ReadOnlines // lấy toàn bộ liên kết
                            where l.idBook.Equals(id)
                            where l.idChapter.Equals(idChapter)
                            select l;
                List<ReadOnline> chapter_list = links.ToList();
                return chapter_list;
            }
        }
    }
}