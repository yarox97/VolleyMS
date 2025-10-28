using Microsoft.AspNetCore.Identity;
using VolleyMS.BusinessLogic.Authorisation;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess;
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

    public async Task<User?> Get(string userName) => await _userRepository.GetByUserName(userName);

    public async Task Modify(string userName, UserModificationRequest userModificationRequest)
    {
        var user = User.Create(Guid.NewGuid(),
            userName,
            userModificationRequest.Password,
            userModificationRequest.userType,
            userModificationRequest.Name,
            userModificationRequest.Surname);

        var password = userModificationRequest.Password;
        string HashedPassword = new PasswordHasher<User>().HashPassword(user.user, password);
        user.user.SetHashedPassword(HashedPassword);
        if (user.error == string.Empty)
        {
            await _userRepository.Modify(userName, user.user);
        }
        else 
        {
            throw new Exception($"Error: {user.error}");
        }
    }
}
