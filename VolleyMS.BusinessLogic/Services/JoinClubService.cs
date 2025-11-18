using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Requests;

namespace VolleyMS.BusinessLogic.Services
{
    public class JoinClubService
    {
        private readonly ClubService _clubService;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;
        public JoinClubService(ClubService clubService, UserService userService, NotificationService notificationService)
        {
            _clubService = clubService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task RequestToJoinClubAsync(string joinCode, string userName)
        {
            var user = await _userService.Get(userName)
                       ?? throw new Exception("User not found");

            var club = await _clubService.GetClubByJoinCode(joinCode)
                       ?? throw new Exception("Club not found");

            var usersByRole = await _clubService.GetUsersIdsByRole(
                club.Id,
                ClubMemberRole.President,
                ClubMemberRole.Coach);

            if (usersByRole == null || !usersByRole.Any())
            {
                throw new Exception("No recipients found");
            }

            var notification = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.ClubJoinRequest,
                Receivers = usersByRole.ToList(),
                Text = $"{user.Name} {user.Surname} requested to join your club {club.Name}",
                LinkedURL = $"/user/{user.UserName}"
            };

            await _notificationService.SendNotification(notification, user.Id);
        }

        public async Task ApproveClubJoinRequest(Guid userId, Guid clubId, Guid responseUserId)
        {
            var user = await _userService.GetById(userId);
            var club = await _clubService.GetById(clubId);
            _ = club ?? throw new Exception("Can't find a club");
            _ = user ?? throw new Exception("Can't add user to a club");

            // Send notification to a user whose request was approved
            await _notificationService.SendNotification(new NotificationRequest()
            {
                NotificationCategory = NotificationCategory.ClubJoinApproved,
                Receivers = new List<Guid> { userId },
                Text = $"Your request to join club {club.Name} was approved.",
            }, responseUserId);

        }
    }
}
