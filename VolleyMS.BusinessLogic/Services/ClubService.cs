using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
using VolleyMS.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService
    {
        private readonly ClubRepository _clubRepository;

        public ClubService(ClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        // GENERATE JOIN CODE
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
        // --------------------------------------------------------------

        // MANAGE CLUB
        public async Task<Guid> Create(CreateClubRequest createClubRequest)
        {
            string joinCode = await GenerateJoinCode();
            var club = Club.Create(
                Guid.NewGuid(),
                createClubRequest.Name,
                joinCode,
                createClubRequest.Description,
                createClubRequest.AvatarURL,
                createClubRequest.BackGroundURL
                );

            await _clubRepository.Create(club, createClubRequest.CreatorId);
            return club.Id;
        }
        public async Task Delete(Guid clubId)
        {
            await _clubRepository.Delete(clubId);
        }
        // --------------------------------------------------------------


        // GET CLUB BY JOIN CODE OR ID
        public async Task<Club?> Get(string joinCode)
        {
            return await _clubRepository.Get(joinCode);
        }
        public async Task<Club?> Get(Guid clubId)
        {
            return await _clubRepository.Get(clubId) ?? throw new Exception("No club was found!");
        }
        // --------------------------------------------------------------


        // MANAGE CLUB MEMBERS
        public async Task<Guid?> AddMember(Guid clubId, Guid userId, ClubMemberRole clubMemberRole)
        {
            if(!await _clubRepository.ContainsUser(clubId, userId))
            {
                await _clubRepository.AddUser(userId, clubId, clubMemberRole);
                return userId;
            }
            return null;
        }
        public async Task<Guid> DeleteMember(Guid clubId, Guid userId, Guid responseUserId)
        {
            await _clubRepository.DeleteUser(clubId, userId);
            // Send notification to removed member
            return userId;
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
