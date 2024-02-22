using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common.CloudDynamoDB;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class MPValidationCSSDynamoDB
    {
            private readonly IConfiguration _configuration;
            private readonly IDynamoDBService _dynamoDBService;
            private readonly string _tableName = string.Empty;
            private readonly string _transactionId = "MileagePlusValidationCSS001";
        private readonly int _absoluteExpirationDays =36500;

        public MPValidationCSSDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            _tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb_MileagePlusValidation_CSS"); //uatb_MileagePlusDevice
            if (string.IsNullOrEmpty(_tableName))
                _tableName = "cuw-validate-mp-appid-deviceid";
            _absoluteExpirationDays = _configuration.GetValue<int>("AbsoluteExpirationDays");
            if (_absoluteExpirationDays==0)
                _absoluteExpirationDays = 36500;
        }

        public Task<bool> SaveRecords<T>(T data, string sessionId, string key, string secondaryKey = "001", string transactionId="transId")
        {
            var requestData = JsonConvert.SerializeObject(data);
            return _dynamoDBService.SaveRecords<string>(_tableName,string.IsNullOrEmpty(transactionId)?_transactionId: transactionId, key, secondaryKey, requestData, sessionId, _absoluteExpirationDays);
        }
    }
}
