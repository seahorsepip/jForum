using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IPostContext
    {
        List<PostModel> Get(); //Get all posts
        List<PostModel> Get(ForumModel forum); //Get all posts in specific forum
        List<PostModel> Get(TopicModel topic); //Get all posts in specific topic
        List<PostModel> Get(PostModel post); //Get all posts that quote a specific post
    }
}