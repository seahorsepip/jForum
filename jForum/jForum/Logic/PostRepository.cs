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

        void Validate(PostModel post)
        {
            new ValidateString(post.Content, 10, 2000, "Post content");
        }

        public PostModel Create(PostModel post, int userId)
        {
            Validate(post);
            if (post.Topic == null  || post.Topic.Id == 0)
            {
                throw new InvalidModelException("Post topic id is missing.");
            }
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
            Validate(post);
            if (post.Id == 0)
            {
                throw new InvalidModelException("Post id is missing.");
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