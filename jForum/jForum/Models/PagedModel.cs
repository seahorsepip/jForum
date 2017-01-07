using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class PagedModel
    {
        int start;
        int stop;
        int count;
        object data;

        public PagedModel(int start, int stop)
        {
            this.start = start;
            this.stop = start < stop ? stop : start + 1;
        }

        public int Start
        {
            get
            {
                return start;
            }
        }

        public int Stop
        {
            get
            {
                return stop;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
                stop = count > stop ? stop : count;
            }
        }

        public object Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }
    }
}