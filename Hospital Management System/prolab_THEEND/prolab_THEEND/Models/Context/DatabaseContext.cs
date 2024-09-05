using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Faker;
namespace prolab_THEEND.Models.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Doktor> Doktor { get; set; }
        public DbSet<Rapor> Rapor { get; set; }
        public DbSet<Yönetici> Yönetici { get; set; }
        public DbSet<Randevu> Randevu { get; set; }
        public DbSet<Hasta> Hasta { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new CreateDataBase_Custom());
        }

    }
    public class CreateDataBase_Custom : CreateDatabaseIfNotExists<DatabaseContext>
    {
        private string[] UzmanlıkAlanları = { "Dahiliye" ,"KulakBurunBogaz","Üroloji","Genel Cerrahi","Kadın Doğum Hastalıkları"};
        private string[] Hastane = { "Kocaeli Devlet Hastanesi", "Kocaeli Seka Hastanesi" ,"Kocaeli Şehir Hastanesi"};
        protected override void Seed(DatabaseContext context)
        {
            for (int i = 0; i < 4; i++)
            {
                Doktor doktor = new Doktor();
                doktor.Ad = NameFaker.Name();
                doktor.SoyAd = NameFaker.LastName();
                doktor.UzmanlıkAlanı = UzmanlıkAlanları[NumberFaker.Number(0, 4)];
                doktor.Hastane = Hastane[NumberFaker.Number(0,2)];

                context.Doktor.Add(doktor);
            }
            context.SaveChanges();
            List<Doktor> doktorlar = context.Doktor.ToList();
            for (int i = 0; i < 10; i++)
            {
                Hasta hasta = new Hasta();
                hasta.Ad = NameFaker.Name();
                hasta.SoyAd = NameFaker.LastName();
                hasta.DogumTarihi = DateTimeFaker.BirthDay();
                hasta.Cinsiyet = NumberFaker.Number(0, 2) == 0 ? "Erkek" : "Kadın" ;
                hasta.TelefonNo = PhoneFaker.Phone();
                hasta.Adres = LocationFaker.City();

                context.Hasta.Add(hasta);
            }
            context.SaveChanges();
        }
    }
}