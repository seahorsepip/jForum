using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jForum.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        [Required]
        public int ForumId { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(5), MaxLength(200)]
        public string Description { get; set; }
        List<Permission> permissions;
        public List<Permission> Permissions {
            get
            {
                return permissions;
            }
            set
            {
                permissions = value.Distinct().ToList();
            }
        }
    }
}