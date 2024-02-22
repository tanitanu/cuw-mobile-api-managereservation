using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.CA.Repository.Reservation.Interface
{
    //public interface IReservationRepository : IRepository<Model.ReservationClient.Reservation>
    public interface IReservationRepository : IRepository<Mobile.Model.Internal.ReservationDomain.Reservation>
    {
        //All domain specific queries go here
        Task<List<Mobile.Model.Internal.ReservationDomain.Reservation>> GetComplexObjectByIdAsync(string loggingContext, string query);
        Task<Mobile.Model.Internal.ReservationDomain.ReservationSegment> GetActiveSegmentForReservationAsync(string loggingContext, string reservationId);
        Task<Mobile.Model.Internal.ReservationDomain.ReservationSegment> GetContextualReservationSegmentByDeviceIdAsync(string loggingContext, string deviceId);
    }
}
