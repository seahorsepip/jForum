using jForum.Data;
using jForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jForum.Logic
{
    public class SectionRepository
    {
        ISectionContext context;

        public SectionRepository(ISectionContext context)
        {
            this.context = context;
        }

        public SectionModel Create(SectionModel section)
        {
            section.Id = context.Create(section);
            return section;
        }

        public SectionModel Read(int id)
        {
            SectionModel section = context.Read(id);
            if(section != null)
            {
                return section;
            }
            throw new NotFoundException();
        }

        public void Update(SectionModel section)
        {
            if (section.Id == 0)
            {
                throw new InvalidModelException("section.Id", "The Id field is required.");
            }
            if (!context.Update(section))
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