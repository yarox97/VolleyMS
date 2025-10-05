using Microsoft.AspNetCore.Identity;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Repositories;

namespace VolleyMS.BusinessLogic;

public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task Register(string userName, string password, string name, string surname)
    {
        var IsLoginTaken = await _userRepository.IsLoginTaken(userName);

        if (IsLoginTaken)
        {
            throw new Exception("Username is taken!");
        }

        var userTuple = User.Create(Guid.NewGuid(), userName, password, UserType.Player, name, surname);

        if (userTuple.error == string.Empty)
        {
            string HashedPassword = new PasswordHasher<User>().HashPassword(userTuple.user, password);
            userTuple.user.SetHashedPassword(HashedPassword);

            await _userRepository.AddUser(userTuple.user);
        }
        else
        { 
            throw new Exception($"Error while creating a user: {userTuple.error}");
        }
    }

  
}
