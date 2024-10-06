using System.ComponentModel.DataAnnotations;
using webApi.Models.Enums;

namespace webApi.DTOs.UserDtos
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string  Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } // or use UserRole if you want to enforce enum
    }
}
