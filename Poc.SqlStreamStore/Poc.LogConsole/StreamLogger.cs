using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Poc.LogConsole
{

    public class StreamLogger : IObserver<Unit>, IDisposable
    {
        private readonly CancellationTokenSource _disposed = new CancellationTokenSource();
        private readonly IReadonlyStreamStore _store;
        private bool _first = true;
        private long _currentHeadPosition = 0;

        public StreamLogger(IReadonlyStreamStore store)
        {
            _store = store;
        }


        public void OnCompleted()
        {
            Console.WriteLine("Additional data will not be transmitted.");
        }

        public void OnError(Exception error)
        {
            // Do nothing.
        }

        public void OnNext(Unit value)
        {
            if (_first)
            {
                Console.WriteLine("reading from begin of streams.");
                Task.Run(SendMessages, _disposed.Token);
                _first = false;
            }
            else
            {
                Console.WriteLine("new message arrived.");
                Task.Run(SendMessages, _disposed.Token);
            }
        }

        public async Task SendMessages()
        {
            var page = await _store.ReadAllForwards(_currentHeadPosition, 10);
            _currentHeadPosition = page.NextPosition;
            var messages = new List<StreamMessage>(page.Messages);

            while (!page.IsEnd)
            {
                page = await page.ReadNext();
                messages.AddRange(page.Messages);
                _currentHeadPosition = page.NextPosition;
            }

            foreach (var streamMessage in messages)
            {
                Console.WriteLine($"{streamMessage.StreamId.PadRight(10)}{streamMessage.StreamVersion.ToString().PadLeft(4)}|{streamMessage.Position.ToString().PadLeft(4)} | {streamMessage.CreatedUtc.ToLongTimeString()} {streamMessage.Type.PadLeft(30)}: {await streamMessage.GetJsonData()}");
                //Console.WriteLine($"Message on { streamMessage.CreatedUtc} from type {streamMessage.Type} with data: {await streamMessage.GetJsonData()}");
            }

        }

        public void Dispose()
        {
            _disposed?.Dispose();
            _store?.Dispose();
        }
    }
}
