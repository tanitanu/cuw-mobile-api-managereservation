using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.TeaserPage
{
    public interface IGetTeaserColumnInfoService
    {
        Task<T> GetTeaserText<T>(string token,string cartID, string langCode, string countryCode, string sessionId);
    }
}
