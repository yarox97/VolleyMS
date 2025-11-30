
using VolleyMS.Core.Exceptions;

namespace VolleyMS.DataAccess.Repositories
{
    [Serializable]
    public class UserNotFoundDomainException : DomainException
    {
        public UserNotFoundDomainException()
        {
        }

        public UserNotFoundDomainException(string? message) : base(message)
        {
        }

        public UserNotFoundDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}