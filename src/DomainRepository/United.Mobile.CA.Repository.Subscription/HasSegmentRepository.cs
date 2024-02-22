using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Repository.Subscription.Interfaces;
using United.Mobile.Model.Internal.Notification;
using United.Utility.Exceptions;

namespace United.PushNotification.Repository.Subscription
{
    public class HasSegmentRepository : NeptuneGremlinBaseRepository, IHasSegment
    {
        private readonly ILogger<HasSegmentRepository> _logger;

        public HasSegmentRepository(ILogger<HasSegmentRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public Task DeleteAsync(HasSegment entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(HasSegment entity)
        {
            throw new NotImplementedException();
        }

        public Task<HasSegment> GetAsync(HasSegment entity)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(HasSegment entity, bool upsertRequired)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> ExistsAsync(HasSegment edge)
        //{
        //    return await base.ExistsEdgeAsync<HasSegment>(edge);
        //}

        //public async Task<HasSegment> GetAsync(HasSegment edge)
        //{
        //    try
        //    {
        //        return await base.GetEdgeAsync<HasSegment>(edge);
        //    }
        //    catch (UnitedMissingGremlinAttributeException attributeEx)
        //    {
        //        _logger.LogError(attributeEx.Message);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return null;
        //    }
        //}

        //public async Task UpsertAsync(HasSegment edge, bool upsertRequired)
        //{
        //    try
        //    {
        //        await UpsertEdgeAsync<HasSegment>(edge, upsertRequired);
        //    }
        //    catch (UnitedMissingGremlinAttributeException attributeEx)
        //    {
        //        _logger.LogError(attributeEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //    }
        //}

        //public async Task DeleteAsync(HasSegment edge)
        //{
        //    long messagingTimestampTicks = System.DateTime.UtcNow.Ticks;
        //    await DeleteEdgeAsync<HasSegment>(edge, messagingTimestampTicks);
        //}
    }
}
