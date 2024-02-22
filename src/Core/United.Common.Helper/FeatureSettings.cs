using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;

namespace United.Common.Helper
{
    public class FeatureSettings : IFeatureSettings
    {
        private readonly IConfiguration _configuration;
        private readonly IAuroraMySqlService _auroraMySqlService;
        private readonly IDPTokenValidationService _dpTokenValidateService;
        private readonly ICachingService _cachingService;
        public FeatureSettings(IConfiguration configuration, IAuroraMySqlService auroraMySqlService, IDPTokenValidationService dpTokenValidateService, ICachingService cachingService)
        {
            _configuration = configuration;
            _auroraMySqlService = auroraMySqlService;
            _dpTokenValidateService = dpTokenValidateService;
            _cachingService = cachingService;
        }
        public async Task<bool> GetFeatureSettingValue(string key)
        {
            if(StaticDataLoader._featureSettingsList == null || !StaticDataLoader._featureSettingsList.Any())
            {
                StaticDataLoader._featureSettingsList=await GetFeatureSettingsList(StaticDataLoader._featureSettingApiname);
            }
            var featureSetting = StaticDataLoader._featureSettingsList?.FirstOrDefault(fs => fs.Key == key);
            if (featureSetting != null)
                return Convert.ToBoolean(StaticDataLoader._featureSettingsList.Find(fs => fs.Key == key).Value);
            else
                return false;
        }
        public async Task<List<MOBFeatureSetting>> GetFeatureSettingsList(string serviceName)
        {
            StaticDataLoader._featureSettingApiname = serviceName;
            List<MOBFeatureSetting> list = new List<MOBFeatureSetting>();
            try
            {
                list = await _auroraMySqlService.GetFeatureSettingsByAPIName(serviceName);
                await _cachingService.SaveCache<List<MOBFeatureSetting>>("FeatureToggles_" + serviceName, list, "FeatureToggles_" + serviceName + "_transactionId", new TimeSpan(1, 0, 0, 0)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var cacheResponse = await _cachingService.GetCache<List<MOBFeatureSetting>>("FeatureToggles_" + serviceName, "FeatureToggles_" + serviceName + "_transactionId").ConfigureAwait(false);
                list = JsonConvert.DeserializeObject<List<MOBFeatureSetting>>(cacheResponse);
            }
            return list;
        }
        public async Task<MOBGetContainerIPAddressesByServiceResponse> RefreshAllContainerFeatureSettingsCache(string serviceName, string baseServiceName, MOBFeatureSettingsCacheRequest request)
        {
            MOBGetContainerIPAddressesByServiceResponse response = new MOBGetContainerIPAddressesByServiceResponse();
            List<MOBContainerIPAddressDetails> containerIPAddressDetails = new List<MOBContainerIPAddressDetails>();
            try
            {
                if(!(await _dpTokenValidateService.isActiveToken(request.Token)))
                {
                    throw new MOBUnitedException("Please enter the valid token");
                }
                if (request.IpAddressList != null)
                {
                    request.IpAddressList.Split(',').ToList().ForEach(ip =>
                    {
                        containerIPAddressDetails.Add(new MOBContainerIPAddressDetails
                        {
                            IpAddress = ip
                        });
                    });
                }
                else
                {
                    containerIPAddressDetails = await _auroraMySqlService.GetContainerIPAddressesByService(serviceName);
                }

                string refreshCacheUrl = string.Empty;
                MOBResponse refreshCacheResponse = new MOBResponse();
                foreach (var containerIpAddress in containerIPAddressDetails)
                {
                    try
                    {
                        var refreshContainerRequest = new MOBFeatureSettingsCacheRequest
                        {
                            ServiceName= serviceName,
                            Token=request.Token
                        };
                        var requestData = JsonConvert.SerializeObject(refreshContainerRequest);
                        refreshCacheUrl = $"http://{containerIpAddress.IpAddress}:80/{baseServiceName}/api/RefreshFeatureSettingCache";
                        var responseData = StaticDataLoader.Post(refreshCacheUrl,"", requestData, "application/json; charset=utf-8");
                        refreshCacheResponse = JsonConvert.DeserializeObject<MOBResponse>(responseData);
                        containerIpAddress.Status = true;
                        if (refreshCacheResponse.Exception != null)
                        {
                            containerIpAddress.Status = false;
                            containerIpAddress.Exception = refreshCacheResponse.Exception.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        containerIpAddress.Status = false;
                        containerIpAddress.Exception = JsonConvert.SerializeObject(ex);
                    }

                }
                response.IpAddresses = containerIPAddressDetails;
                if (response.IpAddresses.Any(ip => ip.Status == true))
                {
                    await _auroraMySqlService.UpdateIsManuallyRefreshedToggle(serviceName,string.Join(",", response.IpAddresses.Where(ip => ip.Status == true).Select(ip => ip.IpAddress)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public async Task InsertContainerIPAdress(string serviceName)
        {
            try
            {
                if (!StaticDataLoader._ipAddress.Contains("-"))
                {
                    return;
                }
                var ContainerIPAddressDetails = new MOBContainerIPAddressDetails
                {
                    IpAddress = StaticDataLoader._ipAddress,
                    ServiceName = serviceName
                };
               await _auroraMySqlService.InsertContainerIPAddress(ContainerIPAddressDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteContainerIPAdress(string serviceName,string ipAddress)
        {
            try
            {     
                var ContainerIPAddressDetails = new MOBContainerIPAddressDetails
                {
                    IpAddress = ipAddress,
                    ServiceName = serviceName
                };
               await _auroraMySqlService.DeleteContainerIPAddress(ContainerIPAddressDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task RefreshFeatureSettingCache(MOBFeatureSettingsCacheRequest request)
        {
            try
            {
                if (!(await _dpTokenValidateService.isActiveToken(request.Token)))
                {
                    throw new MOBUnitedException("Please enter the valid token");
                }
                StaticDataLoader._featureSettingsList =await GetFeatureSettingsList(request.ServiceName).ConfigureAwait(false);
                await _auroraMySqlService.UpdateIsManuallyRefreshedToggle(request.ServiceName.ToUpper().ToString(), StaticDataLoader._ipAddress).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
    
        public async Task<MOBGetAllContainerFeatureSettingsResponse> GetAllContainerFeatureSettings(MOBFeatureSettingsCacheRequest request, string servicename,String baseServiceName)
        {
            MOBGetAllContainerFeatureSettingsResponse response = new MOBGetAllContainerFeatureSettingsResponse();
            List<MOBContainerIPAddressDetails> containerIPAddressDetails = new List<MOBContainerIPAddressDetails>();
            try
            {
                if (!(await _dpTokenValidateService.isActiveToken(request.Token)))
                {
                    throw new MOBUnitedException("Please enter the valid token");
                }
                if (request.IpAddressList != null)
                {
                    request.IpAddressList.Split(',').ToList().ForEach(ip =>
                    {
                        containerIPAddressDetails.Add(new MOBContainerIPAddressDetails
                        {
                            IpAddress = ip
                        });
                    });
                }
                else
                {
                    containerIPAddressDetails = await _auroraMySqlService.GetContainerIPAddressesByService(servicename);
                }

                string url = string.Empty;
                GetFeatureSettingsResponse featureSettingsResponse = new GetFeatureSettingsResponse();
                foreach (var containerIpAddress in containerIPAddressDetails)
                {
                    if(response.ContainerFeatureSettings==null)
                    {
                        response.ContainerFeatureSettings = new List<MOBContainerFeatureSettings>();
                    }
                    MOBContainerFeatureSettings containerFeatureSettings = new MOBContainerFeatureSettings();
                    try
                    {
                                               
                        url = $"http://{containerIpAddress.IpAddress}:80/{baseServiceName}/api/GetFeatureSettings";
                        var serviceResponse = StaticDataLoader.Get(url, "application/json; charset=utf-8", "");
                        featureSettingsResponse = JsonConvert.DeserializeObject<GetFeatureSettingsResponse>(serviceResponse);
                        
                        if (featureSettingsResponse.Exception != null)
                        {
                            containerFeatureSettings.Exception = featureSettingsResponse.Exception.Message;
                        }
                        else
                        {
                            containerFeatureSettings.IpAddress = containerIpAddress.IpAddress;
                            containerFeatureSettings.FeatureSettings = new List<MOBFeatureSetting>();
                            containerFeatureSettings.FeatureSettings = featureSettingsResponse.FeatureSettings;
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        containerIpAddress.Status = false;
                        containerIpAddress.Exception = JsonConvert.SerializeObject(ex);
                    }
                    response.ContainerFeatureSettings.Add(containerFeatureSettings);
                }            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public async Task  LoadFeatureSettings(string apiName)
        {
            StaticDataLoader._featureSettingsList =await GetFeatureSettingsList(apiName);
            await InsertContainerIPAdress(apiName);
        }

        public GetFeatureSettingsResponse GetFeatureSettings()
        {
            GetFeatureSettingsResponse response = new GetFeatureSettingsResponse();
            try
            {
                response.FeatureSettings = new List<MOBFeatureSetting>();
                response.FeatureSettings = StaticDataLoader._featureSettingsList;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
