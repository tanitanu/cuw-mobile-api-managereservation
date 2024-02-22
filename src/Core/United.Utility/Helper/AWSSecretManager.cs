using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;

namespace United.Utility.Helper
{
    public class AWSSecretManager: IAWSSecretManager
    {
        private readonly ICacheLog<AWSSecretManager> _logger;
        private readonly IConfiguration _configuration;
        public AWSSecretManager(ICacheLog<AWSSecretManager> logger,IConfiguration configuration)
        {           
            _logger = logger;    
            _configuration = configuration;
        }
        public async Task<string> GetSecretValue(string secretKey)
        {
            try
            {
                var region = Amazon.RegionEndpoint.GetBySystemName(_configuration.GetSection("AuroraDBConnectionString").GetValue<string>("SecretManager-Region"));
                AmazonSecretsManagerClient secretsManager = new AmazonSecretsManagerClient(region);
                GetSecretValueRequest request = new GetSecretValueRequest
                {
                    SecretId = secretKey,
                    VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
                };
                _logger.LogInformation("AwsSecretManager GetSecretValue {@request} ", request);
                var result = await secretsManager.GetSecretValueAsync(request);
                return result.SecretString;
            }
            catch(Exception ex)
            {
                _logger.LogError("AwsSecretManager GetSecretValueError {@message} {@stackTrace}",ex.Message, ex.StackTrace);
                
            }
            return "";
        }

   
    }
}
