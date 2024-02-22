using System.Collections.Generic;

namespace United.Ebs.Logging.Enrichers
{
    // Added by Ashrith
    public class ContextEnricher : IContextEnricher
    {
        private List<KeyValuePair<string, dynamic>> KeyValues { get; set; }

        public ContextEnricher()
        {
            KeyValues = new List<KeyValuePair<string, dynamic>>();
        }

        public void Add(string key, dynamic value)
        {
            if (key == null || value == null)
                return;

            KeyValues.Add(new KeyValuePair<string, dynamic>(key, value));

            LogEventEnricher.contextEnricherKeyValues = KeyValues;
        }
    }
}
