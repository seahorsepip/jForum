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
    public class GroupController : ApiController
    {
        GroupRepository repository = new GroupRepository(new GroupSQLContext());
        
        [Token(Permission.CREATE_GROUP)]
        public IHttpActionResult Post(GroupModel group)
        {
            //Create a new group
            try
            {
                return Content(HttpStatusCode.Created, repository.Create(group));
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
        
        public IHttpActionResult Get()
        {
            //Get all groups
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
            //Get a specific group
            try
            {
                return Content(HttpStatusCode.OK, repository.Read(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
        [Token(Permission.UPDATE_GROUP)]
        public IHttpActionResult Put(GroupModel group)
        {
            //Update a specific group
            try
            {
                repository.Update(group);
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
        
        [Token(Permission.DELETE_GROUP)]
        public IHttpActionResult Delete(int id)
        {
            //Delete a group
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
