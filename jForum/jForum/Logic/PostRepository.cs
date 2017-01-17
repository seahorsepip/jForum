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
        IPostContext context;

        public PostRepository(IPostContext context)
        {
            this.context = context;
        }

        public PostModel Create(PostModel post, int userId)
        {
            post.Id = context.Create(post, userId);
            return post;
        }

        public PostModel Read(int id)
        {
            PostModel post = context.Read(id);
            if(post != null)
            {
                return post;
            }
            throw new NotFoundException();
        }

        public void Update(PostModel post, int userId)
        {
            if (post.Id == 0)
            {
                throw new InvalidModelException("post.Id", "The Id field is required.");
            }
            if (!context.Update(post, userId))
            {
                throw new NotFoundException();
            }
        }

        public void Delete(int id, int userId)
        {
            if(!context.Delete(id, userId))
            {
                throw new NotFoundException();
            }
        }
    }
}