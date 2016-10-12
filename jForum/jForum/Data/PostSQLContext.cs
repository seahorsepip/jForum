using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public class PostSQLContext : IPostContext
    {
        public List<PostModel> Get()
        {
            throw new NotImplementedException();
        }

        public List<PostModel> Get(PostModel post)
        {
            throw new NotImplementedException();
        }

        public List<PostModel> Get(TopicModel topic)
        {
            throw new NotImplementedException();
        }

        public List<PostModel> Get(ForumModel forum)
        {
            throw new NotImplementedException();
        }
    }
}