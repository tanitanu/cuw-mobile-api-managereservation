using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;

namespace United.Mobile.CA.Repository.TripTips
{
    public class TripTipsNeptuneRepository : NeptuneGremlinBaseRepository, ITripTipsRepository
    {
        private readonly ILogger<TripTipsNeptuneRepository> _logger;

        public TripTipsNeptuneRepository(ILogger<TripTipsNeptuneRepository> logger, IClient client)
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

        public Task<Model.TripTipsDomain.TripTips> GetAsync(string loggingContext, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Model.TripTipsDomain.TripTips> UpsertAsync(string loggingContext, Model.TripTipsDomain.TripTips entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
