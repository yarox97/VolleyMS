using VolleyMS.Core.Models;

namespace VolleyMS.Core.Requests
{
    public class AddUserToClubRequest
    {
        public Guid UserId { get; set; } 
        public string JoinCode { get; set; } = string.Empty;
    } 
}
