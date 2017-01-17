using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class GroupRepository
    {
        IGroupContext context;

        public GroupRepository(IGroupContext context)
        {
            this.context = context;
        }

        public GroupModel Create(GroupModel group)
        {
            if (group.ForumId == 0)
            {
                throw new InvalidModelException("group.ForumId", "The Id field is required.");
            }
            group.Id = context.Create(group);
            return group;
        }

        public Dictionary<int, GroupModel> Read()
        {
            Dictionary<int, GroupModel> groups = context.Read();
            if(groups.Count() > 0)
            {
                return groups;
            }
            throw new NotFoundException();
        }

        public GroupModel Read(int id)
        {
            GroupModel group = context.Read(id);
            if(group != null)
            {
                return group;
            }
            throw new NotFoundException();
        }

        public void Update(GroupModel group)
        {
            if(group.Id == 0)
            {
                throw new InvalidModelException("group.Id", "The Id field is required.");
            }
            if (group.ForumId == 0)
            {
                throw new InvalidModelException("group.ForumId", "The Id field is required.");
            }
            if (!context.Update(group))
            {
                throw new NotFoundException();
            }
        }

        public void Delete(int id)
        {
            if(!context.Delete(id))
            {
                throw new NotFoundException();
            }
        }
    }
}