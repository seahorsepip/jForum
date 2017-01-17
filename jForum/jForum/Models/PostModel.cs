using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace jForum.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        [Required, MinLength(10), MaxLength(2000)]
        public string Content { get; set; }
        public UserModel User { get; set; }
        public Dictionary<int, PostModel> Quotes { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public int TopicId { get; set; }
    }
}