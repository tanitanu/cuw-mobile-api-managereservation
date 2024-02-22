using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using United.Exceptions;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Relation;

namespace United.Mobile.DomainRepository.Flifo
{
    public class EquipmentNeptuneRepository : NeptuneGremlinBaseRepository, IEquipmentRepository
    {
        private readonly ILogger<EquipmentNeptuneRepository> _logger;

        public EquipmentNeptuneRepository(ILogger<EquipmentNeptuneRepository> logger, IClient client)
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

        public async Task<Equipment> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<CA.Model.Flifo.Equipment>(loggingContext, id);
        }

        public async Task<Equipment> UpsertAsync(string loggingContext, Equipment entity)
        {
            return await UpsertNodeAsync<CA.Model.Flifo.Equipment>(loggingContext, entity);
        }

        public async Task<bool> CreateHasOperationalEquipmentRelationship(string loggingContext, string outVId, string inVId)
        {
            try
            {
                HasOperationalEquipment hasOperationalEquipment = new HasOperationalEquipment(outVId, inVId);
                return await UpsertEdgeAsync(loggingContext, hasOperationalEquipment, true) != null;
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
