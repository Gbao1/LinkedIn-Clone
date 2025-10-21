using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Profile
    {
        public int Id { get; set; }

        // Link to Identity user
        [Required]
        public string UserId { get; set; }

        // ✅ FullName is required
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        // Optional fields
        [StringLength(150)]
        public string? Headline { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public string? Summary { get; set; }

        // Optional avatar path or URL
        [StringLength(250)]
        public string? AvatarUrl { get; set; }

        // Optional JSON blob for dynamic sections
        public string? SectionsJson { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
