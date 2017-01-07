using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace jForum.Data
{
    public class UserSQLContext : IUserContext
    {
        public UserModel Login(string email)
        {
            UserModel user = null;
            string query = @"SELECT Id, Password
                             FROM [User]
                             WHERE Email = @Email;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            Id = reader.GetInt32(0),
                            Password = reader.GetString(1)
                        };
                    }
                }
            }
            return user;
        }

        public void Token(int id, string token)
        {
            string query = @"DELETE FROM [UserToken]
                             WHERE UserId = @UserId;
                             INSERT INTO [UserToken]
                             (UserId, Token, Date)
                             VALUES(@UserId, @Token, GETDATE());";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Token(string token, string newToken)
        {
            bool valid = false;
            string query = @"UPDATE [UserToken]
                             SET Token = @NewToken
                             WHERE Token = @Token;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@NewToken", newToken);
                cmd.Parameters.AddWithValue("@Token", token);
                valid = cmd.ExecuteNonQuery() > 0;
            }
            return valid;
        }
    }
}