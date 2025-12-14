using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Repositories;
using Club = VolleyMS.Core.Models.Club;
using User = VolleyMS.Core.Models.User;

namespace VolleyMS.DataAccess.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        public ClubRepository(VolleyMsDbContext context) : base(context)
        {
        }
        public async Task<bool> IfJoinCodeTaken(string joinCode)
        {
            return await _context.Clubs.AnyAsync(c => c.JoinCode == joinCode);
        }

        public async Task<Club?> GetByIdWithMembersAsync(Guid clubId, CancellationToken cancellationToken)
        {
            return await _context.Clubs
                .Include(c => c.UserClubs) 
                .FirstOrDefaultAsync(c => c.Id == clubId);
        }
        public async Task<List<User>> GetAllUsers(Guid clubId)
        {
            return await _context.UserClubs
            .AsNoTracking()
            .Where(uc => uc.ClubId == clubId)
            .Select(uc => uc.User!)           
            .ToListAsync();
        }
        public async Task<List<User>> GetUsersByRole(Guid clubId, params ClubMemberRole[] clubMemberRoles)
        {
            return await _context.UserClubs
            .Where(uc => uc.ClubId == clubId && clubMemberRoles.Contains(uc.ClubMemberRole))
            .Select(uc => uc.User)
            .ToListAsync();
        }
        public async Task<bool> ContainsUser(Guid clubId, Guid? userId)
        {
            return await _context.UserClubs
                .AnyAsync(uc => uc.Club.Id == clubId && uc.User.Id == userId);
        }
    }
}
