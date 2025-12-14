using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Create
{
    public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IClubService _clubService;
        private readonly IUnitOfWork _unitOfWork;
        public CreateClubCommandHandler(IUserRepository userRepository, IClubRepository clubRepository, IClubService clubService, IUnitOfWork unitOfWork)
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateClubCommand command, CancellationToken cancellationToken)
        {
            string joinCode = await _clubService.GenerateJoinCode(cancellationToken);

            var user = await _userRepository.GetByIdAsync(command.CreatorId);
            if (user == null) return Result.Failure<Guid>(DomainErrors.User.UserNotFound);

            var clubResult = Core.Models.Club.Create(
                command.Name, 
                joinCode, 
                command.Description, 
                command.AvatarURL, 
                command.BackGroundURL, 
                user);

            if(clubResult.IsFailure) return Result.Failure<Guid>(clubResult.Error);

            await _clubRepository.AddAsync(clubResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(clubResult.Value.Id);
        }
    }
}
