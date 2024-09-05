using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using prolab_THEEND.Commands;
using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace prolab_THEEND.Controllers
{
    public class DoktorPanelController : Controller
    {
        public DoktorCommand doktorCommand = HomeController.doktorModifier;
        public IslemCommand IslemCommand = HomeController.islemModifier;
        public RaporCommand raporCommand = HomeController.raporModifier;
        public static CombinedModels model = HomeController.AllModelsInOne;
        public ActionResult HastaGoruntule()
        {

            return View();
        }

        public ActionResult TibbiRaporGoruntuleDoc()
        {

            return View();
        }
        [HttpPost]
        public ActionResult TibbiRaporGoruntuleDoc(int LabId)
        {
            if (LabId > 0)
            {
                Session["LabId"] = LabId;
                return RedirectToAction("HastaRaporGuncelle");
            }
            return View();
        }
        public ActionResult HastaRaporGuncelle()
        {
            CombinedModels model = new CombinedModels();
            model.Rapor = raporCommand.FindRapor(Convert.ToInt32(Session["LabId"]));
            return View(model);
        }
        [HttpPost]
        public ActionResult HastaRaporGuncelle(CombinedModels model, HttpPostedFileBase fileUpload)
        {
            Rapor findRapor = raporCommand.FindRapor(model.Rapor.RaporID);

            if (fileUpload != null)
            {
                string dosyaAdi = Path.GetFileName(fileUpload.FileName);

                string dosyaYolu = fileUpload.FileName;

                string hedefDizin = Server.MapPath("~/TıbbiRaporlar/" + dosyaAdi);

                fileUpload.SaveAs(hedefDizin);
                Account account = new Account(
                  "dgo9vu84q",
                  "588119493444437",
                  "bzgi4pm3LthGPUduROOnqd2Zdb8");
                Cloudinary cloudinary = new Cloudinary(account);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(hedefDizin),
                    Folder = "prolab"
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                string imageUrl = uploadResult.Url.ToString();

                findRapor.URL = imageUrl;
            }
            
            findRapor.İçerik = model.Rapor.İçerik;

            int durum = 0;
            try
            {
                durum = raporCommand.Update(findRapor);
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

            return View(model);
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