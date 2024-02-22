using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.CSLModels;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper.Profile
{
    public interface IMPTraveler
    {
        Task<(List<MOBCPTraveler> mobTravelersOwnerFirstInList, bool isProfileOwnerTSAFlagOn, List<MOBKVP> savedTravelersMPList)> PopulateTravelers(List<United.Services.Customer.Common.Traveler> travelers, string mileagePluNumber, bool isProfileOwnerTSAFlagOn, bool isGetCreditCardDetailsCall, MOBCPProfileRequest request, string sessionid, bool getMPSecurityDetails = false, string path = "");

        string GetYAPaxDescByDOB();
        System.Threading.Tasks.Task UpdateViewName(MOBUpdateTravelerRequest request);
        Task<bool> UpdateTravelerBase(MOBUpdateTravelerRequest request);
        Task<(bool updateTravelerSuccess, List<MOBItem> insertUpdateItemKeys)> UpdateTravelerV2(MOBUpdateTravelerRequest request, List<MOBItem> insertUpdateItemKeys);
        List<Mobile.Model.Common.MOBAddress> PopulateTravelerAddresses(List<United.Services.Customer.Common.Address> addresses, MOBApplication application = null, string flow = null);
        List<MOBCPSecureTraveler> PopulatorSecureTravelers(List<United.Services.Customer.Common.SecureTraveler> secureTravelers, ref bool isTSAFlag, bool correctDate, string sessionID, int appID, string deviceID);
        Task<MOBCPMileagePlus> GetCurrentEliteLevelFromAirPreferences(List<United.Services.Customer.Common.AirPreference> airPreferences, string sessionid);
        List<MOBCPPhone> PopulatePhones(List<United.Services.Customer.Common.Phone> phones, bool onlyDayOfTravelContact);
        List<MOBEmail> PopulateEmailAddresses(List<Services.Customer.Common.Email> emailAddresses, bool onlyDayOfTravelContact);
        List<MOBPrefAirPreference> PopulateAirPrefrences(List<United.Services.Customer.Common.AirPreference> airPreferences);
        Task<(List<MOBCPTraveler> mobTravelersOwnerFirstInList, bool isProfileOwnerTSAFlagOn, List<MOBKVP> savedTravelersMPList)> PopulateTravelersV2(List<TravelerProfileResponse> travelers, string mileagePluNumber, bool isProfileOwnerTSAFlagOn, bool isGetCreditCardDetailsCall, MOBCPProfileRequest request, string sessionid, bool getMPSecurityDetails = false, string path = "");
        MOBCPMileagePlus PopulateMileagePlusV2(OwnerResponseModel profileOwnerResponse, string mileageplusId);
        Task<OwnerResponseModel> GetProfileOwnerInfo(string token, string sessionId, string mileagePlusNumber);
        List<MOBCPSecureTraveler> PopulatorSecureTravelersV2(SecureTravelerResponseData secureTravelerResponseData, ref bool isTSAFlag, bool correctDate, string sessionID, int appID, string deviceID);
        List<MOBCPPhone> PopulatePhonesV2(TravelerProfileResponse traveler, bool onlyDayOfTravelContact);
        List<MOBBKLoyaltyProgramProfile> GetTravelerRewardPgrograms(List<RewardProgramData> rewardPrograms, int currentEliteLevel);
        List<MOBEmail> PopulateEmailAddressesV2(List<United.Mobile.Model.CSLModels.Email> emailAddresses, bool onlyDayOfTravelContact);
        List<MOBEmail> PopulateAllEmailAddressesV2(List<United.Mobile.Model.CSLModels.Email> emailAddresses);
        List<MOBPrefAirPreference> PopulateAirPrefrencesV2(TravelerProfileResponse traveler);
        List<Mobile.Model.Common.MOBAddress> PopulateTravelerAddressesV2(List<United.Mobile.Model.CSLModels.Address> addresses, MOBApplication application = null, string flow = null);
        Task<List<MOBTypeOption>> GetProfileDisclaimerList();
        System.Threading.Tasks.Task MakeProfileOwnerServiceCall(MOBCPProfileRequest request);
    }

}
