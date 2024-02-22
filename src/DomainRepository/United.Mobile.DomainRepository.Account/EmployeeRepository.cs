using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using United.GraphDatabase.Gremlin.Groovy.Client;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Internal.ReservationDomain;
using United.Net.Http;

namespace United.Mobile.DomainRepository.Account
{
    public class EmployeeRepository : NeptuneGremlinBaseRepository, IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly byte[] _secretKey;
        private string employeeProfileAuthToken;
        private DateTime employeeProfileAuthExpiration;

        private string EncryptEmployeeIdWithAES(string employeeId)
        {
            byte[] cipherText;

            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(_secretKey, _secretKey);
                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using StreamWriter sw = new StreamWriter(cs);
                    sw.Write(employeeId);
                }
                cipherText = ms.ToArray();
            }

            return Convert.ToBase64String(cipherText);
        }

        private async Task<string> GetAuthTokenAsync()
        {
            string methodName = "GetAuthTokenAsync";
            if (!string.IsNullOrEmpty(employeeProfileAuthToken) && employeeProfileAuthExpiration > DateTime.UtcNow.AddMinutes(15))
                return employeeProfileAuthToken;

            // renew token
            var httpClient = ResilientHttpClient.GetInstance(_configuration["EmployeeProfileAuthURL"]);
            var request = new EmployeeProfileAuthRequest
            {
                ClientId = _configuration["EmployeeProfileClientId"],
                ClientSecret = _configuration["EmployeeProfileClientSecret"],
                EndUserAgentId = "",
                EndUserAgentIp = "127.0.0.1",
                GrantType = "client_credentials",
                Scope = "openid mobile-androidphone",
                UserType = "guest"
            };

            try
            {
                _logger.LogTrace(
                    "EmployeeRepository - Trying to get auth token for employee profile call. MethodName: {MethodName}",
                    methodName);
                var responseString = await httpClient.PostAsync(JsonConvert.SerializeObject(request));
                var response = JsonConvert.DeserializeObject<EmployeeProfileAuthResponse>(responseString);
                employeeProfileAuthToken = response.AccessToken;
                employeeProfileAuthExpiration = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
                _logger.LogTrace(
                    "EmployeeRepository - Renewed auth token for employee profile call successfully. MethodName: {MethodName}",
                    methodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "EmployeeRepository - Failed to get auth token for employee profile call. MethodName: {MethodName}",
                    methodName);
            }
            return employeeProfileAuthToken;
        }

        public EmployeeRepository(ILogger<EmployeeRepository> logger,
            IClient client,
            IConfiguration configuration) : base(logger, client)
        {
            _logger = logger;
            _configuration = configuration;
            _secretKey = Encoding.UTF8.GetBytes(configuration["EmployeeEncryptionKey"]);
        }

        public async Task DeleteAsync(string loggingContext, string id, long messagingTimestampTicks)
        {
            await DeleteNodeAsync(loggingContext, id, messagingTimestampTicks);
        }

        public async Task<bool> ExistsAsync(string loggingContext, string id)
        {
            return await ExistsNodeAsync(loggingContext, id);
        }

        public async Task<Employee> GetAsync(string loggingContext, string id)
        {
            return await GetNodeAsync<Employee>(loggingContext, id);
        }

        public async Task<Employee> GetEmployeeInfoFromCSLAsync(string loggingContext, string employeeId)
        {
            string methodName = "GetEmployeeInfoFromCSLAsync";

            Employee employee = null;
            var employeeProfileUrl = _configuration["EmployeeProfileURL"];
            var httpClient = ResilientHttpClient.GetInstance(employeeProfileUrl);
            var request = new EmployeeProfileRequest { EmployeeId = EncryptEmployeeIdWithAES(employeeId) };
            var authToken = await GetAuthTokenAsync();

            try
            {
                _logger.LogInformation(
                    "EmployeeRepository - Trying to retrieve employee profile from employee {EmployeeId} from CSL. MethodName: {MethodName}",
                    employeeId, methodName);
                var responseString = await httpClient.PostAsync(JsonConvert.SerializeObject(request),
                        new Dictionary<string, string> { { "Authorization", $"Bearer {authToken}" } });
                var employeeProfile = JsonConvert.DeserializeObject<EmployeeProfileResponse>(responseString);
                employee = new Employee(employeeId)
                {
                    EmployeeId = employeeId,
                    MileagePlusNumber = employeeProfile.MileagePlus,
                    FirstName = employeeProfile.EmployeeJA.Employee.FirstName,
                    MiddleName = employeeProfile.EmployeeJA.Employee.MiddleName,
                    LastName = employeeProfile.EmployeeJA.Employee.LastName,
                    NameSuffix = employeeProfile.EmployeeJA.Employee.NameSuffix,
                    Gender = employeeProfile.EmployeeJA.Employee.Gender,
                    DateOfBirth = DateTime.Parse(employeeProfile.EmployeeJA.Employee.BirthDate),
                    MessagingTimestampTicks = DateTime.UtcNow.Ticks
                };
                _logger.LogInformation(
                    "EmployeeRepository - Successfully retrieved employee profile from employee {EmployeeId} from CSL. MethodName: {MethodName}",
                    employeeId, methodName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "EmployeeRepository - Failed to retrieve employee profile from employee {EmployeeId} from CSL. MethodName: {MethodName}",
                    employeeId, methodName);
            }

            return employee;
        }

        public async Task<Employee> UpsertAsync(string loggingContext, Employee entity)
        {
            return await UpsertNodeAsync(loggingContext, entity);
        }

        public async Task<List<ReservationSegment>> GetActiveReservationSegmentsByEmployeeIdAsync(string loggingContext, string employeeId)
        {
            string methodName = "GetActiveReservationSegmentsByEmployeeIdAsync";
            List<ReservationSegment> reservationSegments = new List<ReservationSegment>();

            _logger.LogInformation("GetActiveReservationSegmentsByEmployeeIdAsync - MethodName: {@MethodName}", methodName);

            string gremlinQuery = $"g.V('ACCOUNT::EMPLOYEE::{employeeId}').outE('HAS_RESERVATION').otherV().out('HAS_ACTIVE_SEGMENT').valueMap(true).fold()";

            reservationSegments = await GetListOfNodesAsync<ReservationSegment>(loggingContext, gremlinQuery);

            return reservationSegments;
        }

        public async Task<Employee> GetEmployeeByDeviceIdAsync(string loggingContext, string deviceId)
        {
            string gremlinQuery = $"g.V('APPLICATION::{deviceId}').in('IS_SIGNED_IN').in('IS_EMPLOYEE').valueMap(true).fold()";
            Employee employee = await GetNodeAsyncByQuery<Employee>(loggingContext, gremlinQuery);

            return employee;
        }
    }
}
