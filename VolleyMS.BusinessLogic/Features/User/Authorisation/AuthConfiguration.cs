namespace VolleyMS.BusinessLogic.Features.Users.Authorisation
{
    public class AuthConfiguration
    {
        public TimeSpan Expires { get; set; }
        public string SecretKey {  get; set; }
    }
}
