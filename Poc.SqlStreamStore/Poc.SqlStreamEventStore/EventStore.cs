using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Poc.EventStore.Abstractions;
using Poc.EventStore.Events;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;
using SqlStreamStore.Streams;

namespace Poc.SqlStreamEventStore
{
    public class EventStore : IEventStore
    {

        public const string CommitIdHeader = "CommitId";
        public const string AggregateClrTypeHeader = "AggregateClrType";
        public const string EventClrTypeHeader = "EventClrType";
        public const string CreatedByHeader = "CreatedBy";

        private readonly IStreamStore _store;
        private readonly string _connectionString;

        private const int ReadPageSize = 1000;
        private const int WritePageSize = 1000;
        public EventStore(IStreamStore streamStore, string connectionString)
        {
            _store = streamStore;
            _connectionString = connectionString;
        }

        public async Task<int> ReadLastStreamVersion(string streamId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var end = await _store.ReadStreamBackwards(streamId, -1, 1, cancellationToken);
            return end.LastStreamVersion;
        }
        public async Task DeleteStream(string streamId)
        {
            await _store.DeleteStream(streamId);
        }
        public async Task ReadStreamForwards(string streamId, int fromVersionInclusive, int readPageSize, bool prefetchData, Func<IDomainEvent, int, Task> applyEvent)
        {
            var messages = await ReadStreamMessages(streamId, fromVersionInclusive);

            foreach (var resolvedEvent in messages)
            {
                var currentEvent = Deserialize(resolvedEvent.Type, await resolvedEvent.GetJsonData());
                await applyEvent(currentEvent, resolvedEvent.StreamVersion);
            }
        }

        private async Task<List<StreamMessage>> ReadStreamMessages(string streamId, int fromVersionInclusive)
        {
            if (fromVersionInclusive < 0)
                throw new InvalidOperationException("Cannot get version < 0");

            var page = await _store.ReadStreamForwards(streamId, fromVersionInclusive, ReadPageSize);
            var messages = new List<StreamMessage>(page.Messages);
            while (!page.IsEnd)
            {
                page = await page.ReadNext();
                messages.AddRange(page.Messages);
            }

            return messages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="streamId"></param>
        /// <param name="fromVersionInclusive"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync<T>(string streamId, int fromVersionInclusive)
            where T : IEventSourcingAggregate
        {
            var aggregate = CreateAggregate<T>(streamId);
            var messages = await ReadStreamMessages(streamId, fromVersionInclusive);
            foreach (var resolvedEvent in messages)
            {
                var currentEvent = Deserialize(resolvedEvent.Type, await resolvedEvent.GetJsonData());
                aggregate.ApplyEvent(currentEvent, resolvedEvent.StreamVersion);
            }

            return aggregate;
        }
        private T CreateAggregate<T>(string streamId)
            where T : IEventSourcingAggregate
        {
            var parameters = new object[1];
            parameters[0] = streamId;
            Type d1 = typeof(T);
            Type[] typeArgs = { typeof(string) };

            var res = (T)d1
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, typeArgs, null)
                ?.Invoke(parameters);

            return res;
        }


        public async Task<IEnumerable<string>> ListStreams(string startsWith, int maxCount = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            var streamIds = new List<string>();

            //in next version of SqlStream store this features will be available, for now just use plain old ado.net
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("select TOP 100 idOriginal from es.streams where idOriginal LIKE 'Snake%'", connection);
                //cmd.Parameters.AddWithValue("@maxCount", maxCount);
                //cmd.Parameters.AddWithValue("@startswith", string.Concat("%", startsWith));

                await connection.OpenAsync(cancellationToken).NotOnCapturedContext();
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        streamIds.Add(reader[0].ToString());
                    }
                }
            }
            return streamIds;
        }

        public async Task Save(IEventSourcingAggregate eventSourcingAggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            var commitHeaders = new Dictionary<string, object>
            {
                {CreatedByHeader, Environment.UserName},
                {CommitIdHeader, commitId},
                {AggregateClrTypeHeader, eventSourcingAggregate.GetType().AssemblyQualifiedName}
            };
            updateHeaders(commitHeaders);
            var newEvents = eventSourcingAggregate.GetUncommittedEvents().ToList();
            var originalVersion = eventSourcingAggregate.StreamVersion - newEvents.Count;
            var expectedVersion = originalVersion == -1 ? ExpectedVersion.NoStream : ExpectedVersion.Any;

            var eventsToSave = newEvents.Select(e => ToNewStreamMessage(Guid.NewGuid(), e, commitHeaders)).ToArray();

            try
            {
                if (eventsToSave.Length < WritePageSize)
                {
                    await _store.AppendToStream(eventSourcingAggregate.StreamId, expectedVersion, eventsToSave);
                }
                else
                {
                    //TODO
                }
            }
            catch (WrongExpectedVersionException weve)
            {
                var sqlE = weve.InnerException as SqlException;
                if (sqlE?.Number == 2601)
                {
                    throw new DuplicateEventStoreAggregateException(eventSourcingAggregate.StreamId, weve);
                }
            }
            eventSourcingAggregate.ClearUncommittedEvents();
        }
        private static NewStreamMessage ToNewStreamMessage(Guid eventId, IDomainEvent evnt, IDictionary<string, object> headers)
        {
            var data = JsonConvert.SerializeObject(evnt);

            var eventHeaders = new Dictionary<string, object>(headers){
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = JsonConvert.SerializeObject(eventHeaders);
            var typeName = evnt.EventTypeName;

            return new NewStreamMessage(eventId, typeName, data, metadata);
        }
        private IDomainEvent Deserialize(string evenTypeName, string data)
        {
            //should be optimized for performance.
            var domainAssembly = typeof(IDomainEvent).Assembly;
            var type = domainAssembly.GetTypes()
                .SingleOrDefault(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && x.Name == evenTypeName);
            if (type == null)
            {
                type = Assembly.Load("Poc.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").GetTypes()
                    .SingleOrDefault(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && x.Name == evenTypeName);
            }
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };
            var dEvent = (IDomainEvent)JsonConvert.DeserializeObject(data, type, settings);
            return dEvent;
        }
    }
}
