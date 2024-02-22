using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;

namespace United.Mobile.DomainRepository.SubscribedItem
{
    public class SubscribedItemNeptuneRepository : NeptuneGremlinBaseRepository, ISubscribedItemRepository
    {
        private readonly ILogger<SubscribedItemNeptuneRepository> _logger;

        public SubscribedItemNeptuneRepository(ILogger<SubscribedItemNeptuneRepository> logger, IClient client)
                    : base(logger, client)
        {
            _logger = logger;

        }

        public async Task<Model.Internal.Notification.SubscribedItem> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<Model.Internal.Notification.SubscribedItem>(loggingContext, id);
        }

        public async Task<Model.Internal.Notification.SubscribedItem> UpsertAsync(string loggingContext, Model.Internal.Notification.SubscribedItem entity)
        {
            return await UpsertNodeAsync(loggingContext, entity);
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }
    }
}
