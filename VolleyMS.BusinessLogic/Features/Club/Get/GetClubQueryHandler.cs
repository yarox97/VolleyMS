using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Get
{
    public sealed class GetClubQueryHandler : IRequestHandler<GetClubQuery, Result<ClubDto>>
    {
        private readonly IClubRepository _clubRepository;
        public GetClubQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        public async Task<Result<ClubDto>> Handle(GetClubQuery request, CancellationToken cancellationToken)
        {
            var club = await _clubRepository.GetByIdWithMembersAsync(request.clubId, cancellationToken);
            if (club is null)
                return Result.Failure<ClubDto>(Error.NullValue);

            var clubDto = new ClubDto();

            if (request.userId == null || !await _clubRepository.ContainsUser(request.clubId, request.userId))
            {
                clubDto.Id = club.Id;
                clubDto.Name = club.Name;
                clubDto.Description = club.Description;
                clubDto.AvatarURL = club.AvatarUrl;
                clubDto.BackGroundURL = club.BackGroundURL;
            }
            else
            {
                clubDto.Id = club.Id;
                clubDto.Name = club.Name;
                clubDto.Description = club.Description;
                clubDto.AvatarURL = club.AvatarUrl;
                clubDto.BackGroundURL = club.BackGroundURL;

                clubDto.CreatedAt = club.CreatedAt;
                clubDto.JoinCode = club.JoinCode;
            }

            return Result.Success(clubDto);
        }
    }
}
