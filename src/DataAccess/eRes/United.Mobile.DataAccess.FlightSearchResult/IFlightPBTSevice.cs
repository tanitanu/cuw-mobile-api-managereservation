using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.FlightSearchResult;

namespace United.Mobile.DataAccess.FlightSearchResult
{
   public  interface IFlightPBTSevice
   {
        Task<string> GetFlightPBT(string token, EResFlightPBTRequest request);
    }
}
