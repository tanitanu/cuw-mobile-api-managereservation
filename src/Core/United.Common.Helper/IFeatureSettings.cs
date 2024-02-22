using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;

namespace United.Common.Helper
{
    public interface IFeatureSettings
    {
        Task<bool> GetFeatureSettingValue(string key);
        Task<MOBGetContainerIPAddressesByServiceResponse> RefreshAllContainerFeatureSettingsCache(string serviceName, string baseServiceName, MOBFeatureSettingsCacheRequest request);
        Task RefreshFeatureSettingCache(MOBFeatureSettingsCacheRequest request);
        Task<MOBGetAllContainerFeatureSettingsResponse> GetAllContainerFeatureSettings(MOBFeatureSettingsCacheRequest request, string servicename, String baseServiceName);
        Task<List<MOBFeatureSetting>> GetFeatureSettingsList(string serviceName);
        Task InsertContainerIPAdress(string serviceName);
        Task LoadFeatureSettings(string apiName);
        Task DeleteContainerIPAdress(string serviceName, string ipAddress);
        GetFeatureSettingsResponse GetFeatureSettings();

    }
}
