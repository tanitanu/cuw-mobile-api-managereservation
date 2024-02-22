using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class AirportDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private string tableName = string.Empty;

        public AirportDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("utb_Airport");
            if (string.IsNullOrEmpty(tableName))
                tableName = "utb_Airport";
        }


        public async Task<string> GetAirportName(string airportCode, string sessionId)
        {
            var response = await _dynamoDBService.GetRecords<DisplayAirportDetails>(tableName, "Airport-" + airportCode.ToUpper(), airportCode.ToUpper(), sessionId);
            return string.IsNullOrEmpty(response?.AirportNameMobile) ? airportCode : response?.AirportNameMobile;
        }

        public async Task<string> GetCarrierInfo(string carrierCode, string sessionId)
        {
            string carrierName = carrierCode;
            if (carrierCode.Trim() != "")
            {
                try
                {
                    tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("CarrierInfo");
                    if (string.IsNullOrEmpty(tableName))
                        tableName = "cuw-carrierinfo";
                    var response = await _dynamoDBService.GetRecords<DisplayCarrierDetails>(tableName, "Carrier-" + carrierCode.ToUpper(), carrierCode.ToUpper(), sessionId).ConfigureAwait(false);
                    carrierName = response != null ? response?.AirlineName : carrierName;
                }
                catch { }
            }            
            switch (carrierCode.ToUpper().Trim())
            {
                case "UX": return "United Express";
                case "US": return "US Airways";
                default:
                    return carrierName;
            }
        }

        public async Task<(bool returnvalue,  string airportName, string cityName)> GetAirportCityName(string airportCode, string sessionId, string airportName, string cityName)
        {
            #region
            //    Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //    DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Select_AirportName");
            //    database.AddInParameter(dbCommand, "@AirportCode", DbType.String, airportCode);
            //            USE[iPhone]
            //GO
            ///****** Object: StoredProcedure [dbo].[usp_Select_AirportName] Script Date: 08-11-2021 21:10:00 ******/
            //SET ANSI_NULLS OFF
            //GO
            //SET QUOTED_IDENTIFIER OFF
            //GO



            //ALTER PROCEDURE[dbo].[usp_Select_AirportName]

            //@AirportCode CHAR(3)



            //AS



            //SELECT AirportNameMobile as AirportName, CityName,AirportNameMobile
            //FROM dbo.utb_Airport NOLOCK
            //WHERE AirportCode = @AirportCode
            #endregion
            var airportDetails = await _dynamoDBService.GetRecords<DisplayAirportDetails>(_configuration?.GetValue<string>("DynamoDBTables:utb_Airport"), "airportCode01", airportCode, sessionId);
            if (airportDetails == null)
            {
                return (false,default,default);
            }

            airportName = airportDetails?.AirportNameMobile;
            airportCode = airportDetails?.AirportCode;
            cityName = airportDetails?.CityName;

            return (true,airportName,cityName);
        }


        public string GetAirportCityName(string airportCode, ref string airportName, ref string cityName, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Select_AirportName");
            //database.AddInParameter(dbCommand, "@AirportCode", DbType.String, airportCode);

            //using (IDataReader dataReader = database.ExecuteReader(dbCommand))
            //{
            //    while (dataReader.Read())
            //    {
            //        airportName = dataReader["AirportName"].ToString();
            //        cityName = dataReader["CityName"].ToString();
            //    }
            //}
            #endregion
            try
            {
                var response = _dynamoDBService.GetRecords<DisplayAirportDetails>(tableName, "Airport-" + airportCode.ToUpper(), airportCode.ToUpper(), sessionId).Result;
                if (response != null)
                {
                    airportName = response.AirportNameMobile;
                    cityName = response.CityName;
                }
            }
            catch
            {

            }

            return default;
        }
    }
}
