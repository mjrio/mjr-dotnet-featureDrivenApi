using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(17)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string  IconUrl { get; set; }
        [StringLength(100)]
        public string  AvatarUrl { get; set; }
        public bool IsActive { get; set; }

        public List<Post> Posts { get; set; }

        public User()
        {
            Posts = new List<Post>();
        }

    }
}
