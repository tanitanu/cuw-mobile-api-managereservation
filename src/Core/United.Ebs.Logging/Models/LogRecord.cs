using Newtonsoft.Json;
using System.Collections.Generic;

namespace United.Ebs.Logging.Models
{
    public class LogRecord
    {
        private readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //ContractResolver = SkipEmptyContractResolver.Instance,
        };

        private readonly Dictionary<string, dynamic> items = new Dictionary<string, dynamic>();

        public void Clear()
        {
            items.Clear();
        }
        public void Add(string key, dynamic value)
        {
            if(value != null && key != null)
            {
                items.Add(key, value);
            }
        }
        public void Add(List<KeyValuePair<string, dynamic>> attribs)
        {
            if (attribs == null)
                return;

            foreach(var item in attribs)
            {
                if (item.Value == null)
                    continue;

                items.Add(item.Key, item.Value);
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(items, JsonSettings);
        }
    }
}
