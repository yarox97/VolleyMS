using System.Text.Json;
using VolleyMS.BusinessLogic.NotificationPayloads.Categorized;
using VolleyMS.Core.Requests;

namespace VolleyMS.BusinessLogic.NotificationPayloads
{
    public static class PayLoadFactory
    {
        public static string? CreatePayLoad(NotificationRequest notificationRequest, Guid? senderId)
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

                NotificationCategory.ClubJoinApproved => JsonSerializer.Serialize(new JoinClubRequestApprovedPayLoad(
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
