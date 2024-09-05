using prolab_THEEND.Controllers;
using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Commands
{
    public class RaporCommand : RepoCommand
    {
        public List<Rapor> raporDatabase = new List<Rapor>();
        public int Insert(Rapor data)
        {
            int confirmed = 0;
            string sql = "Insert into Rapor Values(@Tarih,@İçerik,@HastaId,@DoktorId,@URL,@IslemId)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IslemId", data.Islem.IslemId);
                cmd.Parameters.AddWithValue("@HastaId", data.Hasta.HastaId);
                cmd.Parameters.AddWithValue("@DoktorId", data.Doktor.DoktorID);
                cmd.Parameters.AddWithValue("@İçerik", data.İçerik);
                cmd.Parameters.AddWithValue("@URL", data.URL);
                cmd.Parameters.AddWithValue("@Tarih", data.Tarih);

                cmd.ExecuteNonQuery();
                conn.Close();
                raporDatabase.Add(data);
                confirmed = 1;
            }
            raporDatabase = ToList();
            return confirmed;
        }
        public int Delete(Rapor data)
        {
            int returned = -1;
            string sql = "Delete from Rapor Where RaporID = @RaporID";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RaporID", data.RaporID);
                returned = cmd.ExecuteNonQuery();
                conn.Close();
                raporDatabase = ToList();
            }
            return returned;
        }
        public List<Rapor> ToList()
        {
            raporDatabase.Clear();

            string sql = "SELECT * FROM Rapor";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Rapor rapor = new Rapor();
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = HomeController.hastaModifier.FindHastaWithId(Convert.ToInt32(reader["HastaId"]));
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = HomeController.doktorModifier.FindDoctorById(Convert.ToInt32(reader["DoktorId"]));
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.Islem = HomeController.islemModifier.FindIslemWithId(Convert.ToInt32(reader["IslemId"]));
                    rapor.URL = reader["URL"].ToString();
                    raporDatabase.Add(rapor);
                }
                reader.Close();
                connection.Close();
            }
            return raporDatabase;
        }
        public Rapor FindRaporIslemId(int islemId)
        {
            Islem islem = HomeController.islemModifier.FindIslemWithId(islemId);

            string sql = "SELECT * FROM Rapor Where @IslemId = IslemId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@IslemId", islem.IslemId);
                SqlDataReader reader = command.ExecuteReader();

                Rapor rapor = new Rapor();

                while (reader.Read())
                {
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = new Hasta();
                    rapor.Hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = new Doktor();
                    rapor.Doktor.DoktorID = Convert.ToInt32(reader["DoktorId"]);
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.Islem = new Islem();
                    rapor.Islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    rapor.URL = reader["URL"].ToString();

                }
                reader.Close();
                connection.Close();

                return rapor;
            }
        }
        public List<Rapor> FindRaporWithDoktorId(int DoktorId)
        {
            string sql = "SELECT * FROM Rapor Where @DoktorId = DoktorId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DoktorId", DoktorId);
                SqlDataReader reader = command.ExecuteReader();

                List<Rapor> tempRapor = new List<Rapor>();

                while (reader.Read())
                {
                    Rapor rapor = new Rapor();
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = HomeController.hastaModifier.FindHastaWithId(Convert.ToInt32(reader["HastaId"]));
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = HomeController.doktorModifier.FindDoctorById(Convert.ToInt32(reader["DoktorId"]));
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.URL = reader["URL"].ToString();
                    rapor.Islem = HomeController.islemModifier.FindIslemWithId(Convert.ToInt32(reader["IslemId"]));
                    tempRapor.Add(rapor);
                }
                reader.Close();
                connection.Close();
                return tempRapor;
            }
        }
        public Rapor FindRapor(int raporId)
        {
            string sql = "SELECT * FROM Rapor Where @RaporId = RaporID";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RaporId", raporId);
                SqlDataReader reader = command.ExecuteReader();

                Rapor rapor = new Rapor();

                while (reader.Read())
                {
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = new Hasta();
                    rapor.Hasta.HastaId = Convert.ToInt32(reader["HastaId"]);
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = new Doktor();
                    rapor.Doktor.DoktorID = Convert.ToInt32(reader["DoktorId"]);
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.Islem = new Islem();
                    rapor.Islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    rapor.URL = reader["URL"].ToString();

                }
                reader.Close();
                connection.Close();

                return rapor;
            }
        }
        public List<Rapor> FindRaporWithIslemId(int islemId)
        {
            string sql = "SELECT * FROM Rapor Where @IslemId = IslemId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@IslemId", islemId);
                SqlDataReader reader = command.ExecuteReader();

                List<Rapor> kayıtlıRaporlar = new List<Rapor>();

                while (reader.Read())
                {
                    Rapor rapor = new Rapor();
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = HomeController.hastaModifier.FindHastaWithId(Convert.ToInt32(reader["HastaId"]));
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = HomeController.doktorModifier.FindDoctorById(Convert.ToInt32(reader["DoktorId"]));
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.Islem = HomeController.islemModifier.FindIslemWithId(Convert.ToInt32(reader["IslemId"]));
                    rapor.URL = reader["URL"].ToString();

                    kayıtlıRaporlar.Add(rapor);
                }
                reader.Close();
                connection.Close();

                return kayıtlıRaporlar;
            }
        }
        public List<Rapor> FindRaporHastaId(int hastaId)
        {
            string sql = "SELECT * FROM Rapor Where @HastaId = HastaId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HastaId", hastaId);
                SqlDataReader reader = command.ExecuteReader();

                List<Rapor> kayıtlıRaporlar = new List<Rapor>();

                while (reader.Read())
                {
                    Rapor rapor = new Rapor();
                    rapor.RaporID = Convert.ToInt32(reader["RaporID"]);
                    rapor.Hasta = HomeController.hastaModifier.FindHastaWithId(Convert.ToInt32(reader["HastaId"]));
                    rapor.İçerik = (reader["İçerik"]).ToString();
                    rapor.Doktor = HomeController.doktorModifier.FindDoctorById(Convert.ToInt32(reader["DoktorId"]));
                    rapor.Tarih = Convert.ToDateTime(reader["Tarih"]);
                    rapor.Islem = HomeController.islemModifier.FindIslemWithId(Convert.ToInt32(reader["IslemId"]));
                    rapor.URL = reader["URL"].ToString();
                    kayıtlıRaporlar.Add(rapor);
                }
                reader.Close();
                connection.Close();

                return kayıtlıRaporlar;
            }
        }
        public int Update(Rapor data)
        {
            int returned = 0;

            using (SqlConnection conn = CreateConnection())
            {
                string sql = "Update Rapor SET HastaId = @HastaId,IslemId = @Islem,URL = @URL ,Tarih = @TARIH ,İçerik = @ACIKLAMA,DoktorId = @DoktorId where RaporID = @RaporId";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RaporId", data.RaporID);
                cmd.Parameters.AddWithValue("@HastaId", data.Hasta.HastaId);
                cmd.Parameters.AddWithValue("@DoktorId", data.Doktor.DoktorID);
                cmd.Parameters.AddWithValue("@ACIKLAMA", data.İçerik);
                cmd.Parameters.AddWithValue("@TARIH", data.Tarih);
                cmd.Parameters.AddWithValue("@Islem", data.Islem.IslemId);
                cmd.Parameters.AddWithValue("@URL", data.URL);
                returned = cmd.ExecuteNonQuery();
                conn.Close();
                raporDatabase = ToList();
            }
            return returned;
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
    }
}