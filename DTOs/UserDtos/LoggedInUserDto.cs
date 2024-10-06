using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using webApi.Models.Enums;

namespace webApi.DTOs.UserDtos
{
    public class LoggedInUserDto
    {

        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;       
        public bool IsCartAvailable { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
