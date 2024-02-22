using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UnitedClub
{
    public interface IUnitedClubMembershipService
    {
        Task<string> GetCurrentMembershipInfo(string mPNumber);

        Task<string> GetCurrentMembershipInfoV2(string mPNumber, string Token);
    }
}
