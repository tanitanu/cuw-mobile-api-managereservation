using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper
{
    public static class StaticDataLoader
    {
        private static List<MOBDisplayBagTrackAirportDetails> _airportsList = null;
        public static List<MOBFeatureSetting> _featureSettingsList = null;
        public static string _featureSettingApiname = string.Empty;
        public static string _ipAddress = System.Environment.MachineName;

        public static void LoadAirports(IConfiguration Configuration)
        {
            var url = Configuration.GetSection("cachingConfig").GetValue<string>("baseUrl");

            _airportsList = GetAirportNamesList(url);
        }
        
        public static List<MOBDisplayBagTrackAirportDetails> GetAirports()
        {
            return _airportsList;
        }

        public static void SetAirports(List<MOBDisplayBagTrackAirportDetails> airportList)
        {
            _airportsList = airportList;
        }

        public static void FlusAirports(IConfiguration Configuration)
        {
            var url = Configuration.GetSection("cachingConfig").GetValue<string>("baseUrl");
            _airportsList = GetAirportNamesList(url);
        }
       
        internal static List<MOBDisplayBagTrackAirportDetails> GetAirportNamesList(string url)
        {
            var airPorts = new List<MOBDisplayBagTrackAirportDetails>();

            CacheRequest cachingData = new CacheRequest()
            {
                Key = "utb_Airport",
                TransactionId = "utb_Airport"
            };
            string requestData = JsonConvert.SerializeObject(cachingData);

            var airportListFromcache = Post(String.Format("{0}Get", url), "", requestData);
            var airportList = JsonConvert.DeserializeObject<List<AirportList>>(airportListFromcache);
            foreach (AirportList al in airportList)
            {
                airPorts.Add(new MOBDisplayBagTrackAirportDetails()
                {
                    AirportCode = al.AirportCode,
                    CityName = al.Cityname,
                    AirportNameMobile = al.AirportName
                });
            }

            return airPorts;
        }

        internal static string Post(string url, string token, string requestData,  string contentType= "application/json; charset=utf-8",   int? timeout=10000, int? retry=1)
        {
            string responseData = string.Empty;

            if (string.IsNullOrEmpty(requestData))
            {
                throw new Exception("There is no data to post.");
            }

            if (!string.IsNullOrEmpty(url) && url.ToLower().Contains("https://"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; 
            }

            Uri uri = new Uri(url);
            HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;

            if (!string.IsNullOrEmpty(url) && url.ToLower().Contains("https://"))
            {
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }

            //httpWebRequest.ServicePoint.ConnectionLimit = 1;
            if (string.IsNullOrEmpty(contentType))
            {
                httpWebRequest.ContentType = "application/json; charset=utf-8";
            }
            else
            {
                httpWebRequest.ContentType = contentType;
            }
            if (!string.IsNullOrEmpty(token))
            {
                httpWebRequest.Headers.Add("Authorization", token);
            }
            
            if (timeout.HasValue)
            {
                httpWebRequest.Timeout = timeout.Value;
            }
            else
            {
                httpWebRequest.Timeout = System.Configuration.ConfigurationManager.AppSettings["HttpPostTimeout"] != null ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HttpPostTimeout"].ToString()) : 180000;
            }
            // Create a byte array of the request data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(requestData);

            // Set the content length in the request headers  
            httpWebRequest.ContentLength = byteData.Length;

            int numberOfRetry = 1;
            if (retry.HasValue)
            {
                numberOfRetry = retry.Value;
            }

            try
            {
                // Write data  
                using (Stream postStream = httpWebRequest.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // Get response  

                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    StringBuilder sb = new StringBuilder();
                    byte[] buffer = new byte[8192];
                    Stream stream = response.GetResponseStream();
                    string tempString = null;
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, buffer.Length);
                        if (count != 0)
                        {
                            tempString = UTF8Encoding.UTF8.GetString(buffer, 0, count);
                            sb.Append(tempString);
                        }
                    }
                    while (count > 0);

                    responseData = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                if (numberOfRetry == 1)
                {
                    throw ex;
                }
                else
                {
                    responseData = Post(url, contentType, token, requestData, timeout, numberOfRetry - 1);
                }
            }

            return responseData;
        }

        internal static string Get(string url, string contentType, string token, int? timeout = 10000, int? retry = 1)
        {
            //EnableDPToken   
            string responseData = string.Empty;

            if (!string.IsNullOrEmpty(url) && url.ToLower().Contains("https://"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = contentType;
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            else
            {
                request.Timeout = System.Configuration.ConfigurationManager.AppSettings["HttpGetTimeout"] != null ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HttpGetTimeout"].ToString()) : 180000;
            }
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", token);
            }

            int numberOfRetry = 1;
            if (retry.HasValue)
            {
                numberOfRetry = retry.Value;
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                if (numberOfRetry == 1)
                {
                    throw ex;
                }
                else
                {
                    responseData = Get(url, contentType, token, timeout.Value, numberOfRetry - 1);
                }
            }

            return responseData;
        }

      
    }
}
