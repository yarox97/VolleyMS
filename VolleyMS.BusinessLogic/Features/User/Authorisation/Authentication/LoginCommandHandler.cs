using MediatR;
using Microsoft.AspNetCore.Identity;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.Authentication
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService;
        public LoginCommandHandler(IUserRepository repository, IPasswordHasher<User> passwordHasher, JwtService jwtService)
        {
            _userRepository = repository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellation)
        {
            var userResult = await _userRepository.GetByUserNameAsync(command.userName);
            if (userResult == null)
                return Result.Failure<string>(DomainErrors.User.UserNotFound);

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(userResult, userResult.Password, command.password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
                return Result.Failure<string>(DomainErrors.User.InvalidPassword);

            var token = _jwtService.GenerateToken(userResult);
            return Result.Success(token);
        }
    }
}
