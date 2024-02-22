using System.Collections.Generic;

namespace United.Ebs.Logging.Enrichers
{
    public abstract class Enricher : IEnricher
    {
        protected Enricher()
        {
            KeyValues = new List<KeyValuePair<string, dynamic>>();
        }

        public List<KeyValuePair<string, dynamic>> KeyValues { get; set; }

        public void Add(string key, dynamic value)
        {
            if (key == null || value == null)
                return;            

            var index = KeyValues.FindIndex(x => string.Equals(x.Key, key, System.StringComparison.OrdinalIgnoreCase));
            if (index!=-1)
            {
                KeyValues.RemoveAt(index);                               
            }
            KeyValues.Add(new KeyValuePair<string, dynamic>(key, value));
        }

        public void Clear()
        {
            if (KeyValues != null && KeyValues.Count > 0)
            {
                KeyValues.Clear();
            }
        }
    }
}
