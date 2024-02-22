using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Repository.Subscription.Interfaces;
using United.Mobile.Model.Internal.Notification;

namespace United.PushNotification.Repository.Subscription
{
    public class SegmentRepository : NeptuneGremlinBaseRepository, ISegment
    {
        private readonly ILogger<SegmentRepository> _logger;

        public SegmentRepository(ILogger<SegmentRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task DeleteAsync( string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(id,messagingTimestampTicks);
        }
        
        public async Task<bool> ExistsAsync(string id)
        {
            return await ExistsNodeAsync(id);
        }
        public async Task<Segment> GetAsync(string id)
        {
            Segment segment = await GetNodeAsync<Segment>(id);
            return segment;
        }

        public async Task<Segment> UpsertAsync(Segment entity)
        {
            return await UpsertNodeAsync<Segment>(entity);
        }
     
    }
}
