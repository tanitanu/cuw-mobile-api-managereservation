using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Flifo;

namespace United.Mobile.DomainRepository.Flifo
{
    public class OperationalSegmentNeptuneRepository : NeptuneGremlinBaseRepository, IOperationalSegmentRepository
    {
        private readonly ILogger<OperationalSegmentNeptuneRepository> _logger;

        public OperationalSegmentNeptuneRepository(ILogger<OperationalSegmentNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<OperationalFlightSegment> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<OperationalFlightSegment>(loggingContext, id);
        }

        public async Task<OperationalFlightSegment> GetOperationalFlightSegmentByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string gremlinQuery = $"g.V({deviceId}).outE('HAS_CONTEXTUAL_SEGMENT').otherV().outE('HAS_FLIGHT_STATUS').OtherV().outE('HAS_OPERATING_SEGMENTS').OtherV().valueMap(true).fold()";
            var operationalFlightSegment = await GetNodeAsyncByQuery<OperationalFlightSegment>(loggingContext, gremlinQuery);

            return operationalFlightSegment;
        }

        public async Task<OperationalFlightSegment> UpsertAsync(string loggingContext, OperationalFlightSegment entity)
        {
            return await UpsertNodeAsync<OperationalFlightSegment>(loggingContext, entity);
        }
    }
}
