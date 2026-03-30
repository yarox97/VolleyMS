using MediatR;
using System.Globalization;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.GetTaskTrends
{
    public sealed class GetTaskTrendsQueryHandler : IRequestHandler<GetTaskTrendsQuery, Result<TaskTrendsDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskTrendsQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        async Task<Result<TaskTrendsDto>> IRequestHandler<GetTaskTrendsQuery, Result<TaskTrendsDto>>.Handle(GetTaskTrendsQuery request, CancellationToken cancellationToken)
        {
            var rawStats = await _taskRepository.GetStatsForLastMonthsAsync(request.userId, 6);

            var result = new TaskTrendsDto();
            var today = DateTime.UtcNow;

            for (int i = 5; i >= 0; i--)
            {
                var date = today.AddMonths(-i);
                result.Labels.Add(date.ToString("MMM", CultureInfo.InvariantCulture));

                var tasksCount = rawStats
                    .Where(s => s.Year == date.Year && s.Month == date.Month && s.TaskType != "Match")
                    .Sum(s => s.Count);

                var matchesCount = rawStats
                    .Where(s => s.Year == date.Year && s.Month == date.Month && s.TaskType == "Match")
                    .Sum(s => s.Count);

                result.TasksTrend.Add(tasksCount);
                result.MatchesTrend.Add(matchesCount);
            }

            return Result.Success(result);
        }
    }
}
