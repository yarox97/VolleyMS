using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using VolleyMS.Core.DomainEvents;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Outbox;

namespace VolleyMS.BusinessLogic.BackgorundJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly VolleyMsDbContext _context;
        private readonly IPublisher _publisher;
        public ProcessOutboxMessagesJob(VolleyMsDbContext volleyMsDbContext, IPublisher publisher)
        {
            _context = volleyMsDbContext;
            _publisher = publisher;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _context
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null)
                .Take(20)
                .ToListAsync(context.CancellationToken);

            foreach (var message in messages)
            {
                var domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                if (domainEvent == null) continue;

                await _publisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
