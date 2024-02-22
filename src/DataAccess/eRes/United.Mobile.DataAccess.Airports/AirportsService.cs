using Autofac.Features.AttributeFilters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Airports;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Airports
{
    public class AirportsService :IAirportsService
    {     
        private readonly IResilientClient _aiportsResilientClient;
        public AirportsService([KeyFilter("airportsClientKey")] IResilientClient aiportsResilientClient)
        {
            _aiportsResilientClient = aiportsResilientClient;           
        }
        public async Task<AirportsResponse> GetAirports(string token)
        {          

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var response = await _aiportsResilientClient.GetAsync(string.Empty, headers).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<AirportsResponse>(response);
        }
    }
}
