using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.CA.Model.Internal.Inbox;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Inbox
{
    public interface IInboxRepository : IRepository<InboxMessage>
    {
        Task<List<InboxMessage>> GetMessagesAsync(string loggingContext, string mpNumber, string applicationId);
        Task<bool> CreateInboxMessageRelationship(string loggingContext, string outVId, string inVId);
    }
}
