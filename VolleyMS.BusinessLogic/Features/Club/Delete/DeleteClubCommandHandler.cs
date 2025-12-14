using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Delete
{
    internal class DeleteClubCommandHandler : IRequestHandler<DeleteClubCommand, Result<Guid>>
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteClubCommandHandler(IUserRepository userRepository, IClubRepository clubRepository, IClubService clubService, IUnitOfWork unitOfWork)
        {
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(DeleteClubCommand command, CancellationToken cancellationToken)
        {
            var club = await _clubRepository.GetByIdWithMembersAsync(command.clubId, cancellationToken);
            if (club is null)
                return Result.Failure<Guid>(Error.NullValue);

            var userMember = club.UserClubs.FirstOrDefault(uc => uc.UserId == command.clubPresidentId);

            if (userMember is null)
                return Result.Failure<Guid>(DomainErrors.Club.InvalidPermission);


            await _clubRepository.DeleteAsync(club.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(club.Id);
        }
    }
}
