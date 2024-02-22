using System.Threading.Tasks;
using United.Mobile.Model.ManageRes;

namespace United.Mobile.CancelReservation.Domain
{
    public interface ICancelReservationBusiness
    {
        Task<MOBCancelRefundInfoResponse> CheckinCancelRefundInfo(MOBCancelRefundInfoRequest request);
        Task<MOBCancelRefundInfoResponse> CancelRefundInfo(MOBCancelRefundInfoRequest request);
        Task<MOBCancelAndRefundReservationResponse> CancelAndRefund(MOBCancelAndRefundReservationRequest request);
    }
}
