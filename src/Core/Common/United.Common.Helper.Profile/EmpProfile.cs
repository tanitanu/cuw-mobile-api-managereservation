using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using United.Common.Helper.EmployeeReservation;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.Profile;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.CSLModels;
using United.Mobile.Model.Internal.Exception;
using United.Services.Customer.Common;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Helper;
using CslDataVaultResponse = United.Service.Presentation.PaymentResponseModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using JsonSerializer = United.Utility.Helper.DataContextJsonSerializer;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Session = United.Mobile.Model.Common.Session;

namespace United.Common.Helper.Profile
{
    public class EmpProfile : IEmpProfile
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IEmployeeReservations _employeeReservations;
        private readonly ICacheLog<EmpProfile> _logger;
        private readonly IMileagePlus _mileagePlus;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILoyaltyUCBService _loyaltyUCBService;
        private readonly IMPTraveler _mPTraveler;
        private readonly IProfileCreditCard _profileCreditCard;
        private readonly ICustomerProfile _customerProfile;
        private bool IsCorpBookingPath = false;
        private bool IsArrangerBooking = false;

        private string _deviceId = string.Empty;

        private MOBApplication _application = new MOBApplication() { Version = new MOBVersion() };
        private readonly IHeaders _headers;

        public EmpProfile(IConfiguration configuration
            , IDynamoDBService dynamoDBService
            , ISessionHelperService sessionHelperService
            , IEmployeeReservations employeeReservations
            , ICacheLog<EmpProfile> logger
            , IMileagePlus mileagePlus
            , ILoyaltyUCBService loyaltyUCBService
            , IMPTraveler mPTraveler
            , IProfileCreditCard profileCreditCard
            , IHeaders headers
            , ICustomerProfile customerProfile
            )
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            _sessionHelperService = sessionHelperService;
            _employeeReservations = employeeReservations;
            _logger = logger;
            _mileagePlus = mileagePlus;
            _loyaltyUCBService = loyaltyUCBService;
            _mPTraveler = mPTraveler;
            _profileCreditCard = profileCreditCard;
            _headers = headers;
            _customerProfile = customerProfile;
        }

        #region Methods


        private async Task<Reservation> PersistedReservation(MOBCPProfileRequest request)
        {
            Reservation persistedReservation =
                new Reservation();
            if (request != null)
            {
                persistedReservation = await _sessionHelperService.GetSession<Reservation>(request.SessionId, persistedReservation.ObjectName, new List<string> { request.SessionId, persistedReservation.ObjectName }).ConfigureAwait(false);

                if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                {
                    if (persistedReservation != null && persistedReservation.ShopReservationInfo != null &&
                        persistedReservation.ShopReservationInfo.IsCorporateBooking)
                    {
                        this.IsCorpBookingPath = true;
                    }

                    if (persistedReservation != null && persistedReservation.ShopReservationInfo2 != null &&
                        persistedReservation.ShopReservationInfo2.IsArrangerBooking)
                    {
                        this.IsArrangerBooking = true;
                    }
                }
            }

            return persistedReservation;
        }

        #region updated member profile start

        private bool IsEnabledNationalityAndResidence(bool isReShop, int appid, string appversion)
        {
            if (!isReShop && EnableNationalityResidence(appid, appversion))
            {
                return true;
            }
            return false;
        }
        private bool EnableNationalityResidence(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableNationalityAndCountryOfResidence")
           && GeneralHelper.isApplicationVersionGreater(appId, appVersion, "AndroidiPhonePriceChangeVersion", "AndroidiPhonePriceChangeVersion", "", "", true, _configuration);
        }

        private async Task<(string employeeId, string displayEmployeeId)> GetEmployeeId(string transactionId, string mileagePlusNumber, string displayEmployeeId)
        {
            string employeeId = string.Empty;

            if (!string.IsNullOrEmpty(transactionId) && !string.IsNullOrEmpty(mileagePlusNumber))
            {
                var tupleRes = await _mileagePlus.GetEmployeeIdy(transactionId, mileagePlusNumber, _headers.ContextValues.SessionId, displayEmployeeId);
                string eId = tupleRes.employeeId;
                displayEmployeeId = tupleRes.displayEmployeeId;
                if (eId != null)
                {
                    employeeId = eId;
                }
            }

            return (employeeId, displayEmployeeId);
        }

        public async Task<double> GetTravelBankBalance(MOBCPProfileRequest request, string mileagePlusId)
        {
            double tbBalance = 0.00;
            string cslLoyaltryBalanceServiceResponse = await _loyaltyUCBService.GetLoyaltyBalance(request.Token, request.MileagePlusNumber, request.SessionId).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cslLoyaltryBalanceServiceResponse))
            {
                United.TravelBank.Model.BalancesDataModel.BalanceResponse PlusPointResponse = JsonSerializer.NewtonSoftDeserialize<United.TravelBank.Model.BalancesDataModel.BalanceResponse>(cslLoyaltryBalanceServiceResponse);
                United.TravelBank.Model.BalancesDataModel.Balance tbbalance = PlusPointResponse.Balances.FirstOrDefault(tb => tb.ProgramCurrencyType == United.TravelBank.Model.TravelBankConstants.ProgramCurrencyType.UBC);
                if (tbbalance != null && tbbalance.TotalBalance > 0)
                {
                    tbBalance = (double)tbbalance.TotalBalance;
                }
            }
            return tbBalance;
        }
        private static List<MOBCPPhone> GetPassriderPhoneList(Mobile.Model.Common.DayOfContactInformation contactInfo)
        {
            if (!string.IsNullOrEmpty(contactInfo?.PhoneNumber) && !string.IsNullOrEmpty(contactInfo.CountryCode) && !string.IsNullOrEmpty(contactInfo.DialCode))
            {
                return new List<MOBCPPhone>()
                        {
                           new MOBCPPhone()
                           {
                               PhoneNumber = contactInfo.PhoneNumber,
                               CountryCode = contactInfo.CountryCode,
                               CountryPhoneNumber = contactInfo.DialCode
                           }
                        };
            }

            return null;
        }

        private static List<MOBEmail> GetPassriderEmail(Mobile.Model.Common.DayOfContactInformation contactInfo)
        {
            if (!string.IsNullOrEmpty(contactInfo?.Email))
            {
                return new List<MOBEmail>()
                        {
                            new MOBEmail()
                            {
                                EmailAddress=contactInfo.Email
                            }
                        };
            }

            return null;
        }
        #endregion updated member profile End

        #endregion
        #region UCB Migration Mobile Phase 3 Changes
        public async Task<List<MOBCPProfile>> GetEmpProfileV2(MOBCPProfileRequest request, bool getEmployeeIdFromCSLCustomerData = false)
        {

            if (request == null)
            {
                throw new MOBUnitedException("Profile request cannot be null.");
            }
            List<MOBCPProfile> profiles = null;


            var response = await _customerProfile.GetProfileDetails(request);


            if (response != null && response.Data != null)
            {
                profiles = await PopulateEmpProfilesV2(request.SessionId, request.MileagePlusNumber, request.CustomerId, response.Data.Travelers, request, getEmployeeIdFromCSLCustomerData);
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            return profiles;
        }
        private async Task<List<MOBCPProfile>> PopulateEmpProfilesV2(string sessionId, string mileagePlusNumber, int customerId, List<TravelerProfileResponse> profilesTravelers, MOBCPProfileRequest request, bool getEmployeeIdFromCSLCustomerData = false)
        {
            List<MOBCPProfile> mobProfiles = null;
            if (profilesTravelers != null && profilesTravelers.Count > 0)
            {
                mobProfiles = new List<MOBCPProfile>();
                if (!string.IsNullOrEmpty(sessionId))
                {
                    CSLProfile persistedCSLProfile = new CSLProfile();
                    persistedCSLProfile = await _sessionHelperService.GetSession<CSLProfile>(sessionId, "United.Persist.Definition.Profile.CSLProfile", new List<String> { sessionId, "United.Persist.Definition.Profile.CSLProfile" }).ConfigureAwait(false);
                    if (persistedCSLProfile == null)
                    {
                        persistedCSLProfile = new CSLProfile
                        {
                            SessionId = sessionId,
                            MileagePlusNumber = mileagePlusNumber,
                            CustomerId = customerId
                        };
                    }
                    if (persistedCSLProfile.Profiles == null)
                    {

                        persistedCSLProfile.Profiles = mobProfiles;
                    }
                    else
                    {
                        mobProfiles = persistedCSLProfile.Profiles;
                    }
                }
                TravelerProfileResponse owner = profilesTravelers.First(t => t.Profile?.ProfileOwnerIndicator == true);
                if (owner != null && owner.AirPreferences != null && owner.Profile != null)
                {
                    MOBCPProfile mobProfile = new MOBCPProfile
                    {
                        AirportCode = owner.AirPreferences[0].AirportCode,
                        AirportNameLong = owner.AirPreferences[0].AirportNameLong,
                        AirportNameShort = owner.AirPreferences[0].AirportNameShort,
                        // Description = profile.Description,No longer sent by new service
                        // Key = profile.Key,No longer sent by new service
                        // LanguageCode = profile.LanguageCode,No longer sent by new service
                        ProfileId = Convert.ToInt32(owner.Profile.ProfileId),
                        //ProfileMembers = PopulateProfileMembers(profile.ProfileMembers),No longer sent by new service itis just duplicate info 
                        ProfileOwnerId = Convert.ToInt32(owner.Profile.ProfileOwnerId),
                        ProfileOwnerKey = owner.Profile.TravelerKey
                        // QuickCustomerId = profile.QuickCustomerId,No longer sent by new service
                        // QuickCustomerKey = profile.QuickCustomerKey No longer sent by new service
                    };
                    bool isProfileOwnerTSAFlagOn = false;
                    List<MOBKVP> mpList = new List<MOBKVP>();
                    if (_configuration.GetValue<bool>("GetEmp20PassRidersFromEResService"))
                    {
                        var tupleResponse = await PopulateEmpTravelersV2(profilesTravelers, mileagePlusNumber, isProfileOwnerTSAFlagOn, false, request, sessionId, getEmployeeIdFromCSLCustomerData);
                        mobProfile.Travelers = tupleResponse.Item1;
                        isProfileOwnerTSAFlagOn = tupleResponse.isProfileOwnerTSAFlagOn;
                        mpList = tupleResponse.savedTravelersMPList;
                    }

                    mobProfile.SavedTravelersMPList = mpList;
                    mobProfile.IsProfileOwnerTSAFlagON = isProfileOwnerTSAFlagOn;
                    if (mobProfile != null)
                    {
                        mobProfile.DisclaimerList = await _mPTraveler.GetProfileDisclaimerList();
                    }
                    mobProfiles.Add(mobProfile);
                }

            }

            return mobProfiles;
        }
        private async Task<(List<MOBCPTraveler>, bool isProfileOwnerTSAFlagOn, List<MOBKVP> savedTravelersMPList)> PopulateEmpTravelersV2(List<TravelerProfileResponse> travelers, string mileagePluNumber, bool isProfileOwnerTSAFlagOn, bool isGetCreditCardDetailsCall, MOBCPProfileRequest request, string sessionId, bool getEmployeeIdFromCSLCustomerData = false)
        {
            string displayEmployeeId = string.Empty;
            string employeeId = string.Empty;
            OwnerResponseModel profileOwnerResponse = new OwnerResponseModel();
            if (getEmployeeIdFromCSLCustomerData && travelers != null && travelers.Count > 0)
            {
                var owner = travelers.FirstOrDefault(traveler => traveler.Profile?.ProfileOwnerIndicator == true);
                if (owner != null)
                {
                    employeeId = travelers[0].Profile?.EmployeeId;
                }
            }
            else
            {
                var tupleRes = await GetEmployeeId(request.TransactionId, mileagePluNumber, displayEmployeeId);
                employeeId = tupleRes.employeeId;
                displayEmployeeId = tupleRes.displayEmployeeId;
            }


            if (string.IsNullOrEmpty(employeeId))
            {
                throw new MOBUnitedException("Unable to get employee profile.");
            }

            List<MOBKVP> savedTravelersMPList = new List<MOBKVP>();
            MOBCPTraveler profileOwnerDetails = new MOBCPTraveler();
            List<MOBCPTraveler> mobTravelers = new List<MOBCPTraveler>();
            var persistedReservation = await PersistedReservation(request);
            var isRequireNationalityAndResidence = IsEnabledNationalityAndResidence(false, request.Application.Id, request.Application.Version.Major)
                                                    && (persistedReservation?.ShopReservationInfo2?.InfoNationalityAndResidence?.IsRequireNationalityAndResidence ?? false);

            if (travelers != null && travelers.Count > 0)
            {
                int i = 0;
                var traveler = travelers.FirstOrDefault(traveler => traveler.Profile?.ProfileOwnerIndicator == true);
                if (traveler != null && traveler.Profile != null)
                {
                    profileOwnerResponse = await _mPTraveler.GetProfileOwnerInfo(request.Token, request.SessionId, request.MileagePlusNumber);
                    MOBCPTraveler mobTraveler = new MOBCPTraveler
                    {
                        PaxIndex = i
                    };
                    i++;
                    mobTraveler.CustomerId = Convert.ToInt32(traveler.Profile?.CustomerId);
                    if (traveler.Profile?.BirthDate != null)
                    {
                        mobTraveler.BirthDate = GeneralHelper.FormatDateOfBirth(traveler.Profile.BirthDate);
                        if (mobTraveler.BirthDate == "01/01/1")
                            mobTraveler.BirthDate = null;
                    }
                    mobTraveler.FirstName = traveler.Profile?.FirstName;
                    mobTraveler.GenderCode = traveler.Profile?.Gender.ToString() == "Undefined" ? "" : traveler.Profile.Gender.ToString();
                    mobTraveler.IsDeceased = mobTraveler.IsDeceased = profileOwnerResponse?.MileagePlus?.Data?.IsDeceased == true;
                    //mobTraveler.IsExecutive = traveler.IsExecutive;
                    mobTraveler.IsProfileOwner = traveler.Profile?.ProfileOwnerIndicator == true;
                    mobTraveler._employeeId = traveler.Profile?.EmployeeId;
                    mobTraveler.Key = traveler.Profile?.TravelerKey;
                    //mobTraveler.Key = mobTraveler.PaxIndex.ToString();
                    mobTraveler.LastName = traveler.Profile?.LastName;
                    mobTraveler.MiddleName = traveler.Profile?.MiddleName;
                    mobTraveler.MileagePlus = _mPTraveler.PopulateMileagePlusV2(profileOwnerResponse, request.MileagePlusNumber);
                    if (mobTraveler.MileagePlus != null)
                    {
                        mobTraveler.MileagePlus.MpCustomerId = Convert.ToInt32(traveler.Profile?.CustomerId);
                        if (request != null && ConfigUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                        {
                            mobTraveler.MileagePlus.TravelBankBalance = await GetTravelBankBalance(request, mobTraveler.MileagePlus.MileagePlusId);
                        }
                    }
                    //all the below commented properties no longer needed confirmed with service team
                    //mobTraveler.OwnerFirstName = traveler.OwnerFirstName;
                    //mobTraveler.OwnerLastName = traveler.OwnerLastName;
                    //mobTraveler.OwnerMiddleName = traveler.OwnerMiddleName;
                    //mobTraveler.OwnerSuffix = traveler.OwnerSuffix;
                    //mobTraveler.OwnerTitle = traveler.OwnerTitle;
                    mobTraveler.ProfileId = Convert.ToInt32(traveler.Profile.ProfileId);
                    mobTraveler.ProfileOwnerId = traveler.Profile.ProfileOwnerId;
                    bool isTSAFlagOn = false;
                    if (traveler.SecureTravelers != null)
                    {
                        if (request == null)
                        {
                            request = new MOBCPProfileRequest
                            {
                                SessionId = string.Empty,
                                DeviceId = string.Empty,
                                Application = new MOBApplication() { Id = 0 }
                            };
                        }
                        mobTraveler.SecureTravelers = _mPTraveler.PopulatorSecureTravelersV2(traveler.SecureTravelers, ref isTSAFlagOn, false, request.SessionId, request.Application.Id, request.DeviceId);
                        if (mobTraveler.SecureTravelers != null && mobTraveler.SecureTravelers.Count > 0)
                        {
                            mobTraveler.RedressNumber = mobTraveler.SecureTravelers[0].RedressNumber;
                            mobTraveler.KnownTravelerNumber = mobTraveler.SecureTravelers[0].KnownTravelerNumber;
                        }
                    }
                    mobTraveler.IsTSAFlagON = isTSAFlagOn;
                    if (mobTraveler.IsProfileOwner)
                    {
                        isProfileOwnerTSAFlagOn = isTSAFlagOn;
                    }
                    mobTraveler.Suffix = traveler.Profile?.Suffix;
                    mobTraveler.Title = traveler.Profile?.Title;
                    mobTraveler.TravelerTypeCode = traveler.Profile?.TravelerTypeCode;
                    mobTraveler.TravelerTypeDescription = traveler.Profile?.TravelerTypeDescription;
                    //mobTraveler.TravelProgramMemberId = traveler.TravProgramMemberId; No longer needed by Service team  

                    if (traveler != null)
                    {
                        if (mobTraveler.MileagePlus != null)
                        {
                            mobTraveler.CurrentEliteLevel = mobTraveler.MileagePlus.CurrentEliteLevel;
                            //mobTraveler.AirRewardPrograms = GetTravelerLoyaltyProfile(traveler.AirPreferences, traveler.MileagePlus.CurrentEliteLevel);
                        }
                    }

                    mobTraveler.AirRewardPrograms = _mPTraveler.GetTravelerRewardPgrograms(traveler.RewardPrograms, mobTraveler.CurrentEliteLevel);
                    mobTraveler.Phones = _mPTraveler.PopulatePhonesV2(traveler, true);

                    if (mobTraveler.IsProfileOwner)
                    {
                        // These Phone and Email details for Makre Reseravation Phone and Email reason is mobTraveler.Phones = PopulatePhones(traveler.Phones,true) will get only day of travel contacts to register traveler & edit traveler.
                        mobTraveler.ReservationPhones = _mPTraveler.PopulatePhonesV2(traveler, false);
                        mobTraveler.ReservationEmailAddresses = _mPTraveler.PopulateEmailAddressesV2(traveler.Emails, false);
                    }
                    if (mobTraveler.IsProfileOwner && request == null) //**PINPWD//mobTraveler.IsProfileOwner && request == null Means GetProfile and Populate is for MP PIN PWD Path
                    {
                        mobTraveler.ReservationEmailAddresses = _mPTraveler.PopulateAllEmailAddressesV2(traveler.Emails);
                    }
                    mobTraveler.AirPreferences = _mPTraveler.PopulateAirPrefrencesV2(traveler);
                    mobTraveler.Addresses = _mPTraveler.PopulateTravelerAddressesV2(traveler.Addresses, request?.Application, request?.Flow);
                    mobTraveler.EmailAddresses = _mPTraveler.PopulateEmailAddressesV2(traveler.Emails, true);
                    mobTraveler.CreditCards = await _profileCreditCard.PopulateCreditCards(isGetCreditCardDetailsCall, mobTraveler.Addresses, request);

                    //if ((mobTraveler.IsTSAFlagON && string.IsNullOrEmpty(mobTraveler.Title)) || string.IsNullOrEmpty(mobTraveler.FirstName) || string.IsNullOrEmpty(mobTraveler.LastName) || string.IsNullOrEmpty(mobTraveler.GenderCode) || string.IsNullOrEmpty(mobTraveler.BirthDate)) //|| mobTraveler.Phones == null || (mobTraveler.Phones != null && mobTraveler.Phones.Count == 0)
                    if (mobTraveler.IsTSAFlagON || string.IsNullOrEmpty(mobTraveler.FirstName) || string.IsNullOrEmpty(mobTraveler.LastName) || string.IsNullOrEmpty(mobTraveler.GenderCode) || string.IsNullOrEmpty(mobTraveler.BirthDate)) //|| mobTraveler.Phones == null || (mobTraveler.Phones != null && mobTraveler.Phones.Count == 0)
                    {
                        mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                    }

                    if (isRequireNationalityAndResidence)
                    {
                        if (string.IsNullOrEmpty(traveler.CustomerAttributes?.CountryofResidence) || string.IsNullOrEmpty(traveler.CustomerAttributes?.Nationality))
                        {
                            mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                        }
                    }
                    mobTraveler.Nationality = traveler.CustomerAttributes?.Nationality;
                    mobTraveler.CountryOfResidence = traveler.CustomerAttributes?.CountryofResidence;
                    mobTravelers.Add(mobTraveler);
                }
            }


            //IEmployeeReservations employeeReservations = new EmployeeReservations(_employeeReservations );
            //var employeeJA = employeeReservations.GetEResEmp20PassriderDetails(employeeId, request.Token, request.TransactionId, request.Application.Id, request.Application.Version.Major, request.DeviceId);
            var employeeJA = await _employeeReservations.GetEResEmp20PassriderDetails(employeeId, request.Token, request.TransactionId, request.Application.Id, request.Application.Version.Major, request.DeviceId);
            if (employeeJA?.PassRiders?.Any() ?? false)
            {
                //Populate people on JA other than employee
                int paxIndex = 1;
                foreach (var passRider in employeeJA?.PassRiders)
                {
                    var isMoreInfoRequired = false;

                    if (string.IsNullOrEmpty(passRider.FirstName)
                        || string.IsNullOrEmpty(passRider.LastName)
                        || string.IsNullOrEmpty(passRider.Gender)
                        || string.IsNullOrEmpty(passRider.BirthDate.ToString("MM/dd/yyyy")))
                    {
                        isMoreInfoRequired = true;
                    }
                    if (isRequireNationalityAndResidence)
                    {
                        if (string.IsNullOrEmpty(passRider.Residence) || string.IsNullOrEmpty(passRider.Citizenship))
                        {
                            isMoreInfoRequired = true;
                        }
                    }
                    MOBCPTraveler mobTraveler = new MOBCPTraveler
                    {
                        PaxIndex = paxIndex,
                        Key = passRider.DependantID,
                        FirstName = passRider.FirstName,
                        MiddleName = passRider.MiddleName,
                        LastName = passRider.LastName,
                        BirthDate = passRider.BirthDate.ToString("MM/dd/yyyy"),
                        GenderCode = passRider.Gender,
                        KnownTravelerNumber = passRider.SSRs.FirstOrDefault(s => s.Description.Equals("Known Traveler Number", StringComparison.InvariantCultureIgnoreCase))?.KnownTraveler,
                        RedressNumber = passRider.SSRs.FirstOrDefault(s => s.Description.Equals("Known Traveler Number", StringComparison.InvariantCultureIgnoreCase))?.Redress,
                        CountryOfResidence = passRider.Residence,
                        Nationality = passRider.Citizenship,
                        Message = isMoreInfoRequired ? _configuration.GetValue<string>("SavedTravelerInformationNeededMessage") : string.Empty,
                        _employeeId = passRider.DependantID,
                        Phones = GetPassriderPhoneList(passRider.DayOfContactInformation),
                        EmailAddresses = GetPassriderEmail(passRider.DayOfContactInformation)
                    };
                    mobTravelers.Add(mobTraveler);
                    paxIndex++;
                }
            }

            return (mobTravelers, isProfileOwnerTSAFlagOn, savedTravelersMPList);
        }
        #endregion 
    }
}
