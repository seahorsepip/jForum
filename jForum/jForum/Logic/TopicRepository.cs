using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class TopicRepository
    {
        ITopicContext context;

        public TopicRepository(ITopicContext context)
        {
            this.context = context;
        }

        public TopicModel Create(TopicModel topic, int userId)
        {
            topic.Id = context.Create(topic, userId);
            return topic;
        }

        public TopicModel Read(int id, PagedModel page)
        {
            TopicModel result = context.Read(id, page);
            if(result != null && result.Posts.Count >= result.Posts.Start)
            {
                return result;
            }
            throw new NotFoundException();
        }

        public void Update(TopicModel topic, int userId)
        {
            if(!context.Update(topic, userId))
            {
                throw new NotFoundException();
            }
        }

        public void Delete(int id, int userId)
        {
            if(!context.Delete(id, userId))
            {
                throw new NotFoundException();
            }
        }
    }
}