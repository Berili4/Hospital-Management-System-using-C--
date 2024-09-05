using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    [Table("Hasta")]
    public class Hasta
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HastaId { get; set; }
        [Required(ErrorMessage = "Kimlik numarası gereklidir")]
        [Index(IsUnique = true)]  
        [StringLength(11)]  
        public string TcKimlikNo { get; set; }
        [Required(ErrorMessage = "Şifre bilgisi gereklidir")]
        public string Şifre { get; set; }
        [Required(ErrorMessage = "Ad bilgisi gereklidir.")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyad bilgisi gereklidir.")]
        public string SoyAd { get; set; }
        [Required(ErrorMessage = "Doğum tarihi bilgisi gereklidir.")]
        public DateTime DogumTarihi { get; set; }
        [Required(ErrorMessage = "Cinsiyet bilgisi gereklidir.")]
        public string Cinsiyet { get; set; }
        [Required(ErrorMessage = "Telefon numara bilgisi gereklidir.")]
        public string TelefonNo { get; set; }
        [Required(ErrorMessage = "Adres bilgisi gereklidir.")]
        public string Adres { get; set; }
        public virtual List<Doktor> SorumluDoktor { get; set; }
        public virtual List<Randevu> Randevular { get; set; }
        public int BildirimSayısı { get; set; }
    }
}