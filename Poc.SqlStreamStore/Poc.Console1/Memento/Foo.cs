using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Console1.Memento
{
    public class Foo
    {
        public Foo(string id)
        {
            Id = id;
        }
        public Foo(string id, FooMemento state, int originalVersion)
        {
            Id = id;
            Balance = state.Balance;
            OriginalVersion = originalVersion;
        }
        internal FooMemento GetState()
        {
            return new FooMemento
            {
                Balance = Balance
            };
        }

        public string Id { get; private set; }

        public int Balance { get; private set; }

        public int OriginalVersion { get; private set; } = -1;

        public void Add(int i)
        {
            Balance = Balance + i;
        }

       
    }

}
