using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace United.Common.Helper.ManageRes
{
    public class SyncGatewayRESTClient
    {
        string adminUrl;
        string bucket;

        public SyncGatewayRESTClient(string adminUrl, string bucket)
        {
            if (adminUrl.EndsWith("/") == false)
            {
                this.adminUrl = adminUrl + "/";
            }
            else
            {
                this.adminUrl = adminUrl;
            }
            this.bucket = bucket;
        }
        private string GetDocumentUsingSyncGateway(string Id, string doc)
        {
            try
            {
                var request = WebRequest.CreateHttp($"{adminUrl}{bucket}/{Id}?show_exp=true");
                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";
                using (var response = request.GetResponseAsync())
                {
                    //EnsureResponse(response, new int[] { 200 });
                    //using (var responseStream = response.GetResponseStream())
                    //{
                    //    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    //    {
                    //        doc = reader.ReadToEnd();
                    //    }
                    //}
                }
            }

            catch (Exception)
            {
                throw;
            }

            return doc;
        }

        //private void EnsureResponse(WebResponse response, int[] responseCodes)
        //{
        //    if (!responseCodes.Any(x => x == (int)(response as HttpWebResponse).StatusCode))
        //    {
        //        throw new Exception($"Unexpected response {(response as HttpWebResponse).StatusCode}");
        //    }
        //}

        public T GetDocument<T>(string loggingContext, string Id)
        {
            T t = default(T);
            string doc = string.Empty;

            t = JsonConvert.DeserializeObject<T>(GetDocumentUsingSyncGateway(Id, doc), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return t;
        }
    }
}
