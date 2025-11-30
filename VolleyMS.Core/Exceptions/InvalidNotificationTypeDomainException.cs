namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class InvalidNotificationTypeDomainException : DomainException
    {
        public InvalidNotificationTypeDomainException()
        {
        }

        public InvalidNotificationTypeDomainException(string? message) : base(message)
        {
        }

        public InvalidNotificationTypeDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}