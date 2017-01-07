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
        List<SectionModel> sections;

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

        public List<SectionModel> Sections
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