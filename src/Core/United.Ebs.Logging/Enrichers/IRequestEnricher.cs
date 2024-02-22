
using System.Collections.Generic;

namespace United.Ebs.Logging.Enrichers
{
    public interface IRequestEnricher : IEnricher
    {
        void ProcessRequest(dynamic requestBody);
        void ProcessResponse(dynamic responseBody, double responseTime);

        List<KeyValuePair<string, dynamic>> RequestInfo { get; set; }
        List<KeyValuePair<string, dynamic>> ResponseInfo { get; set; }

    }
}
