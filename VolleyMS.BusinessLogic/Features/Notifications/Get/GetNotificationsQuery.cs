using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public sealed record GetNotificationsQuery : IRequest<Result<PaginatedResult<NotificationDto>>>
    {
        public Guid userId;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
