using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Poc.EventStore.Events;

namespace Poc.EventStore.Abstractions
{
    public interface IEventStore
    {
        Task<T> GetByIdAsync<T>(string streamId, int fromVersionInclusive)
            where T : IEventSourcingAggregate;
        Task Save(IEventSourcingAggregate eventSourcingAggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        Task ReadStreamForwards(string streamId, int fromVersionInclusive, int readPageSize, bool prefetchData, Func<IDomainEvent, int, Task> applyEvent);
        Task<IEnumerable<string>> ListStreams(string startsWith, int maxCount = 100, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteStream(string streamId);
        Task<int> ReadLastStreamVersion(string streamId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
