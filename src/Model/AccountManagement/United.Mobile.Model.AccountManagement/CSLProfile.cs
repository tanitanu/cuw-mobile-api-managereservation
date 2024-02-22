using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class CSLProfile : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Profile.CSLProfile";
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

        public string SessionId {get; set;}

        public string MileagePlusNumber {get; set;}

        public int CustomerId { get; set; }

        private List<MOBCPProfile> _cslProfile;
        public List<MOBCPProfile> Profiles {
            get {
                return _cslProfile;
            }
            set {
                _cslProfile = value;
                isDirty = true;
            }
        }

        private bool isDirty;
        private List<MOBProfile> uiProfile;
        public List<MOBProfile> UIProfiles {
            get {
                if (isDirty) {
                    // do the translation
                    uiProfile = new List<MOBProfile>();
                    //transfer profile information.
                    return uiProfile;
                }
                else {
                    return uiProfile;
                }
            }
        }
       

        private List<MOBPhone> GetPhonesForTraveler (List<MOBCPPhone> phones, bool travelerisProfileOwner) {
        List<MOBPhone> mobPhones = new List<MOBPhone>();
        List<MOBPhone> profileOwnerHomePhones = new List<MOBPhone>();
            if (phones != null && phones.Count > 0) {
                bool hasHomePhone = false;
                foreach (MOBCPPhone phone in phones) {
                    DateTime effDate = DateTime.Parse(phone.EffectiveDate);
                    DateTime disContdDate = DateTime.Parse(phone.DiscontinuedDate);
                    if (effDate <= DateTime.UtcNow && disContdDate >= DateTime.UtcNow) {
                        MOBPhone p = new MOBPhone();
                        p.Key = phone.Key;
                        p.Channel = new SHOPChannel();
                        p.Channel.ChannelCode = phone.ChannelCode;
                        p.Channel.ChannelDescription = phone.ChannelCodeDescription;
                        p.Channel.ChannelTypeCode = phone.ChannelTypeCode.ToString();
                        p.Channel.ChannelTypeDescription = phone.ChannelTypeDescription;
                        p.Country = new MOBCountry();
                        p.Country.Code = phone.CountryCode;
                        p.Country.PhoneCode = phone.CountryPhoneNumber;
                        p.IsPrimary = phone.IsPrimary;
                        p.IsPrivate = phone.IsPrivate;
                        p.IsProfileOwner = phone.IsProfileOwner;
                        p.PhoneNumber = phone.PhoneNumber;
                        p.AreaNumber = phone.AreaNumber;
                        p.ExtensionNumber = phone.ExtensionNumber;
                        if(travelerisProfileOwner && phone.ChannelCode.Trim().ToUpper() == "P" && phone.ChannelTypeCode == "Home") {
                            p.IsProfileOwner = true;
                        }
                        p.IsProfileOwner = false;
                        if(!hasHomePhone && phone.ChannelCode.Trim().ToUpper() == "P" && phone.ChannelTypeCode == "Home") {
                            hasHomePhone = true;
                        }  
                    }
                    if(!travelerisProfileOwner && !hasHomePhone) {
                        mobPhones.AddRange(profileOwnerHomePhones);
                    }
                }
            }
            else if(!travelerisProfileOwner && profileOwnerHomePhones != null && profileOwnerHomePhones.Count > 0) {
                // Enhancement to add the Profile Owner Home Phones if the saved travelers does not have any phones (home,work etc..)
                //t.Phones = new List<MOBPhone>();
                //t.Phones = profileOwnerHomePhones;
            }
            return mobPhones;
        }
    }
}