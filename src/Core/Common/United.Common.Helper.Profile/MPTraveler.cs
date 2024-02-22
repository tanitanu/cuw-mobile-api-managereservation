using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.MPAuthentication;
using United.Mobile.DataAccess.Profile;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.CSLModels;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.CommonModel;
using United.Services.Customer.Common;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using InsertTravelerRequest = United.Mobile.Model.Shopping.InsertTravelerRequest;
using JsonSerializer = United.Utility.Helper.DataContextJsonSerializer;
using MOBItem = United.Mobile.Model.Common.MOBItem;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Session = United.Mobile.Model.Common.Session;
using Status = United.Service.Presentation.CommonModel.Status;
using Traveler = United.Services.Customer.Common.Traveler;
using TravelType = United.Common.Helper.Enum.TravelType;

namespace United.Common.Helper.Profile
{
    public class MPTraveler : IMPTraveler
    {
        private readonly IConfiguration _configuration;
        private readonly IReferencedataService _referencedataService;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ICustomerDataService _customerDataService;
        private readonly IDataVaultService _dataVaultService;
        private readonly IUtilitiesService _utilitiesService;
        private readonly ICacheLog<MPTraveler> _logger;
        private readonly IPNRRetrievalService _pNRRetrievalService;
        private readonly IProfileCreditCard _profileCreditCard;
        private readonly IBaseEmployeeResService _baseEmployeeRes;
        private readonly IEServiceCheckin _eServiceCheckin;
        private readonly IInsertOrUpdateTravelInfoService _insertOrUpdateTravelInfoService;
        private readonly ILoyaltyUCBService _loyaltyUCBService;
        private readonly ICustomerProfileOwnerService _customerProfileOwnerService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;

        private string _deviceId = string.Empty;
        private bool IsCorpBookingPath = false;
        private bool IsArrangerBooking = false;
        private readonly IHeaders _headers;

        private MOBApplication _application = new MOBApplication() { Version = new MOBVersion() };

        public MPTraveler(IConfiguration configuration
            , IReferencedataService referencedataService
            , ISessionHelperService sessionHelperService
            , ICustomerDataService mPEnrollmentService
            , IDataVaultService dataVaultService
            , IUtilitiesService utilitiesService
            , ICacheLog<MPTraveler> logger
            , IPNRRetrievalService pNRRetrievalService
            , IProfileCreditCard profileCreditCard
            , IBaseEmployeeResService baseEmployeeRes
            , IEServiceCheckin eServiceCheckin
            , IInsertOrUpdateTravelInfoService insertOrUpdateTravelInfoService
            , ILoyaltyUCBService loyaltyUCBService
            , ICustomerProfileOwnerService customerProfileOwnerService
            , IHeaders headers
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            )
        {
            _configuration = configuration;
            _referencedataService = referencedataService;
            _sessionHelperService = sessionHelperService;
            _customerDataService = mPEnrollmentService;
            _dataVaultService = dataVaultService;
            _utilitiesService = utilitiesService;
            _logger = logger;
            _pNRRetrievalService = pNRRetrievalService;
            _profileCreditCard = profileCreditCard;
            _baseEmployeeRes = baseEmployeeRes;
            _eServiceCheckin = eServiceCheckin;
            _insertOrUpdateTravelInfoService = insertOrUpdateTravelInfoService;
            _loyaltyUCBService = loyaltyUCBService;
            _customerProfileOwnerService = customerProfileOwnerService;
            _headers = headers;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            new ConfigUtility(_configuration);
            //_profileCreditCard = new ProfileCreditCard(null, _configuration, _sessionHelperService, _dataVaultService);

        }

        #region Methods
        private bool IsCorporateLeisureFareSelected(List<MOBSHOPTrip> trips)
        {
            string corporateFareText = _configuration.GetValue<string>("FSRLabelForCorporateLeisure") ?? string.Empty;
            if (trips != null)
            {
                return trips.Any(
                   x =>
                       x.FlattenedFlights.Any(
                           f =>
                               f.Flights.Any(
                                   fl =>
                                       fl.CorporateFareIndicator ==
                                       corporateFareText.ToString())));
            }

            return false;
        }
        public async Task<(List<MOBCPTraveler> mobTravelersOwnerFirstInList, bool isProfileOwnerTSAFlagOn, List<MOBKVP> savedTravelersMPList)> PopulateTravelers(List<Traveler> travelers, string mileagePluNumber,  bool isProfileOwnerTSAFlagOn, bool isGetCreditCardDetailsCall, MOBCPProfileRequest request, string sessionid, bool getMPSecurityDetails = false, string path = "")
        {
            var savedTravelersMPList = new List<MOBKVP>();
            List<MOBCPTraveler> mobTravelers = null;
            List<MOBCPTraveler> mobTravelersOwnerFirstInList = null;
            MOBCPTraveler profileOwnerDetails = new MOBCPTraveler();
            if (travelers != null && travelers.Count > 0)
            {
                mobTravelers = new List<MOBCPTraveler>();
                int i = 0;
                var persistedReservation = await PersistedReservation(request);

                foreach (Traveler traveler in travelers)
                {
                    #region
                    MOBCPTraveler mobTraveler = new MOBCPTraveler();
                    mobTraveler.PaxIndex = i; i++;
                    mobTraveler.CustomerId = traveler.CustomerId;
                    if (_configuration.GetValue<bool>("NGRPAwardCalendarMP2017Switch"))
                    {
                        mobTraveler.CustomerMetrics = PopulateCustomerMetrics(traveler.CustomerMetrics);
                    }
                    if (traveler.BirthDate != null)
                    {
                        mobTraveler.BirthDate = GeneralHelper.FormatDateOfBirth(traveler.BirthDate.GetValueOrDefault());
                        
                    }
                    if (_configuration.GetValue<bool>("EnableNationalityAndCountryOfResidence"))
                    {
                        if (persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.InfoNationalityAndResidence != null
                            && persistedReservation.ShopReservationInfo2.InfoNationalityAndResidence.IsRequireNationalityAndResidence)
                        {
                            if (string.IsNullOrEmpty(traveler.CountryOfResidence) || string.IsNullOrEmpty(traveler.Nationality))
                            {
                                mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                            }
                        }
                        mobTraveler.Nationality = traveler.Nationality;
                        mobTraveler.CountryOfResidence = traveler.CountryOfResidence;
                    }

                    mobTraveler.FirstName = traveler.FirstName;
                    mobTraveler.GenderCode = traveler.GenderCode;
                    mobTraveler.IsDeceased = traveler.IsDeceased;
                    mobTraveler.IsExecutive = traveler.IsExecutive;
                    mobTraveler.IsProfileOwner = traveler.IsProfileOwner;
                    mobTraveler.Key = traveler.Key;
                    mobTraveler.LastName = traveler.LastName;
                    mobTraveler.MiddleName = traveler.MiddleName;
                    mobTraveler.MileagePlus = PopulateMileagePlus(traveler.MileagePlus);
                    if (mobTraveler.MileagePlus != null)
                    {
                        mobTraveler.MileagePlus.MpCustomerId = traveler.CustomerId;

                        if (request != null && ConfigUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                        {
                            Session session = new Session();
                            string cslLoyaltryBalanceServiceResponse = await _loyaltyUCBService.GetLoyaltyBalance(request.Token, request.MileagePlusNumber, request.SessionId);
                            if (!string.IsNullOrEmpty(cslLoyaltryBalanceServiceResponse))
                            {
                                United.TravelBank.Model.BalancesDataModel.BalanceResponse PlusPointResponse = JsonSerializer.NewtonSoftDeserialize<United.TravelBank.Model.BalancesDataModel.BalanceResponse>(cslLoyaltryBalanceServiceResponse);
                                United.TravelBank.Model.BalancesDataModel.Balance tbbalance = PlusPointResponse.Balances.FirstOrDefault(tb => tb.ProgramCurrencyType == United.TravelBank.Model.TravelBankConstants.ProgramCurrencyType.UBC);
                                if (tbbalance != null && tbbalance.TotalBalance > 0)
                                {
                                    mobTraveler.MileagePlus.TravelBankBalance = (double)tbbalance.TotalBalance;
                                }
                            }
                        }
                    }
                    mobTraveler.OwnerFirstName = traveler.OwnerFirstName;
                    mobTraveler.OwnerLastName = traveler.OwnerLastName;
                    mobTraveler.OwnerMiddleName = traveler.OwnerMiddleName;
                    mobTraveler.OwnerSuffix = traveler.OwnerSuffix;
                    mobTraveler.OwnerTitle = traveler.OwnerTitle;
                    mobTraveler.ProfileId = traveler.ProfileId;
                    mobTraveler.ProfileOwnerId = traveler.ProfileOwnerId;
                    bool isTSAFlagOn = false;
                    if (traveler.SecureTravelers != null && traveler.SecureTravelers.Count > 0)
                    {
                        if (request == null)
                        {
                            request = new MOBCPProfileRequest();
                            request.SessionId = string.Empty;
                            request.DeviceId = string.Empty;
                            request.Application = new MOBApplication() { Id = 0 };
                        }
                        else if (request.Application == null)
                        {
                            request.Application = new MOBApplication() { Id = 0 };
                        }
                        mobTraveler.SecureTravelers = PopulatorSecureTravelers(traveler.SecureTravelers, ref isTSAFlagOn, i >= 2, request.SessionId, request.Application.Id, request.DeviceId);
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
                    mobTraveler.Suffix = traveler.Suffix;
                    mobTraveler.Title = traveler.Title;
                    mobTraveler.TravelerTypeCode = traveler.TravelerTypeCode;
                    mobTraveler.TravelerTypeDescription = traveler.TravelerTypeDescription;
                    //mobTraveler.PTCDescription = Utility.GetPaxDescription(traveler.TravelerTypeCode);
                    if (persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelerTypes != null
                        && persistedReservation.ShopReservationInfo2.TravelerTypes.Count > 0)
                    {
                        if (traveler.BirthDate != null)
                        {
                            if (EnableYADesc() && persistedReservation.ShopReservationInfo2.IsYATravel)
                            {
                                mobTraveler.PTCDescription = GetYAPaxDescByDOB();
                            }
                            else
                            {
                                mobTraveler.PTCDescription = GetPaxDescriptionByDOB(traveler.BirthDate.ToString(), persistedReservation.Trips[0].FlattenedFlights[0].Flights[0].DepartDate);
                            }
                        }
                    }
                    else
                    {
                        if (EnableYADesc() && persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.IsYATravel)
                        {
                            mobTraveler.PTCDescription = GetYAPaxDescByDOB();
                        }
                    }
                    mobTraveler.TravelProgramMemberId = traveler.TravProgramMemberId;
                    if (traveler != null)
                    {
                        if (traveler.MileagePlus != null)
                        {
                            mobTraveler.CurrentEliteLevel = traveler.MileagePlus.CurrentEliteLevel;
                            //mobTraveler.AirRewardPrograms = GetTravelerLoyaltyProfile(traveler.AirPreferences, traveler.MileagePlus.CurrentEliteLevel);
                        }
                    }
                    else if (_configuration.GetValue<bool>("BugFixToggleFor17M") && request != null && !string.IsNullOrEmpty(request.SessionId))
                    {
                        //    mobTraveler.CurrentEliteLevel = GetCurrentEliteLevel(mileagePluNumber);//**// Need to work on this with a test scenario with a Saved Traveler added MP Account with a Elite Status. Try to Add a saved traveler(with MP WX664656) to MP Account VW344781
                        /// 195113 : Booking - Travel Options -mAPP: Booking: PA tile is displayed for purchase in Customize screen for Elite Premier member travelling and Login with General member
                        /// Srini - 12/04/2017
                        /// Calling getprofile for each traveler to get elite level for a traveler, who hav mp#
                        mobTraveler.MileagePlus = await GetCurrentEliteLevelFromAirPreferences(traveler.AirPreferences, request.SessionId);
                        if (mobTraveler != null)
                        {
                            if (mobTraveler.MileagePlus != null)
                            {
                                mobTraveler.CurrentEliteLevel = mobTraveler.MileagePlus.CurrentEliteLevel;
                            }
                        }
                    }
                    mobTraveler.AirRewardPrograms = GetTravelerLoyaltyProfile(traveler.AirPreferences, mobTraveler.CurrentEliteLevel);
                    mobTraveler.Phones = PopulatePhones(traveler.Phones, true);

                    if (mobTraveler.IsProfileOwner)
                    {
                        // These Phone and Email details for Makre Reseravation Phone and Email reason is mobTraveler.Phones = PopulatePhones(traveler.Phones,true) will get only day of travel contacts to register traveler & edit traveler.
                        mobTraveler.ReservationPhones = PopulatePhones(traveler.Phones, false);
                        mobTraveler.ReservationEmailAddresses = PopulateEmailAddresses(traveler.EmailAddresses, false);

                        // Added by Hasnan - #53484. 10/04/2017
                        // As per the Bug 53484:PINPWD: iOS and Android - Phone number is blank in RTI screen after booking from newly created account.
                        // If mobTraveler.Phones is empty. Then it newly created account. Thus returning mobTraveler.ReservationPhones as mobTraveler.Phones.
                        if (!_configuration.GetValue<bool>("EnableDayOfTravelEmail") || string.IsNullOrEmpty(path) || !path.ToUpper().Equals("BOOKING"))
                        {
                            if (mobTraveler.Phones.Count == 0)
                            {
                                mobTraveler.Phones = mobTraveler.ReservationPhones;
                            }
                        }
                        #region Corporate Leisure(ProfileOwner must travel)//Client will use the IsMustRideTraveler flag to auto select the travel and not allow to uncheck the profileowner on the SelectTraveler Screen.
                        if (_configuration.GetValue<bool>("EnableCorporateLeisure"))
                        {
                            if (persistedReservation?.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelType == TravelType.CLB.ToString() && IsCorporateLeisureFareSelected(persistedReservation.Trips))
                            {
                                mobTraveler.IsMustRideTraveler = true;
                            }
                        }
                        #endregion Corporate Leisure
                    }
                    if (mobTraveler.IsProfileOwner && request == null) //**PINPWD//mobTraveler.IsProfileOwner && request == null Means GetProfile and Populate is for MP PIN PWD Path
                    {
                        mobTraveler.ReservationEmailAddresses = PopulateAllEmailAddresses(traveler.EmailAddresses);
                    }
                    mobTraveler.AirPreferences = PopulateAirPrefrences(traveler.AirPreferences);
                    if (request?.Application?.Version != null && string.IsNullOrEmpty(request?.Flow) && IsInternationalBillingAddress_CheckinFlowEnabled(request.Application))
                    {
                        try
                        {
                            MOBShoppingCart mobShopCart = new MOBShoppingCart();
                            mobShopCart = await _sessionHelperService.GetSession<MOBShoppingCart>(request.SessionId, mobShopCart.ObjectName, new List<string> { request.SessionId, mobShopCart.ObjectName }).ConfigureAwait(false);
                            if (mobShopCart != null && !string.IsNullOrEmpty(mobShopCart.Flow) && mobShopCart.Flow == FlowType.CHECKIN.ToString())
                            {
                                request.Flow = mobShopCart.Flow;
                            }
                        }
                        catch { }
                    }
                    mobTraveler.Addresses = PopulateTravelerAddresses(traveler.Addresses, request?.Application, request?.Flow);

                    if (_configuration.GetValue<bool>("EnableDayOfTravelEmail") && !string.IsNullOrEmpty(path) && path.ToUpper().Equals("BOOKING"))
                    {
                        mobTraveler.EmailAddresses = PopulateEmailAddresses(traveler.EmailAddresses, true);
                    }
                    else
                    if (!getMPSecurityDetails)
                    {
                        mobTraveler.EmailAddresses = PopulateEmailAddresses(traveler.EmailAddresses, false);
                    }
                    else
                    {
                        mobTraveler.EmailAddresses = PopulateMPSecurityEmailAddresses(traveler.EmailAddresses);
                    }
                    mobTraveler.CreditCards = IsCorpBookingPath ?await _profileCreditCard.PopulateCorporateCreditCards(traveler.CreditCards, isGetCreditCardDetailsCall, mobTraveler.Addresses, persistedReservation, sessionid) : await _profileCreditCard.PopulateCreditCards(traveler.CreditCards, isGetCreditCardDetailsCall, mobTraveler.Addresses, sessionid);

                    //if ((mobTraveler.IsTSAFlagON && string.IsNullOrEmpty(mobTraveler.Title)) || string.IsNullOrEmpty(mobTraveler.FirstName) || string.IsNullOrEmpty(mobTraveler.LastName) || string.IsNullOrEmpty(mobTraveler.GenderCode) || string.IsNullOrEmpty(mobTraveler.BirthDate)) //|| mobTraveler.Phones == null || (mobTraveler.Phones != null && mobTraveler.Phones.Count == 0)
                    if (mobTraveler.IsTSAFlagON || string.IsNullOrEmpty(mobTraveler.FirstName) || string.IsNullOrEmpty(mobTraveler.LastName) || string.IsNullOrEmpty(mobTraveler.GenderCode) || string.IsNullOrEmpty(mobTraveler.BirthDate)) //|| mobTraveler.Phones == null || (mobTraveler.Phones != null && mobTraveler.Phones.Count == 0)
                    {
                        mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                    }
                    if (mobTraveler.IsProfileOwner)
                    {
                        profileOwnerDetails = mobTraveler;
                    }
                    else
                    {
                        #region
                        if (mobTraveler.AirRewardPrograms != null && mobTraveler.AirRewardPrograms.Count > 0)
                        {
                            var airRewardProgramList = (from program in mobTraveler.AirRewardPrograms
                                                        where program.CarrierCode.ToUpper().Trim() == "UA"
                                                        select program).ToList();

                            if (airRewardProgramList != null && airRewardProgramList.Count > 0)
                            {
                                savedTravelersMPList.Add(new MOBKVP() { Key = mobTraveler.CustomerId.ToString(), Value = airRewardProgramList[0].MemberId });
                            }
                        }
                        #endregion
                        mobTravelers.Add(mobTraveler);
                    }
                    #endregion
                }
            }
            mobTravelersOwnerFirstInList = new List<MOBCPTraveler>();
            mobTravelersOwnerFirstInList.Add(profileOwnerDetails);
            if (!IsCorpBookingPath || IsArrangerBooking)
            {
                mobTravelersOwnerFirstInList.AddRange(mobTravelers);
            }

            return (mobTravelersOwnerFirstInList,isGetCreditCardDetailsCall, savedTravelersMPList);
        }
        private List<MOBEmail> PopulateMPSecurityEmailAddresses(List<Services.Customer.Common.Email> emailAddresses)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (Services.Customer.Common.Email email in emailAddresses)
                {
                    if (email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.Channel.ChannelCode = email.ChannelCode;
                        e.Channel.ChannelDescription = email.ChannelCodeDescription;
                        e.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                        e.Channel.ChannelTypeDescription = email.ChannelTypeDescription;
                        e.EmailAddress = email.EmailAddress;
                        e.IsDefault = email.IsDefault;
                        e.IsPrimary = email.IsPrimary;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.IsDayOfTravel;
                        if (email.IsPrimary)
                        {
                            primaryEmailAddress = new MOBEmail();
                            primaryEmailAddress = e;
                            break;
                        }
                        #endregion
                    }
                }
                if (primaryEmailAddress != null)
                {
                    mobEmailAddresses.Add(primaryEmailAddress);
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        public List<Mobile.Model.Common.MOBAddress> PopulateTravelerAddresses(List<United.Services.Customer.Common.Address> addresses, MOBApplication application = null, string flow = null)
        {
            #region

            var mobAddresses = new List<Mobile.Model.Common.MOBAddress>();
            if (addresses != null && addresses.Count > 0)
            {
                bool isCorpAddressPresent = false;
                if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
                {
                    //As per Business / DotCom Kalpen; we are removing the condition for checking the Effectivedate and Discontinued date
                    var corpIndex = addresses.FindIndex(x => x.ChannelTypeDescription != null && x.ChannelTypeDescription.ToLower() == "corporate" && x.AddressLine1 != null && x.AddressLine1.Trim() != "");
                    if (corpIndex >= 0)
                        isCorpAddressPresent = true;

                }
                foreach (United.Services.Customer.Common.Address address in addresses)
                {
                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        if (isCorpAddressPresent && address.ChannelTypeDescription.ToLower() == "corporate" &&
                            (_configuration.GetValue<bool>("USPOSCountryCodes_ByPass") || IsInternationalBilling(application, address.CountryCode, flow)))
                        {
                            var a = new Mobile.Model.Common.MOBAddress();
                            a.Key = address.Key;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = address.ChannelCode;
                            a.Channel.ChannelDescription = address.ChannelCodeDescription;
                            a.Channel.ChannelTypeCode = address.ChannelTypeCode.ToString();
                            a.Channel.ChannelTypeDescription = address.ChannelTypeDescription;
                            a.ApartmentNumber = address.AptNum;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = address.ChannelCode;
                            a.Channel.ChannelDescription = address.ChannelCodeDescription;
                            a.Channel.ChannelTypeCode = address.ChannelTypeCode.ToString();
                            a.Channel.ChannelTypeDescription = address.ChannelTypeDescription;
                            a.City = address.City;
                            a.CompanyName = address.CompanyName;
                            a.Country = new MOBCountry();
                            a.Country.Code = address.CountryCode;
                            a.Country.Name = address.CountryName;
                            a.JobTitle = address.JobTitle;
                            a.Line1 = address.AddressLine1;
                            a.Line2 = address.AddressLine2;
                            a.Line3 = address.AddressLine3;
                            a.State = new Mobile.Model.Common.State();
                            a.State.Code = address.StateCode;
                            a.IsDefault = address.IsDefault;
                            a.IsPrivate = address.IsPrivate;
                            a.PostalCode = address.PostalCode;
                            if (address.ChannelTypeDescription.ToLower().Trim() == "corporate")
                            {
                                a.IsPrimary = true;
                                a.IsCorporate = true; // MakeIsCorporate true inorder to disable the edit on client
                            }
                            // Make IsPrimary true inorder to select the corpaddress by default

                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                a.IsValidForTPIPurchase = IsValidAddressForTPIpayment(address.CountryCode);

                                if (a.IsValidForTPIPurchase)
                                {
                                    a.IsValidForTPIPurchase = IsValidSateForTPIpayment(address.StateCode);
                                }
                            }
                            mobAddresses.Add(a);
                        }
                    }


                    if (address.EffectiveDate <= DateTime.UtcNow && address.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        if (_configuration.GetValue<bool>("USPOSCountryCodes_ByPass") || IsInternationalBilling(application, address.CountryCode, flow)) //##Kirti - allow only US addresses 
                        {
                            var a = new Mobile.Model.Common.MOBAddress();
                            a.Key = address.Key;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = address.ChannelCode;
                            a.Channel.ChannelDescription = address.ChannelCodeDescription;
                            a.Channel.ChannelTypeCode = address.ChannelTypeCode.ToString();
                            a.Channel.ChannelTypeDescription = address.ChannelTypeDescription;
                            a.ApartmentNumber = address.AptNum;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = address.ChannelCode;
                            a.Channel.ChannelDescription = address.ChannelCodeDescription;
                            a.Channel.ChannelTypeCode = address.ChannelTypeCode.ToString();
                            a.Channel.ChannelTypeDescription = address.ChannelTypeDescription;
                            a.City = address.City;
                            a.CompanyName = address.CompanyName;
                            a.Country = new MOBCountry();
                            a.Country.Code = address.CountryCode;
                            a.Country.Name = address.CountryName;
                            a.JobTitle = address.JobTitle;
                            a.Line1 = address.AddressLine1;
                            a.Line2 = address.AddressLine2;
                            a.Line3 = address.AddressLine3;
                            a.State = new Mobile.Model.Common.State();
                            a.State.Code = address.StateCode;
                            //a.State.Name = address.StateName;
                            a.IsDefault = address.IsDefault;
                            a.IsPrimary = address.IsPrimary;
                            a.IsPrivate = address.IsPrivate;
                            a.PostalCode = address.PostalCode;
                            //Adding this check for corporate addresses to gray out the Edit button on the client
                            //if (address.ChannelTypeDescription.ToLower().Trim() == "corporate")
                            //{
                            //    a.IsCorporate = true;
                            //}
                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                a.IsValidForTPIPurchase = IsValidAddressForTPIpayment(address.CountryCode);

                                if (a.IsValidForTPIPurchase)
                                {
                                    a.IsValidForTPIPurchase = IsValidSateForTPIpayment(address.StateCode);
                                }
                            }
                            mobAddresses.Add(a);
                        }
                    }
                }
            }
            return mobAddresses;
            #endregion
        }
        private bool IsInternationalBilling(MOBApplication application, string countryCode, string flow)
        {
            if (_configuration.GetValue<bool>("EnableUCBPhase1_MobilePhase3Changes") && String.IsNullOrEmpty(countryCode))
                return false;
            bool _isIntBilling = IsInternationalBillingAddress_CheckinFlowEnabled(application);
            if (_isIntBilling && flow?.ToLower() == FlowType.CHECKIN.ToString().ToLower()) // need to enable Int Billing address only in Checkin flow
            {
                //check for multiple countries
                return _isIntBilling;
            }
            else
            {
                //Normal Code as usual
                return _configuration.GetValue<string>("USPOSCountryCodes").Contains(countryCode);
            }
        }
        private bool IsValidSateForTPIpayment(string stateCode)
        {
            return !string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(_configuration.GetValue<string>("ExcludeUSStateCodesForTripInsurance")) && !_configuration.GetValue<string>("ExcludeUSStateCodesForTripInsurance").Contains(stateCode.ToUpper().Trim());
        }
        private bool IsValidAddressForTPIpayment(string countryCode)
        {
            return !string.IsNullOrEmpty(countryCode) && countryCode.ToUpper().Trim() == "US";
        }
        public List<MOBPrefAirPreference> PopulateAirPrefrences(List<United.Services.Customer.Common.AirPreference> airPreferences)
        {
            List<MOBPrefAirPreference> mobAirPrefs = new List<MOBPrefAirPreference>();
            if (airPreferences != null && airPreferences.Count > 0)
            {
                foreach (United.Services.Customer.Common.AirPreference pref in airPreferences)
                {
                    MOBPrefAirPreference mobAirPref = new MOBPrefAirPreference();
                    mobAirPref.AirportCode = pref.AirportCode;
                    mobAirPref.AirportCode = pref.AirportNameLong;
                    mobAirPref.AirportNameShort = pref.AirportNameShort;
                    mobAirPref.AirPreferenceId = pref.AirPreferenceId;
                    mobAirPref.ClassDescription = pref.ClassDescription;
                    mobAirPref.ClassId = pref.ClassId;
                    mobAirPref.CustomerId = pref.CustomerId;
                    mobAirPref.EquipmentCode = pref.EquipmentCode;
                    mobAirPref.EquipmentDesc = pref.EquipmentDesc;
                    mobAirPref.EquipmentId = pref.EquipmentId;
                    mobAirPref.IsActive = pref.IsActive;
                    mobAirPref.IsSelected = pref.IsSelected;
                    mobAirPref.IsNew = pref.IsNew;
                    mobAirPref.Key = pref.Key;
                    mobAirPref.LanguageCode = pref.LanguageCode;
                    mobAirPref.MealCode = pref.MealCode;
                    mobAirPref.MealDescription = pref.MealDescription;
                    mobAirPref.MealId = pref.MealId;
                    mobAirPref.NumOfFlightsDisplay = pref.NumOfFlightsDisplay;
                    mobAirPref.ProfileId = pref.ProfileId;
                    mobAirPref.SearchPreferenceDescription = pref.SearchPreferenceDescription;
                    mobAirPref.SearchPreferenceId = pref.SearchPreferenceId;
                    mobAirPref.SeatFrontBack = pref.SeatFrontBack;
                    mobAirPref.SeatSide = pref.SeatSide;
                    mobAirPref.SeatSideDescription = pref.SeatSideDescription;
                    mobAirPref.VendorCode = pref.VendorCode;
                    mobAirPref.VendorDescription = pref.VendorDescription;
                    mobAirPref.VendorId = pref.VendorId;
                    mobAirPref.AirRewardPrograms = GetAirRewardPrograms(pref.AirRewardPrograms);
                    mobAirPref.SpecialRequests = GetTravelerSpecialRequests(pref.SpecialRequests);
                    mobAirPref.ServiceAnimals = GetTravelerServiceAnimals(pref.ServiceAnimals);

                    mobAirPrefs.Add(mobAirPref);
                }
            }
            return mobAirPrefs;
        }
        private List<MOBPrefRewardProgram> GetAirRewardPrograms(List<United.Services.Customer.Common.RewardProgram> programs)
        {
            List<MOBPrefRewardProgram> mobAirRewardsProgs = new List<MOBPrefRewardProgram>();
            if (programs != null && programs.Count > 0)
            {
                foreach (United.Services.Customer.Common.RewardProgram pref in programs)
                {
                    MOBPrefRewardProgram mobAirRewardsProg = new MOBPrefRewardProgram();
                    mobAirRewardsProg.CustomerId = Convert.ToInt32(pref.CustomerId);
                    mobAirRewardsProg.ProfileId = Convert.ToInt32(pref.ProfileId);
                    //mobAirRewardsProg.ProgramCode = pref.ProgramCode;
                    //mobAirRewardsProg.ProgramDescription = pref.ProgramDescription;
                    mobAirRewardsProg.ProgramMemberId = pref.ProgramMemberId;
                    mobAirRewardsProg.VendorCode = pref.VendorCode;
                    mobAirRewardsProg.VendorDescription = pref.VendorDescription;
                    mobAirRewardsProgs.Add(mobAirRewardsProg);
                }
            }
            return mobAirRewardsProgs;
        }
        private List<MOBPrefSpecialRequest> GetTravelerSpecialRequests(List<United.Services.Customer.Common.SpecialRequest> specialRequests)
        {
            List<MOBPrefSpecialRequest> mobSpecialRequests = new List<MOBPrefSpecialRequest>();
            if (specialRequests != null && specialRequests.Count > 0)
            {
                foreach (United.Services.Customer.Common.SpecialRequest req in specialRequests)
                {
                    MOBPrefSpecialRequest mobSpecialRequest = new MOBPrefSpecialRequest();
                    mobSpecialRequest.AirPreferenceId = req.AirPreferenceId;
                    mobSpecialRequest.SpecialRequestId = req.SpecialRequestId;
                    mobSpecialRequest.SpecialRequestCode = req.SpecialRequestCode;
                    mobSpecialRequest.Key = req.Key;
                    mobSpecialRequest.LanguageCode = req.LanguageCode;
                    mobSpecialRequest.Description = req.Description;
                    mobSpecialRequest.Priority = req.Priority;
                    mobSpecialRequest.IsNew = req.IsNew;
                    mobSpecialRequest.IsSelected = req.IsSelected;
                    mobSpecialRequests.Add(mobSpecialRequest);
                }
            }
            return mobSpecialRequests;
        }
        private List<MOBPrefServiceAnimal> GetTravelerServiceAnimals(List<ServiceAnimal> serviceAnimals)
        {
            var results = new List<MOBPrefServiceAnimal>();

            if (serviceAnimals == null || !serviceAnimals.Any())
                return results;

            results = serviceAnimals.Select(x => new MOBPrefServiceAnimal
            {
                AirPreferenceId = x.AirPreferenceId,
                ServiceAnimalId = x.ServiceAnimalId,
                ServiceAnimalIdDesc = x.ServiceAnimalDesc,
                ServiceAnimalTypeId = x.ServiceAnimalTypeId,
                ServiceAnimalTypeIdDesc = x.ServiceAnimalTypeDesc,
                Key = x.Key,
                Priority = x.Priority,
                IsNew = x.IsNew,
                IsSelected = x.IsSelected
            }).ToList();

            return results;
        }
        private List<MOBEmail> PopulateAllEmailAddresses(List<Services.Customer.Common.Email> emailAddresses)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (Services.Customer.Common.Email email in emailAddresses)
                {
                    if (email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.Channel.ChannelCode = email.ChannelCode;
                        e.Channel.ChannelDescription = email.ChannelCodeDescription;
                        e.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                        e.Channel.ChannelTypeDescription = email.ChannelTypeDescription;
                        e.EmailAddress = email.EmailAddress;
                        e.IsDefault = email.IsDefault;
                        e.IsPrimary = email.IsPrimary;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.IsDayOfTravel;
                        mobEmailAddresses.Add(e);
                        #endregion
                    }
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        public List<MOBEmail> PopulateEmailAddresses(List<Services.Customer.Common.Email> emailAddresses, bool onlyDayOfTravelContact)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            bool isCorpEmailPresent = false;

            if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
            {
                //As per Business / DotCom Kalpen; we are removing the condition for checking the Effectivedate and Discontinued date
                var corpIndex = emailAddresses.FindIndex(x => x.ChannelTypeDescription != null && x.ChannelTypeDescription.ToLower() == "corporate" && x.EmailAddress != null && x.EmailAddress.Trim() != "");
                if (corpIndex >= 0)
                    isCorpEmailPresent = true;

            }

            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (Services.Customer.Common.Email email in emailAddresses)
                {
                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        if (isCorpEmailPresent && !onlyDayOfTravelContact && email.ChannelTypeDescription.ToLower() == "corporate")
                        {
                            primaryEmailAddress = new MOBEmail();
                            email.IsPrimary = true;
                            primaryEmailAddress.Key = email.Key;
                            primaryEmailAddress.Channel = new SHOPChannel();
                            primaryEmailAddress.EmailAddress = email.EmailAddress;
                            primaryEmailAddress.Channel.ChannelCode = email.ChannelCode;
                            primaryEmailAddress.Channel.ChannelDescription = email.ChannelCodeDescription;
                            primaryEmailAddress.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                            primaryEmailAddress.Channel.ChannelTypeDescription = email.ChannelTypeDescription;
                            primaryEmailAddress.IsDefault = email.IsDefault;
                            primaryEmailAddress.IsPrimary = email.IsPrimary;
                            primaryEmailAddress.IsPrivate = email.IsPrivate;
                            primaryEmailAddress.IsDayOfTravel = email.IsDayOfTravel;
                            if (!email.IsDayOfTravel)
                            {
                                break;
                            }

                        }
                        else if (isCorpEmailPresent && !onlyDayOfTravelContact && email.ChannelTypeDescription.ToLower() != "corporate")
                        {
                            continue;
                        }
                    }
                    //Fix for CheckOut ArgNull Exception - Empty EmailAddress with null EffectiveDate & DiscontinuedDate for Corp Account Revenue Booking (MOBILE-9873) - Shashank : Added OR condition to allow CorporateAccount ProfileOwner.
                    if ((email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow) ||
                            (!_configuration.GetValue<bool>("DisableCheckforCorpAccEmail") && email.ChannelTypeDescription.ToLower() == "corporate"
                            && email.IsProfileOwner == true && primaryEmailAddress.IsNullOrEmpty()))
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.EmailAddress = email.EmailAddress;
                        e.Channel.ChannelCode = email.ChannelCode;
                        e.Channel.ChannelDescription = email.ChannelCodeDescription;
                        e.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                        e.Channel.ChannelTypeDescription = email.ChannelTypeDescription;
                        e.IsDefault = email.IsDefault;
                        e.IsPrimary = email.IsPrimary;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.IsDayOfTravel;
                        if (email.IsDayOfTravel)
                        {
                            primaryEmailAddress = new MOBEmail();
                            primaryEmailAddress = e;
                            if (onlyDayOfTravelContact)
                            {
                                break;
                            }
                        }
                        if (!onlyDayOfTravelContact)
                        {
                            if (email.IsPrimary)
                            {
                                primaryEmailAddress = new MOBEmail();
                                primaryEmailAddress = e;
                                break;
                            }
                            else if (co == 1)
                            {
                                primaryEmailAddress = new MOBEmail();
                                primaryEmailAddress = e;
                            }
                        }
                        #endregion
                    }
                }
                if (primaryEmailAddress != null)
                {
                    mobEmailAddresses.Add(primaryEmailAddress);
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        public List<MOBCPPhone> PopulatePhones(List<United.Services.Customer.Common.Phone> phones, bool onlyDayOfTravelContact)
        {
            List<MOBCPPhone> mobCPPhones = new List<MOBCPPhone>();
            bool isCorpPhonePresent = false;


            if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
            {
                var corpIndex = phones.FindIndex(x => x.ChannelTypeDescription != null && x.ChannelTypeDescription.ToLower() == "corporate" && x.PhoneNumber != null && x.PhoneNumber != "");
                if (corpIndex >= 0)
                    isCorpPhonePresent = true;
            }


            if (phones != null && phones.Count > 0)
            {
                MOBCPPhone primaryMobCPPhone = null;
                CultureInfo ci = GeneralHelper.EnableUSCultureInfo();
                int co = 0;
                foreach (United.Services.Customer.Common.Phone phone in phones)
                {
                    #region As per Wade Change want to filter out to return only Primary Phone to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                    MOBCPPhone mobCPPhone = new MOBCPPhone();
                    co = co + 1;

                    mobCPPhone.AreaNumber = phone.AreaNumber;
                    mobCPPhone.PhoneNumber = phone.PhoneNumber;

                    mobCPPhone.Attention = phone.Attention;
                    mobCPPhone.ChannelCode = phone.ChannelCode;
                    mobCPPhone.ChannelCodeDescription = phone.ChannelCodeDescription;
                    mobCPPhone.ChannelTypeCode = Convert.ToString(phone.ChannelTypeCode);
                    mobCPPhone.ChannelTypeDescription = phone.ChannelTypeDescription;
                    mobCPPhone.ChannelTypeDescription = phone.ChannelTypeDescription;
                    mobCPPhone.ChannelTypeSeqNumber = phone.ChannelTypeSeqNum;
                    mobCPPhone.CountryCode = phone.CountryCode;
                    //mobCPPhone.CountryCode = GetAccessCode(phone.CountryCode);
                    mobCPPhone.CountryPhoneNumber = phone.CountryPhoneNumber;
                    mobCPPhone.CustomerId = phone.CustomerId;
                    mobCPPhone.Description = phone.Description;
                    mobCPPhone.DiscontinuedDate = Convert.ToString(phone.DiscontinuedDate);
                    mobCPPhone.EffectiveDate = Convert.ToString(phone.EffectiveDate);
                    mobCPPhone.ExtensionNumber = phone.ExtensionNumber;
                    mobCPPhone.IsPrimary = phone.IsPrimary;
                    mobCPPhone.IsPrivate = phone.IsPrivate;
                    mobCPPhone.IsProfileOwner = phone.IsProfileOwner;
                    mobCPPhone.Key = phone.Key;
                    mobCPPhone.LanguageCode = phone.LanguageCode;
                    mobCPPhone.PagerPinNumber = phone.PagerPinNumber;
                    mobCPPhone.SharesCountryCode = phone.SharesCountryCode;
                    mobCPPhone.WrongPhoneDate = Convert.ToString(phone.WrongPhoneDate);
                    if (phone.PhoneDevices != null && phone.PhoneDevices.Count > 0)
                    {
                        mobCPPhone.DeviceTypeCode = phone.PhoneDevices[0].CommDeviceTypeCode;
                        mobCPPhone.DeviceTypeDescription = phone.PhoneDevices[0].CommDeviceTypeDescription;
                    }
                    mobCPPhone.IsDayOfTravel = phone.IsDayOfTravel;

                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        #region
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.ChannelTypeDescription.ToLower() == "corporate")
                        {
                            //return the corporate phone number
                            primaryMobCPPhone = new MOBCPPhone();
                            mobCPPhone.IsPrimary = true;
                            primaryMobCPPhone = mobCPPhone;
                            break;

                        }
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.ChannelTypeDescription.ToLower() != "corporate")
                        {
                            //There is corporate phone number present, continue till corporate phone number is found
                            continue;
                        }
                        #endregion
                    }

                    if (phone.IsDayOfTravel)
                    {
                        primaryMobCPPhone = new MOBCPPhone();
                        primaryMobCPPhone = mobCPPhone;// Only day of travel contact should be returned to use at Edit Traveler
                        if (onlyDayOfTravelContact)
                        {
                            break;
                        }
                    }
                    if (!onlyDayOfTravelContact)
                    {
                        if (phone.IsPrimary)
                        {
                            primaryMobCPPhone = new MOBCPPhone();
                            primaryMobCPPhone = mobCPPhone;
                            break;
                        }
                        else if (co == 1)
                        {
                            primaryMobCPPhone = new MOBCPPhone();
                            primaryMobCPPhone = mobCPPhone;
                        }
                    }
                    #endregion
                }
                if (primaryMobCPPhone != null)
                {
                    mobCPPhones.Add(primaryMobCPPhone);
                }
                GeneralHelper.DisableUSCultureInfo(ci);
            }
            return mobCPPhones;
        }
        private List<MOBBKLoyaltyProgramProfile> GetTravelerLoyaltyProfile(List<United.Services.Customer.Common.AirPreference> airPreferences, int currentEliteLevel)
        {
            List<MOBBKLoyaltyProgramProfile> programs = new List<MOBBKLoyaltyProgramProfile>();
            //if(airPreferences != null && airPreferences.Count > 0 && airPreferences[0].AirRewardPrograms != null && airPreferences[0].AirRewardPrograms.Count > 0) 
            if (airPreferences != null && airPreferences.Count > 0)
            {
                #region
                List<United.Services.Customer.Common.AirPreference> airPreferencesList = new List<Services.Customer.Common.AirPreference>();
                airPreferencesList = (from item in airPreferences
                                      where item.AirRewardPrograms != null && item.AirRewardPrograms.Count > 0
                                      select item).ToList();
                //foreach(United.Services.Customer.Common.RewardProgram rewardProgram in airPreferences[0].AirRewardPrograms) 
                if (airPreferencesList != null && airPreferencesList.Count > 0)
                {
                    foreach (United.Services.Customer.Common.RewardProgram rewardProgram in airPreferencesList[0].AirRewardPrograms)
                    {
                        MOBBKLoyaltyProgramProfile airRewardProgram = new MOBBKLoyaltyProgramProfile();
                        airRewardProgram.ProgramId = rewardProgram.ProgramID.ToString();
                        airRewardProgram.ProgramName = rewardProgram.Description;
                        airRewardProgram.MemberId = rewardProgram.ProgramMemberId;
                        airRewardProgram.CarrierCode = rewardProgram.VendorCode;
                        if (airRewardProgram.CarrierCode.Trim().Equals("UA"))
                        {
                            airRewardProgram.MPEliteLevel = currentEliteLevel;
                        }
                        airRewardProgram.RewardProgramKey = rewardProgram.Key;
                        programs.Add(airRewardProgram);
                    }
                }
                #endregion
            }
            return programs;
        }
        public async Task<MOBCPMileagePlus> GetCurrentEliteLevelFromAirPreferences(List<AirPreference> airPreferences, string sessionid)
        {
            MOBCPMileagePlus mobCPMileagePlus = null;
            if (_configuration.GetValue<bool>("BugFixToggleFor17M") &&
                airPreferences != null &&
                airPreferences.Count > 0 &&
                airPreferences[0].AirRewardPrograms != null &&
                airPreferences[0].AirRewardPrograms.Count > 0)
            {
                mobCPMileagePlus = await GetCurrentEliteLevelFromAirRewardProgram(airPreferences, sessionid);
            }

            return mobCPMileagePlus;
        }
        private async Task<MOBCPMileagePlus> GetCurrentEliteLevelFromAirRewardProgram(List<AirPreference> airPreferences, string sessionid)
        {
            MOBCPMileagePlus mobCPMileagePlus = null;
            var airRewardProgram = airPreferences[0].AirRewardPrograms[0];
            if (!string.IsNullOrEmpty(airRewardProgram.ProgramMemberId))
            {
                Session session = new Session();
                session = await _sessionHelperService.GetSession<Session>(_headers.ContextValues.SessionId, session.ObjectName, new List<string>() { _headers.ContextValues.SessionId, session.ObjectName }).ConfigureAwait(false);

                MOBCPProfileRequest request = new MOBCPProfileRequest();
                request.CustomerId = 0;
                request.MileagePlusNumber = airRewardProgram.ProgramMemberId;
                United.Services.Customer.Common.ProfileRequest profileRequest = (new ProfileRequest(_configuration, IsCorpBookingPath)).GetProfileRequest(request);
                string jsonRequest = JsonSerializer.Serialize<United.Services.Customer.Common.ProfileRequest>(profileRequest);
                string url = string.Format("/GetProfile");

                //Utility utility = new Utility();
                var jsonresponse = await MakeHTTPPostAndLogIt(session.SessionId, session.DeviceID, "GetProfileForTravelerToGetEliteLevel", session.AppID, string.Empty, session.Token, url, jsonRequest);
                mobCPMileagePlus = GetOwnerEliteLevelFromCslResponse(jsonresponse);
            }
            return mobCPMileagePlus;
        }

        private MOBCPMileagePlus GetOwnerEliteLevelFromCslResponse(string jsonresponse)
        {
            MOBCPMileagePlus mobCPMileagePlus = null;
            if (!string.IsNullOrEmpty(jsonresponse))
            {
                United.Services.Customer.Common.ProfileResponse response = JsonSerializer.Deserialize<United.Services.Customer.Common.ProfileResponse>(jsonresponse);
                if (response != null && response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) &&
                    response.Profiles != null &&
                    response.Profiles.Count > 0 &&
                    response.Profiles[0].Travelers != null &&
                    response.Profiles[0].Travelers.Exists(p => p.IsProfileOwner))
                {
                    var owner = response.Profiles[0].Travelers.First(p => p.IsProfileOwner);
                    if (owner != null & owner.MileagePlus != null)
                    {
                        mobCPMileagePlus = PopulateMileagePlus(owner.MileagePlus);
                    }
                }
            }

            return mobCPMileagePlus;
        }

        private async Task<string> MakeHTTPPostAndLogIt(string sessionId, string deviceId, string action, int applicationId, string appVersion, string token, string url, string jsonRequest, bool isXMLRequest = false)
        {
            ////logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "URL", applicationId, appVersion, deviceId, url, true, true));
            ////logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "Request", applicationId, appVersion, deviceId, jsonRequest, true, true));
            string jsonResponse = string.Empty;

            string paypalCSLCallDurations = string.Empty;
            string callTime4Tuning = string.Empty;

            #region//****Get Call Duration Code*******
            Stopwatch cslCallDurationstopwatch1;
            cslCallDurationstopwatch1 = new Stopwatch();
            cslCallDurationstopwatch1.Reset();
            cslCallDurationstopwatch1.Start();
            #endregion

            string applicationRequestType = isXMLRequest ? "xml" : "json";
            //jsonResponse = HttpHelper.Post(url, "application/" + applicationRequestType + "; charset=utf-8", token, jsonRequest, httpPostTimeOut, httpPostNumberOfRetry);
            var response = await _customerDataService.GetCustomerData<United.Services.Customer.Common.ProfileRequest>(token, sessionId, jsonRequest);
            #region
            if (cslCallDurationstopwatch1.IsRunning)
            {
                cslCallDurationstopwatch1.Stop();
            }
            paypalCSLCallDurations = paypalCSLCallDurations + "|2=" + cslCallDurationstopwatch1.ElapsedMilliseconds.ToString() + "|"; // 2 = shopCSLCallDurationstopwatch1
            callTime4Tuning = "|CSL =" + (cslCallDurationstopwatch1.ElapsedMilliseconds / (double)1000).ToString();
            #endregion
            //check
            return response.ToString();
        }

        private string GetPaxDescriptionByDOB(string date, string deptDateFLOF)
        {
            int age = TopHelper.GetAgeByDOB(date, deptDateFLOF);
            if ((18 <= age) && (age <= 64))
            {
                return "Adult (18-64)";
            }
            else
            if ((2 <= age) && (age < 5))
            {
                return "Child (2-4)";
            }
            else
            if ((5 <= age) && (age <= 11))
            {
                return "Child (5-11)";
            }
            else
            //if((12 <= age) && (age <= 17))
            //{

            //}
            if ((12 <= age) && (age <= 14))
            {
                return "Child (12-14)";
            }
            else
            if ((15 <= age) && (age <= 17))
            {
                return "Child (15-17)";
            }
            else
            if (65 <= age)
            {
                return "Senior (65+)";
            }
            else if (age < 2)
                return "Infant (under 2)";

            return string.Empty;
        }
        public string GetYAPaxDescByDOB()
        {
            return "Young adult (18-23)";
        }
        private bool EnableYADesc(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableYoungAdultBooking") && _configuration.GetValue<bool>("EnableYADesc") && !isReshop;
        }
        public List<MOBCPSecureTraveler> PopulatorSecureTravelers(List<United.Services.Customer.Common.SecureTraveler> secureTravelers, ref bool isTSAFlag, bool correctDate, string sessionID, int appID, string deviceID)
        {
            List<MOBCPSecureTraveler> mobSecureTravelers = null;
            try
            {
                if (secureTravelers != null && secureTravelers.Count > 0)
                {
                    mobSecureTravelers = new List<MOBCPSecureTraveler>();
                    int secureTravelerCount = 0;
                    foreach (var secureTraveler in secureTravelers)
                    {
                        if (secureTraveler.DocumentType != null && secureTraveler.DocumentType.Trim().ToUpper() != "X" && secureTravelerCount < 3)
                        {
                            #region
                            MOBCPSecureTraveler mobSecureTraveler = new MOBCPSecureTraveler();
                            if (correctDate)
                            {
                                DateTime tempBirthDate = secureTraveler.BirthDate.GetValueOrDefault().AddHours(1);
                                mobSecureTraveler.BirthDate = tempBirthDate.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                            }
                            else
                            {
                                mobSecureTraveler.BirthDate = secureTraveler.BirthDate.GetValueOrDefault().ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                            }
                            mobSecureTraveler.CustomerId = secureTraveler.CustomerId;
                            mobSecureTraveler.DecumentType = secureTraveler.DocumentType;
                            mobSecureTraveler.Description = secureTraveler.Description;
                            mobSecureTraveler.FirstName = secureTraveler.FirstName;
                            mobSecureTraveler.Gender = secureTraveler.Gender;
                            mobSecureTraveler.Key = secureTraveler.Key;
                            mobSecureTraveler.LastName = secureTraveler.LastName;
                            mobSecureTraveler.MiddleName = secureTraveler.MiddleName;
                            mobSecureTraveler.SequenceNumber = secureTraveler.SequenceNumber;
                            mobSecureTraveler.Suffix = secureTraveler.Suffix;
                            if (secureTraveler.SupplementaryTravelInfos != null)
                            {
                                foreach (Services.Customer.Common.SupplementaryTravelInfo supplementaryTraveler in secureTraveler.SupplementaryTravelInfos)
                                {
                                    if (supplementaryTraveler.Type == Services.Customer.Common.Constants.SupplementaryTravelInfoNumberType.KnownTraveler)
                                    {
                                        mobSecureTraveler.KnownTravelerNumber = supplementaryTraveler.Number;
                                    }
                                    if (supplementaryTraveler.Type == Services.Customer.Common.Constants.SupplementaryTravelInfoNumberType.Redress)
                                    {
                                        mobSecureTraveler.RedressNumber = supplementaryTraveler.Number;
                                    }
                                }
                            }
                            if (!isTSAFlag && secureTraveler.DocumentType.Trim().ToUpper() == "U")
                            {
                                isTSAFlag = true;
                            }
                            if (secureTraveler.DocumentType.Trim().ToUpper() == "C" || secureTraveler.DocumentType.Trim() == "") // This is to get only Customer Cleared Secure Traveler records
                            {
                                mobSecureTravelers = new List<MOBCPSecureTraveler>();
                                mobSecureTravelers.Add(mobSecureTraveler);
                                secureTravelerCount = 4;
                            }
                            else
                            {
                                mobSecureTravelers.Add(mobSecureTraveler);
                                secureTravelerCount = secureTravelerCount + 1;
                            }
                            #endregion
                        }
                        else if (secureTravelerCount > 3)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PopulatorSecureTravelers {@Exception} for {@SecureTravelers}", JsonConvert.SerializeObject(ex), JsonConvert.SerializeObject(secureTravelers));
            }

            return mobSecureTravelers;
        }
        private MOBCPMileagePlus PopulateMileagePlus(United.Services.Customer.Common.MileagePlus onePass)
        {
            MOBCPMileagePlus mileagePlus = null;
            if (onePass != null)
            {
                mileagePlus = new MOBCPMileagePlus();
                mileagePlus.AccountBalance = onePass.AccountBalance;
                mileagePlus.ActiveStatusCode = onePass.ActiveStatusCode;
                mileagePlus.ActiveStatusDescription = onePass.ActiveStatusDescription;
                mileagePlus.AllianceEliteLevel = onePass.AllianceEliteLevel;
                mileagePlus.ClosedStatusCode = onePass.ClosedStatusCode;
                mileagePlus.ClosedStatusDescription = onePass.ClosedStatusDescription;
                mileagePlus.CurrentEliteLevel = onePass.CurrentEliteLevel;
                if (onePass.CurrentEliteLevelDescription != null)
                {
                    mileagePlus.CurrentEliteLevelDescription = onePass.CurrentEliteLevelDescription.ToString().ToUpper() == "NON-ELITE" ? "General member" : onePass.CurrentEliteLevelDescription;
                }
                mileagePlus.CurrentYearMoneySpent = onePass.CurrentYearMoneySpent;
                mileagePlus.EliteMileageBalance = onePass.EliteMileageBalance;
                mileagePlus.EliteSegmentBalance = Convert.ToInt32(onePass.EliteSegmentBalance);
                //mileagePlus.EliteSegmentDecimalPlaceValue = onePass.elite;
                mileagePlus.EncryptedPin = onePass.EncryptedPin;
                mileagePlus.EnrollDate = onePass.EnrollDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.EnrollSourceCode = onePass.EnrollSourceCode;
                mileagePlus.EnrollSourceDescription = onePass.EnrollSourceDescription;
                mileagePlus.FlexPqmBalance = onePass.FlexPQMBalance;
                mileagePlus.FutureEliteDescription = onePass.FutureEliteLevelDescription;
                mileagePlus.FutureEliteLevel = onePass.FutureEliteLevel;
                mileagePlus.InstantEliteExpirationDate = onePass.FutureEliteLevelDescription;
                mileagePlus.IsCEO = onePass.IsCEO;
                mileagePlus.IsClosedPermanently = onePass.IsClosedPermanently;
                mileagePlus.IsClosedTemporarily = onePass.IsClosedTemporarily;
                mileagePlus.IsCurrentTrialEliteMember = onePass.IsCurrentTrialEliteMember;
                mileagePlus.IsFlexPqm = onePass.IsFlexPQM;
                mileagePlus.IsInfiniteElite = onePass.IsInfiniteElite;
                mileagePlus.IsLifetimeCompanion = onePass.IsLifetimeCompanion;
                mileagePlus.IsLockedOut = onePass.IsLockedOut;
                mileagePlus.IsPresidentialPlus = onePass.IsPresidentialPlus;
                mileagePlus.IsUnitedClubMember = onePass.IsPClubMember;
                mileagePlus.Key = onePass.Key;
                mileagePlus.LastActivityDate = onePass.LastActivityDate;
                mileagePlus.LastExpiredMile = onePass.LastExpiredMile;
                mileagePlus.LastFlightDate = onePass.LastFlightDate;
                mileagePlus.LastStatementBalance = onePass.LastStatementBalance;
                mileagePlus.LastStatementDate = onePass.LastStatementDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.LifetimeEliteMileageBalance = onePass.LifetimeEliteMileageBalance;
                mileagePlus.MileageExpirationDate = onePass.MileageExpirationDate;
                mileagePlus.MileagePlusId = onePass.MileagePlusId;
                mileagePlus.MileagePlusPin = onePass.MileagePlusPIN;
                mileagePlus.NextYearEliteLevel = onePass.NextYearEliteLevel;
                mileagePlus.NextYearEliteLevelDescription = onePass.NextYearEliteLevelDescription;
                mileagePlus.PriorUnitedAccountNumber = onePass.PriorUnitedAccountNumber;
                mileagePlus.StarAllianceEliteLevel = onePass.SkyTeamEliteLevelCode;
                if (!_configuration.GetValue<bool>("Keep_MREST_MP_EliteLevel_Expiration_Logic"))
                {
                    mileagePlus.PremierLevelExpirationDate = onePass.PremierLevelExpirationDate;
                    if (onePass.CurrentYearInstantElite != null)
                    {
                        mileagePlus.InstantElite = new MOBInstantElite()
                        {
                            ConsolidatedCode = onePass.CurrentYearInstantElite.ConsolidatedCode,
                            EffectiveDate = onePass.CurrentYearInstantElite.EffectiveDate != null ? onePass.CurrentYearInstantElite.EffectiveDate.ToString("MM/dd/yyyy") : string.Empty,
                            EliteLevel = onePass.CurrentYearInstantElite.EliteLevel,
                            EliteYear = onePass.CurrentYearInstantElite.EliteYear,
                            ExpirationDate = onePass.CurrentYearInstantElite.ExpirationDate != null ? onePass.CurrentYearInstantElite.ExpirationDate.ToString("MM/dd/yyyy") : string.Empty,
                            PromotionCode = onePass.CurrentYearInstantElite.PromotionCode
                        };
                    }
                }
            }

            return mileagePlus;
        }
        private MOBCPCustomerMetrics PopulateCustomerMetrics(United.Services.Customer.Common.CustomerMetrics customerMetrics)
        {
            MOBCPCustomerMetrics travelerCustomerMetrics = new MOBCPCustomerMetrics();
            if (customerMetrics != null && customerMetrics.PTCCode != null)
            {
                travelerCustomerMetrics.PTCCode = customerMetrics.PTCCode;
            }
            return travelerCustomerMetrics;
        }
        private async Task<Reservation> PersistedReservation(MOBCPProfileRequest request)
        {
            Reservation persistedReservation =
                new Reservation();
            if (request != null)
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
            return persistedReservation;
        }

        #region updated member profile start
        private async Task<(bool returnValue, string stateCode)> GetAndValidateStateCode(MOBUpdateTravelerRequest request,  string stateCode)
        {
            bool validStateCode = false;
            #region
            string urlPath = string.Format("/StatesFilter?State={0}&CountryCode={1}&Language={2}", request.Traveler.Addresses[0].State.Code, request.Traveler.Addresses[0].Country.Code, request.LanguageCode);
            _logger.LogInformation("GetAndValidateStateCode - {url} {request} and {sessionID}", urlPath, JsonConvert.SerializeObject(request), _headers.ContextValues.SessionId);
            string jsonResponse = await _referencedataService.GetAndValidateStateCode(request.Token, urlPath, _headers.ContextValues.SessionId);
            if (!string.IsNullOrEmpty(jsonResponse))
            {
                List<United.Service.Presentation.CommonModel.StateProvince> response = JsonSerializer.Deserialize<List<United.Service.Presentation.CommonModel.StateProvince>>(jsonResponse);

                if (response != null && response.Count == 1 && !string.IsNullOrEmpty(response[0].StateProvinceCode))
                {
                    stateCode = response[0].StateProvinceCode;
                    validStateCode = true;
                    _logger.LogInformation("GetAndValidateStateCode - {url} {response} and {sessionID}", urlPath, JsonConvert.SerializeObject(response), _headers.ContextValues.SessionId);
                }
                else
                {
                    string exceptionMessage = _configuration.GetValue<string>("UnableToGetAndValidateStateCode");
                    throw new MOBUnitedException(exceptionMessage);
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToGetAndValidateStateCode");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && Convert.ToBoolean(_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting")))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  GetCommonUsedDataList()";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion
            return (validStateCode,stateCode);
        }
        public async Task<bool> UpdateTravelerBase(MOBUpdateTravelerRequest request)
        {
            bool updateTravelerSuccess = false;
            #region
            UpdateTravelerBaseRequest updateTraveler = GetUpdateTravelerRequest(request);
            string jsonRequest = JsonSerializer.Serialize<UpdateTravelerBaseRequest>(updateTraveler);
            string urlPath = "/UpdateTravelerBase";          
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, urlPath);
            if (response != null)
            {
                if (response != null && response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    updateTravelerSuccess = true;
                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }
                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateTravelerBaseProfileErrorMessage");
                        if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL UpdateTravelerBase(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateTravelerBaseProfileErrorMessage");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  UpdateTravelerBase(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion
            return updateTravelerSuccess;
        }
        public async System.Threading.Tasks.Task UpdateViewName(MOBUpdateTravelerRequest request)
        {
            var bookingPathReservation = await _sessionHelperService.GetSession<United.Mobile.Model.Shopping.Reservation>(request.SessionId, new Reservation().ObjectName, new List<string> { request.SessionId, new Reservation().ObjectName }).ConfigureAwait(false);
            if (bookingPathReservation != null && bookingPathReservation.ShopReservationInfo2 != null && !string.IsNullOrEmpty(bookingPathReservation.ShopReservationInfo2.NextViewName) &&
                !bookingPathReservation.ShopReservationInfo2.NextViewName.ToUpper().Equals("TRAVELOPTION"))
            {
                if (bookingPathReservation.TravelersCSL != null && bookingPathReservation.TravelersCSL.Count > 0)
                {
                    foreach (var t in bookingPathReservation.TravelersCSL)
                    {
                        if (t.Value.Key.Equals(request.Traveler.Key))
                        {
                            if (_configuration.GetValue<bool>("EnableTravelerNationalityChangeFix"))
                            {
                                if (!t.Value.FirstName.ToUpper().Equals(request.Traveler.FirstName.ToUpper()) || !t.Value.LastName.ToUpper().Equals(request.Traveler.LastName.ToUpper()) || !t.Value.BirthDate.Equals(request.Traveler.BirthDate)
                                || !string.Equals(t.Value.Nationality, request.Traveler.Nationality, StringComparison.OrdinalIgnoreCase))
                                {
                                    bookingPathReservation.ShopReservationInfo2.NextViewName = "TravelOption";
                                    await _sessionHelperService.SaveSession(bookingPathReservation, request.SessionId, new List<string> { request.SessionId, new Reservation().ObjectName }).ConfigureAwait(false);
                                }
                            }
                            else
                            {
                                if (!t.Value.FirstName.ToUpper().Equals(request.Traveler.FirstName.ToUpper()) || !t.Value.LastName.ToUpper().Equals(request.Traveler.LastName.ToUpper()) || !t.Value.BirthDate.Equals(request.Traveler.BirthDate))
                                {
                                    bookingPathReservation.ShopReservationInfo2.NextViewName = "TravelOption";
                                    await _sessionHelperService.SaveSession(bookingPathReservation, request.SessionId, new List<string> { request.SessionId, new Reservation().ObjectName }).ConfigureAwait(false);
                                }

                            }
                        }
                    }
                }
            }
        }
        private United.Services.Customer.Common.UpdateTravelerBaseRequest GetUpdateTravelerRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.UpdateTravelerBaseRequest travelUpdateRequest = new Services.Customer.Common.UpdateTravelerBaseRequest();
            if (request.UpdateTravelerBasiInfo)
            {
                #region
                travelUpdateRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices"); ;
                travelUpdateRequest.Title = request.Traveler.Title;
                travelUpdateRequest.FirstName = request.Traveler.FirstName;
                travelUpdateRequest.MiddleName = request.Traveler.MiddleName;
                travelUpdateRequest.LastName = request.Traveler.LastName;
                travelUpdateRequest.Suffix = request.Traveler.Suffix;
                travelUpdateRequest.DateOfBirth = new DateTime();
                int offSetHours = _configuration.GetValue<string>("UpdateTravelerDOBOffSetHours") == null ? 12 : _configuration.GetValue<int>("UpdateTravelerDOBOffSetHours");
                travelUpdateRequest.DateOfBirth = DateTime.ParseExact(request.Traveler.BirthDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddHours(offSetHours);
                travelUpdateRequest.Gender = request.Traveler.GenderCode;
                travelUpdateRequest.KnownTravelerNumber = request.Traveler.KnownTravelerNumber;
                travelUpdateRequest.RedressNumber = request.Traveler.RedressNumber;
                travelUpdateRequest.UpdateId = request.Traveler.CustomerId.ToString();
                travelUpdateRequest.CustomerId = request.Traveler.CustomerId;
                travelUpdateRequest.ProfileId = request.Traveler.ProfileId;
                travelUpdateRequest.TravelerKey = request.Traveler.Key;
                //Need to be updated once Nuget get updated to 7.1
                if (IsEnabledNationalityAndResidence(false, request.Application.Id, request.Application.Version.Major))
                {
                    if (request.Traveler != null && !string.IsNullOrEmpty(request.Traveler.Nationality) && request.Traveler.Nationality.ToUpper().Equals("OTHER"))
                    {
                        travelUpdateRequest.Nationality = null;
                    }
                    else
                    {
                        travelUpdateRequest.Nationality = request.Traveler.Nationality;
                    }

                    if (request.Traveler != null && !string.IsNullOrEmpty(request.Traveler.CountryOfResidence) && request.Traveler.CountryOfResidence.ToUpper().Equals("OTHER"))
                    {
                        travelUpdateRequest.CountryOfResidence = null;
                    }
                    else
                    {
                        travelUpdateRequest.CountryOfResidence = request.Traveler.CountryOfResidence;
                    }
                }
                #endregion
            }

            #endregion
            return travelUpdateRequest;
        }
        private bool UpdateMealPreference(MOBUpdateTravelerRequest request)
        {
            var success = true;
            // Implement update UpdateMealPreference here
            var updateSpecialRequestsRequest = new UpdateMealPreferenceRequest();

            return success;
        }
        private bool UpdateServiceAnimals(MOBUpdateTravelerRequest request)
        {
            var success = true;
            // Implement update UpdateServiceAnimals here
            var updateSpecialRequestsRequest = new UpdateServiceAnimalRequest();

            return success;
        }
        private bool UpdateSpecialRequests(MOBUpdateTravelerRequest request)
        {
            var success = true;
            // Implement update special requests here
            var updateSpecialRequestsRequest = new UpdateSpecialReqsRequest();

            return success;
        }
        private bool EnableSpecialNeeds(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSpecialNeeds")
                    && GeneralHelper.isApplicationVersionGreater(appId, appVersion, "AndroidEnableSpecialNeedsVersion", "iPhoneEnableSpecialNeedsVersion", "", "", true, _configuration);
        }
        private async Task<bool> isPhoneNumberChanged(MOBUpdateTravelerRequest request)
        {
            bool isChanged = true;
            try
            {
                if (_configuration.GetValue<bool>("BugFixToggleFor17M") && !string.IsNullOrEmpty(request.SessionId))
                {
                    Reservation persistedReservation =
                    new Reservation();
                    if (request != null)
                        persistedReservation = await _sessionHelperService.GetSession<Reservation>(request.SessionId, new Reservation().ObjectName, new List<string> { request.SessionId, new Reservation().ObjectName }).ConfigureAwait(false);
                    if (persistedReservation != null &&
                        persistedReservation.ReservationPhone != null &&
                        request.Traveler != null &&
                        request.Traveler.Phones != null &&
                        request.Traveler.Phones.Count > 0 &&
                        request.Traveler.Phones[0].AreaNumber == persistedReservation.ReservationPhone.AreaNumber &&
                        request.Traveler.Phones[0].PhoneNumber == persistedReservation.ReservationPhone.PhoneNumber)
                    {
                        isChanged = false;
                    }
                }
            }
            catch { }
            return isChanged;
        }
        private async Task<(bool returnValue, string insertedAddressKey)> InsertTravelerAddress(MOBUpdateTravelerRequest request,  string insertedAddressKey)
        {
            bool insertTravelerAddress = false;
            #region
            United.Services.Customer.Common.InsertAddressRequest insertAddress = GetInsertAddressRequest(request);
            string jsonRequest = JsonSerializer.Serialize<United.Services.Customer.Common.InsertAddressRequest>(insertAddress);

            string urlPath = "/InsertAddress";
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, urlPath);

            if (response != null)
            {
                if (response != null && response.Status.Equals(Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    var obj = (from st in response.ReturnValues
                               where st.Key.ToUpper().Trim() == "AddressKey".ToUpper().Trim()
                               select st).ToList();
                    insertedAddressKey = obj[0].Value;
                    insertTravelerAddress = true;
                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }

                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToInsertAddressToProfileErrorMessage");
                        if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL InsertTravelerAddress(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToInsertAddressToProfileErrorMessage");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  InsertTravelerAddress(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion
            return (insertTravelerAddress, insertedAddressKey);
        }
        private United.Services.Customer.Common.InsertAddressRequest GetInsertAddressRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.InsertAddressRequest addressInsertRequest = new Services.Customer.Common.InsertAddressRequest();
            if (request.UpdateAddressInfoAssociatedWithCC && request.Traveler.Addresses != null && request.Traveler.Addresses.Count > 0)
            {
                addressInsertRequest.TravelerKey = request.Traveler.Key;
                addressInsertRequest.CustomerId = request.Traveler.CustomerId;
                addressInsertRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
                addressInsertRequest.AddressLine1 = request.Traveler.Addresses[0].Line1;
                addressInsertRequest.AddressLine2 = request.Traveler.Addresses[0].Line2;
                addressInsertRequest.AddressLine3 = request.Traveler.Addresses[0].Line3;
                addressInsertRequest.ChannelTypeCode = request.Traveler.Addresses[0].Channel != null ? request.Traveler.Addresses[0].Channel.ChannelTypeCode : "H";
                addressInsertRequest.City = request.Traveler.Addresses[0].City;
                addressInsertRequest.CountryCode = request.Traveler.Addresses[0].Country != null ? request.Traveler.Addresses[0].Country.Code : "";
                addressInsertRequest.PostalCode = request.Traveler.Addresses[0].PostalCode;
                addressInsertRequest.StateCode = request.Traveler.Addresses[0].State != null ? request.Traveler.Addresses[0].State.Code : "";
                addressInsertRequest.Description = ""; //--> Confirmed with Edwards Babu better to pass emtpy for description as if we pass for EX: Home two times even with different address then second time this service customerdata/api/InsertAddress 
                //will return error as "UserFriendlyMessage=The Address Description entered already exists in your Saved Addresses. Please revise the description." to avoid this error beter pass as empty.
                //request.Traveler.Addresses[0].Channel != null ? request.Traveler.Addresses[0].Channel.ChannelTypeDescription : "";  
                addressInsertRequest.InsertId = request.Traveler.CustomerId.ToString();
                addressInsertRequest.LangCode = request.LanguageCode;
            }
            #endregion
            return addressInsertRequest;
        }
        private async Task<(bool returnValue, string ccKey)> InsertTravelerCreditCard(MOBUpdateTravelerRequest request, string ccKey)
        {
            bool insertTravelerCreditCard = false;

            #region
            United.Services.Customer.Common.InsertCreditCardRequest insertAddress = await GetInsertCreditCardRequest(request);
            string jsonRequest = JsonSerializer.Serialize<United.Services.Customer.Common.InsertCreditCardRequest>(insertAddress);
            string urlPath = "/InsertCreditCard";
           
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, urlPath);

            if (response != null)
            {
                if (response != null && response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    var creditCardKey = (from t in response.ReturnValues
                                         where t.Key.ToUpper().Trim() == "CreditCardKey".ToUpper().Trim()
                                         select t).ToList();
                    insertTravelerCreditCard = true;
                    ccKey = creditCardKey[0].Value;

                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            //Alekhya - Null check Conditon for Message has been added - Aug 16,2016
                            if (!String.IsNullOrEmpty(error.Message) && error.Message.ToLower().Contains("ora-") && error.Message.ToLower().Contains("unique constraint") && error.Message.Contains("violated"))
                                errorMessage = errorMessage + " " + _configuration.GetValue<string>("CCAlreadyExistMessage");
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }
                        errorMessage = string.IsNullOrWhiteSpace(errorMessage) ? _configuration.GetValue<string>("Booking2OGenericExceptionMessage") : errorMessage;
                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                        if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL InsertTravelerCreditCard(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  InsertTravelerCreditCard(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion

            return (insertTravelerCreditCard,ccKey);
        }
        private async Task<(bool returnValue, string ccKey)> UpdateTravelerCreditCard(MOBUpdateTravelerRequest request, string ccKey)
        {
            bool updateTravelerCreditCard = false;

            #region
            United.Services.Customer.Common.UpdateCreditCardRequest updateCreditCard = await GetUpdateCreditCardRequest(request);
            string jsonRequest = JsonSerializer.Serialize<United.Services.Customer.Common.UpdateCreditCardRequest>(updateCreditCard);

            string urlPath = string.Format("/UpdateCreditCard");
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, urlPath);

            if (response != null)
            {
                if (response != null && response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    var creditCardKey = (from t in response.ReturnValues
                                         where t.Key.ToUpper().Trim() == "CreditCardKey".ToUpper().Trim()
                                         select t).ToList();
                    ccKey = creditCardKey[0].Value;
                    updateTravelerCreditCard = true;

                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }

                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateCreditCardToProfileErrorMessage");
                        if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL UpdateTravelerCreditCard(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateCreditCardToProfileErrorMessage");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  UpdateTravelerCreditCard(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion

            return (updateTravelerCreditCard,ccKey);
        }
        private async Task<United.Services.Customer.Common.UpdateCreditCardRequest> GetUpdateCreditCardRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.UpdateCreditCardRequest creditCardUpdateRequest = new Services.Customer.Common.UpdateCreditCardRequest();
            string accountNumberToken = request.Traveler.CreditCards[0].AccountNumberToken;
            bool CCTokenDataVaultCheck = true;
            //if (!request.Traveler.CreditCards[0].UnencryptedCardNumber.Contains("XXXXXXXXXXXX"))
            if (!string.IsNullOrEmpty(request.Traveler.CreditCards[0].EncryptedCardNumber))
            {
                var tupleResponse = await _profileCreditCard.GenerateCCTokenWithDataVault(request.Traveler.CreditCards[0], request.SessionId, request.Token, request.Application, request.DeviceId, accountNumberToken);
                CCTokenDataVaultCheck = tupleResponse.Item1;
                accountNumberToken = tupleResponse.ccDataVaultToken;
            }
            if (request.UpdateCreditCardInfo && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && CCTokenDataVaultCheck)
            {
                creditCardUpdateRequest.CreditCardKey = request.Traveler.CreditCards[0].Key;
                //creditCardUpdateRequest.NewCreditCardNumber = request.Traveler.CreditCards[0].UnencryptedCardNumber;
                if (_configuration.GetValue<string>("17BHotFix_DoNotAllowEditExistingSavedCreditCardNumber") != null &&
                    Convert.ToBoolean(_configuration.GetValue<string>("17BHotFix_DoNotAllowEditExistingSavedCreditCardNumber")))
                {
                    creditCardUpdateRequest.AccountNumberToken = request.Traveler.CreditCards[0].AccountNumberToken;
                }
                else
                {
                    creditCardUpdateRequest.AccountNumberToken = accountNumberToken;
                }
                creditCardUpdateRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
                creditCardUpdateRequest.AddressKey = request.Traveler.CreditCards[0].AddressKey;
                creditCardUpdateRequest.CreditCardType = request.Traveler.CreditCards[0].CardType;
                creditCardUpdateRequest.ExpirationMonth = Convert.ToInt32(request.Traveler.CreditCards[0].ExpireMonth.Trim());
                creditCardUpdateRequest.ExpirationYear = Convert.ToInt32(request.Traveler.CreditCards[0].ExpireYear.Trim().Length == 2 ? "20" + request.Traveler.CreditCards[0].ExpireYear.Trim() : request.Traveler.CreditCards[0].ExpireYear.Trim());
                creditCardUpdateRequest.Name = request.Traveler.CreditCards[0].cCName;
                creditCardUpdateRequest.PhoneKey = request.Traveler.CreditCards[0].PhoneKey;
                creditCardUpdateRequest.Description = request.Traveler.CreditCards[0].Description;
                creditCardUpdateRequest.UpdateId = request.Traveler.CustomerId.ToString();
                creditCardUpdateRequest.LangCode = request.LanguageCode;
            }
            #endregion
            return creditCardUpdateRequest;
        }
        private async Task<bool> InsertTravelerRewardProgram(MOBUpdateTravelerRequest request)
        {
            bool insertTravelerRewardProgram = false;
            United.Services.Customer.Common.InsertRewardProgramRequest insertRewardProgram = GetInsertRewardProgramRequest(request);
            string jsonRequest = JsonSerializer.Serialize<United.Services.Customer.Common.InsertRewardProgramRequest>(insertRewardProgram);

            string url = "/InsertRewardProgram";
         
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, url);

            if (response != null)
            {
                if (response != null && response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    insertTravelerRewardProgram = true;
                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }

                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToInsertTravelerRewardProgramToProfileErrorMessage");
                        if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL InsertTravelerRewardProgram(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToInsertTravelerRewardProgramToProfileErrorMessage");
                if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  InsertTravelerRewardProgram(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            return insertTravelerRewardProgram;
        }
        private United.Services.Customer.Common.InsertRewardProgramRequest GetInsertRewardProgramRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.InsertRewardProgramRequest rewardInsertRequest = new Services.Customer.Common.InsertRewardProgramRequest();

            if (request.UpdateRewardProgramInfo && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0)
            {
                rewardInsertRequest.TravelerKey = request.Traveler.Key;
                rewardInsertRequest.ProfileId = request.Traveler.ProfileId;
                rewardInsertRequest.CustomerId = request.Traveler.CustomerId;
                rewardInsertRequest.ProgramId = Convert.ToInt32(request.Traveler.AirRewardPrograms[0].ProgramId);
                rewardInsertRequest.ProgramMemberId = request.Traveler.AirRewardPrograms[0].MemberId;
                rewardInsertRequest.InsertId = request.Traveler.CustomerId.ToString();
                rewardInsertRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
                rewardInsertRequest.LangCode = request.LanguageCode;
            }
            #endregion
            return rewardInsertRequest;
        }
        private async Task<(bool emailChanged, bool rewardProgramUpdated)> isRewardProgramChanged(MOBUpdateTravelerRequest request, bool emailChanged, bool rewardProgramUpdated)
        {
            emailChanged = false; rewardProgramUpdated = false;
            try
            {
                if (!string.IsNullOrEmpty(request.SessionId))
                {
                    Reservation persistedReservation = new Reservation();
                    if (request != null)
                        persistedReservation = await _sessionHelperService.GetSession<Reservation>(request.SessionId, persistedReservation.ObjectName, new List<string> { request.SessionId, persistedReservation.ObjectName }).ConfigureAwait(false);

                    if (persistedReservation != null &&
                        persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.AllEligibleTravelersCSL != null && persistedReservation.ShopReservationInfo2.AllEligibleTravelersCSL.Count > 0 &&
                        (_configuration.GetValue<bool>("EnableAirRewardProgramsNullCheck") ? (request.Traveler != null && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0) : true))//Removing the null check for airreward program ..Since there is check for email updating logic which doesnt require reward program to be there in the request and before updating the reward program we have null check inside this block.
                    {
                        var traveler = persistedReservation.ShopReservationInfo2.AllEligibleTravelersCSL.Where(t => !string.IsNullOrEmpty(t.Key) && t.Key.ToUpper().Equals(request.Traveler.Key.ToUpper())).FirstOrDefault();
                        if (traveler != null)
                        {
                            if (request.UpdateEmailInfo == true && request.Traveler.EmailAddresses != null && request.Traveler.EmailAddresses.Count > 0 && !String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key) && !String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].EmailAddress))
                            {
                                if (traveler.EmailAddresses != null && traveler.EmailAddresses.Count > 0)
                                {
                                    foreach (var email in traveler.EmailAddresses.Where(e => !string.IsNullOrEmpty(e.Key)))
                                    {
                                        if (email.Key.ToUpper().Equals(request.Traveler.EmailAddresses[0].Key.ToUpper()) && (!email.EmailAddress.ToUpper().Equals(request.Traveler.EmailAddresses[0].EmailAddress.ToUpper())))
                                        {
                                            emailChanged = true;
                                        }
                                    }
                                }
                            }
                            if (request.UpdateRewardProgramInfo == true && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0 && !String.IsNullOrEmpty(request.Traveler.AirRewardPrograms[0].RewardProgramKey))
                            {
                                if (!request.Traveler.IsProfileOwner || (request.Traveler.IsProfileOwner && request.Traveler.AirRewardPrograms[0].CarrierCode.Trim().ToUpper() != "UA")) // to check not to update profile owner UA MP details. It will fail if trying to update the MP Account of the profile owner
                                {
                                    if (traveler.AirRewardPrograms != null && traveler.AirRewardPrograms.Count > 0)
                                    {
                                        foreach (var prog in traveler.AirRewardPrograms.Where(p => p != null && !string.IsNullOrEmpty(p.RewardProgramKey)))
                                        {
                                            var rewardInfo = request.Traveler.AirRewardPrograms.Where(arp => !string.IsNullOrEmpty(arp.RewardProgramKey) && arp.RewardProgramKey.Equals(prog.RewardProgramKey)).FirstOrDefault();
                                            if (rewardInfo != null && (!rewardInfo.MemberId.ToUpper().Equals(prog.MemberId.ToUpper()) || (!rewardInfo.ProgramName.ToUpper().Equals(prog.ProgramName.ToUpper()))))
                                            {
                                                rewardProgramUpdated = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return (emailChanged, rewardProgramUpdated);
        }
        private async Task<bool> UpdateTravelerRewardProgram(MOBUpdateTravelerRequest request)
        {
            bool updateTravelerRewardProgram = false;
            #region
            UpdateRewardProgramRequest updateRewardProgram = GetUpdateRewardProgramRequest(request);
            string jsonRequest = JsonSerializer.Serialize<UpdateRewardProgramRequest>(updateRewardProgram);

            string url = "/UpdateRewardProgram";
            var response = await _customerDataService.GetProfile<SaveResponse>(request.Token, jsonRequest, _headers.ContextValues.SessionId, url);

            if (response != null)
            {
                if (response != null && response.Status.Equals(Services.Customer.Common.Constants.StatusType.Success) && response.ReturnValues != null)
                {
                    updateTravelerRewardProgram = true;
                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }

                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateTravelerRewardProgramToProfileErrorMessage");
                        if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                        {
                            exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL UpdateTravelerRewardProgram(MOBUpdateTravelerRequest request)";
                        }
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToUpdateTravelerRewardProgramToProfileErrorMessage");
                if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  UpdateTravelerRewardProgram(MOBUpdateTravelerRequest request)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }
            #endregion
            return updateTravelerRewardProgram;
        }
        private UpdateRewardProgramRequest GetUpdateRewardProgramRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.UpdateRewardProgramRequest rewardUpdateRequest = new Services.Customer.Common.UpdateRewardProgramRequest();

            if (request.UpdateRewardProgramInfo && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0)
            {
                rewardUpdateRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
                rewardUpdateRequest.ProgramId = Convert.ToInt32(request.Traveler.AirRewardPrograms[0].ProgramId);
                rewardUpdateRequest.ProgramMemberId = request.Traveler.AirRewardPrograms[0].MemberId;
                rewardUpdateRequest.UpdateId = request.Traveler.CustomerId.ToString();
                rewardUpdateRequest.RewardProgramKey = request.Traveler.AirRewardPrograms[0].RewardProgramKey;
                rewardUpdateRequest.PreferenceId = Convert.ToInt32(request.Traveler.AirRewardPrograms[0].ProgramId);//**//--> Need to confirm whats this value could be or where this value would be at get profile resposne?
                rewardUpdateRequest.LangCode = request.LanguageCode;
            }
            #endregion
            return rewardUpdateRequest;
        }
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
        private async Task<United.Services.Customer.Common.InsertCreditCardRequest> GetInsertCreditCardRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            United.Services.Customer.Common.InsertCreditCardRequest creditCardInsertRequest = new Services.Customer.Common.InsertCreditCardRequest();
            string accountNumberToken = string.Empty;
            var tupleResponse = await _profileCreditCard.GenerateCCTokenWithDataVault(request.Traveler.CreditCards[0], request.SessionId, request.Token, request.Application, request.DeviceId, accountNumberToken);
            accountNumberToken = tupleResponse.ccDataVaultToken;
            if (request.UpdateCreditCardInfo && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && !string.IsNullOrEmpty(request.Traveler.CreditCards[0].EncryptedCardNumber) && tupleResponse.Item1)
            {
                creditCardInsertRequest.TravelerKey = request.Traveler.Key;
                creditCardInsertRequest.CustomerId = request.Traveler.CustomerId;
                creditCardInsertRequest.DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices");
                creditCardInsertRequest.AddressKey = request.Traveler.CreditCards[0].AddressKey;
                //creditCardInsertRequest.CreditCardNumber = request.Traveler.CreditCards[0].UnencryptedCardNumber;
                creditCardInsertRequest.AccountNumberToken = accountNumberToken;
                creditCardInsertRequest.CreditCardType = request.Traveler.CreditCards[0].CardType;
                creditCardInsertRequest.ExpirationMonth = Convert.ToInt32(request.Traveler.CreditCards[0].ExpireMonth.Trim());
                //creditCardInsertRequest.ExpirationYear = Convert.ToInt32(request.Traveler.CreditCards[0].ExpireYear.Trim());
                creditCardInsertRequest.ExpirationYear = Convert.ToInt32(request.Traveler.CreditCards[0].ExpireYear.Trim().Length == 2 ? "20" + request.Traveler.CreditCards[0].ExpireYear.Trim() : request.Traveler.CreditCards[0].ExpireYear.Trim());
                creditCardInsertRequest.Name = request.Traveler.CreditCards[0].cCName;
                creditCardInsertRequest.PhoneKey = request.Traveler.CreditCards[0].PhoneKey;
                creditCardInsertRequest.Description = request.Traveler.CreditCards[0].Description;
                creditCardInsertRequest.InsertId = request.Traveler.CustomerId.ToString();
                creditCardInsertRequest.LangCode = request.LanguageCode;
            }
            #endregion
            return creditCardInsertRequest;
        }

        #endregion updated member profile End



        #endregion

        public bool IsInternationalBillingAddress_CheckinFlowEnabled(MOBApplication application)
        {
            if (_configuration.GetValue<bool>("EnableInternationalBillingAddress_CheckinFlow"))
            {
                if (application != null && GeneralHelper.IsApplicationVersionGreater(application.Id, application.Version.Major, "IntBillingCheckinFlowAndroidversion", "IntBillingCheckinFlowiOSversion", "", "", true, _configuration))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<(bool updateTravelerSuccess, List<MOBItem> insertUpdateItemKeys)> UpdateTravelerV2(MOBUpdateTravelerRequest request, List<MOBItem> insertUpdateItemKeys)
        {
            bool updateTravelerSuccess = false;
            if (string.IsNullOrEmpty(request.MileagePlusNumber))
            {
                throw new MOBUnitedException("Profile Owner MileagePlus number is required.");
            }

            if (request.UpdateAddressInfoAssociatedWithCC == true && request.Traveler.Addresses != null && request.Traveler.Addresses.Count > 0 && request.Traveler.Addresses[0].Country.Code.Trim() == "US")
            {
                string stateCode = string.Empty;
                var tupleResponse = await GetAndValidateStateCode(request, stateCode);
                stateCode = tupleResponse.stateCode;
                if (tupleResponse.returnValue)
                {
                    request.Traveler.Addresses[0].State.Code = stateCode;
                }
            }
            if (request.UpdateTravelerBasiInfo == true && !String.IsNullOrEmpty(request.Traveler.Key))
            {
                updateTravelerSuccess = await UpdateTravelerBase(request);

                if (!request.UpdateCreditCardInfo && !request.UpdateAddressInfoAssociatedWithCC)
                {
                    await UpdateViewName(request);
                }
            }

            bool isEmailUpdated = default, isRewardProgramUpdated=default;
            var tupleRes = await isRewardProgramChanged(request,  isEmailUpdated,  isRewardProgramUpdated);
            isEmailUpdated = tupleRes.emailChanged;
            isRewardProgramUpdated = tupleRes.rewardProgramUpdated;

            if ((request.UpdatePhoneInfo == true
                                            && request.Traveler.Phones != null
                                            && request.Traveler.Phones.Count > 0
                                            && !String.IsNullOrEmpty(request.Traveler.Phones[0].PhoneNumber)
                                            && !String.IsNullOrEmpty(request.Traveler.Phones[0].CountryCode)
                                            &&
                                            (
                                               String.IsNullOrEmpty(request.Traveler.Phones[0].Key)
                                               ||
                                               (!String.IsNullOrEmpty(request.Traveler.Phones[0].Key) && await isPhoneNumberChanged(request))
                                            ))
              ||
                (
                 request.UpdateEmailInfo == true
                                           && request.Traveler.EmailAddresses != null
                                           && request.Traveler.EmailAddresses.Count > 0
                                           && !String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].EmailAddress)
                                           &&
                                           (
                                            String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key)
                                            ||
                                            (!String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key) && isEmailUpdated)
                                           )

                ))
            {

                updateTravelerSuccess = await InsertOrUpdateTravelerEmailPhone(request);
            }

            if (request.UpdateRewardProgramInfo == true && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0 && !String.IsNullOrEmpty(request.Traveler.AirRewardPrograms[0].RewardProgramKey))
            {
                if (!request.Traveler.IsProfileOwner || (request.Traveler.IsProfileOwner && request.Traveler.AirRewardPrograms[0].CarrierCode.Trim().ToUpper() != "UA")) // to check not to update profile owner UA MP details. It will fail if trying to update the MP Account of the profile owner
                {
                    if (isRewardProgramUpdated)
                    {
                        updateTravelerSuccess = await UpdateTravelerRewardProgram(request);
                    }
                }
            }
            else if (request.UpdateRewardProgramInfo == true && request.Traveler.AirRewardPrograms != null && request.Traveler.AirRewardPrograms.Count > 0 && String.IsNullOrEmpty(request.Traveler.AirRewardPrograms[0].RewardProgramKey))
            {
                updateTravelerSuccess =await InsertTravelerRewardProgram(request);
            }
            if (request.UpdateAddressInfoAssociatedWithCC || request.UpdateCreditCardInfo)
            {
                insertUpdateItemKeys = new List<MOBItem>();
                string addressKey = string.Empty, ccKey = string.Empty;
                if (Convert.ToBoolean(_configuration.GetValue<string>("CorporateConcurBooking") ?? "false"))
                {
                    #region
                    if (request.UpdateAddressInfoAssociatedWithCC == true && request.Traveler.Addresses != null && request.Traveler.Addresses.Count > 0 && !request.Traveler.Addresses[0].IsCorporate)
                    {
                        var insertOrUpdateTravelerAddressResponse = await InsertOrUpdateTravelerAddress(request, addressKey); //After UCB migration service is same for updating or Inserting the address..So combined both into one service
                        updateTravelerSuccess = insertOrUpdateTravelerAddressResponse.isSuccess;
                        addressKey = insertOrUpdateTravelerAddressResponse.addressKey;
                        MOBItem item = new MOBItem();
                        item.Id = "AddressKey";
                        item.CurrentValue = addressKey;
                        insertUpdateItemKeys.Add(item);
                    }
                    else if (request.UpdateAddressInfoAssociatedWithCC == true && request.Traveler.Addresses != null && request.Traveler.Addresses.Count > 0 && !String.IsNullOrEmpty(request.Traveler.Addresses[0].Key) && request.Traveler.Addresses[0].IsCorporate)
                    {
                        var tupleValue = await InsertTravelerAddress(request, addressKey);
                        updateTravelerSuccess = tupleValue.returnValue;
                        addressKey = tupleValue.insertedAddressKey;
                        MOBItem item = new MOBItem();
                        item.Id = "AddressKey";
                        item.CurrentValue = request.Traveler.Addresses[0].Key;
                        insertUpdateItemKeys.Add(item);
                    }
                    if (_configuration.GetValue<string>("NotAllowUpdateAddressKeyForCorporateCC") != null && Convert.ToBoolean(_configuration.GetValue<string>("NotAllowUpdateAddressKeyForCorporateCC").ToString()))
                    {
                        #region
                        if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && !String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && !request.Traveler.CreditCards[0].IsCorporate)
                        {
                            request.Traveler.CreditCards[0].AddressKey = addressKey;
                            var tupleValue = await UpdateTravelerCreditCard(request,  ccKey); // Same here the CC key going to change after the update.
                            updateTravelerSuccess = tupleValue.returnValue;
                            ccKey = tupleValue.ccKey;
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = ccKey;
                            insertUpdateItemKeys.Add(item);
                        }
                        else if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && !request.Traveler.CreditCards[0].IsCorporate)
                        {
                            request.Traveler.CreditCards[0].AddressKey = addressKey;
                            var tupleValue = await InsertTravelerCreditCard(request, ccKey);
                            updateTravelerSuccess = tupleValue.returnValue;
                            ccKey = tupleValue.ccKey;
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = ccKey;
                            insertUpdateItemKeys.Add(item);
                        }
                        else if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && !String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && request.Traveler.CreditCards[0].IsCorporate)
                        {
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = request.Traveler.CreditCards[0].Key;
                            insertUpdateItemKeys.Add(item);
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && !String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && !(request.Traveler.CreditCards[0].IsCorporate && request.Traveler.Addresses[0].IsCorporate))
                        {
                            request.Traveler.CreditCards[0].AddressKey = addressKey;
                            var tupleValue = await UpdateTravelerCreditCard(request, ccKey); // Same here the CC key going to change after the update.
                            updateTravelerSuccess = tupleValue.returnValue;
                            ccKey = tupleValue.ccKey;
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = ccKey;
                            insertUpdateItemKeys.Add(item);
                        }
                        else if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && !request.Traveler.CreditCards[0].IsCorporate)
                        {
                            request.Traveler.CreditCards[0].AddressKey = addressKey;
                            var tupleValue = await InsertTravelerCreditCard(request, ccKey);
                            updateTravelerSuccess = tupleValue.returnValue;
                            ccKey = tupleValue.ccKey;
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = ccKey;
                            insertUpdateItemKeys.Add(item);
                        }
                        else if (request.UpdateCreditCardInfo == true && request.Traveler.CreditCards != null && request.Traveler.CreditCards.Count > 0 && !String.IsNullOrEmpty(request.Traveler.CreditCards[0].Key) && request.Traveler.CreditCards[0].IsCorporate)
                        {
                            MOBItem item = new MOBItem();
                            item.Id = "CreditCardKey";
                            item.CurrentValue = request.Traveler.CreditCards[0].Key;
                            insertUpdateItemKeys.Add(item);
                        }

                        #endregion
                    }
                    #endregion
                }

            }

            if (EnableSpecialNeeds(request.Application.Id, request.Application.Version.Major))
            {
                if (request.UpdateSpecialRequests)
                {
                    updateTravelerSuccess = UpdateSpecialRequests(request);
                }

                if (request.UpdateServiceAnimals)
                {
                    updateTravelerSuccess = UpdateServiceAnimals(request);
                }

                if (request.UpdateMealPreference)
                {
                    updateTravelerSuccess = UpdateMealPreference(request);
                }
            }

            return (updateTravelerSuccess, insertUpdateItemKeys);
        }


        public async Task<bool> InsertOrUpdateTravelerEmailPhone(MOBUpdateTravelerRequest request)
        {
            bool isSuccess = false;

            string jsonResponse = string.Empty;
            #region
            UpdateMemberContact insertOrUpdateRequest = GetInsertOrUpdateTravelerEmailPhone(request);

            string jsonRequest = JsonSerializer.Serialize<UpdateMemberContact>(insertOrUpdateRequest);
            _logger.LogInformation("InsertOrUpdateTravelerEmailPhone - Request {ApplicationId} {DeviceId} {JsonRequest} {ApplicationVersion}", request.Application.Id, request.DeviceId, jsonRequest, request.Application.Version.Major);

            #region//****Get Call Duration Code - Venkat 03/17/2015*******
            Stopwatch cslStopWatch;
            cslStopWatch = new Stopwatch();
            cslStopWatch.Reset();
            cslStopWatch.Start();
            #endregion//****Get Call Duration Code - Venkat 03/17/2015*******     

            jsonResponse = await _insertOrUpdateTravelInfoService.InsertOrUpdateTravelerInfo(request.Traveler.CustomerId, jsonRequest, request.Token);


            #region// 2 = cslStopWatch//****Get Call Duration Code - Venkat 03/17/2015*******
            if (cslStopWatch.IsRunning)
            {
                cslStopWatch.Stop();
            }
            string cslCallTime = (cslStopWatch.ElapsedMilliseconds / (double)1000).ToString();

            _logger.LogInformation("InsertOrUpdateTravelerEmailPhone - Response {ApplicationId} {ApplicationVersion} {DeviceId} {cslCallTime}", request.Application.Id, request.Application.Version.Major, request.DeviceId, cslCallTime);

            #endregion//****Get Call Duration Code - Venkat 03/17/2015*******    

            _logger.LogInformation("InsertOrUpdateTravelerEmailPhone - Response {ApplicationId} {ApplicationVersion} {DeviceId} {reponse}", request.Application.Id, request.Application.Version.Major, request.DeviceId, jsonResponse);


            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<Mobile.Model.CSLModels.CslResponse<UpdateContactResponse>>(jsonResponse);

                if (responseData.Data != null && responseData.Errors == null)
                {
                    UpdateContactResponse response = new UpdateContactResponse();
                    response = responseData.Data;
                    if (!string.IsNullOrEmpty(response.PhoneKey) || !string.IsNullOrEmpty(response.EmailKey))
                    {
                        isSuccess = true;
                    }
                    _logger.LogInformation("InsertOrUpdateTravelerAddress - DeSerialized Response {ApplicationId} {DeviceId} {reponse}", request.Application.Id, request.Application.Version.Major, request.DeviceId, response);

                }
            }
            #endregion
            return isSuccess;
        }

        private UpdateMemberContact GetInsertOrUpdateTravelerEmailPhone(MOBUpdateTravelerRequest request)
        {
            UpdateMemberContact insertOrUpdateTravelerEmailPhone = new UpdateMemberContact();
            insertOrUpdateTravelerEmailPhone.CustomerId = request.Traveler.CustomerId.ToString();
            #region Build insert or Update Traveler Phone request

            if (request.UpdatePhoneInfo
              && request.Traveler.Phones != null
              && request.Traveler.Phones.Count > 0
              && (!String.IsNullOrEmpty(request.Traveler.Phones[0].Key) || (!String.IsNullOrEmpty(request.Traveler.Phones[0].AreaNumber) && !String.IsNullOrEmpty(request.Traveler.Phones[0].PhoneNumber)))
              )
            {
                //Rajesh K need to fix
                insertOrUpdateTravelerEmailPhone.Phone = new United.Mobile.Model.CSLModels.Phone();
                Mobile.Model.CSLModels.Constants.PhoneType phoneType;
                Mobile.Model.CSLModels.Constants.DeviceType deviceType;
                #region Convert channelTyCode in request to Enum
                if (!String.IsNullOrEmpty(request.Traveler.Phones[0].ChannelTypeCode))
                {
                    System.Enum.TryParse<Mobile.Model.CSLModels.Constants.PhoneType>(request.Traveler.Phones[0].ChannelTypeCode, out phoneType);
                }
                else
                {
                    phoneType = Mobile.Model.CSLModels.Constants.PhoneType.H;
                }
                #endregion
                #region Convert DeviceTypeCode in request to Enum
                if (!String.IsNullOrEmpty(request.Traveler.Phones[0].DeviceTypeCode))
                {
                    System.Enum.TryParse<Mobile.Model.CSLModels.Constants.DeviceType>(request.Traveler.Phones[0].DeviceTypeCode, out deviceType);
                }
                else
                {
                    deviceType = Mobile.Model.CSLModels.Constants.DeviceType.PH;
                }
                #endregion
                if ((!String.IsNullOrEmpty(request.Traveler.Phones[0].Key)))
                {
                    insertOrUpdateTravelerEmailPhone.Phone.Key = request.Traveler.Phones[0].Key;
                    insertOrUpdateTravelerEmailPhone.Phone.UpdateId = request.Traveler.CustomerId.ToString();
                }
                else
                {
                    insertOrUpdateTravelerEmailPhone.Phone.Type = phoneType;
                    insertOrUpdateTravelerEmailPhone.Phone.InsertId = request.Traveler.CustomerId.ToString();
                }
                insertOrUpdateTravelerEmailPhone.Phone.Number = (request.Traveler.Phones[0].AreaNumber + request.Traveler.Phones[0].PhoneNumber).Trim();
                insertOrUpdateTravelerEmailPhone.Phone.CountryCode = string.IsNullOrEmpty(request.Traveler.Phones[0].CountryCode) ? "US" : request.Traveler.Phones[0].CountryCode.Trim();
                insertOrUpdateTravelerEmailPhone.Phone.DeviceType = deviceType;
                if (!request.UpdateCreditCardInfo && String.IsNullOrEmpty(request.Traveler.Phones[0].Key))
                {
                    insertOrUpdateTravelerEmailPhone.Phone.DayOfTravelNotification = true;
                }

                insertOrUpdateTravelerEmailPhone.Phone.Remark = request.Traveler.Phones[0].Description;
                insertOrUpdateTravelerEmailPhone.Phone.LanguageCode = request.LanguageCode;
            }
            #endregion

            #region Build Insert or Update Traveler Email request
            if (request.UpdateEmailInfo
                        && request.Traveler.EmailAddresses != null
                        && request.Traveler.EmailAddresses.Count > 0
                        && (!String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key) || !String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].EmailAddress))
                        )
            {               
                Mobile.Model.CSLModels.Constants.EmailType emailType;
                #region Convert channelTyCode in request to Enum
                if (request.Traveler.EmailAddresses[0].Channel != null && !String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Channel.ChannelCode))
                {
                    System.Enum.TryParse<Mobile.Model.CSLModels.Constants.EmailType>(request.Traveler.EmailAddresses[0].Channel.ChannelCode, out emailType);
                }
                else
                {
                    emailType = Mobile.Model.CSLModels.Constants.EmailType.O;
                }
                #endregion
                //Rajesh K need to fix
                insertOrUpdateTravelerEmailPhone.Email = new United.Mobile.Model.CSLModels.Email();
                insertOrUpdateTravelerEmailPhone.Email.Address = request.Traveler.EmailAddresses[0].EmailAddress;
                insertOrUpdateTravelerEmailPhone.Email.Type = emailType;
                if (!String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key))
                {
                    insertOrUpdateTravelerEmailPhone.Email.Key = request.Traveler.EmailAddresses[0].Key;
                    insertOrUpdateTravelerEmailPhone.Email.UpdateId = request.Traveler.CustomerId.ToString();
                }
                else
                {
                    insertOrUpdateTravelerEmailPhone.Email.InsertId = request.Traveler.CustomerId.ToString();
                }
                insertOrUpdateTravelerEmailPhone.Email.LanguageCode = request.LanguageCode;

                if (!request.UpdateCreditCardInfo && String.IsNullOrEmpty(request.Traveler.EmailAddresses[0].Key))
                {
                    insertOrUpdateTravelerEmailPhone.Email.DayOfTravelNotification = true;
                }
            }
            #endregion

            return insertOrUpdateTravelerEmailPhone;
        }

        public async Task<(bool isSuccess, string addressKey)> InsertOrUpdateTravelerAddress(MOBUpdateTravelerRequest request, string addressKey)
        {
            bool isSuccess = false;
            string jsonResponse = string.Empty;
            try
            {

                UpdateMemberContact insertOrUpdateAddress = GetInsertOrUpdateAddressRequest(request);
                string jsonRequest = JsonSerializer.Serialize<UpdateMemberContact>(insertOrUpdateAddress);

                _logger.LogInformation("InsertOrUpdateTravelerAddress - Request,  {SessionId} {ApplicationId} {ApplicationVersion} {DeviceId} {jsonRequest} ", request.SessionId, request.Application.Id, request.Application.Version.Major, request.DeviceId, jsonRequest);

                #region//****Get Call Duration Code - Venkat 03/17/2015*******
                Stopwatch cslStopWatch;
                cslStopWatch = new Stopwatch();
                cslStopWatch.Reset();
                cslStopWatch.Start();
                #endregion//****Get Call Duration Code - Venkat 03/17/2015*******

                jsonResponse = await _insertOrUpdateTravelInfoService.InsertOrUpdateTravelerInfo(request.Traveler.CustomerId, jsonRequest, request.Token, true);

                #region// 2 = cslStopWatch//****Get Call Duration Code - Venkat 03/17/2015*******
                if (cslStopWatch.IsRunning)
                {
                    cslStopWatch.Stop();
                }

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<Mobile.Model.CSLModels.CslResponse<UpdateContactResponse>>(jsonResponse);

                    if (responseData.Data != null && responseData.Errors == null)
                    {
                        UpdateContactResponse response = new UpdateContactResponse();
                        response = responseData.Data;

                        addressKey = response.AddressKey;
                        isSuccess = true;

                        _logger.LogInformation("InsertOrUpdateTravelerAddress - DeSerialized Response,  {SessionId} {ApplicationId} {ApplicationVersion} {DeviceId} {jsonRequest} ", request.SessionId, request.Application.Id, request.Application.Version.Major, request.DeviceId, response);

                    }

                }

                #endregion
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return (isSuccess, addressKey);
        }

        private UpdateMemberContact GetInsertOrUpdateAddressRequest(MOBUpdateTravelerRequest request)
        {
            #region Build insert Traverl Request
            UpdateMemberContact addressInsertOrUpdateRequest = new UpdateMemberContact();
            if (request.UpdateAddressInfoAssociatedWithCC && request.Traveler.Addresses != null && request.Traveler.Addresses.Count > 0)
            {
                addressInsertOrUpdateRequest.Address = new United.Mobile.Model.CSLModels.Address();
                if (!string.IsNullOrEmpty(request.Traveler.Addresses[0].Key))
                {
                    addressInsertOrUpdateRequest.Address.Key = request.Traveler.Addresses[0].Key;
                    addressInsertOrUpdateRequest.Address.UpdateId = request.Traveler.CustomerId.ToString();
                }
                else
                {
                    addressInsertOrUpdateRequest.Address.InsertId = request.Traveler.CustomerId.ToString();
                }
                addressInsertOrUpdateRequest.CustomerId = request.Traveler.CustomerId.ToString();
                addressInsertOrUpdateRequest.Address.Line1 = request.Traveler.Addresses[0].Line1;
                addressInsertOrUpdateRequest.Address.Line2 = request.Traveler.Addresses[0].Line2;
                addressInsertOrUpdateRequest.Address.Line3 = request.Traveler.Addresses[0].Line3;
                addressInsertOrUpdateRequest.Address.City = request.Traveler.Addresses[0].City;
                addressInsertOrUpdateRequest.Address.CountryCode = request.Traveler.Addresses[0].Country != null ? request.Traveler.Addresses[0].Country.Code : "";
                addressInsertOrUpdateRequest.Address.PostalCode = request.Traveler.Addresses[0].PostalCode;
                addressInsertOrUpdateRequest.Address.StateCode = request.Traveler.Addresses[0].State != null ? request.Traveler.Addresses[0].State.Code : "";
                addressInsertOrUpdateRequest.Address.LanguageCode = request.LanguageCode;
            }
            #endregion
            return addressInsertOrUpdateRequest;
        }

        #region UCB Migration MobilePhase3
        public async Task<(List<MOBCPTraveler> mobTravelersOwnerFirstInList, bool isProfileOwnerTSAFlagOn, List<MOBKVP> savedTravelersMPList)> PopulateTravelersV2(List<TravelerProfileResponse> travelers, string mileagePluNumber, bool isProfileOwnerTSAFlagOn, bool isGetCreditCardDetailsCall, MOBCPProfileRequest request, string sessionid, bool getMPSecurityDetails = false, string path = "")
        {
            var savedTravelersMPList = new List<MOBKVP>();
            List<MOBCPTraveler> mobTravelers = null;
            List<MOBCPTraveler> mobTravelersOwnerFirstInList = null;
            MOBCPTraveler profileOwnerDetails = new MOBCPTraveler();
            OwnerResponseModel profileOwnerResponse = new OwnerResponseModel();
            United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse corpProfileResponse = new United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse();
            if (travelers != null && travelers.Count > 0)
            {
                mobTravelers = new List<MOBCPTraveler>();
                int i = 0;
                var persistedReservation = !getMPSecurityDetails ? await PersistedReservation(request) : new Reservation();

                foreach (TravelerProfileResponse traveler in travelers)
                {
                    #region
                    MOBCPTraveler mobTraveler = new MOBCPTraveler();
                    mobTraveler.PaxIndex = i; i++;
                    mobTraveler.CustomerId = Convert.ToInt32(traveler.Profile?.CustomerId);
                    if (traveler.Profile?.ProfileOwnerIndicator == true)
                    {
           
                        profileOwnerResponse = await _sessionHelperService.GetSession<OwnerResponseModel>(request.SessionId, ObjectNames.CSLGetProfileOwnerResponse, new List<string> { request.SessionId, ObjectNames.CSLGetProfileOwnerResponse }).ConfigureAwait(false); 
                        mobTraveler.CustomerMetrics = PopulateCustomerMetrics(profileOwnerResponse);
                        mobTraveler.MileagePlus = PopulateMileagePlusV2(profileOwnerResponse, request.MileagePlusNumber);
                        mobTraveler.IsDeceased = profileOwnerResponse?.MileagePlus?.Data?.IsDeceased == true;
                        mobTraveler._employeeId = traveler.Profile?.EmployeeId;

                    }
                    if (traveler.Profile?.BirthDate != null)
                    {
                        mobTraveler.BirthDate = GeneralHelper.FormatDateOfBirth(traveler.Profile.BirthDate);
                        if (mobTraveler.BirthDate == "01/01/1")
                            mobTraveler.BirthDate = null;
                    }
                    if (_configuration.GetValue<bool>("EnableNationalityAndCountryOfResidence"))
                    {
                        if (persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.InfoNationalityAndResidence != null
                            && persistedReservation.ShopReservationInfo2.InfoNationalityAndResidence.IsRequireNationalityAndResidence)
                        {
                            if (string.IsNullOrEmpty(traveler.CustomerAttributes?.CountryofResidence) || string.IsNullOrEmpty(traveler.CustomerAttributes?.Nationality))
                            {
                                mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                            }
                        }
                        mobTraveler.Nationality = traveler.CustomerAttributes?.Nationality;
                        mobTraveler.CountryOfResidence = traveler.CustomerAttributes?.CountryofResidence;
                    }

                    mobTraveler.FirstName = traveler.Profile.FirstName;
                    mobTraveler.GenderCode = traveler.Profile?.Gender.ToString() == "Undefined" ? "" : traveler.Profile.Gender.ToString();
                    mobTraveler.IsProfileOwner = traveler.Profile.ProfileOwnerIndicator;
                    mobTraveler.Key = traveler.Profile.TravelerKey;
                    mobTraveler.LastName = traveler.Profile.LastName;
                    mobTraveler.MiddleName = traveler.Profile.MiddleName;
                    if (mobTraveler.MileagePlus != null)
                    {
                        mobTraveler.MileagePlus.MpCustomerId = Convert.ToInt32(traveler.Profile.CustomerId);

                        if (request != null && ConfigUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                        {
                            Session session = new Session();
                            string cslLoyaltryBalanceServiceResponse = await _loyaltyUCBService.GetLoyaltyBalance(request.Token, request.MileagePlusNumber, request.SessionId);
                            if (!string.IsNullOrEmpty(cslLoyaltryBalanceServiceResponse))
                            {
                                United.TravelBank.Model.BalancesDataModel.BalanceResponse PlusPointResponse = JsonSerializer.NewtonSoftDeserialize<United.TravelBank.Model.BalancesDataModel.BalanceResponse>(cslLoyaltryBalanceServiceResponse);
                                United.TravelBank.Model.BalancesDataModel.Balance tbbalance = PlusPointResponse.Balances.FirstOrDefault(tb => tb.ProgramCurrencyType == United.TravelBank.Model.TravelBankConstants.ProgramCurrencyType.UBC);
                                if (tbbalance != null && tbbalance.TotalBalance > 0)
                                {
                                    mobTraveler.MileagePlus.TravelBankBalance = (double)tbbalance.TotalBalance;
                                }
                            }
                        }
                    }

                    mobTraveler.ProfileId = Convert.ToInt32(traveler.Profile.ProfileId);
                    mobTraveler.ProfileOwnerId = Convert.ToInt32(traveler.Profile.ProfileOwnerId);
                    bool isTSAFlagOn = false;
                    if (traveler.SecureTravelers != null)
                    {
                        if (request == null)
                        {
                            request = new MOBCPProfileRequest();
                            request.SessionId = string.Empty;
                            request.DeviceId = string.Empty;
                            request.Application = new MOBApplication() { Id = 0 };
                        }
                        else if (request.Application == null)
                        {
                            request.Application = new MOBApplication() { Id = 0 };
                        }
                        mobTraveler.SecureTravelers = PopulatorSecureTravelersV2(traveler.SecureTravelers, ref isTSAFlagOn, i >= 2, request.SessionId, request.Application.Id, request.DeviceId);
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
                    mobTraveler.Suffix = traveler.Profile.Suffix;
                    mobTraveler.Title = traveler.Profile.Title;
                    mobTraveler.TravelerTypeCode = traveler.Profile?.TravelerTypeCode;
                    mobTraveler.TravelerTypeDescription = traveler.Profile?.TravelerTypeDescription;            
                    if (persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelerTypes != null
                        && persistedReservation.ShopReservationInfo2.TravelerTypes.Count > 0)
                    {
                        if (traveler.Profile?.BirthDate != null)
                        {
                            if (EnableYADesc() && persistedReservation.ShopReservationInfo2.IsYATravel)
                            {
                                mobTraveler.PTCDescription = GetYAPaxDescByDOB();
                            }
                            else
                            {
                                mobTraveler.PTCDescription = GetPaxDescriptionByDOB(traveler.Profile.BirthDate.ToString(), persistedReservation.Trips[0].FlattenedFlights[0].Flights[0].DepartDate);
                            }
                        }
                    }
                    else
                    {
                        if (EnableYADesc() && persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.IsYATravel)
                        {
                            mobTraveler.PTCDescription = GetYAPaxDescByDOB();
                        }
                    }                 
                    if (traveler != null)
                    {
                        if (mobTraveler.MileagePlus != null)
                        {
                            mobTraveler.CurrentEliteLevel = mobTraveler.MileagePlus.CurrentEliteLevel;
                            //mobTraveler.AirRewardPrograms = GetTravelerLoyaltyProfile(traveler.AirPreferences, traveler.MileagePlus.CurrentEliteLevel);
                        }
                    }

                    mobTraveler.AirRewardPrograms = GetTravelerRewardPgrograms(traveler.RewardPrograms, mobTraveler.CurrentEliteLevel);
                    mobTraveler.Phones = PopulatePhonesV2(traveler, true);
                    if (mobTraveler.IsProfileOwner)
                    {
                        // These Phone and Email details for Makre Reseravation Phone and Email reason is mobTraveler.Phones = PopulatePhones(traveler.Phones,true) will get only day of travel contacts to register traveler & edit traveler.
                        mobTraveler.ReservationPhones = PopulatePhonesV2(traveler, false);
                        mobTraveler.ReservationEmailAddresses = PopulateEmailAddressesV2(traveler.Emails, false);

                        // Added by Hasnan - #53484. 10/04/2017
                        // As per the Bug 53484:PINPWD: iOS and Android - Phone number is blank in RTI screen after booking from newly created account.
                        // If mobTraveler.Phones is empty. Then it newly created account. Thus returning mobTraveler.ReservationPhones as mobTraveler.Phones.
                        if (!_configuration.GetValue<bool>("EnableDayOfTravelEmail") || string.IsNullOrEmpty(path) || !path.ToUpper().Equals("BOOKING"))
                        {
                            if (mobTraveler.Phones.Count == 0)
                            {
                                mobTraveler.Phones = mobTraveler.ReservationPhones;
                            }
                        }
                        #region Corporate Leisure(ProfileOwner must travel)//Client will use the IsMustRideTraveler flag to auto select the travel and not allow to uncheck the profileowner on the SelectTraveler Screen.
                        if (_configuration.GetValue<bool>("EnableCorporateLeisure"))
                        {
                            if (persistedReservation?.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelType == TravelType.CLB.ToString() && IsCorporateLeisureFareSelected(persistedReservation.Trips))
                            {
                                mobTraveler.IsMustRideTraveler = true;
                            }
                        }
                        #endregion Corporate Leisure
                    }
                    if (mobTraveler.IsProfileOwner && request == null) //**PINPWD//mobTraveler.IsProfileOwner && request == null Means GetProfile and Populate is for MP PIN PWD Path
                    {
                        mobTraveler.ReservationEmailAddresses = PopulateAllEmailAddressesV2(traveler.Emails);
                    }
                    mobTraveler.AirPreferences = PopulateAirPrefrencesV2(traveler);
                    if (request?.Application?.Version != null && string.IsNullOrEmpty(request?.Flow) && IsInternationalBillingAddress_CheckinFlowEnabled(request.Application))
                    {
                        try
                        {
                            MOBShoppingCart mobShopCart = new MOBShoppingCart();
                            mobShopCart = await _sessionHelperService.GetSession<MOBShoppingCart>(request.SessionId, mobShopCart.ObjectName, new List<string> { request.SessionId, mobShopCart.ObjectName });
                            if (mobShopCart != null && !string.IsNullOrEmpty(mobShopCart.Flow) && mobShopCart.Flow == FlowType.CHECKIN.ToString())
                            {
                                request.Flow = mobShopCart.Flow;
                            }
                        }
                        catch { }
                    }
                    mobTraveler.Addresses = PopulateTravelerAddressesV2(traveler.Addresses, request?.Application, request?.Flow);

                    if (_configuration.GetValue<bool>("EnableDayOfTravelEmail") && !string.IsNullOrEmpty(path) && path.ToUpper().Equals("BOOKING"))
                    {
                        mobTraveler.EmailAddresses = PopulateEmailAddressesV2(traveler.Emails, true);
                    }
                    else
                    if (!getMPSecurityDetails)
                    {
                        mobTraveler.EmailAddresses = PopulateEmailAddressesV2(traveler.Emails, false);
                    }
                    else
                    {
                        mobTraveler.EmailAddresses = PopulateMPSecurityEmailAddressesV2(traveler.Emails);
                    }
                    if (mobTraveler.IsProfileOwner == true)
                    {                     
                        
                      
                        if (!getMPSecurityDetails)
                        {
                            mobTraveler.CreditCards = await _profileCreditCard.PopulateCreditCards(isGetCreditCardDetailsCall, mobTraveler.Addresses, request);
                            if (IsCorpBookingPath)
                            {
                                corpProfileResponse = await _sessionHelperService.GetSession<United.CorporateDirect.Models.CustomerProfile.CorpProfileResponse>(request.SessionId, ObjectNames.CSLCorpProfileResponse, new List<string> { request.SessionId, ObjectNames.CSLCorpProfileResponse }).ConfigureAwait(false);
                                var corpCreditCards = await _profileCreditCard.PopulateCorporateCreditCards(isGetCreditCardDetailsCall, mobTraveler.Addresses, persistedReservation, request);
                                if (mobTraveler.CreditCards == null)
                                {
                                    mobTraveler.CreditCards = new List<MOBCreditCard>();
                                }
                                if (corpCreditCards != null)
                                {
                                    mobTraveler.CreditCards.AddRange(corpCreditCards);
                                }
                            }
                        }
                        if (IsCorpBookingPath && corpProfileResponse?.Profiles != null && corpProfileResponse.Profiles.Count() > 0)
                        {
                            var corporateTraveler = corpProfileResponse?.Profiles[0].Travelers.FirstOrDefault();
                            if (corporateTraveler != null)
                            {
                                if (corporateTraveler.Addresses != null)
                                {
                                    var corporateAddress = PopulateCorporateTravelerAddresses(corporateTraveler.Addresses, request.Application, request.Flow);
                                    if (mobTraveler.Addresses == null)
                                        mobTraveler.Addresses = new List<MOBAddress>();
                                    mobTraveler.Addresses.AddRange(corporateAddress);
                                }
                                if (corporateTraveler.EmailAddresses != null)
                                {
                                    var corporateEmailAddresses = PopulateCorporateEmailAddresses(corporateTraveler.EmailAddresses, false);
                                    mobTraveler.ReservationEmailAddresses = new List<MOBEmail>();
                                    mobTraveler.ReservationEmailAddresses.AddRange(corporateEmailAddresses);
                                }
                                if (corporateTraveler.Phones != null)
                                {
                                    var corporatePhones = PopulateCorporatePhones(corporateTraveler.Phones, false);
                                    mobTraveler.ReservationPhones = new List<MOBCPPhone>();
                                    mobTraveler.ReservationPhones.AddRange(corporatePhones);
                                }
                                if (corporateTraveler.AirPreferences != null)
                                {
                                    var corporateAirpreferences = PopulateCorporateAirPrefrences(corporateTraveler.AirPreferences);
                                    if (mobTraveler.AirPreferences == null)
                                        mobTraveler.AirPreferences = new List<MOBPrefAirPreference>();
                                    mobTraveler.AirPreferences.AddRange(corporateAirpreferences);
                                }
                            }

                        }
                    }
                    if (mobTraveler.IsTSAFlagON || string.IsNullOrEmpty(mobTraveler.FirstName) || string.IsNullOrEmpty(mobTraveler.LastName) || string.IsNullOrEmpty(mobTraveler.GenderCode) || string.IsNullOrEmpty(mobTraveler.BirthDate)) //|| mobTraveler.Phones == null || (mobTraveler.Phones != null && mobTraveler.Phones.Count == 0)
                    {
                        mobTraveler.Message = _configuration.GetValue<string>("SavedTravelerInformationNeededMessage");
                    }
                    if (mobTraveler.IsProfileOwner)
                    {
                        profileOwnerDetails = mobTraveler;
                    }
                    else
                    {
                        #region
                        if (mobTraveler.AirRewardPrograms != null && mobTraveler.AirRewardPrograms.Count > 0)
                        {
                            var airRewardProgramList = (from program in mobTraveler.AirRewardPrograms
                                                        where program.CarrierCode.ToUpper().Trim() == "UA"
                                                        select program).ToList();

                            if (airRewardProgramList != null && airRewardProgramList.Count > 0)
                            {
                                savedTravelersMPList.Add(new MOBKVP() { Key = mobTraveler.CustomerId.ToString(), Value = airRewardProgramList[0].MemberId });
                            }
                        }
                        #endregion
                        mobTravelers.Add(mobTraveler);
                    }
                    #endregion
                }
            }
            mobTravelersOwnerFirstInList = new List<MOBCPTraveler>();
            mobTravelersOwnerFirstInList.Add(profileOwnerDetails);
            if (!IsCorpBookingPath || IsArrangerBooking)
            {
                mobTravelersOwnerFirstInList.AddRange(mobTravelers);
            }

            return (mobTravelersOwnerFirstInList,isProfileOwnerTSAFlagOn,savedTravelersMPList);
        }
        public MOBCPMileagePlus PopulateMileagePlusV2(OwnerResponseModel profileOwnerResponse, string mileageplusId)
        {
            if (profileOwnerResponse?.MileagePlus?.Data != null)
            {
                MOBCPMileagePlus mileagePlus = null;
                var mileagePlusData = profileOwnerResponse.MileagePlus.Data;

                mileagePlus = new MOBCPMileagePlus();
                var balance = profileOwnerResponse.MileagePlus.Data.Balances?.FirstOrDefault(balnc => (int)balnc.Currency == 5);
                mileagePlus.AccountBalance = Convert.ToInt32(balance.Amount);
                mileagePlus.ActiveStatusCode = mileagePlusData.AccountStatus;
                mileagePlus.ActiveStatusDescription = mileagePlusData.AccountStatusDescription;
                mileagePlus.AllianceEliteLevel = mileagePlusData.StarAllianceTierLevel;
                mileagePlus.ClosedStatusCode = mileagePlusData.OpenClosedStatusCode;
                mileagePlus.ClosedStatusDescription = mileagePlusData.OpenClosedStatusDescription;
                mileagePlus.CurrentEliteLevel = mileagePlusData.MPTierLevel;
                if (mileagePlus.CurrentEliteLevelDescription != null)
                {
                    mileagePlus.CurrentEliteLevelDescription = mileagePlusData.MPTierLevelDescription.ToString().ToUpper() == "NON-ELITE" ? "General member" : mileagePlusData.MPTierLevelDescription;
                }
                mileagePlus.CurrentYearMoneySpent = mileagePlusData.CurrentYearMoneySpent;
                mileagePlus.EliteMileageBalance = Convert.ToInt32(mileagePlusData.EliteMileageBalance);         
                mileagePlus.EnrollDate = mileagePlusData.EnrollDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.EnrollSourceCode = mileagePlusData.EnrollSourceCode;
                mileagePlus.EnrollSourceDescription = mileagePlusData.EnrollSourceDescription;
                mileagePlus.FutureEliteDescription = mileagePlusData.NextStatusLevelDescription;
                mileagePlus.FutureEliteLevel = mileagePlusData.NextStatusLevel;
                mileagePlus.InstantEliteExpirationDate = mileagePlusData.NextStatusLevelDescription;
                mileagePlus.IsCEO = mileagePlusData.CEO;
                mileagePlus.IsClosedPermanently = mileagePlusData.IsClosedPermanently;
                mileagePlus.IsClosedTemporarily = mileagePlusData.IsClosedTemporarily;         
                mileagePlus.IsLockedOut = mileagePlusData.IsLockedOut;
                mileagePlus.IsUnitedClubMember = mileagePlusData.IsPClubMember;
                mileagePlus.LastActivityDate = mileagePlusData.LastActivityDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.LastFlightDate = mileagePlusData.LastFlightDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.LastStatementDate = mileagePlusData.LastStatementDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                mileagePlus.LifetimeEliteMileageBalance = Convert.ToInt32(mileagePlusData.LifetimeMiles);
                mileagePlus.MileagePlusId = mileageplusId;             
                return mileagePlus;
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

        }
        public async Task<OwnerResponseModel> GetProfileOwnerInfo(String token, string sessionId, string mileagePlusNumber)
        {
            var response = await _customerProfileOwnerService.GetProfileOwnerInfo<OwnerResponseModel>(token, sessionId, mileagePlusNumber);
            if (response != null && response.MileagePlus != null)
            {
                return response;
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }
        public List<MOBCPSecureTraveler> PopulatorSecureTravelersV2(SecureTravelerResponseData secureTravelerResponseData, ref bool isTSAFlag, bool correctDate, string sessionID, int appID, string deviceID)
        {
            List<MOBCPSecureTraveler> mobSecureTravelers = null;
            try
            {
                if (secureTravelerResponseData?.SecureTraveler != null)
                {
                    mobSecureTravelers = new List<MOBCPSecureTraveler>();
                    var secureTraveler = secureTravelerResponseData.SecureTraveler;

                    if (secureTraveler.DocumentType != null && secureTraveler.DocumentType.Trim().ToUpper() != "X")
                    {
                        #region
                        MOBCPSecureTraveler mobSecureTraveler = new MOBCPSecureTraveler();
                        if (correctDate)
                        {
                            DateTime tempBirthDate = secureTraveler.BirthDate.GetValueOrDefault().AddHours(1);
                            mobSecureTraveler.BirthDate = tempBirthDate.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            mobSecureTraveler.BirthDate = secureTraveler.BirthDate.GetValueOrDefault().ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                        }
                        mobSecureTraveler.CustomerId = Convert.ToInt32(secureTraveler.CustomerId);
                        mobSecureTraveler.DecumentType = secureTraveler.DocumentType;
                        mobSecureTraveler.Description = secureTraveler.Description;
                        mobSecureTraveler.FirstName = secureTraveler.FirstName;
                        mobSecureTraveler.Gender = secureTraveler.Gender;
                        // mobSecureTraveler.Key = secureTraveler.Key;No longer needed confirmed from service
                        mobSecureTraveler.LastName = secureTraveler.LastName;
                        mobSecureTraveler.MiddleName = secureTraveler.MiddleName;
                        mobSecureTraveler.SequenceNumber = (int)secureTraveler.SequenceNumber;
                        mobSecureTraveler.Suffix = secureTraveler.Suffix;
                        if (secureTravelerResponseData.SupplementaryTravelInfos != null)
                        {
                            foreach (SupplementaryTravelDocsDataMembers supplementaryTraveler in secureTravelerResponseData.SupplementaryTravelInfos)
                            {
                                if (supplementaryTraveler.Type == "K")
                                {
                                    mobSecureTraveler.KnownTravelerNumber = supplementaryTraveler.Number;
                                }
                                if (supplementaryTraveler.Type == "R")
                                {
                                    mobSecureTraveler.RedressNumber = supplementaryTraveler.Number;
                                }
                            }
                        }
                        if (!isTSAFlag && secureTraveler.DocumentType.Trim().ToUpper() == "U")
                        {
                            isTSAFlag = true;
                        }
                        if (secureTraveler.DocumentType.Trim().ToUpper() == "C" || secureTraveler.DocumentType.Trim() == "") // This is to get only Customer Cleared Secure Traveler records
                        {
                            mobSecureTravelers = new List<MOBCPSecureTraveler>();
                            mobSecureTravelers.Add(mobSecureTraveler);
                        }
                        else
                        {
                            mobSecureTravelers.Add(mobSecureTraveler);
                        }
                        #endregion
                    }

                }

            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError("PopulatorSecureTravelers {@Exception} for {@SecureTravelerResponseData}", JsonConvert.SerializeObject(ex), JsonConvert.SerializeObject(secureTravelerResponseData));
                }
                catch { }
            }

            return mobSecureTravelers;
        }

        public List<MOBCPPhone> PopulatePhonesV2(TravelerProfileResponse traveler, bool onlyDayOfTravelContact)
        {
            List<MOBCPPhone> mobCPPhones = new List<MOBCPPhone>();
            bool isCorpPhonePresent = false;
            var phones = traveler.Phones;
            if (phones != null && phones.Count > 0)
            {
                if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
                {
                    var corpIndex = phones.FindIndex(x => x.TypeDescription != null && x.TypeDescription.ToLower() == "corporate" && x.Number != null && x.Number != "");
                    if (corpIndex >= 0)
                        isCorpPhonePresent = true;
                }
                MOBCPPhone primaryMobCPPhone = null;
                CultureInfo ci = GeneralHelper.EnableUSCultureInfo();
                int co = 0;
                foreach (United.Mobile.Model.CSLModels.Phone phone in phones)
                {
                    #region As per Wade Change want to filter out to return only Primary Phone to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                    MOBCPPhone mobCPPhone = new MOBCPPhone();
                    co = co + 1;
                    mobCPPhone.AreaNumber = phone.AreaCode;
                    mobCPPhone.PhoneNumber = phone.Number;
                    //mobCPPhone.Attention = phone.Attention;No longer needed confirmed from service
                    mobCPPhone.ChannelCode = "P";
                    mobCPPhone.ChannelCodeDescription = "Phone";
                    mobCPPhone.ChannelTypeCode = phone.Type.ToString();
                    mobCPPhone.ChannelTypeDescription = phone.TypeDescription;
                    mobCPPhone.ChannelTypeSeqNumber = phone.SequenceNumber;
                    mobCPPhone.CountryCode = phone.CountryCode;
                    //mobCPPhone.CountryCode = GetAccessCode(phone.CountryCode);
                    mobCPPhone.CountryPhoneNumber = phone.CountryPhoneNumber;
                    mobCPPhone.CustomerId = Convert.ToInt32(traveler.Profile.CustomerId);
                    mobCPPhone.Description = phone.Remark;
                    mobCPPhone.DiscontinuedDate = Convert.ToString(phone.DiscontinuedDate);
                    mobCPPhone.EffectiveDate = Convert.ToString(phone.EffectiveDate);
                    mobCPPhone.ExtensionNumber = phone.ExtensionNumber;
                    mobCPPhone.IsPrimary = phone.PrimaryIndicator;
                    mobCPPhone.IsPrivate = phone.IsPrivate;
                    mobCPPhone.IsProfileOwner = traveler.Profile.ProfileOwnerIndicator;
                    mobCPPhone.Key = phone.Key;
                    mobCPPhone.LanguageCode = phone.LanguageCode;
                    // mobCPPhone.PagerPinNumber = phone.PagerPinNumber;
                    // mobCPPhone.SharesCountryCode = phone.SharesCountryCode;
                    mobCPPhone.WrongPhoneDate = Convert.ToString(phone.WrongPhoneDate);
                    mobCPPhone.DeviceTypeCode = phone.DeviceType.ToString();
                    mobCPPhone.DeviceTypeDescription = phone.TypeDescription;

                    mobCPPhone.IsDayOfTravel = phone.DayOfTravelNotification;

                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        #region
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.TypeDescription.ToLower() == "corporate")
                        {
                            //return the corporate phone number
                            primaryMobCPPhone = new MOBCPPhone();
                            mobCPPhone.IsPrimary = true;
                            primaryMobCPPhone = mobCPPhone;
                            break;

                        }
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.TypeDescription.ToLower() != "corporate")
                        {
                            //There is corporate phone number present, continue till corporate phone number is found
                            continue;
                        }
                        #endregion
                    }

                    if (phone.DayOfTravelNotification)
                    {
                        primaryMobCPPhone = new MOBCPPhone();
                        primaryMobCPPhone = mobCPPhone;// Only day of travel contact should be returned to use at Edit Traveler
                        if (onlyDayOfTravelContact)
                        {
                            break;
                        }
                    }
                    if (!onlyDayOfTravelContact)
                    {
                        if (phone.DayOfTravelNotification)
                        {
                            primaryMobCPPhone = new MOBCPPhone();
                            primaryMobCPPhone = mobCPPhone;
                            break;
                        }
                        else if (co == 1)
                        {
                            primaryMobCPPhone = new MOBCPPhone();
                            primaryMobCPPhone = mobCPPhone;
                        }
                    }
                    #endregion
                }
                if (primaryMobCPPhone != null)
                {
                    mobCPPhones.Add(primaryMobCPPhone);
                }
                GeneralHelper.DisableUSCultureInfo(ci);
            }
            return mobCPPhones;
        }
        public List<MOBEmail> PopulateEmailAddressesV2(List<United.Mobile.Model.CSLModels.Email> emailAddresses, bool onlyDayOfTravelContact)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            bool isCorpEmailPresent = false;

            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
                {
                    //As per Business / DotCom Kalpen; we are removing the condition for checking the Effectivedate and Discontinued date
                    var corpIndex = emailAddresses.FindIndex(x => x.TypeDescription != null && x.TypeDescription.ToLower() == "corporate" && x.Address != null && x.Address.Trim() != "");
                    if (corpIndex >= 0)
                        isCorpEmailPresent = true;
                }

                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (United.Mobile.Model.CSLModels.Email email in emailAddresses)
                {
                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        if (isCorpEmailPresent && !onlyDayOfTravelContact && email.TypeDescription.ToLower() == "corporate")
                        {
                            primaryEmailAddress = new MOBEmail();
                            email.PrimaryIndicator = true;
                            primaryEmailAddress.Key = email.Key;
                            primaryEmailAddress.Channel = new SHOPChannel();
                            primaryEmailAddress.EmailAddress = email.Address;
                            primaryEmailAddress.Channel.ChannelCode = "E";
                            primaryEmailAddress.Channel.ChannelDescription = "Email";
                            primaryEmailAddress.Channel.ChannelTypeCode = email.Type.ToString();
                            primaryEmailAddress.Channel.ChannelTypeDescription = email.TypeDescription;
                            primaryEmailAddress.IsDefault = email.PrimaryIndicator;
                            primaryEmailAddress.IsPrimary = email.PrimaryIndicator;
                            primaryEmailAddress.IsPrivate = email.IsPrivate;
                            primaryEmailAddress.IsDayOfTravel = email.DayOfTravelNotification;
                            if (!email.DayOfTravelNotification)
                            {
                                break;
                            }

                        }
                        else if (isCorpEmailPresent && !onlyDayOfTravelContact && email.TypeDescription.ToLower() != "corporate")
                        {
                            continue;
                        }
                    }
                    //Fix for CheckOut ArgNull Exception - Empty EmailAddress with null EffectiveDate & DiscontinuedDate for Corp Account Revenue Booking (MOBILE-9873) - Shashank : Added OR condition to allow CorporateAccount ProfileOwner.
                    if ((email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow) ||
                            (!_configuration.GetValue<bool>("DisableCheckforCorpAccEmail") && email.TypeDescription.ToLower() == "corporate"
                            && email.PrimaryIndicator == true && primaryEmailAddress.IsNullOrEmpty()))
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.EmailAddress = email.Address;
                        e.Channel.ChannelCode = "E";
                        e.Channel.ChannelDescription = "Email";
                        e.Channel.ChannelTypeCode = email.Type.ToString();
                        e.Channel.ChannelTypeDescription = email.TypeDescription;
                        e.IsDefault = email.PrimaryIndicator;
                        e.IsPrimary = email.PrimaryIndicator;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.DayOfTravelNotification;
                        if (email.DayOfTravelNotification)
                        {
                            primaryEmailAddress = new MOBEmail();
                            primaryEmailAddress = e;
                            if (onlyDayOfTravelContact)
                            {
                                break;
                            }
                        }
                        if (!onlyDayOfTravelContact)
                        {
                            if (email.PrimaryIndicator)
                            {
                                primaryEmailAddress = new MOBEmail();
                                primaryEmailAddress = e;
                                break;
                            }
                            else if (co == 1)
                            {
                                primaryEmailAddress = new MOBEmail();
                                primaryEmailAddress = e;
                            }
                        }
                        #endregion
                    }
                }
                if (primaryEmailAddress != null)
                {
                    mobEmailAddresses.Add(primaryEmailAddress);
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        public List<MOBEmail> PopulateAllEmailAddressesV2(List<United.Mobile.Model.CSLModels.Email> emailAddresses)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                int co = 0;
                foreach (United.Mobile.Model.CSLModels.Email email in emailAddresses)
                {
                    if (email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.Channel.ChannelCode = "E";
                        e.Channel.ChannelDescription = "Email";
                        e.Channel.ChannelTypeCode = email.Type.ToString();
                        e.Channel.ChannelTypeDescription = email.TypeDescription;
                        e.EmailAddress = email.Address;
                        e.IsDefault = email.PrimaryIndicator;
                        e.IsPrimary = email.PrimaryIndicator;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.DayOfTravelNotification;
                        mobEmailAddresses.Add(e);
                        #endregion
                    }
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        public List<MOBPrefAirPreference> PopulateAirPrefrencesV2(TravelerProfileResponse traveler)
        {
            var airPreferences = traveler.AirPreferences;
            List<MOBPrefAirPreference> mobAirPrefs = new List<MOBPrefAirPreference>();
            if (airPreferences != null && airPreferences.Count > 0)
            {
                foreach (AirPreferenceDataModel pref in airPreferences)
                {
                    MOBPrefAirPreference mobAirPref = new MOBPrefAirPreference();
                    mobAirPref.AirportCode = pref.AirportCode;
                    mobAirPref.AirportCode = pref.AirportNameLong;
                    mobAirPref.AirportNameShort = pref.AirportNameShort;
                    mobAirPref.AirPreferenceId = pref.AirPreferenceId;
                    mobAirPref.ClassDescription = pref.ClassDescription;
                    mobAirPref.ClassId = pref.ClassID;
                    mobAirPref.CustomerId = traveler.Profile.CustomerId;
                    mobAirPref.EquipmentCode = pref.EquipmentCode;
                    mobAirPref.EquipmentDesc = pref.EquipmentDescription;
                    mobAirPref.EquipmentId = pref.EquipmentID;
                    mobAirPref.IsActive = true;//By default if it is returned it is active confirmed with service team
                    mobAirPref.IsSelected = true;// By default if it is returned it is active confirmed with service team
                    mobAirPref.IsNew = false;// By default if it is returned it is false confirmed with service team
                    mobAirPref.Key = pref.Key;
                    //mobAirPref.LanguageCode = pref.LanguageCode;No longer sent from service confirmed with them
                    mobAirPref.MealCode = pref.MealCode;
                    mobAirPref.MealDescription = pref.MealDescription;
                    mobAirPref.MealId = pref.MealId;
                    // mobAirPref.NumOfFlightsDisplay = pref.NumOfFlightsDisplay;No longer sent from service confirmed with them
                    mobAirPref.ProfileId = traveler.Profile.ProfileId;
                    mobAirPref.SearchPreferenceDescription = pref.SearchPreferenceDescription;
                    mobAirPref.SearchPreferenceId = pref.SearchPreferenceID;
                    //mobAirPref.SeatFrontBack = pref.SeatFrontBack;No longer sent from service confirmed with them
                    mobAirPref.SeatSide = pref.SeatSide;
                    mobAirPref.SeatSideDescription = pref.SeatSideDescription;
                    mobAirPref.VendorCode = pref.VendorCode;//Service confirmed we can hard code this as we dont have any other vendor it is always United airlines
                    mobAirPref.VendorDescription = pref.VendorDescription;//Service confirmed we can hard code this as we dont have any other vendor it is always United airlines
                    mobAirPref.VendorId = pref.VendorId;
                    mobAirPref.AirRewardPrograms = GetAirRewardPrograms(traveler);
                    // mobAirPref.SpecialRequests = GetTravelerSpecialRequests(pref.SpecialRequests);Client is not using this even we send this ..
                    // mobAirPref.ServiceAnimals = GetTravelerServiceAnimals(pref.ServiceAnimals);Client is not using this even we send this ..
                    mobAirPrefs.Add(mobAirPref);
                }
            }
            return mobAirPrefs;
        }
        public List<Mobile.Model.Common.MOBAddress> PopulateTravelerAddressesV2(List<United.Mobile.Model.CSLModels.Address> addresses, MOBApplication application = null, string flow = null)
        {
            #region

            var mobAddresses = new List<Mobile.Model.Common.MOBAddress>();
            if (addresses != null && addresses.Count > 0)
            {
                bool isCorpAddressPresent = false;
                if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
                {
                    //As per Business / DotCom Kalpen; we are removing the condition for checking the Effectivedate and Discontinued date
                    var corpIndex = addresses.FindIndex(x => x.TypeDescription != null && x.TypeDescription.ToLower() == "corporate" && x.Line1 != null && x.Line1.Trim() != "");
                    if (corpIndex >= 0)
                        isCorpAddressPresent = true;

                }
                foreach (United.Mobile.Model.CSLModels.Address address in addresses)
                {
                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        if (isCorpAddressPresent && address.TypeDescription.ToLower() == "corporate" &&
                            (_configuration.GetValue<bool>("USPOSCountryCodes_ByPass") || IsInternationalBilling(application, address.CountryCode, flow)))
                        {
                            var a = new Mobile.Model.Common.MOBAddress();
                            a.Key = address.Key;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = "A";
                            a.Channel.ChannelDescription = "Address";
                            a.Channel.ChannelTypeCode = address.Type.ToString();
                            a.Channel.ChannelTypeDescription = address.TypeDescription;
                            //a.ApartmentNumber = address.AptNum; No longer needed confirmed from service
                            a.City = address.City;
                            // a.CompanyName = address.CompanyName;No longer needed confirmed from service
                            a.Country = new MOBCountry();
                            a.Country.Code = address.CountryCode;
                            a.Country.Name = address.CountryName;
                            // a.JobTitle = address.JobTitle;No longer needed confirmed from service
                            a.Line1 = address.Line1;
                            a.Line2 = address.Line2;
                            a.Line3 = address.Line3;
                            a.State = new Mobile.Model.Common.State();
                            a.State.Code = address.StateCode;
                            a.IsDefault = address.PrimaryIndicator;
                            a.IsPrivate = address.IsPrivate;
                            a.PostalCode = address.PostalCode;
                            if (address.TypeDescription.ToLower().Trim() == "corporate")
                            {
                                a.IsPrimary = true;
                                a.IsCorporate = true; // MakeIsCorporate true inorder to disable the edit on client
                            }
                            // Make IsPrimary true inorder to select the corpaddress by default

                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                a.IsValidForTPIPurchase = IsValidAddressForTPIpayment(address.CountryCode);

                                if (a.IsValidForTPIPurchase)
                                {
                                    a.IsValidForTPIPurchase = IsValidSateForTPIpayment(address.StateCode);
                                }
                            }
                            mobAddresses.Add(a);
                        }
                    }


                    if (address.EffectiveDate <= DateTime.UtcNow && address.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        if (_configuration.GetValue<bool>("USPOSCountryCodes_ByPass") || IsInternationalBilling(application, address.CountryCode, flow)) //##Kirti - allow only US addresses 
                        {
                            var a = new Mobile.Model.Common.MOBAddress();
                            a.Key = address.Key;
                            a.Channel = new SHOPChannel();
                            a.Channel.ChannelCode = "A";
                            a.Channel.ChannelDescription = "Address";
                            a.Channel.ChannelTypeCode = address.Type.ToString();
                            a.Channel.ChannelTypeDescription = address.TypeDescription;
                            a.City = address.City;
                            a.Country = new MOBCountry();
                            a.Country.Code = address.CountryCode;
                            a.Country.Name = address.CountryName;
                            a.Line1 = address.Line1;
                            a.Line2 = address.Line2;
                            a.Line3 = address.Line3;
                            a.State = new Mobile.Model.Common.State();
                            a.State.Code = address.StateCode;
                            //a.State.Name = address.StateName;
                            a.IsDefault = address.PrimaryIndicator;
                            a.IsPrimary = address.PrimaryIndicator;
                            a.IsPrivate = address.IsPrivate;
                            a.PostalCode = address.PostalCode;
                            //Adding this check for corporate addresses to gray out the Edit button on the client
                            //if (address.ChannelTypeDescription.ToLower().Trim() == "corporate")
                            //{
                            //    a.IsCorporate = true;
                            //}
                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                a.IsValidForTPIPurchase = IsValidAddressForTPIpayment(address.CountryCode);

                                if (a.IsValidForTPIPurchase)
                                {
                                    a.IsValidForTPIPurchase = IsValidSateForTPIpayment(address.StateCode);
                                }
                            }
                            mobAddresses.Add(a);
                        }
                    }
                }
            }
            return mobAddresses;
            #endregion
        }
        private List<MOBEmail> PopulateMPSecurityEmailAddressesV2(List<United.Mobile.Model.CSLModels.Email> emailAddresses)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (United.Mobile.Model.CSLModels.Email email in emailAddresses)
                {
                    if (email.EffectiveDate <= DateTime.UtcNow && email.DiscontinuedDate >= DateTime.UtcNow)
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Key = email.Key;
                        e.Channel = new SHOPChannel();
                        e.Channel.ChannelCode = "E";
                        e.Channel.ChannelDescription = "Email";
                        e.Channel.ChannelTypeCode = email.Type.ToString();
                        e.Channel.ChannelTypeDescription = email.TypeDescription;
                        e.EmailAddress = email.Address;
                        e.IsDefault = email.PrimaryIndicator;
                        e.IsPrimary = email.PrimaryIndicator;
                        e.IsPrivate = email.IsPrivate;
                        e.IsDayOfTravel = email.DayOfTravelNotification;
                        if (email.PrimaryIndicator)
                        {
                            primaryEmailAddress = new MOBEmail();
                            primaryEmailAddress = e;
                            break;
                        }
                        #endregion
                    }
                }
                if (primaryEmailAddress != null)
                {
                    mobEmailAddresses.Add(primaryEmailAddress);
                }
            }
            return mobEmailAddresses;
            #endregion
        }
        private MOBCPCustomerMetrics PopulateCustomerMetrics(OwnerResponseModel profileOwnerResponse)
        {
            if (profileOwnerResponse?.CustomerMetrics?.Data != null)
            {
                MOBCPCustomerMetrics travelerCustomerMetrics = new MOBCPCustomerMetrics();
                if (!String.IsNullOrEmpty(profileOwnerResponse.CustomerMetrics.Data.PTCCode))
                {
                    travelerCustomerMetrics.PTCCode = profileOwnerResponse.CustomerMetrics.Data.PTCCode;
                }
                return travelerCustomerMetrics;
            }
            return null;
        }
        private List<MOBPrefRewardProgram> GetAirRewardPrograms(TravelerProfileResponse traveler)
        {
            List<MOBPrefRewardProgram> mobAirRewardsProgs = new List<MOBPrefRewardProgram>();
            if (traveler?.RewardPrograms != null && traveler?.RewardPrograms.Count > 0)
            {
                foreach (United.Mobile.Model.CSLModels.RewardProgramData pref in traveler?.RewardPrograms)
                {
                    MOBPrefRewardProgram mobAirRewardsProg = new MOBPrefRewardProgram();
                    if (traveler?.Profile != null)
                    {
                        mobAirRewardsProg.CustomerId = traveler.Profile.CustomerId;
                        mobAirRewardsProg.ProfileId = traveler.Profile.ProfileId;
                    }
                    mobAirRewardsProg.ProgramMemberId = pref.ProgramMemberId;
                    mobAirRewardsProg.VendorCode = pref.VendorCode;
                    mobAirRewardsProg.VendorDescription = pref.VendorDescription;
                    mobAirRewardsProgs.Add(mobAirRewardsProg);
                }
            }
            return mobAirRewardsProgs;
        }

        public List<MOBBKLoyaltyProgramProfile> GetTravelerRewardPgrograms(List<RewardProgramData> rewardPrograms, int currentEliteLevel)
        {
            List<MOBBKLoyaltyProgramProfile> programs = new List<MOBBKLoyaltyProgramProfile>();

            if (rewardPrograms != null && rewardPrograms.Count > 0)
            {
                foreach (RewardProgramData rewardProgram in rewardPrograms)
                {
                    MOBBKLoyaltyProgramProfile airRewardProgram = new MOBBKLoyaltyProgramProfile();
                    airRewardProgram.ProgramId = rewardProgram.ProgramId.ToString();
                    airRewardProgram.ProgramName = rewardProgram.Description;
                    airRewardProgram.MemberId = rewardProgram.ProgramMemberId;
                    airRewardProgram.CarrierCode = rewardProgram.VendorCode;
                    if (airRewardProgram.CarrierCode.Trim().Equals("UA"))
                    {
                        airRewardProgram.MPEliteLevel = currentEliteLevel;
                    }
                    airRewardProgram.RewardProgramKey = rewardProgram.Key;
                    programs.Add(airRewardProgram);
                }
            }
            return programs;
        }
        public async Task<List<MOBTypeOption>> GetProfileDisclaimerList()
        {

            List<MOBLegalDocument> profileDisclaimerList = await GetLegalDocumentsForTitles("ProfileDisclamerList");
            List<MOBTypeOption> disclaimerList = new List<MOBTypeOption>();
            List<MOBTypeOption> travelerDisclaimerTextList = new List<MOBTypeOption>();

            List<string> mappingTextList = _configuration.GetValue<string>("Booking20TravelerDisclaimerMapping").Split('~').ToList();
            foreach (string mappingText in mappingTextList)
            {
                string disclaimerTextTitle = mappingText.Split('=')[0].ToString().Trim();
                List<string> travelerTextTitleList = mappingText.Split('=')[1].ToString().Split('|').ToList();
                int co = 0;
                foreach (string travelerTextTile in travelerTextTitleList)
                {
                    if (profileDisclaimerList != null)
                    {
                        foreach (MOBLegalDocument legalDocument in profileDisclaimerList)
                        {
                            if (legalDocument.Title.ToUpper().Trim() == travelerTextTile.ToUpper().Trim())
                            {
                                MOBTypeOption typeOption = new MOBTypeOption();
                                co++;
                                typeOption.Key = disclaimerTextTitle + co.ToString();
                                typeOption.Value = legalDocument.LegalDocument;
                                travelerDisclaimerTextList.Add(typeOption);
                            }
                        }
                    }
                }
            }
            return travelerDisclaimerTextList;
        }

        private async Task<List<MOBLegalDocument>> GetLegalDocumentsForTitles(string titles)
        {
            var legalDocuments = new List<MOBLegalDocument>();
            legalDocuments = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(titles, _headers.ContextValues.TransactionId, true);

            if (legalDocuments == null)
            {
                legalDocuments = new List<United.Definition.MOBLegalDocument>();
            }
            return legalDocuments;
        }
        public async System.Threading.Tasks.Task MakeProfileOwnerServiceCall(MOBCPProfileRequest request)
        {
            var ownerProfileResponse = await _customerProfileOwnerService.GetProfileOwnerInfo<OwnerResponseModel>(request.Token, request.SessionId, request.MileagePlusNumber);
            await _sessionHelperService.SaveSession<OwnerResponseModel>(ownerProfileResponse, request.SessionId, new List<string> { request.SessionId, ObjectNames.CSLGetProfileOwnerResponse }, ObjectNames.CSLGetProfileOwnerResponse);
        }
        public List<MOBAddress> PopulateCorporateTravelerAddresses(List<United.CorporateDirect.Models.CustomerProfile.Address> addresses, MOBApplication application = null, string flow = null)
        {
            #region
            List<MOBAddress> mobAddresses = new List<MOBAddress>();
            if (addresses != null && addresses.Count > 0)
            {

                foreach (United.CorporateDirect.Models.CustomerProfile.Address address in addresses)
                {
                    if ((_configuration.GetValue<bool>("USPOSCountryCodes_ByPass") || IsInternationalBilling(application, address.CountryCode, flow)))
                    {
                        MOBAddress a = new MOBAddress();
                        a.Key = address.Key;
                        a.Channel = new SHOPChannel();
                        a.Channel.ChannelCode = address.ChannelCode;
                        a.Channel.ChannelDescription = address.ChannelCodeDescription;
                        a.Channel.ChannelTypeCode = address.ChannelTypeCode.ToString();
                        a.Channel.ChannelTypeDescription = address.ChannelTypeDescription;
                        a.City = address.City;
                        a.Country = new MOBCountry();
                        a.Country.Code = address.CountryCode;
                        a.Line1 = address.AddressLine1;
                        a.State = new State();
                        a.State.Code = address.StateCode;
                        a.PostalCode = address.PostalCode;
                        a.IsPrimary = true;
                        a.IsCorporate = true;
                        if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                        {
                            a.IsValidForTPIPurchase = IsValidAddressForTPIpayment(address.CountryCode);

                            if (a.IsValidForTPIPurchase)
                            {
                                a.IsValidForTPIPurchase = IsValidSateForTPIpayment(address.StateCode);
                            }
                        }
                        mobAddresses.Add(a);
                    }
                }
            }
            return mobAddresses;
            #endregion
        }
        private List<MOBEmail> PopulateCorporateEmailAddresses(List<United.CorporateDirect.Models.CustomerProfile.Email> emailAddresses, bool onlyDayOfTravelContact)
        {
            #region
            List<MOBEmail> mobEmailAddresses = new List<MOBEmail>();
            bool isCorpEmailPresent = false;

            if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
            {
                //As per Business / DotCom Kalpen; we are removing the condition for checking the Effectivedate and Discontinued date
                var corpIndex = emailAddresses.FindIndex(x => x.ChannelTypeDescription != null && x.ChannelTypeDescription.ToLower() == "corporate" && x.EmailAddress != null && x.EmailAddress.Trim() != "");
                if (corpIndex >= 0)
                    isCorpEmailPresent = true;

            }

            if (emailAddresses != null && emailAddresses.Count > 0)
            {
                MOBEmail primaryEmailAddress = null;
                int co = 0;
                foreach (United.CorporateDirect.Models.CustomerProfile.Email email in emailAddresses)
                {
                    if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                    {
                        if (isCorpEmailPresent && !onlyDayOfTravelContact && email.ChannelTypeDescription.ToLower() == "corporate")
                        {
                            primaryEmailAddress = new MOBEmail();
                            primaryEmailAddress.Channel = new SHOPChannel();
                            primaryEmailAddress.EmailAddress = email.EmailAddress;
                            primaryEmailAddress.Channel.ChannelCode = email.ChannelCode;
                            primaryEmailAddress.Channel.ChannelDescription = email.ChannelCodeDescription;
                            primaryEmailAddress.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                            primaryEmailAddress.Channel.ChannelTypeDescription = email.ChannelTypeDescription;
                            primaryEmailAddress.IsPrimary = true;
                            break;
                        }
                        else if (isCorpEmailPresent && !onlyDayOfTravelContact && email.ChannelTypeDescription.ToLower() != "corporate")
                        {
                            continue;
                        }
                    }
                    //Fix for CheckOut ArgNull Exception - Empty EmailAddress with null EffectiveDate & DiscontinuedDate for Corp Account Revenue Booking (MOBILE-9873) - Shashank : Added OR condition to allow CorporateAccount ProfileOwner.
                    if ((!_configuration.GetValue<bool>("DisableCheckforCorpAccEmail")
                            && email.IsProfileOwner == true && primaryEmailAddress.IsNullOrEmpty()))
                    {
                        #region As per Wade Change want to filter out to return only Primary email to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                        co = co + 1;
                        MOBEmail e = new MOBEmail();
                        e.Channel = new SHOPChannel();
                        e.EmailAddress = email.EmailAddress;
                        e.Channel.ChannelCode = email.ChannelCode;
                        e.Channel.ChannelDescription = email.ChannelCodeDescription;
                        e.Channel.ChannelTypeCode = email.ChannelTypeCode.ToString();
                        e.Channel.ChannelTypeDescription = email.ChannelTypeDescription;

                        if (!onlyDayOfTravelContact)
                        {
                            if (co == 1)
                            {
                                primaryEmailAddress = new MOBEmail();
                                primaryEmailAddress = e;
                            }
                        }
                        #endregion
                    }
                }
                if (primaryEmailAddress != null)
                {
                    mobEmailAddresses.Add(primaryEmailAddress);
                }
            }
            return mobEmailAddresses;
            #endregion
        }

        private List<MOBCPPhone> PopulateCorporatePhones(List<United.CorporateDirect.Models.CustomerProfile.Phone> phones, bool onlyDayOfTravelContact)
        {
            List<MOBCPPhone> mobCPPhones = new List<MOBCPPhone>();
            bool isCorpPhonePresent = false;


            if (_configuration.GetValue<bool>("CorporateConcurBooking") && IsCorpBookingPath)
            {
                var corpIndex = phones.FindIndex(x => x.ChannelTypeDescription != null && x.ChannelTypeDescription.ToLower() == "corporate" && x.PhoneNumber != null && x.PhoneNumber != "");
                if (corpIndex >= 0)
                    isCorpPhonePresent = true;
            }


            if (phones != null && phones.Count > 0)
            {
                MOBCPPhone primaryMobCPPhone = null;
                int co = 0;
                foreach (United.CorporateDirect.Models.CustomerProfile.Phone phone in phones)
                {
                    #region As per Wade Change want to filter out to return only Primary Phone to client if not primary phone exist return the first one from the list.So lot of work at client side will save time. Sep 15th 2014
                    MOBCPPhone mobCPPhone = new MOBCPPhone();
                    co = co + 1;
                    mobCPPhone.PhoneNumber = phone.PhoneNumber;
                    mobCPPhone.ChannelCode = phone.ChannelCode;
                    mobCPPhone.ChannelCodeDescription = phone.ChannelCodeDescription;
                    mobCPPhone.ChannelTypeCode = Convert.ToString(phone.ChannelTypeCode);
                    mobCPPhone.ChannelTypeDescription = phone.ChannelTypeDescription;
                    mobCPPhone.ChannelTypeDescription = phone.ChannelTypeDescription;
                    mobCPPhone.ChannelTypeSeqNumber = 0;
                    mobCPPhone.CountryCode = phone.CountryCode;
                    mobCPPhone.IsProfileOwner = phone.IsProfileOwner;
                    if (phone.PhoneDevices != null && phone.PhoneDevices.Count > 0)
                    {
                        mobCPPhone.DeviceTypeCode = phone.PhoneDevices[0].CommDeviceTypeCode;
                    }
         
                        #region
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.ChannelTypeDescription.ToLower() == "corporate")
                        {
                            //return the corporate phone number
                            primaryMobCPPhone = new MOBCPPhone();
                            mobCPPhone.IsPrimary = true;
                            primaryMobCPPhone = mobCPPhone;
                            break;

                        }
                        if (IsCorpBookingPath && isCorpPhonePresent && !onlyDayOfTravelContact && phone.ChannelTypeDescription.ToLower() != "corporate")
                        {
                            //There is corporate phone number present, continue till corporate phone number is found
                            continue;
                        }
                        #endregion
                    

                    if (!onlyDayOfTravelContact)
                    {
                        if (co == 1)
                        {
                            primaryMobCPPhone = new MOBCPPhone();
                            primaryMobCPPhone = mobCPPhone;
                        }
                    }
                    #endregion
                }
                if (primaryMobCPPhone != null)
                {
                    mobCPPhones.Add(primaryMobCPPhone);
                }
            }
            return mobCPPhones;
        }
        private List<MOBPrefAirPreference> PopulateCorporateAirPrefrences(List<United.CorporateDirect.Models.CustomerProfile.AirPreference> airPreferences)
        {
            List<MOBPrefAirPreference> mobAirPrefs = new List<MOBPrefAirPreference>();
            if (airPreferences != null && airPreferences.Count > 0)
            {
                foreach (United.CorporateDirect.Models.CustomerProfile.AirPreference pref in airPreferences)
                {
                    MOBPrefAirPreference mobAirPref = new MOBPrefAirPreference();
                    mobAirPref.MealCode = pref.MealCode;
                    mobAirPrefs.Add(mobAirPref);
                }
            }
            return mobAirPrefs;
        }
        #endregion

    }
}