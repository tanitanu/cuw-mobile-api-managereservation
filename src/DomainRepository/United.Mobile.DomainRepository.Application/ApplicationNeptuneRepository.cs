using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Exceptions;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.ReservationDomain;
using United.Mobile.Model.Relation;

namespace United.Mobile.DomainRepository.Application
{
    public class ApplicationNeptuneRepository : NeptuneGremlinBaseRepository, IApplicationRepository
    {
        private readonly ILogger<ApplicationNeptuneRepository> _logger;

        public ApplicationNeptuneRepository(ILogger<ApplicationNeptuneRepository> logger, IClient client)
            : base(logger, client)
        {
            _logger = logger;
        }

        public async Task<bool> CreateApplicationSubscribedItemRelation(string loggingContext, string outVId, string inVId)
        {
            string methodName = "CreateApplicationSubscribedItemRelation";

            _logger.LogInformation("ApplicationNeptuneRepository - Create Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                outVId, inVId, methodName);

            try
            {
                var isSubscribed = new IsSubscribed(outVId, inVId);

                return await UpsertEdgeAsync(loggingContext, isSubscribed, true) != null;
            }
            catch (UnitedMissingGremlinAttributeException attributeEx)
            {
                _logger.LogError(attributeEx, "ApplicationNeptuneRepository - Failed to Create Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    outVId, inVId, methodName);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ApplicationNeptuneRepository - Failed to Create Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    outVId, inVId, methodName);

                return false;
            }
        }

        public async Task<Task> DeleteApplicationSubscribedItemRelation(string loggingContext, string outVId, string inVId)
        {
            string methodName = "DeleteApplicationSubscribedItemRelation";
            _logger.LogInformation("ApplicationNeptuneRepository - Delete Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                outVId, inVId, methodName);

            try
            {
                var isSubscribed = new IsSubscribed(outVId, inVId);
                await DeleteEdgeAsync(loggingContext, isSubscribed, DateTime.UtcNow.Ticks);
            }
            catch (UnitedMissingGremlinAttributeException attributeEx)
            {
                _logger.LogError(attributeEx, "ApplicationNeptuneRepository - Failed to Delete Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    outVId, inVId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ApplicationNeptuneRepository - Failed to Delete Application Subscribed Item Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    outVId, inVId, methodName);
            }

            return Task.CompletedTask;
        }

        public async Task<List<Model.Internal.Application.Application>> GetApplicationsByFlifo(string loggingContext, string flifoPredictableKey)
        {
            string methodName = "GetApplicationsByFlifo";
            var gremlinQuery = new StringBuilder();

            _logger.LogInformation("ApplicationNeptuneRepository - Get Application Notification Settings - Flight: {@FlifoPredictableKey} - MethodName: {@MethodName}",
                flifoPredictableKey, methodName);

            gremlinQuery.Append("g.V()");
            gremlinQuery.Append($".hasLabel('{EntityAndRelationConstants.SubscribedItemLabel}').has('isDeleted', false).has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks}))");
            gremlinQuery.Append($".has('predictableKey', '{flifoPredictableKey}')");
            gremlinQuery.Append($".in().has('isDeleted', false).has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks})).valueMap(true)");

            return await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery.ToString());
        }

        public async Task<Model.Internal.Application.Application> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<Model.Internal.Application.Application>(loggingContext, id);
        }

        public async Task<Model.Internal.Application.Application> UpsertAsync(string loggingContext, Model.Internal.Application.Application entity)
        {
            return await UpsertNodeAsync(loggingContext, entity);
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<List<Model.Internal.Application.Application>> GetApplicationsByMpNumber(string loggingContext, string mpNumber)
        {
            string methodName = "GetApplicationsByMpNumber";
            var gremlinQuery = new StringBuilder();

            _logger.LogInformation("ApplicationNeptuneRepository - Get Application Notification Settings - MpNumber: {mpNumber} - MethodName: {@MethodName}",
                mpNumber, methodName);

            gremlinQuery.Append("g.V()");
            gremlinQuery.Append($".hasLabel('{EntityAndRelationConstants.ApplicationLabel}')");
            gremlinQuery.Append($".has('mileagePlusNumber', '{mpNumber}')");
            gremlinQuery.Append($".has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks})).valueMap(true)");

            return await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery.ToString());
        }

        public async Task<List<Model.Internal.Application.Application>> GetApplicationsByPushToken(string loggingContext, string pushToken)
        {
            string methodName = "GetApplicationsByPushToken";
            var gremlinQuery = new StringBuilder();

            _logger.LogInformation("ApplicationNeptuneRepository - Get Application Notification Settings - PushToken: {pushToken} - MethodName: {@MethodName}",
                pushToken, methodName);

            gremlinQuery.Append("g.V()");
            gremlinQuery.Append($".hasLabel('{EntityAndRelationConstants.ApplicationLabel}')");
            gremlinQuery.Append($".has('pushToken', '{pushToken}')");
            gremlinQuery.Append($".has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks})).valueMap(true)");

            return await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery.ToString());
        }

        public async Task<List<Model.Internal.Application.Application>> GetApplicationsBySubscribedItem(string loggingContext, string predictableKey)
        {
            string methodName = "GetApplicationsBySubscribedItem";
            var gremlinQuery = new StringBuilder();

            _logger.LogInformation("ApplicationNeptuneRepository - Get Application Notification Settings - Flight: {@PredictableKey} - MethodName: {@MethodName}",
                predictableKey, methodName);

            gremlinQuery.Append("g.V()");
            gremlinQuery.Append($".hasLabel('{EntityAndRelationConstants.SubscribedItemLabel}').has('isDeleted', false).has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks}))");
            gremlinQuery.Append($".has('predictableKey', '{predictableKey}')");
            gremlinQuery.Append($".in().has('isDeleted', false).has('timeToLiveTicks', gt({System.DateTime.UtcNow.Ticks})).valueMap(true)");

            return await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery.ToString());
        }
        public Task<List<Model.Internal.Application.Application>> GetApplicationsByPNR(string loggingContext, string recordLocator, string carrierCode, int flightNumber, string flightDaate, string origin, string destination)
        {
            string methodName = "GetApplicationsByPNR";
            var gremlinQuery = new StringBuilder();
            _logger.LogInformation("ApplicationNeptuneRepository - Get Application Notification Settings ");

            return null;
        }

        public async Task<bool> CreateApplicationReservationRelation(string loggingContext, string applicationId, string reservationId)
        {

            string methodName = "CreateApplicationReservationRelation";

            _logger.LogInformation("ApplicationNeptuneRepository - Create Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                applicationId, reservationId, methodName);

            try
            {
                var hasReservation = new HasReservationAdded(applicationId, reservationId);

                return await UpsertEdgeAsync(loggingContext, hasReservation, true) != null;
            }
            catch (UnitedMissingGremlinAttributeException attributeEx)
            {
                _logger.LogError(attributeEx, "ApplicationNeptuneRepository - Failed to Create Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    applicationId, reservationId, methodName);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ApplicationNeptuneRepository - Failed to Create Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    applicationId, reservationId, methodName);

                return false;
            }
        }

        public async Task<bool> DeleteApplicationReservationRelation(string loggingContext, string applicationId, string reservationId)
        {
            string methodName = "DeleteApplicationReservationRelation";
            _logger.LogInformation("ApplicationNeptuneRepository - Delete Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                applicationId, reservationId, methodName);

            try
            {
                var hasReservation = new HasReservationAdded(applicationId, reservationId);
                await DeleteEdgeAsync(loggingContext, hasReservation, DateTime.UtcNow.Ticks);
            }
            catch (UnitedMissingGremlinAttributeException attributeEx)
            {
                _logger.LogError(attributeEx, "ApplicationNeptuneRepository - Failed to Delete Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    applicationId, reservationId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ApplicationNeptuneRepository - Failed to Delete Application Reservation Relationship: OutVertexId: {outVId} - InVertexId: {inVId} - MethodName: {@MethodName}",
                    applicationId, reservationId, methodName);
            }

            return true;
        }

        public async Task<List<Model.Internal.Application.Application>> GetSignedInApplicationsByMpNumberForReservation(string loggingContext, string recordLocator, string creationDate)
        {
            var gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('MILEAGEPLUS').out('IS_SIGNED_IN').valueMap(true).fold()";
            var mpSignedInApplications = await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery);

            return mpSignedInApplications;
        }

        public async Task<List<Model.Internal.Application.Application>> GetSignedInApplicationsByEmployeeIdForReservation(string loggingContext, string recordLocator, string creationDate)
        {
            var gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('EMPLOYEE').out('IS_EMPLOYEE').out('IS_SIGNED_IN').valueMap(true).fold()";
            var employeeApplications = await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery);

            return employeeApplications;
        }

        public async Task<List<Model.Internal.Application.Application>> GetApplicationsForReservation(string loggingContext, string recordLocator, string creationDate)
        {
            var gremlinQuery = $"g.V('RESERVATION::{recordLocator}::{creationDate}').in('HAS_RESERVATION').hasLabel('APPLICATION').valueMap(true).fold()";
            var applications = await GetListOfNodesAsync<Model.Internal.Application.Application>(loggingContext, gremlinQuery);

            return applications;
        }

        public async Task<List<ReservationSegment>> GetActiveReservationSegmentsByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string methodName = "GetActiveReservationSegmentsByDeviceIdAsync";
            List<ReservationSegment> reservationSegments = new List<ReservationSegment>();

            _logger.LogInformation("GetActiveReservationSegmentsByDeviceIdAsync - MethodName: {@MethodName}", methodName);

            string gremlinQuery = $"g.V('APPLICATION::{deviceId}').outE('HAS_RESERVATION').otherV().out('HAS_ACTIVE_SEGMENT').valueMap(true).fold()";

            reservationSegments = await GetListOfNodesAsync<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegments;
        }
    }
}
