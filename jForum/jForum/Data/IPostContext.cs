using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IPostContext
    {
        int Create(PostModel post, int userId);
        PostModel Read(int id);
        bool Update(PostModel post, int userId);
        bool Delete(int id, int userId);
    }
}