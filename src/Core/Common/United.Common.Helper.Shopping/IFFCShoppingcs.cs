using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Common.Helper.Shopping
{
    public interface IFFCShoppingcs
    {
        Task AssignFFCValues(string sessionid, MOBShoppingCart shoppingCart, MOBRequest request, MOBFormofPaymentDetails formOfPaymentDetails, MOBSHOPReservation reservation);
        void AssignTravelerTotalFFCNewValueAfterReDeem(MOBCPTraveler traveler);
        Task<List<CMSContentMessage>> GetSDLContentByGroupName(MOBRequest request, string sessionId, string token, string groupName, string docNameConfigEntry, bool useCache = false);
        MOBSHOPPrice UpdateCertificatePrice(MOBSHOPPrice ffc, double totalAmount, string priceType, string priceTypeDescription, string status = "", bool isAddNegative = false);
        void AssignIsOtherFOPRequired(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices);
        void AssignFormOfPaymentType(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, bool IsSecondaryFOP = false, bool isRemoveAll = false);
        void UpdatePricesInReservation(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBSHOPPrice> prices);
        List<MOBMobileCMSContentMessages> BuildReviewFFCHeaderMessage(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBCPTraveler> travelers, List<CMSContentMessage> lstMessages);
        void UpdateTravelCreditAmountWithSelectedETCOrFFC(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, List<MOBCPTraveler> travelers);
        void ApplyFFCToAncillary(List<ProdDetail> products, MOBApplication application, MOBFormofPaymentDetails mobFormofPaymentDetails, List<MOBSHOPPrice> prices, bool isAncillaryON = false);
        void AssignNullToETCAndFFCCertificates(MOBFormofPaymentDetails fopDetails, MOBRequest request);
        List<MOBMobileCMSContentMessages> AssignEmailMessageForFFCRefund(List<CMSContentMessage> lstMessages, List<MOBSHOPPrice> prices, string email, FOPTravelFutureFlightCredit futureFlightCredit, MOBApplication application);
        bool UpdateFFCAmountAsPerChangedPrice(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBCPTraveler> travelersCSL, string sessionid);
    }
}
