using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
using VolleyMS.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService
    {
        private readonly ClubRepository _clubRepository;
        private readonly NotificationService _notificationService;

        public ClubService(ClubRepository clubRepository, NotificationService notificationService)
        {
            _clubRepository = clubRepository;
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
            return await _clubRepository.GetById(clubId) ?? throw new Exception("No club was found!");
        }

        public async Task AddMember(Guid clubId, Guid userId, ClubMemberRole clubMemberRole)
        {
            await _clubRepository.AddUser(userId, clubId, clubMemberRole);

            //Send notification to added member
        }

        public async Task DeleteMember(Guid clubId, Guid userId, Guid responseUserId)
        {
            await _clubRepository.DeleteUser(clubId, userId);

            //Create notification to user who was deleted from team
            var club = await _clubRepository.GetById(clubId);
            _ = club ?? throw new Exception("Can't find a club");
            await _notificationService.SendNotification(new NotificationRequest()
            {
                NotificationCategory = NotificationCategory.Informative,
                Receivers = new List<Guid> { userId },
                Text = $"You have been removed from the club {club.Name}.",
            }, responseUserId);
        }

        public async Task<IList<User>> GetAllUsers(Guid clubId)
        {
            return await _clubRepository.GetAllUsers(clubId);
        }

        public async Task<List<Guid>> GetUsersIdsByRole(Guid clubId, params ClubMemberRole[] clubMemberRole)
        {
            return await _clubRepository.GetUsersIdsByRole(clubId, clubMemberRole);
        }

        public async Task<bool> ContainsUser(Guid clubId, Guid userId)
        {
            return await _clubRepository.ContainsUser(clubId, userId);
        }
    }
}
