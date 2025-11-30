namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class InvalidClubMemeberRoleDomainException : DomainException
    {
        public InvalidClubMemeberRoleDomainException()
        {
        }

        public InvalidClubMemeberRoleDomainException(string? message) : base(message)
        {
        }

        public InvalidClubMemeberRoleDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}