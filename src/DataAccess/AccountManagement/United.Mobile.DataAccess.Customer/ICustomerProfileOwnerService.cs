using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerProfileOwnerService
    {
        Task<T> GetProfileOwnerInfo<T>(string token, string sessionId, string mpNumber);
    }
}
