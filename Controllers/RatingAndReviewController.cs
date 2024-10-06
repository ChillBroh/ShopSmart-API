using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webApi.DTOs.RatingAndReviewDtos;
using webApi.Interfaces.Service;
using webApi.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingAndReviewController : ControllerBase
    {
        private readonly IRatingAndReviewService _ratingAndReviewService;
        private readonly IProductService _productService;
        public RatingAndReviewController(IRatingAndReviewService ratingAndReviewService, IProductService productService)
        {
            _ratingAndReviewService = ratingAndReviewService;
            _productService = productService;
        }

        [HttpPost("CreateRatingAndReview")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRatingAndReview([FromBody] RatingAndReviewDto ratingAndReview)
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                // Extract the role claim
                var NameIdentifier = claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                if (NameIdentifier != null)
                {
                    Guid userId = Guid.Parse(NameIdentifier.Value);
                    var ratingAndReviewAvailability =await _ratingAndReviewService.RatingAndReviewByUserIdAndProductId(userId, ratingAndReview.ProductId);
                    if (ratingAndReviewAvailability == null) {

                        var product = await _productService.GetProductByProductId(ratingAndReview.ProductId);
                        if (product != null)
                        {
                            var newRatingAndReview = new RatingAndReview
                            {
                                RatingAndReviewId = Guid.NewGuid(),
                                UserId = userId,
                                ProductId = ratingAndReview.ProductId,
                                Review = ratingAndReview.Review,
                                Rating = ratingAndReview.Rating,
                                CreatedAt = DateTime.UtcNow,
                                UpdateAt = DateTime.UtcNow,
                            };

                            await _ratingAndReviewService.CreateRatingAndReview(newRatingAndReview);
                            return Ok(new { message = "Rating and Reviewed created successfully." });
                        }
                        else
                        {
                            return NotFound(new { message = "Product not found" });
                        }
                    }
                    else
                    {
                        return Ok(new { message = "You already reviewed this product.t" });
                    }


                }
                else
                {
                    return BadRequest(new { message = "UserId not found." });
                }
            }
            return Unauthorized(new { message = "User not found." });
        }

        [HttpPost("GetRatingAndReviews")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRatingAndReviews()
        {
            var ratingAndReview =await _ratingAndReviewService.GetRatingAndReviews();
            return Ok(ratingAndReview);
        }


    }
}
