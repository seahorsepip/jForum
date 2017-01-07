using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class SectionModel
    {
        int id;
        string title;
        string description;
        ForumModel forum;
        List<SectionModel> parentSections;

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

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
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

        public ForumModel Forum
        {
            get
            {
                return forum;
            }

            set
            {
                forum = value;
            }
        }

        public List<SectionModel> ParentSections
        {
            get
            {
                return parentSections;
            }

            set
            {
                parentSections = value;
            }
        }
    }
}