using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
using VolleyMS.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService
    {
        private readonly ClubRepository _clubRepository;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;

        public ClubService(ClubRepository clubRepository, UserService userService, NotificationService notificationService)
        {
            _clubRepository = clubRepository;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<string> GenerateJoinCode()
        {
            string joinCode = string.Empty;
            const string salt = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            do
            {
                joinCode = new string(Enumerable.Range(0, 9)
                                        .Select(_ => salt[random.Next(salt.Length)])
                                        .ToArray());

            } while (await _clubRepository.IfJoinCodeTaken(joinCode));

            return joinCode;
        }

        public async Task<Guid> Create(CreateClubRequest createClubRequest)
        {
            string joinCode = await GenerateJoinCode();
            var clubTuple = Club.Create(
                Guid.NewGuid(), 
                createClubRequest.Name, 
                joinCode,
                createClubRequest.Description,
                createClubRequest.AvatarURL,
                createClubRequest.BackGroundURL
                );

            if (clubTuple.error == string.Empty)
            {
                await _clubRepository.Create(clubTuple.club, createClubRequest.CreatorId);
                return clubTuple.club.Id;
            }
            else
            {
                throw new Exception(clubTuple.error);
            }
        }
        public async Task Delete(Guid clubId)
        {
            try
            {
                await _clubRepository.Delete(clubId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<Club?> GetClubByJoinCode(string joinCode)
        {
            return await _clubRepository.GetClubByCode(joinCode);
        }

        public async Task<Club?> GetById(Guid clubId)
        {
            return await _clubRepository.GetById(clubId);
        }

        public async Task AddMember(Guid userId, Guid clubId, Guid responseUserId)
        {
            var user = await _userService.GetById(userId);
            var club = await _clubRepository.GetById(clubId);
            _ = club ?? throw new Exception("Can't find a club");
            _ = user ?? throw new Exception("Can't add user to a club");

            if (!await _clubRepository.ContainsUser(club, user))
            {
                await _clubRepository.AddUser(user, club);

                // Send notification to a user whose request was approved
                await _notificationService.SendNotification(new NotificationRequest()
                {
                    NotificationCategory = NotificationCategory.ClubJoinApproved,
                    Receivers = new List<Guid> { userId },
                    Text = $"Your request to join club {club.Name} was approved.",
                }, responseUserId);
            }
            else
            {
                throw new Exception("User is already a member of the club");
            }
        }

        public async Task DeleteMember(Guid clubId, Guid userId, Guid responseUserId)
        {
            await _clubRepository.DeleteUser(clubId, userId);

            //Create notification to user who was deleted from team
            var club = await _clubRepository.GetById(clubId);
            _ = club ?? throw new Exception("Can't find a club using provided join code");
            await _notificationService.SendNotification(new NotificationRequest()
            {
                NotificationCategory = NotificationCategory.Informative,
                Receivers = new List<Guid> { userId },
                Text = $"You have been removed from the club {club.Name}.",
            }, responseUserId);
        }

        public async Task<List<Guid>> GetUsersIdsByRole(Guid clubId, params ClubMemberRole[] clubMemberRole)
        {
            return await _clubRepository.GetUsersIdsByRole(clubId, clubMemberRole);
        }
    }
}
