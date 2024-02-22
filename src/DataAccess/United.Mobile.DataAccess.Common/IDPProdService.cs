using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using United.Mobile.Model.Internal;

namespace United.Mobile.DataAccess.Common
{
    public interface IDPProdService
    {
       public Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration);     
       public Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey);

    }
}