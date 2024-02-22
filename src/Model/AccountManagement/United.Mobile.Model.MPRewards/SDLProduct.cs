using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class SDLProduct
    {
        public string id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ComponentTitle { get; set; }
        public string ComponentSchema { get; set; }
        public string Id { get; set; }
        public object View { get; set; }
        public object Controller { get; set; }
        public object Action { get; set; }
        public object Region { get; set; }
        public object BlockSize { get; set; }
        public string HeaderTag { get; set; }
        public bool IncrementHeaderTag { get; set; }
        public object FormAction { get; set; }
        public string ActionEndpoint { get; set; }
        public object ItemWeight { get; set; }
        public object Location { get; set; }
        public string OfferTile { get; set; }
        public string ConfigDetails { get; set; }

    }
}
