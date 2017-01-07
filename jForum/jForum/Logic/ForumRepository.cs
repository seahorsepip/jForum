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
        IForumContext forumContext;

        public ForumRepository(IForumContext forumContext)
        {
            this.forumContext = forumContext;
        }

        public ForumModel Read(int id)
        {
            return forumContext.Read(id);
        }
    }
}