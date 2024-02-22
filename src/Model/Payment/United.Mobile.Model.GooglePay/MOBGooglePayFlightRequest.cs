using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.GooglePay
{
    public class MOBGooglePayFlightRequest : MOBRequest
    {
        private string localScheduledDepartureDateTime;
        private string carrierCode;
        private string carrierName;
        private string flightNumber;
        private string originCode;
        private string originTerminal;
        private string originGate;
        private string destinationCode;
        private string destinationTerminal;
        private string destinationGate;
        private string passengerName;
        private string boardingGroup;
        private string seatNumber;
        private string boardingPosition;
        private string sequenceNumber;
        private string seatClass;
        private string boardingDoor;
        private string confirmationCode;
        private string eTicketNumber;
        private string frequentFlyerProgramName;
        private string frequentFlyerNumber;
        private string localBoardingDateTime;
        //private string boardingPolicy;
        //private string seatingPolicy;
        private bool securityProgram;
        private string barCodeMessage;
        private string localEstimatedOrActualDepartureDateTime;
        private string pnrType;
        private string flightClassId;
        private string flightObjectId;
        private string zoneBoardingText; //i:e: "Global Services"
        private string eliteAccessType; //i:e: "Premier Access"
        private string localScheduledArrivalDateTime;
        private string localEstimatedOrActualArrivalDateTime;
        private string operatingCarrierCode;
        private string operatingCarrierName;
        private string operatingFlightNumber;
        private string filterKey;
        private string deleteKey;
        private string fareType;
        private List<MOBKVP> messages;
        private bool isTravelReady;
        private bool isVerifiedByUnited;
        private bool isBagDropShortCutEligible;
        private bool isDigitalIdEligible;

        public string LocalScheduledDepartureDateTime
        {
            get { return localScheduledDepartureDateTime; }
            set { localScheduledDepartureDateTime = value; }
        }
        public string CarrierCode
        {
            get { return carrierCode; }
            set { carrierCode = value; }
        }
        public string CarrierName
        {
            get { return carrierName; }
            set { carrierName = value; }
        }
        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }
        public string OriginCode
        {
            get { return originCode; }
            set { originCode = value; }
        }
        public string OriginTerminal
        {
            get { return originTerminal; }
            set { originTerminal = value; }
        }
        public string OriginGate
        {
            get { return originGate; }
            set { originGate = value; }
        }
        public string DestinationCode
        {
            get { return destinationCode; }
            set { destinationCode = value; }
        }
        public string DestinationTerminal
        {
            get { return destinationTerminal; }
            set { destinationTerminal = value; }
        }
        public string DestinationGate
        {
            get { return destinationGate; }
            set { destinationGate = value; }
        }
        public string PassengerName
        {
            get { return passengerName; }
            set { passengerName = value; }
        }
        public string BoardingGroup
        {
            get { return boardingGroup; }
            set { boardingGroup = value; }
        }
        public string SeatNumber
        {
            get { return seatNumber; }
            set { seatNumber = value; }
        }
        public string BoardingPosition
        {
            get { return boardingPosition; }
            set { boardingPosition = value; }
        }
        public string SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
        public string SeatClass
        {
            get { return seatClass; }
            set { seatClass = value; }
        }
        public string BoardingDoor
        {
            get { return boardingDoor; }
            set { boardingDoor = value; }
        }
        public string ConfirmationCode
        {
            get { return confirmationCode; }
            set { confirmationCode = value; }
        }
        public string ETicketNumber
        {
            get { return eTicketNumber; }
            set { eTicketNumber = value; }
        }
        public string FrequentFlyerProgramName
        {
            get { return frequentFlyerProgramName; }
            set { frequentFlyerProgramName = value; }
        }
        public string FrequentFlyerNumber
        {
            get { return frequentFlyerNumber; }
            set { frequentFlyerNumber = value; }
        }

        public string LocalBoardingDateTime
        {
            get { return localBoardingDateTime; }
            set { localBoardingDateTime = value; }
        }
        //public string BoardingPolicy
        //{
        //    get { return boardingPolicy; }
        //    set { boardingPolicy = value; }
        //}
        //public string SeatingPolicy
        //{
        //    get { return seatingPolicy; }
        //    set { seatingPolicy = value; }
        //}
        public bool SecurityProgram
        {
            get { return securityProgram; }
            set { securityProgram = value; }
        }
        public string BarCodeMessage
        {
            get { return barCodeMessage; }
            set { barCodeMessage = value; }
        }
        public string LocalEstimatedOrActualDepartureDateTime
        {
            get { return localEstimatedOrActualDepartureDateTime; }
            set { localEstimatedOrActualDepartureDateTime = value; }
        }
        public string PNRType
        {
            get { return pnrType; }
            set { pnrType = value; }
        }
        public string FlightClassId
        {
            get { return flightClassId; }
            set { flightClassId = value; }
        }
        public string FlightObjectId
        {
            get { return flightObjectId; }
            set { flightObjectId = value; }
        }
        public string ZoneBoardingText
        {
            get { return zoneBoardingText; }
            set { zoneBoardingText = value; }
        }
        public string EliteAccessType
        {
            get { return eliteAccessType; }
            set { eliteAccessType = value; }
        }
        public string LocalScheduledArrivalDateTime
        {
            get { return localScheduledArrivalDateTime; }
            set { localScheduledArrivalDateTime = value; }
        }
        public string LocalEstimatedOrActualArrivalDateTime
        {
            get { return localEstimatedOrActualArrivalDateTime; }
            set { localEstimatedOrActualArrivalDateTime = value; }
        }
        public string OperatingCarrierCode
        {
            get { return operatingCarrierCode; }
            set { operatingCarrierCode = value; }
        }
        public string OperatingCarrierName
        {
            get { return operatingCarrierName; }
            set { operatingCarrierName = value; }
        }
        public string OperatingFlightNumber
        {
            get { return operatingFlightNumber; }
            set { operatingFlightNumber = value; }
        }
        public string FilterKey
        {
            get { return filterKey; }
            set { filterKey = value; }
        }
        public string DeleteKey
        {
            get { return deleteKey; }
            set { deleteKey = value; }
        }
        public string FareType
        {
            get { return fareType; }
            set { fareType = value; }
        }
        public List<MOBKVP> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        public bool IsTravelReady
        {
            get { return isTravelReady; }
            set { isTravelReady = value; }
        }
        public bool IsVerifiedByUnited
        {
            get { return isVerifiedByUnited; }
            set { isVerifiedByUnited = value; }
        }
        public bool IsBagDropShortCutEligible
        {
            get { return isBagDropShortCutEligible; }
            set { isBagDropShortCutEligible = value; }
        }
        public bool IsDigitalIdEligible
        {
            get { return isDigitalIdEligible; }
            set { isDigitalIdEligible = value; }
        }
    }
}
