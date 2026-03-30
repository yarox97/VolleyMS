using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;
using VolleyMS.Core.ValueObjects;

namespace VolleyMS.BusinessLogic.Features.Users.UpdateAttributes
{
    public class UpdatePlayerAttributesCommandHandler : IRequestHandler<UpdatePlayerAttributesCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlayerAttributesCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdatePlayerAttributesCommand request, CancellationToken cancellationToken)
        {
            // get user
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure(DomainErrors.User.UserNotFound);
            }

            // create attributes value object
            var attributesResult = PlayerAttributes.Create(
                request.Position,
                request.HeightCm,
                request.WeightKg,
                request.DominantHand,
                request.SpikeReachCm,
                request.BlockReachCm
            );

            if (!attributesResult.IsSuccess)
            {
                return Result.Failure(attributesResult.Error);
            }

            // update user attributes
            var updateResult = user.UpdateAttributes(attributesResult.Value);

            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }

            // sasve changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
