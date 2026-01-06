using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Check
{
    public sealed record CheckNotificationCommand(Guid userId, Guid notificationId) : IRequest<Result>;
}
