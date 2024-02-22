using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;
using United.Mobile.DataAccess.MPAuthentication;
using United.Mobile.Model.Common;
using United.Utility.Helper;

namespace United.Common.Helper.EmployeeReservation
{
    public class EmployeeReservations : IEmployeeReservations
    {
        private readonly ICacheLog<EmployeeReservations> _logger;
        private readonly IEResEmployeeProfileService _eResEmployeeProfileService;
        private readonly IHeaders _headers;

        public EmployeeReservations(ICacheLog<EmployeeReservations> logger
            , IEResEmployeeProfileService eResEmployeeProfileService
            , IHeaders headers)
        {
            _logger = logger;
            _eResEmployeeProfileService = eResEmployeeProfileService;
            _headers = headers;
        }

        public async Task<EmployeeJA> GetEResEmp20PassriderDetails(string employeeId, string token, string TransactionId, int ApplicationId, string AppVersion, string DeviceId)
        {
            var encryptedEmployeeId = new AesEncryptAndDecrypt().Encrypt(employeeId);
            string path = $"/Employee/Emp20PassriderDetails?employeeID={HttpUtility.UrlEncode(encryptedEmployeeId)}";

            EmployeeJA response;

            try
            {
                //string jsonResponse = HttpHelper.Get(url, "application/json; charset=utf-8", token);
                response = await _eResEmployeeProfileService.GetEResEmpProfile<EmployeeJA>(token, path, _headers.ContextValues.SessionId).ConfigureAwait(false);
            }
            catch (System.Net.WebException webException)
            {
                _logger.LogError("GetEmpProfileCSL_CFOP - eResEmp20PassriderDetails Error {@WebException}", JsonConvert.SerializeObject(webException));
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetEmpProfileCSL_CFOP - eResEmp20PassriderDetails Error {@Exception}", JsonConvert.SerializeObject(ex));
                throw;
            }

            return response;
        }
    }
}
