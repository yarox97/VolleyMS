using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.GetTaskTrends
{
    public sealed record GetTaskTrendsQuery(Guid userId) : IRequest<Result<TaskTrendsDto>>
    {
    }
}
