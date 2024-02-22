using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Catalog
{
    public interface IOptimizelyService
    {
        Task<string> GetFormatJson(string token, string sessionId);
    }
}
