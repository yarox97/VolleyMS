
using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Delete
{
    public sealed record DeleteNotificationCommand(Guid userId, Guid notificationId) : IRequest<Result>;
}
