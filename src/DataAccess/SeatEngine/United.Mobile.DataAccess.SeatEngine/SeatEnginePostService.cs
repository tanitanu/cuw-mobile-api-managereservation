using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.FligtStatus.Internal;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.SeatEngine
{
    public class SeatEnginePostService : ISeatEnginePostService
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<SeatEnginePostService> _logger;

        public SeatEnginePostService(IConfiguration configuration
            , ICacheLog<SeatEnginePostService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        /* DeadCode Removed

        public Task<string> SeatEnginePostNew(string transactionId, string url, string contentType, string token, string requestData)
        {
            #region ShuffleVIPSBasedOnCSS_r_DPTOken
            //if(ShuffleVIPSBasedOnCSS_r_DPTOken)// need to add a ShuffleVIPSBasedOnCSS_r_DPTOken
            //{
            //EnableDPToken
            // Check if the "token" is a DPToken or a CSS Token
            // if ("EnableDPToken" == true  && CSSTOken && ShuffleVIPSBasedOnCSS_r_DPTOken == true) then need to replace the VIP "csmc.qa.api.united.com" with "unitedservicesqa.ual.com" and continue with CSS Token
            // if ("EnableDPToken" == false && DPToken && ShuffleVIPSBasedOnCSS_r_DPTOken == true) then need to replace the VIP "unitedservicesqa.ual.com" with "csmc.qa.api.united.com" and continue with DPToken
            //}
            url = IsTokenMiddleOfFlowDPDeployment() ? ModifyVIPMiddleOfFlowDPDeployment(token, url) : url;

            _logger.LogInformation("CSL - SeatEnginePostNew {transactionID}, {url}, {token} and {requestdata}", transactionId, url, token, JsonConvert.SerializeObject(requestData));

            #endregion
            string responseData = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(requestData))
                {
                    throw new MOBUnitedException("There is no data to post.");
                }

                Uri uri = new Uri(url);
                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json;";
                httpWebRequest.Headers.Add("Authorization", token);
                httpWebRequest.Timeout = 180000;

                // Create a byte array of the request data we want to send  
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(requestData);

                // Set the content length in the request headers  
                httpWebRequest.ContentLength = byteData.Length;

                // Write data  
                using (Stream postStream = httpWebRequest.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // Get response  
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    // Console application output  
                    responseData = reader.ReadToEnd();

                    _logger.LogInformation("CSL - SeatEnginePostNew Response {Response} and {sessionID}", JsonConvert.SerializeObject(responseData), transactionId);

                }
            }
            catch (WebException ex)
            {
                var exReader = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string seatMapUnavailable = string.Empty;
                if (!string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapUnavailable-MinorDescription")))
                {
                    seatMapUnavailable = _configuration.GetValue<string>("SeatMapUnavailable-MinorDescription");
                    string[] seatMapUnavailableMinorDescription = seatMapUnavailable.Split('|');

                    if (!string.IsNullOrEmpty(exReader))
                    {
                        var exceptionDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBFlightStatusError>(exReader);
                        if (exceptionDetails.Errors != null)
                            if (!string.IsNullOrEmpty(exceptionDetails.Errors[0].MinorDescription))
                                //if (exceptionDetails.Errors[0].MinorDescription.Contains("SEAT DISPLAY NOT AVAILABLE FOR DATE") || exceptionDetails.Errors[0].MinorDescription.Contains("UNABLE TO DISPLAY INTERLINE SEAT MAP"))

                                foreach (string minorDescription in seatMapUnavailableMinorDescription)
                                    if (exceptionDetails.Errors[0].MinorDescription.Contains(minorDescription))
                                        throw new MOBUnitedException(_configuration.GetValue<string>("OASeatMapUnavailableMessage"));
                    }
                }
                else
                {
                    throw ex;
                }
            }
            return Task.FromResult(responseData);
        }

        public bool IsTokenMiddleOfFlowDPDeployment()
        {
            return (_configuration.GetValue<bool>("ShuffleVIPSBasedOnCSS_r_DPTOken") && _configuration.GetValue<bool>("EnableDpToken")) ? true : false;

        }

        public string ModifyVIPMiddleOfFlowDPDeployment(string token, string url)
        {
            url = token.Length < 50 ? url.Replace(_configuration.GetValue<string>("DPVIPforDeployment"), _configuration.GetValue<string>("CSSVIPforDeployment")) : url;
            return url;
        }
        
        */
    }
}
