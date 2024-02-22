using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.MPAuthentication;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.ReferenceDataModel;
using United.TravelBank.Model;
using United.TravelBank.Model.BalancesDataModel;
using United.Utility.Helper;

namespace United.Common.Helper.Profile
{
    public class MileagePlus : IMileagePlus
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<MileagePlus> _logger;
        private readonly IDPService _dPService;
        private readonly ILoyaltyUCBService _loyaltyBalanceServices;
        private readonly IEmployeeIdByMileageplusNumber _employeeIdByMileageplusNumber;
        private readonly IFeatureSettings _featureSettings;

        public MileagePlus(IConfiguration configuration
            , ICacheLog<MileagePlus> logger
            , IDPService dPService
            , IEmployeeIdByMileageplusNumber employeeIdByMileageplusNumber
            , ILoyaltyUCBService loyaltyBalanceServices
            , IFeatureSettings featureSettings
           )
        {
            _configuration = configuration;
            _logger = logger;
            _dPService = dPService;
            _employeeIdByMileageplusNumber = employeeIdByMileageplusNumber;
            _loyaltyBalanceServices = loyaltyBalanceServices;
            _featureSettings = featureSettings;
        }


        public async Task<(string employeeId, string displayEmployeeId)> GetEmployeeIdy(string mileageplusNumber, string transactionId, string sessionId, string displayEmployeeId)
        {
            string employeeId = string.Empty;

            var response = await _employeeIdByMileageplusNumber.GetEmployeeIdy(mileageplusNumber, transactionId, sessionId).ConfigureAwait(false);
            var empResponse = JsonConvert.DeserializeObject<GetEmpIdByMpNumber>(response);
            if (empResponse.MPLinkedId != null)
            {
                displayEmployeeId = empResponse.FileNumber;
                employeeId = empResponse.MPLinkedId;
            }
            else
            {
                displayEmployeeId = empResponse.EmployeeId;
                employeeId = empResponse.EmployeeId;
            }
            if (employeeId == null)
            {
                displayEmployeeId = string.Empty;
                employeeId = string.Empty;
            }
            return (employeeId, displayEmployeeId);
        }

        /// <summary>
        /// Takes the Request And Token And returns the pluspoiunts Object. 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="dpToken"></param>
        /// <returns></returns>
        public async Task<MOBPlusPoints> GetPlusPointsFromLoyaltyBalanceService(MPAccountValidationRequest req, string dpToken)
        {
            string loyaltyBalanceUrl = string.Format(_configuration.GetValue<string>("MyAccountLoyaltyBalanceUrl"), req.MileagePlusNumber);
            string jsonResponse = string.Empty;
            try
            {
                //if (Utility.GetBooleanConfigValue("ByPassGetPremierActivityRequestValidationnGetCachedDPTOken") == true) // If the value of ByPassGetPremierActivityRequestValidationnGetCachedDPTOken is true then go get FLIFO Dp Token which we currenlty used by Flight Status NOTE: Alreday confirmed with Greg & Bob that they are not using internal to validate DP Token.
                dpToken = await _dPService.GetAnonymousToken(req.Application.Id, req.DeviceId, _configuration);

                jsonResponse = await _loyaltyBalanceServices.GetLoyaltyBalance(dpToken, req.MileagePlusNumber, req.SessionId);
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    throw new System.Exception(wex.Message);
                }
            }
            if (!string.IsNullOrEmpty(jsonResponse))
            {
                return await GetPlusPointsFromJson(jsonResponse, req).ConfigureAwait(false);
            }
            return null;
        }

        /// <summary>
        /// Takes the service response and returns the plus points object. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<MOBPlusPoints> GetPlusPointsFromJson(string request, MPAccountValidationRequest req)
        {

            BalanceResponse PlusPointResponse = JsonConvert.DeserializeObject<BalanceResponse>(request);
            Balance plusPointsBalance;
            SubBalance subBalanceRequested;
            SubBalance subBalanceConfirmed;
            if (PlusPointResponse.Balances != null && (plusPointsBalance = PlusPointResponse.Balances.FirstOrDefault(ct => ct.ProgramCurrencyType == TravelBankConstants.ProgramCurrencyType.UGC)) != null && plusPointsBalance.SubBalances != null &&
                (subBalanceRequested = plusPointsBalance.SubBalances.FirstOrDefault(s => s.Type.ToUpper() == "REQUESTED")) != null &&
                (subBalanceConfirmed = plusPointsBalance.SubBalances.FirstOrDefault(s => s.Type.ToUpper() == "CONFIRMED")) != null &&
                !(plusPointsBalance.TotalBalance == 0 && subBalanceRequested.Amount == 0 && subBalanceConfirmed.Amount == 0))
            {
                List<MOBKVP> kvpList = new List<MOBKVP>();
                if (plusPointsBalance.BalanceDetails != null)
                {
                    foreach (BalanceDetail bd in plusPointsBalance.BalanceDetails)
                    {
                        kvpList.Add(new MOBKVP(bd.ProgramCurrencyAmount.ToString("0.##"), bd.ExpirationDate.ToString("MMM dd, yyyy")));
                    }
                }
                MOBPlusPoints pluspoints = new MOBPlusPoints();
                pluspoints.PlusPointsAvailableText = _configuration.GetValue<string>("PlusPointsAvailableText");
                pluspoints.PlusPointsAvailableValue = plusPointsBalance.TotalBalance.ToString("0.##") +
                                                      " (" + subBalanceRequested.Amount.ToString("0.##") + " requested)";
                pluspoints.PlusPointsDeductedText = _configuration.GetValue<string>("PlusPointsDeductedText");
                pluspoints.PlusPointsDeductedValue = subBalanceConfirmed.Amount.ToString("0.##");
                pluspoints.PlusPointsExpirationText = _configuration.GetValue<string>("PlusPointsExpirationText");
                if (plusPointsBalance.EarliestExpirationDate != null && plusPointsBalance.EarliestExpirationDate.Value != null)
                {
                    pluspoints.PlusPointsExpirationValue = plusPointsBalance.EarliestExpirationDate.Value.ToString("MMM dd, yyyy");
                }
                else
                {
                    pluspoints.IsHidePlusPointsExpiration = true;
                }
                pluspoints.PlusPointsUpgradesText = _configuration.GetValue<string>("viewUpgradesText");
                pluspoints.PlusPointsUpgradesLink = _configuration.GetValue<string>("viewUpgradesLink");
                pluspoints.PlusPointsExpirationInfo = _configuration.GetValue<string>("PlusPointsExpirationInfo");
                pluspoints.PlusPointsExpirationInfoHeader = _configuration.GetValue<string>("PlusPointsExpirationInfoHeader");
                pluspoints.PlusPointsExpirationInfoPointsSubHeader = _configuration.GetValue<string>("PlusPointsExpirationInfoPointsSubHeader");
                pluspoints.PlusPointsExpirationInfoDateSubHeader = _configuration.GetValue<string>("PlusPointsExpirationInfoDateSubHeader");
                pluspoints.ExpirationPointsAndDatesKVP = kvpList;
                pluspoints.RedirectToDotComMyTripsWithSSOCheck = _configuration.GetValue<bool>("EnablePlusPointsSSO");
                if (_configuration.GetValue<bool>("EnablePlusPointsSSO"))
                {
                    pluspoints.WebSessionShareUrl = _configuration.GetValue<string>("DotcomSSOUrl");

                    //pluspoints.WebShareToken = Utility.GetSSOToken
                    //  (req.Application.Id, req.DeviceId, req.Application.Version.Major, req.TransactionId,
                    //  null, req.SessionId, req.MileagePlusNumber, LogEntries, levelSwitch);
                    pluspoints.WebShareToken = _dPService.GetSSOTokenString(req.Application.Id, req.MileagePlusNumber, _configuration)?.ToString();

                    if (await _featureSettings.GetFeatureSettingValue("EnableRedirectURLUpdate").ConfigureAwait(false))
                    {
                        pluspoints.PlusPointsUpgradesLink = $"{_configuration.GetValue<string>("NewDotcomSSOUrl")}?type=sso&token={pluspoints.WebShareToken}&landingUrl={pluspoints.PlusPointsUpgradesLink}";
                        pluspoints.WebSessionShareUrl = pluspoints.WebShareToken = string.Empty;
                    }
                }
                return await Task.FromResult(pluspoints);
            }
            return null;
        }

    }
}
