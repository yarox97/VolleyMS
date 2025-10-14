using VolleyMS.Core.Models;

namespace VolleyMS.Contracts
{
    public class AddUserToClubRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
    } 
}
