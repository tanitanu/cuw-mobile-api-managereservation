using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Repository.Subscription.Interfaces;
using Application = United.Mobile.Model.Internal.Application.Application;

namespace United.PushNotification.Repository.Subscription
{
    public class ApplicationRepository : NeptuneGremlinBaseRepository, IApplication
    {
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(ILogger<ApplicationRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task<Application> GetAsync( string id)
        {
            Application application = await GetNodeAsync<Application>(id);
            return application;
        }
        public async Task<bool> ExistsAsync( string id)
        {
            return await ExistsNodeAsync(id);
        }

        public async Task DeleteAsync(string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(id, messagingTimestampTicks);
        }

        public async Task<Application> UpsertAsync(Application entity)
        {
            return await UpsertNodeAsync<Application>(entity);
        }
    }
}
