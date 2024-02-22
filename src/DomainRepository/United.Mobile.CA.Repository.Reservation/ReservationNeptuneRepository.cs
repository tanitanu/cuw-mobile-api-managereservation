using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Repository.Reservation.Interface;
using United.Mobile.Model.Internal.ReservationDomain;
using United.Mobile.Model.Relation;
using United.Utility.Extension;
using System.Linq;

namespace United.Mobile.CA.Repository.Reservation
{
    public class ReservationNeptuneRepository : NeptuneGremlinBaseRepository, IReservationRepository
    {
        private readonly ILogger<ReservationNeptuneRepository> _logger;

        private readonly string hasTripLabel = typeof(HasTrip).GetLabel();
        private readonly string hasReservationSegmentLabel = typeof(HasReservationSegment).GetLabel();
        private readonly string hasPassengerLabel = typeof(HasPassenger).GetLabel();
        private readonly string hasPersistDataLabel = typeof(HasPersistData).GetLabel();

        private string BuildGremlinQueryForOutNodes(string id, string edgeLabel)
        {
            return $"g.V('{id}').has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).outE({edgeLabel}).has('timeToLiveTicks', gt({DateTime.UtcNow.Ticks})).has('isDeleted', false).valueMap(true)";
        }

        public ReservationNeptuneRepository(ILogger<ReservationNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        //public async Task<Model.ReservationClient.Reservation> UpsertAsync(Model.ReservationClient.Reservation reservation)
        public async Task<Mobile.Model.Internal.ReservationDomain.Reservation> UpsertAsync(string loggingContext, Mobile.Model.Internal.ReservationDomain.Reservation reservation)
        {
            var reservationClient = await UpsertNodeAsync(loggingContext, reservation);

            foreach (var trip in reservation.Trips)
            {
                // Add trip node and edge which links the trip and reservation
                var tripClient = await UpsertNodeAsync(loggingContext, trip);
                await UpsertEdgeAsync(loggingContext, new HasTrip(reservationClient.Id, tripClient.Id));

                foreach (var segment in trip.Segments)
                {
                    // Add segment node and the edge which links the trip and segment
                    var segmentClient = await UpsertNodeAsync(loggingContext, segment);
                    await UpsertEdgeAsync(loggingContext, new HasReservationSegment(tripClient.Id, segmentClient.Id));

                    foreach (var passenger in segment.Passengers)
                    {
                        // Add passenger node and the edge which links the passenger and segment
                        var passengerClient = await UpsertNodeAsync(loggingContext, passenger);
                        await UpsertEdgeAsync(loggingContext, new HasPassenger(segmentClient.Id, passengerClient.Id));

                        // Add passenger data and edge which links the data and passenger
                        var passengerDataClient = await UpsertNodeAsync(loggingContext, passenger.PersistData);
                        await UpsertEdgeAsync(loggingContext, new HasPersistData(passengerClient.Id, passengerDataClient.Id));
                    }
                }
            }

            return reservationClient;
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<Mobile.Model.Internal.ReservationDomain.Reservation> GetAsync(string loggingContext, string id)
        {
            Mobile.Model.Internal.ReservationDomain.Reservation reservation = await GetNodeAsync<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, id);

            // Load the linked nodes
            var trips = await GetListOfNodesAsync<TripInformation>(loggingContext, BuildGremlinQueryForOutNodes(id, hasTripLabel));
            foreach (var trip in trips)
            {
                var reservationSegs = await GetListOfNodesAsync<ReservationSegment>(loggingContext, BuildGremlinQueryForOutNodes(id, hasReservationSegmentLabel));

                foreach (var reservationSeg in reservationSegs)
                {
                    var passengers = await GetListOfNodesAsync<PassengerSegment>(loggingContext, BuildGremlinQueryForOutNodes(id, hasPassengerLabel)); ;

                    foreach (var passenger in passengers)
                    {
                        var persistData = (await GetListOfNodesAsync<PassengerSegmentData>(loggingContext, BuildGremlinQueryForOutNodes(id, hasPersistDataLabel)))?[0];
                        passenger.PersistData = persistData;
                    }

                    reservationSeg.Passengers = passengers;
                }

                trip.Segments = reservationSegs;

                reservation.Trips = trips;
            }
            return reservation;
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<List<Mobile.Model.Internal.ReservationDomain.Reservation>> GetAllReservationsAsync(string loggingContext)
        {
            return await GetListOfNodesAsync<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, "g.V().hasLabel('Reservation').valueMap(true)");
        }

        public async Task<string> BuildGremlinQueryForComplexObjectsAsync<T>(string loggingContext, string id)
        {
            var gremlinQuery = await Task.Run(() => BuildGremlinQueryForComplexObjectById<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, id)).ConfigureAwait(false);

            return gremlinQuery;
        }

        public async Task<string> BuildGremlinQueryForComplexObjectsByTraversalAsync<T>(string loggingContext, string rootVertexLabel, Dictionary<string, object> labelValuePair, string relationName)
        {
            var gremlinQuery = await Task.Run(() => BuildGremlinQueryForComplexObjectByTraversal<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, rootVertexLabel, labelValuePair, relationName)).ConfigureAwait(false);

            return gremlinQuery;
        }

        public async Task<string> BuildGremlinQueryForComplexObjectsByTraversalAsync<T>(string loggingContext, string id, string relationName)
        {
            var gremlinQuery = await Task.Run(() => BuildGremlinQueryForComplexObjectByTraversal<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, id, relationName)).ConfigureAwait(false);

            return gremlinQuery;
        }

        public async Task<List<Mobile.Model.Internal.ReservationDomain.Reservation>> GetComplexObjectByIdAsync(string loggingContext, string id)
        {
            List<Mobile.Model.Internal.ReservationDomain.Reservation> reservationComplexObject = null;
            var gremlinQuery = await Task.Run(() => BuildGremlinQueryForComplexObjectById<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, id)).ConfigureAwait(false);

            var graphResponse = await GetComplexObjectAsync<Mobile.Model.Internal.ReservationDomain.Reservation>(loggingContext, gremlinQuery);

            if(graphResponse != null && graphResponse.Any())
            {
                reservationComplexObject = graphResponse.Where(x => x.Data != null).Select(x => x.Data).ToList();
            }

            return reservationComplexObject;
        }

        public void Test()
        {
            string query = $"g.V('RESERVATION::PQR123::20210410').in()";

            GetListOfNodesAsync<Mobile.Model.Internal.ReservationDomain.Reservation>("", query).GetAwaiter().GetResult();
        }

        public async Task<ReservationSegment> GetActiveSegmentForReservationAsync(string loggingContext, string reservationId)
        {
            string gremlinQuery = $"g.V({reservationId}).outE('HAS_ACTIVE_SEGMENT').otherV().valueMap(true).fold()";
            var reservationSegment = await GetNodeAsyncByQuery<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegment;
        }

        public async Task<ReservationSegment> GetContextualReservationSegmentByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string gremlinQuery = $"g.V({deviceId}).outE('HAS_CONTEXTUAL_SEGMENT').otherV().valueMap(true).fold()";
            var reservationSegment = await GetNodeAsyncByQuery<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegment;
        }
    }
}
