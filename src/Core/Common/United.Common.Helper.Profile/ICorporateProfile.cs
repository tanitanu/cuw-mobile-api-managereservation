using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper.Profile
{
    public interface ICorporateProfile
    {
        MOBCPCorporate PopulateCorporateData(United.Services.Customer.Common.Corporate corporateData, MOBApplication application = null);
        Task<MOBCPCorporate> PopulateCorporateData(MOBCPProfileRequest request);
        Task MakeCorpFopServiceCall(MOBCPProfileRequest request);
        Task MakeCorpProfileServiecall(MOBCPProfileRequest request);
    }
}
