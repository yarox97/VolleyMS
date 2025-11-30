namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class InvalidUserTypeDomainException : DomainException
    {
        public InvalidUserTypeDomainException()
        {
        }

        public InvalidUserTypeDomainException(string? message) : base(message)
        {
        }

        public InvalidUserTypeDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}