using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Modify
{
    public sealed class ModifyUserCommandHandler : IRequestHandler<ModifyUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ModifyUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ModifyUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user is null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            user.ChangePrimaryData(request.Name, request.Surname);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
