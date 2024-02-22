using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.AlertCheckFSR
{
   public interface IAlertCheckFSRService
   {
        Task<string> GetAlertCheckFSR(string token, string requestData, string sessionId);
   }
}
