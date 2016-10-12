using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class GroupModel
    {
        string name;
        string description;

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

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }
    }
}