using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class MPCSSValidationDetailsDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly string _tableName = "abh_uatb_MileagePlusValidation_CSS";
        private readonly string _transactionId = "MileagePlusValidationCSS001";

        public MPCSSValidationDetailsDynamoDB(IConfiguration configuration
            , IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public Task<bool> SaveRecords<T>(string mileagePlusNumber, T data, string sessionId)
        {
            return _dynamoDBService.SaveRecords<T>(_tableName, _transactionId, mileagePlusNumber, data, sessionId);
        }
    }
}
