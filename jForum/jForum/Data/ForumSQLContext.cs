using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace jForum.Data
{
    public class ForumSQLContext : IForumContext
    {
        public int Create(ForumModel forum)
        {
            int id = 0;
            string query = @"INSERT INTO [Forum]
                             (Name, Description)
                             VALUES(@Name, @Description);
                             SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Name", forum.Name);
                cmd.Parameters.AddWithValue("@Description", forum.Description);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        public Dictionary<int, ForumModel> Read()
        {
            Dictionary<int, ForumModel> forums = new Dictionary<int, ForumModel>();
            string query = @"SELECT Id, Name, Description
                             FROM [Forum];";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        forums.Add(reader.GetInt32(0), new ForumModel
                        {
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }
            return forums;
        }

        public ForumModel Read(int id)
        {
            ForumModel forum = null;
            string query = @"SELECT [Forum].Name, [Forum].Description, [Section].Id, [Section].Title, [Section].Description
                             FROM [Forum]
                             LEFT JOIN [Section] ON [Forum].Id = [Section].ForumId AND [Section].ParentSectionId IS NULL
                             WHERE [Forum].Id = @Id;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (reader.Read())
                    {
                        if (first)
                        {
                            forum = new ForumModel
                            {
                                Name = reader.GetString(0),
                                Description = reader.GetString(1),
                                Sections = new Dictionary<int, SectionModel>()
                            };
                            first = false;
                        }
                        if(!reader.IsDBNull(2))
                        {
                            forum.Sections.Add(reader.GetInt32(2), new SectionModel
                            {
                                Title = reader.GetString(3),
                                Description = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return forum;
        }

        public bool Update(ForumModel forum)
        {
            bool success = false;
            string query = @"UPDATE [Forum]
                             SET Name = @Name, Description = @Description
                             WHERE Id = @Id;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", forum.Id);
                cmd.Parameters.AddWithValue("@Name", forum.Name);
                cmd.Parameters.AddWithValue("@Description", forum.Description);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = false;
            string query = @"DELETE FROM [Forum]
                             WHERE Id = @Id;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
    }
}