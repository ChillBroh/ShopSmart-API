using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using webApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace webApi.DTOs.ProductDtos
{
    public class CreateProductDto
    {

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public ProductCategory ProductCategory { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
