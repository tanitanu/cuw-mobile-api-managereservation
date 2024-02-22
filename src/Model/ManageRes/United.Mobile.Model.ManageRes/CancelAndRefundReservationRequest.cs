using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Definition.Shopping;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.PersonModel;

namespace United.Mobile.Model.ManageRes
{
    public class CancelAndRefundReservationRequest
    {
        public decimal AwardRedepositFeeTotal { get; set; }
        public decimal AwardRedepositFee { get; set; }
        public string RecordLocator { get; set; }
        public string EmailAddress { get; set; }
        public string LastName { get; set; }
        public int Channel { get; set; }
        public string PointOfSale { get; set; }
        public string MileagePlusNumber { get; set; }
        public string QuoteType { get; set; }
        public decimal RefundAmount { get; set; }
        public string CurrencyCode { get; set; }
        public double RefundMiles { get; set; }
        public double RefundUpgradeInstruments { get; set; }
        public United.Service.Presentation.PaymentModel.FormOfPayment FormOfPayment { get; set; }
        public Boolean CancelOnly { get; set; }
        public Boolean ETCRefund { get; set; }
        public decimal RefundFee { get; set; }
        public int RefundUpgradePoints { get; set; }
        public double RefundAmountOtherCurrency { get; set; }
        public string CurrencyCodeOther { get; set; }
        public string DateOfBirth { get; set; }
    }
}
