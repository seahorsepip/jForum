using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface ITokenContext
    {
        UserModel Login(string email);
        void Create(int id, string password);
        int Read(string token);
        bool Update(string token, string newToken);
        bool Permission(Permission permission, int userId);
    }
}