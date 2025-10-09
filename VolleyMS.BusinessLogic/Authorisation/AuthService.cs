using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Repositories;

namespace VolleyMS.BusinessLogic.Authorisation
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        public AuthService(UserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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

        public async Task<string> Authorize(string userName, string password)
        {

            var user = await _userRepository.GetByUserName(userName);
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Success)
            {
                return _jwtService.GenerateToken(user);
            }
            else
            {
                throw new Exception("Authorisation failed!");
            }
        }
    }
}
