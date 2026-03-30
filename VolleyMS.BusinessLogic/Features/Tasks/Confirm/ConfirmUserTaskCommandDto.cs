namespace VolleyMS.BusinessLogic.Features.Tasks.Confirm
{
    public sealed record ConfirmUserTaskCommandDto
        (
        TaskStatus TaskStatus,
        bool ApplyPenalty
        );
}
