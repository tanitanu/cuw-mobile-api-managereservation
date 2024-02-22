using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class MileagePlusDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ICacheLog _logger;
        public MileagePlusDynamoDB(IConfiguration configuration, IDynamoDBService dynamoDBService, ICacheLog logger)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            _logger = logger;
        }
        public async Task<T> GetMPAuthTokenCSS<T>(string accountNumber, int applicationId, string deviceId, string appVersion, string sessionId)
        {
            //SQL storedProc : "uasp_Get_MileagePlus_AuthToken_CSS"
            #region //SQL query to get storedProc MPData
            /*
             USE [iPhone]
                GO

                declare @MileagePlusNumber [varchar](32)
                declare @ApplicationID [int]
                declare @AppVersion [varchar](50)
                declare @DeviceID [varchar](256)

                set @MileagePlusNumber = 'AW791957'
                set @AppVersion = '4.1.30'
                set @ApplicationID = 1
                set @DeviceID = 'd007548c-addf-43fb-8f46-6870aec49647'

                declare @custID bigint
                set @custID = (select top 1 CustomerID from uatb_MileagePlusDevice nolock where MileagePlusNumber = @MileagePlusNumber order by InsertDateTime desc )

                select @custID
                SELECT top 1 * , @custID as CustID, getdate() SystemDate
                FROM uatb_MileagePlusValidation_CSS (NOLOCK)
                WHERE
                (MileagePlusNumber = @MileagePlusNumber or MPUserName = @MileagePlusNumber)
                and ApplicationID = @ApplicationID
                --and DeviceID = @DeviceID
                and IsTokenValid = 1
             */
            #endregion
            try
            {
                var key = string.Format("{0}::{1}::{2}", accountNumber, applicationId, deviceId);
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_MileagePlusValidation_CSS");
                var dbServiceResponse = await _dynamoDBService.GetRecords<string>(tableName, "AccountManagement001", key, sessionId);
                return JsonConvert.DeserializeObject<T>(dbServiceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMPAuthTokenCSS DynamoDb Get call {Exception}", JsonConvert.SerializeObject(ex));
            }

            return default;
        }

        public async Task<T> ValidateHashPinAndGetAuthToken<T>(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string sessionId)
        {
            #region 
            /* SQL storedProc : "uasp_Get_MileagePlus_AuthToken_CSS"

            /// CSS Token length is 36 and Data Power Access Token length is more than 1500 to 1700 chars
            //if (iSDPAuthentication)
            //{
            //    SPname = "uasp_select_MileagePlusAndPin_DP";
            //}
            //else
            //{
            //    SPname = "uasp_select_MileagePlusAndPin_CSS";
            //}


            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand(SPname);
            //database.AddInParameter(dbCommand, "@MileagePlusNumber", DbType.String, accountNumber);
            //database.AddInParameter(dbCommand, "@HashPincode", DbType.String, hashPinCode);
            //database.AddInParameter(dbCommand, "@ApplicationID", DbType.Int32, applicationId);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, appVersion);
            //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, deviceId);

            try
            {
                using (IDataReader dataReader = database.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        if (Convert.ToInt32(dataReader["AccountFound"]) == 1)
                        {
                            ok = true;
                            validAuthToken = dataReader["AuthenticatedToken"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { string msg = ex.Message; }
             */
            #endregion         
            try
            {
                var key = string.Format("{0}::{1}::{2}", accountNumber, applicationId, deviceId);
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_MileagePlusValidation_CSS");
                var responseData = await _dynamoDBService.GetRecords<string>(tableName, "AccountManagement001", key, sessionId);

                return JsonConvert.DeserializeObject<T>(responseData);
            }
            catch { }
            return default;
        }

    }
}
