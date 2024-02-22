using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Flifo
{
    public interface IFlightRepository : IRepository<CA.Model.Flifo.Flight>
    {
        Task<bool> CreateHasScheduledSegmentRelationship(string loggingContext, string outVId, string inVId);
    }
}
