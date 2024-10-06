using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs.RatingAndReviewDtos
{
    public class RatingAndReviewDto
    {

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string Review { get; set; } = string.Empty;

        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; set; }
    }
}
