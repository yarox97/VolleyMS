using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;


namespace VolleyMS.DataAccess.Repositories
{
    public class JoinClubRepository : GenericRepository<JoinClub>, IJoinClubRepository
    {
        public JoinClubRepository(VolleyMsDbContext context) : base(context)
        {
        }
        public async Task<IList<JoinClub>> GetPendingByClubIdAsync(Guid clubId)
        {
            return await _context.JoinClubRequests
                .Where(jc => jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending)
                .ToListAsync();
        }
    }
}
