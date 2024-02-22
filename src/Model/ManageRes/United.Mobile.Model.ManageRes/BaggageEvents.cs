using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace United.Mobile.Model.ManageRes
{
        //For text part of baggage message  
        public class BaggageEvents
        {
            [JsonProperty("bag")]
            public BagDTO Bag { get; set; }

            [JsonProperty("bagFlightLegs")]
            public List<BagFlightLegDTO> BagFlightLegs { get; set; }

            [JsonProperty("passenger")]
            public PassengerDTO Passenger { get; set; }

            [JsonProperty("passengerFlightLegs")]
            public List<PassengerFlightLegDTO> PassengerFlightLegs { get; set; }

            [JsonProperty("scans")]
            public List<ScanDTO> Scans { get; set; }

            //[JsonProperty("bagClaimFiles")]
            //public List<BoltDTO> BagClaimFiles { get; set; }

            //[JsonProperty("properties")]
            //public string Properties { get; set; }
        }

        
        public class BagDTO
        {
            [JsonProperty("bagKey")]
            public string BagKey { get; set; }

            [JsonProperty("bagTagNumber")]
            public string BagTagNumber { get; set; }

            [JsonProperty("bagTagUniqueKey")]
            public Int64 BagTagUniqueKey { get; set; }

            [JsonProperty("correlationID")]
            public string CorrelationID { get; set; }

            [JsonProperty("upsertTimestamp")]
            public string UpsertTimestamp { get; set; }

            [JsonProperty("originStation")]
            public string OriginStation { get; set; }

            [JsonProperty("journeyKey")]
            public string JourneyKey { get; set; }

            [JsonProperty("passengerKey")]
            public string PassengerKey { get; set; }

            [JsonProperty("finalDestinationStation")]
            public string FinalDestinationStation { get; set; }

            [JsonProperty("issueTimestamp")]
            public Int64 IssueTimestamp { get; set; }

            [JsonProperty("actionCode")]
            public string ActionCode { get; set; }

            [JsonProperty("subActionCode")]
            public string SubActionCode { get; set; }

            [JsonProperty("dataSource")]
            public string DataSource { get; set; }

            [JsonProperty("bagTypes")]
            public List<string> BagTypes { get; set; }

            [JsonProperty("bagTypesCodes")]
            public List<string> BagTypesCodes { get; set; }

            [JsonProperty("airwayBillNumber")]
            public string AirwayBillNumber { get; set; }

            [JsonProperty("printerID")]
            public string PrinterID { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("isPriority")]
            public bool IsPriority { get; set; }

            [JsonProperty("isPassengerBag")]
            public bool IsPassengerBag { get; set; }

            [JsonProperty("isCrewBag")]
            public bool IsCrewBag { get; set; }

            [JsonProperty("isGateBag")]
            public bool IsGateBag { get; set; }

            [JsonProperty("isValetBag")]
            public bool IsValetBag { get; set; }

            [JsonProperty("isPlannedUnaccompanied")]
            public bool IsPlannedUnaccompanied { get; set; }

            [JsonProperty("isHeavy")]
            public bool IsHeavy { get; set; }

            [JsonProperty("isNoRec")]
            public bool IsNoRec { get; set; }

            [JsonProperty("isNoRecMerged")]
            public bool IsNoRecMerged { get; set; }

            [JsonProperty("isShortChecked")]
            public bool IsShortChecked { get; set; }

            [JsonProperty("bagCreationType")]
            public string BagCreationType { get; set; }

            [JsonProperty("checkedWeight")]
            public string CheckedWeight { get; set; }

            [JsonProperty("checkedWeightUOM")]
            public string CheckedWeightUOM { get; set; }

            [JsonProperty("rawMessageKey")]
            public string RawMessageKey { get; set; }

            [JsonProperty("bsmPNRNumber")]
            public string BsmPNRNumber { get; set; }

            [JsonProperty("bsmFirstName")]
            public string BsmFirstName { get; set; }

            [JsonProperty("bsmLastName")]
            public string BsmLastName { get; set; }

            [JsonProperty("isSelectee")]
            public bool IsSelectee { get; set; }

            [JsonProperty("assignmentDTO")]
            public AssignmentDTO AssignmentDTO { get; set; }

            [JsonProperty("attachedBagTagNumber")]
            public string AttachedBagTagNumber { get; set; }

            [JsonProperty("noteDTOs")]
            public List<NoteDTO> NoteDTOs { get; set; }

            [JsonProperty("bagTypeDesc")]
            public List<string> BagTypeDesc { get; set; }

            [JsonProperty("bagTypesDescSetDTO")]
            public List<BagTypesDTO> BagTypesDescSetDTO { get; set; }

            [JsonProperty("actionStation")]
            public string ActionStation { get; set; }

            [JsonProperty("sls")]
            public string Sls { get; set; }
        }

        public class AssignmentDTO
        {
            [JsonProperty("channel")]
            public string Channel { get; set; }

            [JsonProperty("agentID")]
            public string AgentID { get; set; }

            [JsonProperty("upsertTimestamp")]
            public int UpsertTimestamp { get; set; }

            [JsonProperty("station")]
            public string Station { get; set; }

            [JsonProperty("assignmentStatus")]
            public string AssignmentStatus { get; set; }

            [JsonProperty("assignmentType")]
            public string AssignmentType { get; set; }
        }

        public class NoteDTO
        {
            [JsonProperty("noteKey")]
            public string NoteKey { get; set; }

            [JsonProperty("channel")]
            public string Channel { get; set; }

            [JsonProperty("agentID")]
            public string AgentID { get; set; }

            [JsonProperty("utcUpsertTimestamp")]
            public string UtcUpsertTimestamp { get; set; }

            [JsonProperty("station")]
            public string Station { get; set; }

            [JsonProperty("note")]
            public string Note { get; set; }
        }

        public class BagTypesDTO
        {
            [JsonProperty("bagTypeDesc")]
            public string BagTypeDesc { get; set; }

            [JsonProperty("bagType")]
            public string BagType { get; set; }

            [JsonProperty("bagTypeCode")]
            public string BagTypeCode { get; set; }
        }

        //
        public class BagFlightLegDTO
        {
            [JsonProperty("bagFlightLegKey")]
            public string BagFlightLegKey { get; set; }

            [JsonProperty("bagKey")]
            public string BagKey { get; set; }

            [JsonProperty("correlationID")]
            public string CorrelationID { get; set; }

            [JsonProperty("journeyKey")]
            public string JourneyKey { get; set; }

            [JsonProperty("bagTagNumber")]
            public string BagTagNumber { get; set; }

            [JsonProperty("carrierCode")]
            public string CarrierCode { get; set; }

            [JsonProperty("flightNumber")]
            public int FlightNumber { get; set; }

            [JsonProperty("legSequenceNumber")]
            public int LegSequenceNumber { get; set; }

            [JsonProperty("opsLegSequenceNumber")]
            public int OpsLegSequenceNumber { get; set; }

            [JsonProperty("departureStation")]
            public string DepartureStation { get; set; }

            [JsonProperty("departureStationCountryCode")]
            public string DepartureStationCountryCode { get; set; }

            [JsonProperty("arrivalStation")]
            public string ArrivalStation { get; set; }

            [JsonProperty("arrivalStationCountryCode")]
            public string ArrivalStationCountryCode { get; set; }

            [JsonProperty("localScheduledDepartureDate")]
            public string LocalScheduledDepartureDate { get; set; }

            [JsonProperty("sourceType")]
            public string SourceType { get; set; }

            [JsonProperty("dataSource")]
            public string DataSource { get; set; }

            [JsonProperty("loadStatus")]
            public string LoadStatus { get; set; }

            [JsonProperty("handlingStatus")]
            public string HandlingStatus { get; set; }

            [JsonProperty("handlingAction")]
            public string HandlingAction { get; set; }

            [JsonProperty("rushType")]
            public string RushType { get; set; }

            [JsonProperty("rerouteType")]
            public string RerouteType { get; set; }

            [JsonProperty("arrivalRouteType")]
            public string ArrivalRouteType { get; set; }

            [JsonProperty("departureRouteType")]
            public string DepartureRouteType { get; set; }

            [JsonProperty("isDepartureScan")]
            public bool IsDepartureScan { get; set; }

            [JsonProperty("isInternational")]
            public bool IsInternational { get; set; }

            [JsonProperty("isOriginal")]
            public bool IsOriginal { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("isCurrent")]
            public bool IsCurrent { get; set; }

            [JsonProperty("isPlanned")]
            public bool IsPlanned { get; set; }

            [JsonProperty("isMishandled")]
            public bool IsMishandled { get; set; }

            [JsonProperty("isOverlayStickerPrinted")]
            public bool IsOverlayStickerPrinted { get; set; }

            [JsonProperty("isRush")]
            public bool IsRush { get; set; }

            [JsonProperty("isReroute")]
            public bool IsReroute { get; set; }

            [JsonProperty("isPending")]
            public bool IsPending { get; set; }

            [JsonProperty("transferFromFlightKey")]
            public string TransferFromFlightKey { get; set; }

            [JsonProperty("transferToFlightKey")]
            public string TransferToFlightKey { get; set; }

            [JsonProperty("upsertTimestamp")]
            public string UpsertTimestamp { get; set; }

            [JsonProperty("csrAuthorizationDTO")]
            public CSRAuthorizationDTO CsrAuthorizationDTO { get; set; }

            [JsonProperty("actionCode")]
            public string ActionCode { get; set; }

            [JsonProperty("rawMessageKey")]
            public string RawMessageKey { get; set; }

            [JsonProperty("isArrivalScan")]
            public bool IsArrivalScan { get; set; }

            [JsonProperty("localScheduledDepartureDateTime")]
            public string LocalScheduledDepartureDateTime { get; set; }

            [JsonProperty("utcScheduledDepartureDateTime")]
            public string UtcScheduledDepartureDateTime { get; set; }

            [JsonProperty("localScheduledArrivalDateTime")]
            public string LocalScheduledArrivalDateTime { get; set; }

            [JsonProperty("utcScheduledArrivalDateTime")]
            public string UtcScheduledArrivalDateTime { get; set; }

            [JsonProperty("scanKey")]
            public string ScanKey { get; set; }

            [JsonProperty("isHotBag")]
            public bool IsHotBag { get; set; }

            [JsonProperty("isAdvanced")]
            public bool IsAdvanced { get; set; }

            [JsonProperty("isClaimAreaScan")]
            public bool IsClaimAreaScan { get; set; }

            [JsonProperty("actionStation")]
            public string ActionStation { get; set; }

            [JsonProperty("isTrueThru")]
            public bool IsTrueThru { get; set; }

            [JsonProperty("chgOfGaugeInd")]
            public bool ChgOfGaugeInd { get; set; }

            [JsonProperty("recordLocator")]
            public string RecordLocator { get; set; }  //writeOnly: true
        }

        public class CSRAuthorizationDTO
        {
            [JsonProperty("isAuthorizedToLoad")]
            public bool IsAuthorizedToLoad { get; set; }

            [JsonProperty("agentID")]
            public string AgentID { get; set; }

            [JsonProperty("reasonCode")]
            public string ReasonCode { get; set; }

            [JsonProperty("reasonDescription")]
            public string ReasonDescription { get; set; }

            [JsonProperty("channelCode")]
            public string ChannelCode { get; set; }
        }

        //
        public class PassengerDTO
        {
            [JsonProperty("passengerKey")]
            public string PassengerKey { get; set; }

            [JsonProperty("upsertTimestamp")]
            public string UpsertTimestamp { get; set; }

            [JsonProperty("pnrNumber")]
            public string PnrNumber { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("frequentTravelerInfo")]
            public FrequentTravelerInfo FrequentTravelerInfo { get; set; }

            [JsonProperty("connectionPNRNumber")]
            public string ConnectionPNRNumber { get; set; }

            [JsonProperty("pnrCreationDate")]
            public string PnrCreationDate { get; set; }

            [JsonProperty("splitFromPNRNumber")]
            public string SplitFromPNRNumber { get; set; }

            [JsonProperty("journeyDTOs")]
            public List<JourneyDTO> JourneyDTOs { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("otherAirlineInfos")]
            public List<OtherAirlineInfoDTO> OtherAirlineInfos { get; set; }

            [JsonProperty("isNonRevenueStandby")]
            public bool IsNonRevenueStandby { get; set; }

            [JsonProperty("groupTier")]
            public string GroupTier { get; set; }

            [JsonProperty("oaPnrNumber")]
            public List<string> OaPnrNumber { get; set; }
        }

        public class FrequentTravelerInfo
        {
            [JsonProperty("number")]
            public string Number { get; set; }

            [JsonProperty("tier")]
            public string Tier { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("carrierCode")]
            public string CarrierCode { get; set; }

            [JsonProperty("statusCode")]
            public string StatusCode { get; set; }

            [JsonProperty("starAllianceTier")]
            public string StarAllianceTier { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class JourneyDTO
        {
            [JsonProperty("journeyKey")]
            public string JourneyKey { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("finalDestination")]
            public string FinalDestination { get; set; }

            [JsonProperty("origin")]
            public string Origin { get; set; }

            [JsonProperty("creationTimestamp")]
            public string CreationTimestamp { get; set; }
        }

        public class OtherAirlineInfoDTO
        {
            [JsonProperty("entityCode")]
            public string EntityCode { get; set; }

            [JsonProperty("otherAirlinePNRNumber")]
            public string OtherAirlinePNRNumber { get; set; }
        }

        //
        public class PassengerFlightLegDTO
        {
            [JsonProperty("passengerFlightLegKey")]
            public string PassengerFlightLegKey { get; set; }

            [JsonProperty("journeyKey")]
            public string JourneyKey { get; set; }

            [JsonProperty("correlationID")]
            public string CorrelationID { get; set; }

            [JsonProperty("rawMessageKey")]
            public string RawMessageKey { get; set; }

            [JsonProperty("flightNumber")]
            public int FlightNumber { get; set; }

            [JsonProperty("cabinType")]
            public string CabinType { get; set; }

            [JsonProperty("transferFromFlightKey")]
            public string TransferFromFlightKey { get; set; }

            [JsonProperty("transferToFlightKey")]
            public string TransferToFlightKey { get; set; }

            [JsonProperty("departureStation")]
            public string DepartureStation { get; set; }

            [JsonProperty("arrivalStation")]
            public string ArrivalStation { get; set; }

            [JsonProperty("legSequenceNumber")]
            public int LegSequenceNumber { get; set; }

            [JsonProperty("seatNumber")]
            public string SeatNumber { get; set; }

            [JsonProperty("departureStationCountryCode")]
            public string DepartureStationCountryCode { get; set; }

            [JsonProperty("arrivalStationCountryCode")]
            public string ArrivalStationCountryCode { get; set; }

            [JsonProperty("localScheduledDepartureDate")]
            public string LocalScheduledDepartureDate { get; set; }

            [JsonProperty("carrierCode")]
            public string CarrierCode { get; set; }

            [JsonProperty("sourceType")]
            public string SourceType { get; set; }

            [JsonProperty("dataSource")]
            public string DataSource { get; set; }

            [JsonProperty("isPassengerBoarded")]
            public bool IsPassengerBoarded { get; set; }

            [JsonProperty("isPassengerCheckedIn")]
            public bool IsPassengerCheckedIn { get; set; }

            [JsonProperty("isPassengerStandby")]
            public bool IsPassengerStandby { get; set; }

            [JsonProperty("hasNoSeat")]
            public bool HasNoSeat { get; set; }

            [JsonProperty("isInternational")]
            public bool IsInternational { get; set; }

            [JsonProperty("isActive")]
            public bool IsActive { get; set; }

            [JsonProperty("isCurrent")]
            public bool IsCurrent { get; set; }

            [JsonProperty("actionCode")]
            public string ActionCode { get; set; }

            [JsonProperty("arrivalRouteType")]
            public string ArrivalRouteType { get; set; }

            [JsonProperty("departureRouteType")]
            public string DepartureRouteType { get; set; }

            [JsonProperty("segmentStatusCode")]
            public string SegmentStatusCode { get; set; }

            [JsonProperty("upsertTimestamp")]
            public string UpsertTimestamp { get; set; }

            [JsonProperty("localScheduledDepartureDateTime")]
            public string LocalScheduledDepartureDateTime { get; set; }

            [JsonProperty("utcScheduledDepartureDateTime")]
            public string UtcScheduledDepartureDateTime { get; set; }

            [JsonProperty("localScheduledArrivalDateTime")]
            public string LocalScheduledArrivalDateTime { get; set; }

            [JsonProperty("utcScheduledArrivalDateTime")]
            public string UtcScheduledArrivalDateTime { get; set; }

            [JsonProperty("sequenceNumber")]
            public int SequenceNumber { get; set; }

            [JsonProperty("actionStation")]
            public string ActionStation { get; set; }

            [JsonProperty("paxCategoryCode")]
            public string PaxCategoryCode { get; set; }
        }

        //
        public class ScanDTO
        {
            [JsonProperty("scanKey")]
            public string ScanKey { get; set; }

            [JsonProperty("bagKey")]
            public string BagKey { get; set; }

            [JsonProperty("bagTagNumber")]
            public string BagTagNumber { get; set; }

            [JsonProperty("carrierCode")]
            public string CarrierCode { get; set; }

            [JsonProperty("flightNumber")]
            public int FlightNumber { get; set; }

            [JsonProperty("departureDate")]
            public string DepartureDate { get; set; }

            [JsonProperty("departureStation")]
            public string DepartureStation { get; set; }

            [JsonProperty("bagFlightLegKey")]
            public string BagFlightLegKey { get; set; }

            [JsonProperty("correlationID")]
            public string CorrelationID { get; set; }

            [JsonProperty("upsertTimestamp")]
            public string UpsertTimestamp { get; set; }

            [JsonProperty("scanType")]
            public string ScanType { get; set; }

            [JsonProperty("actionCode")]
            public string ActionCode { get; set; }

            [JsonProperty("subActionCode")]
            public string SubActionCode { get; set; }

            [JsonProperty("scanLocation")]
            public string ScanLocation { get; set; }

            [JsonProperty("sendToLocation")]
            public string SendToLocation { get; set; }

            [JsonProperty("scanStation")]
            public string ScanStation { get; set; }

            [JsonProperty("localScanTimestamp")]
            public string LocalScanTimestamp { get; set; }

            [JsonProperty("utcScanTimeStamp")]
            public string UtcScanTimeStamp { get; set; }

            [JsonProperty("uploadTimestamp")]
            public string UploadTimestamp { get; set; }

            [JsonProperty("scannerID")]
            public string ScannerID { get; set; }

            [JsonProperty("scanDeviceType")]
            public string ScanDeviceType { get; set; }

            [JsonProperty("storageID")]
            public string StorageID { get; set; }

            [JsonProperty("containerType")]
            public string ContainerType { get; set; }

            [JsonProperty("stageID")]
            public string StageID { get; set; }

            [JsonProperty("bagLoadSequenceNumber")]
            public int BagLoadSequenceNumber { get; set; }

            [JsonProperty("originalSectorNumber")]
            public string OriginalSectorNumber { get; set; }

            [JsonProperty("currentSectorNumber")]
            public string CurrentSectorNumber { get; set; }

            [JsonProperty("isAnimalOK")]
            public bool IsAnimalOK { get; set; }

            [JsonProperty("isCrateOK")]
            public bool IsCrateOK { get; set; }

            [JsonProperty("isDepartureScan")]
            public bool IsDepartureScan { get; set; }

            [JsonProperty("isArrivalScan")]
            public bool IsArrivalScan { get; set; }

            [JsonProperty("legSequenceNumber")]
            public int LegSequenceNumber { get; set; }

            [JsonProperty("rawMessageKey")]
            public string RawMessageKey { get; set; }

            [JsonProperty("agentID")]
            public string AgentID { get; set; }

            [JsonProperty("scannerMACAddress")]
            public string ScannerMACAddress { get; set; }

            [JsonProperty("appVersion")]
            public string AppVersion { get; set; }

            [JsonProperty("brsVersion")]
            public string BrsVersion { get; set; }

            [JsonProperty("remarks")]
            public string Remarks { get; set; }

            [JsonProperty("agentName")]
            public string AgentName { get; set; }

            [JsonProperty("containerNumber")]
            public string ContainerNumber { get; set; }

            [JsonProperty("destination")]
            public string Destination { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("isAdvanced")]
            public bool IsAdvanced { get; set; }

            [JsonProperty("isClaimAreaScan")]
            public bool IsClaimAreaScan { get; set; }
        }

        
}
