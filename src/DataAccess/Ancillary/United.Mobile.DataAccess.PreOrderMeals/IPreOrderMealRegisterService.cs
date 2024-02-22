using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.PreOrderMeals
{
    public interface IPreOrderMealRegisterService
    {
        Task<string> PreOrderMealRegister(string token, string request, string sessionId, string path);
    }
}
