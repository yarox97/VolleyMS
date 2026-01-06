using MediatR;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.LoginCheck;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.EmailCheck
{
    public class EmailCheckQueryHandler : IRequestHandler<EmailCheckQuery, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        public EmailCheckQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<bool>> Handle(EmailCheckQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.email))
            {
                return Result.Failure<bool>(DomainErrors.User.UserNameEmpty);
            }
            var isTaken = await _userRepository.IsEmailTaken(request.email);

            return Result.Success(isTaken);
        }
    }
}
