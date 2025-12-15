using MediatR;
using Microsoft.AspNetCore.Identity;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        public RegistrationCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = repository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<Guid>> Handle(RegistrationCommand command, CancellationToken cancellation)
        {
            if (await _userRepository.IsLoginTaken(command.userName))
                return Result.Failure<Guid>(DomainErrors.User.UserNameTaken);

            string hashedPassword = _passwordHasher.HashPassword(null, command.password);

            var userResult = User.Create(
                Guid.NewGuid(),
                command.userName,
                hashedPassword,
                UserType.User,
                command.name, 
                command.surname, 
                command.email, 
                command.avatarUrl);

            if (userResult.IsFailure)
                return Result.Failure<Guid>(userResult.Error);

            var user = userResult.Value;

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellation);

            return Result.Success(user.Id);
        }
    }
}
