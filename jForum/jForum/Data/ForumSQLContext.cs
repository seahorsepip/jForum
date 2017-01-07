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
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ForumModel Read(int id)
        {
            ForumModel forum = null;
            int count = 0;
            string query = @"SELECT [Forum].Name, [Forum].Description, [Section].Title
                             FROM [Forum]
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

                            };
                            first = false;
                        }
                    }
                }
            }
            return forum;
        }
    }
}