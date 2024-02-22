using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common.FeatureSettings;

namespace United.Mobile.DataAccess.Common
{
    public interface IAuroraMySqlService
    {
        Task<bool> InsertBaggageEventMessage(string pnrNumber, string bagTagNumber, Int64 bagTagUniqueKey, bool isActive, string FirstName, string LastName);
        Task<List<MOBFeatureSetting>> GetFeatureSettingsByAPIName(string apiName);
        Task<MOBFeatureSetting> InsertFeatureSettings(MOBFeatureSetting featureSetting);
        Task<List<MOBContainerIPAddressDetails>> GetContainerIPAddressesByService(string apiName);
        Task<bool> UpdateIsManuallyRefreshedToggle(string serviceName, string containerIpAdressList);
        Task<bool> InsertContainerIPAddress(MOBContainerIPAddressDetails request);
        Task<bool> DeleteContainerIPAddress(MOBContainerIPAddressDetails request);
        Task<bool> IsAllContainersRefreshed(string serviceName);
        Task<List<MOBFeatureSetting>> GetServiceContainersRefreshStatus();
        Task<bool> UpdateRefreshToggleToFalse(string serviceNameList);
    }
}
