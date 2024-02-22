using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.Flifo
{
    public interface IEquipmentRepository : IRepository<CA.Model.Flifo.Equipment>
    {
        Task<bool> CreateHasOperationalEquipmentRelationship(string loggingContext, string outVId, string inVId);
    }
}
