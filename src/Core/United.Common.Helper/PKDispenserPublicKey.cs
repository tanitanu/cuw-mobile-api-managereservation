using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Service.Presentation.SecurityResponseModel;
using United.Utility.Enum;
using United.Utility.Helper;

namespace United.Common.Helper
{
    public class PKDispenserPublicKey
    {
        private readonly IConfiguration _configuration;
        private readonly IPKDispenserService _pKDispenserService;
        private readonly ICachingService _cachingService;
        private readonly IDPService _dPService;
        private readonly IHeaders _headers;

        public PKDispenserPublicKey(
            IConfiguration configuration
            , ICachingService cachingService
            , IDPService dPService
            , IPKDispenserService pKDispenserService
            , IHeaders headers
        )
        {
            _configuration = configuration;
            _cachingService = cachingService;
            _dPService = dPService;
            _pKDispenserService = pKDispenserService;
            _headers = headers;
        }

        public async Task<string> GetCachedOrNewpkDispenserPublicKey(int appId, string appVersion, string deviceId, string transactionId, string token, List<MOBItem> catalogItems = null, string flow="")
        {
            string pkDispenserPublicKey = string.Empty;
            if (!IsSuppressPkDispenserKey(appId, appVersion, catalogItems, flow)) { 
                if (_configuration.GetValue<bool>("EnablePKDispenserKeyRotationAndOAEPPadding"))
                {
                    var key = string.Format(_configuration.GetValue<string>("PKDispenserKeyTokenKeyFormat"), _headers.ContextValues.Application.Id);
                    var pKDispenserKey = await _cachingService.GetCache<string>(key, "TID1");
                    United.Service.Presentation.SecurityResponseModel.PKDispenserKey obj = JsonConvert.DeserializeObject<United.Service.Presentation.SecurityResponseModel.PKDispenserKey>(pKDispenserKey);
                    pkDispenserPublicKey = obj == null ? null : obj.PublicKey;
                }
                else
                    pkDispenserPublicKey = await _cachingService.GetCache<string>(GetCSSPublicKeyPersistSessionStaticGUID(appId) + "pkDispenserPublicKey", "TID1");

                return string.IsNullOrEmpty(pkDispenserPublicKey)
                    ? await GetPkDispenserPublicKey(appId, deviceId, appVersion, transactionId, token)
                    : pkDispenserPublicKey;
            }
            return pkDispenserPublicKey;
    }
        private async Task<string> GetPkDispenserPublicKey(int applicationId, string deviceId, string appVersion, string transactionId, string token)
        {
            #region
            //**RSA Public Key Implementation**//
            string transId = string.IsNullOrEmpty(_headers.ContextValues.TransactionId) ? "trans0" : _headers.ContextValues.TransactionId;
            string key = string.Format(_configuration.GetValue<string>("PKDispenserKeyTokenKeyFormat"), _headers.ContextValues.Application.Id);
            var cacheResponse = await _cachingService.GetCache<string>(key, transId);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var obj = JsonConvert.DeserializeObject<PKDispenserKey>(cacheResponse);

                if (!string.IsNullOrEmpty(obj.PublicKey))
                {
                    return obj.PublicKey;
                }
            }

            var response = await _pKDispenserService.GetPkDispenserPublicKey<PKDispenserResponse>(token, _headers.ContextValues.SessionId, string.Empty);
            return GetPublicKeyFromtheResponse(response);
            #endregion
        }

        private string GetPublicKeyFromtheResponse(PKDispenserResponse response)
        {
            string pkDispenserPublicKey = string.Empty;

            if (response != null && response.Keys != null && response.Keys.Count > 0)
            {
                var obj = (from st in response.Keys
                           where st.CryptoTypeID.Trim().Equals("2")
                           select st).ToList();
                obj[0].PublicKey = obj[0].PublicKey.Replace("\r", "").Replace("\n", "").Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim();
                pkDispenserPublicKey = obj[0].PublicKey;

                string transId = string.IsNullOrEmpty(_headers.ContextValues.TransactionId) ? "trans0" : _headers.ContextValues.TransactionId;
                string key = string.Format(_configuration.GetValue<string>("PKDispenserKeyTokenKeyFormat"), _headers.ContextValues.Application.Id);
                _cachingService.SaveCache<United.Service.Presentation.SecurityResponseModel.PKDispenserKey>(key, obj[0], transId, new TimeSpan(1, 30, 0));

                string tokenKey = string.Format(_configuration.GetValue<string>("PKDispenserKeyTokenKeyFormat"), "Token::" + _headers.ContextValues.Application.Id);
                _cachingService.SaveCache<string>(tokenKey, pkDispenserPublicKey, transId, new TimeSpan(1, 30, 0));
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("Booking2OGenericExceptionMessage");
                if (!String.IsNullOrEmpty(_configuration.GetValue<string>("UnableToGetPkDispenserPublicKeyErrorMessage")))
                {
                    exceptionMessage = _configuration.GetValue<string>("UnableToGetPkDispenserPublicKeyErrorMessage");
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            return pkDispenserPublicKey;
        }

        public string GetCSSPublicKeyPersistSessionStaticGUID(int applicationId)
        {
            #region Get Aplication and Profile Ids
            string[] cSSPublicKeyPersistSessionStaticGUIDs = _configuration.GetValue<string>("CSSPublicKeyPersistSessionStaticGUID").Split('|');
            List<string> applicationDeviceTokenSessionIDList = new List<string>();
            foreach (string applicationSessionGUID in cSSPublicKeyPersistSessionStaticGUIDs)
            {
                #region
                if (Convert.ToInt32(applicationSessionGUID.Split('~')[0].ToString().ToUpper().Trim()) == applicationId)
                {
                    return applicationSessionGUID.Split('~')[1].ToString().Trim();
                }
                #endregion
            }
            return "1CSSPublicKeyPersistStatSesion4IphoneApp";
            #endregion
        }

        public bool IsSuppressPkDispenserKey(int appId, string appVersion, List<MOBItem> catalogItems = null, string flow="")
        {
            return (_configuration.GetValue<bool>("EnableSuppressPkDispenserKey") &&
                   GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("EnableSuppressPkDispenserKey_Android_Version"), _configuration.GetValue<string>("EnableSuppressPkDispenserKey_iPhone_Version"))
                   && ((catalogItems != null && catalogItems.Count > 0 &&
                   catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.DisablePKDispenserKeyFromCSL).ToString() || a.Id == ((int)AndroidCatalogEnum.DisablePKDispenserKeyFromCSL).ToString())?.CurrentValue == "1")
                   || IsViewResFlowPaymentSvcEnabled(flow)));
        }

        public  bool IsViewResFlowPaymentSvcEnabled(string flow)
        {
            return !string.IsNullOrEmpty(flow) && _configuration.GetValue<bool>("EnableViewResFlowPaymentService") &&
                    (flow == FlowType.VIEWRES.ToString() ||
                    flow == FlowType.MANAGERES.ToString() ||
                    flow == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString() ||
                    flow == FlowType.VIEWRES_SEATMAP.ToString());
        }
    }
}
