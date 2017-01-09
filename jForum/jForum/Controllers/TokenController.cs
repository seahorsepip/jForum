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

        [HttpPost]
        public IHttpActionResult Create(UserModel user)
        {
            return Content(HttpStatusCode.Created, repository.Create(user.Email, user.Password));
        }

        [HttpPut]
        [HttpOptions]
        [Token]
        public IHttpActionResult Update()
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
