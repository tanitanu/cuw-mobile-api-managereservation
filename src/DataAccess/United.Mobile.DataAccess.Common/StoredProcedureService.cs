using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Airport = United.Mobile.Model.Internal.Common.Airport;

namespace United.Mobile.DataAccess.Common
{
    public class StoredProcedureService : IStoredProcedureService
    {
        public async Task<string> GetAirportName(string connectionString, string airportCode)
        {
            string airportName = airportCode;
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Select_AirportName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AirportCode", SqlDbType.VarChar).Value = airportCode;
                    try
                    {
                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            airportName = Convert.ToString(reader["AirportName"]);
                        }
                    }
                    catch (Exception ex) { string msg = ex.Message; }
                }
            }
            return airportName;
        }

        public async Task<string> GetCarrierInfo(string connectionString, string carrierCode)
        {
            string carrierName = carrierCode;
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCarrierInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CarrierCode", SqlDbType.VarChar).Value = carrierCode;
                    try
                    {
                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            carrierName = Convert.ToString(reader["AirlineName"]);
                        }
                    }
                    catch (Exception ex) { string msg = ex.Message; }
                }
            }
            switch (carrierCode.ToUpper().Trim())
            {
                case "UX": return "United Express";
                case "US": return "US Airways";
                default: return carrierName;
            }
        }

        public async Task<string> GetEquipmentDescription(string connectionString, string equipmentCode)
        {
            string equipmentDescription = equipmentCode;
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("uasp_Select_EquipmentDescription", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EquipmentCode", SqlDbType.VarChar).Value = equipmentCode;
                    try
                    {
                        await conn.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            equipmentDescription = Convert.ToString(reader["EquipmentDescription"]);
                        }
                    }
                    catch (Exception ex) { string msg = ex.Message; }
                }
            }
            return equipmentDescription;
        }

        //public async Task<string> GetGMTTime(string connectionString, string localTime, string airportCode)
        //{
        //    string gmtTime = localTime;
        //    DateTime dateTime = new DateTime(0);
        //    if (DateTime.TryParse(localTime, out dateTime) && airportCode != null && airportCode.Trim().Length == 3)
        //    {
        //        long dateTime1 = 0L;
        //        long dateTime2 = 0L;
        //        long dateTime3 = 0L;
        //        using (var conn = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("sel_GMT_STD_DST_Dates", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@InputYear", SqlDbType.Int).Value = dateTime.Year;
        //                cmd.Parameters.Add("@StationCode", SqlDbType.VarChar).Value = airportCode.Trim().ToUpper();
        //                cmd.Parameters.Add("@CarrierCode", SqlDbType.VarChar).Value = "CO";
        //                try
        //                {
        //                    await conn.OpenAsync();
        //                    var reader = await cmd.ExecuteReaderAsync();
        //                    while (reader.Read())
        //                    {
        //                        dateTime1 = Convert.ToInt64(reader["DateTime_1"]);
        //                        dateTime2 = Convert.ToInt64(reader["DateTime_2"]);
        //                        dateTime3 = Convert.ToInt64(reader["DateTime_3"]);
        //                    }
        //                }
        //                catch (Exception ex) { string msg = ex.Message; }
        //            }
        //        }
        //        long time = Convert.ToInt64(dateTime.Year.ToString() + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00"));
        //        bool dayLightSavingTime = false;
        //        if (time >= dateTime2 && time <= dateTime3)
        //        {
        //            dayLightSavingTime = true;
        //        }

        //        int offsetMunite = 0;
        //        using (var conn = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("sp_GMT_City", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@StationCode", SqlDbType.VarChar).Value = airportCode.Trim().ToUpper();
        //                cmd.Parameters.Add("@Carrier", SqlDbType.VarChar).Value = "CO";
        //                try
        //                {
        //                    await conn.OpenAsync();
        //                    var reader = await cmd.ExecuteReaderAsync();
        //                    while (reader.Read())
        //                    {
        //                        if (dayLightSavingTime)
        //                        {
        //                            offsetMunite = Convert.ToInt32(reader["DaySavTime"]);
        //                        }
        //                        else
        //                        {
        //                            offsetMunite = Convert.ToInt32(reader["StandardTime"]);
        //                        }
        //                    }
        //                }
        //                catch (Exception ex) { string msg = ex.Message; }
        //            }
        //        }
        //        dateTime = dateTime.AddMinutes(-offsetMunite);
        //        gmtTime = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        //    }

        //    return gmtTime;
        //}

        //public async Task<List<DisplayBagTrackAirportDetails>> GetAirportNamesList(string connectionString, string airportCode)
        //{

        //    string airportName = airportCode;
        //    List<DisplayBagTrackAirportDetails> airPorts = new List<DisplayBagTrackAirportDetails>();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_Get_AirportNamesListFromCommaDelimitedCodes", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@AirportCode", SqlDbType.VarChar).Value = airportCode;
        //            try
        //            {
        //                await conn.OpenAsync();
        //                SqlDataReader reader = await cmd.ExecuteReaderAsync();
        //                while (reader.Read())
        //                {
        //                    if (reader["AirportCode"] != null)
        //                    {
        //                        /*airPortDetail.AirportCode = dataReader["AirportCode"].ToString();
        //                        airPortDetail.AirportCityName = dataReader["CityName"].ToString();
        //                        airPortDetail.AirportInfo = dataReader["AirportName"].ToString();*/
        //                        airPorts.Add(new DisplayBagTrackAirportDetails()
        //                        {
        //                            AirportCode = reader["AirportCode"].ToString(),
        //                            AirportCityName = reader["CityName"].ToString(),
        //                            AirportInfo = reader["AirportName"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //            catch (Exception ex) { string msg = ex.Message; }
        //        }
        //    }
        //    return airPorts;

        //}

        //public async Task<Airport> GetAirportCityName(string connectionString, string airportCode)
        //{
        //    var airPort = new Airport();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_Select_AirportName", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@AirportCode", SqlDbType.VarChar).Value = airportCode.Trim().ToUpper();
        //            try
        //            {
        //                await conn.OpenAsync();
        //                var reader = await cmd.ExecuteReaderAsync();
        //                while (reader.Read())
        //                {
        //                    airPort.AirportName = Convert.ToString(reader["AirportName"]);
        //                    airPort.CityName = Convert.ToString(reader["CityName"]);
        //                }
        //            }
        //            catch (Exception ex) { string msg = ex.Message; }
        //        }
        //    }
        //    return airPort;
        //}

        //public async Task<Tuple<bool, bool, List<string>>> VerifyWiFiAvailable(string connectionString, string shipNumbersToQuery)
        //{

        //    //"ConnectionString - DB_Flightrequest2"
        //    bool isWifiAvailable = false;
        //    var shipsWithWiFi = new List<string>();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_Get_Verify_WiFi_Available_Ships", conn))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@ships", SqlDbType.VarChar).Value = shipNumbersToQuery.Trim(',').ToString();
        //            try
        //            {
        //                await conn.OpenAsync();
        //                var reader = await cmd.ExecuteReaderAsync();
        //                while (reader.Read())
        //                {
        //                    isWifiAvailable = true;
        //                    shipsWithWiFi.Add(reader["ship"].ToString());
        //                }
        //            }
        //            catch (Exception ex) { string msg = ex.Message; }
        //        }
        //    }
        //    return new Tuple<bool, bool, List<string>>(true, isWifiAvailable, shipsWithWiFi);

        //}

        //public async Task<AirportAdvisoryMessage> GetAirportAdvisoryMessages(string connectionString, string airports, string flightDate)
        //{
        //    AirportAdvisoryMessage airportAdvisoryMessages = new AirportAdvisoryMessage();
        //    airportAdvisoryMessages.AdvisoryMessages = new List<Model.FlightStatus.TypeOption>();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("uasp_Get_Airport_Advisory_Messages", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@AIRPORTS", SqlDbType.VarChar).Value = airports;
        //            cmd.Parameters.Add("@flightDate", SqlDbType.VarChar).Value = flightDate;

        //            try
        //            {
        //                await conn.OpenAsync();
        //                var reader = await cmd.ExecuteReaderAsync();
        //                bool isFirstRow = false;
        //                while (reader.Read())
        //                {
        //                    if (!isFirstRow)
        //                    {
        //                        airportAdvisoryMessages.ButtonTitle = reader["ButtonTitle"].ToString();
        //                        airportAdvisoryMessages.HeaderTitle = reader["HeaderTitle"].ToString();
        //                        isFirstRow = true;
        //                    }
        //                    Model.FlightStatus.TypeOption typeOption = new Model.FlightStatus.TypeOption();
        //                    //typeOption.Key = (dataReader["DepAirportCode"].ToString().Trim().ToUpper() == "XXX" ? "All Airports" : dataReader["DepAirportCode"].ToString().Trim().ToUpper()) + " - " + (dataReader["ArrAirportCode"].ToString().Trim().ToUpper() == "XXX" ? "All Airports" : dataReader["ArrAirportCode"].ToString().Trim().ToUpper());
        //                    typeOption.Key = (reader["DepAirportCode"].ToString().Trim().ToUpper() == "XXX" ? reader["ArrAirportCode"].ToString().Trim().ToUpper() : reader["DepAirportCode"].ToString().Trim().ToUpper());
        //                    //**NOTE**: As discussed with Priya, The logic above is for Check In Work FLow service expecting a Airport instead of segment (like SFO - EWR or SFO - All airports or All Airport - SFO) just want a Valid Airport Code if its Departure is all airports (XXX) then return arrival airport code and Priya confirmed Clients (Iphone , Andriod etc.. ) do not use this propery (key) its only for Check In Service
        //                    typeOption.Value = reader["Message"].ToString();
        //                    airportAdvisoryMessages.AdvisoryMessages.Add(typeOption);
        //                    //airportAdvisoryMessages.AdvisoryMessages.Add(dataReader["Message"].ToString());
        //                }
        //            }
        //            catch (Exception ex) { string msg = ex.Message; }
        //        }
        //    }
        //    return airportAdvisoryMessages;
        //}

        //public async Task<int> GetCabinCountByShipNumber(string connectionString, string ship)
        //{

        //    //"ConnectionString - DB_Flightrequest"
        //    int cabinCount = 1;

        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_GetCabinCount_By_ShipNumber", conn))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@ships", SqlDbType.VarChar).Value = ship;
        //            try
        //            {
        //                await conn.OpenAsync();
        //                var reader = await cmd.ExecuteReaderAsync();
        //                while (reader.Read())
        //                {
        //                    cabinCount = Convert.ToInt32(reader["NumberOfCabins"].ToString().Trim());
        //                }
        //            }
        //            catch (Exception ex) { string msg = ex.Message; }
        //        }
        //    }

        //    return cabinCount;

        //}

        //public async Task<bool> InsertFlifoPushToken(string connectionString, FlifoPushRequest fsRequest)
        //{
        //    bool succeed = false;
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        using SqlCommand cmd = new SqlCommand("uasp_Insert_PushToken_Complications", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@PushToken", SqlDbType.VarChar).Value = fsRequest.PushToken;
        //        cmd.Parameters.Add("@DeviceID", SqlDbType.VarChar).Value = fsRequest.DeviceId;
        //        cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = fsRequest.Application.Id;
        //        cmd.Parameters.Add("@AppVersion", SqlDbType.VarChar).Value = fsRequest.Application.Version.Major;
        //        try
        //        {
        //            await conn.OpenAsync();
        //            succeed = cmd.ExecuteNonQuery() > 0;
        //        }
        //        catch (Exception ex) { string msg = ex.Message; }
        //    }
        //    return succeed;
        //}
    }
}