using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Base
    {
        // Summary:
        //     Service Name    
        public Constants.ServiceName ServiceName { get; set; }
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
