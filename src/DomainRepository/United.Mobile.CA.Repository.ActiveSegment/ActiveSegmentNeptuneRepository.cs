using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.CA.Model.ActiveSegmentDomain;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Internal.Application;
using United.Mobile.Model.Internal.ReservationDomain;

namespace United.Mobile.CA.Repository.ActiveSegment
{
    public class ActiveSegmentNeptuneRepository : NeptuneGremlinBaseRepository, IActiveSegmentRepository
    {
        private readonly ILogger<ActiveSegmentNeptuneRepository> _logger;

        public ActiveSegmentNeptuneRepository(ILogger<ActiveSegmentNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task<List<ReservationSegment>> GetReservationSegmentsByFlifoKey(string loggingContext, string flifoKey)
        {
            string methodName = "GetReservationSegmentsByFlifoKey";
            List<ReservationSegment> reservationSegments = new List<ReservationSegment>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V().has('ReservationSegment','FlightSegment','{0}').valueMap(true)", flifoKey);

            reservationSegments = await GetListOfNodesAsync<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegments;
        }

        public async Task<List<PassengerSegment>> GetPassengerSegmentsByReservationSegmentId(string loggingContext, string reservationSegmentId)
        {
            string methodName = "GetPassengerSegmentsByReservationSegmentId";
            List<PassengerSegment> passengerSegments = new List<PassengerSegment>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE().otherV().valueMap(true)", reservationSegmentId);

            passengerSegments = await GetListOfNodesAsync<PassengerSegment>(loggingContext, gremlinQuery);

            return passengerSegments;
        }

        public async Task<List<TripInformation>> GetTripsByReservationId(string loggingContext, string reservationId)
        {
            string methodName = "GetTripsByReservationId";
            List<TripInformation> tripsList = new List<TripInformation>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE().otherV().valueMap(true)", reservationId);

            tripsList = await GetListOfNodesAsync<TripInformation>(loggingContext, gremlinQuery);

            return tripsList;
        }

        public async Task<List<Reservation>> GetReservationsByMPNumberAsync(string loggingContext, string mpNumber)
        {
            string methodName = "GetReservationsByMPNumberAsync";
            List<Reservation> reservationList = new List<Reservation>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V().has('MILEAGEPLUS','MileagePlusNumber','{0}').outE('HAS_RESERVATION').otherV().valueMap(true)", mpNumber);

            reservationList = await GetListOfNodesAsync<Reservation>(loggingContext, gremlinQuery);

            return reservationList;
        }

        public async Task<List<Reservation>> GetReservationsByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string methodName = "GetReservationsByDeviceIdAsync";
            List<Reservation> reservationList = new List<Reservation>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V().has('APPLICATION','DeviceId','{0}').outE('HAS_RESERVATION').otherV().valueMap(true)", deviceId);

            reservationList = await GetListOfNodesAsync<Reservation>(loggingContext, gremlinQuery);

            return reservationList;
        }

        public async Task<List<Reservation>> GetReservationsByEmployeeIdAsync(string loggingContext, string employeeId)
        {
            string methodName = "GetReservationsByEmployeeIdAsync";
            List<Reservation> reservationList = new List<Reservation>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V().has('EMPLOYEE','EmployeeId','{0}').outE('HAS_RESERVATION').otherV().valueMap(true)", employeeId);

            reservationList = await GetListOfNodesAsync<Reservation>(loggingContext, gremlinQuery);

            return reservationList;
        }

        public async Task<List<PassengerSegmentData>> GetPassengerPersistedDataByPassengerSegmentId(string loggingContext, string passengerSegmentId)
        {
            string methodName = "GetPassengerPersistedDataByPassengerSegmentId";
            List<PassengerSegmentData> passengerSegmentPersistedData = new List<PassengerSegmentData>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE().otherV().valueMap(true)", passengerSegmentId);

            passengerSegmentPersistedData = await GetListOfNodesAsync<PassengerSegmentData>(loggingContext, gremlinQuery);

            return passengerSegmentPersistedData;
        }

        public async Task<List<ReservationSegment>> GetReservationSegmentsByTripId(string loggingContext, string tripId)
        {
            string methodName = "GetReservationSegmentsByTripId";
            List<ReservationSegment> reservationSegments = new List<ReservationSegment>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE().otherV().valueMap(true)", tripId);

            reservationSegments = await GetListOfNodesAsync<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegments;
        }

        public async Task<List<Application>> GetApplicationsByMpNumber(string loggingContext, string mpNumber)
        {
            string methodName = "GetApplicationsByMpNumber";
            List<Application> applications = new List<Application>();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE('IS_SIGNED_IN').otherV().valueMap(true)", mpNumber);

            applications = await GetListOfNodesAsync<Application>(loggingContext, gremlinQuery);

            return applications;
        }

        public async Task UpdatePassengerPersistedNode(string loggingContext, PassengerSegmentData passengerSegmentPersistData)
        {
            string methodName = "UpdatePassengerPersistedNode";

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            await UpsertNodeAsync<PassengerSegmentData>(loggingContext, passengerSegmentPersistData);
        }

        public async Task<Model.ActiveSegmentDomain.ActiveSegment> SaveActiveSegmentAsync(string loggingContext, Model.ActiveSegmentDomain.ActiveSegment activeSegment)
        {
            string methodName = "SaveActiveSegmentAsync";

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            Model.ActiveSegmentDomain.ActiveSegment activeSegmentFromRepo = await UpsertNodeAsync<Model.ActiveSegmentDomain.ActiveSegment>(loggingContext, activeSegment);

            return activeSegmentFromRepo;
        }

        public async Task<ScheduledFlightSegment> GetScheduledFlightSegmentByFlifoKey(string loggingContext, string flightStatusSegmentId)
        {
            string methodName = "GetScheduledFlightSegmentByFlifoKey";
            ScheduledFlightSegment scheduledFlightSegment = new ScheduledFlightSegment();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').valueMap(true)", flightStatusSegmentId);

            var scheduledFlightSegments = await GetListOfNodesAsync<ScheduledFlightSegment>(loggingContext, gremlinQuery);

            if (scheduledFlightSegments != null && scheduledFlightSegments.Count > 0)
            {
                foreach (var scheduledSegment in scheduledFlightSegments)
                {
                    scheduledFlightSegment = scheduledSegment;
                }
            }

            return scheduledFlightSegment;
        }

        public async Task<OperationalFlightSegment> GetOperationalFlightSegmentByFlifoKey(string loggingContext, string scheduledFlightSegmentId)
        {
            string methodName = "GetOperationalFlightSegmentByFlifoKey";
            OperationalFlightSegment operationalFlightSegment = new OperationalFlightSegment();

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            string gremlinQuery = string.Format("g.V('{0}').outE().otherV().valueMap(true)", scheduledFlightSegmentId);

            var operationalFlightSegments = await GetListOfNodesAsync<OperationalFlightSegment>(loggingContext, gremlinQuery);

            if (operationalFlightSegments != null && operationalFlightSegments.Count > 0)
            {
                operationalFlightSegments = operationalFlightSegments.OrderBy(s => s.EstimatedDepartureDateTimeUTC).ToList();
                foreach (var operationalLeg in operationalFlightSegments)
                {
                    if (!operationalLeg.IsSegmentCancelled)
                    {
                        if (!(operationalLeg.ActualArrivalDateTimeUtc != null && operationalLeg.ActualArrivalDateTimeUtc != DateTime.MinValue))
                        {
                            operationalFlightSegment = operationalLeg;
                        }
                    }
                }
            }

            return operationalFlightSegment;
        }

        public async Task UpdateActiveSegmentForReservationAsync(string loggingContext, string recordLocator, string creationDate, string reservationSegmentPredictableKey)
        {
            string methodName = "UpdateActiveSegmentForReservationAsync";

            _logger.LogInformation("ActiveSegmentNeptuneRepository - MethodName: {@MethodName}", methodName);

            var reservationPredictableKey = $"RESERVATION::{recordLocator}::{creationDate}";
            HasActiveSegment hasActiveSegment = new HasActiveSegment(reservationPredictableKey, reservationSegmentPredictableKey);

            await UpsertEdgeAsync<HasActiveSegment>(loggingContext, hasActiveSegment);
        }

        public async Task<Employee> GetEmployeeDetailsAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<Employee>(loggingContext, id);
        }
        
        public Task<Model.ActiveSegmentDomain.ActiveSegment> GetAsync(string loggingContext, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Model.ActiveSegmentDomain.ActiveSegment> UpsertAsync(string loggingContext, Model.ActiveSegmentDomain.ActiveSegment entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string loggingContext, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reservation>> GetListOfReservationsByTraversalAsync<Reservation>(string loggingContext, string id, string relationName)
        {
            string methodName = "GetListOfReservationsByTraversalAsync";
            string gremlinQuery = await Task.Run(() => BuildGremlinQueryForComplexObjectByTraversal<Reservation>(loggingContext, id, relationName)).ConfigureAwait(false);
            List<Mobile.Model.Common.GraphResponse<Reservation>> graphResponse = await GetComplexObjectAsync<Reservation>(loggingContext, gremlinQuery);
            List<Reservation> reservations = null;

            if(graphResponse != null && graphResponse.Any())
            {
                reservations = graphResponse.Select(x => x.Data).ToList();
            }

            return reservations;
        }

        public async Task<ReservationSegment> GetActiveSegmentForReservationAsync(string loggingContext, string recordLocator, string creationDate)
        {
            string methodName = "GetActiveSegmentForReservationAsync";
            string gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').outE('HAS_ACTIVESEGMENT').inV().valueMap(true).fold()";
            ReservationSegment reservationSegment = await GetNodeAsyncByQuery<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegment;
        }

        public async Task<List<Application>> GetApplicationsForReservationAsync(string loggingContext, string recordLocator, string creationDate)
        {
            string methodName = "GetApplicationsForReservationAsync";
            string gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('APPLICATION').valueMap(true).fold()";
            List<Application> appications = await GetListOfNodesAsync<Application>(loggingContext, gremlinQuery);

            return appications;
        }
        
        public async Task<List<MileagePlus>> GetMileagePlusForReservationAsync(string loggingContext, string recordLocator, string creationDate)
        {
            string methodName = "GetMileagePlusForReservationAsync";
            string gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('MILEAGEPLUS').valueMap(true).fold()";
            List<MileagePlus> mileagePlus = await GetListOfNodesAsync<MileagePlus>(loggingContext, gremlinQuery);

            return mileagePlus;
        }

        public async Task<Employee> GetEmployeeForReservationAsync(string loggingContext, string recordLocator, string creationDate)
        {
            string methodName = "GetEmployeeForReservationAsync";
            string gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('EMPLOYEE').valueMap(true).fold()";
            Employee employee = await GetNodeAsyncByQuery<Employee>(loggingContext, gremlinQuery);

            return employee;
        }
    }
}
