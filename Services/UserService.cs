using MongoDB.Bson;
using System.Runtime.InteropServices;
using webApi.Interfaces.Repository;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Models.Enums;

namespace webApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<ApplicationUser> GetUserById(Guid userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task CreateUser(ApplicationUser user)
        {
            await _userRepository.CreateUser(user);
        }

        public async Task<bool> UpdateUser(Guid UserId, ApplicationUser user)
        {
            return await _userRepository.UpdateUser(UserId, user);
        }

        public async Task<bool> DeleteUser(Guid UserId)
        {
            return await _userRepository.DeleteUser(UserId);
        }

        public async Task<bool> EmailExists(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            var user = await _userRepository.GetUserByUserName(userName);
            if(user == null)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public async Task<List<ApplicationUser>> GetUsersPendingApprovalAsync()
        {
            var users = await _userRepository.GetUsersPendingApprovalAsync();
            return users;
        }

        public async Task<bool> ApproveUserByUserIdAsync(Guid UserId)
        {
            return await _userRepository.ApproveUserByUserIdAsync(UserId);
        }

        public async Task<bool> ActiveateDeactivateUser(Guid UserId, UserActivateDeactivate activateDeactivate)
        {
               return await _userRepository.ActiveateDeactivateUser(UserId, activateDeactivate);
            
        }
    }
}
