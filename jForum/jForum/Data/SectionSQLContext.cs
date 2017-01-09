using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace jForum.Data
{
    public class SectionSQLContext : ISectionContext
    {
        public int Create(SectionModel section)
        {
            int id = 0;
            string query = @"INSERT INTO [Section]
                             (Title, Description, ParentSectionId)
                             VALUES(@Title, @Description, @ParentSectionId);
                             SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Title", section.Title);
                cmd.Parameters.AddWithValue("@Description", section.Description);
                cmd.Parameters.AddWithValue("@ParentSectionId", section.ParentSection);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        public SectionModel Read(int id)
        {
            SectionModel section = null;
            string query = @"SELECT [Section].Title, [Section].Description, ChildSection.Id, ChildSection.Title, ChildSection.Description, [Topic].Id, [Topic].Title
                             FROM [Section]
                             LEFT JOIN [Topic] ON [Section].Id = [Topic].SectionId
                             LEFT JOIN [Section] ChildSection ON [Section].Id = ChildSection.ParentSectionId
                             WHERE [Section].Id = @Id;";
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
                            section = new SectionModel
                            {
                                Title = reader.GetString(0),
                                Description = reader.GetString(1),
                                Sections = new Dictionary<int, SectionModel>(),
                                Topics = new Dictionary<int, TopicModel>()
                            };
                            first = false;
                        }
                        if(!reader.IsDBNull(2))
                        {
                            section.Sections[reader.GetInt32(2)] = new SectionModel
                            {
                                Title = reader.GetString(3),
                                Description = reader.GetString(4)
                            };
                        }
                        if (!reader.IsDBNull(5))
                        {
                            section.Sections[reader.GetInt32(5)] = new SectionModel
                            {
                                Title = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return section;
        }

        public bool Update(SectionModel section)
        {
            bool success = false;
            string query = @"UPDATE [Section]
                             SET Title = @Title, Description = Description, ParentSectionId = @ParentSectionId
                             VALUES(@Title, @Description, @ParentSectionId);";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Title", section.Title);
                cmd.Parameters.AddWithValue("@Description", section.Description);
                cmd.Parameters.AddWithValue("@ParentSectionId", section.ParentSection);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }

        public bool Delete(int id)
        {
            bool success = false;
            string query = @"DELETE FROM [Section]
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