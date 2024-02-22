using System.Threading.Tasks;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Flifo
{
    public interface IScheduledSegmentRepository : IRepository<CA.Model.Flifo.ScheduledFlightSegment>
    {
        Task<bool> CreateHasOperationalSegmentRelationship(string loggingContext, string outVId, string inVId);

        Task<ScheduledFlightSegment> GetScheduledFlightSegmentByDeviceIdAsync(string loggingContext, string deviceId);
    }
}
