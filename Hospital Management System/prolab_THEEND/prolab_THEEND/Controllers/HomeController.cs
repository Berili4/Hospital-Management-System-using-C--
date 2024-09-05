using Faker;
using prolab_THEEND.Commands;
using prolab_THEEND.Models;
using prolab_THEEND.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace prolab_THEEND.Controllers
{
    public class HomeController : Controller
    {
        public static HastaCommand hastaModifier = new HastaCommand();
        public static DoktorCommand doktorModifier = new DoktorCommand();
        public static YoneticiCommand yöneticiModifier = new YoneticiCommand();
        public static RandevuCommand randevuModifier = new RandevuCommand();
        public static IslemCommand islemModifier = new IslemCommand();
        public static RaporCommand raporModifier = new RaporCommand();
        public static CombinedModels AllModelsInOne = new CombinedModels();
        public ActionResult Index()
        {
            islemModifier.IslemDatabase = islemModifier.ToList(); // ilkten tablo null başlangıç atamaları yapılır
            doktorModifier.DoktorDatabase = doktorModifier.ToList();    // İŞLEM SIRASI ÇOK ONEMLİ !!!!!!
            randevuModifier.RandevuDatabase = randevuModifier.ToList();
            hastaModifier.HastaDatabase = hastaModifier.ToList();
            yöneticiModifier.YöneticiDatabase = yöneticiModifier.ToList();
            raporModifier.raporDatabase = raporModifier.ToList();

          /*  for (int i = 0; i < 50; i++)
            {
                Hasta hasta = new Hasta();

                hasta.Ad = NameFaker.Name();
                hasta.SoyAd = NameFaker.LastName();
                hasta.Adres = LocationFaker.City();
                hasta.Cinsiyet = NumberFaker.Number(0, 2) % 2 == 0 ? "Erkek" : "Kadın";
                hasta.BildirimSayısı = 0;
                hasta.DogumTarihi = DateTimeFaker.BirthDay();
                hasta.TcKimlikNo = "100000000"+i;
                hasta.Şifre = hasta.SoyAd + i;
                hasta.TelefonNo = PhoneFaker.Phone();

                hastaModifier.Insert(hasta);

            }*/
          /*for (int i = 0; i < 25; i++)
            {
                Doktor doktor = new Doktor();
                doktor.Ad = NameFaker.Name();
                doktor.SoyAd = NameFaker.LastName();
                doktor.Hastane = DoktorCommand.Hastane[NumberFaker.Number(0,3)];
                doktor.Şifre = doktor.SoyAd + i;
                doktor.UzmanlıkAlanı = DoktorCommand.Hastalıklar[NumberFaker.Number(0, 3)];
                doktorModifier.Insert(doktor);
            }*/

            return View();
        }
        public ActionResult HastaLogin()
        {
            AllModelsInOne.Hasta = new Hasta();
            return View(AllModelsInOne);
        }
        [HttpPost]
        public ActionResult HastaLogin(CombinedModels LoggedHasta)
        {
            if (LoggedHasta.Hasta.TcKimlikNo == null || LoggedHasta.Hasta.Şifre == null)
                return View(AllModelsInOne);

            string[] parameters = { "@TcKimlikNo", "@Şifre" };
            string[] parameterValues = { LoggedHasta.Hasta.TcKimlikNo, LoggedHasta.Hasta.Şifre };

            var HastaFromDatabase = hastaModifier.WhereGetAll("Select * from Hasta Where TcKimlikNo = @TcKimlikNo AND Şifre = @Şifre", parameters, parameterValues);

            if (HastaFromDatabase.Count != 0)
            {
                AllModelsInOne.Hasta = HastaFromDatabase[0];
                foreach (var hasta in hastaModifier.HastaDatabase)
                {
                    if (AllModelsInOne.Hasta.HastaId == hasta.HastaId)
                    {
                        AllModelsInOne.Hasta = hasta;
                        break;
                    }
                }
                Session["HastaLogin"] = AllModelsInOne;
                return Redirect("~/HastaPanel/Index");
            }
            else
            {
                AllModelsInOne.Hasta = new Hasta();
                return View(AllModelsInOne);

            }
        }
        public ActionResult YoneticiLogin()
        {

            AllModelsInOne.Yönetici = new Yönetici();
            return View(AllModelsInOne);
        }
        [HttpPost]
        public ActionResult YoneticiLogin(CombinedModels LoggedYönetici)
        {
            if (LoggedYönetici.Yönetici.Ad == null || LoggedYönetici.Yönetici.Şifre == null)
                return View(AllModelsInOne);

            string[] parameters = { "@Ad", "@Şifre" };
            string[] parameterValues = { LoggedYönetici.Yönetici.Ad, LoggedYönetici.Yönetici.Şifre };

            
            var YöneticiFromDatabase = yöneticiModifier.WhereGetAll("Select * from Yönetici Where Ad = @Ad AND Şifre = @Şifre", parameters, parameterValues);

            if (YöneticiFromDatabase.Count != 0)
            {
                AllModelsInOne.Yönetici = YöneticiFromDatabase[0];
                Session["YöneticiLogin"] = AllModelsInOne;
                return RedirectToAction("Index");
            }
            else
                return View(AllModelsInOne);
        }
        public ActionResult DoktorLogin()
        {
            AllModelsInOne.Doktor = new Doktor();

            return View(AllModelsInOne);
        }
        [HttpPost]
        public ActionResult DoktorLogin(CombinedModels LoggedDoktor)
        {
            if (LoggedDoktor.Doktor.Ad == null || LoggedDoktor.Doktor.Şifre == null)
                return View(AllModelsInOne);

            string[] parameters = { "@Ad", "@Şifre" };
            string[] parameterValues = { LoggedDoktor.Doktor.Ad, LoggedDoktor.Doktor.Şifre };

            var doktorFromDatabase = doktorModifier.WhereGetAll("Select * from Doktor Where Ad = @Ad AND Şifre = @Şifre", parameters, parameterValues);
            if (doktorFromDatabase.Count != 0)
            {
                AllModelsInOne.Doktor = doktorFromDatabase[0];
                foreach (var doktor in doktorModifier.DoktorDatabase)
                {
                    if (AllModelsInOne.Doktor.DoktorID == doktor.DoktorID)
                    {
                        AllModelsInOne.Doktor = doktor;
                        break;
                    }
                }
                Session["DoktorLogin"] = AllModelsInOne;
                return RedirectToAction("Index");
            }
            else
            {
                AllModelsInOne.Doktor = new Doktor();

                return View(AllModelsInOne);
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}