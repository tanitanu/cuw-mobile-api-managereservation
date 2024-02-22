using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Utility.Helper;

namespace United.Utility.Filters
{
    public class ForceUpdateFilter : ActionFilterAttribute
    {
        private readonly ILogger<ForceUpdateFilter> _logger;
        private readonly IConfiguration _configuration;
        public ForceUpdateFilter(ILogger<ForceUpdateFilter> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
        bool IsForceUpdateCheckRequired = _configuration.GetValue<bool>("isForceUpdateCheckRequiredAlertCheckFSR");
            if (IsForceUpdateCheckRequired)
            {
                
                string AppMajorVersion = context.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString();
                int AppID = Convert.ToInt32(context.HttpContext.Request.Headers[Constants.HeaderAppIdText]);
                bool isKTNCleanupSupportedAppVersion = TopHelper.IsApplicationVersionGreaterorEqual(AppMajorVersion, (AppID == 1) ?
                    _configuration.GetValue<string>("eRes_AddAdditionalFields_Supported_AppVersion_iPhone") :
                    _configuration.GetValue<string>("eRes_AddAdditionalFields_Supported_AppVersion_Android"));

                if (!isKTNCleanupSupportedAppVersion)
                {
                    _logger.LogInformation("Device {DeviceId} as been triggered for forceupdate", context.HttpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString());

                    var statusCode = (int)HttpStatusCode.BadRequest;
                    string forceUpdateMessage = _configuration.GetValue<string>("forceUpdateMessageAlertCheckFSR");

                    var response = new Response<bool?>();
                    response.Data = null; 
                    response.Errors.Add(statusCode.ToString(), new List<string> { forceUpdateMessage });
                    response.Status = statusCode;
                    response.Title = "Force Update";

                    context.HttpContext.Response.StatusCode = statusCode;
                    context.HttpContext.Response.ContentType = Constants.ContentTypeJsonText;

                    var formater = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response,formater));

                    return;
                }
            }
                
            await next();
        }
    }
}
