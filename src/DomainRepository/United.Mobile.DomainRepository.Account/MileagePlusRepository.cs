using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Internal.Application;
using United.Mobile.Model.Internal.ReservationDomain;
using United.Mobile.Model.Relation;
using United.Net.Http;

namespace United.Mobile.DomainRepository.Account
{
    public class MileagePlusRepository : NeptuneGremlinBaseRepository, IMileagePlusRepository
    {
        private readonly ILogger<MileagePlusRepository> _logger;
        private readonly IConfiguration _configuration;

        public MileagePlusRepository(ILogger<MileagePlusRepository> logger,
            IClient client,
            IConfiguration configuration)
            : base(logger, client)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return ExistsNodeAsync(loggingContext, id);
        }

        public async Task<MileagePlus> GetAccountInfoFromLoyaltyAsync(string loggingContext, string mpNumber)
        {
            string methodName = "GetAccountInfoFromLoyaltyAsync";
            var loyaltyProfileUrl = _configuration["LoyaltyWebserviceBaseURL"];
            var httpClient = ResilientHttpClient.GetInstance(loyaltyProfileUrl);

            MileagePlus mileagePlus = null;
            try
            {
                _logger.LogInformation(
                       "MileagePlusRepository - Trying to retrieve MileagePlus profile from account {MpNumber} from loyalty. MethodName: {MethodName}",
                       mpNumber, methodName);
                var responseStr = await httpClient.GetAsync($"/core/account/{mpNumber}/");
                var serializer = new XmlSerializer(typeof(AccountProfileInfoResponse));
                AccountProfileInfoResponse loyaltyProfileResponse = null;
                using (var reader = new StringReader(responseStr))
                {
                    loyaltyProfileResponse = (AccountProfileInfoResponse)serializer.Deserialize(reader);
                }
                if (loyaltyProfileResponse != null
                                    && loyaltyProfileResponse.AccountProfileInfo != null
                                    && !loyaltyProfileResponse.AccountProfileInfo.IsClosedTemporarily
                                    && !loyaltyProfileResponse.AccountProfileInfo.IsClosedPermanently
                                    && !loyaltyProfileResponse.AccountProfileInfo.IsClosed)
                {
                    mileagePlus = new MileagePlus(mpNumber)
                    {
                        CustomerId = loyaltyProfileResponse.AccountProfileInfo.CustomerId,
                        MileagePlusNumber = loyaltyProfileResponse.AccountProfileInfo.AccountId,
                        EliteStatus = loyaltyProfileResponse.AccountProfileInfo.EliteLevel,
                        FirstName = loyaltyProfileResponse.AccountProfileInfo.FirstName,
                        LastName = loyaltyProfileResponse.AccountProfileInfo.LastName,
                        DateOfBirth = loyaltyProfileResponse.AccountProfileInfo.BirthDate
                    };
                }
                _logger.LogInformation(
                      "MileagePlusRepository - Successfully retrieved MileagePlus profile from account {MpNumber} from loyalty. MethodName: {MethodName}",
                      mpNumber, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                         "MileagePlusRepository - Failed to retrieve MileagePlus profile from account {MpNumber} from loyalty. MethodName: {MethodName}",
                         mpNumber, methodName);
            }

            return mileagePlus;
        }

        public async Task<List<ReservationSegment>> GetActiveReservationSegmentsByMPNumberAsync(string loggingContext, string mpNumber)
        {
            string methodName = "GetActiveReservationSegmentsByMPNumberAsync";
            List<ReservationSegment> reservationSegments = new List<ReservationSegment>();

            _logger.LogInformation("GetActiveReservationSegmentsByMPNumberAsync - MethodName: {@MethodName}", methodName);

            string gremlinQuery = $"g.V('ACCOUNT::{mpNumber}').outE('HAS_RESERVATION').otherV().out('HAS_ACTIVE_SEGMENT').valueMap(true).fold()";

            reservationSegments = await GetListOfNodesAsync<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegments;
        }

        public async Task<MileagePlus> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<MileagePlus>(loggingContext, id);
        }

        public async Task<MileagePlus> GetMileagePlusNumberByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string gremlinQuery = $"g.V('APPLICATION::{deviceId}').in('IS_SIGNED_IN').hasLabel('MILEAGEPLUS').valueMap(true).fold()";
            MileagePlus mileagePlus = await GetNodeAsyncByQuery<MileagePlus>(loggingContext, gremlinQuery);

            return mileagePlus;
        }

        public async Task<bool> RemoveEmployeeRelationAsync(string loggingContext, string mpNumber, string employeeId)
        {
            string methodName = "RemoveEmployeeRelation";
            var updateSucceeded = false;
            try
            {
                _logger.LogInformation(
                    "MileagePlusRepository - Trying to remove relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
                var isEmployee = new IsEmployee(Employee.PredictableKey(employeeId), MileagePlus.PredictableKey(mpNumber));
                await DeleteEdgeAsync(loggingContext, isEmployee, DateTime.UtcNow.Ticks);
                updateSucceeded = true;
                _logger.LogInformation(
                    "MileagePlusRepository - Successfully removed relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "MileagePlusRepository - Failed to remove relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
            }
            return updateSucceeded;
        }

        public async Task<bool> RemoveSignInRelationAsync(string loggingContext, string mpNumber, string deviceId)
        {
            string methodName = "RemoveSignInRelation";
            var updateSucceeded = false;
            try
            {
                _logger.LogInformation(
                    "MileagePlusRepository - Trying to remove relation between MileagePlus {MpNumber} and Device {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
                var isSignedIn = new IsSignedIn(Application.PredictableKey(deviceId), MileagePlus.PredictableKey(mpNumber));
                await DeleteEdgeAsync(loggingContext, isSignedIn, DateTime.UtcNow.Ticks);
                updateSucceeded = true;
                _logger.LogInformation(
                    "MileagePlusRepository - Successfully removed relation between MileagePlus {MpNumber} and Device {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "MileagePlusRepository - Failed to remove relation between MileagePlus {MpNumber} and Device {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
            }
            return updateSucceeded;
        }

        public async Task<MileagePlus> UpsertAsync(string loggingContext, MileagePlus entity)
        {
            return await UpsertNodeAsync(loggingContext, entity);
        }

        public async Task<bool> UpsertEmployeeRelationAsync(string loggingContext, string mpNumber, string employeeId)
        {
            string methodName = "UpsertEmployeeRelation";
            var updateSucceeded = false;
            try
            {
                _logger.LogInformation(
                    "MileagePlusRepository - Trying to upsert relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
                var isEmployee = new IsEmployee(Employee.PredictableKey(employeeId), MileagePlus.PredictableKey(mpNumber));
                updateSucceeded = await UpsertEdgeAsync(loggingContext, isEmployee) != null;
                _logger.LogInformation(
                    "MileagePlusRepository - Successfully upserted relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "MileagePlusRepository - Failed to upsert relation between MileagePlus {MpNumber} and Employee {EmployeeId} - MethodName: {@MethodName}",
                    mpNumber, employeeId, methodName);
            }
            return updateSucceeded;
        }

        public async Task<bool> UpsertSignInRelationAsync(string loggingContext, string mpNumber, string deviceId)
        {
            string methodName = "UpsertSignInRelation";
            var updateSucceeded = false;
            try
            {
                _logger.LogInformation(
                    "MileagePlusRepository - Trying to upsert relation between MileagePlus {MpNumber} and Device {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
                var isSignedIn = new IsSignedIn(Application.PredictableKey(deviceId), MileagePlus.PredictableKey(mpNumber));
                updateSucceeded = await UpsertEdgeAsync(loggingContext, isSignedIn) != null;
                _logger.LogInformation(
                    "MileagePlusRepository - Successfully upserted relation between MileagePlus {MpNumber} and Employee {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "MileagePlusRepository - Failed to upsert relation between MileagePlus {MpNumber} and Employee {DeviceId} - MethodName: {@MethodName}",
                    mpNumber, deviceId, methodName);
            }
            return updateSucceeded;
        }
    }
}
