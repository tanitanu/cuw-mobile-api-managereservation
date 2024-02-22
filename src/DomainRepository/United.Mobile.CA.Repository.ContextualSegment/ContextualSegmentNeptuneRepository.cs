using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;

namespace United.Mobile.CA.Repository.ContextualSegment
{
    public class ContextualSegmentNeptuneRepository : NeptuneGremlinBaseRepository, IContextualSegmentRepository
    {
        private readonly ILogger<ContextualSegmentNeptuneRepository> _logger;

        public ContextualSegmentNeptuneRepository(ILogger<ContextualSegmentNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistsAsync(string loggingContext, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Model.ContextualSegmentDomain.ContextualSegment> GetAsync(string loggingContext, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Model.ContextualSegmentDomain.ContextualSegment> UpsertAsync(string loggingContext, Model.ContextualSegmentDomain.ContextualSegment entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
