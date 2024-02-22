using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper.Shopping
{
    public interface ITravelerCSL
    {
        Task<string> GETCSLCMSContent(MobileCMSContentRequest request, bool isTravelAdvisory = false);
        Task<CSLContentMessagesResponse> GetBookingRTICMSContentMessages(MOBRequest request, Session session);
    }
}
