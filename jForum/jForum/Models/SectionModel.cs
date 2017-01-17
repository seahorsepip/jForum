using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class SectionModel
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Title { get; set; }
        [Required, MinLength(5), MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int ForumId { get; set; }
        public int ParentSectionId { get; set; }
        public Dictionary<int, SectionModel> Sections { get; set; }
        public Dictionary<int, TopicModel> Topics { get; set; }
    }
}