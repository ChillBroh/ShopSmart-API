using MongoDB.Bson;
using webApi.Models;

namespace webApi.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(ObjectId id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task CreateUser(ApplicationUser user);
        Task<ApplicationUser> GetUserByUserName(string userName);
        Task<bool> UpdateUser(Guid UserId, ApplicationUser user);
        Task<bool> DeleteUser(Guid UserId);
        Task<List<ApplicationUser>> GetUsersPendingApprovalAsync();
        Task<bool> ApproveUserByUserIdAsync(Guid UserId);
    }
}
