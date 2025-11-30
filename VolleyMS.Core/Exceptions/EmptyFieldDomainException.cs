namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    public class EmptyFieldDomainException : DomainException
    {
        public EmptyFieldDomainException()
        {
        }

        public EmptyFieldDomainException(string? message) : base(message)
        {
        }

        public EmptyFieldDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}