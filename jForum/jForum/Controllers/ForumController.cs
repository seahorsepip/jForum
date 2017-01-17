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
        
        [Token(Permission.CREATE_FORUM)]
        public IHttpActionResult Post(ForumModel forum)
        {
            //Create a new forum
            return Content(HttpStatusCode.Created, repository.Create(forum));
        }
        
        public IHttpActionResult Get()
        {
            //Get all forums
            try {
                return Content(HttpStatusCode.OK, repository.Read());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
        public IHttpActionResult Get(int id)
        {
            //Get a specific forum and it's sections, no token required since it's public
            try
            {
                return Content(HttpStatusCode.OK, repository.Read(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
        [Token(Permission.UPDATE_FORUM)]
        public IHttpActionResult Put(ForumModel forum)
        {
            //Update a specific forum
            try
            {
                repository.Update(forum);
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
        
        [Token(Permission.DELETE_FORUM)]
        public IHttpActionResult Delete(int id)
        {
            //Delete a forum
            try
            {
                repository.Delete(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
