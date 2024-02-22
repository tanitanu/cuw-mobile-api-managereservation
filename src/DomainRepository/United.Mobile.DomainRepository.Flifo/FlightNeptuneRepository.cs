using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using United.Exceptions;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Relation;

namespace United.Mobile.DomainRepository.Flifo
{
    public class FlightNeptuneRepository : NeptuneGremlinBaseRepository, IFlightRepository
    {
        private readonly ILogger<FlightNeptuneRepository> _logger;

        public FlightNeptuneRepository(ILogger<FlightNeptuneRepository> logger, IClient client) 
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task<Flight> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<CA.Model.Flifo.Flight>(loggingContext, id);
        }

        public async Task<Flight> UpsertAsync(string loggingContext, CA.Model.Flifo.Flight entity)
        {
            return await UpsertNodeAsync<CA.Model.Flifo.Flight>(loggingContext, entity);
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<bool> CreateHasScheduledSegmentRelationship(string loggingContext, string outVId, string inVId)
        {
            try
            {
                HasScheduledSegment hasScheduledSegment = new HasScheduledSegment(outVId, inVId);
                return await UpsertEdgeAsync(loggingContext, hasScheduledSegment, true) != null;
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
    }
}
        