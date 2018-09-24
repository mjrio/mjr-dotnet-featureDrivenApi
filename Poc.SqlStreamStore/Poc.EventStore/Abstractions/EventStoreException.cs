using System;
using System.Net;

namespace Poc.EventStore.Abstractions
{
    [Serializable]
    public abstract class EventStoreException : Exception
    {
        protected EventStoreException(string title) : base(title) { }
        protected EventStoreException(string title, Exception inner = default(Exception)) : base(title,  inner) { }

        protected EventStoreException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DuplicateEventStoreAggregateException : EventStoreException
    {
        public DuplicateEventStoreAggregateException(string message) : base(message) { }
        public DuplicateEventStoreAggregateException(string message, Exception inner) : base(message,  inner) { }

    }

    [Serializable]
    public class EventStoreCommunicationException : EventStoreException
    {
        public EventStoreCommunicationException(string message) : base(message) { }
        public EventStoreCommunicationException(string message, Exception inner) : base(message,  inner) { }

    }

    [Serializable]
    public class EventStoreApplyEventException : EventStoreException
    {
        public EventStoreApplyEventException(Guid messageId, string streamId, int streamVersion, Guid eventId, string eventTypeName, Exception inner)
            : base($"MessageId: {messageId} - Event with id: {eventId} and type: {eventTypeName} could not be applied to aggregate: {streamId} with streamversion {streamVersion}, please see inner exception for more details. Ask to replay the event or to remove the broken event.", inner) { }
    }
}
