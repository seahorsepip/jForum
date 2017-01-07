using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IForumContext
    {
        ForumModel Read(int id);
    }
}