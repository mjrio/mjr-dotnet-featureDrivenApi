using Poc.EventStore.Abstractions;
using Poc.EventStore.Events;

namespace Poc.Domain.FundModule
{
    public class FundDesigned : DomainEventBase
    {
        public string Isin { get; private set; }
        public string Currency { get; private set; }
        public string Name { get; private set; }

        public FundDesigned(string streamId, string isin, string currency, string name) : base(streamId)
        {
            Isin = isin;
            Currency = currency;
            Name = name;
        }
    }
    public class Fund : EventSourcingAggregate
    {
        public string Isin { get; private set; }
        public string Currency { get; private set; }
        public string Name { get; private set; }

        public Fund(string streamId) : base(streamId)
        {
        }

        public static void DesignFund(string isin, string currency, string name)
        {
            var fund = new Fund(isin);
            fund.ApplyChange(new FundDesigned(fund.StreamId,isin,currency,name));
        }

        protected internal void Apply(FundDesigned @event)
        {
            Isin = @event.Isin;
            Currency = @event.Currency;
            Name = @event.Name;
        }
    }
}
