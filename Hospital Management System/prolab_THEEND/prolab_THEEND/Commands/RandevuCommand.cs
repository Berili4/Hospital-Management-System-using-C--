using prolab_THEEND.Controllers;
using prolab_THEEND.Models;
using prolab_THEEND.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace prolab_THEEND.Commands
{
    public class RandevuCommand : RepoCommand
    {
        public List<Randevu> RandevuDatabase = new List<Randevu>();
        public void Insert(Randevu data)
        {
            string sql = "Insert into Randevu Values(@RandevuTarih,@Hasta,@Doktor)";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RandevuTarih", data.RandevuTarih);
                cmd.Parameters.AddWithValue("@Hasta", data.Hasta.HastaId);
                cmd.Parameters.AddWithValue("@Doktor", data.Doktor.DoktorID);

                cmd.ExecuteNonQuery();

                RandevuDatabase = ToList();
                conn.Close();
            }
        }
        public int Delete(Randevu data)
        {
            string sql = "Delete from Randevu Where RandevuId = @RandevuId";
            using (SqlConnection conn = CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RandevuId", data.RandevuId);
                HomeController.islemModifier.Delete(HomeController.islemModifier.FindIslem(data.RandevuId));  // to be continued
                int returned = cmd.ExecuteNonQuery();
                conn.Close();
                RandevuDatabase = ToList();
                return returned;
            }
        }
        public Randevu FindRandevu(int RandevuId)
        {

            string sql = "SELECT * FROM Randevu Where @RandevuId = RandevuId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RandevuId", RandevuId);
                SqlDataReader reader = command.ExecuteReader();

                Randevu islem = new Randevu();

                while (reader.Read())
                {
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.Hasta = new Hasta();
                    islem.Hasta.HastaId = Convert.ToInt32(reader["Hasta_HastaId"]);
                    islem.RandevuTarih = Convert.ToDateTime(reader["RandevuTarih"]);
                    islem.Doktor = new Doktor();
                    islem.Doktor.DoktorID = Convert.ToInt32(reader["Doktor_DoktorId"]);
                }
                reader.Close();
                connection.Close();

                return islem;
            }
        }
        public List<Randevu> FindRandevuAsList(string hastaTC)
        {
            Hasta hasta = HomeController.hastaModifier.FindHastaWithTC(hastaTC);

            string sql = "SELECT * FROM Randevu Where @HastaId = Hasta_HastaId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HastaId", hasta.HastaId);
                SqlDataReader reader = command.ExecuteReader();

                List<Randevu> kayıtlıRandevular = new List<Randevu>();

                while (reader.Read())
                {
                    Randevu islem = new Randevu();
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.Hasta = new Hasta();
                    islem.Hasta.HastaId = Convert.ToInt32(reader["Hasta_HastaId"]);
                    islem.RandevuTarih = Convert.ToDateTime(reader["RandevuTarih"]);
                    kayıtlıRandevular.Add(islem);
                }
                reader.Close();
                connection.Close();

                return kayıtlıRandevular;
            }
        }
        public List<Randevu> FindRandevuAsListDoc(int doktorId)
        {

            string sql = "SELECT * FROM Randevu Where @DoktorId = Doktor_DoktorId";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DoktorId", doktorId);
                SqlDataReader reader = command.ExecuteReader();

                List<Randevu> kayıtlıRandevular = new List<Randevu>();

                while (reader.Read())
                {
                    Randevu islem = new Randevu();
                    islem.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    islem.Hasta = new Hasta();
                    islem.Hasta.HastaId = Convert.ToInt32(reader["Hasta_HastaId"]);
                    islem.RandevuTarih = Convert.ToDateTime(reader["RandevuTarih"]);
                    kayıtlıRandevular.Add(islem);
                }
                reader.Close();
                connection.Close();

                return kayıtlıRandevular;
            }
        }
        public List<Randevu> WhereGetAll(string SQLQUERY, string[] parameters, string[] parameterValues , DateTime Tarih)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(SQLQUERY, connection);

                for (int i = 0; i < parameters.Length; i++)
                {
                    if(i != parameters.Length - 1)
                        command.Parameters.AddWithValue(parameters[i], parameterValues[i]);
                    else
                        command.Parameters.AddWithValue(parameters[i], Tarih);
                }

                SqlDataReader reader = command.ExecuteReader();

                List<Randevu> tempList = new List<Randevu>();
                Randevu randevu = new Randevu();

                while (reader.Read())
                {
                    randevu.Hasta = new Hasta();
                    randevu.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    randevu.Hasta.HastaId = Convert.ToInt32(reader["Hasta_HastaId"]);
                    randevu.RandevuTarih = Convert.ToDateTime(reader["RandevuTarih"]);
                    randevu.Doktor = new Doktor();
                    randevu.Doktor.DoktorID = Convert.ToInt32(reader["Doktor_DoktorId"]);

                    tempList.Add(randevu);
                }
                reader.Close();
                connection.Close();
                return tempList;
            }

        }
        public List<Randevu> ToList()
        {
            RandevuDatabase.Clear();


            string sql = "SELECT * FROM Randevu";

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Randevu randevu = new Randevu();
                    randevu.RandevuId = Convert.ToInt32(reader["RandevuId"]);
                    randevu.RandevuTarih = Convert.ToDateTime(reader["RandevuTarih"]);
                    randevu.Hasta = new Hasta();
                    randevu.Hasta.HastaId = Convert.ToInt32(reader["Hasta_HastaId"]);
                    randevu.Doktor = new Doktor();
                    randevu.Doktor.DoktorID = Convert.ToInt32(reader["Doktor_DoktorId"]);
                    MatchTheIll(randevu);
                    RandevuDatabase.Add(randevu);
                }
                reader.Close();
                connection.Close();
            }
            return RandevuDatabase;
        }
        public void MatchTheIll(Randevu randevu)
        {
            foreach (Hasta hasta in HomeController.hastaModifier.HastaDatabase)
                if (randevu.Hasta.HastaId == hasta.HastaId)
                {
                    randevu.Hasta = hasta;
                    return;
                }
        }
        public int Update(Randevu data)
        {
            int returned = 0;

            using (SqlConnection conn = CreateConnection())
            {
                string sql = "Update Randevu SET Hasta_HastaId = @Hasta,RandevuTarih = @RandevuTarih,Doktor_DoktorId = @Doktor where RandevuId = @RandevuId";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RandevuTarih", data.RandevuTarih);
                cmd.Parameters.AddWithValue("@Hasta", data.Hasta.HastaId);
                cmd.Parameters.AddWithValue("@RandevuId", data.RandevuId);
                cmd.Parameters.AddWithValue("@Doktor", data.Doktor.DoktorID);

                returned = cmd.ExecuteNonQuery();
                conn.Close();
                foreach (var randevu in RandevuDatabase)
                    if(randevu.RandevuId ==  data.RandevuId)
                        randevu.RandevuTarih = data.RandevuTarih;
            }
            return returned;
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionAddress);
        }
    }
}