using CryptSharp;
using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class UserRepository
    {
        IUserContext context;

        public UserRepository(IUserContext context)
        {
            this.context = context;
        }

        public UserModel Create(UserModel user)
        {
            user.Password = Crypter.Blowfish.Crypt(user.Password);
            context.Create(user);
            user.Password = null;
            return user;
        }

        public UserModel Read(int id)
        {
            UserModel user = context.Read(id);
            if(user != null)
            {
                return user;
            }
            throw new NotFoundException();
        }

        public void Update(UserModel user)
        {
            user.Password = Crypter.Blowfish.Crypt(user.Password);
            if (!context.Update(user))
            {
                throw new NotFoundException();
            }
        }

        public void Delete(int id, int userId)
        {
            if (!context.Delete(id, userId))
            {
                throw new NotFoundException();
            }
        }
    }
}