using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Person
    {
       
        public virtual string Key { get; set; }
       
        public virtual string Surname { get; set; }
       
        public virtual string GivenName { get; set; }
       
        public virtual string MiddleName { get; set; }
       
        public virtual string Title { get; set; }
       
        public virtual string Suffix { get; set; }
       
        public virtual string PreferredName { get; set; }
       
        public virtual string DateOfBirth { get; set; }
       
        public virtual string Sex { get; set; }
       
        public virtual string ChildIndicator { get; set; }
       
        //public virtual Collection<Document> Documents { get; set; }
       
        public virtual string CustomerID { get; set; }
       
       // public virtual Country CountryOfResidence { get; set; }
       
        public virtual string DisserviceIndex { get; set; }
       
        public virtual string FlightValue { get; set; }
       
        public virtual string ProfitScore { get; set; }
       
       // public virtual Status Status { get; set; }
       
        public virtual string Type { get; set; }
       
        public virtual string InfantIndicator { get; set; }
       
      //  public virtual Collection<Country> Nationality { get; set; }
       
      //  public virtual Language PreferredLanguage { get; set; }
       
        public virtual string ReservationIndex { get; set; }
       
       // public virtual Contact Contact { get; set; }
       
        public virtual string SocialSecurityNumber { get; set; }
       
        public virtual int Age { get; set; }
       
        public virtual string DisserviceScore { get; set; }
       
        public virtual string Prefix { get; set; }
       
        public virtual string IsDeceased { get; set; }
       
        public virtual string PricingPaxType { get; set; }
       
        public virtual string MedicalAlert { get; set; }
       
      //  public virtual EmergencyContact EmergencyContact { get; set; }
       
        public virtual string HasNoPassport { get; set; }
       
        public virtual string HasNoPassportSpec { get; set; }
       
       // public virtual Collection<Charge> Charges { get; set; }
       
        public virtual string FirstName { get; set; }
    }
}
