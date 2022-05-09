using SampleMvcProject.Classes;
using SampleMvcProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SampleMvcProject.Database
{
    public class UserService
    {
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_GET_USER_BY_USERNAME";
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        users.Add(new User
                        {
                            Id = (int)item["Id"],
                            Name = (string)item["Name"]

                        });
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return users;
        }


        public User GetUserById(int id)
        {
            var user = new User();

            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_GET_USER_BY_USERNAME";
                cmd.Parameters.AddWithValue("@ID", id.ToString());
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    user.Id = (int)dt.Rows[0]["Id"];
                    user.Name = (string)dt.Rows[0]["Name"];
                    user.Password = EncryptorDecryptor.Base64Decode((string)dt.Rows[0]["password"]);
                }

            }
            catch (Exception ex)
            {

            }

            return user;
        }
        public bool CreateUser(string name, string password)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_USER_CREATE";
                cmd.Parameters.AddWithValue("@USERNAME", name);
                cmd.Parameters.AddWithValue("@PASSWORD", EncryptorDecryptor.Base64Encode(password));
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool UpdateUser(User user)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_USER_UPDATE ";
                cmd.Parameters.AddWithValue("@USERNAME", user.Name);
                cmd.Parameters.AddWithValue("@PASSWORD", EncryptorDecryptor.Base64Encode(user.Password));
                cmd.Parameters.AddWithValue("@ID", user.Id.ToString());
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_USER_DELETE ";
                cmd.Parameters.AddWithValue("@ID", id.ToString());
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}