using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;
using jForum.Logic;

namespace jForum.Data
{
    public class UserSQLContext : IUserContext
    {
        public void Create(UserModel user)
        {
            try
            {
                string query = @"INSERT INTO [User]
                             (Name, Email, Password)
                             VALUES(@Name, @Email, @Password);";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(SqlException e)
            {
                if (e.Errors[0].Number == 2627)
                {
                    throw new InvalidModelException("user.Email", "An account with this email address already exists.");
                }
                throw;
            }
        }

        public UserModel Read(int id)
        {
            UserModel user = null;
            string query = @"SELECT Name, Email
                             FROM [User]
                             WHERE Id = @Id;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            Name = reader.GetString(0),
                            Email = reader.GetString(1)
                        };
                    }
                }
            }
            return user;
        }

        public bool Update(UserModel user)
        {
            try
            {
                bool success = false;
                string query = @"UPDATE [User]
                             SET Name = @Name, Email = @Email, Password = @Password
                             WHERE Id = @Id";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    success = cmd.ExecuteNonQuery() > 0;
                }
                return success;
            }
            catch (SqlException e)
            {
                if (e.Errors[0].Number == 2627)
                {
                    throw new InvalidModelException("user.Email", "An account with this email address already exists.");
                }
                throw;
            }
        }

        public bool Delete(int id, int userId)
        {
            bool success = false;
            string query = @"IF @Id = @UserId OR [dbo].CheckPermission(@All, @UserId) = 1
                             BEGIN
                                 DELETE FROM [User]
                                 WHERE Id = @Id;
                             END;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Parameters.AddWithValue("@All", Permission.DELETE_ALL_USERS);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
    }
}