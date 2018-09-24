using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Poc.SqlStreamEventStore;
using TinyIoC;

namespace Poc.Console1
{
    public static class Bootstrapper
    {
        public static async Task Start()
        {
            var container = TinyIoCContainer.Current;
            MsSqlStreamStore ms = new MsSqlStreamStore("es", "Poc");
            
            SqlStreamEventStore.EventStore es = new SqlStreamEventStore.EventStore(await ms.GetStreamStore(),ms.ConnectionString);

            container.Register(es);
        }
    }
}
