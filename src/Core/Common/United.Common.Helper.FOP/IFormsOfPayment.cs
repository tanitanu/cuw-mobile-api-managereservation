using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Payment;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using FormofPaymentOption = United.Mobile.Model.Common.Shopping.FormofPaymentOption;
using MOBShoppingCart = United.Mobile.Model.Shopping.MOBShoppingCart;
using MOBSHOPReservation = United.Mobile.Model.Shopping.MOBSHOPReservation;

namespace United.Common.Helper.FOP
{
    public interface IFormsOfPayment
    {
        Task<(List<FormofPaymentOption> response, bool isDefault)> EligibleFormOfPayments(FOPEligibilityRequest request, Session session, bool isDefault, bool IsMilesFOPEnabled = false, List<LogEntry> eligibleFoplogEntries = null);

        string GetPaymentTargetForRegisterFop(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isCompleteFarelockPurchase = false);

        Task<(List<FormofPaymentOption> response, bool isDefault)> GetEligibleFormofPayments(MOBRequest request, Session session, MOBShoppingCart shoppingCart, string cartId, string flow, bool isDefault, MOBSHOPReservation reservation = null, bool IsMilesFOPEnabled = false, SeatChangeState persistedState = null);
    }
}
