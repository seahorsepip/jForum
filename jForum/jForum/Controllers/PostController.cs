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
        
        [Token(Permission.CREATE_POST)]
        public IHttpActionResult Post(PostModel post)
        {
            //Create a new post
            try
            {
                return Content(HttpStatusCode.Created, repository.Create(post, (int)Request.Properties["UserId"]));
            }
            catch(InvalidModelException e)
            {
                ModelState.AddModelError(e.Key, e.Value);
                return BadRequest(ModelState);
            }
        }
        
        public IHttpActionResult Get(int id)
        {
            //Get a specific post
            try {
                return Content(HttpStatusCode.OK, repository.Read(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
        [Token(Permission.UPDATE_POST)]
        public IHttpActionResult Put(PostModel post)
        {
            try
            {
                repository.Update(post, (int)Request.Properties["UserId"]);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidModelException e)
            {
                ModelState.AddModelError(e.Key, e.Value);
                return BadRequest(ModelState);
            }
        }
        
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
                return NotFound();
            }
        }
    }
}
