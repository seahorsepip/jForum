using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface ISectionContext
    {
        int Create(SectionModel section);
        SectionModel Read(int id);
        bool Update(SectionModel section);
        bool Delete(int id);
    }
}