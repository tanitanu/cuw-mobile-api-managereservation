using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.Model.Common;

namespace United.Common.Helper
{
    public class CatalogHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IHeaders _headers;
        public CatalogHelper(IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IDynamoDBService dynamoDBService
            , IHeaders headers
            )
        {
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _dynamoDBService = dynamoDBService;
            _headers = headers;
        }
        public async Task<bool> GetBooleanValueFromCatalogCache(string key, int appId)
        {
            var value = await GetValueFromCatalogCache(key, appId);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return value.Trim() == "1";
        }
        internal async Task<string> GetValueFromCatalogCache(string key, int appId)
        {
            var id = GetIdForKey(key, appId);
            if (string.IsNullOrEmpty(id))
                return string.Empty;

            // var items = await LoadCatalogItemsFromPersistOrGetFromDatabaseAndSavetoPersist(appId);
            CatalogDynamoDB catalogDynamoDB = new CatalogDynamoDB(_configuration, _dynamoDBService);
            var items = await catalogDynamoDB.GetCatalogItems<MOBCatalogItem>(id, _headers.ContextValues.SessionId);

            if (items != null)
            {
                return items.CurrentValue;
            }
            return string.Empty;
        }

        private string GetIdForKey(string key, int appId)
        {
            if (string.IsNullOrWhiteSpace(key) || appId == -1)
            {
                return string.Empty;
            }

            var catalogIds = _configuration.GetValue<string>(key);
            if (!string.IsNullOrEmpty(catalogIds))
            {
                var items = catalogIds.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var keyValue = item.Split('~');
                        if (keyValue.FirstOrDefault() == appId.ToString() && keyValue.Length == 2)
                        {
                            return keyValue[1];
                        }
                    }
                }
            }

            return string.Empty;
        }

        //private async Task<List<MOBItem>> LoadCatalogItemsFromPersistOrGetFromDatabaseAndSavetoPersist(int id, bool isForceUpdate = false)
        //{
        //    var staticGuid = "CatalogValueListStaticGuid_";

        //    var catalogEnv = _configuration.GetValue<string>("Catalogenvironment");

        //    if (!string.IsNullOrEmpty(catalogEnv))
        //    {
        //        staticGuid = staticGuid + catalogEnv;
        //    }
        //    var items = (!isForceUpdate) ? _sessionHelperService.GetSession<List<MOBItem>>(staticGuid, ObjectNames.PersistedCatalogTableFullName, new List<string> { staticGuid, ObjectNames.PersistedCatalogTableFullName }).Result : null;
        //    if (items == null || !items.Any())
        //    {
        //         items = null;
        //        string storedproc = "uasp_Select_CatalogItems"; //default
        //        if (_configuration.GetValue<string>("Catalogenvironment") != null && _configuration.GetValue<string>("Catalogenvironment") != string.Empty)
        //        {
        //            string env = _configuration.GetValue<string>("Catalogenvironment").ToString();
        //            storedproc = "uasp_Select_CatalogItems_" + env;
        //        }
        //        CatalogDynamoDB catalogDynamoDB = new CatalogDynamoDB(_configuration, _dynamoDBService);
        //     items = await catalogDynamoDB.GetCatalogItems<List<MOBItem>>(id.ToString(), _headers.ContextValues.SessionId);

        //        #region Database
        //        //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
        //        //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand(storedproc.Trim());
        //        //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, 1);  //iOS
        //        //using (IDataReader dataReader = database.ExecuteReader(dbCommand))
        //        //{
        //        //    while (dataReader.Read())
        //        //    {
        //        //        if (items == null)
        //        //        {
        //        //            items = new List<MOBItem>();
        //        //        }
        //        //        MOBItem item = new MOBItem();
        //        //        item.Id = dataReader["Id"].ToString();
        //        //        item.CurrentValue = dataReader["CurrentValue"].ToString();
        //        //        items.Add(item);
        //        //    }
        //        //}

        //        //dbCommand = (DbCommand)database.GetStoredProcCommand(storedproc.Trim());
        //        //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, 2); //Android
        //        //using (IDataReader dataReader = database.ExecuteReader(dbCommand))
        //        //{
        //        //    while (dataReader.Read())
        //        //    {
        //        //        if (items == null)
        //        //        {
        //        //            items = new List<MOBItem>();
        //        //        }
        //        //        MOBItem item = new MOBItem();
        //        //        item.Id = dataReader["Id"].ToString();
        //        //        item.CurrentValue = dataReader["CurrentValue"].ToString();
        //        //        items.Add(item);
        //        //    }
        //        //}
        //        #endregion

        //        if (items != null && items.Any())
        //        {
        //            await _sessionHelperService.SaveSession<List<MOBItem>>(items, staticGuid, new List<string> { staticGuid, ObjectNames.PersistedCatalogTableFullName }, ObjectNames.PersistedCatalogTableFullName).ConfigureAwait(false);
        //        }
        //    }

        //    return await System.Threading.Tasks.Task.FromResult(items);
        //}
    }
}
