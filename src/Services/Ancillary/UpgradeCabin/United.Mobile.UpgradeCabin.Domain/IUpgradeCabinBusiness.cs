using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.UpgradeCabin;

namespace United.Mobile.UpgradeCabin.Domain
{
    public interface IUpgradeCabinBusiness
    {
        Task<MOBUpgradeCabinEligibilityResponse> UpgradeCabinEligibleCheck(MOBUpgradeCabinEligibilityRequest request);
        Task<MOBUpgradeCabinRegisterOfferResponse> UpgradeCabinRegisterOfferAsync(MOBUpgradeCabinRegisterOfferRequest request, United.Mobile.Model.Common.Session session);
        Task<MOBUpgradePlusPointWebMyTripResponse> UpgradePlusPointWebMyTrip(MOBUpgradePlusPointWebMyTripRequest request);
        Task<List<MOBItem>> CreateSSOInformation(MOBRequest request, string mileageplusnumber, string hashpincode, string sessionid);
    }
}
