using System;
using System.Net;

namespace United.Mobile.DataAccess.FlightStatus
{
    public class CustomWebResponse : WebResponse
    {
        public override long ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override System.IO.Stream GetResponseStream() { return System.IO.Stream.Null; }

        public override Uri ResponseUri { get; }

        public override System.Net.WebHeaderCollection Headers { get; }
    }
}