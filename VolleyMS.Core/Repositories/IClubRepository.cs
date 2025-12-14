
using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        Task<bool> IfJoinCodeTaken(string joinCode);
        Task<List<User>> GetAllUsers(Guid clubId);
        Task<Club?> GetByIdWithMembersAsync(Guid clubId, CancellationToken cancellationToken);
        Task<List<User>> GetUsersByRole(Guid clubId, params ClubMemberRole[] clubMemberRoles);
        Task<bool> ContainsUser(Guid clubId, Guid? userId);
    }
}
