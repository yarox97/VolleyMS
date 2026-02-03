namespace VolleyMS.Core.Shared
{
    public  interface IValidationResult
    {
        public static readonly Error ValidationError = new Error(
            code: "ValidationError",
            message: "One or more validation errors occurred."
        );

        Error[] Errors { get; }
    }
}
