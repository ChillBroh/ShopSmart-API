using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApi.Models;
using webApi.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using webApi.Models.Enums;
using webApi.DTOs.ProductDtos;
using webApi.Interfaces.Service;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("CreateProduct")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Vendor")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                // Extract the role claim
                var NameIdentifier = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                if (NameIdentifier != null)
                {

                    var newProduct = new Product
                    {
                        ProductId = Guid.NewGuid(),
                        VendorId = Guid.Parse(NameIdentifier.Value),
                        ProductName = product.ProductName,
                        ProductCategory = product.ProductCategory.ToString(),
                        Price = product.Price,
                        Stock = product.Stock,
                        Description = product.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdateAt = DateTime.UtcNow,
                        AdminApproved = false,
                    };
                    await _productService.CreateProduct(newProduct);

                    return Ok(new { message = "Product created successfully." });
                }
                else
                {
                    return BadRequest(new { message = "VendorId not found." });
                }
            }
            return Unauthorized(new { message = "User not found." });
        }

        [HttpPost("ApproveProduct/{productId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        public async Task<IActionResult> ApproveUser(Guid productId)
        {
            var result = await _productService.ApproveProductByProductIdAsync(productId);

            if (!result)
            {
                return NotFound("Product not found or already approved.");
            }

            return Ok(new { message = "Product approved successfully." });
        }

        [HttpGet("GetPendingApprovalProducts")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
        public async Task<IActionResult> GetPendingApprovalProducts()
        {
            var products = await _productService.GetProductsPendingApprovalAsync();
            return Ok(products);
        }

        [HttpGet("GetAllProducts")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("GetAllApprovedProducts")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllApprovedProducts()
        {
            var products = await _productService.GetAllApprovedProducts();
            return Ok(products);
        }

        [HttpDelete("DeleteProduct/{productId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Vendor")]
        public async Task<IActionResult> DeleteProduct(Guid productId) 
        {
           var  product = await _productService.GetProductByProductId(productId);

           if(product != null)
            {

                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    // Extract the role claim
                    var NameIdentifier = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                    if (NameIdentifier != null)
                    {
                        if (product.VendorId == Guid.Parse(NameIdentifier.Value))
                        {
                            await _productService.DeleteProduct(productId);
                            return Ok(new { message = "Product deleted successfully." });

                        }
                        else
                        {
                            return Unauthorized(new { message = "You Don't have Permission to delete this Product" });
                        }                           
                    }
                    else
                    {
                        return BadRequest(new { message = "VendorId not found." });
                    }
                }
                return Unauthorized(new { message = "User not found." });
            }
            else
            {
                return NotFound(new { message = "Product not found." });
            }

        }

        [HttpGet("GetProductByProductId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProductByProductId(Guid ProductId)
        {
            var product = await _productService.GetProductByProductId(ProductId);
            return Ok(product);
        }

        [HttpPut("UpdateProduct/{productId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Vendor")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] CreateProductDto updatedProduct)
        {
            var product = await _productService.GetProductByProductId(productId);

            if (product != null)
            {

                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    // Extract the role claim
                    var NameIdentifier = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                    if (NameIdentifier != null)
                    {
                        if (product.VendorId == Guid.Parse(NameIdentifier.Value))
                        {
                            var newUpdateProduct = new Product
                            {
                                Id = product.Id,
                                ProductId = productId,
                                VendorId = product.VendorId,
                                ProductName = updatedProduct.ProductName,
                                ProductCategory = updatedProduct.ProductCategory.ToString(),
                                Price = updatedProduct.Price,
                                Stock = updatedProduct.Stock,
                                Description = updatedProduct.Description,
                                CreatedAt = product.CreatedAt,
                                UpdateAt = DateTime.UtcNow,
                                AdminApproved = product.AdminApproved,
                            };
                            await _productService.UpdateProduct(productId, newUpdateProduct);
                            return Ok(new { message = "Product updated successfully." });

                        }
                        else
                        {
                            return Unauthorized(new { message = "You Don't have Permission to update this Product" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "VendorId not found." });
                    }
                }
                return Unauthorized(new { message = "User not found." });
            }
            else
            {
                return NotFound(new { message = "Product not found." });
            }

        }


    }
}
