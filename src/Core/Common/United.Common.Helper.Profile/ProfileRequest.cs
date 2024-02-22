using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;

namespace United.Common.Helper.Profile
{
    public class ProfileRequest
    {
        public readonly IConfiguration _configuration;
        private bool _IsCorpBookingPath = false;

        public ProfileRequest(IConfiguration configuration, bool IsCorpBookingPath)
        {
            _configuration = configuration;
            _IsCorpBookingPath = IsCorpBookingPath;
        }
        public United.Services.Customer.Common.ProfileRequest GetProfileRequest(MOBCPProfileRequest mobCPProfileRequest, bool getEmployeeIdFromCSLCustomerData = false)
        {
            United.Services.Customer.Common.ProfileRequest request = new United.Services.Customer.Common.ProfileRequest();
            request.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
            if (mobCPProfileRequest.CustomerId != 0)
            {
                request.CustomerId = mobCPProfileRequest.CustomerId;
            }

            List<string> requestStringList = new List<string>();
            if (mobCPProfileRequest.ProfileOwnerOnly)
            {
                requestStringList.Add("ProfileOwnerOnly");
                if (mobCPProfileRequest.IncludeCreditCards)
                {
                    requestStringList.Add("CreditCards");
                }
            }
            else
            {
                if (mobCPProfileRequest.IncludeAllTravelerData)
                {
                    requestStringList.Add("AllTravelerData");
                    requestStringList.Add("TravelerData");
                }
                else
                {
                    #region
                    requestStringList.Add("TravelerData");
                    if (mobCPProfileRequest.IncludeAddresses)
                    {
                        requestStringList.Add("Addresses");
                    }
                    if (mobCPProfileRequest.IncludeEmailAddresses)
                    {
                        requestStringList.Add("EmailAddresses");
                    }
                    if (mobCPProfileRequest.IncludePhones)
                    {
                        requestStringList.Add("Phones");
                    }
                    if (mobCPProfileRequest.IncludeSubscriptions)
                    {
                        requestStringList.Add("Subscriptions");
                    }
                    if (mobCPProfileRequest.IncludeTravelMarkets)
                    {
                        requestStringList.Add("TravelMarkets");
                    }
                    if (mobCPProfileRequest.IncludeCustomerProfitScore)
                    {
                        requestStringList.Add("CustomerProfitScore");
                    }
                    if (mobCPProfileRequest.IncludePets)
                    {
                        requestStringList.Add("Pets");
                    }
                    if (mobCPProfileRequest.IncludeCarPreferences)
                    {
                        requestStringList.Add("CarPreferences");
                    }
                    if (mobCPProfileRequest.IncludeDisplayPreferences)
                    {
                        requestStringList.Add("DisplayPreferences");
                    }
                    if (mobCPProfileRequest.IncludeHotelPreferences)
                    {
                        requestStringList.Add("HotelPreferences");
                    }
                    if (mobCPProfileRequest.IncludeAirPreferences)
                    {
                        requestStringList.Add("AirPreferences");
                    }
                    if (mobCPProfileRequest.IncludeContacts)
                    {
                        requestStringList.Add("Contacts");
                    }
                    if (mobCPProfileRequest.IncludePassports)
                    {
                        requestStringList.Add("Passports");
                    }
                    if (mobCPProfileRequest.IncludeSecureTravelers)
                    {
                        requestStringList.Add("SecureTravelers");
                    }
                    if (mobCPProfileRequest.IncludeFlexEQM)
                    {
                        requestStringList.Add("FlexEQM");
                    }
                    if (mobCPProfileRequest.IncludeCreditCards)
                    {
                        requestStringList.Add("CreditCards");
                    }
                    if (mobCPProfileRequest.IncludeServiceAnimals)
                    {
                        requestStringList.Add("ServiceAnimals");
                    }
                    if (mobCPProfileRequest.IncludeSpecialRequests)
                    {
                        requestStringList.Add("SpecialRequests");
                    }
                    if (mobCPProfileRequest.IncludePosCountyCode)
                    {
                        requestStringList.Add("PosCountyCode");
                    }
                    #endregion
                }
            }
            if (requestStringList.Count == 0)
            {
                requestStringList.Add("AllTravelerData"); // This option means return all the data
            }

            if (getEmployeeIdFromCSLCustomerData)
            {
                requestStringList.Add("EmployeeLinkage");
            }
            if (_configuration.GetValue<bool>("CorporateConcurBooking") && _IsCorpBookingPath)
            {
                requestStringList.Add("CorporateCC");
            }
            request.DataToLoad = requestStringList;

            if (mobCPProfileRequest.ReturnAllSavedTravelers || mobCPProfileRequest.CustomerId == 0)
            {
                if (!string.IsNullOrEmpty(mobCPProfileRequest.MileagePlusNumber))
                {
                    request.LoyaltyId = mobCPProfileRequest.MileagePlusNumber;
                }
                else
                {
                    throw new MOBUnitedException("Profile Owner MileagePlus number is required.");
                }
            }
            else
            {
                request.MemberCustomerIdsToLoad = new List<int>();
                request.MemberCustomerIdsToLoad.Add(mobCPProfileRequest.CustomerId);
            }

            return request;
        }



    }
}
