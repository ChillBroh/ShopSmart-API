using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace webApi.Models
{
    public class ApplicationUser
    {
        [BsonId] 
        public ObjectId Id { get; set; }

        [BsonElement("UserId")]
        public Guid UserId{ get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("Role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("IsCartAvailable")]
        public bool IsCartAvailable { get; set; }

        [BsonElement("AdminOrCSRApproved")]
        public bool AdminOrCSRApproved { get; set; }

        [BsonElement("ActiveUser")]
        public bool ActiveUser { get; set; }   

    }
}
