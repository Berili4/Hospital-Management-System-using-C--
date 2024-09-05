using prolab_THEEND.Controllers;
using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Commands
{
    public class DoktorCommand : RepoCommand
    {
        public List<Doktor> DoktorDatabase = new List<Doktor>();
        public static List<string> Hastane = new List<string>() { "Kocaeli Seka Hastanesi", "Kocaeli Devlet Hastanesi", "Kocaeli Şehir Hastanesi" };
        public static List<string> Hastalıklar = new List<string>(){ "Dahiliye", "KulakBurunBogaz", "Genel Cerrahi" };
        public int Insert(Doktor data)
        {
            int durum = -1;
            string sql = "Insert into Doktor Values(@AD,@SOYAD,@UZMANLIKALANI,@HASTANE,@SIFRE)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);
                cmd.Parameters.AddWithValue("@UZMANLIKALANI", data.UzmanlıkAlanı);
                cmd.Parameters.AddWithValue("@HASTANE", data.Hastane);

                durum = cmd.ExecuteNonQuery();
                conn.Close();
            }
            DoktorDatabase = ToList();
            return durum;
        }
        public int Delete(Doktor data)
        {
            int durum = -1;
            string sql = "Delete from Doktor Where DoktorID = @ID";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                List<Islem> islemler = HomeController.islemModifier.FindIslemDocId(data.DoktorID);
                List<Randevu> randevular = HomeController.randevuModifier.FindRandevuAsListDoc(data.DoktorID);
                foreach (var islem in islemler)
                    HomeController.islemModifier.Delete(islem);
                foreach (var islem in randevular)
                    HomeController.randevuModifier.Delete(islem);

                cmd.Parameters.AddWithValue("@ID", data.DoktorID);
                durum = cmd.ExecuteNonQuery();
                conn.Close();
                DoktorDatabase = ToList();
            }
            return durum;
        }
        public List<Doktor> ToList()
        {
            DoktorDatabase.Clear();
            if (DoktorDatabase.Count != 0)
                return DoktorDatabase;

            string sql = "SELECT * FROM Doktor";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Doktor Doktor = new Doktor();
                    Doktor.DoktorID = Convert.ToInt32(reader["DoktorId"]);
                    Doktor.Ad = reader["Ad"].ToString();
                    Doktor.Şifre = reader["Şifre"].ToString();
                    Doktor.SoyAd = reader["SoyAd"].ToString();
                    Doktor.Hastane = reader["Hastane"].ToString();
                    Doktor.UzmanlıkAlanı = reader["UzmanlıkAlanı"].ToString();

                    // Doktor nesnesini listeye ekle
                    DoktorDatabase.Add(Doktor);
                }
                reader.Close();
                connection.Close();
            }
            return DoktorDatabase;
        }
        public Doktor FindDoctorById(int id)
        {
            string sql = "select * from Doktor Where @Id = DoktorId";
            using(SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                Doktor doktor = new Doktor();
                while (reader.Read())
                {
                    doktor.DoktorID = Convert.ToInt32(reader["DoktorId"]);
                    doktor.Ad = reader["Ad"].ToString();
                    doktor.Şifre = reader["Şifre"].ToString();
                    doktor.SoyAd = reader["SoyAd"].ToString();
                    doktor.Hastane = reader["Hastane"].ToString();
                    doktor.UzmanlıkAlanı = reader["UzmanlıkAlanı"].ToString();
                }
                reader.Close();
                connection.Close();
                foreach (var theDoc in DoktorDatabase)
                {
                    if (theDoc.DoktorID == doktor.DoktorID)
                        doktor = theDoc;
                }
                return doktor;
            }
        }
        public List<Doktor> WhereGetAll(string SQLQUERY,string[] parameters, string[] parameterValues)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(SQLQUERY, connection);

                for(int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue(parameters[i], parameterValues[i]);
                }

                SqlDataReader reader = command.ExecuteReader();

                List<Doktor> tempList = new List<Doktor>();

                while (reader.Read())
                {
                    Doktor Doktor = new Doktor();
                    Doktor.DoktorID = Convert.ToInt32(reader["DoktorId"]);
                    Doktor.Ad = reader["Ad"].ToString();
                    Doktor.Şifre = reader["Şifre"].ToString();
                    Doktor.SoyAd = reader["SoyAd"].ToString();
                    Doktor.Hastane = reader["Hastane"].ToString();
                    Doktor.UzmanlıkAlanı = reader["UzmanlıkAlanı"].ToString();

                    // Doktor nesnesini listeye ekle
                    tempList.Add(Doktor);
                }
                reader.Close();
                connection.Close();
                return tempList;
            }
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
        public int Update(Doktor data)
        {
            string sql = "UPDATE Doktor SET Ad = @AD, SoyAd = @SOYAD, Şifre = @SIFRE, UzmanlıkAlanı = @UZMANLIKALANI, Hastane = @HASTANE WHERE DoktorId = @ID";
            int returned = 0;

            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);
                cmd.Parameters.AddWithValue("@UZMANLIKALANI", data.UzmanlıkAlanı);
                cmd.Parameters.AddWithValue("@HASTANE", data.Hastane);
                cmd.Parameters.AddWithValue("@ID", data.DoktorID);

                returned = cmd.ExecuteNonQuery();

                conn.Close();
                DoktorDatabase = ToList();
            }
            return returned;
        }

    }
}