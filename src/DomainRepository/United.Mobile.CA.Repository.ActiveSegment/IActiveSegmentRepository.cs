using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.ReservationDomain;
using Application = United.Mobile.Model.Internal.Application.Application;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.CA.Model.ActiveSegmentDomain;

namespace United.Mobile.CA.Repository.ActiveSegment
{
    public interface IActiveSegmentRepository : IRepository<Model.ActiveSegmentDomain.ActiveSegment>
    {
        Task<List<ReservationSegment>> GetReservationSegmentsByFlifoKey(string loggingContext, string flifoKey);

        Task<List<PassengerSegment>> GetPassengerSegmentsByReservationSegmentId(string loggingContext, string reservationSegmentId);

        Task<List<TripInformation>> GetTripsByReservationId(string loggingContext, string reservationId);

        Task<List<Reservation>> GetReservationsByMPNumberAsync(string loggingContext, string mpNumber);

        Task<List<Reservation>> GetReservationsByDeviceIdAsync(string loggingContext, string deviceId);
        Task<List<Reservation>> GetReservationsByEmployeeIdAsync(string loggingContext, string employeeId);

        Task<List<PassengerSegmentData>> GetPassengerPersistedDataByPassengerSegmentId(string loggingContext, string passengerSegmentId);

        Task<List<ReservationSegment>> GetReservationSegmentsByTripId(string loggingContext, string tripId);

        Task<List<Application>> GetApplicationsByMpNumber(string loggingContext, string mpNumber);

        Task UpdatePassengerPersistedNode(string loggingContext, PassengerSegmentData passengerSegmentPersistData);

        Task<Model.ActiveSegmentDomain.ActiveSegment> SaveActiveSegmentAsync(string loggingContext, Model.ActiveSegmentDomain.ActiveSegment activeSegment);

        Task<ScheduledFlightSegment> GetScheduledFlightSegmentByFlifoKey(string loggingContext, string flightStatusSegmentId);

        Task<OperationalFlightSegment> GetOperationalFlightSegmentByFlifoKey(string loggingContext, string scheduledFlightSegmentId);

        Task UpdateActiveSegmentForReservationAsync(string loggingContext, string recordLocator, string creationDate, string reservationSegmentPredictableKey);

        Task<ReservationSegment> GetActiveSegmentForReservationAsync(string loggingContext, string recordLocator, string creationDate);
    }
}
