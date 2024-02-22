using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPRewards
{
    public class SDLResult
    {
        public string id { get; set; }
        public object Description { get; set; }
        public object Category { get; set; }
        public object GroupName { get; set; }
        public object Image { get; set; }
        public object ItemKey { get; set; }
        public string ComponentTitle { get; set; }
        public string ComponentSchema { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SDLProduct[] Products { get; set; }
    }
}
