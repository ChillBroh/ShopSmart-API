using MongoDB.Bson;
using MongoDB.Driver;
using webApi.Data;
using webApi.Interfaces.Repository;
using webApi.Models;

namespace webApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(ObjectId id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName) 
        {
            return await _users.Find(user => user.UserName == userName).FirstOrDefaultAsync();
        }


        public async Task CreateUser(ApplicationUser user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> UpdateUser(Guid UserId, ApplicationUser user)
        {
            var result = await _users.ReplaceOneAsync(u => u.UserId == UserId, user);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUser(Guid UserId)
        {
            var result = await _users.DeleteOneAsync(user => user.UserId == UserId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<ApplicationUser>> GetUsersPendingApprovalAsync()
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.AdminOrCSRApproved, false) &
                         Builders<ApplicationUser>.Filter.Eq(u => u.Role, "Customer");

            return await _users.Find(filter).ToListAsync();
        }

        public async Task<bool> ApproveUserByUserIdAsync(Guid UserId)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, UserId);
            var update = Builders<ApplicationUser>.Update.Set(u => u.AdminOrCSRApproved, true);

            var result = await _users.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
