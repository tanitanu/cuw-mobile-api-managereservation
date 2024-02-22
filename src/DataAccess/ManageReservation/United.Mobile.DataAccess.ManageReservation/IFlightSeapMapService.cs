using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ManageReservation
{
    public interface IFlightSeapMapService
    {
        Task<T> ViewChangeSeats<T>(string token, string request, string sessionId, string path);
    }
}
