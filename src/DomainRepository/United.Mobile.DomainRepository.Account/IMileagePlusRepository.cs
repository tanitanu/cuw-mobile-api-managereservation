using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Internal.ReservationDomain;

namespace United.Mobile.DomainRepository.Account
{
    public interface IMileagePlusRepository : IRepository<MileagePlus>
    {
        Task<MileagePlus> GetAccountInfoFromLoyaltyAsync(string loggingContext, string mpNumber);
        Task<bool> UpsertSignInRelationAsync(string loggingContext, string mpNumber, string deviceId);
        Task<bool> UpsertEmployeeRelationAsync(string loggingContext, string mpNumber, string employeeId);
        Task<bool> RemoveSignInRelationAsync(string loggingContext, string mpNumber, string deviceId);
        Task<bool> RemoveEmployeeRelationAsync(string loggingContext, string mpNumber, string employeeId);
        Task <MileagePlus> GetMileagePlusNumberByDeviceIdAsync(string loggingContext, string deviceId);
        Task<List<ReservationSegment>> GetActiveReservationSegmentsByMPNumberAsync(string loggingContext, string mpNumber);
    }
}
