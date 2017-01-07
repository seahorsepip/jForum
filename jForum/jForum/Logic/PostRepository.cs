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

        public PagedModel Read(int topicId, PagedModel page)
        {
            return postContext.Read(topicId, page);
        }

        public PostModel Create(PostModel post)
        {
            post.Id = postContext.Create(post);
            return post;
        }

        public bool Delete(int id)
        {
            return postContext.Delete(id);
        }
    }
}