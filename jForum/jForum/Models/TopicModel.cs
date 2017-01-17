using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class TopicModel : PostModel
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Title { get; set; }
        public int Views { get; set; }
        public bool Sticky { get; set; }
        public List<TagModel> Tags { get; set; }
        public PagedModel Posts { get; set; }
        [Required]
        public SectionModel Section { get; set; }
    }
}