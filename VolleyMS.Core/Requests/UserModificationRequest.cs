namespace VolleyMS.Core.Requests
{
    public class UserModificationRequest
    {
        public string Surname {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public UserType userType { get; set; }
    }
}
