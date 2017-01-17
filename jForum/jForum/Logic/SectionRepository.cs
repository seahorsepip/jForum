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

        void Validate(SectionModel section)
        {
            new ValidateString(section.Title, 3, 50, "Section title");
            new ValidateString(section.Description, 10, 200, "Section description");
            if (section.Forum == null || section.Forum.Id == 0)
            {
                throw new InvalidModelException("Section forum id is missing");
            }
        }

        public SectionModel Create(SectionModel section)
        {
            Validate(section);
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
            Validate(section);
            if(section.Id == 0)
            {
                throw new InvalidModelException("Section id is missing");
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