using MongoDB.Driver;
using webApi.Data;
using webApi.Interfaces.Repository;
using webApi.Models;

namespace webApi.Repositories
{
    public class RatingAndReviewRepository : IRatingAndReviewRepository
    {
        private readonly IMongoCollection<RatingAndReview> _ratingAndReviewCollection;
 
        public RatingAndReviewRepository(MongoDbContext context)
        {
            _ratingAndReviewCollection = context.RatingAndReview;
        }

        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviews()
        {
            return await _ratingAndReviewCollection.Find(r => true).ToListAsync();
        }

        // Get Rating and Review by RatingAndReviewId (note: this is a unique identifier)
        public async Task<RatingAndReview> GetRatingAndReviewByRatingAndReviewId(Guid ratingAndReviewId)
        {
            var filter = Builders<RatingAndReview>.Filter.Eq(r => r.RatingAndReviewId, ratingAndReviewId);
            return await _ratingAndReviewCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Get all Ratings and Reviews for a specific UserId
        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByUserId(Guid userId)
        {
            var filter = Builders<RatingAndReview>.Filter.Eq(r => r.UserId, userId);
            return await _ratingAndReviewCollection.Find(filter).ToListAsync();
        }

        // Get all Ratings and Reviews for a specific ProductId
        public async Task<IEnumerable<RatingAndReview>> GetRatingAndReviewByProductId(Guid productId)
        {
            var filter = Builders<RatingAndReview>.Filter.Eq(r => r.ProductId, productId);
            return await _ratingAndReviewCollection.Find(filter).ToListAsync();
        }

        // Create a new Rating and Review
        public async Task CreateRatingAndReview(RatingAndReview ratingAndReview)
        {
            await _ratingAndReviewCollection.InsertOneAsync(ratingAndReview);
        }

        // Delete a Rating and Review by RatingAndReviewId
        public async Task<bool> DeleteRatingAndReview(Guid ratingAndReviewId)
        {
            var filter = Builders<RatingAndReview>.Filter.Eq(r => r.RatingAndReviewId, ratingAndReviewId);
            var result = await _ratingAndReviewCollection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<RatingAndReview> RatingAndReviewByUserIdAndProductId(Guid userId, Guid productId)
        {
            return await _ratingAndReviewCollection
                        .Find(r => r.UserId == userId && r.ProductId == productId)
                        .FirstOrDefaultAsync();
        }
    }
}
