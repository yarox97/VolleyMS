namespace VolleyMS.Core.Requests
{
    public class CreateClubRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AvatarURL { get; set; } = string.Empty;
        public string BackGroundURL { get; set; } = string.Empty;
    }
}
