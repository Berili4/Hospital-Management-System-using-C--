using prolab_THEEND.Controllers;
using prolab_THEEND.Models;
using prolab_THEEND.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace prolab_THEEND.Commands
{
    public class HastaCommand : RepoCommand
    {
        public List<Hasta> HastaDatabase = new List<Hasta>();
        public int Insert(Hasta data)
        {
            int durum = -1;
            string sql = "Insert into Hasta Values(@AD,@SOYAD,@CINSIYET,@TELNO,@ADRES,@TC,@SIFRE,@DOGUM,@BILDIRI)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TC", data.TcKimlikNo);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);
                cmd.Parameters.AddWithValue("@DOGUM", data.DogumTarihi);
                cmd.Parameters.AddWithValue("@CINSIYET", data.Cinsiyet);
                cmd.Parameters.AddWithValue("@TELNO", data.TelefonNo);
                cmd.Parameters.AddWithValue("@ADRES", data.Adres);
                cmd.Parameters.AddWithValue("@BILDIRI", data.BildirimSayısı);
                durum = cmd.ExecuteNonQuery();
                conn.Close();
                HastaDatabase = ToList();
            }
            HastaDatabase = ToList();
            return durum;
        }
        public int Delete(Hasta data)
        {
            int durum = -1;
            string sql = "Delete from Hasta Where TcKimlikNo = @TcKimlikNo";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TcKimlikNo", data.TcKimlikNo);
                List<Islem> islemler = HomeController.islemModifier.FindIslem(data.TcKimlikNo);
                List<Randevu> randevular = HomeController.randevuModifier.FindRandevuAsList(data.TcKimlikNo);
                foreach (var islem in islemler)
                    HomeController.islemModifier.Delete(islem);
                foreach (var islem in randevular)
                    HomeController.randevuModifier.Delete(islem);
                durum = cmd.ExecuteNonQuery();
                conn.Close();
                HastaDatabase = ToList();
            }
            HastaDatabase = ToList();
            return durum;
        }
        public Hasta FindHastaWithTC(string tcno)
        {
            string sql = "Select * from Hasta Where @TC = TcKimlikNo";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TC", tcno);
                SqlDataReader reader = cmd.ExecuteReader();

                Hasta hasta = new Hasta();

                while (reader.Read())
                {
                    hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    hasta.Ad = reader["Ad"].ToString();
                    hasta.SoyAd = reader["SoyAd"].ToString();
                    hasta.Şifre = reader["Şifre"].ToString();
                    hasta.DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]);
                    hasta.Cinsiyet = reader["Cinsiyet"].ToString();
                    hasta.TelefonNo = reader["TelefonNo"].ToString();
                    hasta.Adres = reader["Adres"].ToString();
                    hasta.TcKimlikNo = reader["TcKimlikNo"].ToString();
                    hasta.BildirimSayısı = Convert.ToInt32(reader["BildirimSayisi"]);
                    AddListToAppointment(hasta);

                }
                reader.Close();
                conn.Close();
                return hasta;
            }
        }
        public Hasta FindHastaWithId(int id)
        {
            string sql = "Select * from Hasta Where @id = HastaId";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                Hasta hasta = new Hasta();

                while (reader.Read())
                {
                    hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    hasta.Ad = reader["Ad"].ToString();
                    hasta.SoyAd = reader["SoyAd"].ToString();
                    hasta.DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]);
                    hasta.Cinsiyet = reader["Cinsiyet"].ToString();
                    hasta.TelefonNo = reader["TelefonNo"].ToString();
                    hasta.Adres = reader["Adres"].ToString();
                    hasta.BildirimSayısı = Convert.ToInt32(reader["BildirimSayisi"]);
                    hasta.TcKimlikNo = reader["TcKimlikNo"].ToString();
                    AddListToAppointment(hasta);

                }
                reader.Close();
                conn.Close();
                return hasta;
            }
        }
        public List<Hasta> ToList()
        {

            HastaDatabase.Clear();

            string sql = "SELECT * FROM Hasta";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Hasta hasta = new Hasta();
                    hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    hasta.Ad = reader["Ad"].ToString();
                    hasta.DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]);
                    hasta.SoyAd = reader["SoyAd"].ToString();
                    hasta.Cinsiyet = reader["Cinsiyet"].ToString();
                    hasta.TelefonNo = reader["TelefonNo"].ToString();
                    hasta.Adres = reader["Adres"].ToString();
                    hasta.BildirimSayısı = Convert.ToInt32(reader["BildirimSayisi"]);
                    hasta.TcKimlikNo = reader["TcKimlikNo"].ToString();
                    // LATER HASH THE DOCTOR WITH THE REAL ONE..
                    AddListToAppointment(hasta);
                    // Hasta nesnesini listeye ekle
                    HastaDatabase.Add(hasta);
                }
                reader.Close();
                connection.Close();
            }
            return HastaDatabase;
        }
        public void AddListToAppointment(Hasta hasta)
        {
            if (hasta.Randevular == null)
                hasta.Randevular = new List<Randevu>();
            if (hasta.SorumluDoktor == null)
                hasta.SorumluDoktor = new List<Doktor>();
            else
                hasta.Randevular.Clear();

            foreach (Islem islem in HomeController.islemModifier.IslemDatabase)
                if (islem.HastaId == hasta.HastaId)
                {
                    foreach (Randevu randevu in HomeController.randevuModifier.RandevuDatabase)
                    {
                        if (islem.RandevuId == randevu.RandevuId)
                        {
                            hasta.Randevular.Add(randevu);
                            Doktor tempDoktor = HomeController.doktorModifier.FindDoctorById(islem.DoktorId);

                            if (!hasta.SorumluDoktor.Contains(tempDoktor))
                                hasta.SorumluDoktor.Add(tempDoktor);

                            if (tempDoktor.HastaList == null)
                                tempDoktor.HastaList = new List<Hasta>();

                            if (tempDoktor.HastaList.Count == 0)
                                tempDoktor.HastaList.Add(hasta);
                            else
                            {
                                bool doesExist = false;

                                for (int i = 0; i < tempDoktor.HastaList.Count; i++)
                                {
                                    if (tempDoktor.HastaList[i].HastaId == hasta.HastaId)
                                        doesExist = true;
                                }

                                if (!doesExist)
                                    tempDoktor.HastaList.Add(hasta);
                            }
                           
                            randevu.Hasta = hasta;
                            randevu.Doktor = tempDoktor;
                        }
                    }
                }
        }
        public List<Hasta> WhereGetAll(string SQLQUERY, string[] parameters, string[] parameterValues)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(SQLQUERY, connection);

                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue(parameters[i], parameterValues[i]);
                }

                SqlDataReader reader = command.ExecuteReader();

                List<Hasta> tempList = new List<Hasta>();

                while (reader.Read())
                {
                    Hasta hasta = new Hasta();
                    hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    hasta.Ad = reader["Ad"].ToString();
                    hasta.SoyAd = reader["SoyAd"].ToString();
                    hasta.DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]);
                    hasta.Cinsiyet = reader["Cinsiyet"].ToString();
                    hasta.TelefonNo = reader["TelefonNo"].ToString();
                    hasta.Adres = reader["Adres"].ToString();
                    hasta.BildirimSayısı = Convert.ToInt32(reader["BildirimSayisi"]);
                    hasta.TcKimlikNo = reader["TcKimlikNo"].ToString();
                    AddListToAppointment(hasta);

                    // Hasta nesnesini listeye ekle
                    tempList.Add(hasta);
                }
                reader.Close();
                connection.Close();
                return tempList;
            }

        }
        public int Update(Hasta data)
        {
            int returned = 0;
            using (SqlConnection conn = CreateConnection())
            {
                string sql = "Update Hasta SET Ad = @AD, SoyAd = @SOYAD, BildirimSayisi = @BILDIRI , DogumTarihi = @DOGUM , Şifre = @SIFRE , Cinsiyet = @CINSIYET , TelefonNo = @TELNO , Adres = @ADRES , TcKimlikNo = @TC WHERE HastaId = @ID";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@TC", data.TcKimlikNo);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);
                cmd.Parameters.AddWithValue("@CINSIYET", data.Cinsiyet);
                cmd.Parameters.AddWithValue("@TELNO", data.TelefonNo);
                cmd.Parameters.AddWithValue("@ADRES", data.Adres);
                cmd.Parameters.AddWithValue("@DOGUM", data.DogumTarihi);
                cmd.Parameters.AddWithValue("@ID", data.HastaId);
                cmd.Parameters.AddWithValue("@BILDIRI", data.BildirimSayısı);

                returned = cmd.ExecuteNonQuery();
                conn.Close();
                HastaDatabase = ToList();
            }
            return returned;
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
       
    }
}