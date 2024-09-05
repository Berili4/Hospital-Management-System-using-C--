using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    [Table("Yönetici")]
    public class Yönetici
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int YoneticiID { get; set; }
        [Required(ErrorMessage = "Ad bilgisi gereklidir")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyad bilgisi gereklidir")]
        public string SoyAd { get; set; }
        [Required(ErrorMessage = "Şifre bilgisi gereklidir")]
        public string Şifre { get; set; }

    }
}