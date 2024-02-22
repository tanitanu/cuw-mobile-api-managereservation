using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class TravelerProfileResponse
    {
        /// <summary>
        /// Address detail
        /// </summary>
        public CustomerAttributes CustomerAttributes { get; set; }
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// Phone detail
        /// </summary>

        public List<Phone> Phones { get; set; }

        /// <summary>
        /// Email detail
        /// </summary>

        public List<Email> Emails { get; set; }

        public List<AirPreferenceDataModel> AirPreferences { get; set; }
        public ReadCustomerBaseProfile Profile { get; set; }
        public List<RewardProgramData> RewardPrograms { get; set; }
        public List<AirSpecialRequestDataMembers> SpecialRequests { get; set; }
        public List<ResponseTime> ResponseTimes { get; set; }
        public List<CreditCards> CreditCards { get; set; }

        public SecureTravelerResponseData SecureTravelers { get; set; }

        public SecureTravelerInformation SecureTraveler { get; set; }

        public PartnerCardDetails PartnerCard { get; set; }

        public GetMileagePlusDataMembers MileagePlusData { get; set; }
       
    }
}
