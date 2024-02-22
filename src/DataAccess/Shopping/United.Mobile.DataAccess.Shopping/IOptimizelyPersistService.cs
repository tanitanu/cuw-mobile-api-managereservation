using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Shopping
{
    public interface IOptimizelyPersistService
    {
        Task<string> GetFormatJson(string sessionId);
    }
}
