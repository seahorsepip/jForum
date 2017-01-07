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
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public PagedModel Read(int topicId, PagedModel page)
        {
            Dictionary<int, PostModel> posts = new Dictionary<int, PostModel>();
            int count = 0;
            string query = @"SELECT [Post].Id, [Post].UserId, [Post].TopicId, [Post].Content, [Post].Date, [PostReply].ReplyPostId, Count(*) Over() AS Count
                             FROM [Post]
                             LEFT JOIN [PostReply] ON [Post].Id = [PostReply].PostId
                             WHERE [Post].TopicId = @Id
                             ORDER BY [Post].Id
                             OFFSET @Start ROWS
                             FETCH NEXT (@Stop - @Start) ROWS ONLY;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", topicId);
                cmd.Parameters.AddWithValue("@Start", page.Start);
                cmd.Parameters.AddWithValue("@Stop", page.Stop);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        PostModel post;
                        if (posts.ContainsKey(id))
                        {
                            post = posts[id];
                        }
                        else
                        {
                            post = new PostModel
                            {
                                User = new UserModel
                                {
                                    Id = reader.GetInt32(1)
                                },
                                Topic = new TopicModel
                                {
                                    Id = reader.GetInt32(2)
                                },
                                Content = reader.GetString(3),
                                Date = reader.GetDateTime(4),
                                Quotes = new Dictionary<int, PostModel>()
                            };
                        }
                        if (!reader.IsDBNull(5))
                        {
                            post.Quotes.Add(reader.GetInt32(5), null);

                        }
                        posts[id] = post;
                        if (first)
                        {
                            count = reader.GetInt32(6);
                            first = false;
                        }
                    }
                }
            }
            page.Count = count;
            page.Data = posts;
            return page;
        }

        public int Create(PostModel post)
        {
            int id = 0;
            string query = @"INSERT INTO [Post]
                             (UserId, TopicId, Content, Date)
                             VALUES(@UserId, @TopicId, @Content, @Date);
                             SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@UserId", post.User.Id);
                cmd.Parameters.AddWithValue("@TopicId", post.Topic.Id);
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@Date", post.Date);
                id = (int)cmd.ExecuteScalar();
            }
            return id;
        }

        public bool Delete(int id)
        {
            bool success = false;
            string query = @"DELETE FROM [PostReply]
                             WHERE PostId = @Id OR ReplyPostId = @Id;
                             DELETE FROM [Post]
                             WHERE Id = @Id";
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