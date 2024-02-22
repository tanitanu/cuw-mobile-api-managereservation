using System.Collections.Generic;

namespace United.Ebs.Logging.Enrichers
{
    public interface IEnricher
    {
        List<KeyValuePair<string, dynamic>> KeyValues { get; set; }

        void Add(string key, dynamic value);
        void Clear();
    }
}
