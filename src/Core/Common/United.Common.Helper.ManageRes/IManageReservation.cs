using System.Collections.ObjectModel;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Service.Presentation.SegmentModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.PersonalizationResponseModel;

namespace United.Common.Helper.ManageRes
{
    public interface IManageReservation
    {
        Task<MOBPNRByRecordLocatorResponse> GetPNRByRecordLocatorCommonMethod(MOBPNRByRecordLocatorRequest request);
        Task<TravelOptionsResponse> GetProductOfferAndDetails(TravelOptionsRequest request, Session session);
        Task<ReservationDetail> GetCslReservation(string sessionId);
        Task<MOBPNR> GetPNRResponse(string sessionId);
        Task<DynamicOfferDetailResponse> GetDynamicOfferDetailResponse(TravelOptionsRequest request, ReservationDetail cslReservation, MOBPNR mobPnr, string Token);
        Task<DynamicOfferDetailResponse> GetDODResponseFromCCE(TravelOptionsRequest request, Session session, ReservationDetail cslReservation, bool isLoadPCUOfferForSeatMap);
        Task<MOBSeatChangeInitializeResponse> SeatChangeInitialize(MOBSeatChangeInitializeRequest request);
    }
}
