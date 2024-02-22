using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class CSLReservation : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.CSLReservation";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion
        private readonly IConfiguration _configuration;
        private readonly ICachingService _cachingService;
        public CSLReservation(IConfiguration configuration,
            ICachingService cachingService)
        {
            _configuration = configuration;
            _cachingService = cachingService;
        }

        public CSLReservation()
        {
           
        }
        public string SessionId { get; set; }

        private MOBResReservation _cslReservation;
        public MOBResReservation Reservation
        {
            get
            {
                return _cslReservation;
            }
            set
            {
                _cslReservation = value;
                isDirty = true;
            }
        }

        private bool isDirty;
        private MOBSHOPReservation uiReservation;
        public MOBSHOPReservation UIReservation
        {
            get
            {
                if (isDirty)
                {
                    // do the translation
                    uiReservation = new MOBSHOPReservation();
                    //transfer travelers..
                    //uiReservation.Travelers = transferTravelers(Reservation.Travelers);
                    return uiReservation;
                }
                else
                {
                    return uiReservation;
                }
            }
        }

        //private List<Traveler> transferTravelers(List<MOBResTraveler> travelers)
        //{
        //    List<Traveler> mobTravelers = null;
        //    if (travelers != null)
        //    {
        //        FlightConfirmTravelerResponse response = new FlightConfirmTravelerResponse();
        //        response = FilePersist.Load<FlightConfirmTravelerResponse>(SessionId, response.ObjectName);
        //        mobTravelers = new List<Traveler>();
        //        foreach (var traveler in travelers)
        //        {
        //            Traveler mobTraveler = new Traveler();
        //            if (traveler.Person != null)
        //            {
        //                mobTraveler.Person = new Person();
        //                //mobTraveler.Person.ChildIndicator = traveler.Person.ChildIndicator;
        //                mobTraveler.CustomerId = long.Parse(traveler.Person.CustomerId);
        //                mobTraveler.Person.DateOfBirth = traveler.Person.DateOfBirth;
        //                mobTraveler.Person.Title = traveler.Person.Title;
        //                mobTraveler.Person.GivenName = traveler.Person.GivenName;
        //                mobTraveler.Person.MiddleName = traveler.Person.MiddleName;
        //                mobTraveler.Person.Surname = traveler.Person.Surname;
        //                mobTraveler.Person.Suffix = traveler.Person.Suffix;
        //                mobTraveler.Person.Suffix = traveler.Person.Sex;
        //                //populate phones
        //                mobTraveler.Person.Phones = populatePhonesForTraveler(mobTraveler, response.Response.Travelers);
        //                if (traveler.Person.Documents != null)
        //                {
        //                    mobTraveler.Person.Documents = new List<Document>();
        //                    foreach (var dcoument in traveler.Person.Documents)
        //                    {
        //                        Document mobDocument = new Document();
        //                        mobDocument.DocumentId = dcoument.DocumentId;
        //                        mobDocument.KnownTravelerNumber = dcoument.KnownTravelerNumber;
        //                        mobDocument.RedressNumber = dcoument.RedressNumber;
        //                        mobTraveler.Person.Documents.Add(mobDocument);
        //                    }
        //                }
        //            }

        //            if (traveler.LoyaltyProgramProfile != null)
        //            {
        //                mobTraveler.LoyaltyProgramProfile = new LoyaltyProgramProfile();
        //                mobTraveler.LoyaltyProgramProfile.CarrierCode = traveler.LoyaltyProgramProfile.CarrierCode;
        //                mobTraveler.LoyaltyProgramProfile.MemberId = traveler.LoyaltyProgramProfile.MemberId;
        //            }

        //            mobTravelers.Add(mobTraveler);
        //        }
        //    }

        //    return mobTravelers;
        //}

        private List<Phone> populatePhonesForTraveler(MOBSHOPTraveler traveler, List<MOBSHOPTraveler> confirmedTravelers)
        {
            List<Phone> phones = null;
            foreach (MOBSHOPTraveler confirmedTraveler in confirmedTravelers)
            {
                if (confirmedTraveler.CustomerId == traveler.CustomerId)
                {
                    phones = confirmedTraveler.Person.Phones;
                    break;
                }
            }
            return phones;

        }
    }
}
