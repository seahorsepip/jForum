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
using System.Web.SessionState;

namespace jForum.Controllers
{
    public class TokenController : ApiController
    {
        TokenRepository repository = new TokenRepository(new TokenSQLContext());
        
        public IHttpActionResult Post(UserModel user)
        {
            try
            {
                return Content(HttpStatusCode.Created, repository.Create(user));
            }
            catch (InvalidModelException e)
            {
                ModelState.AddModelError(e.Key, e.Value);
                return BadRequest(ModelState);
            }
        }
        
        [Token]
        public IHttpActionResult Put()
        {
            try
            {
                return Content(HttpStatusCode.OK, repository.Update((string)Request.Properties["Token"]));
            }
            catch (InvalidTokenException)
            {
                return Unauthorized();
            }
        }
    }
}
