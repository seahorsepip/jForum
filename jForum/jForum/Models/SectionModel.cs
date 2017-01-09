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
        SectionModel parentSection;
        Dictionary<int, SectionModel> sections;
        Dictionary<int, TopicModel> topics;

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

        public SectionModel ParentSection
        {
            get
            {
                return parentSection;
            }

            set
            {
                parentSection = value;
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

        public Dictionary<int, TopicModel> Topics
        {
            get
            {
                return topics;
            }

            set
            {
                topics = value;
            }
        }
    }
}