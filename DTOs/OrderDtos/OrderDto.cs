using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using webApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs.OrderDtos
{
    public class OrderDto
    {
        [Required]
        public ProductDetails[]? ProductIdsWithPurchasedQuantity { get; set; }

    }

    public class ProductDetails
    {
        [Required]
        public Guid ProductId { get; set; } = Guid.Empty;

        [Required]
        public int Quantity { get; set; } = 1;
    }
}
