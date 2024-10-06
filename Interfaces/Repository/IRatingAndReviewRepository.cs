using webApi.Models;

namespace webApi.Interfaces.Repository
{
    public interface IRatingAndReviewRepository
    {
        Task<IEnumerable<RatingAndReview>> GetRatingAndReviews();
        Task<RatingAndReview> GetRatingAndReviewByRatingAndReviewId(Guid OrderId);
        Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByUserId(Guid userId);
        Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByProductId(Guid userId);
        Task CreateRatingAndReview(RatingAndReview ratingAndReview);
        Task<bool> DeleteRatingAndReview(Guid ratingAndReviewId);
        Task<RatingAndReview> RatingAndReviewByUserIdAndProductId(Guid userId,Guid productId);
    }
}
