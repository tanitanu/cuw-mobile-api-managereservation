using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Internal.Inbox;

namespace United.Mobile.DomainRepository.Inbox
{
    public class ChatInboxNeptuneRepository : NeptuneGremlinBaseRepository, IChatInboxRepository
    {
        private readonly ILogger<ChatInboxNeptuneRepository> _logger;

        public ChatInboxNeptuneRepository(ILogger<ChatInboxNeptuneRepository> logger, IClient client)
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

        public async Task<ChatInboxMessage> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<ChatInboxMessage>(loggingContext, id);
        }

        public async Task<string> GetMessageIdAsync(string loggingContext, string deviceId)
        {
            var predictableKey = ChatInboxMessage.PredictableKey(deviceId);
            var entity = await GetAsync(loggingContext, predictableKey);
            if (entity == null)
                return null;
            return entity.MessageId;
        }

        public async Task UpsertAsync(string loggingContext, string deviceId, string messageId)
        {
            var entity = new ChatInboxMessage(deviceId) { MessageId = messageId };
            await UpsertAsync(loggingContext, entity);
        }

        public Task<ChatInboxMessage> UpsertAsync(string loggingContext, ChatInboxMessage entity)
        {
            return UpsertNodeAsync(loggingContext, entity);
        }
    }
}
