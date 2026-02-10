using VolleyMS.Core.Models;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        public Task<Contract?> GetActiveContractByUserAndClubAsync(Guid userId, Guid clubId, CancellationToken cancellationToken);
        Task<(IEnumerable<Contract> Items, int TotalCount)> GetUserContractsPaged(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

        Task<(IEnumerable<Contract> Items, int TotalCount)> GetClubContractsPaged(
        Guid clubId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

        Task<Contract> GetByIdWithDetailsAsync(Guid ClubId, CancellationToken cancellation);
    }
}
