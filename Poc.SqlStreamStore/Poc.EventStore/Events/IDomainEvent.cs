using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.EventStore.Events
{
    public interface IDomainEvent
    {
        /// <summary>
        /// This field is used to associate the particular event to a specific aggregate root (=stream).
        /// </summary>
        string StreamId { get; }
        /// <summary>
        /// This field can be used to build audit logs.
        /// </summary>
        string EventCreator { get; }
        /// <summary>
        /// The event identifier
        /// At its simplest, this field can be used to tie a series of events back to their originating command. However, it can also be used to ensure the idempotence of the event.
        /// </summary>
        Guid EventId { get; }

        DateTime EventTime { get; }

        string EventTypeName { get; }
    }
}
