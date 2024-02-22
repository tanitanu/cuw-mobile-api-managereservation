using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.SeatEngine
{
    public class ComplimentaryUpgradeService : IComplimentaryUpgradeService
    {
        private readonly ICacheLog<ComplimentaryUpgradeService> _logger;
        private readonly IResilientClient _resilientClient;

        public ComplimentaryUpgradeService([KeyFilter("SQLDBComplimentaryUpgradeClientKey")] IResilientClient resilientClient, ICacheLog<ComplimentaryUpgradeService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<List<CabinBrand>> GetComplimentaryUpgradeOfferedFlagByCabinCount(string Origin, string destination, int numberOfCabins, string sessionId, string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" }
                     };
            string path = string.Format("?Origin={0}&destination={1}&numberOfCabins={2}&sessionId={3}", Origin, destination, numberOfCabins, sessionId);

            _logger.LogInformation("SeatEngine OnPrem SQL Call- GetComplimentaryUpgradeOfferedFlagByCabinCount {RequestPath}", path);

            IDisposable timer = null;
            try
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("SeatEngine GetComplimentaryUpgradeOffer SQLDB Service {RequestUrl} error {@esponse}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                _logger.LogInformation("SeatEngine GetComplimentaryUpgradeOffer SQLDB Service {RequestUrl} , {Response}", responseData.url, responseData.response);

                return JsonConvert.DeserializeObject<List<CabinBrand>>(responseData.response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CSL service-GetComplimentaryUpgradeOffer error {Exception}", JsonConvert.SerializeObject(ex));
            }

            return default;
        }
    }
}
