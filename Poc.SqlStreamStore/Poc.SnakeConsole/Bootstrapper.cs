using System.Threading.Tasks;
using Poc.SqlStreamEventStore;

namespace Poc.SnakeConsole
{
    public static class Bootstrapper
    {
        public static async Task<SqlStreamEventStore.EventStore> Start()
        {
            StreamStoreFactory ms = new StreamStoreFactory();
            
            return new SqlStreamEventStore.EventStore(await ms.GetStreamStore(),ms.ConnectionString);

        }
    }
}
