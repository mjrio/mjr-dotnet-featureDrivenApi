namespace SqlStreamStore
{
    using System;
    using System.Threading.Tasks;
    using Shouldly;
    using SqlStreamStore.Streams;
    using Xunit;
    using Xunit.Abstractions;

    public class MsSqlStreamStoreAcceptanceTests : StreamStoreAcceptanceTests
    {
        public MsSqlStreamStoreAcceptanceTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        { }

        protected override StreamStoreAcceptanceTestFixture GetFixture()
            => new MsSqlStreamStoreFixture("foo");

    }
}