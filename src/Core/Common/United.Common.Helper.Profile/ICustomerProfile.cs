using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.CSLModels;

namespace United.Common.Helper.Profile
{
    public interface ICustomerProfile
    {
        Task<List<MOBCPProfile>> PopulateProfiles(string sessionId, string mileagePlusNumber, int customerId, List<United.Services.Customer.Common.Profile> profiles, MOBCPProfileRequest request, bool getMPSecurityDetails = false, string path = "", MOBApplication application = null);
        bool EnableYoungAdult(bool isReshop = false);
        System.Threading.Tasks.Task GetChaseCCStatement(MOBCPProfileRequest req);
        Task<Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse>> GetProfileDetails(MOBCPProfileRequest request, bool getMPSecurityDetails = false, bool isCorporateBooking = false);
    }
}
