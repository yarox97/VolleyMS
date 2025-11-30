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
            if (await _userRepository.IsLoginTaken(userName))
                throw new Exception($"Username {userName} is taken!");

            var user = User.Create(Guid.NewGuid(), userName, password, UserType.Player, name, surname);

            string HashedPassword = new PasswordHasher<User>().HashPassword(user, password);
            user.SetHashedPassword(HashedPassword);

            await _userRepository.AddUser(user);
        }

        public async Task<string> Authorize(string userName, string password)
        {
            var user = await _userRepository.Get(userName);
            if (user == null) 
                throw new Exception("User not found!"); 

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
