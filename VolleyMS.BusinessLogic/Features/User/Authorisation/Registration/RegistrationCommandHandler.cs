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
        private readonly RegistrationValidator _validator;
        private readonly IPasswordHasher<User> _passwordHasher;
        public RegistrationCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, RegistrationValidator validator, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<Guid>> Handle(RegistrationCommand command, CancellationToken cancellation)
        {
            if (await _userRepository.IsLoginTaken(command.userName))
                return Result.Failure<Guid>(DomainErrors.User.UserNameTaken);

            string hashedPassword = _passwordHasher.HashPassword(null, command.password);

            // Validate input data 
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var dictErrros = validationResult.ToDictionary();
                if (dictErrros != null)
                {
                    //Return json error list 
                }
            }
            // Create user
            var userResult = User.Create(
                Guid.NewGuid(),
                command.userName,
                hashedPassword,
                UserType.User,
                command.name, 
                command.surname, 
                command.email,
                null);

            if (userResult.IsFailure)
                return Result.Failure<Guid>(userResult.Error);

            var user = userResult.Value;

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellation);

            return Result.Success(user.Id);
        }
    }
}
