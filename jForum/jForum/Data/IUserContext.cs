using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IUserContext
    {
        void Create(UserModel user);
        UserModel Read(int id);
        bool Update(UserModel user);
        bool Delete(int id, int userId);
    }
}