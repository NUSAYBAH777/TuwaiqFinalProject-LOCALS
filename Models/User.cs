using System.ComponentModel.DataAnnotations;

namespace lLOCALS.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; 

        public string Role { get; set; } = "User"; 
    }
}