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

namespace jForum.Controllers
{
    //[CollectionDataContract(Name = "posts")]
    public class PostController : ApiController
    {
        PostRepository repository = new PostRepository(new PostSQLContext());

        [Route("api/post")]
        [HttpGet]
        public List<PostModel> Get()
        {
            //Get all posts
            return repository.Get();
        }

        [Route("api/post/forum")]
        [HttpGet]
        public List<PostModel> Get(ForumModel forum)
        {
            //Get all posts in specific forum
            return repository.Get(forum);
        }

        [Route("api/post/topic")]
        [HttpGet]
        public List<PostModel> Get(TopicModel topic)
        {
            //Get all posts in specific topic
            return repository.Get(topic);
        }

        [Route("api/post/post")]
        [HttpGet]
        public List<PostModel> Get(PostModel post)
        {
            //Get all posts that quote a specific post
            return repository.Get(post);
        }
    }
}
