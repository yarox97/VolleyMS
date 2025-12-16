using VolleyMS.BusinessLogic.Contracts.DTOs;

namespace VolleyMS.BusinessLogic.Features.Users.Get
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }

        public IList<UserClubDto>? clubDtos { get; set; } = new List<UserClubDto>();
        public DateTime? createdAt { get; set; }
    }
}
