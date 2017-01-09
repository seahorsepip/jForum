using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace jForum.Data
{
    public class PostSQLContext : IPostContext
    {
        public int Create(PostModel post, int userId)
        {
            int id = 0;
            string query = @"INSERT INTO [Post]
                             (UserId, TopicId, Content, Date)
                             VALUES(@UserId, @TopicId, @Content, GETDATE());
                             SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@TopicId", post.Topic.Id);
                cmd.Parameters.AddWithValue("@Content", post.Content);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        public PostModel Read(int id)
        {
            PostModel post = null;
            string query = @"SELECT [Post].Content, [Post].Date, [User].Id, [User].Name, [Post].TopicId, [PostReply].ReplyPostId
                             FROM [Post]
                             JOIN [User] ON [Post].UserId = [User].Id
                             LEFT JOIN [PostReply] ON [Post].Id = [PostReply].PostId
                             WHERE [Post].Id = @Id;";
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
                            post = new PostModel
                            {
                                Content = reader.GetString(0),
                                Date = reader.GetDateTime(1),
                                User = new UserModel
                                {
                                    Id = reader.GetInt32(2),
                                    Name = reader.GetString(3)
                                },
                                Topic = new TopicModel
                                {
                                    Id = reader.GetInt32(4)
                                },
                                Quotes = new Dictionary<int, PostModel>()
                            };
                            first = false;
                        }
                        else if (!reader.IsDBNull(5))
                        {
                            post.Quotes.Add(reader.GetInt32(5), null);

                        }
                    }
                }
            }
            return post;
        }

        public bool Update(PostModel post, int userId)
        {
            bool success = false;
            string query = @"IF EXISTS(
                                SELECT *
                                FROM [Post]
                                WHERE Id = @Id AND UserId = @UserId
                             )
                             OR [dbo].CheckPermission(@All, @UserId) = 1
                             BEGIN
                                 UPDATE [Post]
                                 SET Content = @Content
                                 WHERE Id = @Id
                             END;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", post.Id);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@All", Permission.UPDATE_ALL_POSTS);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }

        public bool Delete(int id, int userId)
        {
            bool success = false;
            string query = @"IF EXISTS(
                                SELECT *
                                FROM [Post]
                                WHERE Id = @Id AND UserId = @UserId
                             )
                             OR [dbo].CheckPermission(@All, @UserId) = 1
                             BEGIN
                                 DELETE FROM [Post]
                                 WHERE Id = @Id;
                             END;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@All", Permission.DELETE_ALL_POSTS);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
    }
}