using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        public ClubRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }
        public async Task<bool> IfJoinCodeTaken(string joinCode)
        {
            return await _volleyMsDbContext.Clubs.AnyAsync(c => c.JoinCode == joinCode);
        }

        public async Task<Club?> GetByJoinCodeAsync(string joinCode)
        {
            return await _volleyMsDbContext.Clubs
            .Include(c => c.JoinClubRequests)
            .Include(c => c.UserClubs)
            .FirstOrDefaultAsync(c => c.JoinCode == joinCode);
        }

        public async Task<Club?> GetByIdWithMembersAsync(Guid clubId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Clubs
                .Include(c => c.UserClubs) 
                .FirstOrDefaultAsync(c => c.Id == clubId);
        }
        public async Task<List<User>> GetAllUsers(Guid clubId)
        {
            return await _volleyMsDbContext.UserClubs
            .AsNoTracking()
            .Where(uc => uc.ClubId == clubId)
            .Select(uc => uc.User!)           
            .ToListAsync();
        }
        public async Task<List<User>> GetUsersByRole(Guid clubId, params ClubMemberRole[] clubMemberRoles)
        {
            return await _volleyMsDbContext.UserClubs
            .Where(uc => uc.ClubId == clubId && clubMemberRoles.Contains(uc.ClubMemberRole))
            .Select(uc => uc.User)
            .ToListAsync();
        }
        public async Task<bool> ContainsUser(Guid clubId, Guid? userId)
        {
            return await _volleyMsDbContext.UserClubs
                .AnyAsync(uc => uc.Club.Id == clubId && uc.User.Id == userId);
        }

        public async Task<IList<JoinClubRequest>> GetAllJoinRequestsAsync(Guid clubId, CancellationToken cancellation)
        {
            return await _volleyMsDbContext.JoinClubRequests
                .Include(x => x.Club)
                .ThenInclude(c => c.UserClubs)
                .Where(jc => jc.Club.Id == clubId)
                .ToListAsync(cancellation);
        }
        public async Task<JoinClubRequest?> GetJoinRequestByIdAsync(Guid joinRequestId, CancellationToken cancellation)
        {
            return await _volleyMsDbContext.JoinClubRequests
                .Include(x => x.Club)
                .ThenInclude(c => c.UserClubs)
                .FirstOrDefaultAsync(jc => jc.Id == joinRequestId);
        }
    }
}
