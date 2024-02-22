using System.Collections.Generic;
using System.Threading.Tasks;
using United.Definition;

namespace United.Mobile.DataAccess.DynamoDB
{
    public interface ILegalDocumentsForTitlesService
    {
        Task<List<MOBLegalDocument>> GetNewLegalDocumentsForTitles(string titles, string transactionId, bool isTermsnConditions);
    }
}
