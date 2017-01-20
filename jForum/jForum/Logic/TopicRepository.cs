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
            if(page.Start < 0)
            {
                throw new InvalidModelException("page.Start", "The Start field value must be greater then 0.");
            }
            if (page.Stop <= page.Start)
            {
                throw new InvalidModelException("page.Stop", "The Stop field value must be greater then Start field value.");
            }
            TopicModel result = context.Read(id, page);
            if(result != null)
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