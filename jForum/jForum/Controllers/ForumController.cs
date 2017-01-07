using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using jForum.Models;
using jForum.Data;
using jForum.Logic;
using System.Runtime.Serialization;
using System.Web;

namespace jForum.Controllers
{
    public class ForumController : ApiController
    {
        ForumRepository repository = new ForumRepository(new ForumSQLContext());

        [HttpGet]
        public ForumModel Index(int id, int start = 0, int stop = int.MaxValue)
        {
            //Get all sections in specific forum
            return repository.Read(id);
        }
    }
}
