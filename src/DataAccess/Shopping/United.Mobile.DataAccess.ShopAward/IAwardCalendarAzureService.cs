using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopAward
{
    public interface IAwardCalendarAzureService
    {
        Task<T> AwardDynamicCalendar<T>(string token, string sessionId, string request);
    }
}
