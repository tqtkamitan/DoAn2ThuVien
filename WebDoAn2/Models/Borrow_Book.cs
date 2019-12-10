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
        public int idborrow { get; set; }
        [Required]
        public string bookName { get; set; }
        [Required]
        public DateTime until { get; set; }
        public string user { get; set; }
    }
}