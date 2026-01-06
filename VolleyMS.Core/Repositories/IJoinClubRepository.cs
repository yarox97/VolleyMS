using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IJoinClubRepository : IGenericRepository<JoinClubRequest>
    {
        public Task<List<JoinClubRequest>> GetUserRequests(Guid userId);
    }
}
