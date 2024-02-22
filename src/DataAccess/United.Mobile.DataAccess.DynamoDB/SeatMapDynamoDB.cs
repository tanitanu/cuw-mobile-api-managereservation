using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class SeatMapDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;

        public SeatMapDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public async Task<bool> AddPaymentNew<T>(T saveObj, string key, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Insert_Payment");
            //database.AddInParameter(dbCommand, "@TransactionId", DbType.String, transactionId);
            //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int64, applicationId);
            //database.AddInParameter(dbCommand, "@ApplicationVersion", DbType.String, applicationVersion);
            //database.AddInParameter(dbCommand, "@PaymentType", DbType.String, paymentType);
            //database.AddInParameter(dbCommand, "@Amount", DbType.Currency, amount);
            //database.AddInParameter(dbCommand, "@CurrencyCode", DbType.String, currencyCode);
            //database.AddInParameter(dbCommand, "@Mileage", DbType.Int64, mileage);
            //database.AddInParameter(dbCommand, "@Remark", DbType.String, remark);
            //database.AddInParameter(dbCommand, "@InsertBy", DbType.String, insertBy);
            //database.AddInParameter(dbCommand, "@SessionID", DbType.String, sessionId);
            //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, deviceId);
            //database.AddInParameter(dbCommand, "@RecordLocator", DbType.String, recordLocator);
            //database.AddInParameter(dbCommand, "@MileagePlusNumber", DbType.String, mileagePlusNumber);
            ////database.AddInParameter(dbCommand, "@RESTAPIVersion", DbType.String, "WEBAPI-BOOKING 2.0");
            //database.AddInParameter(dbCommand, "@RESTAPIVersion", DbType.String, (string.IsNullOrEmpty(restAPIVersion) ? (System.Configuration.ConfigurationManager.AppSettings["LogExceptionOnly"] != null ? System.Configuration.ConfigurationManager.AppSettings["RESTWEBAPIVersion"].ToString() : null) : restAPIVersion));

            #endregion
            return await _dynamoDBService.SaveRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb-Payment"), "trans0", key, saveObj, sessionId);
        }

        public async Task<T> GetBillingAddressCountries<T>(string key, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Select_BillingCountryList");
            //using (IDataReader dataReader = database.ExecuteReader(dbCommand))
            //{
            //    while (dataReader.Read())
            //    {
            //        billingCountries.Add(new MOBCPBillingCountry
            //        {
            //            CountryName = Convert.ToString(dataReader["CountryName"]),
            //            CountryCode = Convert.ToString(dataReader["CountryCode"]),
            //            Id = Convert.ToString(dataReader["BillingCountryOrder"]),
            //            IsStateRequired = Convert.ToBoolean(dataReader["IsStateRequired"]),
            //            IsZipRequired = Convert.ToBoolean(dataReader["IsZipPostalRequired"]),
            //        });
            //    }
            //}

            #endregion
            return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_BillingCountryList"), "billingCountries01", key, sessionId);
        }

        public async Task<T> GetSeatPreference<T>(string key, string sessionId)
        {
            return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_SeatPreference"), "UnitedSeatPreference", key, sessionId);
        }
    }
}
