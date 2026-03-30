using MediatR;

namespace VolleyMS.Core.DomainEvents.Tasks
{
    public record UserTaskEvaluatedDomainEvent(
        Guid UserId,
        Guid TaskId,
        bool ApplyPenalty,
        decimal? PenaltyAmount,
        PenaltyType PenaltyType,
        string Reason
    ) : IDomainEvent
    { }
}
