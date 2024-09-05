using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    [Table("Randevu")]
    public class Randevu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RandevuId { get; set; }  
        public DateTime RandevuTarih { get; set; }
        public Hasta Hasta { get; set; }
        public Doktor Doktor { get; set; }


    }
}