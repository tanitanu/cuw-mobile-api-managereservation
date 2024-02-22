using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerProfileCreditCardsService
    {
        Task<T> GetProfileCreditCards<T>(string token, string sessionId, string mpNumber);
    }
}
