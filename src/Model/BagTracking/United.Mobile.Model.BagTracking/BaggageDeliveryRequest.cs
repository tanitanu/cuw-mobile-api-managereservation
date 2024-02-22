using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagTracking
{
    public class BaggageDeliveryRequest : MOBRequest
    {
        private string claimAirline { get; set; } = string.Empty;
        public string ClaimAirline
        {
            get
            {
                return claimAirline;
            }
            set
            {
                this.claimAirline = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string claimStation { get; set; } = string.Empty;
        public string ClaimStation
        {
            get
            {
                return claimStation;
            }
            set
            {
                this.claimStation = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string agent { get; set; } = string.Empty;
        public string Agent
        {
            get
            {
                return agent;
            }
            set
            {
                this.agent = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string fileType { get; set; } = string.Empty;
        public string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                this.fileType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string fileStatus { get; set; } = string.Empty;
        public string FileStatus
        {
            get
            {
                return fileStatus;
            }
            set
            {
                this.fileStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string createDate { get; set; } = string.Empty;
        public string CreateDate
        {
            get
            {
                return createDate;
            }
            set
            {
                this.createDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private int noOfCustomers { get; set; }
        public int NoOfCustomers
        {
            get
            {
                return noOfCustomers;
            }
            set
            {
                this.noOfCustomers = value;
            }
        }

        private PassengerDetails passengerDetails { get; set; }
        public PassengerDetails PassengerDetails
        {
            get
            {
                return passengerDetails;
            }
            set
            {
                this.passengerDetails = value;
            }
        }

        private OtherFlightInfo otherFlightInfo { get; set; }
        public OtherFlightInfo OtherFlightInfo
        {
            get
            {
                return otherFlightInfo;
            }
            set
            {
                this.otherFlightInfo = value;
            }
        }

        private List<FlightInformation> flightInformation { get; set; }
        public List<FlightInformation> FlightInformation
        {
            get
            {
                return flightInformation;
            }
            set
            {
                this.flightInformation = value;
            }
        }

        private BagDetails bagDetails { get; set; }
        public BagDetails BagDetails
        {
            get
            {
                return bagDetails;
            }
            set
            {
                this.bagDetails = value;
            }
        }

        private List<BagInformation> bagInformation { get; set; }
        public List<BagInformation> BagInformation
        {
            get
            {
                return bagInformation;
            }
            set
            {
                this.bagInformation = value;
            }
        }

        private List<PassengerBagDetails> passengerBagDetails { get; set; }
        public List<PassengerBagDetails> PassengerBagDetails
        {
            get
            {
                return passengerBagDetails;
            }
            set
            {
                this.passengerBagDetails = value;
            }
        }

    }
    public class PassengerBagDetails
    {
        private PassengerDetails passengerDetails { get; set; }
        public PassengerDetails PassengerDetails
        {
            get
            {
                return passengerDetails;
            }
            set
            {
                this.passengerDetails = value;
            }
        }

        private OtherFlightInfo otherFlightInfo { get; set; }
        public OtherFlightInfo OtherFlightInfo
        {
            get
            {
                return otherFlightInfo;
            }
            set
            {
                this.otherFlightInfo = value;
            }
        }

        private List<FlightInformation> flightInformation { get; set; }
        public List<FlightInformation> FlightInformation
        {
            get
            {
                return flightInformation;
            }
            set
            {
                this.flightInformation = value;
            }
        }
        private BagDetails bagDetails { get; set; }
        public BagDetails BagDetails
        {
            get
            {
                return bagDetails;
            }
            set
            {
                this.bagDetails = value;
            }
        }
        private List<BagInformation> bagInformation { get; set; }
        public List<BagInformation> BagInformation
        {
            get
            {
                return bagInformation;
            }
            set
            {
                this.bagInformation = value;
            }
        }

    }
    public class PassengerDetails
    {
        private string firstName { get; set; } = string.Empty;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string lastName { get; set; } = string.Empty;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string initials { get; set; } = string.Empty;
        public string Initials
        {
            get
            {
                return initials;
            }
            set
            {
                this.initials = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string frequentFlyer { get; set; } = string.Empty;
        public string FrequentFlyer
        {
            get
            {
                return frequentFlyer;
            }
            set
            {
                this.frequentFlyer = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string customerStatus { get; set; } = string.Empty;
        public string CustomerStatus
        {
            get
            {
                return customerStatus;
            }
            set
            {
                this.customerStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string paxLevel { get; set; } = string.Empty;
        public string PaxLevel
        {
            get
            {
                return paxLevel;
            }
            set
            {
                this.paxLevel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string homeAddr1 { get; set; } = string.Empty;
        public string HomeAddr1
        {
            get
            {
                return homeAddr1;
            }
            set
            {
                this.homeAddr1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string homeAddr2 { get; set; } = string.Empty;
        public string HomeAddr2
        {
            get
            {
                return homeAddr2;
            }
            set
            {
                this.homeAddr2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string city { get; set; } = string.Empty;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                this.city = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string state { get; set; } = string.Empty;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                this.state = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string country { get; set; } = string.Empty;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                this.country = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string zipCode { get; set; } = string.Empty;
        public string ZipCode
        {
            get
            {
                return zipCode;
            }
            set
            {
                this.zipCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryAddr1 { get; set; } = string.Empty;
        public string DeliveryAddr1
        {
            get
            {
                return deliveryAddr1;
            }
            set
            {
                this.deliveryAddr1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryAddr2 { get; set; } = string.Empty;
        public string DeliveryAddr2
        {
            get
            {
                return deliveryAddr2;
            }
            set
            {
                this.deliveryAddr2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryCity { get; set; } = string.Empty;
        public string DeliveryCity
        {
            get
            {
                return deliveryCity;
            }
            set
            {
                this.deliveryCity = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryState { get; set; } = string.Empty;
        public string DeliveryState
        {
            get
            {
                return deliveryState;
            }
            set
            {
                this.deliveryState = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryZipCode { get; set; } = string.Empty;
        public string DeliveryZipCode
        {
            get
            {
                return deliveryZipCode;
            }
            set
            {
                this.deliveryZipCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryCountry { get; set; } = string.Empty;
        public string DeliveryCountry
        {
            get
            {
                return deliveryCountry;
            }
            set
            {
                this.deliveryCountry = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string email1 { get; set; } = string.Empty;
        public string Email1
        {
            get
            {
                return email1;
            }
            set
            {
                this.email1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string email2 { get; set; } = string.Empty;
        public string Email2
        {
            get
            {
                return email2;
            }
            set
            {
                this.email2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string homePhone { get; set; } = string.Empty;
        public string HomePhone
        {
            get
            {
                return homePhone;
            }
            set
            {
                this.homePhone = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string cellPhone1 { get; set; } = string.Empty;
        public string CellPhone1
        {
            get
            {
                return cellPhone1;
            }
            set
            {
                this.cellPhone1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string cellPhone2 { get; set; } = string.Empty;
        public string CellPhone2
        {
            get
            {
                return cellPhone2;
            }
            set
            {
                this.cellPhone2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string sendUpdatesTo { get; set; } = string.Empty;
        public string SendUpdatesTo
        {
            get
            {
                return sendUpdatesTo;
            }
            set
            {
                this.sendUpdatesTo = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string hotelName { get; set; } = string.Empty;
        public string HotelName
        {
            get
            {
                return hotelName;
            }
            set
            {
                this.hotelName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempAddr1 { get; set; } = string.Empty;
        public string TempAddr1
        {
            get
            {
                return tempAddr1;
            }
            set
            {
                this.tempAddr1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempAddr2 { get; set; } = string.Empty;
        public string TempAddr2
        {
            get
            {
                return tempAddr2;
            }
            set
            {
                this.tempAddr2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempCity { get; set; } = string.Empty;
        public string TempCity
        {
            get
            {
                return tempCity;
            }
            set
            {
                this.tempCity = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempState { get; set; } = string.Empty;
        public string TempState
        {
            get
            {
                return tempState;
            }
            set
            {
                this.tempState = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempZipCode { get; set; } = string.Empty;
        public string TempZipCode
        {
            get
            {
                return tempZipCode;
            }
            set
            {
                this.tempZipCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tempCountry { get; set; } = string.Empty;
        public string TempCountry
        {
            get
            {
                return tempCountry;
            }
            set
            {
                this.tempCountry = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string throughDate { get; set; } = string.Empty;
        public string ThroughDate
        {
            get
            {
                return throughDate;
            }
            set
            {
                this.throughDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string deliveryInstructions { get; set; } = string.Empty;
        public string DeliveryInstructions
        {
            get
            {
                return deliveryInstructions;
            }
            set
            {
                this.deliveryInstructions = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string additionalDeliveryInstructions { get; set; } = string.Empty;
        public string AdditionalDeliveryInstructions
        {
            get
            {
                return additionalDeliveryInstructions;
            }
            set
            {
                this.additionalDeliveryInstructions = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    public class OtherFlightInfo
    {
        private string pNR { get; set; } = string.Empty;
        public string PNR
        {
            get
            {
                return pNR;
            }
            set
            {
                this.pNR = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string partnerCode { get; set; } = string.Empty;
        public string PartnerCode
        {
            get
            {
                return partnerCode;
            }
            set
            {
                this.partnerCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    public class FlightInformation
    {
        private string airlineCode { get; set; } = string.Empty;
        public string AirlineCode
        {
            get
            {
                return airlineCode;
            }
            set
            {
                this.airlineCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string flightNumber { get; set; } = string.Empty;
        public string FlightNumber
        {
            get
            {
                return flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string flightDate { get; set; } = string.Empty;
        public string FlightDate
        {
            get
            {
                return flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string origin { get; set; } = string.Empty;
        public string Origin
        {
            get
            {
                return origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string destination { get; set; } = string.Empty;
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    public class BagDetails
    {
        private string bagChecked { get; set; } = string.Empty;
        public string BagChecked
        {
            get
            {
                return bagChecked;
            }
            set
            {
                this.bagChecked = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string bagReceived { get; set; } = string.Empty;
        public string BagReceived
        {
            get
            {
                return bagReceived;
            }
            set
            {
                this.bagReceived = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string lastSeen { get; set; } = string.Empty;
        public string LastSeen
        {
            get
            {
                return lastSeen;
            }
            set
            {
                this.lastSeen = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    public class BagInformation
    {
        private int bagSequenceNum { get; set; }
        public int BagSequenceNum
        {
            get
            {
                return bagSequenceNum;
            }
            set
            {
                this.bagSequenceNum = value;
            }
        }
        private string tagNumber10 { get; set; } = string.Empty;
        public string TagNumber10
        {
            get
            {
                return tagNumber10;
            }
            set
            {
                this.tagNumber10 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string color { get; set; } = string.Empty;
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                this.color = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string type { get; set; } = string.Empty;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string description { get; set; } = string.Empty;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string contents { get; set; } = string.Empty;
        public string Contents
        {
            get
            {
                return contents;
            }
            set
            {
                this.contents = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string specialFeatures { get; set; } = string.Empty;
        public string SpecialFeatures
        {
            get
            {
                return specialFeatures;
            }
            set
            {
                this.specialFeatures = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string zipper { get; set; } = string.Empty;
        public string Zipper
        {
            get
            {
                return zipper;
            }
            set
            {
                this.zipper = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
