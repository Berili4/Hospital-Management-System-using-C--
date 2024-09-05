using prolab_THEEND.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Commands
{
    public class YoneticiCommand : RepoCommand
    {
        public List<Yönetici> YöneticiDatabase = new List<Yönetici>();
        public void Insert(Yönetici data)
        {
            string sql = "Insert into Yönetici Values(@YoneticiID,@Ad,@SoyAd,@Şifre)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void Delete(Yönetici data)
        {
            string sql = "Delete from Doktor Where YoneticiID = @Id";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", data.YoneticiID);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Yönetici> ToList()
        {
            if (YöneticiDatabase.Count != 0)
                return YöneticiDatabase;

            string sql = "SELECT * FROM Yönetici";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Yönetici yönetici = new Yönetici();
                    yönetici.YoneticiID = Convert.ToInt32(reader["YoneticiId"]);
                    yönetici.Ad = reader["Ad"].ToString();
                    yönetici.Şifre = reader["Şifre"].ToString();
                    yönetici.SoyAd = reader["SoyAd"].ToString();


                    YöneticiDatabase.Add(yönetici);
                }
                reader.Close();
                connection.Close();
            }
            return YöneticiDatabase;
        }
        public List<Yönetici> WhereGetAll(string SQLQUERY, string[] parameters, string[] parameterValues)
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

                List<Yönetici> tempList = new List<Yönetici>();

                while (reader.Read())
                {
                    Yönetici yönetici = new Yönetici();
                    yönetici.YoneticiID = Convert.ToInt32(reader["YoneticiID"]);
                    yönetici.Ad = reader["Ad"].ToString();
                    yönetici.Şifre = reader["Şifre"].ToString();
                    yönetici.SoyAd = reader["SoyAd"].ToString();

                    // Doktor nesnesini listeye ekle
                    tempList.Add(yönetici);
                }
                reader.Close();
                connection.Close();
                return tempList;

            }
        }
        public int Update(Yönetici data)
        {
            int returned = 0;

            using (SqlConnection conn = CreateConnection())
            {
                string sql = "Update Yönetici SET Ad = @AD, SoyAd = @SOYAD,Şifre = @SIFRE where YoneticiID = @ID";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AD", data.Ad);
                cmd.Parameters.AddWithValue("@SOYAD", data.SoyAd);
                cmd.Parameters.AddWithValue("@SIFRE", data.Şifre);
                cmd.Parameters.AddWithValue("@ID", data.YoneticiID);

                returned = cmd.ExecuteNonQuery();
                conn.Close();
                YöneticiDatabase = ToList();
            }
            return returned;
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
    }
}