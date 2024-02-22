using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.CSLModels;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IInsertOrUpdateTravelInfoService
    {
        Task<string> InsertOrUpdateTravelerInfo(int customerId, string jsonRequest, string token, bool isAddress = false);
    }
}
