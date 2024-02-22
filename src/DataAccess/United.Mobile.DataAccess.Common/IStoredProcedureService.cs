using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airport = United.Mobile.Model.Internal.Common.Airport;

namespace United.Mobile.DataAccess.Common
{

    public interface IStoredProcedureService
    {
        Task<string> GetAirportName(string connectionString, string airportCode);
        //Task<string> GetGMTTime(string connectionString, string localTime, string airportCode);
        Task<string> GetCarrierInfo(string connectionString, string carrierCode);
        Task<string> GetEquipmentDescription(string connectionString, string equipmentCode);
        /*Task<List<DisplayBagTrackAirportDetails>> GetAirportNamesList(string connectionString, string airportCode);
        Task<Airport> GetAirportCityName(string connectionString, string airportCode);
        Task<Tuple<bool, bool, List<string>>> VerifyWiFiAvailable(string connectionString, string shipNumbersToQuery);
        Task<AirportAdvisoryMessage> GetAirportAdvisoryMessages(string connectionString, string airports, string flightDate);
        Task<int> GetCabinCountByShipNumber(string connectionString, string ship);
        Task<bool> InsertFlifoPushToken(string connectionString, FlifoPushRequest fsRequest);*/
    }

}
