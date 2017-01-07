using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IUserContext
    {
        UserModel Login(string email);
        void Token(int id, string password);
        bool Token(string token, string newToken);
    }
}