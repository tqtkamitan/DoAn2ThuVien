using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn2.Models
{
    public class Account
    {
        [Required]
        public string name { get; set; }
        [Key]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string address { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        public string img { get; set; }
        [Required]
        public string role { get; set; }
        [Required]
        public string status { get; set; }
    }
}