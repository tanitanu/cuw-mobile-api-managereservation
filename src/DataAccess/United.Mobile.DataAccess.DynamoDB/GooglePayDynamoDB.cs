using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class GooglePayDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;

        public GooglePayDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public async Task<bool> InsertGooglePayPasses<T>(T data, string key, string sessionID)
        {
            #region
            /*
                //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
                //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_GooglePay_InsertPasses");
                //database.AddInParameter(dbCommand, "@ConfirmationNumber", DbType.String, confirmationNumber);
                //database.AddInParameter(dbCommand, "@ClassID", DbType.String, classId);
                //database.AddInParameter(dbCommand, "@ObjectID", DbType.String, objectId);
                //database.AddInParameter(dbCommand, "@FilterKey", DbType.String, filterKey);
                //database.AddInParameter(dbCommand, "@DeleteKey", DbType.String, deleteKey);
                //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
                //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, appVersion);
                //database.AddInParameter(dbCommand, "@DeviceId", DbType.String, deviceId);
                //database.AddInParameter(dbCommand, "@Payload", DbType.String, payload);

                //IDataReader dataReader = database.ExecuteReader(dbCommand);
             */
            #endregion
            try
            {
                
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_GooglePay_Passes");
                var deviceID = await _dynamoDBService.SaveRecords<T>(tableName, "TransactionId", key, data, sessionID);
                return true;
            }
            catch { }
            return false;

        }
    }
}
