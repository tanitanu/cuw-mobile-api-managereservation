using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public class AddFareLockEmailService : IAddFareLockEmailService
    {
        private readonly ICacheLog<AddFareLockEmailService> _logger;
        private readonly IResilientClient _resilientClient;

        public AddFareLockEmailService([KeyFilter("FareLockEmailOnPremSqlClientKey")] IResilientClient resilientClient, ICacheLog<AddFareLockEmailService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<bool> AddFareLockEmail(string farelockoffertype, string emaiaddr, string pnr, string pnrcreatedatetime, string sessionId)
        {
            //todo-incorporate OnPremSQLDbService
            return default;
        }
    }
}
