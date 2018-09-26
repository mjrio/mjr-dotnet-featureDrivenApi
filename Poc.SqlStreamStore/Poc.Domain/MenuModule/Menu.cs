using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poc.Domain.MenuModule
{
    public class Menu : EventSourcingAggregate
    {
        public string Name { get; private set; }
        public List<string> Drinks { get; private set; }
        public Menu(string streamId) : base(streamId)
        {
        }

        public static Menu Create(string name)
        {
            var menu = new Menu(name);
            menu.ApplyChange(new MenuCreated(menu.StreamId, name));
            return menu;
        }

        internal void Apply(MenuCreated @event)
        {
            Name = @event.Name;
        }

        public void AddDrinks(params string[] drinks)
        {
           ApplyChange(new DrinksAdded(StreamId,drinks.ToList()));
        }

        internal void Apply(DrinksAdded @event)
        {
            Drinks = @event.Drinks;
        }
    }
}
