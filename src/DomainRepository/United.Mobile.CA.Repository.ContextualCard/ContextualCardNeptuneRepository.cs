using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.ContextualDomain;

namespace United.Mobile.CA.Repository.Contextual
{
    public class ContextualCardNeptuneRepository : NeptuneGremlinBaseRepository, IContextualCardRepository
    {
        private readonly ILogger<ContextualCardNeptuneRepository> _logger;

        public ContextualCardNeptuneRepository(ILogger<ContextualCardNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<ContextualCard> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<ContextualCard>(loggingContext, id);
        }

        public async Task<ContextualCard> CreateContextualCardAsync(string loggingContext, Model.Contextual.ContextualCard contextualCard)
        {
            string methodName = "CreateContextualCardAsync";
            ContextualCard saveContextualCard = new ContextualCard();

            _logger.LogInformation("ContextualCardNeptuneRepository - MethodName: {@MethodName}", methodName);

            saveContextualCard.ActionItems = (ICollection<ActionItem>)contextualCard.ActionItems;
            saveContextualCard.CardType = contextualCard.CardType;
            saveContextualCard.DisclosureItems = (ICollection<ActionItem>)contextualCard.DisclosureItems;
            saveContextualCard.FlightNumberAndStatus = contextualCard.FlightNumberAndStatus;
            //saveContextualCard.FlightRoute = contextualCard.FlightRoute;
            saveContextualCard.FlightStatusSegmentPredictableKey = contextualCard.FlightStatusSegmentPredictableKey;
            saveContextualCard.GateText = contextualCard.GateText;
            saveContextualCard.GateValue = contextualCard.GateValue;
            //saveContextualCard.InfoDoc = contextualCard.InfoDoc;
            saveContextualCard.IsDivertedSegment = contextualCard.IsDivertedSegment;
            saveContextualCard.IsIrropPnr = contextualCard.IsIrropPnr;
            saveContextualCard.RecordLocator = contextualCard.RecordLocator;
            //saveContextualCard.SegmentToBeHighlighted = contextualCard.SegmentToBeHighlighted;
            saveContextualCard.TimeText = contextualCard.TimeText;
            saveContextualCard.TimeValue = contextualCard.TimeValue;
            saveContextualCard.TravelDate = contextualCard.TravelDate;
            saveContextualCard.TravelStatusText = contextualCard.TravelStatusText;
            saveContextualCard.TripTipRows = (ICollection<UITemplateBlockContent>)contextualCard.TripTipRows;
            saveContextualCard.UserId = contextualCard.UserId;

            ContextualCard savedContextualCard = await UpsertAsync(loggingContext, saveContextualCard);

            return savedContextualCard;
        }

        public async Task CreateContextualCardRelationshipAsync(string loggingContext, string deviceId, string contextualCardId)
        {
            string methodName = "CreateContextualCardRelationshipAsync";

            _logger.LogInformation("ContextualCardNeptuneRepository - MethodName: {@MethodName}", methodName);

            // Create HAS_CONTEXTUAL_CARD between Application Node and ContextualCard Node
            HasContextualCard hasContextualCard = new HasContextualCard($"APPLICATION::{deviceId}", contextualCardId);

            await UpsertEdgeAsync<HasContextualCard>(loggingContext, hasContextualCard);
        }

        public async Task CreateActiveReservationRelationshipAsync(string loggingContext, string ContextualCardId, string activeReservationId)
        {
            string methodName = "CreateActiveReservationRelationshipAsync";

            _logger.LogInformation("ContextualCardNeptuneRepository - MethodName: {@MethodName}", methodName);

            // Create HAS_ACTIVE_RESERVATION between ContextualCard Node and Reservation Node
            HasActiveReservation hasActiveReservation = new HasActiveReservation(ContextualCardId, activeReservationId);

            await UpsertEdgeAsync<HasActiveReservation>(loggingContext, hasActiveReservation);
        }

        public async Task CreateContextualSegmentRelationshipAsync(string loggingContext, string deviceId, string reservationSegmentId)
        {
            string methodName = "CreateContextualSegmentRelationshipAsync";

            _logger.LogInformation("ContextualCardNeptuneRepository - MethodName: {@MethodName}", methodName);

            HasContextualSegment hasContextualSegment = new HasContextualSegment($"APPLICATION::{deviceId}", reservationSegmentId);

            await UpsertEdgeAsync<HasContextualSegment>(loggingContext, hasContextualSegment);
        }

        public async Task<ContextualCard> UpsertAsync(string loggingContext, ContextualCard entity)
        {
            return await UpsertNodeAsync(loggingContext, entity);
        }
    }
}
