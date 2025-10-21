using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Headline { get; set; }

        public string AvatarUrl { get; set; } = "/images/profilepictures/img2.webp";
    }
}