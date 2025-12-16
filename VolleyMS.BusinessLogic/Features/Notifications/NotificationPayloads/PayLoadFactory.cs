using System.Text.Json;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads
{
    public static class PayLoadFactory
    {
        public static Result<string>? CreatePayLoad(NotificationRequest notificationRequest, Guid? senderId)
        {
            return notificationRequest.NotificationCategory switch
            {
                NotificationCategory.ClubJoinRequest => JsonSerializer.Serialize(new JoinClubRequestPayLoad(
                    (Guid)senderId,
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    notificationRequest.SenderName ?? throw new ArgumentException("SenderName is required"),
                    notificationRequest.SenderSurname ?? throw new ArgumentException("SenderSurname is required")
                )),

                NotificationCategory.ClubJoinApproved => JsonSerializer.Serialize(new JoinClubRequestApprovedPayload(
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required")
                )),

                NotificationCategory.ClubJoinRejected => JsonSerializer.Serialize(new JoinClubRequestRejectedPayLoad(
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required")
                )),

                // Можно добавить другие категории здесь...
                _ => null
            };
        }
    }
}
