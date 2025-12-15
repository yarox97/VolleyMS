using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;


namespace VolleyMS.DataAccess.Repositories
{
    public class JoinClubRepository : GenericRepository<JoinClubRequest>, IJoinClubRepository
    {
        public JoinClubRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }
        public async Task<IList<JoinClubRequest>> GetPendingByClubIdAsync(Guid clubId)
        {
            return await _volleyMsDbContext.JoinClubRequests
                .Where(jc => jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending)
                .ToListAsync();
        }
    }
}
