using System;
using System.Collections.Generic;
using System.Linq;
using Poc.EventStore.Abstractions;
using Poc.EventStore.Events;

namespace Poc.Domain
{
    public abstract class EventSourcingAggregate : IEventSourcingAggregate
    {
        public const int NewStreamVersion = -1;

        private readonly ICollection<IDomainEvent> _uncommittedEvents = new LinkedList<IDomainEvent>();
        private int _streamVersion = NewStreamVersion;
        public string StreamId { get; }
        public int StreamVersion => _streamVersion;
        public string TypeName => GetType().Name;
        protected EventSourcingAggregate(string streamId)
        {
            if (string.IsNullOrEmpty(streamId))
                throw new ArgumentNullException(nameof(streamId), "Stream should have a streamId, pass in the streamid.");
            if (!streamId.StartsWith(TypeName))
                streamId = string.Concat(TypeName, streamId);
            StreamId = streamId;
        }
        void IEventSourcingAggregate.ApplyEvent(IDomainEvent @event, int streamVersion)
        {
            if (!_uncommittedEvents.Any(x => Equals(x.EventId, @event.EventId)))
            {
                ((dynamic)this).Apply((dynamic)@event);
                _streamVersion = streamVersion;
            }
        }

        void IEventSourcingAggregate.ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        IEnumerable<IDomainEvent> IEventSourcingAggregate.GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        protected void ApplyChange<TEvent>(TEvent @event)
            where TEvent : DomainEventBase
        {
            ((IEventSourcingAggregate)this).ApplyEvent(@event, _streamVersion + 1);
            _uncommittedEvents.Add(@event);
        }
    }
}
