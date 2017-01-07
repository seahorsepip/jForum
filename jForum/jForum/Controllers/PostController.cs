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
    public class PostController : ApiController
    {
        PostRepository repository = new PostRepository(new PostSQLContext());

        [HttpGet]
        public PagedModel Index(int id, int start = 0, int stop = int.MaxValue)
        {
            //Get all posts in specific topic
            return repository.Read(id, new PagedModel(start, stop));
        }

        [HttpPost]
        public PostModel Index(PostModel post, string token)
        {
            //Create a new post
            return repository.Create(post, token);
        }

        [HttpDelete]
        public IHttpActionResult Index(int id, string token)
        {
            //Delete a post
            if (repository.Delete(id, token))
            {
                return Ok();
            }
            return BadRequest("Id not found.");
        }
    }
}
