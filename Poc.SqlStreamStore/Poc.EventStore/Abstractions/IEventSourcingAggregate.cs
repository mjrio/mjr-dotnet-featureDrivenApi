using System.Collections.Generic;
using Poc.EventStore.Events;

namespace Poc.EventStore.Abstractions
{
    public interface IEventSourcingAggregate
    {
        string StreamId { get; }
        int StreamVersion { get; }
        void ApplyEvent(IDomainEvent @event, int streamVersion);
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}
