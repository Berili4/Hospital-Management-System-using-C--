using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    [Table("Rapor")]
    public class Rapor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaporID { get; set; }
        public DateTime Tarih { get; set; }
        public string İçerik { get; set; }
        public Hasta Hasta { get; set; }
        public Doktor Doktor { get; set; }
        public Islem Islem { get; set; }
        public string URL { get; set; }
    }
}