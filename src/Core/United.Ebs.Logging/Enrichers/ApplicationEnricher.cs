using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace United.Ebs.Logging.Enrichers
{
    public class ApplicationEnricher : Enricher, IApplicationEnricher
    {
        public ApplicationEnricher(IHttpContextAccessor accessor)
        {
            LogEventEnricher.ContextAccessor = accessor;
            LoggerExtensions.ContextAccessor = accessor;
        }

        // Added by Ashrith
        public ApplicationEnricher(Dictionary<string, dynamic> keyValues)
        {
            var keyValuePairs = new List<KeyValuePair<string, dynamic>>();

            foreach (var keyvalue in keyValues)
            {
                if (keyvalue.Key != null || keyvalue.Value != null || !string.IsNullOrWhiteSpace(keyvalue.Key))
                    keyValuePairs.Add(new KeyValuePair<string, dynamic>(keyvalue.Key, keyvalue.Value));
            }

            LogEventEnricher.appEnricherKeyValues = keyValuePairs;
        }
    }
}
