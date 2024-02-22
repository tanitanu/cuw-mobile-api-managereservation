using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ManageReservation
{
    public interface IIRROPValidateService
    {
        Task<string> GetIRROPSStatus(string token, string request, string sessionId, string path, string eServiceAuthorization);
    }
}
