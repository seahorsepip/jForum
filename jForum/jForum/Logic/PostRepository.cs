using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class PostRepository
    {
        IPostContext postContext;

        public PostRepository(IPostContext postContext)
        {
            this.postContext = postContext;
        }

        public List<PostModel> Get()
        {
            return postContext.Get();
        }

        public List<PostModel> Get(PostModel post)
        {
            return postContext.Get(post);
        }

        public List<PostModel> Get(TopicModel topic)
        {
            return postContext.Get(topic);
        }

        public List<PostModel> Get(ForumModel forum)
        {
            return postContext.Get(forum);
        }
    }
}