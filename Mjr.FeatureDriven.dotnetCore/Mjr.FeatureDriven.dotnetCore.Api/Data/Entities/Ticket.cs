using System;
using System.ComponentModel.DataAnnotations;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        //public int FromId { get; set; }
        //public User From { get; set; }
        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
        [MaxLength(30)]
        public string Priority { get; set; }

        [Required]
        [MaxLength(30)]
        public string Type { get; set; }

        //public List<User> Participants { get; set; }
        //public List<Post> Posts { get; set; }
    }
}
