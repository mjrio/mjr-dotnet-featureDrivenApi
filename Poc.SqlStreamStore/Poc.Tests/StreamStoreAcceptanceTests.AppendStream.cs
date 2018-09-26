using System;
using System.Threading.Tasks;
using Poc.Tests.Infrastructure;
using Shouldly;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;
using Xunit;

namespace Poc.Tests
{
    public class StreamStoreAcceptanceTests2: StreamStoreAcceptanceTestsBase
    {

        [Fact, Trait("Category", "AppendStream")]
        public async Task Can_append_message_to_a_new_stream()
        {
            using (var fixture = GetFixture())
            {
                using (var store = await fixture.GetStreamStore())
                {
                    const string streamId = "stream-1";
                    var message = new NewStreamMessage(Guid.NewGuid(), "type", "\"data\"", "\"metadata\"");
                    var result = await store
                        .AppendToStream(streamId, ExpectedVersion.NoStream, message);

                    result.CurrentVersion.ShouldBe(0);
                    result.CurrentPosition.ShouldBe(0);
                }
            }
        }

        [Fact, Trait("Category", "AppendStream")]
        public async Task Can_DeterministicGuidGenerator()
        {
            Guid myAppGuid = new Guid("8D1E0B02-0D78-408E-8211-F899BE6F8AA2");
            var generator = new DeterministicGuidGenerator(myAppGuid);
            using (var fixture = GetFixture())
            {
                using (var store = await fixture.GetStreamStore())
                {

                    const string streamId = "stream-1";
                    var messageId = generator.Create(streamId, ExpectedVersion.Any, "data");
                    var messageId2 = generator.Create(streamId, ExpectedVersion.Any, "data"); 
                    var messageId3 = generator.Create(streamId, ExpectedVersion.Any, "other data");

                    messageId2.ShouldBe(messageId, "message ids should be the same for same messages");
                    messageId3.ShouldNotBe(messageId);

                    var m1 = new NewStreamMessage(messageId, "t", "data");
                    var m2 = new NewStreamMessage(messageId2, "t", "data");
                    var m3 = new NewStreamMessage(messageId3, "t", "other data");

                    // Creates stream with idempotent attempt
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m1,  m3 });

                }
            }
        }


        [Fact, Trait("Category", "AppendStream")]
        public async Task Can_append_multiple_messages_to_stream_with_correct_expected_version()
        {
            using (var fixture = GetFixture())
            {
                using (var store = await fixture.GetStreamStore())
                {
                    const string streamId = "stream-2";
                    var result1 = await store
                        .AppendToStream(streamId, ExpectedVersion.NoStream, CreateNewStreamMessages(1, 2, 3));
                    //appending to same stream but we expect the stream to be at a specific version.
                    var result2 =
                        await store.AppendToStream(streamId, result1.CurrentVersion, CreateNewStreamMessages(4, 5, 6));

                    result2.CurrentVersion.ShouldBe(5);
                    result2.CurrentPosition.ShouldBeGreaterThan(result1.CurrentPosition);
                }
            }
        }

        [Fact, Trait("Category", "AppendStream")]
        public async Task ReadSamples()
        {

            using (var fixture = GetFixture())
            {
                using (var store = await fixture.GetStreamStore())
                {

                    const string streamId = "menu";

                    var m1 = new NewStreamMessage(Guid.NewGuid(), "DrinkAdded", "Soda");
                    var m2 = new NewStreamMessage(Guid.NewGuid(), "FoodAdded", "Sandwich");
                    var m3 = new NewStreamMessage(Guid.NewGuid(), "FoodUpdated", "Sandwich with cheese.");

                    // Creates stream
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m1, m2, m3 });
                    const string dining = "dining";
                    m1 = new NewStreamMessage(Guid.NewGuid(), "TableOpened", "Table 1 opened for 2 by waiter Piet.");
                    m2 = new NewStreamMessage(Guid.NewGuid(), "SodaAdded", "Size:Large for Table 1.");
                    await store.AppendToStream(dining, ExpectedVersion.Any, new[] { m1, m2 });
                }
            }
        }
    }

}
