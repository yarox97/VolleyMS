using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public sealed record GetNotificationsQuery(Guid userId) : IRequest<Result<IEnumerable<NotificationDto>>>
    {
    }
}
