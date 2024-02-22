using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    public class Base
    {
        // Summary:
        //     Service Name    
        public Constant.ServiceName ServiceName { get; set; }
        //
        // Summary:
        //     The external service start and end date time     
        public List<ResponseTime> ResponseTimes { get; set; }
        //CustomerProfile TODO:
        // Summary:
        //     HTTP Status code from the service.

        public HttpStatusCode? StatusCode { get; set; }
    }
}
