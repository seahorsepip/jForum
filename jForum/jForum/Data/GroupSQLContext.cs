using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using jForum.Logic;

namespace jForum.Data
{
    public class GroupSQLContext : IGroupContext
    {
        DataTable Permissions(List<Permission> permissions)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            if (permissions != null)
            {
                foreach (int id in permissions)
                {
                    table.Rows.Add(id);
                }
            }
            return table;
        }

        public int Create(GroupModel group)
        {
            try
            {
                int id = 0;
                string query = "EXEC [dbo].CreateGroup @ForumId, @Name, @Description, @Permissions;";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ForumId", group.ForumId);
                    cmd.Parameters.AddWithValue("@Name", group.Name);
                    cmd.Parameters.AddWithValue("@Description", group.Description);
                    SqlParameter permissionsParameter = cmd.Parameters.AddWithValue("@Permissions", Permissions(group.Permissions));
                    permissionsParameter.TypeName = "[dbo].Permission";
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return id;
            }
            catch (SqlException e)
            {
                if (e.Errors[0].Number == 547)
                {
                    throw new InvalidModelException("group.ForumId", "The ForumId field is invalid.");
                }
                throw;
            }
        }

        public Dictionary<int, GroupModel> Read()
        {
            Dictionary<int, GroupModel> groups = new Dictionary<int, GroupModel>();
            string query = @"SELECT Id, ForumId, Name, Description
                             FROM [Group];";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(reader.GetInt32(0), new GroupModel
                        {
                            ForumId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Description = reader.GetString(3)
                        });
                    }
                }
            }
            return groups;
        }

        public GroupModel Read(int id)
        {
            GroupModel group = null;
            string query = @"SELECT [Group].ForumId, [Group].Name, [Group].Description, [GroupPermission].Permission
                             FROM [Group]
                             LEFT JOIN [GroupPermission] ON [Group].Id = [GroupPermission].GroupId
                             WHERE [Group].Id = @Id;";
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
                            group = new GroupModel
                            {
                                ForumId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Permissions = new List<Permission>()
                            };
                            first = false;
                        }
                        if(!reader.IsDBNull(3))
                        {
                            group.Permissions.Add((Permission)reader.GetInt32(3));
                        }
                    }
                }
            }
            return group;
        }

        public bool Update(GroupModel group)
        {
            try
            {
                bool success = false;
                string query = "EXEC [dbo].UpdateGroup @Id, @ForumId, @Name, @Description, @Permissions;";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", group.Id);
                    cmd.Parameters.AddWithValue("@ForumId", group.ForumId);
                    cmd.Parameters.AddWithValue("@Name", group.Name);
                    cmd.Parameters.AddWithValue("@Description", group.Description);
                    SqlParameter permissionsParameter = cmd.Parameters.AddWithValue("@Permissions", Permissions(group.Permissions));
                    permissionsParameter.TypeName = "[dbo].Permission";
                    success = cmd.ExecuteNonQuery() > 0;
                }
                return success;
            }
            catch (SqlException e)
            {
                if (e.Errors[0].Number == 547)
                {
                    throw new InvalidModelException("group.Id", "The Id and/or ForumId field is invalid.");
                }
                throw;
            }
        }

        public bool Delete(int id)
        {
            bool success = false;
            string query = @"DELETE FROM [Group]
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