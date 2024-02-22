using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class AODEncryptService : IAODEncryptService
    {
        private readonly ILogger<AODEncryptService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResilientClient _resilientClient;

        public AODEncryptService([KeyFilter("AODEncryptServiceKey")] IResilientClient resilientClient,ILogger<AODEncryptService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> GetAODEncryptedString(string jsonString)
        {
            string output = string.Empty;
            var endPoint = "encrypt";
            string url = _configuration.GetSection("AODEncryptService").GetValue<string>("baseUrl") + endPoint;
            try
            {
                _logger.LogInformation("GetAODEncryptedString - {@url} {@JsonRequestUrl}", url, jsonString);
                var response = await _resilientClient.PostAsync(endPoint, jsonString);
                if (!string.IsNullOrEmpty(response))
                {
                    output = Newtonsoft.Json.Linq.JObject.Parse(response)["data"].ToString();
                    _logger.LogInformation("GetAODEncryptedString - Deserialized - {GetAESEncryptedStringResponse}", JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception - GetAODEncryptedString {@Exception}", JsonConvert.SerializeObject(ex));
            }
            return output;
        }
    }

}
