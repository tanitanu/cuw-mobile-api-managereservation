using System.Threading.Tasks;
using United.Mobile.CA.Model.Flifo;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Flifo
{
    public interface IOperationalSegmentRepository : IRepository<CA.Model.Flifo.OperationalFlightSegment>
    {
        Task<OperationalFlightSegment> GetOperationalFlightSegmentByDeviceIdAsync(string loggingContext, string deviceId);
    }
}
