using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper
{
    public class CSLStatistics
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ICSLStatisticsService _cslStatisticsService;
        public CSLStatistics(ILogger logger, IConfiguration configuraiton, IDynamoDBService dynamoDBService, ICSLStatisticsService cslStatisticsService)
        {
            _logger = logger;
            _configuration = configuraiton;
            _dynamoDBService = dynamoDBService;
            _cslStatisticsService = cslStatisticsService;
        }

        public async Task<bool> AddCSLCallStatisticsDetails(string mileagePlusNumber,
                                string category, //Category = booking or view res or my account .....
                                string cartID,
                                string callStatistics, // CSS = 0.084|ITA = 1.57|CSL_Shop =3.228|REST Total = 4.789
                                string controllerAction, // Shopping/Shop , Shopping/SelectTrip, ...
                                string sessionID)
        {
            try
            {
                if (_configuration.GetValue<bool>("Log_CSL_Call_Statistics"))
                {
                    await Task.Factory.StartNew(() => FireForgetAddCSLCallStatisticsDetails(mileagePlusNumber, category, cartID, callStatistics, controllerAction, sessionID));
                }
            }
            catch { }
            return true;
        }

        public void FireForgetAddCSLCallStatisticsDetails(string mileagePlusNumber,
                                string category, //Category = booking or view res or my account .....
                                string cartID,
                                string callStatistics, // CSS = 0.084|ITA = 1.57|CSL_Shop =3.228|REST Total = 4.789
                                string controllerAction, // Shop() , SelectTrip(), ...
                                string sessionID)
        {
            try
            {
                var record = new CSLStatisticsModel
                {
                    CallStatistics = callStatistics,
                    CartID = cartID,
                    Category = category,
                    MileagePlusNumber = mileagePlusNumber,
                    RESTAction = controllerAction,
                    SessionID = sessionID
                };
                string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb-CSL-Call-Statistics-Details");
                _dynamoDBService?.SaveRecords<CSLStatisticsModel>(tableName, "CSLStatistics01", controllerAction + cartID, record, sessionID);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void AddCSLStatistics(int applicationId,
                                 string applicationVersion,
                                 string deviceId,
                                 string sessionID,
                                 string origin,
                                 string destination,
                                 string flight_Date,
                                 string trip_Type,
                                 string classOfService,
                                 string fareOption,
                                 bool isAward,
                                 int numberOfTravelers,
                                 string mileagePlusNumber,
                                 string recordLocator,
                                 string category, //Category = booking or view res or my account .....
                                 string cartID,
                                 string callStatistics, // CSS = 0.084|ITA = 1.57|CSL_Shop =3.228|REST Total = 4.789
                                 string rESTAction
                                 ) // Shop() , SelectTrip(), ..
        {
            if (_configuration.GetValue<string>("Log_CSL_Call_Statistics") != null && _configuration.GetValue<string>("Log_CSL_Call_Statistics").ToString().ToUpper().Trim() == "TRUE")
            {
                Task.Factory.StartNew(() => FireForgetAddCSLStatistics(applicationId, applicationVersion, deviceId, sessionID, origin,
                    destination, flight_Date, trip_Type, classOfService, fareOption, isAward, numberOfTravelers, mileagePlusNumber, recordLocator,
                    category, cartID, callStatistics, rESTAction));
            }
        }

        private async Task FireForgetAddCSLStatistics(int applicationId,
                                 string applicationVersion,
                                 string deviceId,
                                 string sessionID,
                                 string origin,
                                 string destination,
                                 string flight_Date,
                                 string trip_Type,
                                 string classOfService,
                                 string fareOption,
                                 bool isAward,
                                 int numberOfTravelers,
                                 string mileagePlusNumber,
                                 string recordLocator,
                                 string category, //Category = booking or view res or my account .....
                                 string cartID,
                                 string callStatistics, // CSS = 0.084|ITA = 1.57|CSL_Shop =3.228|REST Total = 4.789
                                 string rESTAction // Shop() , SelectTrip(), ..
           )
        {
            try
            {
                var shopping = new FireCSLStatistics()
                {
                    ApplicationId = applicationId,
                    ApplicationVersion = applicationVersion,
                    DeviceId = deviceId,
                    SessionID = sessionID,
                    Origin = origin,
                    Destination = destination,
                    Flight_Date = flight_Date,
                    Trip_Type = trip_Type,
                    ClassOfService = classOfService,
                    FareOption = fareOption,
                    IsAward = isAward,
                    NumberOfTravelers = numberOfTravelers,
                    MileagePlusNumber = mileagePlusNumber,
                    RecordLocator = recordLocator,
                    Category = category,
                    CartID = cartID,
                    CallStatistics = callStatistics,
                    RESTAction = rESTAction
                };

                //   string key = string.Format("{0}::{1}", mileagePlusNumber, applicationId);
                var requestData = JsonConvert.SerializeObject(shopping);

               var response =  await _cslStatisticsService.AddCSLStatistics(requestData, shopping.SessionID);

                #region
                // string tableName = _configuration.GetSection("DynamoDBTables").GetValue<string>("uatb-CSL-Call-Statistics-Details");

                //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone_CSLStatistics");
                //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Insert_CSL_Statistics");
                //database.AddInParameter(dbCommand, "@AppID", DbType.Int64, applicationId);
                //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, applicationVersion);
                //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, deviceId);
                //database.AddInParameter(dbCommand, "@SessionID", DbType.String, sessionID);
                //database.AddInParameter(dbCommand, "@Origin", DbType.String, origin);
                //database.AddInParameter(dbCommand, "@Destination", DbType.String, destination);
                //database.AddInParameter(dbCommand, "@Flight_Date", DbType.String, flight_Date);
                //database.AddInParameter(dbCommand, "@Trip_Type", DbType.String, trip_Type);
                //database.AddInParameter(dbCommand, "@ClassOfService", DbType.String, classOfService);
                //database.AddInParameter(dbCommand, "@FareOption", DbType.String, fareOption);
                //database.AddInParameter(dbCommand, "@IsAward", DbType.Boolean, isAward);
                //database.AddInParameter(dbCommand, "@NumberOfTravelers", DbType.Int64, numberOfTravelers);
                //database.AddInParameter(dbCommand, "@MileagePlusNumber", DbType.String, mileagePlusNumber);
                //database.AddInParameter(dbCommand, "@RecordLocator", DbType.String, recordLocator);
                //database.AddInParameter(dbCommand, "@Category", DbType.String, category);
                //database.AddInParameter(dbCommand, "@CartID", DbType.String, cartID);
                //database.AddInParameter(dbCommand, "@CallStatistics", DbType.String, callStatistics);
                //database.AddInParameter(dbCommand, "@RESTAction", DbType.String, rESTAction);

                //database.ExecuteNonQuery(dbCommand);
                #endregion
            }
            catch(System.Exception ex)
            {
                string temp = ex.Message;
            }
        }
    }
}
