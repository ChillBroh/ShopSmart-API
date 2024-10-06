using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using webApi.Models.Enums;

namespace webApi.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId OrderObjectId { get; set; }

        [BsonElement("OrderId")]
        public Guid OrderId { get; set; } = Guid.Empty;

        [BsonElement("UserId")]
        public Guid UserId { get; set; } = Guid.Empty;
        
        [BsonElement("ProductIdsWithPurchasedQuantity")]
        public List<ProductDetails>? ProductIdsWithPurchasedQuantity { get; set; }

        [BsonElement("TotalPrice")]
        public float TotalPrice { get; set; }

        [BsonElement("OrderStatus")]
        public string OrderStatus { get; set; } = string.Empty;
         
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        [BsonElement("UpdateAt")]
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }

    public class ProductDetails
    {
        [BsonElement("ProductId")]
        public Guid ProductId { get; set; } = Guid.Empty;

        [BsonElement("Quantity")]
        public int Quantity { get; set; } = 1;
    }
}
