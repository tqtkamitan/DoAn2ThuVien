using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDoAn2.Models
{
    public class AuthorAction
    {
        public static void Create(string author)
        {
            using (var db = new DataContext())
            {
                db.Authors.Add(new Author { author = author });
                db.SaveChanges();
                db.Dispose();
            }
        }

        public static List<Author> GetAll()
        {
            List<Author> list_author = null;
            using (var db = new DataContext())
            {
                list_author = db.Authors.ToList();
                db.Dispose();
            }
            return list_author;
        }

        public static int CountAuthor()
        {
            List<Author> list_auhtor = null;
            using (var db = new DataContext())
            {
                list_auhtor = db.Authors.ToList();
                db.Dispose();
            }
            return list_auhtor.Count();
        }

        public static void EditAuthor(int id, string authorName)
        {
            using (var db = new DataContext())
            {
                var author = db.Authors.Find(id);
                author.author = authorName;
                db.Entry(author).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
        }
    }
}