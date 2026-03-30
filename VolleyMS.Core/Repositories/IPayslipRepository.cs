using VolleyMS.Core.Models.Payslips;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Repositories
{
    public interface IPayslipRepository : IGenericRepository<Payslip>
    {
        public Task<Payslip?> GetByUserIdAndMonthYearAsync(Guid userId, int month, int year, CancellationToken cancellationToken);
        public Task<PaginatedResult<Payslip>> GetByUserIdWithDetailsAsync(
            Guid userId,
            int page,
            int pageSize,
            Guid? contractId = null,
            CancellationToken cancellationToken = default);

        public Task<PaginatedResult<PayslipAdjustment>> GetByPayslipIdWithDetailsAsync(
            Guid payslipId,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        public Task<PaginatedResult<Payslip>> GetByClubIdWithDetailsAsync(
            Guid clubId,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        public Task<PaginatedResult<Payslip>> GetByContractIdWithDetailsAsync(Guid contractId, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<PayslipAdjustment>> GetAdjustmentsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
        public Task<Payslip?> GetPayslipByIdWithDetailsAsync(Guid payslipId, CancellationToken cancellationToken);
    }
}
