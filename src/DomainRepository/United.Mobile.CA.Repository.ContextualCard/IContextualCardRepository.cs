using System.Threading.Tasks;
using United.Mobile.CA.Model.ContextualDomain;
using United.Mobile.Model.Common;

namespace United.Mobile.CA.Repository.Contextual
{
    public interface IContextualCardRepository : IRepository<ContextualCard>
    {
        Task<ContextualCard> CreateContextualCardAsync(string loggingContext, Model.Contextual.ContextualCard contextualCard);

        Task CreateContextualCardRelationshipAsync(string loggingContext, string deviceId, string contextualCardId);

        Task CreateActiveReservationRelationshipAsync(string loggingContext, string ContextualCardId, string activeReservationId);

        Task CreateContextualSegmentRelationshipAsync(string loggingContext, string deviceId, string reservationSegmentId);        
    }
}
