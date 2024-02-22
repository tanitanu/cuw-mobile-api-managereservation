using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerCorporateProfileService
    {
        Task<T> GetCorporateprofile<T>(string token, string request, string sessionId);
        Task<T> GetCorporateCreditCards<T>(string token, string request, string sessionId);

    }
}
