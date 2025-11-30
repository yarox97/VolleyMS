namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class InvalidNotificationCategoryDomainException : DomainException
    {
        public InvalidNotificationCategoryDomainException()
        {
        }

        public InvalidNotificationCategoryDomainException(string? message) : base(message)
        {
        }

        public InvalidNotificationCategoryDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}