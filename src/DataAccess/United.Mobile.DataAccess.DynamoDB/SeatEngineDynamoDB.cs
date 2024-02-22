using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class SeatEngineDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private string tableName = string.Empty;

        public SeatEngineDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("utb_Airport");
            if (string.IsNullOrEmpty(tableName))
                tableName = "utb_Airport";
        }

        public async Task<T> GetSeatMapLegendId<T>(string from, string to, int numberOfCabins, string sessionId)
        {
            #region
            /* Database database = DatabaseFactory.CreateDatabase("ConnectionString - DB_Flightrequest");
            DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_GetComplimentary_Upgrade_Offered_flag_By_Cabin_Count");
            database.AddInParameter(dbCommand, "@Origin", DbType.String, from);
            database.AddInParameter(dbCommand, "@destination", DbType.String, to);
            database.AddInParameter(dbCommand, "@numberOfCabins", DbType.Int32, numberOfCabins);
              try
             {
                 using (IDataReader dataReader = database.ExecuteReader(dbCommand))
                 {
                     while (dataReader.Read())
                     {
                         int secondCabinBrandingId = dataReader["SecondCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["SecondCabinBrandingId"]);
                         int thirdCabinBrandingId = dataReader["ThirdCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["ThirdCabinBrandingId"]);

                         if (thirdCabinBrandingId == 0)
                         {
                             if (secondCabinBrandingId == 1)
                             {
                                 seatMapLegendId = "seatmap_legend5";
                             }
                             else if (secondCabinBrandingId == 2)
                             {
                                 seatMapLegendId = "seatmap_legend4";
                             }
                             else if (secondCabinBrandingId == 3)
                             {
                                 seatMapLegendId = "seatmap_legend3";
                             }
                         }
                         else if (thirdCabinBrandingId == 1)
                         {
                             seatMapLegendId = "seatmap_legend2";
                         }
                         else if (thirdCabinBrandingId == 4)
                         {
                             seatMapLegendId = "seatmap_legend1";
                         }
                     }
                 }
             }
             catch (System.Exception ex)
             {
                 Console.Write(ex.Message);
             }
            */
            #endregion

            try
            {
                string key = string.Format("{0}::{1}::{2}", from, to, numberOfCabins);
                //proc contains multiple tables
                var tableName = _configuration?.GetSection("DynamoDBTable").GetValue<string>("usp_GetComplimentary_Upgrade_Offered_flag_By_Cabin_Count");
                return await _dynamoDBService.GetRecords<T>(tableName, "LegendId", key, sessionId);
            }
            catch
            {

            }

            return default;
        }        
    }
}
