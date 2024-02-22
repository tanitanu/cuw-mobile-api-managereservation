using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using United.Exceptions;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Relation;

namespace United.Mobile.DomainRepository.Flifo
{
    public class ScheduledSegmentNeptuneRepository : NeptuneGremlinBaseRepository, IScheduledSegmentRepository
    {
        private readonly ILogger<ScheduledSegmentNeptuneRepository> _logger;

        public ScheduledSegmentNeptuneRepository(ILogger<ScheduledSegmentNeptuneRepository> logger, IClient client)
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

        public async Task<ScheduledFlightSegment> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<ScheduledFlightSegment>(loggingContext, id);
        }

        public async Task<ScheduledFlightSegment> UpsertAsync(string loggingContext, ScheduledFlightSegment entity)
        {
            return await UpsertNodeAsync<ScheduledFlightSegment>(loggingContext, entity);
        }

        public async Task<bool> CreateHasOperationalSegmentRelationship(string loggingContext, string outVId, string inVId)
        {
            try
            {
                HasOperationalSegment hasOperationalSegment = new HasOperationalSegment(outVId, inVId);
                return await UpsertEdgeAsync(loggingContext, hasOperationalSegment, true) != null;
            }
            catch (UnitedMissingGremlinAttributeException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<ScheduledFlightSegment> GetScheduledFlightSegmentByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string gremlinQuery = $"g.V({deviceId}).outE('HAS_CONTEXTUAL_SEGMENT').otherV().outE('HAS_FLIGHT_STATUS').OtherV().valueMap(true).fold()";
            var scheduledFlightSegment = await GetNodeAsyncByQuery<ScheduledFlightSegment>(loggingContext, gremlinQuery);

            return scheduledFlightSegment;
        }
    }
}
