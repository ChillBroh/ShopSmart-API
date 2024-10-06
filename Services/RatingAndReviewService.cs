using webApi.Interfaces.Repository;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Repositories;

namespace webApi.Services
{
    public class RatingAndReviewService: IRatingAndReviewService
    {
        private readonly IRatingAndReviewRepository _ratingAndReviewRepository;

    
        public RatingAndReviewService(IRatingAndReviewRepository ratingAndReviewRepository)
        {
            _ratingAndReviewRepository = ratingAndReviewRepository;
        }

        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviews()
        {
            return await _ratingAndReviewRepository.GetRatingAndReviews();
        }


        public async Task<RatingAndReview> GetRatingAndReviewByRatingAndReviewId(Guid ratingAndReviewId)
        {
            return await _ratingAndReviewRepository.GetRatingAndReviewByRatingAndReviewId(ratingAndReviewId);
        }


        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByUserId(Guid userId)
        {
            return await _ratingAndReviewRepository.GetRatingAndReviewByUserId(userId);
        }

        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByProductId(Guid productId)
        {
            return await _ratingAndReviewRepository.GetRatingAndReviewByProductId(productId);
        }


        public async Task CreateRatingAndReview(RatingAndReview ratingAndReview)
        {
            await _ratingAndReviewRepository.CreateRatingAndReview(ratingAndReview);
        }


        public async Task<bool> DeleteRatingAndReview(Guid ratingAndReviewId)
        {
            return await _ratingAndReviewRepository.DeleteRatingAndReview(ratingAndReviewId);
        }

        public async Task<RatingAndReview> RatingAndReviewByUserIdAndProductId(Guid userId, Guid productId)
        {
            return await _ratingAndReviewRepository.RatingAndReviewByUserIdAndProductId(userId, productId);
        }
    }
}
