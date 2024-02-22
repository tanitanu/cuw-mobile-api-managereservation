using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AddTravellerRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
        
        public string Pax1Id { get; set; } = string.Empty;
       
        public string Pax2Id { get; set; } = string.Empty;
       
        public string Pax3Id { get; set; } = string.Empty;
        
        public string Pax4Id { get; set; } = string.Empty;
       
        public string Pax5Id { get; set; } = string.Empty;
        
        public string Pax6Id { get; set; } = string.Empty;
       
        public string Pax7Id { get; set; } = string.Empty;
      
        public string Pax8Id { get; set; } = string.Empty;
       
        public string Pax1Type { get; set; } = string.Empty;
        
        public string Pax2Type { get; set; } = string.Empty;
      
        public string Pax3Type { get; set; } = string.Empty;
      
        public string Pax4Type { get; set; } = string.Empty;
        
        public string Pax5Type { get; set; } = string.Empty;
        
        public string Pax6Type { get; set; } = string.Empty;
       
        public string Pax7Type { get; set; } = string.Empty;
       
        public string Pax8Type { get; set; } = string.Empty;
        
        public string MPAccountNumber { get; set; } = string.Empty;
       
    }
}
