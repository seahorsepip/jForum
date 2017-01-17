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
    public class UserController : ApiController
    {
        UserRepository repository = new UserRepository(new UserSQLContext());
        
        public IHttpActionResult Post(UserModel user)
        {
            //Create a new user
            try
            {
                return Content(HttpStatusCode.Created, repository.Create(user));
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

        [Token]
        public IHttpActionResult Get()
        {
            //Get current user
            try
            {
                return Content(HttpStatusCode.OK, repository.Read((int)Request.Properties["UserId"]));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [Token]
        public IHttpActionResult Put(UserModel user)
        {
            //Update current user
            try
            {
                user.Id = (int)Request.Properties["UserId"];
                repository.Update(user);
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
        
        [Token]
        public IHttpActionResult Delete()
        {
            //Delete current user
            return Delete((int)Request.Properties["UserId"]);
        }

        [Token]
        public IHttpActionResult Delete(int id)
        {
            //Delete a user
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
