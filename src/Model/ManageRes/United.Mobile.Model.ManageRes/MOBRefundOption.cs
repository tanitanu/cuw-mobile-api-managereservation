using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public enum RefundType
    {
        [EnumMember(Value = "NONE")]
        [Display(Name = "None")]
        NONE,
        [EnumMember(Value = "FFC")]
        [Display(Name = "Future flight credit")]
        FFC,
        [EnumMember(Value = "FutureFlightCredit")]
        [Display(Name = "Future Flight Credit")]
        FutureFlightCredit,
        [EnumMember(Value = "ETC")]
        [Display(Name = "Electronic travel certificate")]
        ETC,
        [EnumMember(Value = "OFOP")]
        [Display(Name = "Orignal form of payment")]
        OFOP,
        [EnumMember(Value = "CNO")]
        [Display(Name = "Cancel only")]
        CNO,
        [EnumMember(Value = "CNRM")]
        [Display(Name = "Cancel redeposit miles")]
        CNRM,
    }

    [Serializable()]
    public class MOBRefundOption
    {
        private RefundType type;
        private string typeDescription;
        private string itemHeader;
        private List<MOBItem> subText;
        private string detailHeader;
        private string detailText;        
        private List<MOBItem> detailItems;
        private List<MOBItem> priceItems;
        private string refundAmount;
        private string quoteDisplayText;

        public RefundType Type { get { return type; } set { type = value; } }
        public string TypeDescription { get { return typeDescription; } set { typeDescription = value; } }
        public string ItemHeader { get { return itemHeader; } set { itemHeader = value; } }
        public List<MOBItem> SubText { get { return subText; } set { subText = value; } }
        public string DetailHeader { get { return detailHeader; } set { detailHeader = value; } }
        public string DetailText { get { return detailText; } set { detailText = value; } }        
        public List<MOBItem> DetailItems { get { return detailItems; } set { detailItems = value; } }
        public List<MOBItem> PriceItems { get { return priceItems; } set { priceItems = value; } }
        public string RefundAmount { get { return refundAmount; } set { refundAmount = value; } }
        public string QuoteDisplayText { get { return quoteDisplayText; } set { quoteDisplayText = value; } }
    }
}
