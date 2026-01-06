using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.LoginCheck
{
    public sealed class LoginCheckQueryHandler : IRequestHandler<LoginCheckQuery, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        public LoginCheckQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<bool>> Handle(LoginCheckQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.login))
            {
                return Result.Failure<bool>(DomainErrors.User.UserNameEmpty);
            }
            var isFree = !await _userRepository.IsLoginTaken(request.login);

            return Result.Success(isFree);
        }
    }
}
