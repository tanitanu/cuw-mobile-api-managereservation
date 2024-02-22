using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.DataAccess.SeatEngine
{
    public interface IComplimentaryUpgradeService
    {
        Task<List<CabinBrand>> GetComplimentaryUpgradeOfferedFlagByCabinCount(string Origin, string destination, int numberOfCabins, string sessionId, string transactionId);
    }
}
