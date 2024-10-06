using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using webApi.Models.Enums;

namespace webApi.Models
{
    public class RatingAndReview
    {
        [BsonId]
        public ObjectId RatingAndReviewObjectId { get; set; }

        [BsonElement("RatingAndReviewId")]
        public Guid RatingAndReviewId { get; set; } = Guid.Empty;

        [BsonElement("UserId")]
        public Guid UserId { get; set; } = Guid.Empty;

        [BsonElement("ProductId")]
        public Guid? ProductId { get; set; }

        [BsonElement("Review")]
        public string Review { get; set; } = string.Empty;

        [BsonElement("Rating")]
        public float Rating { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        [BsonElement("UpdateAt")]
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
