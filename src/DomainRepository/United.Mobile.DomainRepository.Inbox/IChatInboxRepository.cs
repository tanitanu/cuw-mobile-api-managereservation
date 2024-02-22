using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.CA.Model.Internal.Inbox;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Inbox
{
    public interface IChatInboxRepository : IRepository<ChatInboxMessage>
    {
        Task UpsertAsync(string loggingContext, string deviceId, string messageId);
        Task<string> GetMessageIdAsync(string loggingContext, string deviceId);
    }
}
