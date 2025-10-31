using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        public List<Project> Projects { get; set; } = new();
    }
}