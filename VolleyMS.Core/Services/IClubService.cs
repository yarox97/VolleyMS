namespace VolleyMS.Core.Services
{
    public interface IClubService
    {
        Task<string> GenerateJoinCode(CancellationToken cancellation);
    }
}
