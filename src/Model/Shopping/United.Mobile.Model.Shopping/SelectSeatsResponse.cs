using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("MOBSHOPSelectSeatsResponse")]
    public class SelectSeatsResponse:MOBResponse
    {
        private string sessionId = string.Empty;
        private string epaMessageTitle = string.Empty;
        private string epaMessage = string.Empty;
        private string clearOption = string.Empty;


        public Section PromoCodeRemovalAlertMessage { get; set; }

        public string EPAMessageTitle
        {
            get { return this.epaMessageTitle; }
            set { this.epaMessageTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string EPAMessage
        {
            get { return this.epaMessage; }
            set { this.epaMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public SelectSeatsRequest FlightTravelerSeatsRequest { get; set; }
     
        public List<MOBSeat> Seats { get; set; }

        public List<MOBSeatMap> SeatMap { get; set; }

        public List<MOBTypeOption> ExitAdvisory { get; set; }

        public string Flow { get; set; } = string.Empty;

        public List<MOBFSRAlertMessage> SeatmapMessaging { get; set; }

        public string CartId { get; set; } = string.Empty;

        public SelectSeatsResponse()
        {
            Seats = new List<MOBSeat>();
            SeatMap = new List<MOBSeatMap>();
            ExitAdvisory = new List<MOBTypeOption>();
        }
        public string ClearOption
        {
            get { return this.clearOption; }
            set { clearOption = value; }
        }
    }
}
