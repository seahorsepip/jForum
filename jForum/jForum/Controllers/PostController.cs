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

        [HttpPost]
        [HttpOptions]
        [Token(Permission.CREATE_POST)]
        public IHttpActionResult Create(PostModel post)
        {
            //Create a new post
            return Content(HttpStatusCode.Created, repository.Create(post, (int)Request.Properties["UserId"]));
        }

        [HttpGet]
        [HttpOptions]
        public IHttpActionResult Read(int id)
        {
            //Get posts in specific topic
            try {
                return Content(HttpStatusCode.OK, repository.Read(id));
            }
            catch (NotFoundException)
            {
                return BadRequest("No posts found.");
            }
        }

        [HttpPut]
        [HttpOptions]
        [Token(Permission.UPDATE_POST)]
        public IHttpActionResult Update(PostModel post)
        {
            try
            {
                repository.Update(post, (int)Request.Properties["UserId"]);
                return Ok();
            }
            catch (NotFoundException)
            {
                return BadRequest("Post does not exist or the user is not the creator of the post and is missing the UPDATE_ALL_POST permission.");
            }
        }

        [HttpDelete]
        [HttpOptions]
        [Token(Permission.DELETE_POST)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                repository.Delete(id, (int)Request.Properties["UserId"]);
                return Ok();
            }
            catch (NotFoundException)
            {
                return BadRequest("Post does not exist or the user is not the creator of the post and is missing the DELETE_ALL_POST permission.");
            }
        }
    }
}
