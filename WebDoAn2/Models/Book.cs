using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn2.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idBook { get; set; }
        [Required]
        public string bookName { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string dateAdded { get; set; }
        public string img { get; set; }
        [Required]
        public string status { get; set; }
    }
}