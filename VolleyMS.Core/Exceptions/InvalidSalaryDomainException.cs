namespace VolleyMS.Core.Exceptions
{
    [Serializable]
    internal class InvalidSalaryDomainException : Exception
    {
        public InvalidSalaryDomainException()
        {
        }

        public InvalidSalaryDomainException(string? message) : base(message)
        {
        }

        public InvalidSalaryDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}