using Microsoft.AspNetCore.Identity;
using VolleyMS.BusinessLogic.Authorisation;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Repositories;

namespace VolleyMS.BusinessLogic.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Get(string userName)
    {
        return await _userRepository.GetByUserName(userName);
    }
}
