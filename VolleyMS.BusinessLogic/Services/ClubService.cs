using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        public async Task<string> GenerateJoinCode(CancellationToken cancellation)
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
    }
}
