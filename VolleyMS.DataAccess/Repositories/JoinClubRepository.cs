using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Abstractions;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Repositories
{
    public class JoinClubRepository : IJoinClubRepository
    {
        private readonly VolleyMsDbContext _context;
        public JoinClubRepository(VolleyMsDbContext context)
        {
            _context = context;
        }

        public async Task AddJoinRequestAsync(Guid clubId, Guid userId)
        {
            var joinClubRequest = new JoinClubEntity
            {
                ClubId = clubId,
                UserId = userId,
                CreatorId = userId,
                requestStatus = JoinClubRequestStatus.Pending
            };
            await _context.JoinClubRequests.AddAsync(joinClubRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<JoinClubEntity?> GetJoinRequestAsync(Guid requestId)
        {
            return await _context.JoinClubRequests
                .FirstOrDefaultAsync(jc => jc.Id == requestId);
        }

        public async Task<IList<JoinClubEntity>> GetClubJoinRequests(Guid clubId)
        {
            return await _context.JoinClubRequests
                .Where(jc => jc.ClubId == clubId && jc.requestStatus == JoinClubRequestStatus.Pending)
                .ToListAsync();
        }

        public async Task<Guid> ApproveClubJoinRequest(Guid requestId)
        {
            var joinRequest = await _context.JoinClubRequests
                .FirstOrDefaultAsync(jc => jc.Id == requestId);

            if (joinRequest == null) throw new Exception("Join request not found");

            joinRequest.requestStatus = JoinClubRequestStatus.Approved;
            joinRequest.UpdatedAt = DateTime.UtcNow;

            _context.JoinClubRequests.Update(joinRequest);
            await _context.SaveChangesAsync();

            return joinRequest.UserId;
        }

        public async Task<Guid> RejectClubJoinRequest(Guid requestId)
        {
            var joinRequest = await _context.JoinClubRequests
                .FirstOrDefaultAsync(jc => jc.Id == requestId);

            if (joinRequest == null) throw new Exception("Join request not found");

            joinRequest.requestStatus = JoinClubRequestStatus.Rejected;
            joinRequest.UpdatedAt = DateTime.UtcNow;

            _context.JoinClubRequests.Update(joinRequest);
            await _context.SaveChangesAsync();

            return joinRequest.UserId;
        }
    }
}
