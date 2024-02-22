using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.DomainRepository.DBManagement
{
    public interface IDBManagementRepository
    {
        Task CleanupNodes(string loggingContext, int deleteDurationInDays);
    }
}
