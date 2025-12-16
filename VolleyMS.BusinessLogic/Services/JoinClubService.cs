//using VolleyMS.Core.Models;
//using VolleyMS.Core.Requests;
//using VolleyMS.DataAccess.Repositories;

//namespace VolleyMS.BusinessLogic.Services
//{
//    public class JoinClubService
//    {
//        private readonly JoinClubRepository _joinClubRepository;
//        private readonly ClubService _clubService;
//        private readonly NotificationService _notificationService;
//        public JoinClubService(JoinClubRepository joinClubRepository, ClubService clubService, NotificationService notificationService)
//        {
//            _joinClubRepository = joinClubRepository;
//            _clubService = clubService;
//            _notificationService = notificationService;
//        }
//        public async Task<Guid> ApproveClubJoinRequest(Guid clubId, Guid requestId, ClubMemberRole clubMemberRole, Guid responseUserId)
//        {
//            var joinRequest = await _joinClubRepository.GetJoinRequestAsync(requestId) ?? throw new Exception("Join request not found");
//            var userId = joinRequest.UserId;
//            await _clubService.AddMember(clubId, userId, clubMemberRole);

//            var ResponseUser = await _userService.Get(responseUserId) ?? throw new Exception("User not found");
//            var Club = await _clubService.Get(clubId) ?? throw new Exception("Club not found");

//            await _joinClubRepository.ApproveClubJoinRequest(joinRequest.Id);

//            // Create and send notification about join request APPROVAL
//            var notification = new NotificationRequest
//            {
//                //Basic info
//                NotificationCategory = NotificationCategory.ClubJoinApproved,
//                Receivers = new List<Guid> { userId },
//                Text = $"User {ResponseUser.Name} {ResponseUser.Surname} has approved your request to join {Club.Name}!",
//                LinkedURL = $"/club/{clubId}",
//                //Payload 
//                ClubId = clubId,
//                ClubName = Club.Name
//            };
//            await _notificationService.SendNotification(notification, responseUserId);
//            return joinRequest.UserId;
//        }

//        public async Task<Guid> RejectClubJoinRequest(Guid clubId, Guid requestId, Guid responseUserId)
//        {
//            var Club = await _clubService.Get(clubId) ?? throw new Exception("Club not found");
//            var JoinRequestEntity = await _joinClubRepository.GetJoinRequestAsync(requestId) ?? throw new Exception("Join request not found");

//            await _joinClubRepository.RejectClubJoinRequest(JoinRequestEntity.Id);

//            // Create and send notification about join request REJECTION
//            var notification = new NotificationRequest
//            {
//                //Basic info
//                NotificationCategory = NotificationCategory.ClubJoinRejected,
//                Receivers = new List<Guid> { JoinRequestEntity.UserId },
//                Text = $"{Club.Name} has rejected your join request!",
//                LinkedURL = $"/club/{clubId}",
//                //Payload 
//                ClubId = clubId,
//                ClubName = Club.Name
//            };
//            await _notificationService.SendNotification(notification, responseUserId);
//            return JoinRequestEntity.UserId;
//        }
//    }
//}
