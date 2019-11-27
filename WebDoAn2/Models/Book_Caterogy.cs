using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDoAn2.Models
{
    public class Book_Caterogy
    {
        [Key, Column(Order = 0)]
        public int idBook { get; set; }
        public string book { get; set; }
        [Key, Column(Order = 1)]
        public int idCaterogy { get; set; }
        public string caterogy { get; set; }
    }
}