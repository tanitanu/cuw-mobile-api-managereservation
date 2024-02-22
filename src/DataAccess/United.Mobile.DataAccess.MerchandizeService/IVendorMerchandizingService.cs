using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MerchandizeService
{
    public interface IVendorMerchandizingService
    {
        Task<T> GetVendorOfferInfo<T>(string token, string request, string sessionId);
    }
}
