using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using jForum.Models;
using CryptSharp;

namespace jForum.Logic
{
    public class TokenRepository
    {
        ITokenContext context;

        public TokenRepository(ITokenContext context)
        {
            this.context = context;
        }

        string Generate()
        {
            var random = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            random.GetBytes(randomBytes);
            var token = HttpServerUtility.UrlTokenEncode(randomBytes);
            return token;
        }

        public string Create(string email, string password)
        {
            string token = null;
            UserModel user = context.Login(email);
            if (user != null && Crypter.CheckPassword(password, user.Password))
            {
                token = Generate();
                context.Create(user.Id, token);
                return token;
            }
            throw new InvalidModelException("user.Email", "Invalid email and/or password.");
        }

        public int Read(string token)
        {
            int id = context.Read(token);
            if (id != 0)
            {
                return id;
            }
            throw new InvalidTokenException();
        }

        public string Update(string token)
        {
            string newToken = Generate();
            if(context.Update(token, newToken))
            {
                return newToken;
            }
            throw new InvalidTokenException();
        }

        public void Permission(Permission permission, int userId)
        {
            if(permission != Models.Permission.NONE && !context.Permission(permission, userId))
            {
                throw new InvalidTokenException();
            }
        }
    }
}