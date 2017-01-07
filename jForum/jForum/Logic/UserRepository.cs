using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace jForum.Logic
{
    public class UserRepository
    {
        IUserContext userContext;

        public UserRepository(IUserContext userContext)
        {
            this.userContext = userContext;
        }

        string Token()
        {
            var random = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            random.GetBytes(randomBytes);
            var token = HttpServerUtility.UrlTokenEncode(randomBytes);
            return token;
        }

        public string Token(string email, string password)
        {
            string token = null;
            UserModel user = userContext.Login(email);
            if (user != null && user.Password == password)
            {
                token = Token();
                userContext.Token(user.Id, token);
            }
            return token;
        }

        public string Token(string token)
        {
            string newToken = Token();
            return userContext.Token(token, newToken) ? newToken : null;
        }
    }
}