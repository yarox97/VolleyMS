namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateReceivers
{
    public sealed record UpdateTaskReceiversRequest(IEnumerable<Guid> NewReceiverIds)
    {
    }
}
