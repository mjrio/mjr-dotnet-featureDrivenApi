using System;
using System.ComponentModel.DataAnnotations;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public int FromId { get; set; }
        [Required]
        public User From { get; set; }
        [Required]
        [StringLength(500)]
        public string Content { get; set; }

       // public Ticket Ticket { get; set; }
    }
}
