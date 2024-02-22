using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Services.SSO;
using United.Utility.Helper;

namespace United.Common.Helper
{
    public class SSOTokenKeyHelper : ISSOTokenKeyHelper
    {
        private readonly ICacheLog<SSOTokenKeyHelper> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISSOTokenKeyService _iSSOTokenKeyService;
        private readonly IDPService _tokenService;
        private readonly IHeaders _headers;

        public SSOTokenKeyHelper(ICacheLog<SSOTokenKeyHelper> logger
            , IConfiguration configuraiton
           , IDPService tokenService
            , ISSOTokenKeyService iSSOTokenKeyService
            , IHeaders headers)
        {
            _logger = logger;
            _configuration = configuraiton;
            _tokenService = tokenService;
            _iSSOTokenKeyService = iSSOTokenKeyService;
            _headers = headers;
        }

        public async Task<string> DecryptData(string encryptedData, MOBRequest mobRequest, string token)
        {
            var decryptedData = string.Empty;
            if (!string.IsNullOrEmpty(encryptedData))
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await _tokenService.GetAnonymousToken(mobRequest.Application.Id, mobRequest.DeviceId, _configuration);
                }
                var jsonResponse = await _iSSOTokenKeyService.DecryptToken(token, _headers.ContextValues.SessionId, encryptedData);
                var response = DataContextJsonSerializer.Deserialize<PartnerSSOToken>(jsonResponse);
                decryptedData = response?.SSOToken?.MemberId;

                if (string.IsNullOrWhiteSpace(decryptedData))
                {
                    _logger.LogInformation("DecryptionData CSLPartnerTokenResponse {JSONResponseStatusDescription}", response?.SSOToken?.StatusDescription);
                }
            }

            return decryptedData;
        }

        public async Task<string> EncryptData(string plainText, string token)
        {
            var encryptedData = string.Empty;
            if (!string.IsNullOrWhiteSpace(plainText))
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var jsonResponse = await _iSSOTokenKeyService.EncryptKey(token, _headers.ContextValues.SessionId, plainText);
                    var response = DataContextJsonSerializer.Deserialize<PartnerSSOTokenDecode>(jsonResponse);
                    encryptedData = response?.EncryptedToken?.SSOToken;
                }
            }

            return encryptedData;
        }

    }
}
