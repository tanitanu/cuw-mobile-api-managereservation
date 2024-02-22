using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;

namespace United.Mobile.DomainRepository.Segment
{
    public class SegmentNeptuneRepository : NeptuneGremlinBaseRepository, ISegmentRepository
    {
        private readonly ILogger<SegmentNeptuneRepository> _logger;

        public SegmentNeptuneRepository(ILogger<SegmentNeptuneRepository> logger, IClient client)
                    : base(logger, client)
        {
            _logger = logger;

        }

        public async Task<Model.Internal.Segment.Segment> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<Model.Internal.Segment.Segment>(loggingContext, id);
        }

        public async Task<Model.Internal.Segment.Segment> UpsertAsync(string loggingContext, Model.Internal.Segment.Segment entity)
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
