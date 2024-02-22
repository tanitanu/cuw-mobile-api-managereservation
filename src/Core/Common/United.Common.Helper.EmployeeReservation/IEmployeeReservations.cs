using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Common.Helper.EmployeeReservation
{
    public interface IEmployeeReservations
    {
        Task<EmployeeJA> GetEResEmp20PassriderDetails(string employeeId, string token, string TransactionId, int ApplicationId, string AppVersion, string DeviceId);
    }
}
