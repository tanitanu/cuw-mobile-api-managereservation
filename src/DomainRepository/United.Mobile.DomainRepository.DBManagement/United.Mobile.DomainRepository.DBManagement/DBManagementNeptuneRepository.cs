using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;

namespace United.Mobile.DomainRepository.DBManagement
{
    public class DBManagementNeptuneRepository : NeptuneGremlinBaseRepository, IDBManagementRepository
    {
        private readonly ILogger<DBManagementNeptuneRepository> _logger;

        public DBManagementNeptuneRepository(ILogger<DBManagementNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }
        public async Task CleanupNodes(string loggingContext, int deleteDurationInDays)
        {
            string methodName = "CleanupNodes";
            int nodesCountByTimeToLiveTicks;
            int nodesCountByMessageTimestampTicks;
            var utcNowTicks = DateTime.UtcNow.Ticks;
            var deleteMessagingTimestampTicks = DateTime.UtcNow.AddDays(-deleteDurationInDays).Ticks;

            _logger.LogInformation("DBManagementNeptuneRepository - Cleanup Nodes - MethodName: {@MethodName}", methodName);

            nodesCountByTimeToLiveTicks = await GetDeleteNodesCountByTimeToLiveTicks(loggingContext, utcNowTicks);

            if (nodesCountByTimeToLiveTicks > 0)
            {
                await DeleteNodesByTimeToLiveTicks(loggingContext, utcNowTicks);
            }

            nodesCountByMessageTimestampTicks = await GetDeleteNodesCountByMessageTimestampTicks(loggingContext, deleteMessagingTimestampTicks);

            if (nodesCountByMessageTimestampTicks > 0)
            {
                await DeleteNodesByMessageTimestampTicks(loggingContext, deleteMessagingTimestampTicks);
            }
        }

        public async Task<int> GetDeleteNodesCountByTimeToLiveTicks(string loggingContext, long utcNowTicks)
        {
            int nodesCount;
            string methodName = "GetDeleteNodesCountByTimeToLiveTicks";
            var queryDeleteNodesCountByTimeToLiveTicks = new StringBuilder();

            _logger.LogInformation("DBManagementNeptuneRepository - Cleanup Nodes - MethodName: {@MethodName}", methodName);

            // Get the Nodes count to be deleted based on TimeToLiveTicks
            queryDeleteNodesCountByTimeToLiveTicks.Append("g.V()");
            queryDeleteNodesCountByTimeToLiveTicks.Append($".has('timeToLiveTicks', lt({utcNowTicks})).valueMap(true)");
            queryDeleteNodesCountByTimeToLiveTicks.Append(".count()");

            nodesCount = await GetCountOfNodes(loggingContext, queryDeleteNodesCountByTimeToLiveTicks.ToString());

            return nodesCount;
        }

        public async Task<int> GetDeleteNodesCountByMessageTimestampTicks(string loggingContext, long deleteMessagingTimestampTicks)
        {
            int nodesCount;
            string methodName = "GetDeleteNodesCountByMessageTimestampTicks";
            var queryDeleteNodesCountByMessageTimestampTicks = new StringBuilder();

            _logger.LogInformation("DBManagementNeptuneRepository - Cleanup Nodes - MethodName: {@MethodName}", methodName);

            // Get the Nodes count to be deleted based on isDeleted='true'and MessageTimestampTicks
            queryDeleteNodesCountByMessageTimestampTicks.Append("g.V()");
            queryDeleteNodesCountByMessageTimestampTicks.Append($".has('isDeleted', true).has('messagingTimestampTicks', gt({deleteMessagingTimestampTicks})).valueMap(true)");
            queryDeleteNodesCountByMessageTimestampTicks.Append(".count()");

            nodesCount = await GetCountOfNodes(loggingContext, queryDeleteNodesCountByMessageTimestampTicks.ToString());

            return nodesCount;
        }

        public async Task DeleteNodesByTimeToLiveTicks(string loggingContext, long utcNowTicks)
        {
            string methodName = "DeleteNodesByTimeToLiveTicks";
            var queryDeleteNodesByTimeToLiveTicks = new StringBuilder();

            _logger.LogInformation("DBManagementNeptuneRepository - Cleanup Nodes - MethodName: {@MethodName}", methodName);

            // Get the Nodes to be deleted based on TimeToLiveTicks
            queryDeleteNodesByTimeToLiveTicks.Append("g.V()");
            queryDeleteNodesByTimeToLiveTicks.Append($".has('timeToLiveTicks', lt({utcNowTicks}))");
            queryDeleteNodesByTimeToLiveTicks.Append(".drop()");

            await DeleteNodesAsync(loggingContext, queryDeleteNodesByTimeToLiveTicks.ToString());
        }

        public async Task DeleteNodesByMessageTimestampTicks(string loggingContext, long deleteMessagingTimestampTicks)
        {
            string methodName = "DeleteNodesByMessageTimestampTicks";
            var queryDeleteNodesByMessageTimestampTicks = new StringBuilder();

            _logger.LogInformation("DBManagementNeptuneRepository - Cleanup Nodes - MethodName: {@MethodName}", methodName);

            // Get the Nodes to be deleted based on isDeleted='true'and MessageTimestampTicks
            queryDeleteNodesByMessageTimestampTicks.Append("g.V()");
            queryDeleteNodesByMessageTimestampTicks.Append($".has('isDeleted', true).has('messagingTimestampTicks', gt({deleteMessagingTimestampTicks}))");
            queryDeleteNodesByMessageTimestampTicks.Append(".drop()");

            await DeleteNodesAsync(loggingContext, queryDeleteNodesByMessageTimestampTicks.ToString());
        }
    }
}
