using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Definition;

namespace United.Mobile.DataAccess.Loyalty
{
    public interface ILoyaltyAWSService
    {
       Task<string> OneClickEnrollment(string token, string requestData, string sessionId);
    }
}
