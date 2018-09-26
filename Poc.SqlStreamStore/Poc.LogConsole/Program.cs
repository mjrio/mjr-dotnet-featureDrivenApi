using System;
using System.Threading.Tasks;
using Poc.SqlStreamEventStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Subscriptions;

namespace Poc.LogConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Configuring store.");

            StartPolling().Wait();
        }

        private static async Task StartPolling()
        {
            var factory = new StreamStoreFactory();
            var store = await factory.GetStreamStore();
            using (var notifier = new PollingStreamStoreNotifier(store, 500))
            {
                var tcs = new TaskCompletionSource<Unit>();
                notifier.Subscribe(new StreamLogger(store));

                await tcs.Task;
            }
        }
    }
}
