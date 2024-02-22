using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Exceptions;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.Internal.Inbox;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Relation;
using Application = United.Mobile.Model.Internal.Application.Application;

namespace United.Mobile.DomainRepository.Inbox
{
    public class InboxNeptuneRepository : NeptuneGremlinBaseRepository, IInboxRepository
    {
        private readonly ILogger<InboxNeptuneRepository> _logger;

        public InboxNeptuneRepository(ILogger<InboxNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task<bool> CreateInboxMessageRelationship(string loggingContext, string outVId, string inVId)
        {
            try 
            {
                HasMessage hasMessage = new HasMessage(outVId, inVId);
                return await UpsertEdgeAsync(loggingContext, hasMessage, true)!= null;
            }
            catch (UnitedMissingGremlinAttributeException attributeEx)
            {
                //_logger.LogError(attributeEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return false;
            }
        }


        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<InboxMessage> GetAsync(string loggingContext, string id)
        {
            string queryForDeleteMessage = $"g.V('{id}').has('timeToLiveTicks', lt('{DateTime.UtcNow.Ticks}')).drop()";
            await Client.SubmitAsync(queryForDeleteMessage);
            return await GetNodeAsync<InboxMessage>(loggingContext, id);
        }

        public async Task<List<InboxMessage>> GetMessagesAsync(string loggingContext, string mpNumber, string applicationId)
        {
            try
            {
                if (string.IsNullOrEmpty(mpNumber) && string.IsNullOrEmpty(applicationId))
                    return null;
                List<InboxMessage> inboxMessages = new List<InboxMessage>();
                Application application = new Application(applicationId);
                string queryForApplication = $"g.V('{application.Id}').has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).outE().hasLabel('{EntityAndRelationConstants.HasMessageLabel}').otherV().has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).valueMap(true)";
                string quertForDeleteExpiredApplicationMsg = $"g.V('{application.Id}').outE().hasLabel('{EntityAndRelationConstants.HasMessageLabel}').otherV().has('timeToLiveTicks', lt({DateTime.UtcNow.Ticks})).drop()";
                MileagePlus MpMember = new MileagePlus(mpNumber);
                string queryForMpMember = $"g.V('{MpMember.Id}').has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).outE().hasLabel('{EntityAndRelationConstants.HasMessageLabel}').otherV().has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).valueMap(true)";
                string quertForDeleteExpiredMpMemberMsg = $"g.V('{MpMember.Id}').outE().hasLabel('{EntityAndRelationConstants.HasMessageLabel}').otherV().has('timeToLiveTicks', lt({DateTime.UtcNow.Ticks})).drop()";

                if (!string.IsNullOrEmpty(mpNumber))
                {
                    var messagesForMpMember = await GetListOfNodesAsync<InboxMessage>(loggingContext, queryForMpMember);
                    await Client.SubmitAsync(quertForDeleteExpiredApplicationMsg);
                    if (messagesForMpMember != null && messagesForMpMember.Count > 0)
                    {
                        inboxMessages.AddRange(messagesForMpMember);
                    }    
                }
                if (!string.IsNullOrEmpty(applicationId))
                {
                    var messagesForApplication = await GetListOfNodesAsync<InboxMessage>(loggingContext, queryForApplication);
                    await Client.SubmitAsync(quertForDeleteExpiredMpMemberMsg);
                    if (messagesForApplication != null && messagesForApplication.Count > 0)
                    {
                        inboxMessages.AddRange(messagesForApplication);
                    }
                }

                return inboxMessages;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InboxMessage> UpsertAsync(string loggingContext, InboxMessage message)
        {
            return await UpsertNodeAsync(loggingContext, message);
        }
    }
}
