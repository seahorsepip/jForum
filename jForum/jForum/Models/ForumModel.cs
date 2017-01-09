using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class ForumModel
    {
        int id;
        string name;
        string description;
        Dictionary<int, SectionModel> sections;

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

        public Dictionary<int, SectionModel> Sections
        {
            get
            {
                return sections;
            }

            set
            {
                sections = value;
            }
        }
    }
}