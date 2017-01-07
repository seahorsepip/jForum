﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jForum.Models;

namespace jForum.Data
{
    public interface IPostContext
    {
        PagedModel Read(int topicId, PagedModel page);
        int Create(PostModel post, string token);
        bool Delete(int id, string token);
    }
}