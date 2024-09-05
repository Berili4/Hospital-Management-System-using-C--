using prolab_THEEND.Commands;
using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace prolab_THEEND.Controllers
{
    public class HastaPanelController : Controller
    {
        // GET: HastaPanel
        public HastaCommand hastaCommand = HomeController.hastaModifier;
        public RandevuCommand randevuCommand = HomeController.randevuModifier;
        public DoktorCommand doktorCommand = HomeController.doktorModifier;
        public IslemCommand islemCommand = HomeController.islemModifier;

        private CombinedModels currentHasta;
        public ActionResult Index()
        {
            currentHasta = Session["HastaLogin"] as CombinedModels;

            return View();
        }
        public ActionResult RandevuPanel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RandevuPanel(int randevuId)
        {
            currentHasta = Session["HastaLogin"] as CombinedModels;

            if (randevuCommand.Delete(new Randevu {RandevuId = randevuId }) != 0)
            {
                hastaCommand.AddListToAppointment(currentHasta.Hasta);
                Session["HastaLogin"] = currentHasta;
            }
            return View();
        }
        public ActionResult RandevuOlustur()
        {
            CombinedModels instance = new CombinedModels();
            instance.Doktor = new Doktor();
            return View(instance);
        }
        [HttpPost]
        public ActionResult RandevuOlustur(int doktor , DateTime randevuTarihi)  // parametre adı Html.Hidden'daki ile aynı olmalı :DDDDDD kafayı yicem az kaldı
        {
            currentHasta = Session["HastaLogin"] as CombinedModels;

            if (doktor >= 0)
            {
                Islem yeniIslem = new Islem();  
                Randevu randevu = new Randevu();

                randevu.Hasta = currentHasta.Hasta;
                randevu.Doktor = doktorCommand.FindDoctorById(doktor);
                randevu.RandevuTarih = randevuTarihi;
                randevuCommand.Insert(randevu);

                string sql = "select * from Randevu where @Id = Hasta_HastaId AND @Tarih = RandevuTarih AND @DoktorId = Doktor_DoktorId";
                string[] parameters = {"@Id","@DoktorId", "@Tarih" };
                string[] parameterValues = { currentHasta.Hasta.HastaId.ToString() , doktor.ToString() };
                List<Randevu> randevular = randevuCommand.WhereGetAll(sql, parameters, parameterValues,randevuTarihi);
                int randevuId = randevular[0].RandevuId;

                yeniIslem.HastaId = currentHasta.Hasta.HastaId;
                yeniIslem.RandevuId = randevuId;
                yeniIslem.DoktorId = doktor;
                int durum = 0;
                try
                {
                    durum = islemCommand.Insert(yeniIslem);
                }
                catch (Exception ex)
                {
                    if (durum == 0)
                    {
                        TempData["GuncellemeMesaj"] = "İşlem geçersiz! Tekrar deneyin.";
                        TempData["GuncellemeMesaj_Durum"] = "Red";
                    }
                }
                finally
                {
                    if (durum == 0)
                    {
                        TempData["GuncellemeMesaj"] = "İşlem geçersiz! Tekrar deneyin.";
                        TempData["GuncellemeMesaj_Durum"] = "Red";
                    }
                    else if (durum != 0)
                    {
                        TempData["GuncellemeMesaj"] = "İşlem başarıyla gerçekleştirildi!";
                        TempData["GuncellemeMesaj_Durum"] = "Onay";
                    }
                }
                hastaCommand.AddListToAppointment(currentHasta.Hasta);

            }
            return View();
        }
        [HttpPost]
        public ActionResult RandevuOlustur_Sec(string selectedBolum)
        {
            string sql = "select * from Doktor where @Alan = UzmanlıkAlanı";
            string[] parameters = { "@Alan" };
            string[] parametersVal = { selectedBolum };

            IEnumerable<Doktor> filteredDoktors = HomeController.doktorModifier.WhereGetAll(sql, parameters, parametersVal);

            return Json(filteredDoktors);
        }

        public ActionResult TibbiRaporGoruntuleHas()
        {
            CombinedModels combinedModels = Session["HastaLogin"] as CombinedModels;

            if (combinedModels.Hasta.BildirimSayısı != 0)
            {
                var hasta = hastaCommand.FindHastaWithTC(combinedModels.Hasta.TcKimlikNo);
                hasta.BildirimSayısı = 0;
                hastaCommand.Update(hasta);
                combinedModels.Hasta.BildirimSayısı = 0;
                Session["HastaLogin"] = combinedModels;
            }
            return View();
        }
        [HttpPost]
        public ActionResult DosyaIndir(string url)
        {
            try
            {
                string fileName = Path.GetFileName(new Uri(url).LocalPath);
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string downloadPath = Path.Combine(desktopPath, "Prolab", fileName);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, downloadPath);
                }

                return Json(new { success = true, message = "Dosya başarıyla indirildi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Dosya indirme sırasında bir hata oluştu: " + ex.Message });
            }
        }
    }
}