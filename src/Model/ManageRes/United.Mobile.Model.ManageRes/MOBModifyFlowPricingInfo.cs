using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBModifyFlowPricingInfo
    {
        private string totalPaid;
        private string taxesAndFeesTotal;
        private List<MOBModifyFlowPricePerType> pricesPerTypes;
        private string quoteType;
        private string currencyCode;
        private string refundMiles;
        private string totalDue;
        private string refundMilesLabel = string.Empty;
        private string refundFOPLabel = string.Empty;
        private bool hasTotalDue = false;
        private double redepositFee;
        private string formattedPricingDetail = string.Empty;
        private string quoteDisplayText;
        private List<MOBItem> priceItems;
        private bool showMessageOnly;
        private MOBConversionPricingInfo conversionInfo;

        public List<MOBItem> PriceItems
        {
            get { return priceItems; }
            set { priceItems = value; }
        }

        public string TotalPaid
        {
            get { return totalPaid; }
            set { totalPaid = value; }
        }

        public string TaxesAndFeesTotal
        {
            get { return taxesAndFeesTotal; }
            set { taxesAndFeesTotal = value; }
        }

        public List<MOBModifyFlowPricePerType> PricesPerTypes
        {
            get { return pricesPerTypes; }
            set { pricesPerTypes = value; }
        }

        public string QuoteType
        {
            get { return quoteType; }
            set { quoteType = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public string RefundMiles
        {
            get { return refundMiles; }
            set { refundMiles = value; }
        }

        public string TotalDue
        {
            get { return totalDue; }
            set { totalDue = value; }
        }       

        public string RefundMilesLabel
        {
            get { return refundMilesLabel; }
            set { refundMilesLabel = value; }
        }
        public string RefundFOPLabel
        {
            get { return refundFOPLabel; }
            set { refundFOPLabel = value; }
        }
        public bool HasTotalDue
        {
            get { return hasTotalDue; }
            set { hasTotalDue = value; }
        }

        public double RedepositFee
        {
            get { return redepositFee; }
            set { redepositFee = value; }
        }

        public string FormattedPricingDetail
        {
            get { return formattedPricingDetail;}
            set { formattedPricingDetail = value; }
        }
        public string QuoteDisplayText { get { return quoteDisplayText; } set { quoteDisplayText = value; } }

        public bool ShowMessageOnly
        {
            get { return showMessageOnly; }
            set { showMessageOnly = value; }
        }

        public MOBConversionPricingInfo ConversionInfo
        {
            get { return conversionInfo; }
            set { conversionInfo = value; }
        }
    }
    [Serializable]
    public class MOBConversionPricingInfo
    {
        private string baseFare;
        private string taxesAndFees;
        private string cancellationFees;
        private string conversionEquivalent;
        private ConversionToolTipInfo toolTipInfo;
        public string BaseFare { get { return baseFare; } set { baseFare = value; } }
        public string TaxesAndFees { get { return taxesAndFees; } set { taxesAndFees = value; } }
        public string CancellationFees { get { return cancellationFees; } set { cancellationFees = value; } }
        public string ConversionEquivalent { get { return conversionEquivalent; } set { conversionEquivalent = value; } }
        public ConversionToolTipInfo ToolTipInfo { get { return toolTipInfo; } set { toolTipInfo = value; } }

    }

    [Serializable]
    public class ConversionToolTipInfo
    {
        private string title;
        private string header;
        private string body;
        private string buttonText;
        public string Title { get { return title; } set { title = value; } }
        public string Header { get { return header; } set { header = value; } }
        public string Body { get { return body; } set { body = value; } }
        public string ButtonText { get { return buttonText; } set { buttonText = value; } }
    }
}
