using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VolleyMS.Core.Shared
{
    public sealed class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }

        public string Message { get; }

        public static implicit operator string(Error error) => error.Code;
        public static bool operator !=(Error? left, Error? right) => !(left == right);
        public static bool operator ==(Error? left, Error? right)
        {
            if(left is null && right is null) return true;
            if(left is null || right is null) return false;

            return left.Equals(right);
        }
        public bool Equals(Error? other)
        {
            if(other is null) return false;

            return Code == other.Code && Message == other.Message;
        }

        public override bool Equals(object? obj) => obj is Error other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Code, Message);
        public override string ToString() => Code;
    }
}
