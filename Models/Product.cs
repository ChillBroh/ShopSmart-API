using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using webApi.Models.Enums;

namespace webApi.Models
{
    public class Product
    {
        [BsonId] 
        public ObjectId Id { get; set; }

        [BsonElement("ProductId")]
        public Guid ProductId { get; set; }

        [BsonElement("VendorId")]
        public Guid VendorId { get; set; } = Guid.Empty;

        [BsonElement("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("ProductCategory")]
        public string? ProductCategory { get; set; }

        [BsonElement("Price")]
        public float Price { get; set; }

        [BsonElement("Stock")]
        public int Stock { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        [BsonElement("UpdateAt")]
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        [BsonElement("AdminApproved")]
        public bool AdminApproved { get; set; }
    }
}
