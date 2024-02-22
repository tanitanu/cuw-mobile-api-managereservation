using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class InsertClubPassDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;

        public InsertClubPassDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public async Task<bool> InsertUnitedClubPassToDB<T>(T data, string key, string sessionID)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Insert_UnitedClubPass");
            //database.AddInParameter(dbCommand, "@ClubPassCode", DbType.String, clubPassCode);
            //database.AddInParameter(dbCommand, "@MPAccountNumber", DbType.String, mpAccountNumber);
            //database.AddInParameter(dbCommand, "@FirstName", DbType.String, firstName);
            //database.AddInParameter(dbCommand, "@LastName", DbType.String, lastName);
            //database.AddInParameter(dbCommand, "@EMail", DbType.String, eMail);
            //database.AddInParameter(dbCommand, "@BarCodeString", DbType.String, barCodeString);
            //database.AddInParameter(dbCommand, "@PaymentAmount", DbType.Currency, paymentAmount);
            //database.AddInParameter(dbCommand, "@ExpirationDate", DbType.DateTime, expirationDate);
            //database.AddInParameter(dbCommand, "@DeviceType", DbType.String, deviceType);
            //database.AddInParameter(dbCommand, "@IsTest", DbType.StringFixedLength, isTest ? "Y" : "N");
            //database.AddInParameter(dbCommand, "@RecordLocator", DbType.String, recordLocator);
            //try
            //{
            //    database.ExecuteNonQuery(dbCommand);
            //}
            //catch (System.Exception) { }
            #endregion
            try
            {
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("utb_UnitedClubPass");
                string transId = string.IsNullOrEmpty(sessionID) ? "device001" : sessionID;
                return await _dynamoDBService.SaveRecords<T>(tableName, transId, key, data, sessionID);
            }
            catch { }
            return false;
        }
    }
}
