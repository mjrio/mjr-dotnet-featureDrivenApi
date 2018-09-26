using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Poc.SqlStreamEventStore;
using SqlStreamStore.Streams;

namespace Poc.Tests.Infrastructure
{
    public abstract class StreamStoreAcceptanceTestsBase
    {

        protected StreamStoreFactory GetFixture() => new StreamStoreFactory("poc", true);


        public static NewStreamMessage[] CreateNewStreamMessages(params int[] messageNumbers)
        {
            return CreateNewStreamMessages("\"data\"", messageNumbers);
        }

        public static NewStreamMessage[] CreateNewStreamMessages(string jsonData, params int[] messageNumbers)
        {
            return messageNumbers
                .Select(number =>
                {
                    var id = Guid.Parse("00000000-0000-0000-0000-" + number.ToString().PadLeft(12, '0'));
                    return new NewStreamMessage(id, "type", jsonData, "\"metadata\"");
                })
                .ToArray();
        }

        public static NewStreamMessage[] CreateNewStreamMessageSequence(int startId, int count)
        {
            var messages = new List<NewStreamMessage>();
            for (int i = 0; i < count; i++)
            {
                var mwssageNumber = startId + i;
                var messageId = Guid.Parse("00000000-0000-0000-0000-" + mwssageNumber.ToString().PadLeft(12, '0'));
                var newStreamMessage = new NewStreamMessage(messageId, "type", "\"data\"", "\"metadata\"");
                messages.Add(newStreamMessage);
            }
            return messages.ToArray();
        }

        public static StreamMessage ExpectedStreamMessage(
            string streamId,
            int messageNumber,
            int sequenceNumber,
            DateTime created)
        {
            var id = Guid.Parse("00000000-0000-0000-0000-" + messageNumber.ToString().PadLeft(12, '0'));
            return new StreamMessage(streamId, id, sequenceNumber, 0, created, "type", "\"metadata\"", "\"data\"");
        }
    }

    public static class TaskExtensions
    {
        private static Func<int, int> TaskTimeout => timeout => Debugger.IsAttached ? 30000 : timeout;

        public static async Task<T> WithTimeout<T>(this Task<T> task, int timeout = 3000)
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return await task;
            }
            throw new TimeoutException("Timed out waiting for task");
        }

        public static async Task WithTimeout(this Task task, int timeout = 3000)
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return;
            }
            throw new TimeoutException("Timed out waiting for task");
        }
    }
}
