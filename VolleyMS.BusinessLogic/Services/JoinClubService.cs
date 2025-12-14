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
//        public JoinClubService(JoinClubRepository joinClubRepository ,ClubService clubService, NotificationService notificationService)
//        {
//            _joinClubRepository = joinClubRepository;
//            _clubService = clubService;
//            _notificationService = notificationService;
//        }

//        public async Task<Guid> RequestToJoinClubAsync(string joinCode, string userName)
//        {
//            var user = await _userService.Get(userName) ?? throw new Exception("User not found");
//            var club = await _clubService.Get(joinCode) ?? throw new Exception("Club not found");

//            if(await _clubService.ContainsUser(club.Id, user.Id))
//            {
//                throw new Exception("User is already a member of the club");
//            }

//            // Send notification to club moderators (president and coaches)
//            var usersByRole = await _clubService.GetUsersIdsByRole( // CAN TO IMPLEMENTED MORE FLEXIBLY IN THE FUTURE (to make dynamic roles)
//                club.Id,
//                ClubMemberRole.President,
//                ClubMemberRole.Coach);

//            if (usersByRole == null || !usersByRole.Any())
//            {
//                throw new Exception("No recipients found");
//            }

//            // Add join request to the database
//            await _joinClubRepository.AddJoinRequestAsync(club.Id, user.Id);

//            // Create and send notification about join request
//            var notification = new NotificationRequest // DOMAIN EVENT!
//            {
//                //Basic info
//                NotificationCategory = NotificationCategory.ClubJoinRequest,
//                Receivers = usersByRole.ToList(),
//                Text = $"{user.Name} {user.Surname} requested to join your club {club.Name}",
//                LinkedURL = $"/user/{user.UserName}",
//                //Payload
//                ClubName = club.Name,
//                SenderName = user.Name,
//                SenderSurname = user.Surname,
//                SenderUserName = user.UserName,
//                ClubId = club.Id
//            };
//            await _notificationService.SendNotification(notification, user.Id);
//            return user.Id;
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

//        public async Task<IList<JoinClub>> GetAll(Guid clubId)
//        {
//            var joinRequestsEntities = await _joinClubRepository.GetClubJoinRequests(clubId);
//            if (joinRequestsEntities == null) return new List<JoinClub>();

//            var joinRequests = new List<JoinClub>();
//            foreach(var request in joinRequestsEntities)
//            {
//                joinRequests.Add(JoinClubMapper.ToDomain(request));
//            }
//            return joinRequests;
//        }
//    }
//}
