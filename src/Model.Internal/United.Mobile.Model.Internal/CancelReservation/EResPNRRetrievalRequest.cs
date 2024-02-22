using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.CancelReservation
{
    public class EResPNRRetrievalRequest : EResBaseRequest
    {   
        [Required]
        public string TravelPlan { get; set; }
        [Required]
        public string LastName { get; set; }
        public string RecepitEmailId { get; set; }
        public string PnrCreationDate { get; set; }       
    }
}
