using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopSeats
{
    public interface ISeatEngineService
    {
        Task<string> GetSeatMapDetail(string token, string action, string request, string sessionId);
    }
}
