using System;
using System.Collections.Generic;
using Poc.EventStore.Events;

namespace Poc.Domain.GameModule
{
    public class Snake : EventSourcingAggregate
    {
        public List<Position> Positions = new List<Position>();
        public int GameNr { get; private set; }
        public Snake(string streamId) : base(streamId)
        {

        }

        public static Snake Start(int gameNr)
        {
            var menu = new Snake(gameNr.ToString());
            menu.ApplyChange(new SnakeStarted(menu.StreamId, gameNr));
            return menu;
        }

        protected internal void Apply(SnakeStarted @event)
        {
            GameNr = @event.GameNr;
        }
        public void Up()
        {
            ApplyChange(new WentUp(StreamId));
        }
        protected internal void Apply(WentUp @event)
        {
            Positions.Add(Position.Up);
        }
        public void Down()
        {
            ApplyChange(new WentDown(StreamId));
        }
        protected internal void Apply(WentDown @event)
        {
            Positions.Add(Position.Down);
        }
        public void Left()
        {
            ApplyChange(new WentLeft(StreamId));
        }
        protected internal void Apply(WentLeft @event)
        {
            Positions.Add(Position.Left);
        }
        public void Right()
        {
            ApplyChange(new WentRight(StreamId));
        }
        protected internal void Apply(WentRight @event)
        {
            Positions.Add(Position.Right);
        }
        
    }

    public struct Position : IEquatable<Position>
    {
        public static Position Right = new Position(0, 1);
        public static Position Left = new Position(0, -1);
        public static Position Down = new Position(1, 0);
        public static Position Up = new Position(-1, 0);
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Position a, Position b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Position a, Position b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Position other)
        {
            return row == other.row && col == other.col;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (row * 397) ^ col;
            }
        }
    }

    public class WentUp : DomainEventBase
    {

        public WentUp(string streamId) : base(streamId)
        {
        }
    }
    public class WentRight : DomainEventBase
    {

        public WentRight(string streamId) : base(streamId)
        {
        }
    }
    public class WentLeft : DomainEventBase
    {

        public WentLeft(string streamId) : base(streamId)
        {
        }
    }
    public class WentDown : DomainEventBase
    {

        public WentDown(string streamId) : base(streamId)
        {
        }
    }

    public class SnakeStarted : DomainEventBase
    {
        public int GameNr { get; private set; }

        public SnakeStarted(string streamId, int gameNr) : base(streamId)
        {
            GameNr = gameNr;
        }
    }
}
