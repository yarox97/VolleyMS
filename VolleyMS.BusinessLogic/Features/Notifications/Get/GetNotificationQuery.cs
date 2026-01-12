using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public sealed record GetNotificationQuery(Guid NotificationId, Guid UserId) : IRequest<Result<NotificationDto>>
    {

    }
}
