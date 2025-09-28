using System.Reflection.Metadata;
using VolleyMS.Core.Common;
using static System.Net.Mime.MediaTypeNames;

namespace VolleyMS.Core.Models
{
    public class Club : AuditableFields
    {
        private Club(Guid id, string name, string? description, string? avatarUrl, string? backGroundUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            AvatarURL = avatarUrl;
            BackGroundURL = backGroundUrl;
        }
        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string? Description {  get; }
        public string? AvatarURL { get; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // To make a dictionary avatar <-> path  
        public string? BackGroundURL { get; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // Change to a real default bg pic from dict path

        public static (Club club, string error) Create(Guid id, string name, string? description, string? avatarUrl, string? backGroundUrl)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                error = "Team name can't be empty!";
            }
            avatarUrl = avatarUrl ?? "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg";
            backGroundUrl = backGroundUrl ?? "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg";

            var Club = new Club(id, name, description, avatarUrl, backGroundUrl);
            return (Club, error);
        }
    }
}