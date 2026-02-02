using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.ChangeEmail
{
    public sealed class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChangeEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            if (request.email == null)
                return Result.Failure(DomainErrors.User.InvalidEmail);

            if(await _userRepository.IsEmailTaken(request.email))
                return Result.Failure(DomainErrors.User.EmailTaken);

            var changeEmailResult = user.ChangeEmail(request.email);

            if(changeEmailResult.IsFailure)
                return Result.Failure(changeEmailResult.Error);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
