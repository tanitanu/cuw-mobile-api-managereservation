using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.ReservationDomain;

namespace United.Mobile.DomainRepository.Application
{
    public interface IApplicationRepository : IRepository<Model.Internal.Application.Application>
    {
        Task<bool> CreateApplicationSubscribedItemRelation(string loggingContext, string outVId, string inVId);

        Task<Task> DeleteApplicationSubscribedItemRelation(string loggingContext, string outVId, string inVId);

        Task<bool> CreateApplicationReservationRelation(string loggingContext, string applicationId, string reservationId);

        Task<bool> DeleteApplicationReservationRelation(string loggingContext, string applicationId, string reservationId);

        Task<List<Model.Internal.Application.Application>> GetApplicationsByFlifo(string loggingContext, string flifoPredictableKey);

        Task<List<Model.Internal.Application.Application>> GetApplicationsByMpNumber(string loggingContext, string mpNumber);

        Task<List<Model.Internal.Application.Application>> GetApplicationsByPushToken(string loggingContext, string pushToken);

        Task<List<Model.Internal.Application.Application>> GetApplicationsBySubscribedItem(string loggingContext, string predictableKey);

        Task<List<Model.Internal.Application.Application>> GetApplicationsByPNR(string loggingContext, string recordLocator, string carrierCode, int flightNumber, string flightDaate, string origin, string destination);
        Task<List<Model.Internal.Application.Application>> GetSignedInApplicationsByMpNumberForReservation(string loggingContext, string recordLocator, string creationDate);
        Task<List<Model.Internal.Application.Application>> GetSignedInApplicationsByEmployeeIdForReservation(string loggingContext, string recordLocator, string creationDate);
        Task<List<Model.Internal.Application.Application>> GetApplicationsForReservation(string loggingContext, string recordLocator, string creationDate);
        Task<List<ReservationSegment>> GetActiveReservationSegmentsByDeviceIdAsync(string loggingContext, string deviceId);
    }
}
