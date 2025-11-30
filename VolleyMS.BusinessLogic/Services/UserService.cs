using Microsoft.AspNetCore.Identity;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Repositories;
using VolleyMS.Core.Requests;

namespace VolleyMS.BusinessLogic.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Get(string userName) => await _userRepository.Get(userName);
    public async Task<User?> Get(Guid userId) => await _userRepository.Get(userId);

    public async Task Modify(string userName, UserModificationRequest userModificationRequest)
    {
        var user = User.Create(Guid.NewGuid(),
            userName,
            userModificationRequest.Password,
            userModificationRequest.userType,
            userModificationRequest.Name,
            userModificationRequest.Surname);

        var password = userModificationRequest.Password;
        string HashedPassword = new PasswordHasher<User>().HashPassword(user, password);

        user.SetHashedPassword(HashedPassword);
        await _userRepository.Modify(userName, user);
    }
}
