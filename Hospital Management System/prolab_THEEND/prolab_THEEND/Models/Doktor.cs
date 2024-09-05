using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    [Table("Doktor")]
    public class Doktor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoktorID { get; set; }
        [Required(ErrorMessage = "Şifre bilgisi gereklidir")]
        public string Şifre { get; set; }
        [Required(ErrorMessage = "Ad bilgisi gereklidir")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyad bilgisi gereklidir")]
        public string SoyAd { get; set; }
        [Required(ErrorMessage = "UzmanlıkAlanı bilgisi gereklidir")]
        public string UzmanlıkAlanı { get; set; }
        [Required(ErrorMessage = "Hastane bilgisi gereklidir")]
        public string Hastane { get; set; }
        public List<Hasta> HastaList { get; set;}
    }
}