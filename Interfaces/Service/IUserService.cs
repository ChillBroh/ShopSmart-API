using MongoDB.Bson;
using webApi.Models;

namespace webApi.Interfaces.Service
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(ObjectId id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task CreateUser(ApplicationUser user);
        Task<bool> UpdateUser(Guid UserId, ApplicationUser user);
        Task<bool> DeleteUser(Guid UserId);
        Task<bool> EmailExists(string email);
        Task<bool> UserExists(string userName);
        Task<List<ApplicationUser>> GetUsersPendingApprovalAsync();
        Task<bool> ApproveUserByUserIdAsync(Guid UserId);
    }
}
