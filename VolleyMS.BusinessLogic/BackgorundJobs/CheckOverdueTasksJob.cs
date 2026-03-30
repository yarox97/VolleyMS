using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using VolleyMS.Core.Enums; 
using VolleyMS.DataAccess;

namespace VolleyMS.BusinessLogic.BackgorundJobs
{
    [DisallowConcurrentExecution]
    public class CheckOverdueTasksJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CheckOverdueTasksJob> _logger;

        public CheckOverdueTasksJob(IServiceScopeFactory scopeFactory, ILogger<CheckOverdueTasksJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VolleyMsDbContext>();

            var now = DateTime.UtcNow;

            try
            {
                var overdueUserTasks = await dbContext.UserTasks
                    .Include(ut => ut.Task)
                    .Where(ut =>
                        ut.Status == TaskStatus.Uncompleted && 
                        ut.Task.TaskSchedule.EndTime != null &&
                        ut.Task.TaskSchedule.EndTime < now.TimeOfDay)
                    .ToListAsync(context.CancellationToken);

                if (!overdueUserTasks.Any())
                    return;

                int updatedCount = 0;

                foreach (var userTask in overdueUserTasks)
                {
                    userTask.Overdue();

                    updatedCount++;
                }

                if (updatedCount > 0)
                {
                    await dbContext.SaveChangesAsync(context.CancellationToken);
                    _logger.LogInformation($"Marked as overdue: {updatedCount} .");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while checking task overdue!");
                throw new JobExecutionException(ex);
            }
        }
    }
}