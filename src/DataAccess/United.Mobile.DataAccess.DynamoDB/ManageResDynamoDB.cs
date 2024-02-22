using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class ManageResDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        public ManageResDynamoDB(IConfiguration configuration, IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public async Task<T> GetAllEligiblePlacePasses<T>(string destinationcode, int flag, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Get_PlacepassDetails");
            //database.AddInParameter(dbCommand, "@DestinationCode", DbType.String, "ALL");
            //database.AddInParameter(dbCommand, "@flag", DbType.Int32, 3);

            //using (IDataReader dataReader = database.ExecuteReader(dbCommand))
            //{
            //    while (dataReader.Read())
            //    {
            //        MOBPlacePass placepass = new MOBPlacePass();
            //        placepass.PlacePassID = Convert.ToInt32(dataReader["ID"].ToString().Trim());
            //        placepass.Destination = dataReader["DestinationCode"].ToString().Trim();
            //        placepass.PlacePassImageSrc = dataReader["PlacePassImageSrc"].ToString().Trim();
            //        placepass.OfferDescription = dataReader["CityDescription"].ToString().Trim();
            //        placepass.PlacePassUrl = dataReader["PlacePassUrl"].ToString().Trim();
            //        placepass.TxtPoweredBy = "Powered by";
            //        placepass.TxtPlacepass = "PLACEPASS";
            //        placepasses.Add(placepass);
            //    }
            //}
            #endregion
            string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_placepass");
            string key = string.Format("{0}::{1}", destinationcode, flag);
            return await _dynamoDBService.GetRecords<T>(tableName, "DestinationCode", key, sessionId);
        }

        public async Task<int> SaveLogToTempAnalysisTable<T>(T data, string key,string sesionID)
        {
            #region
            //Microsoft.Practices.EnterpriseLibrary.Data.Database database = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_LOGGING);
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Insert_tmp_Analysis");
            //database.AddInParameter(dbCommand, "@comment", DbType.String, logStatement);
            //database.AddInParameter(dbCommand, "@cnt", DbType.Int32, 0);

            //try
            //{
            //    database.ExecuteNonQuery(dbCommand);
            //}
            //catch (Exception ex)
            //{

            //}
            #endregion
            try
            {
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_Device");
                var deviceID = await _dynamoDBService.SaveRecords<T>(tableName, "Device001", key, data, sesionID);
                return Convert.ToInt32(deviceID);
            }
            catch { }

            return 0;
        }
    }
}
