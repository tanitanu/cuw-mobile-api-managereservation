using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DAL.CSLModel
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel", IsNullable = false)]
    public partial class ArrayOfOperationalRoute
    {

        private ArrayOfOperationalRouteOperationalRoute[] operationalRouteField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OperationalRoute")]
        public ArrayOfOperationalRouteOperationalRoute[] OperationalRoute
        {
            get
            {
                return this.operationalRouteField;
            }
            set
            {
                this.operationalRouteField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel")]
    public partial class ArrayOfOperationalRouteOperationalRoute
    {

        private Error[] errorField;

        private ArrayOfOperationalRouteOperationalRouteFlight flightField;

        private Link[] linksField;

        private OperationalFlightSegment[] operationalFlightSegmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.E" +
            "xceptionModel", IsNullable = false)]
        public Error[] Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        public ArrayOfOperationalRouteOperationalRouteFlight Flight
        {
            get
            {
                return this.flightField;
            }
            set
            {
                this.flightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("OperationalFlightSegment", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel", IsNullable = false)]
        public OperationalFlightSegment[] OperationalFlightSegments
        {
            get
            {
                return this.operationalFlightSegmentsField;
            }
            set
            {
                this.operationalFlightSegmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel")]
    public partial class ArrayOfOperationalRouteOperationalRouteFlight
    {

        private string arrivalAirportField;

        private string arrivalAirportNameField;

        private string arrivalDateField;

        private string carrierCodeField;

        private string departureAirportField;

        private string departureAirportNameField;

        private string departureDateField;

        private string flightNumberField;

        private string flightOriginationDateField;

        private string isInternationalField;

        private LinksLink[] linksField;

        private string marketingCarrierIndicatorField;

        private string operatingCarrierCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportNameField;
            }
            set
            {
                this.arrivalAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CarrierCode
        {
            get
            {
                return this.carrierCodeField;
            }
            set
            {
                this.carrierCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportNameField;
            }
            set
            {
                this.departureAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureDate
        {
            get
            {
                return this.departureDateField;
            }
            set
            {
                this.departureDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightOriginationDate
        {
            get
            {
                return this.flightOriginationDateField;
            }
            set
            {
                this.flightOriginationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string MarketingCarrierIndicator
        {
            get
            {
                return this.marketingCarrierIndicatorField;
            }
            set
            {
                this.marketingCarrierIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCodeField;
            }
            set
            {
                this.operatingCarrierCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel", IsNullable = false)]
    public partial class OperationalRoute
    {

        private Error[] errorField;

        private OperationalRouteFlight flightField;

        private Link[] linksField;

        private OperationalFlightSegment[] operationalFlightSegmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.E" +
            "xceptionModel", IsNullable = false)]
        public Error[] Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        public OperationalRouteFlight Flight
        {
            get
            {
                return this.flightField;
            }
            set
            {
                this.flightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("OperationalFlightSegment", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel", IsNullable = false)]
        public OperationalFlightSegment[] OperationalFlightSegments
        {
            get
            {
                return this.operationalFlightSegmentsField;
            }
            set
            {
                this.operationalFlightSegmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.E" +
        "xceptionModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.E" +
        "xceptionModel", IsNullable = false)]
    public partial class Error
    {

        private string codeField;

        private string errorTypeField;

        private Link[] linksField;

        private string textField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string ErrorType
        {
            get
            {
                return this.errorTypeField;
            }
            set
            {
                this.errorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Link
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Amenity
    {

        private double amountField;

        private string cabinField;

        private string descriptionField;

        private AmenityLink[] linksField;

        private string nameField;

        private AmenityStatus statusField;

        private AmenityType typeField;

        private string[] valueField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Cabin
        {
            get
            {
                return this.cabinField;
            }
            set
            {
                this.cabinField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AmenityLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AmenityStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public AmenityType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public string[] Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AmenityStatusLink[] linksField;

        private AmenityStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AmenityStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AmenityStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AmenityStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AmenityStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AmenityTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AmenityTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Characteristic
    {

        private string codeField;

        private string descriptionField;

        private CharacteristicGenre genreField;

        private CharacteristicLink[] linksField;

        private CharacteristicStatus statusField;

        private string valueField;

        private CharacteristicCharacteristic[] characteristic1Field;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public CharacteristicGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CharacteristicStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Characteristic")]
        public CharacteristicCharacteristic[] Characteristic1
        {
            get
            {
                return this.characteristic1Field;
            }
            set
            {
                this.characteristic1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicStatusLink[] linksField;

        private CharacteristicStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CharacteristicStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristic
    {

        private string codeField;

        private string descriptionField;

        private CharacteristicCharacteristicGenre genreField;

        private CharacteristicCharacteristicLink[] linksField;

        private CharacteristicCharacteristicStatus statusField;

        private string valueField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public CharacteristicCharacteristicGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicCharacteristicLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CharacteristicCharacteristicStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicCharacteristicGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicCharacteristicGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicCharacteristicStatusLink[] linksField;

        private CharacteristicCharacteristicStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicCharacteristicStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CharacteristicCharacteristicStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CharacteristicCharacteristicStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CharacteristicCharacteristicStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CharacteristicCharacteristicStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class BookingClass
    {

        private BookingClassCabin cabinField;

        private string codeField;

        private BookingClassCounts countsField;

        private BookingClassLink[] linksField;

        private BookingClassStatus statusField;

        /// <remarks/>
        public BookingClassCabin Cabin
        {
            get
            {
                return this.cabinField;
            }
            set
            {
                this.cabinField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public BookingClassCounts Counts
        {
            get
            {
                return this.countsField;
            }
            set
            {
                this.countsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BookingClassLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BookingClassStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassCabin
    {

        private uint columnCountField;

        private string descriptionField;

        private string isUpperDeckField;

        private string keyField;

        private string layoutField;

        private Link[] linksField;

        private string nameField;

        private uint rowCountField;

        private SeatRowsSeatRow[] seatRowsField;

        private string statusField;

        private uint totalSeatsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint ColumnCount
        {
            get
            {
                return this.columnCountField;
            }
            set
            {
                this.columnCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsUpperDeck
        {
            get
            {
                return this.isUpperDeckField;
            }
            set
            {
                this.isUpperDeckField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint RowCount
        {
            get
            {
                return this.rowCountField;
            }
            set
            {
                this.rowCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("SeatRow", IsNullable = false)]
        public SeatRowsSeatRow[] SeatRows
        {
            get
            {
                return this.seatRowsField;
            }
            set
            {
                this.seatRowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint TotalSeats
        {
            get
            {
                return this.totalSeatsField;
            }
            set
            {
                this.totalSeatsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class SeatRowsSeatRow
    {

        private string cabinDescriptionField;

        private Characteristic[] characteristicsField;

        private string genreField;

        private Link[] linksField;

        private uint rowNumberField;

        private SeatRowsSeatRowSeat[] seatsField;

        private string statusField;

        /// <remarks/>
        public string CabinDescription
        {
            get
            {
                return this.cabinDescriptionField;
            }
            set
            {
                this.cabinDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public uint RowNumber
        {
            get
            {
                return this.rowNumberField;
            }
            set
            {
                this.rowNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Seat", IsNullable = false)]
        public SeatRowsSeatRowSeat[] Seats
        {
            get
            {
                return this.seatsField;
            }
            set
            {
                this.seatsField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class SeatRowsSeatRowSeat
    {

        private Characteristic[] characteristicsField;

        private string descriptionField;

        private string identifierField;

        private Link[] linksField;

        private SeatRowsSeatRowSeatPrice priceField;

        private string seatClassField;

        private string seatTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public SeatRowsSeatRowSeatPrice Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public string SeatClass
        {
            get
            {
                return this.seatClassField;
            }
            set
            {
                this.seatClassField = value;
            }
        }

        /// <remarks/>
        public string SeatType
        {
            get
            {
                return this.seatTypeField;
            }
            set
            {
                this.seatTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class SeatRowsSeatRowSeatPrice
    {

        private BasePrice basePriceField;

        private BasePriceEquivalent basePriceEquivalentField;

        private FeesCharge[] feesField;

        private LinksLink[] linksField;

        private string promotionCodeField;

        private TaxesCharge[] taxesField;

        private TotalsCharge[] totalsField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public BasePrice BasePrice
        {
            get
            {
                return this.basePriceField;
            }
            set
            {
                this.basePriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public BasePriceEquivalent BasePriceEquivalent
        {
            get
            {
                return this.basePriceEquivalentField;
            }
            set
            {
                this.basePriceEquivalentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public FeesCharge[] Fees
        {
            get
            {
                return this.feesField;
            }
            set
            {
                this.feesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PromotionCode
        {
            get
            {
                return this.promotionCodeField;
            }
            set
            {
                this.promotionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public TaxesCharge[] Taxes
        {
            get
            {
                return this.taxesField;
            }
            set
            {
                this.taxesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public TotalsCharge[] Totals
        {
            get
            {
                return this.totalsField;
            }
            set
            {
                this.totalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class BasePrice
    {

        private double amountField;

        private string codeField;

        private BasePriceCombinationCharge[] combinationsField;

        private BasePriceCurrency currencyField;

        private string descriptionField;

        private string isCombinationField;

        private BasePriceLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CombinationCharge", IsNullable = false)]
        public BasePriceCombinationCharge[] Combinations
        {
            get
            {
                return this.combinationsField;
            }
            set
            {
                this.combinationsField = value;
            }
        }

        /// <remarks/>
        public BasePriceCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsCombination
        {
            get
            {
                return this.isCombinationField;
            }
            set
            {
                this.isCombinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationCharge
    {

        private double amountField;

        private string codeField;

        private BasePriceCombinationChargeCurrency currencyField;

        private string descriptionField;

        private BasePriceCombinationChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public BasePriceCombinationChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCombinationChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private BasePriceCombinationChargeCurrencyLink[] linksField;

        private string nameField;

        private BasePriceCombinationChargeCurrencyStatus statusField;

        private BasePriceCombinationChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCombinationChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public BasePriceCombinationChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public BasePriceCombinationChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCombinationChargeCurrencyStatusLink[] linksField;

        private BasePriceCombinationChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCombinationChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BasePriceCombinationChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCombinationChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCombinationChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCombinationChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCombinationChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCombinationChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private BasePriceCurrencyLink[] linksField;

        private string nameField;

        private BasePriceCurrencyStatus statusField;

        private BasePriceCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public BasePriceCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public BasePriceCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCurrencyStatusLink[] linksField;

        private BasePriceCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BasePriceCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class BasePriceEquivalent
    {

        private double amountField;

        private string codeField;

        private BasePriceEquivalentCombinationCharge[] combinationsField;

        private BasePriceEquivalentCurrency currencyField;

        private string descriptionField;

        private string isCombinationField;

        private BasePriceEquivalentLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CombinationCharge", IsNullable = false)]
        public BasePriceEquivalentCombinationCharge[] Combinations
        {
            get
            {
                return this.combinationsField;
            }
            set
            {
                this.combinationsField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsCombination
        {
            get
            {
                return this.isCombinationField;
            }
            set
            {
                this.isCombinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationCharge
    {

        private double amountField;

        private string codeField;

        private BasePriceEquivalentCombinationChargeCurrency currencyField;

        private string descriptionField;

        private BasePriceEquivalentCombinationChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCombinationChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCombinationChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private BasePriceEquivalentCombinationChargeCurrencyLink[] linksField;

        private string nameField;

        private BasePriceEquivalentCombinationChargeCurrencyStatus statusField;

        private BasePriceEquivalentCombinationChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCombinationChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCombinationChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCombinationChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCombinationChargeCurrencyStatusLink[] linksField;

        private BasePriceEquivalentCombinationChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCombinationChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCombinationChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCombinationChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCombinationChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCombinationChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCombinationChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCombinationChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private BasePriceEquivalentCurrencyLink[] linksField;

        private string nameField;

        private BasePriceEquivalentCurrencyStatus statusField;

        private BasePriceEquivalentCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCurrencyStatusLink[] linksField;

        private BasePriceEquivalentCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BasePriceEquivalentCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BasePriceEquivalentCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BasePriceEquivalentCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BasePriceEquivalentLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesCharge
    {

        private double amountField;

        private string codeField;

        private FeesChargeCombinationCharge[] combinationsField;

        private FeesChargeCurrency currencyField;

        private string descriptionField;

        private string isCombinationField;

        private FeesChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CombinationCharge", IsNullable = false)]
        public FeesChargeCombinationCharge[] Combinations
        {
            get
            {
                return this.combinationsField;
            }
            set
            {
                this.combinationsField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsCombination
        {
            get
            {
                return this.isCombinationField;
            }
            set
            {
                this.isCombinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationCharge
    {

        private double amountField;

        private string codeField;

        private FeesChargeCombinationChargeCurrency currencyField;

        private string descriptionField;

        private FeesChargeCombinationChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCombinationChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCombinationChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private FeesChargeCombinationChargeCurrencyLink[] linksField;

        private string nameField;

        private FeesChargeCombinationChargeCurrencyStatus statusField;

        private FeesChargeCombinationChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCombinationChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCombinationChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCombinationChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCombinationChargeCurrencyStatusLink[] linksField;

        private FeesChargeCombinationChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCombinationChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCombinationChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCombinationChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCombinationChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCombinationChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCombinationChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCombinationChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private FeesChargeCurrencyLink[] linksField;

        private string nameField;

        private FeesChargeCurrencyStatus statusField;

        private FeesChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCurrencyStatusLink[] linksField;

        private FeesChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public FeesChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private FeesChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public FeesChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class FeesChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class LinksLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesCharge
    {

        private double amountField;

        private string codeField;

        private TaxesChargeCombinationCharge[] combinationsField;

        private TaxesChargeCurrency currencyField;

        private string descriptionField;

        private string isCombinationField;

        private TaxesChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CombinationCharge", IsNullable = false)]
        public TaxesChargeCombinationCharge[] Combinations
        {
            get
            {
                return this.combinationsField;
            }
            set
            {
                this.combinationsField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsCombination
        {
            get
            {
                return this.isCombinationField;
            }
            set
            {
                this.isCombinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationCharge
    {

        private double amountField;

        private string codeField;

        private TaxesChargeCombinationChargeCurrency currencyField;

        private string descriptionField;

        private TaxesChargeCombinationChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCombinationChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCombinationChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private TaxesChargeCombinationChargeCurrencyLink[] linksField;

        private string nameField;

        private TaxesChargeCombinationChargeCurrencyStatus statusField;

        private TaxesChargeCombinationChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCombinationChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCombinationChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCombinationChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCombinationChargeCurrencyStatusLink[] linksField;

        private TaxesChargeCombinationChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCombinationChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCombinationChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCombinationChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCombinationChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCombinationChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCombinationChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCombinationChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private TaxesChargeCurrencyLink[] linksField;

        private string nameField;

        private TaxesChargeCurrencyStatus statusField;

        private TaxesChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCurrencyStatusLink[] linksField;

        private TaxesChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public TaxesChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TaxesChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TaxesChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TaxesChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsCharge
    {

        private double amountField;

        private string codeField;

        private TotalsChargeCombinationCharge[] combinationsField;

        private TotalsChargeCurrency currencyField;

        private string descriptionField;

        private string isCombinationField;

        private TotalsChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CombinationCharge", IsNullable = false)]
        public TotalsChargeCombinationCharge[] Combinations
        {
            get
            {
                return this.combinationsField;
            }
            set
            {
                this.combinationsField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsCombination
        {
            get
            {
                return this.isCombinationField;
            }
            set
            {
                this.isCombinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationCharge
    {

        private double amountField;

        private string codeField;

        private TotalsChargeCombinationChargeCurrency currencyField;

        private string descriptionField;

        private TotalsChargeCombinationChargeLink[] linksField;

        private string nameField;

        private string statusField;

        private string typeField;

        /// <remarks/>
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCombinationChargeCurrency Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCombinationChargeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private TotalsChargeCombinationChargeCurrencyLink[] linksField;

        private string nameField;

        private TotalsChargeCombinationChargeCurrencyStatus statusField;

        private TotalsChargeCombinationChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCombinationChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCombinationChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCombinationChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCombinationChargeCurrencyStatusLink[] linksField;

        private TotalsChargeCombinationChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCombinationChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCombinationChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCombinationChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCombinationChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCombinationChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCombinationChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCombinationChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private TotalsChargeCurrencyLink[] linksField;

        private string nameField;

        private TotalsChargeCurrencyStatus statusField;

        private TotalsChargeCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCurrencyStatusLink[] linksField;

        private TotalsChargeCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public TotalsChargeCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TotalsChargeCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TotalsChargeCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TotalsChargeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Type
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassCounts
    {

        private string countTypeField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BookingClassCountsLink[] linksField;

        private string statusField;

        private string valueField;

        /// <remarks/>
        public string CountType
        {
            get
            {
                return this.countTypeField;
            }
            set
            {
                this.countTypeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BookingClassCountsLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassCountsLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BookingClassStatusLink[] linksField;

        private BookingClassStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BookingClassStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public BookingClassStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private BookingClassStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public BookingClassStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class BookingClassStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class Airport
    {

        private AirportAddress addressField;

        private AirportAirportTerminals airportTerminalsField;

        private AirportGenre genreField;

        private string geocodeField;

        private AirportIATACityCode iATACityCodeField;

        private string iATACodeField;

        private AirportIATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private AirportStateProvince stateProvinceField;

        private AirportStatus statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        public AirportAddress Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public AirportAirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        public AirportGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public AirportIATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        public AirportIATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public AirportStateProvince StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        public AirportStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAddress
    {

        private string[] addressLinesField;

        private Characteristic characteristicField;

        private string cityField;

        private Country countryField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private Genre genreField;

        private string keyField;

        private LinksLink[] linksField;

        private string nameField;

        private string postalCodeField;

        private Region regionField;

        private StateProvince stateProvinceField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public string[] AddressLines
        {
            get
            {
                return this.addressLinesField;
            }
            set
            {
                this.addressLinesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Characteristic Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Country Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Genre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Region Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public StateProvince StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Country
    {

        private string countryCodeField;

        private CountryDefaultCurrency defaultCurrencyField;

        private CountryDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private CountryLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private CountryStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public CountryDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public CountryDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public CountryStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private CountryDefaultCurrencyLink[] linksField;

        private string nameField;

        private CountryDefaultCurrencyStatus statusField;

        private CountryDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public CountryDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public CountryDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryDefaultCurrencyStatusLink[] linksField;

        private CountryDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CountryDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private CountryDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryStatusLink[] linksField;

        private CountryStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CountryStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Genre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private GenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public GenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class GenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Region
    {

        private RegionCountry countryField;

        private RegionGenre genreField;

        private RegionLink[] linksField;

        private string nameField;

        private string regionCodeField;

        /// <remarks/>
        public RegionCountry Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public RegionGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string RegionCode
        {
            get
            {
                return this.regionCodeField;
            }
            set
            {
                this.regionCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountry
    {

        private string countryCodeField;

        private RegionCountryDefaultCurrency defaultCurrencyField;

        private RegionCountryDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private RegionCountryLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private RegionCountryStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public RegionCountryDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public RegionCountryDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public RegionCountryStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private RegionCountryDefaultCurrencyLink[] linksField;

        private string nameField;

        private RegionCountryDefaultCurrencyStatus statusField;

        private RegionCountryDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public RegionCountryDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public RegionCountryDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionCountryDefaultCurrencyStatusLink[] linksField;

        private RegionCountryDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public RegionCountryDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionCountryDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionCountryDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private RegionCountryDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionCountryStatusLink[] linksField;

        private RegionCountryStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public RegionCountryStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionCountryStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionCountryStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionCountryStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private RegionGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public RegionGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class RegionLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class StateProvince
    {

        private StateProvinceCountryCode countryCodeField;

        private StateProvinceLink[] linksField;

        private string nameField;

        private string shortNameField;

        private string stateProvinceCodeField;

        /// <remarks/>
        public StateProvinceCountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public string StateProvinceCode
        {
            get
            {
                return this.stateProvinceCodeField;
            }
            set
            {
                this.stateProvinceCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCode
    {

        private string countryCodeField;

        private StateProvinceCountryCodeDefaultCurrency defaultCurrencyField;

        private StateProvinceCountryCodeDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private StateProvinceCountryCodeLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private StateProvinceCountryCodeStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private StateProvinceCountryCodeDefaultCurrencyLink[] linksField;

        private string nameField;

        private StateProvinceCountryCodeDefaultCurrencyStatus statusField;

        private StateProvinceCountryCodeDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateProvinceCountryCodeDefaultCurrencyStatusLink[] linksField;

        private StateProvinceCountryCodeDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateProvinceCountryCodeDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateProvinceCountryCodeDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private StateProvinceCountryCodeDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateProvinceCountryCodeStatusLink[] linksField;

        private StateProvinceCountryCodeStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public StateProvinceCountryCodeStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateProvinceCountryCodeStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateProvinceCountryCodeStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceCountryCodeStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateProvinceLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Status
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StatusLink[] linksField;

        private StatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public StatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAirportTerminals
    {

        private AirportAirportTerminalsAirportGate[] airportGatesField;

        private Characteristic[] characteristicsField;

        private string geocodeField;

        private string keyField;

        private Link[] linksField;

        private string nameField;

        private AirportAirportTerminalsStatus statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AirportGate", IsNullable = false)]
        public AirportAirportTerminalsAirportGate[] AirportGates
        {
            get
            {
                return this.airportGatesField;
            }
            set
            {
                this.airportGatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AirportAirportTerminalsStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAirportTerminalsAirportGate
    {

        private Characteristic[] characteristicsField;

        private string gateBankField;

        private AirportAirportTerminalsAirportGateGenre genreField;

        private string geocodeField;

        private string identifierField;

        private string keyField;

        private Link[] linksField;

        private string operationalGateField;

        private AirportAirportTerminalsAirportGateStatus statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string GateBank
        {
            get
            {
                return this.gateBankField;
            }
            set
            {
                this.gateBankField = value;
            }
        }

        /// <remarks/>
        public AirportAirportTerminalsAirportGateGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public string Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string OperationalGate
        {
            get
            {
                return this.operationalGateField;
            }
            set
            {
                this.operationalGateField = value;
            }
        }

        /// <remarks/>
        public AirportAirportTerminalsAirportGateStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAirportTerminalsAirportGateGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAirportTerminalsAirportGateStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportAirportTerminalsStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportIATACityCode
    {

        private string cityCodeField;

        private string iSOAlpha3CodeField;

        private string latitudeField;

        private LinksLink[] linksField;

        private string longitudeField;

        private string nameField;

        private string phoneCityCodeField;

        private string shortNameField;

        private StateCode stateCodeField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CityCode
        {
            get
            {
                return this.cityCodeField;
            }
            set
            {
                this.cityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PhoneCityCode
        {
            get
            {
                return this.phoneCityCodeField;
            }
            set
            {
                this.phoneCityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public StateCode StateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class StateCode
    {

        private StateCodeCountryCode countryCodeField;

        private StateCodeLink[] linksField;

        private string nameField;

        private string shortNameField;

        private string stateProvinceCodeField;

        /// <remarks/>
        public StateCodeCountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public string StateProvinceCode
        {
            get
            {
                return this.stateProvinceCodeField;
            }
            set
            {
                this.stateProvinceCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCode
    {

        private string countryCodeField;

        private StateCodeCountryCodeDefaultCurrency defaultCurrencyField;

        private StateCodeCountryCodeDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private StateCodeCountryCodeLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private StateCodeCountryCodeStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private StateCodeCountryCodeDefaultCurrencyLink[] linksField;

        private string nameField;

        private StateCodeCountryCodeDefaultCurrencyStatus statusField;

        private StateCodeCountryCodeDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateCodeCountryCodeDefaultCurrencyStatusLink[] linksField;

        private StateCodeCountryCodeDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateCodeCountryCodeDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateCodeCountryCodeDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private StateCodeCountryCodeDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateCodeCountryCodeStatusLink[] linksField;

        private StateCodeCountryCodeStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public StateCodeCountryCodeStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private StateCodeCountryCodeStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public StateCodeCountryCodeStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeCountryCodeStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class StateCodeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportIATACountryCode
    {

        private CountryCode countryCodeField;

        private DefaultCurrency defaultCurrencyField;

        private DefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private LinksLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public CountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class CountryCode
    {

        private string countryCode1Field;

        private CountryCodeDefaultCurrency defaultCurrencyField;

        private CountryCodeDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private CountryCodeLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private CountryCodeStatus statusField;

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CountryCode")]
        public string CountryCode1
        {
            get
            {
                return this.countryCode1Field;
            }
            set
            {
                this.countryCode1Field = value;
            }
        }

        /// <remarks/>
        public CountryCodeDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public CountryCodeDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public CountryCodeStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private CountryCodeDefaultCurrencyLink[] linksField;

        private string nameField;

        private CountryCodeDefaultCurrencyStatus statusField;

        private CountryCodeDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public CountryCodeDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public CountryCodeDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryCodeDefaultCurrencyStatusLink[] linksField;

        private CountryCodeDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CountryCodeDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryCodeDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryCodeDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private CountryCodeDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryCodeStatusLink[] linksField;

        private CountryCodeStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CountryCodeStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountryCodeStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountryCodeStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountryCodeStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class DefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private DefaultCurrencyLink[] linksField;

        private string nameField;

        private DefaultCurrencyStatus statusField;

        private DefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public DefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public DefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public DefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private DefaultCurrencyStatusLink[] linksField;

        private DefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public DefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public DefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private DefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public DefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private DefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public DefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class DefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private DefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public DefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class DefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportStateProvince
    {

        private CountryCode countryCodeField;

        private LinksLink[] linksField;

        private string nameField;

        private string shortNameField;

        private string stateProvinceCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public CountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string StateProvinceCode
        {
            get
            {
                return this.stateProvinceCodeField;
            }
            set
            {
                this.stateProvinceCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Count
    {

        private string countTypeField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private CountLink[] linksField;

        private string statusField;

        private string valueField;

        /// <remarks/>
        public string CountType
        {
            get
            {
                return this.countTypeField;
            }
            set
            {
                this.countTypeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public CountLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class CountLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel", IsNullable = false)]
    public partial class OperationalFlightSegment
    {

        private Amenity[] amenitiesField;

        private OperationalFlightSegmentArrivalAirport arrivalAirportField;

        private string arrivalDateTimeField;

        private uint arrivalTimeZoneField;

        private string arrivalUTCDateTimeField;

        private string boardTimeField;

        private string boardUTCTimeField;

        private BookingClass[] bookingClassesField;

        private OperationalFlightSegmentChangeOfGauge[] changeOfGaugeField;

        private Characteristic[] characteristicField;

        private OperationalFlightSegmentDepartureAirport departureAirportField;

        private string departureDateTimeField;

        private string departureDayOfWeekField;

        private uint departureTimeZoneField;

        private string departureUTCDateTimeField;

        private string directionIndicatorField;

        private string discountAppliedField;

        private string displayIndicatorField;

        private uint distanceField;

        private OperationalFlightSegmentEquipment equipmentField;

        private string flightNumberField;

        private string flightSegmentTypeField;

        private OperationalFlightSegmentFlightStatus[] flightStatusesField;

        private string groundTimeField;

        private string isChangeOfGaugeField;

        private string isInternationalField;

        private string journeyDurationField;

        private OperationalFlightSegmentFlightLeg[] legsField;

        private Link[] linksField;

        private OperationalFlightSegmentMarketedFlightSegment[] marketedFlightSegmentField;

        private string marriageGroupField;

        private OperationalFlightSegmentMessage[] messageField;

        private uint numberofStopsField;

        private double onTimeRateField;

        private OperationalFlightSegmentOperatingAirline operatingAirlineField;

        private string operatingAirlineCodeField;

        private string operatingAirlineNameField;

        private OperationalFlightSegmentPerformance performanceField;

        private string scheduledFlightDurationField;

        private string scheduledFrequencyField;

        private uint segmentNumberField;

        private Airport[] stopLocationsField;

        private OperationalFlightSegmentTravelerCount[] travelerCountsField;

        private string actualArrivalTimeField;

        private string actualArrivalUTCTimeField;

        private string actualDepartureTimeField;

        private string actualDepartureUTCTimeField;

        private string actualEnrouteTimeField;

        private string actualTaxiInField;

        private string actualTaxiOutField;

        private string arrivalBagClaimUnitField;

        private string arrivalDelayMinutesField;

        private double arrivalFuelWeightField;

        private string arrivalGateField;

        private string arrivalTermimalField;

        private string bridgeOnTimeField;

        private string bridgeOnUTCTimeField;

        private string cancelDivertFlightNumberField;

        private string clearedFuelIndicatorField;

        private double clearedFuelWeightField;

        private string departureBagSectorField;

        private string departureDelayMinutesField;

        private double departureFuelWeightField;

        private string departureGateField;

        private string departureTerminalField;

        private string estimatedArrivalDelayMinutesField;

        private string estimatedArrivalTimeField;

        private string estimatedArrivalUTCTimeField;

        private string estimatedDepartureDelayMinutesField;

        private string estimatedDepartureTimeField;

        private string estimatedDepartureUTCTimeField;

        private string flightTypeField;

        private string inTimeField;

        private string inUTCTimeField;

        private OperationalFlightSegmentInboundFlightSegment inboundFlightSegmentField;

        private string offTimeField;

        private string offUTCTimeField;

        private string onTimeField;

        private string onUTCTimeField;

        private string outTimeField;

        private string outUTCTimeField;

        private OperationalFlightSegmentOutboundFlightSegment outboundFlightSegmentField;

        private string plannedEnrouteTimeField;

        private string plannedTaxiInField;

        private string plannedTaxiOutField;

        private OperationalFlightSegmentReasonStatus[] reasonStatusesField;

        private uint remainingMinutesToBoardField;

        private OperationalFlightSegmentShip shipField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Amenity", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Amenity[] Amenities
        {
            get
            {
                return this.amenitiesField;
            }
            set
            {
                this.amenitiesField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentArrivalAirport ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        public string ArrivalDateTime
        {
            get
            {
                return this.arrivalDateTimeField;
            }
            set
            {
                this.arrivalDateTimeField = value;
            }
        }

        /// <remarks/>
        public uint ArrivalTimeZone
        {
            get
            {
                return this.arrivalTimeZoneField;
            }
            set
            {
                this.arrivalTimeZoneField = value;
            }
        }

        /// <remarks/>
        public string ArrivalUTCDateTime
        {
            get
            {
                return this.arrivalUTCDateTimeField;
            }
            set
            {
                this.arrivalUTCDateTimeField = value;
            }
        }

        /// <remarks/>
        public string BoardTime
        {
            get
            {
                return this.boardTimeField;
            }
            set
            {
                this.boardTimeField = value;
            }
        }

        /// <remarks/>
        public string BoardUTCTime
        {
            get
            {
                return this.boardUTCTimeField;
            }
            set
            {
                this.boardUTCTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("BookingClass", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public BookingClass[] BookingClasses
        {
            get
            {
                return this.bookingClassesField;
            }
            set
            {
                this.bookingClassesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ChangeOfGauge", IsNullable = false)]
        public OperationalFlightSegmentChangeOfGauge[] ChangeOfGauge
        {
            get
            {
                return this.changeOfGaugeField;
            }
            set
            {
                this.changeOfGaugeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentDepartureAirport DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        public string DepartureDateTime
        {
            get
            {
                return this.departureDateTimeField;
            }
            set
            {
                this.departureDateTimeField = value;
            }
        }

        /// <remarks/>
        public string DepartureDayOfWeek
        {
            get
            {
                return this.departureDayOfWeekField;
            }
            set
            {
                this.departureDayOfWeekField = value;
            }
        }

        /// <remarks/>
        public uint DepartureTimeZone
        {
            get
            {
                return this.departureTimeZoneField;
            }
            set
            {
                this.departureTimeZoneField = value;
            }
        }

        /// <remarks/>
        public string DepartureUTCDateTime
        {
            get
            {
                return this.departureUTCDateTimeField;
            }
            set
            {
                this.departureUTCDateTimeField = value;
            }
        }

        /// <remarks/>
        public string DirectionIndicator
        {
            get
            {
                return this.directionIndicatorField;
            }
            set
            {
                this.directionIndicatorField = value;
            }
        }

        /// <remarks/>
        public string DiscountApplied
        {
            get
            {
                return this.discountAppliedField;
            }
            set
            {
                this.discountAppliedField = value;
            }
        }

        /// <remarks/>
        public string DisplayIndicator
        {
            get
            {
                return this.displayIndicatorField;
            }
            set
            {
                this.displayIndicatorField = value;
            }
        }

        /// <remarks/>
        public uint Distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentEquipment Equipment
        {
            get
            {
                return this.equipmentField;
            }
            set
            {
                this.equipmentField = value;
            }
        }

        /// <remarks/>
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        public string FlightSegmentType
        {
            get
            {
                return this.flightSegmentTypeField;
            }
            set
            {
                this.flightSegmentTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FlightStatus", IsNullable = false)]
        public OperationalFlightSegmentFlightStatus[] FlightStatuses
        {
            get
            {
                return this.flightStatusesField;
            }
            set
            {
                this.flightStatusesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string GroundTime
        {
            get
            {
                return this.groundTimeField;
            }
            set
            {
                this.groundTimeField = value;
            }
        }

        /// <remarks/>
        public string IsChangeOfGauge
        {
            get
            {
                return this.isChangeOfGaugeField;
            }
            set
            {
                this.isChangeOfGaugeField = value;
            }
        }

        /// <remarks/>
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string JourneyDuration
        {
            get
            {
                return this.journeyDurationField;
            }
            set
            {
                this.journeyDurationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FlightLeg", IsNullable = false)]
        public OperationalFlightSegmentFlightLeg[] Legs
        {
            get
            {
                return this.legsField;
            }
            set
            {
                this.legsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("MarketedFlightSegment", IsNullable = false)]
        public OperationalFlightSegmentMarketedFlightSegment[] MarketedFlightSegment
        {
            get
            {
                return this.marketedFlightSegmentField;
            }
            set
            {
                this.marketedFlightSegmentField = value;
            }
        }

        /// <remarks/>
        public string MarriageGroup
        {
            get
            {
                return this.marriageGroupField;
            }
            set
            {
                this.marriageGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public OperationalFlightSegmentMessage[] Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public uint NumberofStops
        {
            get
            {
                return this.numberofStopsField;
            }
            set
            {
                this.numberofStopsField = value;
            }
        }

        /// <remarks/>
        public double OnTimeRate
        {
            get
            {
                return this.onTimeRateField;
            }
            set
            {
                this.onTimeRateField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentOperatingAirline OperatingAirline
        {
            get
            {
                return this.operatingAirlineField;
            }
            set
            {
                this.operatingAirlineField = value;
            }
        }

        /// <remarks/>
        public string OperatingAirlineCode
        {
            get
            {
                return this.operatingAirlineCodeField;
            }
            set
            {
                this.operatingAirlineCodeField = value;
            }
        }

        /// <remarks/>
        public string OperatingAirlineName
        {
            get
            {
                return this.operatingAirlineNameField;
            }
            set
            {
                this.operatingAirlineNameField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentPerformance Performance
        {
            get
            {
                return this.performanceField;
            }
            set
            {
                this.performanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string ScheduledFlightDuration
        {
            get
            {
                return this.scheduledFlightDurationField;
            }
            set
            {
                this.scheduledFlightDurationField = value;
            }
        }

        /// <remarks/>
        public string ScheduledFrequency
        {
            get
            {
                return this.scheduledFrequencyField;
            }
            set
            {
                this.scheduledFrequencyField = value;
            }
        }

        /// <remarks/>
        public uint SegmentNumber
        {
            get
            {
                return this.segmentNumberField;
            }
            set
            {
                this.segmentNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Airport", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel", IsNullable = false)]
        public Airport[] StopLocations
        {
            get
            {
                return this.stopLocationsField;
            }
            set
            {
                this.stopLocationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TravelerCount", IsNullable = false)]
        public OperationalFlightSegmentTravelerCount[] TravelerCounts
        {
            get
            {
                return this.travelerCountsField;
            }
            set
            {
                this.travelerCountsField = value;
            }
        }

        /// <remarks/>
        public string ActualArrivalTime
        {
            get
            {
                return this.actualArrivalTimeField;
            }
            set
            {
                this.actualArrivalTimeField = value;
            }
        }

        /// <remarks/>
        public string ActualArrivalUTCTime
        {
            get
            {
                return this.actualArrivalUTCTimeField;
            }
            set
            {
                this.actualArrivalUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string ActualDepartureTime
        {
            get
            {
                return this.actualDepartureTimeField;
            }
            set
            {
                this.actualDepartureTimeField = value;
            }
        }

        /// <remarks/>
        public string ActualDepartureUTCTime
        {
            get
            {
                return this.actualDepartureUTCTimeField;
            }
            set
            {
                this.actualDepartureUTCTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string ActualEnrouteTime
        {
            get
            {
                return this.actualEnrouteTimeField;
            }
            set
            {
                this.actualEnrouteTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string ActualTaxiIn
        {
            get
            {
                return this.actualTaxiInField;
            }
            set
            {
                this.actualTaxiInField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string ActualTaxiOut
        {
            get
            {
                return this.actualTaxiOutField;
            }
            set
            {
                this.actualTaxiOutField = value;
            }
        }

        /// <remarks/>
        public string ArrivalBagClaimUnit
        {
            get
            {
                return this.arrivalBagClaimUnitField;
            }
            set
            {
                this.arrivalBagClaimUnitField = value;
            }
        }

        /// <remarks/>
        public string ArrivalDelayMinutes
        {
            get
            {
                return this.arrivalDelayMinutesField;
            }
            set
            {
                this.arrivalDelayMinutesField = value;
            }
        }

        /// <remarks/>
        public double ArrivalFuelWeight
        {
            get
            {
                return this.arrivalFuelWeightField;
            }
            set
            {
                this.arrivalFuelWeightField = value;
            }
        }

        /// <remarks/>
        public string ArrivalGate
        {
            get
            {
                return this.arrivalGateField;
            }
            set
            {
                this.arrivalGateField = value;
            }
        }

        /// <remarks/>
        public string ArrivalTermimal
        {
            get
            {
                return this.arrivalTermimalField;
            }
            set
            {
                this.arrivalTermimalField = value;
            }
        }

        /// <remarks/>
        public string BridgeOnTime
        {
            get
            {
                return this.bridgeOnTimeField;
            }
            set
            {
                this.bridgeOnTimeField = value;
            }
        }

        /// <remarks/>
        public string BridgeOnUTCTime
        {
            get
            {
                return this.bridgeOnUTCTimeField;
            }
            set
            {
                this.bridgeOnUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string CancelDivertFlightNumber
        {
            get
            {
                return this.cancelDivertFlightNumberField;
            }
            set
            {
                this.cancelDivertFlightNumberField = value;
            }
        }

        /// <remarks/>
        public string ClearedFuelIndicator
        {
            get
            {
                return this.clearedFuelIndicatorField;
            }
            set
            {
                this.clearedFuelIndicatorField = value;
            }
        }

        /// <remarks/>
        public double ClearedFuelWeight
        {
            get
            {
                return this.clearedFuelWeightField;
            }
            set
            {
                this.clearedFuelWeightField = value;
            }
        }

        /// <remarks/>
        public string DepartureBagSector
        {
            get
            {
                return this.departureBagSectorField;
            }
            set
            {
                this.departureBagSectorField = value;
            }
        }

        /// <remarks/>
        public string DepartureDelayMinutes
        {
            get
            {
                return this.departureDelayMinutesField;
            }
            set
            {
                this.departureDelayMinutesField = value;
            }
        }

        /// <remarks/>
        public double DepartureFuelWeight
        {
            get
            {
                return this.departureFuelWeightField;
            }
            set
            {
                this.departureFuelWeightField = value;
            }
        }

        /// <remarks/>
        public string DepartureGate
        {
            get
            {
                return this.departureGateField;
            }
            set
            {
                this.departureGateField = value;
            }
        }

        /// <remarks/>
        public string DepartureTerminal
        {
            get
            {
                return this.departureTerminalField;
            }
            set
            {
                this.departureTerminalField = value;
            }
        }

        /// <remarks/>
        public string EstimatedArrivalDelayMinutes
        {
            get
            {
                return this.estimatedArrivalDelayMinutesField;
            }
            set
            {
                this.estimatedArrivalDelayMinutesField = value;
            }
        }

        /// <remarks/>
        public string EstimatedArrivalTime
        {
            get
            {
                return this.estimatedArrivalTimeField;
            }
            set
            {
                this.estimatedArrivalTimeField = value;
            }
        }

        /// <remarks/>
        public string EstimatedArrivalUTCTime
        {
            get
            {
                return this.estimatedArrivalUTCTimeField;
            }
            set
            {
                this.estimatedArrivalUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string EstimatedDepartureDelayMinutes
        {
            get
            {
                return this.estimatedDepartureDelayMinutesField;
            }
            set
            {
                this.estimatedDepartureDelayMinutesField = value;
            }
        }

        /// <remarks/>
        public string EstimatedDepartureTime
        {
            get
            {
                return this.estimatedDepartureTimeField;
            }
            set
            {
                this.estimatedDepartureTimeField = value;
            }
        }

        /// <remarks/>
        public string EstimatedDepartureUTCTime
        {
            get
            {
                return this.estimatedDepartureUTCTimeField;
            }
            set
            {
                this.estimatedDepartureUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string FlightType
        {
            get
            {
                return this.flightTypeField;
            }
            set
            {
                this.flightTypeField = value;
            }
        }

        /// <remarks/>
        public string InTime
        {
            get
            {
                return this.inTimeField;
            }
            set
            {
                this.inTimeField = value;
            }
        }

        /// <remarks/>
        public string InUTCTime
        {
            get
            {
                return this.inUTCTimeField;
            }
            set
            {
                this.inUTCTimeField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentInboundFlightSegment InboundFlightSegment
        {
            get
            {
                return this.inboundFlightSegmentField;
            }
            set
            {
                this.inboundFlightSegmentField = value;
            }
        }

        /// <remarks/>
        public string OffTime
        {
            get
            {
                return this.offTimeField;
            }
            set
            {
                this.offTimeField = value;
            }
        }

        /// <remarks/>
        public string OffUTCTime
        {
            get
            {
                return this.offUTCTimeField;
            }
            set
            {
                this.offUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string OnTime
        {
            get
            {
                return this.onTimeField;
            }
            set
            {
                this.onTimeField = value;
            }
        }

        /// <remarks/>
        public string OnUTCTime
        {
            get
            {
                return this.onUTCTimeField;
            }
            set
            {
                this.onUTCTimeField = value;
            }
        }

        /// <remarks/>
        public string OutTime
        {
            get
            {
                return this.outTimeField;
            }
            set
            {
                this.outTimeField = value;
            }
        }

        /// <remarks/>
        public string OutUTCTime
        {
            get
            {
                return this.outUTCTimeField;
            }
            set
            {
                this.outUTCTimeField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentOutboundFlightSegment OutboundFlightSegment
        {
            get
            {
                return this.outboundFlightSegmentField;
            }
            set
            {
                this.outboundFlightSegmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string PlannedEnrouteTime
        {
            get
            {
                return this.plannedEnrouteTimeField;
            }
            set
            {
                this.plannedEnrouteTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string PlannedTaxiIn
        {
            get
            {
                return this.plannedTaxiInField;
            }
            set
            {
                this.plannedTaxiInField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string PlannedTaxiOut
        {
            get
            {
                return this.plannedTaxiOutField;
            }
            set
            {
                this.plannedTaxiOutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ReasonStatus", IsNullable = false)]
        public OperationalFlightSegmentReasonStatus[] ReasonStatuses
        {
            get
            {
                return this.reasonStatusesField;
            }
            set
            {
                this.reasonStatusesField = value;
            }
        }

        /// <remarks/>
        public uint RemainingMinutesToBoard
        {
            get
            {
                return this.remainingMinutesToBoardField;
            }
            set
            {
                this.remainingMinutesToBoardField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentShip Ship
        {
            get
            {
                return this.shipField;
            }
            set
            {
                this.shipField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentArrivalAirport
    {

        private Address addressField;

        private AirportTerminals airportTerminalsField;

        private Genre2 genreField;

        private string geocodeField;

        private IATACityCode iATACityCodeField;

        private string iATACodeField;

        private IATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private StateProvince2 stateProvinceField;

        private Status2 statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public AirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Genre2 Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public StateProvince2 StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Status2 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class Address
    {

        private string[] addressLinesField;

        private Characteristic characteristicField;

        private string cityField;

        private Country countryField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private Genre genreField;

        private string keyField;

        private LinksLink[] linksField;

        private string nameField;

        private string postalCodeField;

        private Region regionField;

        private StateProvince stateProvinceField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public string[] AddressLines
        {
            get
            {
                return this.addressLinesField;
            }
            set
            {
                this.addressLinesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Characteristic Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Country Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Genre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Region Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public StateProvince StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class AirportTerminals
    {

        private AirportTerminalsAirportGate[] airportGatesField;

        private Characteristic[] characteristicsField;

        private string geocodeField;

        private string keyField;

        private Link[] linksField;

        private string nameField;

        private AirportTerminalsStatus statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AirportGate", IsNullable = false)]
        public AirportTerminalsAirportGate[] AirportGates
        {
            get
            {
                return this.airportGatesField;
            }
            set
            {
                this.airportGatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AirportTerminalsStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportTerminalsAirportGate
    {

        private Characteristic[] characteristicsField;

        private string gateBankField;

        private AirportTerminalsAirportGateGenre genreField;

        private string geocodeField;

        private string identifierField;

        private string keyField;

        private Link[] linksField;

        private string operationalGateField;

        private AirportTerminalsAirportGateStatus statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string GateBank
        {
            get
            {
                return this.gateBankField;
            }
            set
            {
                this.gateBankField = value;
            }
        }

        /// <remarks/>
        public AirportTerminalsAirportGateGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public string Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string OperationalGate
        {
            get
            {
                return this.operationalGateField;
            }
            set
            {
                this.operationalGateField = value;
            }
        }

        /// <remarks/>
        public AirportTerminalsAirportGateStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportTerminalsAirportGateGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportTerminalsAirportGateStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    public partial class AirportTerminalsStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute("Genre", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class Genre2
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class IATACityCode
    {

        private string cityCodeField;

        private string iSOAlpha3CodeField;

        private string latitudeField;

        private LinksLink[] linksField;

        private string longitudeField;

        private string nameField;

        private string phoneCityCodeField;

        private string shortNameField;

        private StateCode stateCodeField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CityCode
        {
            get
            {
                return this.cityCodeField;
            }
            set
            {
                this.cityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PhoneCityCode
        {
            get
            {
                return this.phoneCityCodeField;
            }
            set
            {
                this.phoneCityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public StateCode StateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class IATACountryCode
    {

        private CountryCode countryCodeField;

        private DefaultCurrency defaultCurrencyField;

        private DefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private LinksLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public CountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute("StateProvince", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class StateProvince2
    {

        private CountryCode countryCodeField;

        private LinksLink[] linksField;

        private string nameField;

        private string shortNameField;

        private string stateProvinceCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public CountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string StateProvinceCode
        {
            get
            {
                return this.stateProvinceCodeField;
            }
            set
            {
                this.stateProvinceCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute("Status", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class Status2
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentChangeOfGauge
    {

        private OperationalFlightSegmentChangeOfGaugeFromEquipment fromEquipmentField;

        private uint groundTimeField;

        private Link[] linksField;

        private OperationalFlightSegmentChangeOfGaugeStopLocation stopLocationField;

        private OperationalFlightSegmentChangeOfGaugeToEquipment toEquipmentField;

        /// <remarks/>
        public OperationalFlightSegmentChangeOfGaugeFromEquipment FromEquipment
        {
            get
            {
                return this.fromEquipmentField;
            }
            set
            {
                this.fromEquipmentField = value;
            }
        }

        /// <remarks/>
        public uint GroundTime
        {
            get
            {
                return this.groundTimeField;
            }
            set
            {
                this.groundTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentChangeOfGaugeStopLocation StopLocation
        {
            get
            {
                return this.stopLocationField;
            }
            set
            {
                this.stopLocationField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentChangeOfGaugeToEquipment ToEquipment
        {
            get
            {
                return this.toEquipmentField;
            }
            set
            {
                this.toEquipmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentChangeOfGaugeFromEquipment
    {

        private uint cabinCountField;

        private CabinsCabin[] cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private string hasBoardingAssistanceField;

        private string isWheelchairAllowedField;

        private Link[] linksField;

        private Model modelField;

        private string noseNumberField;

        private string ownerAirlineCodeField;

        private string pseudoTailNumberField;

        private string registrationNumberField;

        private string seatMapConfigurationCodeField;

        private string shipNumberField;

        private string tailNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Cabin", IsNullable = false)]
        public CabinsCabin[] Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string HasBoardingAssistance
        {
            get
            {
                return this.hasBoardingAssistanceField;
            }
            set
            {
                this.hasBoardingAssistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsWheelchairAllowed
        {
            get
            {
                return this.isWheelchairAllowedField;
            }
            set
            {
                this.isWheelchairAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCodeField;
            }
            set
            {
                this.ownerAirlineCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string PseudoTailNumber
        {
            get
            {
                return this.pseudoTailNumberField;
            }
            set
            {
                this.pseudoTailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string SeatMapConfigurationCode
        {
            get
            {
                return this.seatMapConfigurationCodeField;
            }
            set
            {
                this.seatMapConfigurationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string ShipNumber
        {
            get
            {
                return this.shipNumberField;
            }
            set
            {
                this.shipNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string TailNumber
        {
            get
            {
                return this.tailNumberField;
            }
            set
            {
                this.tailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class CabinsCabin
    {

        private uint columnCountField;

        private string descriptionField;

        private string isUpperDeckField;

        private string keyField;

        private string layoutField;

        private Link[] linksField;

        private string nameField;

        private uint rowCountField;

        private CabinsCabinSeatRow[] seatRowsField;

        private string statusField;

        private uint totalSeatsField;

        /// <remarks/>
        public uint ColumnCount
        {
            get
            {
                return this.columnCountField;
            }
            set
            {
                this.columnCountField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string IsUpperDeck
        {
            get
            {
                return this.isUpperDeckField;
            }
            set
            {
                this.isUpperDeckField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        public string Layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public uint RowCount
        {
            get
            {
                return this.rowCountField;
            }
            set
            {
                this.rowCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SeatRow", IsNullable = false)]
        public CabinsCabinSeatRow[] SeatRows
        {
            get
            {
                return this.seatRowsField;
            }
            set
            {
                this.seatRowsField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public uint TotalSeats
        {
            get
            {
                return this.totalSeatsField;
            }
            set
            {
                this.totalSeatsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class CabinsCabinSeatRow
    {

        private string cabinDescriptionField;

        private Characteristic[] characteristicsField;

        private string genreField;

        private Link[] linksField;

        private uint rowNumberField;

        private CabinsCabinSeatRowSeat[] seatsField;

        private string statusField;

        /// <remarks/>
        public string CabinDescription
        {
            get
            {
                return this.cabinDescriptionField;
            }
            set
            {
                this.cabinDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public uint RowNumber
        {
            get
            {
                return this.rowNumberField;
            }
            set
            {
                this.rowNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Seat", IsNullable = false)]
        public CabinsCabinSeatRowSeat[] Seats
        {
            get
            {
                return this.seatsField;
            }
            set
            {
                this.seatsField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class CabinsCabinSeatRowSeat
    {

        private Characteristic[] characteristicsField;

        private string descriptionField;

        private string identifierField;

        private Link[] linksField;

        private CabinsCabinSeatRowSeatPrice priceField;

        private string seatClassField;

        private string seatTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public CabinsCabinSeatRowSeatPrice Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public string SeatClass
        {
            get
            {
                return this.seatClassField;
            }
            set
            {
                this.seatClassField = value;
            }
        }

        /// <remarks/>
        public string SeatType
        {
            get
            {
                return this.seatTypeField;
            }
            set
            {
                this.seatTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class CabinsCabinSeatRowSeatPrice
    {

        private BasePrice basePriceField;

        private BasePriceEquivalent basePriceEquivalentField;

        private FeesCharge[] feesField;

        private LinksLink[] linksField;

        private string promotionCodeField;

        private TaxesCharge[] taxesField;

        private TotalsCharge[] totalsField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public BasePrice BasePrice
        {
            get
            {
                return this.basePriceField;
            }
            set
            {
                this.basePriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public BasePriceEquivalent BasePriceEquivalent
        {
            get
            {
                return this.basePriceEquivalentField;
            }
            set
            {
                this.basePriceEquivalentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public FeesCharge[] Fees
        {
            get
            {
                return this.feesField;
            }
            set
            {
                this.feesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PromotionCode
        {
            get
            {
                return this.promotionCodeField;
            }
            set
            {
                this.promotionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public TaxesCharge[] Taxes
        {
            get
            {
                return this.taxesField;
            }
            set
            {
                this.taxesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Charge", IsNullable = false)]
        public TotalsCharge[] Totals
        {
            get
            {
                return this.totalsField;
            }
            set
            {
                this.totalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel", IsNullable = false)]
    public partial class Engine
    {

        private Characteristic[] characteristicField;

        private Link[] linksField;

        private string manufacturerField;

        private string serialNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Manufacturer
        {
            get
            {
                return this.manufacturerField;
            }
            set
            {
                this.manufacturerField = value;
            }
        }

        /// <remarks/>
        public string SerialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel", IsNullable = false)]
    public partial class Model
    {

        private string descriptionField;

        private string fleetField;

        private string genreField;

        private string keyField;

        private Link[] linksField;

        private string nameField;

        private string statusField;

        private string subFleetField;

        private string vendorNameField;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Fleet
        {
            get
            {
                return this.fleetField;
            }
            set
            {
                this.fleetField = value;
            }
        }

        /// <remarks/>
        public string Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string SubFleet
        {
            get
            {
                return this.subFleetField;
            }
            set
            {
                this.subFleetField = value;
            }
        }

        /// <remarks/>
        public string VendorName
        {
            get
            {
                return this.vendorNameField;
            }
            set
            {
                this.vendorNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentChangeOfGaugeStopLocation
    {

        private Address addressField;

        private AirportTerminals airportTerminalsField;

        private Genre2 genreField;

        private string geocodeField;

        private IATACityCode iATACityCodeField;

        private string iATACodeField;

        private IATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private StateProvince2 stateProvinceField;

        private Status2 statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public AirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Genre2 Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public StateProvince2 StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Status2 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentChangeOfGaugeToEquipment
    {

        private uint cabinCountField;

        private CabinsCabin[] cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private string hasBoardingAssistanceField;

        private string isWheelchairAllowedField;

        private Link[] linksField;

        private Model modelField;

        private string noseNumberField;

        private string ownerAirlineCodeField;

        private string pseudoTailNumberField;

        private string registrationNumberField;

        private string seatMapConfigurationCodeField;

        private string shipNumberField;

        private string tailNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Cabin", IsNullable = false)]
        public CabinsCabin[] Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string HasBoardingAssistance
        {
            get
            {
                return this.hasBoardingAssistanceField;
            }
            set
            {
                this.hasBoardingAssistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsWheelchairAllowed
        {
            get
            {
                return this.isWheelchairAllowedField;
            }
            set
            {
                this.isWheelchairAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCodeField;
            }
            set
            {
                this.ownerAirlineCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string PseudoTailNumber
        {
            get
            {
                return this.pseudoTailNumberField;
            }
            set
            {
                this.pseudoTailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string SeatMapConfigurationCode
        {
            get
            {
                return this.seatMapConfigurationCodeField;
            }
            set
            {
                this.seatMapConfigurationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string ShipNumber
        {
            get
            {
                return this.shipNumberField;
            }
            set
            {
                this.shipNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string TailNumber
        {
            get
            {
                return this.tailNumberField;
            }
            set
            {
                this.tailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentDepartureAirport
    {

        private Address addressField;

        private AirportTerminals airportTerminalsField;

        private Genre2 genreField;

        private string geocodeField;

        private IATACityCode iATACityCodeField;

        private string iATACodeField;

        private IATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private StateProvince2 stateProvinceField;

        private Status2 statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public AirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Genre2 Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public StateProvince2 StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Status2 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentEquipment
    {

        private uint cabinCountField;

        private CabinsCabin[] cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private string hasBoardingAssistanceField;

        private string isWheelchairAllowedField;

        private Link[] linksField;

        private Model modelField;

        private string noseNumberField;

        private string ownerAirlineCodeField;

        private string pseudoTailNumberField;

        private string registrationNumberField;

        private string seatMapConfigurationCodeField;

        private string shipNumberField;

        private string tailNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Cabin", IsNullable = false)]
        public CabinsCabin[] Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string HasBoardingAssistance
        {
            get
            {
                return this.hasBoardingAssistanceField;
            }
            set
            {
                this.hasBoardingAssistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsWheelchairAllowed
        {
            get
            {
                return this.isWheelchairAllowedField;
            }
            set
            {
                this.isWheelchairAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCodeField;
            }
            set
            {
                this.ownerAirlineCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string PseudoTailNumber
        {
            get
            {
                return this.pseudoTailNumberField;
            }
            set
            {
                this.pseudoTailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string SeatMapConfigurationCode
        {
            get
            {
                return this.seatMapConfigurationCodeField;
            }
            set
            {
                this.seatMapConfigurationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string ShipNumber
        {
            get
            {
                return this.shipNumberField;
            }
            set
            {
                this.shipNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string TailNumber
        {
            get
            {
                return this.tailNumberField;
            }
            set
            {
                this.tailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentFlightStatus
    {

        private string codeField;

        private string descriptionField;

        private Link[] linksField;

        private string statusTypeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string StatusType
        {
            get
            {
                return this.statusTypeField;
            }
            set
            {
                this.statusTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentFlightLeg
    {

        private OperationalFlightSegmentFlightLegArrivalAirport arrivalAirportField;

        private string arrivalTimeField;

        private uint cabinCountField;

        private OperationalFlightSegmentFlightLegDepartureAirport departureAirportField;

        private string departureTimeField;

        private OperationalFlightSegmentFlightLegEquipment equipmentField;

        private string idField;

        private Link[] linksField;

        /// <remarks/>
        public OperationalFlightSegmentFlightLegArrivalAirport ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        public string ArrivalTime
        {
            get
            {
                return this.arrivalTimeField;
            }
            set
            {
                this.arrivalTimeField = value;
            }
        }

        /// <remarks/>
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentFlightLegDepartureAirport DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        public string DepartureTime
        {
            get
            {
                return this.departureTimeField;
            }
            set
            {
                this.departureTimeField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentFlightLegEquipment Equipment
        {
            get
            {
                return this.equipmentField;
            }
            set
            {
                this.equipmentField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentFlightLegArrivalAirport
    {

        private Address addressField;

        private AirportTerminals airportTerminalsField;

        private Genre2 genreField;

        private string geocodeField;

        private IATACityCode iATACityCodeField;

        private string iATACodeField;

        private IATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private StateProvince2 stateProvinceField;

        private Status2 statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public AirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Genre2 Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public StateProvince2 StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Status2 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentFlightLegDepartureAirport
    {

        private Address addressField;

        private AirportTerminals airportTerminalsField;

        private Genre2 genreField;

        private string geocodeField;

        private IATACityCode iATACityCodeField;

        private string iATACodeField;

        private IATACountryCode iATACountryCodeField;

        private string iCAOCodeField;

        private Link[] linksField;

        private string nameField;

        private string shortNameField;

        private StateProvince2 stateProvinceField;

        private Status2 statusField;

        private string timeZoneField;

        private double uTCOffsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Address Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public AirportTerminals AirportTerminals
        {
            get
            {
                return this.airportTerminalsField;
            }
            set
            {
                this.airportTerminalsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Genre2 Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACityCode IATACityCode
        {
            get
            {
                return this.iATACityCodeField;
            }
            set
            {
                this.iATACityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public IATACountryCode IATACountryCode
        {
            get
            {
                return this.iATACountryCodeField;
            }
            set
            {
                this.iATACountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public StateProvince2 StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public Status2 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
            "entModel.AirportModel")]
        public double UTCOffset
        {
            get
            {
                return this.uTCOffsetField;
            }
            set
            {
                this.uTCOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentFlightLegEquipment
    {

        private uint cabinCountField;

        private CabinsCabin[] cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private string hasBoardingAssistanceField;

        private string isWheelchairAllowedField;

        private Link[] linksField;

        private Model modelField;

        private string noseNumberField;

        private string ownerAirlineCodeField;

        private string pseudoTailNumberField;

        private string registrationNumberField;

        private string seatMapConfigurationCodeField;

        private string shipNumberField;

        private string tailNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Cabin", IsNullable = false)]
        public CabinsCabin[] Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string HasBoardingAssistance
        {
            get
            {
                return this.hasBoardingAssistanceField;
            }
            set
            {
                this.hasBoardingAssistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsWheelchairAllowed
        {
            get
            {
                return this.isWheelchairAllowedField;
            }
            set
            {
                this.isWheelchairAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCodeField;
            }
            set
            {
                this.ownerAirlineCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string PseudoTailNumber
        {
            get
            {
                return this.pseudoTailNumberField;
            }
            set
            {
                this.pseudoTailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string SeatMapConfigurationCode
        {
            get
            {
                return this.seatMapConfigurationCodeField;
            }
            set
            {
                this.seatMapConfigurationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string ShipNumber
        {
            get
            {
                return this.shipNumberField;
            }
            set
            {
                this.shipNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string TailNumber
        {
            get
            {
                return this.tailNumberField;
            }
            set
            {
                this.tailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentMarketedFlightSegment
    {

        private string flightNumberField;

        private Link[] linksField;

        private string marketingAirlineCodeField;

        /// <remarks/>
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string MarketingAirlineCode
        {
            get
            {
                return this.marketingAirlineCodeField;
            }
            set
            {
                this.marketingAirlineCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentMessage
    {

        private string codeField;

        private OperationalFlightSegmentMessageFlight flightField;

        private Link[] linksField;

        private OperationalFlightSegmentMessageStatus statusField;

        private string textField;

        private OperationalFlightSegmentMessageType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentMessageFlight Flight
        {
            get
            {
                return this.flightField;
            }
            set
            {
                this.flightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentMessageStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public OperationalFlightSegmentMessageType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentMessageFlight
    {

        private string arrivalAirportField;

        private string arrivalAirportNameField;

        private string arrivalDateField;

        private string carrierCodeField;

        private string departureAirportField;

        private string departureAirportNameField;

        private string departureDateField;

        private string flightNumberField;

        private string flightOriginationDateField;

        private string isInternationalField;

        private LinksLink[] linksField;

        private string marketingCarrierIndicatorField;

        private string operatingCarrierCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportNameField;
            }
            set
            {
                this.arrivalAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CarrierCode
        {
            get
            {
                return this.carrierCodeField;
            }
            set
            {
                this.carrierCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportNameField;
            }
            set
            {
                this.departureAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureDate
        {
            get
            {
                return this.departureDateField;
            }
            set
            {
                this.departureDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightOriginationDate
        {
            get
            {
                return this.flightOriginationDateField;
            }
            set
            {
                this.flightOriginationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string MarketingCarrierIndicator
        {
            get
            {
                return this.marketingCarrierIndicatorField;
            }
            set
            {
                this.marketingCarrierIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCodeField;
            }
            set
            {
                this.operatingCarrierCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentMessageStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentMessageType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentOperatingAirline
    {

        private Address1[] addressesField;

        private Characteristic[] characteristicsField;

        private uint displaySequenceField;

        private string isCassMemberField;

        private string isMustRideField;

        private string keyField;

        private Link[] linksField;

        private string memberNameField;

        private string nameField;

        private Status3 statusField;

        private Telephone telephoneField;

        private Type2 typeField;

        private string webSiteField;

        private string accountingCodeField;

        private Country2 countryField;

        private string iATACodeField;

        private string iCAOCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Address", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Address1[] Addresses
        {
            get
            {
                return this.addressesField;
            }
            set
            {
                this.addressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Characteristic[] Characteristics
        {
            get
            {
                return this.characteristicsField;
            }
            set
            {
                this.characteristicsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string IsCassMember
        {
            get
            {
                return this.isCassMemberField;
            }
            set
            {
                this.isCassMemberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string IsMustRide
        {
            get
            {
                return this.isMustRideField;
            }
            set
            {
                this.isMustRideField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string MemberName
        {
            get
            {
                return this.memberNameField;
            }
            set
            {
                this.memberNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public Status3 Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public Telephone Telephone
        {
            get
            {
                return this.telephoneField;
            }
            set
            {
                this.telephoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public Type2 Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string WebSite
        {
            get
            {
                return this.webSiteField;
            }
            set
            {
                this.webSiteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string AccountingCode
        {
            get
            {
                return this.accountingCodeField;
            }
            set
            {
                this.accountingCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public Country2 Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
            "endorModel")]
        public string ICAOCode
        {
            get
            {
                return this.iCAOCodeField;
            }
            set
            {
                this.iCAOCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute("Address", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Address1
    {

        private string[] addressLinesField;

        private AddressCharacteristic[] characteristicField;

        private string cityField;

        private AddressCountry countryField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private AddressGenre genreField;

        private string keyField;

        private AddressLink[] linksField;

        private string nameField;

        private string postalCodeField;

        private AddressRegion regionField;

        private AddressStateProvince stateProvinceField;

        private AddressStatus statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public string[] AddressLines
        {
            get
            {
                return this.addressLinesField;
            }
            set
            {
                this.addressLinesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Characteristic", IsNullable = false)]
        public AddressCharacteristic[] Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public AddressCountry Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public AddressGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public AddressRegion Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvince StateProvince
        {
            get
            {
                return this.stateProvinceField;
            }
            set
            {
                this.stateProvinceField = value;
            }
        }

        /// <remarks/>
        public AddressStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristic
    {

        private string codeField;

        private string descriptionField;

        private AddressCharacteristicGenre genreField;

        private AddressCharacteristicLink[] linksField;

        private AddressCharacteristicStatus statusField;

        private string valueField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public AddressCharacteristicGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCharacteristicLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressCharacteristicStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCharacteristicGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCharacteristicGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCharacteristicStatusLink[] linksField;

        private AddressCharacteristicStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCharacteristicStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressCharacteristicStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCharacteristicStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCharacteristicStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCharacteristicStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountry
    {

        private string countryCodeField;

        private AddressCountryDefaultCurrency defaultCurrencyField;

        private AddressCountryDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private AddressCountryLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private AddressCountryStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public AddressCountryDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public AddressCountryDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public AddressCountryStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private AddressCountryDefaultCurrencyLink[] linksField;

        private string nameField;

        private AddressCountryDefaultCurrencyStatus statusField;

        private AddressCountryDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AddressCountryDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public AddressCountryDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCountryDefaultCurrencyStatusLink[] linksField;

        private AddressCountryDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressCountryDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCountryDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCountryDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private AddressCountryDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCountryStatusLink[] linksField;

        private AddressCountryStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressCountryStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressCountryStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressCountryStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressCountryStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegion
    {

        private AddressRegionCountry countryField;

        private AddressRegionGenre genreField;

        private AddressRegionLink[] linksField;

        private string nameField;

        private string regionCodeField;

        /// <remarks/>
        public AddressRegionCountry Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public AddressRegionGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string RegionCode
        {
            get
            {
                return this.regionCodeField;
            }
            set
            {
                this.regionCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountry
    {

        private string countryCodeField;

        private AddressRegionCountryDefaultCurrency defaultCurrencyField;

        private AddressRegionCountryDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private AddressRegionCountryLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private AddressRegionCountryStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private AddressRegionCountryDefaultCurrencyLink[] linksField;

        private string nameField;

        private AddressRegionCountryDefaultCurrencyStatus statusField;

        private AddressRegionCountryDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionCountryDefaultCurrencyStatusLink[] linksField;

        private AddressRegionCountryDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionCountryDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionCountryDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private AddressRegionCountryDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionCountryStatusLink[] linksField;

        private AddressRegionCountryStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressRegionCountryStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionCountryStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionCountryStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionCountryStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressRegionGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressRegionGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressRegionLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvince
    {

        private AddressStateProvinceCountryCode countryCodeField;

        private AddressStateProvinceLink[] linksField;

        private string nameField;

        private string shortNameField;

        private string stateProvinceCodeField;

        /// <remarks/>
        public AddressStateProvinceCountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public string StateProvinceCode
        {
            get
            {
                return this.stateProvinceCodeField;
            }
            set
            {
                this.stateProvinceCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCode
    {

        private string countryCodeField;

        private AddressStateProvinceCountryCodeDefaultCurrency defaultCurrencyField;

        private AddressStateProvinceCountryCodeDefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private AddressStateProvinceCountryCodeLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private AddressStateProvinceCountryCodeStatus statusField;

        /// <remarks/>
        public string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeDefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeDefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrency
    {

        private string codeField;

        private uint decimalPlaceField;

        private string idField;

        private uint iSONumericCodeField;

        private AddressStateProvinceCountryCodeDefaultCurrencyLink[] linksField;

        private string nameField;

        private AddressStateProvinceCountryCodeDefaultCurrencyStatus statusField;

        private AddressStateProvinceCountryCodeDefaultCurrencyType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public uint DecimalPlace
        {
            get
            {
                return this.decimalPlaceField;
            }
            set
            {
                this.decimalPlaceField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public uint ISONumericCode
        {
            get
            {
                return this.iSONumericCodeField;
            }
            set
            {
                this.iSONumericCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeDefaultCurrencyLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeDefaultCurrencyStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeDefaultCurrencyType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStateProvinceCountryCodeDefaultCurrencyStatusLink[] linksField;

        private AddressStateProvinceCountryCodeDefaultCurrencyStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeDefaultCurrencyStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeDefaultCurrencyStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStateProvinceCountryCodeDefaultCurrencyStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeDefaultCurrencyStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStateProvinceCountryCodeDefaultCurrencyTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeDefaultCurrencyTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultCurrencyTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultLanguage
    {

        private string characterSetField;

        private string languageCodeField;

        private AddressStateProvinceCountryCodeDefaultLanguageLink[] linksField;

        private string nameField;

        /// <remarks/>
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeDefaultLanguageLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeDefaultLanguageLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStateProvinceCountryCodeStatusLink[] linksField;

        private AddressStateProvinceCountryCodeStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressStateProvinceCountryCodeStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStateProvinceCountryCodeStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStateProvinceCountryCodeStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceCountryCodeStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStateProvinceLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStatusLink[] linksField;

        private AddressStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public AddressStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private AddressStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public AddressStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AddressStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute("Status", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Status3
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        private Type typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Telephone
    {

        private Telephone1[] telephone1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Telephone", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Telephone1[] Telephone1
        {
            get
            {
                return this.telephone1Field;
            }
            set
            {
                this.telephone1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute("Telephone", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Telephone1
    {

        private string aNIField;

        private string areaCityCodeField;

        private string attentionField;

        private string countryAccessCodeField;

        private string descriptionField;

        private uint displaySequenceField;

        private string extensionField;

        private TelephoneGenre genreField;

        private string isSMSEnabledField;

        private TelephoneLink[] linksField;

        private string pINField;

        private TelephonePhoneLocation phoneLocationField;

        private string phoneNumberField;

        private TelephoneStatus statusField;

        /// <remarks/>
        public string ANI
        {
            get
            {
                return this.aNIField;
            }
            set
            {
                this.aNIField = value;
            }
        }

        /// <remarks/>
        public string AreaCityCode
        {
            get
            {
                return this.areaCityCodeField;
            }
            set
            {
                this.areaCityCodeField = value;
            }
        }

        /// <remarks/>
        public string Attention
        {
            get
            {
                return this.attentionField;
            }
            set
            {
                this.attentionField = value;
            }
        }

        /// <remarks/>
        public string CountryAccessCode
        {
            get
            {
                return this.countryAccessCodeField;
            }
            set
            {
                this.countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
            }
        }

        /// <remarks/>
        public TelephoneGenre Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        public string IsSMSEnabled
        {
            get
            {
                return this.isSMSEnabledField;
            }
            set
            {
                this.isSMSEnabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TelephoneLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string PIN
        {
            get
            {
                return this.pINField;
            }
            set
            {
                this.pINField = value;
            }
        }

        /// <remarks/>
        public TelephonePhoneLocation PhoneLocation
        {
            get
            {
                return this.phoneLocationField;
            }
            set
            {
                this.phoneLocationField = value;
            }
        }

        /// <remarks/>
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumberField;
            }
            set
            {
                this.phoneNumberField = value;
            }
        }

        /// <remarks/>
        public TelephoneStatus Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneGenre
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TelephoneGenreLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TelephoneGenreLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneGenreLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephonePhoneLocation
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TelephonePhoneLocationLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TelephonePhoneLocationLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephonePhoneLocationLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneStatus
    {

        private string codeField;

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TelephoneStatusLink[] linksField;

        private TelephoneStatusType typeField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TelephoneStatusLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public TelephoneStatusType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneStatusLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneStatusType
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private TelephoneStatusTypeLink[] linksField;

        /// <remarks/>
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public TelephoneStatusTypeLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class TelephoneStatusTypeLink
    {

        private string hrefField;

        private string linkTypeField;

        private string relField;

        /// <remarks/>
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        public string LinkType
        {
            get
            {
                return this.linkTypeField;
            }
            set
            {
                this.linkTypeField = value;
            }
        }

        /// <remarks/>
        public string Rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute("Type", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Type2
    {

        private string defaultIndicatorField;

        private string descriptionField;

        private uint displaySequenceField;

        private string keyField;

        private LinksLink[] linksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicatorField;
            }
            set
            {
                this.defaultIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint DisplaySequence
        {
            get
            {
                return this.displaySequenceField;
            }
            set
            {
                this.displaySequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute("Country", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Country2
    {

        private CountryCode countryCodeField;

        private DefaultCurrency defaultCurrencyField;

        private DefaultLanguage defaultLanguageField;

        private string iSOAlpha3CodeField;

        private LinksLink[] linksField;

        private string nameField;

        private string phoneCountryCodeField;

        private string shortNameField;

        private uint stateCountField;

        private Status statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public CountryCode CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultCurrency DefaultCurrency
        {
            get
            {
                return this.defaultCurrencyField;
            }
            set
            {
                this.defaultCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public DefaultLanguage DefaultLanguage
        {
            get
            {
                return this.defaultLanguageField;
            }
            set
            {
                this.defaultLanguageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ISOAlpha3Code
        {
            get
            {
                return this.iSOAlpha3CodeField;
            }
            set
            {
                this.iSOAlpha3CodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public uint StateCount
        {
            get
            {
                return this.stateCountField;
            }
            set
            {
                this.stateCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Status Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentPerformance
    {

        private uint arrivalMoreThan30MinLateRateField;

        private uint cancellationRateField;

        private uint dOTOnTimeCodeField;

        private uint dOTOnTimeRateField;

        private string isArrivalOnTimeLessThan50PercentField;

        private Link[] linksField;

        private uint onTimeArrivalRateField;

        /// <remarks/>
        public uint ArrivalMoreThan30MinLateRate
        {
            get
            {
                return this.arrivalMoreThan30MinLateRateField;
            }
            set
            {
                this.arrivalMoreThan30MinLateRateField = value;
            }
        }

        /// <remarks/>
        public uint CancellationRate
        {
            get
            {
                return this.cancellationRateField;
            }
            set
            {
                this.cancellationRateField = value;
            }
        }

        /// <remarks/>
        public uint DOTOnTimeCode
        {
            get
            {
                return this.dOTOnTimeCodeField;
            }
            set
            {
                this.dOTOnTimeCodeField = value;
            }
        }

        /// <remarks/>
        public uint DOTOnTimeRate
        {
            get
            {
                return this.dOTOnTimeRateField;
            }
            set
            {
                this.dOTOnTimeRateField = value;
            }
        }

        /// <remarks/>
        public string IsArrivalOnTimeLessThan50Percent
        {
            get
            {
                return this.isArrivalOnTimeLessThan50PercentField;
            }
            set
            {
                this.isArrivalOnTimeLessThan50PercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public uint OnTimeArrivalRate
        {
            get
            {
                return this.onTimeArrivalRateField;
            }
            set
            {
                this.onTimeArrivalRateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentTravelerCount
    {

        private Count[] countsField;

        private Link[] linksField;

        private string travelCountTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Count", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Count[] Counts
        {
            get
            {
                return this.countsField;
            }
            set
            {
                this.countsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string TravelCountType
        {
            get
            {
                return this.travelCountTypeField;
            }
            set
            {
                this.travelCountTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentInboundFlightSegment
    {

        private string arrivalAirportField;

        private string arrivalAirportNameField;

        private string arrivalDateField;

        private string carrierCodeField;

        private string departureAirportField;

        private string departureAirportNameField;

        private string departureDateField;

        private string flightNumberField;

        private string flightOriginationDateField;

        private string isInternationalField;

        private LinksLink[] linksField;

        private string marketingCarrierIndicatorField;

        private string operatingCarrierCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportNameField;
            }
            set
            {
                this.arrivalAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CarrierCode
        {
            get
            {
                return this.carrierCodeField;
            }
            set
            {
                this.carrierCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportNameField;
            }
            set
            {
                this.departureAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureDate
        {
            get
            {
                return this.departureDateField;
            }
            set
            {
                this.departureDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightOriginationDate
        {
            get
            {
                return this.flightOriginationDateField;
            }
            set
            {
                this.flightOriginationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string MarketingCarrierIndicator
        {
            get
            {
                return this.marketingCarrierIndicatorField;
            }
            set
            {
                this.marketingCarrierIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCodeField;
            }
            set
            {
                this.operatingCarrierCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentOutboundFlightSegment
    {

        private string arrivalAirportField;

        private string arrivalAirportNameField;

        private string arrivalDateField;

        private string carrierCodeField;

        private string departureAirportField;

        private string departureAirportNameField;

        private string departureDateField;

        private string flightNumberField;

        private string flightOriginationDateField;

        private string isInternationalField;

        private LinksLink[] linksField;

        private string marketingCarrierIndicatorField;

        private string operatingCarrierCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportNameField;
            }
            set
            {
                this.arrivalAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CarrierCode
        {
            get
            {
                return this.carrierCodeField;
            }
            set
            {
                this.carrierCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportNameField;
            }
            set
            {
                this.departureAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureDate
        {
            get
            {
                return this.departureDateField;
            }
            set
            {
                this.departureDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightOriginationDate
        {
            get
            {
                return this.flightOriginationDateField;
            }
            set
            {
                this.flightOriginationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string MarketingCarrierIndicator
        {
            get
            {
                return this.marketingCarrierIndicatorField;
            }
            set
            {
                this.marketingCarrierIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCodeField;
            }
            set
            {
                this.operatingCarrierCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentReasonStatus
    {

        private string customerFacingCodeField;

        private string customerFacingDescriptionField;

        private string isDelayEffectiveField;

        private Link[] linksField;

        private string operationalDescriptionField;

        private string reasonDefinitionField;

        private Genre[] reasonDescriptionsField;

        private string typeField;

        private string unimaticReasonCodeField;

        /// <remarks/>
        public string CustomerFacingCode
        {
            get
            {
                return this.customerFacingCodeField;
            }
            set
            {
                this.customerFacingCodeField = value;
            }
        }

        /// <remarks/>
        public string CustomerFacingDescription
        {
            get
            {
                return this.customerFacingDescriptionField;
            }
            set
            {
                this.customerFacingDescriptionField = value;
            }
        }

        /// <remarks/>
        public string IsDelayEffective
        {
            get
            {
                return this.isDelayEffectiveField;
            }
            set
            {
                this.isDelayEffectiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        public string OperationalDescription
        {
            get
            {
                return this.operationalDescriptionField;
            }
            set
            {
                this.operationalDescriptionField = value;
            }
        }

        /// <remarks/>
        public string ReasonDefinition
        {
            get
            {
                return this.reasonDefinitionField;
            }
            set
            {
                this.reasonDefinitionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Genre", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Genre[] ReasonDescriptions
        {
            get
            {
                return this.reasonDescriptionsField;
            }
            set
            {
                this.reasonDescriptionsField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string UnimaticReasonCode
        {
            get
            {
                return this.unimaticReasonCodeField;
            }
            set
            {
                this.unimaticReasonCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class OperationalFlightSegmentShip
    {

        private uint cabinCountField;

        private CabinsCabin[] cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private string hasBoardingAssistanceField;

        private string isWheelchairAllowedField;

        private Link[] linksField;

        private Model modelField;

        private string noseNumberField;

        private string ownerAirlineCodeField;

        private string pseudoTailNumberField;

        private string registrationNumberField;

        private string seatMapConfigurationCodeField;

        private string shipNumberField;

        private string tailNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public uint CabinCount
        {
            get
            {
                return this.cabinCountField;
            }
            set
            {
                this.cabinCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Cabin", IsNullable = false)]
        public CabinsCabin[] Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string HasBoardingAssistance
        {
            get
            {
                return this.hasBoardingAssistanceField;
            }
            set
            {
                this.hasBoardingAssistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string IsWheelchairAllowed
        {
            get
            {
                return this.isWheelchairAllowedField;
            }
            set
            {
                this.isWheelchairAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Link[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCodeField;
            }
            set
            {
                this.ownerAirlineCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string PseudoTailNumber
        {
            get
            {
                return this.pseudoTailNumberField;
            }
            set
            {
                this.pseudoTailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string SeatMapConfigurationCode
        {
            get
            {
                return this.seatMapConfigurationCodeField;
            }
            set
            {
                this.seatMapConfigurationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string ShipNumber
        {
            get
            {
                return this.shipNumberField;
            }
            set
            {
                this.shipNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string TailNumber
        {
            get
            {
                return this.tailNumberField;
            }
            set
            {
                this.tailNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.FlightRespons" +
        "eModel")]
    public partial class OperationalRouteFlight
    {

        private string arrivalAirportField;

        private string arrivalAirportNameField;

        private string arrivalDateField;

        private string carrierCodeField;

        private string departureAirportField;

        private string departureAirportNameField;

        private string departureDateField;

        private string flightNumberField;

        private string flightOriginationDateField;

        private string isInternationalField;

        private LinksLink[] linksField;

        private string marketingCarrierIndicatorField;

        private string operatingCarrierCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirportField;
            }
            set
            {
                this.arrivalAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportNameField;
            }
            set
            {
                this.arrivalAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string CarrierCode
        {
            get
            {
                return this.carrierCodeField;
            }
            set
            {
                this.carrierCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirport
        {
            get
            {
                return this.departureAirportField;
            }
            set
            {
                this.departureAirportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportNameField;
            }
            set
            {
                this.departureAirportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string DepartureDate
        {
            get
            {
                return this.departureDateField;
            }
            set
            {
                this.departureDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string FlightOriginationDate
        {
            get
            {
                return this.flightOriginationDateField;
            }
            set
            {
                this.flightOriginationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string IsInternational
        {
            get
            {
                return this.isInternationalField;
            }
            set
            {
                this.isInternationalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
        public LinksLink[] Links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string MarketingCarrierIndicator
        {
            get
            {
                return this.marketingCarrierIndicatorField;
            }
            set
            {
                this.marketingCarrierIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCodeField;
            }
            set
            {
                this.operatingCarrierCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Links
    {

        private LinksLink[] linkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Link")]
        public LinksLink[] Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class AddressLines
    {

        private string[] stringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("string", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] @string
        {
            get
            {
                return this.stringField;
            }
            set
            {
                this.stringField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Fees
    {

        private FeesCharge[] chargeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Charge")]
        public FeesCharge[] Charge
        {
            get
            {
                return this.chargeField;
            }
            set
            {
                this.chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Taxes
    {

        private TaxesCharge[] chargeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Charge")]
        public TaxesCharge[] Charge
        {
            get
            {
                return this.chargeField;
            }
            set
            {
                this.chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
    public partial class Totals
    {

        private TotalsCharge[] chargeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Charge")]
        public TotalsCharge[] Charge
        {
            get
            {
                return this.chargeField;
            }
            set
            {
                this.chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel")]
    [System.Xml.Serialization.XmlRootAttribute("Links", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.TravelManagem" +
        "entModel.AirportModel", IsNullable = false)]
    public partial class Links1
    {

        private Link[] linkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Link[] Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    [System.Xml.Serialization.XmlRootAttribute("Links", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel", IsNullable = false)]
    public partial class Links2
    {

        private Link[] linkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Link[] Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel", IsNullable = false)]
    public partial class SeatRows
    {

        private SeatRowsSeatRow[] seatRowField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SeatRow")]
        public SeatRowsSeatRow[] SeatRow
        {
            get
            {
                return this.seatRowField;
            }
            set
            {
                this.seatRowField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel", IsNullable = false)]
    public partial class Cabins
    {

        private CabinsCabin[] cabinField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Cabin")]
        public CabinsCabin[] Cabin
        {
            get
            {
                return this.cabinField;
            }
            set
            {
                this.cabinField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Addresses
    {

        private Address1[] addressField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Address", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Address1[] Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Characteristics
    {

        private Characteristic[] characteristicField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Characteristic", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Characteristic[] Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel")]
    [System.Xml.Serialization.XmlRootAttribute("Links", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.V" +
        "endorModel", IsNullable = false)]
    public partial class Links3
    {

        private Link[] linkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Link", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Link[] Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    

#region CSL FlightAmenities Model from XML  Generated 

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel", IsNullable = false)]
    public partial class ArrayOfFlightSegment
    {

        private ArrayOfFlightSegmentFlightSegment flightSegmentField;

        /// <remarks/>
        public ArrayOfFlightSegmentFlightSegment FlightSegment
        {
            get
            {
                return this.flightSegmentField;
            }
            set
            {
                this.flightSegmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class ArrayOfFlightSegmentFlightSegment
    {

        private Amenity[] amenitiesField;

        private ArrayOfFlightSegmentFlightSegmentEquipment equipmentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Amenity", Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel", IsNullable = false)]
        public Amenity[] Amenities
        {
            get
            {
                return this.amenitiesField;
            }
            set
            {
                this.amenitiesField = value;
            }
        }

        /// <remarks/>
        public ArrayOfFlightSegmentFlightSegmentEquipment Equipment
        {
            get
            {
                return this.equipmentField;
            }
            set
            {
                this.equipmentField = value;
            }
        }
    }
       
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
    public partial class AmenityValue
    {

        private string stringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string @string
        {
            get
            {
                return this.stringField;
            }
            set
            {
                this.stringField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.SegmentModel")]
    public partial class ArrayOfFlightSegmentFlightSegmentEquipment
    {

        private Cabins cabinsField;

        private string cruiseSpeedField;

        private Engine engineField;

        private Model modelField;

        private ushort noseNumberField;

        private string wingspanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Cabins Cabins
        {
            get
            {
                return this.cabinsField;
            }
            set
            {
                this.cabinsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string CruiseSpeed
        {
            get
            {
                return this.cruiseSpeedField;
            }
            set
            {
                this.cruiseSpeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Engine Engine
        {
            get
            {
                return this.engineField;
            }
            set
            {
                this.engineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public Model Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public ushort NoseNumber
        {
            get
            {
                return this.noseNumberField;
            }
            set
            {
                this.noseNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
            "ircraftModel")]
        public string Wingspan
        {
            get
            {
                return this.wingspanField;
            }
            set
            {
                this.wingspanField = value;
            }
        }
    }
        
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel.A" +
        "ircraftModel")]
    public partial class EngineCharacteristic
    {

        private Characteristic characteristicField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel")]
        public Characteristic Characteristic
        {
            get
            {
                return this.characteristicField;
            }
            set
            {
                this.characteristicField = value;
            }
        }
    }

    public enum AmenityTypes
    {
        BEVERAGES,
        ENTERTAINMENT,
        INSEATPOWER,
        SEATING
    }

#endregion



}
