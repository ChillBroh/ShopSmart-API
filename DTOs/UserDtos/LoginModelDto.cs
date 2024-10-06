using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs.UserDtos
{
    public class LoginModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; }
    }
}
