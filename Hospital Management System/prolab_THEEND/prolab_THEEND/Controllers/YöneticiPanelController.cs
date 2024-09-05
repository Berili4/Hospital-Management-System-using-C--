using prolab_THEEND.Commands;
using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text;
using System.Web.Security;
using System.Net;

namespace prolab_THEEND.Controllers
{
    public class YöneticiPanelController : Controller
    {
        public HastaCommand hastaCommand = HomeController.hastaModifier;
        public List<Hasta> hastaList = HomeController.hastaModifier.HastaDatabase;
        public DoktorCommand doktorCommand = HomeController.doktorModifier;
        public List<Doktor> doktorList = HomeController.doktorModifier.DoktorDatabase;
        public RandevuCommand randevuCommand = HomeController.randevuModifier;
        public RaporCommand raporCommand = HomeController.raporModifier;
        public IslemCommand islemCommand = HomeController.islemModifier;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HastaListele()
        {
            ViewBag.ToplamHastaSayisi = hastaList.Count; // Toplam hasta sayısını saklayalım
            int sayfa = Convert.ToInt32(Request.QueryString["sayfaNumarasi"]);

            List<Hasta> tempList = new List<Hasta>();
            for (int i = 10 * sayfa; i < 10 * sayfa + 10 && i < hastaList.Count ; i++)
            {
                tempList.Add(hastaList[i]);
            }
            Session["TempList"] = tempList;
            return View();
        }
        [HttpPost]
        public ActionResult HastaListele(string HastaTc)
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = hastaCommand.FindHastaWithTC(HastaTc);
            Session["HastaTcTemp"] = tempModel.Hasta.TcKimlikNo as string;
            if (tempModel.Hasta != null)
            {
                string encryptedTC = EncryptionHelper.EncryptString(tempModel.Hasta.TcKimlikNo , "TCKey");
                return RedirectToAction("HastaGüncelle", "YöneticiPanel", new { HastaTc = encryptedTC });
            }
            return View();
        }
        public ActionResult HastaEkle()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = new Hasta();
            return View(tempModel);
        }
        [HttpPost]
        public ActionResult HastaEkle(CombinedModels model, DateTime DogumTarihi)
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = new Hasta();
            string HastaTc = Request.QueryString["HastaTc"];
            model.Hasta.DogumTarihi = DogumTarihi;


            var x = model.Hasta;

            int durum = 0;
            try
            {
                durum = hastaCommand.Insert(x);
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
            return View(tempModel);
        }
        public ActionResult HastaSil()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = new Hasta();
            return View(tempModel);
        }
        [HttpPost]
        public ActionResult HastaSil(CombinedModels model)
        {

            var x = model.Hasta;

            int durum = 0;
            try
            {
                durum = hastaCommand.Delete(x);
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
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = new Hasta();

            return View(tempModel);
        }
        public ActionResult HastaGüncelle()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Hasta = new Hasta();

            return View(tempModel);
        }
        [HttpPost]
        public ActionResult HastaGüncelle(CombinedModels model , DateTime DogumTarihi)
        {
            string hastaTc = "";
            if (Session["HastaTcTemp"] != null)
                hastaTc = Session["HastaTcTemp"] as string;
            else
                hastaTc = model.Hasta.TcKimlikNo;

            model.Hasta.DogumTarihi = DogumTarihi;

            if (model.Hasta.Ad == null || model.Hasta.SoyAd == null || model.Hasta.DogumTarihi == null || model.Hasta.Adres == null || model.Hasta.Cinsiyet == null || model.Hasta.TcKimlikNo == null || model.Hasta.TelefonNo == null || model.Hasta.Şifre == null)
                return View(model);

            Hasta realHasta = hastaCommand.FindHastaWithTC(hastaTc);
            model.Hasta.HastaId = realHasta.HastaId;
            model.Hasta.BildirimSayısı = realHasta.BildirimSayısı;
            int durum = 0;
            try
            {
                durum = hastaCommand.Update(model.Hasta);
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
            Session["HastaTcTemp"] = model.Hasta.TcKimlikNo;
            return View(model);
        }

        public ActionResult DoktorListele()
        {
            ViewBag.ToplamHastaSayisi = doktorList.Count; // Toplam hasta sayısını saklayalım
            int sayfa = Convert.ToInt32(Request.QueryString["sayfaNumarasi"]);

            List<Doktor> tempList = new List<Doktor>();
            for (int i = 10 * sayfa; i < 10 * sayfa + 10 && i < doktorList.Count; i++)
            {
                tempList.Add(doktorList[i]);
            }
            Session["TempList"] = tempList;
            return View();
        }
        [HttpPost]
        public ActionResult DoktorListele(int DoktorId)
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = doktorCommand.FindDoctorById(DoktorId);
            Session["DoktorId"] = DoktorId;
            if (tempModel.Doktor != null)
            {
                string encryptedDocId = EncryptionHelper.EncryptInt(DoktorId, "DoktorIdKey");
                return RedirectToAction("DoktorGuncelle", "YöneticiPanel", new { DoktorId = encryptedDocId });
            }
            return View();
        }
        public ActionResult DoktorEkle()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = new Doktor();
            return View(tempModel);
        }
        [HttpPost]
        public ActionResult DoktorEkle(CombinedModels model)
        {
            var x = model.Doktor;
            int durum = 0;
            try
            {
                durum = doktorCommand.Insert(x);
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

            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = new Doktor();
            return View(tempModel);
        }

        public ActionResult DoktorSil()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = new Doktor();
            return View(tempModel);
        }
        [HttpPost]
        public ActionResult DoktorSil(CombinedModels model)
        {

            var x = model.Doktor;
            int durum = 0;
            try
            {
                durum = doktorCommand.Delete(x);
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

            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = new Doktor();

            return View(tempModel);
        }
        public ActionResult DoktorGuncelle()
        {
            CombinedModels tempModel = new CombinedModels();
            tempModel.Doktor = new Doktor();

            return View(tempModel);
        }
        [HttpPost]
        public ActionResult DoktorGuncelle(CombinedModels model)
        {
            model.Doktor.DoktorID = Convert.ToInt32(Session["DoktorId"]);

            int durum = 0;
            try
            {
                durum = doktorCommand.Update(model.Doktor);
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
        public ActionResult RandevuPanel()
        {
            ViewBag.ToplamHastaSayisi = randevuCommand.RandevuDatabase.Count; // Toplam hasta sayısını saklayalım
            int sayfa = Convert.ToInt32(Request.QueryString["sayfaNumarasi"]);

            List<Randevu> tempList = new List<Randevu>();
            for (int i = 10 * sayfa; i < 10 * sayfa + 10 && i < randevuCommand.RandevuDatabase.Count; i++)
            {
                tempList.Add(randevuCommand.RandevuDatabase[i]);
            }
            Session["TempList"] = tempList;
            return View();
        }
        [HttpPost]
        public ActionResult RandevuPanel(int RandevuId)
        {
            Session["RandevuId"] = RandevuId;

            if (Session["RandevuId"] != null)
            {
                string encryptedAppointmentId = EncryptionHelper.EncryptInt(RandevuId, "AppointmentIDKey");
                return RedirectToAction("RandevuGuncelle", new { RandevuId = encryptedAppointmentId });
            }
            else
                return View();
        }
        public ActionResult RandevuGuncelle()
        {
            Randevu selectedRandevu = randevuCommand.FindRandevu(Convert.ToInt32(Session["RandevuId"]));
            CombinedModels model = new CombinedModels();
            model.Hasta = selectedRandevu.Hasta;
            model.Doktor = selectedRandevu.Doktor;
            return View(model);
        }
        [HttpPost]
        public ActionResult RandevuGuncelle(DateTime RandevuTarih)
        {
            Randevu selectedRandevu = randevuCommand.FindRandevu(Convert.ToInt32(Session["RandevuId"]));
            CombinedModels model = new CombinedModels();
            model.Hasta = selectedRandevu.Hasta;
            model.Doktor = selectedRandevu.Doktor;
            selectedRandevu.RandevuTarih = RandevuTarih;
            int durum = 0;
            try
            {
                durum = randevuCommand.Update(selectedRandevu);
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
        public ActionResult TıbbiRaporEkle()
        {
            CombinedModels model = new CombinedModels();
            model.Rapor = new Rapor();
            model.Rapor.Doktor = new Doktor();
            model.Rapor.Hasta = new Hasta();
            model.Rapor.Islem = new Islem();
            return View(model);
        }
        [HttpPost]
        public ActionResult TıbbiRaporEkle(CombinedModels models,DateTime SonucTarih, HttpPostedFileBase fileUpload)
        {
            Islem islem = islemCommand.FindIslemWithId(models.Rapor.Islem.IslemId);
            Hasta hasta = hastaCommand.FindHastaWithId(islem.HastaId);

            if (hasta.TcKimlikNo != models.Rapor.Hasta.TcKimlikNo || models.Rapor.Hasta.TcKimlikNo == null || models.Rapor.Islem.IslemId == 0)
                return View(models);

   
            Rapor data = new Rapor();
            
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

            data.Tarih = SonucTarih;
            data.URL = imageUrl;
            data.İçerik = models.Rapor.İçerik;
            data.Doktor = doktorCommand.FindDoctorById(islem.DoktorId);
            data.Hasta = hastaCommand.FindHastaWithTC(models.Rapor.Hasta.TcKimlikNo);
            data.Islem = islemCommand.FindIslemWithId(models.Rapor.Islem.IslemId);
            try
            {
                if (raporCommand.Insert(data) == 1)
                {
                    data.Islem.TıbbiRaporId = raporCommand.FindRaporIslemId(data.Islem.IslemId).RaporID;
                    islemCommand.Update(data.Islem);
                    data.Hasta.BildirimSayısı++;
                    hastaCommand.Update(data.Hasta);
                }
            }catch(Exception ex)
            {
                return View(models);

            }
            return View(models);
        }
        public ActionResult LabSonuc()
        {
            raporCommand.ToList();
            ViewBag.ToplamHastaSayisi = randevuCommand.RandevuDatabase.Count; // Toplam hasta sayısını saklayalım
            int sayfa = Convert.ToInt32(Request.QueryString["sayfaNumarasi"]);

            List<Rapor> tempList = new List<Rapor>();
            for (int i = 10 * sayfa; i < 10 * sayfa + 10 && i < raporCommand.raporDatabase.Count; i++)
            {
                tempList.Add(raporCommand.raporDatabase[i]);
            }
            Session["TempList"] = tempList;
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
        [HttpPost]
        public ActionResult LabSonuc(int LabId)
        {
            int id = LabId;
            if (id != 0)
            {
                Session["LabSonucId"] = LabId;
                string encryptedLabId = EncryptionHelper.EncryptInt(LabId,"LabIDKey");

                return RedirectToAction("LabRaporGuncelle" , new { LabSonucId = encryptedLabId });
            }
            return View();
        }
        public ActionResult LabRaporGuncelle()
        {
            CombinedModels model = new CombinedModels();
            model.Rapor = raporCommand.FindRapor(Convert.ToInt32(Session["LabSonucId"]));
            return View(model);
        }
        [HttpPost]
        public ActionResult LabRaporGuncelle(CombinedModels model, HttpPostedFileBase fileUpload)
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
        public ActionResult LabSonucSil()
        {
            CombinedModels model = new CombinedModels();
            model.Rapor = new Rapor();
            return View(model);
        }
        [HttpPost]
        public ActionResult LabSonucSil(CombinedModels model)
        {
            if(model.Rapor.RaporID == 0)
                return View(model);
            Rapor selectedRapor = raporCommand.FindRapor(model.Rapor.RaporID);
            int islemId = -1;
            int durum = 0;
            try
            {
                if (selectedRapor.RaporID != 0)
                {
                    islemId = selectedRapor.Islem.IslemId;
                    durum = raporCommand.Delete(selectedRapor);
                    if (durum != 0)
                    {
                        Islem islem = islemCommand.FindIslemWithId(islemId);
                        islem.TıbbiRaporId = 0;
                        islemCommand.Update(islem);
                    }
                }
                
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
    }
}
public static class EncryptionHelper
{
    public static string EncryptInt(int data,string key)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
        byte[] protectedBytes = MachineKey.Protect(bytes, key);
        return HttpServerUtility.UrlTokenEncode(protectedBytes);
    }
    public static string EncryptString(string data, string key)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] protectedBytes = MachineKey.Protect(bytes, key);
        return HttpServerUtility.UrlTokenEncode(protectedBytes);
    }
    public static int DecryptInt(string encryptedLabId)
    {
        byte[] protectedBytes = HttpServerUtility.UrlTokenDecode(encryptedLabId);
        byte[] bytes = MachineKey.Unprotect(protectedBytes, "LabIdEncryption");
        return int.Parse(Encoding.UTF8.GetString(bytes));
    }
}