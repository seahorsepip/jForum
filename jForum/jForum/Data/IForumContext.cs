using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IForumContext
    {
        int Create(ForumModel forum);
        Dictionary<int, ForumModel> Read();
        ForumModel Read(int id);
        bool Update(ForumModel forum);
        bool Delete(int id);
    }
}