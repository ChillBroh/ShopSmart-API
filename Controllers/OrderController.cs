using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webApi.DTOs.OrderDtos;
using webApi.DTOs.ProductDtos;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Models.Enums;
using webApi.Services;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost("CreateOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
        {
            List<Guid> updateUnsucsessFullProducts = new(); ;
           
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                // Extract the role claim
                var NameIdentifier = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                
                if (NameIdentifier != null)
                {
                    var newOrder = new Order
                    {
                        OrderId = Guid.NewGuid(),
                        UserId = Guid.Parse(NameIdentifier.Value),
                        TotalPrice = 0,
                        OrderStatus = Models.Enums.OrderStatus.Processing.ToString(),
                        CreatedAt = DateTime.UtcNow,
                        ProductIdsWithPurchasedQuantity = new List<Models.ProductDetails>()
                    };
                    foreach (var item in order.ProductIdsWithPurchasedQuantity)
                    {
                       if (await _productService.UpdateStock(item.ProductId, item.Quantity))
                        {
                            var  product = await _productService.GetProductByProductId(item.ProductId);
                            newOrder.ProductIdsWithPurchasedQuantity.Add(new Models.ProductDetails { ProductId = item.ProductId , Quantity = item.Quantity});
                            newOrder.TotalPrice += product.Price*item.Quantity;
                        }
                        else
                        {
                            updateUnsucsessFullProducts.Add(item.ProductId);
                        }
                    }

                    if (updateUnsucsessFullProducts.Count == 0)
                    {
                        await _orderService.CreateOrder(newOrder);
                        return Ok(new { message = "Order created successfully." });
                    }
                    else if(updateUnsucsessFullProducts.Count >= order.ProductIdsWithPurchasedQuantity.Length)
                    {
                        return NotFound(new { message = "Product Or Stocks unavailable" });
                    }
                    else if(updateUnsucsessFullProducts.Count > 0 && updateUnsucsessFullProducts.Count < order.ProductIdsWithPurchasedQuantity.Length)
                    {
                        await _orderService.CreateOrder(newOrder);                
                        return Ok(new { message =   $"Order created successfully, but some products are unavailable at the moment." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "UserId not found." });
                }
            }
            return Unauthorized(new { message = "User not found." });

        }

        [HttpGet("GetAllOrders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllOrders()
        {
            var  orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteOrder(Guid orderId) {
            var order = await _orderService.GetOrderByOrderId(orderId);
            if (order != null)
            {
                order.ProductIdsWithPurchasedQuantity?.ForEach(item =>
                {
                    _productService.UpdateStock(item.ProductId, -item.Quantity);
                });

                await _orderService.DeleteOrder(orderId);
                return Ok("Order Delete sucsessfully");
            }
            else
            {
                return NotFound("Order Not Found");
            }  
        }

        [HttpPost("UpdateOrderStatus")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId,OrderStatus status)
        {
            if(await _orderService.UpdateOrderStatus(orderId,status.ToString()))
            {
                return Ok("Status update successful.");

            }
            else
            {
                return BadRequest("Unable to Update Status");
            }
        }


    }
}
