using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Club
{
    public interface IUnitedAndStarLoungesService
    {
        Task<string> GetUnitedAndStarLoungesByAirport(string token,string airportCode, string sessionId);
    }
}
