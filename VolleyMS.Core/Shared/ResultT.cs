namespace VolleyMS.Core.Shared
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;
        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            if (isSuccess && value == null!)
            {
                throw new InvalidOperationException("A successful result must have a value.");
            }
            _value = value;
        }
        public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
