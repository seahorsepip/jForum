using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class TopicModel : PostModel
    {
        string title;
        int views;
        bool sticky;
        List<TagModel> tags;
        PagedModel posts;
        SectionModel section;

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

        public int Views
        {
            get
            {
                return views;
            }

            set
            {
                views = value;
            }
        }

        public bool Sticky
        {
            get
            {
                return sticky;
            }

            set
            {
                sticky = value;
            }
        }

        public List<TagModel> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                tags = value;
            }
        }

        public PagedModel Posts
        {
            get
            {
                return posts;
            }

            set
            {
                posts = value;
            }
        }

        public SectionModel Section
        {
            get
            {
                return section;
            }

            set
            {
                section = value;
            }
        }
    }
}