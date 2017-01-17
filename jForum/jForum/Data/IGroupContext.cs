using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IGroupContext
    {
        int Create(GroupModel group);
        Dictionary<int, GroupModel> Read();
        GroupModel Read(int id);
        bool Update(GroupModel group);
        bool Delete(int id);
    }
}