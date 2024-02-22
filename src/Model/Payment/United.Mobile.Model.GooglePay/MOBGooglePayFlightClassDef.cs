using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace United.Mobile.Model.GooglePay
{
    [Serializable]
    public class MOBGooglePayFlightClassDef
    {

        //private string kind;
        //private string id;
        //public long version;
        //private string issuerName;
        //private string reviewStatus;
        //private string localScheduledDepartureDateTime;
        //private Flightheader flightHeader;
        //private Origin origin;
        //private Destination destination;
        //private string localBoardingDateTime;
        //private BoardingAndSeatingPolicy boardingAndSeatingPolicy;
        //private string hexBackgroundColor;
        ////private string localEstimatedOrActualDepartureDateTime;
        //private string localScheduledArrivalDateTime;
        ////private string localEstimatedOrActualArrivalDateTime;

       // private string kind;
        public string kind { get; set; }
        public string id { get; set; }
        public string issuerName { get; set; }
        public string reviewStatus { get; set; }
        public string localScheduledDepartureDateTime { get; set; }
        public Flightheader flightHeader { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public string localBoardingDateTime { get; set; }
        public BoardingAndSeatingPolicy boardingAndSeatingPolicy { get; set; }
        public string hexBackgroundColor { get; set; }
        //public string LocalEstimatedOrActualDepartureDateTime
        //{
        //    get { return localEstimatedOrActualDepartureDateTime; }
        //    set { localEstimatedOrActualDepartureDateTime = value; }
        //}
        public string localScheduledArrivalDateTime { get; set; }
        //public string LocalEstimatedOrActualArrivalDateTime
        //{
        //    get { return localEstimatedOrActualArrivalDateTime; }
        //    set { localEstimatedOrActualArrivalDateTime = value; }
        //}
        public MOBGooglePayFlightClassDef()
        {
            this.kind = "walletobjects#flightClass";
            this.issuerName = "united-207920";
            ////this.version = "v1";
            this.reviewStatus = "underReview";

            this.flightHeader = new Flightheader();
            this.flightHeader.kind = "walletobjects#flightHeader";
            this.flightHeader.carrier = new Carrier();
            this.flightHeader.carrier.kind = "walletobjects#flightCarrier";
            this.flightHeader.carrier.airlineName = new Airlinename();
            this.flightHeader.carrier.airlineName.kind = "walletobjects#localizedString";
            //Translatedvalue translatedvalue2 = new Translatedvalue();
            //translatedvalue2.Kind = "walletobjects#translatedString";
            //translatedvalue2.Language = "en-US";
            //translatedvalue2.Value = "United Airlines";
            //this.flightHeader.Carrier.AirlineName.TranslatedValues = new List<Translatedvalue>();
            //this.flightHeader.Carrier.AirlineName.TranslatedValues.Add(translatedvalue2);
            this.flightHeader.carrier.airlineName.defaultValue = new Defaultvalue();
            this.flightHeader.carrier.airlineName.defaultValue.kind = "walletobjects#translatedString";
            this.flightHeader.carrier.airlineName.defaultValue.language = "en-US";
            this.flightHeader.carrier.airlineName.defaultValue.value = "United Airlines"; //2

            this.flightHeader.carrier.airlineLogo = new Airlinelogo();
            this.flightHeader.carrier.airlineLogo.kind = "walletobjects#image";
            this.flightHeader.carrier.airlineLogo.sourceUri = new Sourceuri();
            this.flightHeader.carrier.airlineLogo.sourceUri.kind = "walletobjects#uri";
            this.flightHeader.carrier.airlineLogo.sourceUri.description = "United Airlines Logo";
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription = new Localizeddescription();
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription.kind = "walletobjects#localizedString";
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription.defaultValue = new Defaultvalue();
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription.defaultValue.kind = "walletobjects#translatedString";
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription.defaultValue.value = "United Airlines Logo";
            this.flightHeader.carrier.airlineLogo.sourceUri.localizedDescription.defaultValue.language = "en-US";
            //Translatedvalue translatedvalue3 = new Translatedvalue();
            //translatedvalue3.Kind = "walletobjects#translatedString";
            //translatedvalue3.Language = "en-US";
            //translatedvalue3.Value = "United Airlines";
            //this.flightHeader.carrier.AirlineLogo.SourceUri.LocalizedDescription.TranslatedValues = new List<Translatedvalue>();
            //this.flightHeader.carrier.AirlineLogo.SourceUri.LocalizedDescription.TranslatedValues.Add(translatedvalue3);

            this.flightHeader.carrier.airlineAllianceLogo = new Airlinealliancelogo();
            this.flightHeader.carrier.airlineAllianceLogo.kind = "walletobjects#image";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri = new Sourceuri();
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.kind = "walletobjects#uri";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.description = "Star Alliance Logo";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription = new Localizeddescription();
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription.kind = "walletobjects#localizedString";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription.defaultValue = new Defaultvalue();
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription.defaultValue.kind = "walletobjects#translatedString";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription.defaultValue.value = "Star Alliance Logo";
            this.flightHeader.carrier.airlineAllianceLogo.sourceUri.localizedDescription.defaultValue.language = "en-US";
            //Translatedvalue translatedvalue4 = new Translatedvalue();
            //translatedvalue4.Kind = "walletobjects#translatedString";
            //translatedvalue4.Language = "en-US";
            //translatedvalue4.Value = "United Airlines";
            //this.flightHeader.Carrier.airlineAllianceLogo.SourceUri.LocalizedDescription.TranslatedValues = new List<Translatedvalue>();
            //this.flightHeader.Carrier.airlineAllianceLogo.SourceUri.LocalizedDescription.TranslatedValues.Add(translatedvalue4);
            //this.FlightHeader.Carrier.AirlineAlliance = "starAlliance";
            this.boardingAndSeatingPolicy = new BoardingAndSeatingPolicy();
            this.boardingAndSeatingPolicy.kind = "walletobjects#boardingAndSeatingPolicy";
            this.boardingAndSeatingPolicy.boardingPolicy = "groupBased";
            this.boardingAndSeatingPolicy.seatClassPolicy = "cabinBased";

            this.origin = new Origin();
            this.origin.kind = "walletobjects#airportInfo";
            this.destination = new Destination();
            this.destination.kind = "walletobjects#airportInfo";

            this.flightHeader.operatingCarrier = new Operatingcarrier();
            this.flightHeader.operatingCarrier.kind = "walletobjects#flightCarrier";
            this.flightHeader.operatingCarrier.airlineName = new Airlinename();
            this.flightHeader.operatingCarrier.airlineName.kind = "walletobjects#localizedString";
            this.flightHeader.operatingCarrier.airlineName.defaultValue = new Defaultvalue();
            this.flightHeader.operatingCarrier.airlineName.defaultValue.kind = "walletobjects#translatedString";
            this.flightHeader.operatingCarrier.airlineName.defaultValue.language = "en-US";
            //this.FlightHeader.Carrier.AirlineName.DefaultValue.Value = "United Airlines"; //2
        }
    }
    [Serializable]
    public class MOBGooglePayFlightObjectDef
    {

        public string kind { get; set; }
        public string id { get; set; }
        public string classId { get; set; }
        public long version { get; set; }
        public string state { get; set; }
        public string passengerName { get; set; }
        public Boardingandseatinginfo boardingAndSeatingInfo { get; set; }
        public Reservationinfo reservationInfo { get; set; }
        public Securityprogramlogo securityProgramLogo { get; set; }
        public Barcode barcode { get; set; }
        public List<Textmodulesdata> textModulesData { get; set; }
        public Linksmoduledata linksModuleData { get; set; }
        public List<Imagemodulesdata> imageModulesData { get; set; }

        public MOBGooglePayFlightObjectDef()

        {
            this.kind = "walletobjects#flightObject";

            this.version = 1;
            this.state = "active";

            this.boardingAndSeatingInfo = new Boardingandseatinginfo();
            this.boardingAndSeatingInfo.kind = "walletobjects#boardingAndSeatingInfo";
            this.boardingAndSeatingInfo.boardingPrivilegeImage = new Boardingprivilegeimage();
            this.boardingAndSeatingInfo.boardingPrivilegeImage.kind = "walletobjects#image";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri = new Sourceuri();
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.kind = "walletobjects#uri";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.description = "United Privilege Image";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription = new Localizeddescription();
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription.kind = "walletobjects#localizedString";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription.defaultValue = new Defaultvalue();
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription.defaultValue.kind = "walletobjects#translatedString";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription.defaultValue.value = "United Privilege Image";
            this.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.localizedDescription.defaultValue.language = "en-US";
            //Translatedvalue translatedvalue = new Translatedvalue();
            //translatedvalue.Kind = "walletobjects#translatedString";
            //translatedvalue.Language = "en-US";
            //translatedvalue.Value = "United Privilege Image";
            //this.BoardingAndSeatingInfo.BoardingPrivilegeImage.SourceUri.LocalizedDescription.TranslatedValues = new List<Translatedvalue>();
            //this.BoardingAndSeatingInfo.BoardingPrivilegeImage.SourceUri.LocalizedDescription.TranslatedValues.Add(translatedvalue);

            this.reservationInfo = new Reservationinfo();
            this.reservationInfo.kind = "walletobjects#reservationInfo";
            this.reservationInfo.frequentFlyerInfo = new Frequentflyerinfo();
            this.reservationInfo.frequentFlyerInfo.kind = "walletobjects#frequentFlyerInfo";
            this.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName = new Frequentflyerprogramname();
            this.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName.kind = "walletobjects#localizedString";
            this.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName.defaultValue = new Defaultvalue();
            this.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName.defaultValue.kind = "walletobjects#translatedString";
            this.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName.defaultValue.language = "en-US";

            this.barcode = new Barcode();
            this.barcode.kind = "walletobjects#barcode";
            this.barcode.type = "aztec";

            this.securityProgramLogo = new Securityprogramlogo();
            this.securityProgramLogo.kind = "walletobjects#image";
            this.securityProgramLogo.sourceUri = new Sourceuri();
            this.securityProgramLogo.sourceUri.kind = "walletobjects#uri";
            this.securityProgramLogo.sourceUri.description = "Security Program Logo";
            this.securityProgramLogo.sourceUri.localizedDescription = new Localizeddescription();
            this.securityProgramLogo.sourceUri.localizedDescription.kind = "walletobjects#localizedString";
            this.securityProgramLogo.sourceUri.localizedDescription.defaultValue = new Defaultvalue();
            this.securityProgramLogo.sourceUri.localizedDescription.defaultValue.kind = "walletobjects#translatedString";
            this.securityProgramLogo.sourceUri.localizedDescription.defaultValue.value = "Security Program Logo";
            this.securityProgramLogo.sourceUri.localizedDescription.defaultValue.language = "en-US";
            //Translatedvalue translatedvalue5 = new Translatedvalue();
            //translatedvalue5.Kind = "walletobjects#translatedString";
            //translatedvalue5.Language = "en-US";
            //translatedvalue5.Value = "United Privilege Image";
            //this.SecurityProgramLogo.SourceUri.LocalizedDescription.TranslatedValues = new List<Translatedvalue>();
            //this.SecurityProgramLogo.SourceUri.LocalizedDescription.TranslatedValues.Add(translatedvalue5);            
            this.linksModuleData = new Linksmoduledata();
            Localizeddescription1 localizeddescription0 = new Localizeddescription1();
            localizeddescription0.kind = "walletobjects#localizedString";
            localizeddescription0.defaultValue = new Defaultvalue();
            localizeddescription0.defaultValue.kind = "walletobjects#translatedString";
            localizeddescription0.defaultValue.value = "www.united.com";
            localizeddescription0.defaultValue.language = "en-US";
            Uris uris0 = new Uris();
            uris0.kind = "walletobjects#uri";
            uris0.uri = "https://www.united.com";
            uris0.description = "Website";
            uris0.localizedDescription = localizeddescription0;

            Localizeddescription1 localizeddescription1 = new Localizeddescription1();
            localizeddescription1.kind = "walletobjects#localizedString";
            localizeddescription1.defaultValue = new Defaultvalue();
            localizeddescription1.defaultValue.kind = "walletobjects#translatedString";
            localizeddescription1.defaultValue.value = "Help";
            localizeddescription1.defaultValue.language = "en-US";
            Uris uris1 = new Uris();
            uris1.kind = "walletobjects#uri";
            uris1.uri = "https://www.united.com/ual/en/us/fly/help.html";
            uris1.description = "Help";
            uris1.localizedDescription = localizeddescription1;

            Localizeddescription1 localizeddescription2 = new Localizeddescription1();
            localizeddescription2.kind = "walletobjects#localizedString";
            localizeddescription2.defaultValue = new Defaultvalue();
            localizeddescription2.defaultValue.kind = "walletobjects#translatedString";
            localizeddescription2.defaultValue.value = "(800) 864-8331";
            localizeddescription2.defaultValue.language = "en-US";
            Uris uris2 = new Uris();
            uris2.kind = "walletobjects#uri";
            uris2.uri = "tel:8008648331";
            uris2.description = "Telephone";
            uris2.localizedDescription = localizeddescription2;
            this.linksModuleData.uris = new List<Uris>();
            this.linksModuleData.uris.Add(uris0);
            this.linksModuleData.uris.Add(uris1);
            this.linksModuleData.uris.Add(uris2);

            Localizeddescription localizeddescription3 = new Localizeddescription();
            localizeddescription3.kind = "walletobjects#localizedString";
            localizeddescription3.defaultValue = new Defaultvalue();
            localizeddescription3.defaultValue.kind = "walletobjects#translatedString";
            localizeddescription3.defaultValue.value = "Main Image";
            localizeddescription3.defaultValue.language = "en-US";
            Mainimage mainImage = new Mainimage();
            mainImage.kind = "walletobjects#image";
            mainImage.sourceUri = new Sourceuri();
            mainImage.sourceUri.kind = "walletobjects#uri";
            mainImage.sourceUri.uri = "https://smartphone.united.com/UnitedVendorImages/main_image.png";
            mainImage.sourceUri.description = "Main Image";
            mainImage.sourceUri.localizedDescription = localizeddescription3;
            Imagemodulesdata imagemodulesdata = new Imagemodulesdata();
            imagemodulesdata.mainImage = mainImage;
            this.imageModulesData = new List<Imagemodulesdata>();
            this.imageModulesData.Add(imagemodulesdata);
        }
    }

    [Serializable]
    public class Imagemodulesdata
    {
        public Mainimage mainImage { get; set; }

    }

    [Serializable]
    public class Mainimage
    {
        public string kind { get; set; }
        public Sourceuri sourceUri { get; set; }
    }

    [Serializable]
    public class Linksmoduledata
    {
        public List<Uris> uris { get; set; }

    }

    [Serializable]
    public class Uris
    {
        public string kind { get; set; }
        public string uri { get; set; }
        public string description { get; set; }
        public Localizeddescription1 localizedDescription { get; set; }
    }

    [Serializable]
    public class Textmodulesdata
    {
        public string header { get; set; }
        public string body { get; set; }
    }

    [Serializable]
    public class Securityprogramlogo
    {
        public string kind { get; set; }
        public Sourceuri sourceUri { get;set; }
    }

    [Serializable]
    public class Flightheader
    {
        public string kind { get; set; }
        public Carrier carrier { get; set; }
        public string flightNumber { get; set; }
        public Operatingcarrier operatingCarrier { get; set; }
        public string operatingFlightNumber { get; set; }

    }
    [Serializable]
    public class Operatingcarrier
    {
        public string kind { get; set; }
        public string carrierIataCode { get; set; }
        public Airlinename airlineName { get; set; }
        //private Airlinelogo airlineLogo;
        //private Airlinealliancelogo airlineAllianceLogo;
        //private string airlineAlliance;

        //public string Kind
        //{
        //    get { return kind; }
        //    set { kind = value; }
        //}
        //public string CarrierIataCode
        //{
        //    get { return carrierIataCode; }
        //    set { carrierIataCode = value; }
        //}
        //public Airlinename AirlineName
        //{
        //    get { return airlineName; }
        //    set { airlineName = value; }
        //}
        //public Airlinelogo AirlineLogo
        //{
        //    get { return airlineLogo; }
        //    set { airlineLogo = value; }
        //}
        //public Airlinealliancelogo AirlineAllianceLogo
        //{
        //    get { return airlineAllianceLogo; }
        //    set { airlineAllianceLogo = value; }
        //}
        //public string AirlineAlliance
        //{
        //    get { return airlineAlliance; }
        //    set { airlineAlliance = value; }
        //}
    }
    [Serializable]
    public class BoardingAndSeatingPolicy
    {
        public string kind { get; set; }
        public string boardingPolicy { get; set; }
        //private string seatingPolicy;
        public string seatClassPolicy { get; set; }
    }
    [Serializable]
    public class Barcode
    {
        public string kind { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string alternateText { get; set; }
    }
    [Serializable]
    public class Carrier
    {
        public string kind { get; set; }
        public string carrierIataCode { get; set; }
        public Airlinename airlineName { get; set; }
        public Airlinelogo airlineLogo { get; set; }
        public Airlinealliancelogo airlineAllianceLogo { get; set; }
        //private string airlineAlliance;

        //public string Kind
        //{
        //    get { return kind; }
        //    set { kind = value; }
        //}
        //public string CarrierIataCode
        //{
        //    get { return carrierIataCode; }
        //    set { carrierIataCode = value; }
        //}
        //public Airlinename AirlineName
        //{
        //    get { return airlineName; }
        //    set { airlineName = value; }
        //}
        //public Airlinelogo AirlineLogo
        //{
        //    get { return airlineLogo; }
        //    set { airlineLogo = value; }
        //}
        //public Airlinealliancelogo AirlineAllianceLogo
        //{
        //    get { return airlineAllianceLogo; }
        //    set { airlineAllianceLogo = value; }
        //}
        ////public string AirlineAlliance
        ////{
        ////    get { return airlineAlliance; }
        ////    set { airlineAlliance = value; }
        ////}
    }
    [Serializable]
    public class Airlinename
    {
        public string kind { get; set; }
        public List<Translatedvalue> translatedValues { get; set; }
        public Defaultvalue defaultValue { get; set; }

        //public string Kind
        //{
        //    get { return kind; }
        //    set { kind = value; }
        //}
        //public List<Translatedvalue> TranslatedValues
        //{
        //    get { return translatedValues; }
        //    set { translatedValues = value; }
        //}
        //public Defaultvalue DefaultValue
        //{
        //    get { return defaultValue; }
        //    set { defaultValue = value; }
        //}
    }
    [Serializable]
    public class Defaultvalue
    {
        public string kind { get; set; }
        public string language { get; set; }
        public string value { get; set; }
    }
    [Serializable]
    public class Translatedvalue
    {
        public string kind { get; set; }
        public string language { get; set; }
        public string value { get; set; }
    }
    [Serializable]
    public class Airlinelogo
    {
        public string kind { get; set; }
        public Sourceuri sourceUri { get; set; }

        //public string Kind
        //{
        //    get { return kind; }
        //    set { kind = value; }
        //}
        //public Sourceuri SourceUri
        //{
        //    get { return sourceUri; }
        //    set { sourceUri = value; }
        //}
    }
    [Serializable]
    public class Sourceuri
    {
        public string kind { get; set; }
        public string uri { get; set; }
        public string description { get; set; }
        public Localizeddescription localizedDescription { get; set; }
    }
    [Serializable]
    public class Localizeddescription
    {
        public string kind { get; set; }
        //private List<Translatedvalue> translatedValues;
        public Defaultvalue defaultValue { get; set; }
    }
    [Serializable]
    public class Localizeddescription1
    {
        public string kind { get; set; }
        //private List<Translatedvalue> translatedValues;
        public Defaultvalue defaultValue { get; set; }
    }
    [Serializable]
    public class Airlinealliancelogo
    {
        public string kind { get; set; }
        public Sourceuri sourceUri { get; set; }
    }
    [Serializable]
    public class Origin
    {
        public string kind { get; set; }
        public string airportIataCode { get; set; }
        //private string terminal;
        public string gate { get; set; }

    }
    [Serializable]
    public class Destination
    {
        public string kind { get; set; }
        public string airportIataCode { get; set; }
    }
    [Serializable]
    public class Boardingandseatinginfo
    {
        public string kind { get; set; }
        public string boardingGroup { get; set; }
        public string seatNumber { get; set; }
        //private string boardingPosition;
        public string sequenceNumber { get; set; }
        public string seatClass { get; set; }
        public Boardingprivilegeimage boardingPrivilegeImage { get; set; }
    }
    [Serializable]
    public class Boardingprivilegeimage
    {
        public string kind { get; set; }
        public Sourceuri sourceUri { get; set; }
    }
    [Serializable]
    public class Reservationinfo
    {
        public string kind { get; set; }
        public string confirmationCode { get; set; }
        //private string eticketNumber;
        public Frequentflyerinfo frequentFlyerInfo { get; set; }
    }
    [Serializable]
    public class Frequentflyerinfo
    {
        public string kind { get; set; }
        public Frequentflyerprogramname frequentFlyerProgramName { get; set; }
        public string frequentFlyerNumber { get; set; }
    }
    [Serializable]
    public class Frequentflyerprogramname
    {
        public string kind { get; set; }
        public List<Translatedvalue> translatedValues { get; set; }
        public Defaultvalue defaultValue { get; set; }
    }
}
