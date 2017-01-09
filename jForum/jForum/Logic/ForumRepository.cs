using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class ForumRepository
    {
        IForumContext context;

        public ForumRepository(IForumContext context)
        {
            this.context = context;
        }

        void Validate(ForumModel forum)
        {
            new ValidateString(forum.Name, 3, 50, "Forum name");
            new ValidateString(forum.Description, 10, 200, "Forum description");
        }

        public ForumModel Create(ForumModel forum)
        {
            Validate(forum);
            forum.Id = context.Create(forum);
            return forum;
        }

        public Dictionary<int, ForumModel> Read()
        {
            Dictionary<int, ForumModel> forums = context.Read();
            if(forums.Count() > 0)
            {
                return forums;
            }
            throw new NotFoundException();
        }

        public ForumModel Read(int id)
        {
            ForumModel forum = context.Read(id);
            if(forum != null)
            {
                return forum;
            }
            throw new NotFoundException();
        }

        public void Update(ForumModel forum)
        {
            Validate(forum);
            if(forum.Id == 0)
            {
                throw new InvalidValueException("Forum id is missing.");
            }
            if (!context.Update(forum))
            {
                throw new NotFoundException();
            }
        }

        public void Delete(int id)
        {
            if(!context.Delete(id))
            {
                throw new NotFoundException();
            }
        }
    }
}