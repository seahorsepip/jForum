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
    public class UserController : ApiController
    {
        UserRepository repository = new UserRepository(new UserSQLContext());

        [HttpPost]
        public string Index(string email, string password)
        {
            return repository.Token(email, password);
        }

        [HttpPost]
        public IHttpActionResult Index(string token)
        {
            string newToken = repository.Token(token);
            if(newToken != null)
            {
                return Ok(newToken);
            }
            return BadRequest("Invalid token.");
        }
    }
}
