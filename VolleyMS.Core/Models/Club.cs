using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class Club : BaseEntity
    {
        private Club(Guid id, 
            string name, 
            string joinCode, 
            string? description, 
            string? avatarUrl, 
            string? backGroundUrl)
            : base(id)
        {
            Name = name;
            Description = description;
            JoinCode = joinCode;
            AvatarURL = avatarUrl;
            BackGroundURL = backGroundUrl;
        }
        public string Name { get; } = string.Empty;
        public string JoinCode { get; private set; } = string.Empty;
        public string? Description {  get; }
        public string? AvatarURL { get; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // To make a dictionary avatar <-> path  
        public string? BackGroundURL { get; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // Change to a real default bg pic from dict path

        public static Club Create(Guid id, string name, string joinCode, string? description, string? avatarUrl, string? backGroundUrl)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                throw new EmptyFieldDomainException("Club name cannot be null or empty");
            }
            avatarUrl = avatarUrl ?? "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg";
            backGroundUrl = backGroundUrl ?? "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg";

            var Club = new Club(id, name, joinCode, description, avatarUrl, backGroundUrl);
            return Club;
        }
    }
}