using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.BagCalculator;

namespace United.Mobile.DataAccess.FlightStatus
{
    public interface IAirlineCarrierService
    {
        Task<T> GetCarriers<T>(string sessionId);
    }
}
