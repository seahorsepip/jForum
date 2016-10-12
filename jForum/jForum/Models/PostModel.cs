using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace jForum.Models
{
    public class PostModel
    {
        int id;
        string content;
        UserModel user;
        List<PostModel> quotes;
        DateTime date;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }

        public UserModel User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public List<PostModel> Quotes
        {
            get
            {
                return quotes;
            }

            set
            {
                quotes = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }
    }
}