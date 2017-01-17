using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace jForum.Data
{
    public class TopicSQLContext : ITopicContext
    {
        public int Create(TopicModel topic, int userId)
        {
            int id = 0;
            string query = @"INSERT INTO [Topic]
                             (SectionId, Title)
                             VALUES(@SectionId, @Title);
                             INSERT INTO [Post]
                             (UserId, TopicId, Content, Date)
                             VALUES(@UserId, SCOPE_IDENTITY(), @Content, GETDATE());";
            string query2 = @"SELECT TopicId
                             FROM [Post]
                             WHERE Id = @Id;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@SectionId", topic.SectionId);
                cmd.Parameters.AddWithValue("@Title", topic.Title);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Content", topic.Content);
                id = (int)cmd.ExecuteScalar();
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query2, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                id = (int)cmd.ExecuteScalar();
            }
            return id;
        }

        public TopicModel Read(int id, PagedModel page)
        {
            TopicModel topic = null;
            string query = @"SELECT *
                             FROM
                             (
                                 SELECT [Topic].Title, [Post].Content, [Post].Date, [User].Id AS UserId, [User].Name, [Post].Id AS PostId, [PostReply].ReplyPostId, ((Count(*) Over()) - 1) AS Count
                                 FROM [Topic]
                                 JOIN [Post] ON [Topic].Id = [Post].TopicId
                                 JOIN [User] ON [Post].UserId = [User].Id
                                 LEFT JOIN [PostReply] ON [Post].Id = [PostReply].PostId
                                 WHERE [Topic].Id = @Id
	                             ORDER BY [Post].Id
	                             OFFSET 0 ROWS
	                             FETCH NEXT 1 ROWS ONLY
                             ) a
                             UNION
                             SELECT *
                             FROM
                             (
	                             SELECT [Topic].Title, [Post].Content, [Post].Date, [User].Id AS UserId, [User].Name, [Post].Id AS PostId, [PostReply].ReplyPostId, 0 AS Count
	                             FROM [Topic]
	                             JOIN [Post] ON [Topic].Id = [Post].TopicId
	                             JOIN [User] ON [Post].UserId = [User].Id
	                             LEFT JOIN [PostReply] ON [Post].Id = [PostReply].PostId
	                             WHERE [Topic].Id = @Id
	                             ORDER BY [Post].Id
	                             OFFSET (@Start + 1) ROWS
	                             FETCH NEXT (@Stop - @Start - 1) ROWS ONLY
                             ) b
                             ORDER BY PostId";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Start", page.Start);
                cmd.Parameters.AddWithValue("@Stop", page.Stop);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (reader.Read())
                    {
                        if (first)
                        {
                            page.Data = new Dictionary<int, PostModel>();
                            page.Count = reader.GetInt32(7);
                            topic = new TopicModel
                            {
                                Title = reader.GetString(0),
                                Content = reader.GetString(1),
                                Date = reader.GetDateTime(2),
                                User = new UserModel
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(4)
                                },
                                Quotes = new Dictionary<int, PostModel>(),
                                Posts = page
                            };
                            first = false;
                        }
                        else
                        {
                            int postId = reader.GetInt32(5);
                            PostModel post = null;
                            Dictionary<int, PostModel> posts = (Dictionary<int, PostModel>)topic.Posts.Data;
                            if (posts.ContainsKey(postId))
                            {
                                post = posts[id];
                            }
                            else
                            {
                                post = new PostModel
                                {
                                    Content = reader.GetString(1),
                                    Date = reader.GetDateTime(2),
                                    User = new UserModel
                                    {
                                        Id = reader.GetInt32(3),
                                        Name = reader.GetString(4)
                                    },
                                    Quotes = new Dictionary<int, PostModel>()
                                };
                            }
                            if (!reader.IsDBNull(6))
                            {
                                post.Quotes.Add(reader.GetInt32(6), null);

                            }
                            posts[postId] = post;
                            topic.Posts.Data = posts;
                        }
                    }
                }
            }
            return topic;
        }
        
        public bool Update(TopicModel topic, int userId)
        {
            bool success = false;
            string query = @"IF EXISTS(
                                SELECT *
                                FROM [Post]
                                WHERE UserId = @UserId AND Id = (
                                   SELECT TOP 1 Id
                                   FROM [Post]
                                   WHERE TopicId = @Id
                                )
                             )
                             OR [dbo].CheckPermission(@All, @UserId) = 1
                             BEGIN
                                UPDATE [Topic]
                                SET SectionId = @SectionId, @Title = @Title
                                WHERE Id = @Id;
                                UPDATE [Post]
                                SET Content = @Content
                                WHERE Id = (
                                   SELECT TOP 1 Id
                                   FROM [Post]
                                   WHERE TopicId = @Id
                                );
                             END;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", topic.Id);
                cmd.Parameters.AddWithValue("@SectionId", topic.SectionId);
                cmd.Parameters.AddWithValue("@Content", topic.Content);
                cmd.Parameters.AddWithValue("@Title", topic.Title);
                cmd.Parameters.AddWithValue("@All", Permission.UPDATE_ALL_TOPIC);
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
                                WHERE UserId = @UserId AND Id = (
                                    SELECT TOP 1 Id
                                    FROM [Post]
                                    WHERE TopicId = @Id
                                 )
                             )
                             OR [dbo].CheckPermission(@All, @UserId) = 1
                             BEGIN
                                 DELETE FROM [Topic]
                                 WHERE Id = @Id;
                             END;";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@All", Permission.DELETE_ALL_TOPIC);
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
    }
}