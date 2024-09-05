using prolab_THEEND.Controllers;
using prolab_THEEND.Models;
using prolab_THEEND.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Commands
{
    public class IslemCommand : RepoCommand
    {
        public List<Islem> IslemDatabase = new List<Islem>();
        public int Insert(Islem data)
        {
            int durum = 0;
            string sql = "Insert into Islem Values(@HastaId,@RandevuId,@DoktorId,@Rapor)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Rapor", data.TıbbiRaporId);
                cmd.Parameters.AddWithValue("@RandevuId", data.RandevuId);
                cmd.Parameters.AddWithValue("@HastaId", data.HastaId);
                cmd.Parameters.AddWithValue("@DoktorId", data.DoktorId);

                durum = cmd.ExecuteNonQuery();
                conn.Close();
                IslemDatabase.Add(data);
            }
            IslemDatabase = ToList();
            return durum;
        }
        public void Delete(Islem data)
        {
            string sql = "Delete from Islem Where RandevuId = @RandevuId";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                List<Rapor> list = HomeController.raporModifier.FindRaporWithIslemId(data.IslemId);
                foreach (Rapor rapor in list)
                    HomeController.raporModifier.Delete(rapor);
                cmd.Parameters.AddWithValue("@RandevuId", data.RandevuId);
                cmd.ExecuteNonQuery();
                conn.Close();
                IslemDatabase = ToList();
            }
        }
        public List<Islem> ToList()
        {
            IslemDatabase.Clear();

            string sql = "SELECT * FROM Islem";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Islem islem = new Islem();
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]);
                    IslemDatabase.Add(islem);
                }
                reader.Close();
                connection.Close();
            }
            return IslemDatabase;
        }
        public List<Islem> FindIslem(string hastaTC)
        {
            Hasta hasta = HomeController.hastaModifier.FindHastaWithTC(hastaTC);

            string sql = "SELECT * FROM Islem Where @HastaId = HastaId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HastaId", hasta.HastaId);
                SqlDataReader reader = command.ExecuteReader();

                List<Islem> kayıtlıIslemler = new List<Islem>();

                while (reader.Read())
                {
                    Islem islem = new Islem();
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]); kayıtlıIslemler.Add(islem);
                }
                reader.Close();
                connection.Close();

                return kayıtlıIslemler;
            }
        }
        public List<Islem> FindIslemDocId(int DoktorId)
        {

            string sql = "SELECT * FROM Islem Where @DoktorId = DoktorId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DoktorId", DoktorId);
                SqlDataReader reader = command.ExecuteReader();

                List<Islem> kayıtlıIslemler = new List<Islem>();

                while (reader.Read())
                {
                    Islem islem = new Islem();
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]); kayıtlıIslemler.Add(islem);
                }
                reader.Close();
                connection.Close();

                return kayıtlıIslemler;
            }
        }
        public Islem FindIslem(int randevuId)
        {
            string sql = "SELECT * FROM Islem Where @RandevuId = RandevuId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RandevuId", randevuId);
                SqlDataReader reader = command.ExecuteReader();

                Islem islem = new Islem();

                while (reader.Read())
                {
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]);
                }
                reader.Close();
                connection.Close();

                return islem;
            }
        }
        public Islem FindIslemWithId(int IslemId)
        {
            string sql = "SELECT * FROM Islem Where @IslemId = IslemId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@IslemId", IslemId);
                SqlDataReader reader = command.ExecuteReader();

                Islem islem = new Islem();

                while (reader.Read())
                {
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]);
                }
                reader.Close();
                connection.Close();

                return islem;
            }
        }
        public List<Islem> FindIslemHastaId(int hastaId)
        {
            string sql = "SELECT * FROM Islem Where @HastaId = HastaId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HastaId", hastaId);
                SqlDataReader reader = command.ExecuteReader();

                List<Islem> kayıtlıIslemler = new List<Islem>();

                while (reader.Read())
                {
                    Islem islem = new Islem();
                    islem.IslemId = Convert.ToInt32(reader["IslemId"]);
                    islem.HastaId = Convert.ToInt32(reader["HastaId"]);
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.DoktorId = Convert.ToInt32(reader["DoktorId"]);
                    if (!reader.IsDBNull(4))
                        islem.TıbbiRaporId = Convert.ToInt32(reader["TıbbiRaporId"]);

                    kayıtlıIslemler.Add(islem);
                }
                reader.Close();
                connection.Close();

                return kayıtlıIslemler;
            }
        }
        public int Update(Islem data)
        {
            int returned = 0;

            using (SqlConnection conn = CreateConnection())
            {
                string sql = "Update Islem SET HastaId = @HastaId, TıbbiRaporId = @Rapor ,RandevuId = @RandevuId,DoktorId = @DoktorId where IslemId = @IslemId";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RandevuId", data.RandevuId);
                cmd.Parameters.AddWithValue("@HastaId", data.HastaId);
                cmd.Parameters.AddWithValue("@DoktorId", data.DoktorId);
                cmd.Parameters.AddWithValue("@IslemId", data.IslemId);
                cmd.Parameters.AddWithValue("@Rapor", data.TıbbiRaporId);

                returned = cmd.ExecuteNonQuery();
                conn.Close();
                IslemDatabase = ToList();
            }
            return returned;
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
    }
}