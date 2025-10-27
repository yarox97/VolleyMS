using VolleyMS.Core.Models;

namespace VolleyMS.Contracts
{
    public class AddUserToClubRequest
    {
        public Guid UserId { get; set; } 
        public string JoinCode { get; set; } = string.Empty;
    } 
}
