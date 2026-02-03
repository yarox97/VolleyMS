namespace VolleyMS.Core.Shared
{
    public sealed class ValidationResultT<TValue> : Result<TValue>, IValidationResult
    {
        private ValidationResultT(Error[] errors)
            : base(default, false, IValidationResult.ValidationError)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResultT<TValue> WithErrors(Error[] errors)
        {
            return new ValidationResultT<TValue>(errors);
        }
    }
}
