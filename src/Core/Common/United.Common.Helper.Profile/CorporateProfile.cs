using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.CorporateDirect.Models.CustomerProfile;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Utility.Helper;

namespace United.Common.Helper.Profile
{
    public class CorporateProfile : ICorporateProfile
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<CorporateProfile> _logger;
        private readonly ICustomerCorporateProfileService _customerCorporateProfileService;
        private readonly ISessionHelperService _sessionHelperService;
        public CorporateProfile(
             ICacheLog<CorporateProfile> logger
            , IConfiguration configuration
            , ICustomerCorporateProfileService customerCorporateProfileService
            , ISessionHelperService sessionHelperService
            )
        {
            _configuration = configuration;
            _logger = logger;
            _customerCorporateProfileService = customerCorporateProfileService;
            _sessionHelperService = sessionHelperService;

        }

        public MOBCPCorporate PopulateCorporateData(United.Services.Customer.Common.Corporate corporateData, MOBApplication application = null)
        {
            MOBCPCorporate profileCorporateData = new MOBCPCorporate();
            if (corporateData != null && corporateData.IsValid)
            {
                profileCorporateData.CompanyName = corporateData.CompanyName;
                profileCorporateData.DiscountCode = corporateData.DiscountCode;
                profileCorporateData.FareGroupId = corporateData.FareGroupId;
                profileCorporateData.IsValid = corporateData.IsValid;
                profileCorporateData.VendorId = corporateData.VendorId;
                profileCorporateData.VendorName = corporateData.VendorName;
                if (IsEnableCorporateLeisureBooking(application))
                {
                    profileCorporateData.LeisureDiscountCode = corporateData.LeisureCode;
                }
                if (_configuration.GetValue<bool>("EnableIsArranger"))
                {
                    if (!string.IsNullOrEmpty(corporateData.IsArranger) && corporateData.IsArranger.ToUpper().Equals("TRUE"))
                    {
                        profileCorporateData.NoOfTravelers = string.IsNullOrEmpty(_configuration.GetValue<string>("TravelArrangerCount")) ? 1 : _configuration.GetValue<int>("TravelArrangerCount");
                        profileCorporateData.CorporateBookingType = CORPORATEBOOKINGTYPE.TravelArranger.ToString();
                    }
                }
            }
            return profileCorporateData;
        }
        public bool IsEnableCorporateLeisureBooking(MOBApplication application)
        {
            if (_configuration.GetValue<bool>("EnableCorporateLeisure"))
            {
                if (application != null && GeneralHelper.IsApplicationVersionGreater(application.Id, application.Version.Major, "CorpLiesureAndroidVersion", "CorpLiesureiOSVersion", "", "", true, _configuration))
                {
                    return true;
                }
            }
            return false;
        }
        #region UCB Migration work 
        public async Task<MOBCPCorporate> PopulateCorporateData(MOBCPProfileRequest request)
        {
            var _corprofileResponse = await _sessionHelperService.GetSession<United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse>(request.SessionId, ObjectNames.CSLCorpProfileResponse, new List<string> { request.SessionId, ObjectNames.CSLCorpProfileResponse }).ConfigureAwait(false);
            MOBCPCorporate profileCorporateData = new MOBCPCorporate();
            if (_corprofileResponse?.Profiles != null && (_corprofileResponse.Errors == null || _corprofileResponse.Errors.Count == 0))
            {
                var corporateData = _corprofileResponse.Profiles[0].CorporateData;
                if (corporateData != null && corporateData.IsValid)
                {
                    profileCorporateData.CompanyName = corporateData.CompanyName;
                    profileCorporateData.DiscountCode = corporateData.DiscountCode;
                    profileCorporateData.FareGroupId = corporateData.FareGroupId;
                    profileCorporateData.IsValid = corporateData.IsValid;
                    profileCorporateData.VendorId = corporateData.VendorId;
                    profileCorporateData.VendorName = corporateData.VendorName;
                    profileCorporateData.LeisureDiscountCode = corporateData.LeisureCode;
                    if (_configuration.GetValue<bool>("EnableIsArranger"))
                    {
                        if (corporateData.IsArranger == true)
                        {
                            profileCorporateData.NoOfTravelers = string.IsNullOrEmpty(_configuration.GetValue<string>("TravelArrangerCount")) ? 1 : _configuration.GetValue<int>("TravelArrangerCount");
                            profileCorporateData.CorporateBookingType = CORPORATEBOOKINGTYPE.TravelArranger.ToString();
                        }
                    }
                }
                return profileCorporateData;
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

        }
        public async Task MakeCorpFopServiceCall(MOBCPProfileRequest request)
        {
            CorporateProfileRequest corpProfileRequest = new CorporateDirect.Models.CustomerProfile.CorporateProfileRequest();
            corpProfileRequest.LoyaltyId = request.MileagePlusNumber;
            string jsonRequest = United.Utility.Helper.DataContextJsonSerializer.Serialize<CorporateProfileRequest>(corpProfileRequest);
            var _corprofileResponse = await _customerCorporateProfileService.GetCorporateCreditCards<United.CorporateDirect.Models.CustomerProfile.CorpFopResponse>(request.Token, jsonRequest, request.SessionId).ConfigureAwait(false);

            #region
            if (_corprofileResponse != null && (_corprofileResponse.Errors == null || _corprofileResponse.Errors.Count == 0))
            {
                await _sessionHelperService.SaveSession<United.CorporateDirect.Models.CustomerProfile.CorpFopResponse>(_corprofileResponse, request.SessionId, new List<string> { request.SessionId, ObjectNames.CSLCorFopResponse }, ObjectNames.CSLCorFopResponse);
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            #endregion
        }
        public async Task MakeCorpProfileServiecall(MOBCPProfileRequest request)
        {
            CorporateProfileRequest corpProfileRequest = new CorporateDirect.Models.CustomerProfile.CorporateProfileRequest();
            corpProfileRequest.LoyaltyId = request.MileagePlusNumber;
            string jsonRequest = United.Utility.Helper.DataContextJsonSerializer.Serialize<CorporateProfileRequest>(corpProfileRequest);
            var _corprofileResponse = await _customerCorporateProfileService.GetCorporateprofile<United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse>(request.Token, jsonRequest, request.SessionId).ConfigureAwait(false);
            if (_corprofileResponse != null && (_corprofileResponse.Errors == null || _corprofileResponse.Errors.Count == 0))
            {
                await _sessionHelperService.SaveSession<United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse>(_corprofileResponse, request.SessionId, new List<string> { request.SessionId, ObjectNames.CSLCorpProfileResponse }, ObjectNames.CSLCorpProfileResponse);
            }
            else
            {
                _logger.LogWarning("CSL Call GetCorporateprofile Exception error ", "Logging this error as corporatedirect service failed to return the data.But not throwing error back to client");
            }

        }
        #endregion
    }
}
