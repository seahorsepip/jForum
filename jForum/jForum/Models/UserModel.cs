using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class UserModel
    {
        string name;
        string email;
        string password;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
    }
}