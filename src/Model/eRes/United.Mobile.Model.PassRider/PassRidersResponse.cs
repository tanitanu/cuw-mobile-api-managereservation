using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.Model.PassRider
{
    public class PassRidersResponse: ResponseBase
    {
        public string FlightsSelectedHeader { get; set; }
        public string SelectionsByTravelersHeader { get; set; }
        public string SelectionsByTravelersContent { get; set; }
        public string SelectionsByKTNRedressGenderContent { get; set; }
        public string CrewTravellingCheckboxText { get; set; }
        public EResAlert BaseAlert { get; set; }
        public PassRidersRequest BookingPassRiderListRequest { get; set; }
        public List<BookingPassRider> BookingPassRiders { get; set; }
        public List<BookingTrip> BookingTrips { get; set; }
        public bool BuddiesAllowed { get; set; }
        public bool OnlyFamilyBuddies { get; set; }
        public List<United.Mobile.Model.Internal.Common.PassRider> SuspendedPassRider { get; set; }
        public bool WorkingCrewMember { get; set; }
        public string TravelDocToolTip { get; set; }
        public string UnaccompaniedPickDropMessage { get; set; }
        public List<TravelDocument> DefaultTravelDocuments { get; set; }
        public bool IsRequireNationalityAndResidence { get; set; }
        public bool IsKtnRedressCleanupFeatureEnabled { get; set; }
        public SpecialService SpecialService { get; set; }
        public List<SpecialService> SpecialServices { get; set; }

    }
    public class BookingPassRider
    {
        public List<DropOption> ClassOfService { get; set; }
        public List<CustomizedRoute> CustomizedRoutes { get; set; }
        public string DisplayName { get; set; }
        public List<DropOption> JumpSeatTypes { get; set; }
        public List<DropOption> PassType { get; set; }
        public string PaxId { get; set; }
        public bool PrimaryFriend { get; set; }
        public RelationShip Relationship { get; set; }
        public int SortOrder { get; set; }
        public List<SSRInfo> SSRs { get; set; }
        public List<TravelDocument> TravelDocuments { get; set; }
        public List<string> DayOfContactInformation { get; set; }
        public int Age { get; set; }
        public List<int> TripIds { get; set; }
        public bool IsTravelDocumentAvailable { get; set; }
        public bool IsSameMetalTravel { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; }
        public string Country { get; set; }
        public List<TypeOption> KnownTraveler { get; set; }
        public List<TypeOption> Redress { get; set; }
        public BookingPassRider()
        {
            KnownTraveler = new List<TypeOption>();
            Redress = new List<TypeOption>();
        }
    }



}
