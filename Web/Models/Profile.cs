using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Profile
    {
        public int Id { get; set; }

        // link to Identity user
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(150)]
        public string Headline { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        public string Summary { get; set; }

        // optional avatar path or url
        [StringLength(250)]
        public string AvatarUrl { get; set; }

        // JSON blob for dynamic sections [{ "title":"Experience", "content":"..." }, ...]
        public string SectionsJson { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}