using System.Collections.Generic;
using Poc.EventStore.Events;

namespace Poc.Domain.MenuModule
{
    public class DrinksAdded : DomainEventBase
    {
        public List<string> Drinks { get; private set; }

        public DrinksAdded(string streamId, List<string> drinks) : base(streamId)
        {
            Drinks = drinks;
        }
    }
}