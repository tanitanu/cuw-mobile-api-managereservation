using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class FitbitDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        public FitbitDynamoDB(IConfiguration configuration, IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }
        public async Task<bool> HasCheckedBags(string recordLocator, string lastNames, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Select_Bags");
            //database.AddInParameter(dbCommand, "@RecordLocator", DbType.String, recordLocator);
            //database.AddInParameter(dbCommand, "@PassengerLastNames", DbType.String, lastNames);

            //            USE[iPhone]
            //GO
            ///****** Object: StoredProcedure [dbo].[uasp_Select_Bags] Script Date: 08-11-2021 22:54:02 ******/
            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO
            //ALTER PROCEDURE[dbo].[uasp_Select_Bags]
            //@RecordLocator CHAR(6),
            //@PassengerLastNames VARCHAR(256)
            //AS
            //BEGIN
            //SET NOCOUNT ON;

            //            IF(1 = 1)
            //SELECT 1 as BagCount
            //ELSE
            //SELECT COUNT(BagId) as BagCount
            //FROM tb_Bag NOLOCK
            //WHERE RecordLocator = @RecordLocator
            //AND CHARINDEX(PassengerLastName, @PassengerLastNames) > 0

            //END
            #endregion

            //As per the StoredProc : "uasp_Select_Bags", it is always returning 1 (true)
            if (_configuration.GetValue<bool>("EnableHasCheckedBagsDB")) 
            {

                string key = recordLocator + "::" + lastNames;
                return await _dynamoDBService.GetRecords<bool>(_configuration?.GetValue<string>("DynamoDBTables:tb_Bag"), "checkedBags01", key, sessionId);
            }
            //As per the StoredProc : "uasp_Select_Bags", it is always returning 1 (true)
            return true;
        }

        public async Task<bool> InsertPayLoad<T>(T saveObj, string key, string sessionId)
        {
            #region
            //TODO
            //string CONNECTION_STRING_LOGGING = @"Connection String - ApplicationLogging";
            //Microsoft.Practices.EnterpriseLibrary.Data.Database database = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase(CONNECTION_STRING_LOGGING);
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Insert_PayLoad");
            //database.AddInParameter(dbCommand, "@PNR", DbType.String, recordLocator);
            //database.AddInParameter(dbCommand, "@LastName", DbType.String, lastName);
            //database.AddInParameter(dbCommand, "@JSONPayLoad", DbType.String, jsonPayLoad);
            //database.AddInParameter(dbCommand, "@ApplicationID", DbType.String, applicationId);
            //database.AddInParameter(dbCommand, "@Guid", DbType.String, guid);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, appVersion);
            //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, deviceId);

            //            USE[iPhone_TransactionLogs]
            //GO
            ///****** Object: StoredProcedure [dbo].[uasp_Insert_PayLoad] Script Date: 08-11-2021 22:28:32 ******/
            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO



            //ALTER PROCEDURE[dbo].[uasp_Insert_PayLoad]
            //@PNR VARCHAR(20),
            //@LastName VARCHAR(256),
            //@JSONPayLoad TEXT,
            //@ApplicationID int = 0,
            //@Guid NVARCHAR(64),
            //@AppVersion varchar(64) = '',
            //@DeviceID varchar(256) = ''
            //AS




            //if (@JSONPayLoad not like '%"FFC"%')
            //BEGIN
            //INSERT INTO[uatb_LogPayLoad]
            //(PNR, LastName, JSONPayLoad, ApplicationID, [Guid], AppVersion, DeviceID)
            //VALUES
            //(@PNR, @LastName, @JSONPayLoad, @ApplicationID, @Guid, @AppVersion, @DeviceID)
            //END
            #endregion
            return await _dynamoDBService.SaveRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_LogPayLoad"), "insertPayLoad01", key, saveObj, sessionId);
        }

        public async Task<T> GetGMTTime<T>(int localTime, string airportCode, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - GMTConversion");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("sel_GMT_STD_DST_Dates");
            //database.AddInParameter(dbCommand, "@InputYear", DbType.Int32, dateTime.Year);
            //database.AddInParameter(dbCommand, "@StationCode", DbType.String, airportCode.Trim().ToUpper());
            //database.AddInParameter(dbCommand, "@CarrierCode", DbType.String, "CO");

            #endregion
            return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTable:sel_GMT_STD_DST_Dates"), "gmtTime01", airportCode, sessionId);
        }

        public async Task<int> GetGMTCity<T>(string airportCode, string sessionId)
        {
            #region
            //database = DatabaseFactory.CreateDatabase("ConnectionString - GMTConversion");
            //dbCommand = (DbCommand)database.GetStoredProcCommand("sp_GMT_City");
            //database.AddInParameter(dbCommand, "@StationCode", DbType.String, airportCode.Trim().ToUpper());
            //database.AddInParameter(dbCommand, "@Carrier", DbType.String, "CO");

            #endregion
            return await _dynamoDBService.GetRecords<int>(_configuration?.GetValue<string>("DynamoDBTable:sp_GMT_City"), "gmtCity01", airportCode, sessionId);
        }
    }
}
