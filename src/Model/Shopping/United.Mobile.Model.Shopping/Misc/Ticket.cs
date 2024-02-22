using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Ticket
    {
        public string Number { get; set; } = string.Empty;
       
        public int Sequence { get; set; } 

        public string Status { get; set; } = string.Empty;
       
        public string IssuedTo { get; set; } = string.Empty;
       
        public string IssuedBy { get; set; } = string.Empty;
       
        public string Origin { get; set; } = string.Empty;
       
        public string Destination { get; set; } = string.Empty;
       
        public string FlightDate { get; set; } = string.Empty;
     
        public bool IsActive { get; set; } 
        public bool IsBulkTicket { get; set; } 

        public bool IsGiftTicket { get; set; } 
        public int GiftSequence { get; set; } 
        public string FareBasis { get; set; } = string.Empty;
        
        public decimal TotalFare { get; set; } 

        public string CurrencyOfIssuance { get; set; } = string.Empty;
       

    }
}
