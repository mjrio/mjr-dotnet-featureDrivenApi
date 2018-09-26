using Poc.EventStore.Events;

namespace Poc.Domain.MenuModule
{
    public class MenuCreated : DomainEventBase
    {
        public string Name { get; private set; }

        public MenuCreated(string streamId, string name) : base(streamId)
        {
            Name = name;
        }
    }
}