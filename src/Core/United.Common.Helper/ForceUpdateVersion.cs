using Microsoft.Extensions.Configuration;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Enum;
using United.Utility.Helper;

namespace United.Common.Helper
{
    public class ForceUpdateVersion
    {
        private readonly IConfiguration _configuration;
        public ForceUpdateVersion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ForceUpdateForNonSupportedVersion(int applicationId, string appVersion, FlowType flow)
        {
            var isSupportedVersion = ValidateSupportedVersion(applicationId, appVersion, flow);
            if (!isSupportedVersion)
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("NonELFVersionMessage"));
            }
        }

        private bool ValidateSupportedVersion(int applicationId, string appVersion, FlowType flow)
        {
            var androidVersionkey = GetAndroidVersionKey(flow);
            var iphoneVersionkey = GetIphoneVersionKey(flow);

           return GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, androidVersionkey, iphoneVersionkey, "WindowsNonELFVersion", "MWebNonELFVersion", true, _configuration);
        }

        public string GetAndroidVersionKey(FlowType flow)
        {
            switch (flow)
            {
                case FlowType.ALL:
                case FlowType.BOOKING:
                case FlowType.BAGGAGECALCULATOR:
                    return "AndroidELFVersion";
                case FlowType.MANAGERES:
                    return "AndroidManageResMinSupportedVersion";
                case FlowType.RESHOP:
                    return "AndroidReshopMinSupportedVersion";
                case FlowType.ERES:
                    return "AndroidERESVersion";
                default:
                    return "AndroidELFVersion";
            }
        }

        public string GetIphoneVersionKey(FlowType flow)
        {
            switch (flow)
            {
                case FlowType.ALL:
                case FlowType.BOOKING:
                case FlowType.BAGGAGECALCULATOR:
                    return "iPhoneELFVersion";
                case FlowType.MANAGERES:
                    return "iPhoneManageResMinSupportedVersion";
                case FlowType.RESHOP:
                    return "iPhoneReshopMinSupportedVersion";
                case FlowType.ERES:
                    return "iPhoneERESVersion";
                default:
                    return "iPhoneELFVersion";
            }
        }
    }
}
