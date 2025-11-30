namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class DateOutOfBoundsDomainException : DomainException
    {
        public DateOutOfBoundsDomainException()
        {
        }

        public DateOutOfBoundsDomainException(string? message) : base(message)
        {
        }

        public DateOutOfBoundsDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}