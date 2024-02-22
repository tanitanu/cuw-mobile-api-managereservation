using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Account;
using United.Mobile.Model.Internal.ReservationDomain;

namespace United.Mobile.DomainRepository.Account
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetEmployeeInfoFromCSLAsync(string loggingContext, string employeeId);
        Task<List<ReservationSegment>> GetActiveReservationSegmentsByEmployeeIdAsync(string loggingContext, string employeeId);
        Task<Employee> GetEmployeeByDeviceIdAsync(string loggingContext, string deviceId);
    }
}
