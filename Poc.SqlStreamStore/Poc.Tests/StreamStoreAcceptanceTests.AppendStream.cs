using SqlStreamStore.Infrastructure;

namespace SqlStreamStore
{
    using System;
    using System.Threading.Tasks;
    using Shouldly;
    using SqlStreamStore.Streams;
    using Xunit;

    public partial class StreamStoreAcceptanceTests
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

            using (var fixture = GetFixture())
            {
                using (var store = await fixture.GetStreamStore())
                {

                    // ReSharper disable once SuggestVarOrType_SimpleTypes
                    Guid myAppGuid = new Guid("8D1E0B02-0D78-408E-8211-F899BE6F8AA2");
                    var generator = new DeterministicGuidGenerator(myAppGuid);

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

                    const string streamId = "stream-1";

                    var m1 = new NewStreamMessage(new Guid("00000000-0000-0000-0000-000000000001"), "t", "data");
                    var m2 = new NewStreamMessage(new Guid("00000000-0000-0000-0000-000000000002"), "t", "data");
                    var m3 = new NewStreamMessage(new Guid("00000000-0000-0000-0000-000000000003"), "t", "data");

                    // Creates stream
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m1, m2, m3 });

                    // Idempotent appends
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m1, m2, m3 });
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m1, m2 });
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m2, m3 });
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m3 });

                    // Throws WrongExpectedVersionException
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m2, m1, m3 }); // out of order

                    var m4 = new NewStreamMessage(new Guid("00000000-0000-0000-0000-000000000004"), "t", "data");
                    await store.AppendToStream(streamId, ExpectedVersion.Any, new[] { m3, m4 }); // partial previous write
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
