using System;
using System.Threading;

namespace Poc.EventStore.Events
{
    public abstract class DomainEventBase : IDomainEvent
    {
        /// <inheritdoc />
        public string StreamId { get; }
        /// <inheritdoc />
        public string EventCreator { get; private set; }//should be private set for JsonDeserialize
        /// <inheritdoc />
        public Guid EventId { get; private set; }//should be private set for JsonDeserialize
        /// <inheritdoc />
        public DateTime EventTime { get; private set; }//should be private set for JsonDeserialize
        /// <inheritdoc />
        public virtual string EventTypeName => GetType().Name;

        protected DomainEventBase(string streamId)
        {
            StreamId = streamId;
            //will be overwritten on rehadration of the event.
            EventCreator = Environment.UserName;
            EventId = Guid.NewGuid();
            EventTime = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Stream: {StreamId} raised: {EventTypeName} on {EventTime}";
        }
    }
}