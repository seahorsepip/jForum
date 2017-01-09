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
    public class SectionController : ApiController
    {
        SectionRepository repository = new SectionRepository(new SectionSQLContext());

        [HttpPost]
        [HttpOptions]
        [Token(Permission.CREATE_SECTION)]
        public IHttpActionResult Create(SectionModel section)
        {
            //Create a new forum
            return Content(HttpStatusCode.Created, repository.Create(section));
        }

        [HttpGet]
        public IHttpActionResult Read(int id)
        {
            //Get a specific section and it's sections and topics, no token required since it's public
            try
            {
                return Content(HttpStatusCode.OK, repository.Read(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [HttpOptions]
        [Token(Permission.UPDATE_SECTION)]
        public IHttpActionResult Update(SectionModel section)
        {
            //Update a specific section
            try
            {
                repository.Update(section);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [HttpOptions]
        [Token(Permission.DELETE_SECTION)]
        public IHttpActionResult Delete(int id)
        {
            //Delete a section
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
