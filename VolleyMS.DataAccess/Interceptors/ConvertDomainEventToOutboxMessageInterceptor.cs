using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Outbox;

namespace VolleyMS.DataAccess.Interceptors
{
    public sealed class ConvertDomainEventToOutboxMessageInterceptor
        : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;

            if (dbContext is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var events = dbContext.ChangeTracker
                .Entries<BaseEntity>()
                .Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccuredOnUtc = DateTime.UtcNow,
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
                })
                .ToList();

            if (events.Any())
            {
                dbContext.Set<OutboxMessage>().AddRange(events);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}