using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Profile;
using United.Mobile.DataAccess.ShopProducts;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.CSLModels;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.PersonalizationModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Services.ProfileValidation.Common;
using United.Utility.Helper;
using JsonSerializer = United.Utility.Helper.DataContextJsonSerializer;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Session = United.Mobile.Model.Common.Session;
using Traveler = United.Services.Customer.Common.Traveler;

namespace United.Common.Helper.Profile
{
    public class CustomerProfile : ICustomerProfile
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ICacheLog<CustomerProfile> _logger;
        private readonly ICustomerDataService _customerDataService;
        private readonly ICustomerPreferencesService _customerPreferencesService;
        private readonly IProfileService _profileService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IReferencedataService _referencedataService;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IMPTraveler _mpTraveler;
        private readonly IDPService _tokenService;
        private readonly IShoppingCcePromoService _shoppingCcePromoService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private bool IsCorpBookingPath = false;
        private bool IsArrangerBooking = false;
        private readonly ICustomerProfileTravelerService _customerProfileTravelerService;
        private readonly ICorporateProfile _corpProfile;
        private readonly IProfileCreditCard _profileCreditCard;
        private string _deviceId = string.Empty;
        private readonly IHeaders _headers;

        public CustomerProfile(
             IConfiguration configuration
            , IReferencedataService referencedataService
            , IDynamoDBService dynamoDBService
            , ISessionHelperService sessionHelperService
            , ICustomerDataService mPEnrollmentService
            , ICacheLog<CustomerProfile> logger
            , IProfileService profileService
            , ICustomerPreferencesService customerPreferencesService
            , IMPTraveler mPTraveler
            , IUtilitiesService utilitiesService
             , IDPService tokenService
            , IShoppingCcePromoService shoppingCcePromoService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , ICustomerProfileTravelerService customerProfileTravelerService
            , ICorporateProfile corpProfile
            , IHeaders headers
            , IProfileCreditCard profileCreditCard
            )
        {
            _configuration = configuration;
            _referencedataService = referencedataService;
            _dynamoDBService = dynamoDBService;
            _sessionHelperService = sessionHelperService;
            _customerDataService = mPEnrollmentService;
            _logger = logger;
            _profileService = profileService;
            _customerPreferencesService = customerPreferencesService;
            _utilitiesService = utilitiesService;
            _mpTraveler = mPTraveler;
            _tokenService = tokenService;
            _shoppingCcePromoService = shoppingCcePromoService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _customerProfileTravelerService = customerProfileTravelerService;
            _corpProfile = corpProfile;
            _headers = headers;
            _profileCreditCard = profileCreditCard;
        }

        public async Task<List<MOBCPProfile>> PopulateProfiles(string sessionId, string mileagePlusNumber, int customerId, List<Services.Customer.Common.Profile> profiles, MOBCPProfileRequest request, bool getMPSecurityDetails = false, string path = "", MOBApplication application = null)
        {
            List<MOBCPProfile> mobProfiles = null;
            if (profiles != null && profiles.Count > 0)
            {
                CSLProfile persistedCSLProfile = new CSLProfile();
                persistedCSLProfile = await _sessionHelperService.GetSession<CSLProfile>(sessionId, persistedCSLProfile.ObjectName, new List<string>() { sessionId, persistedCSLProfile.ObjectName }).ConfigureAwait(false);
                if (persistedCSLProfile == null)
                {
                    persistedCSLProfile = new CSLProfile();
                    persistedCSLProfile.SessionId = sessionId;
                    persistedCSLProfile.MileagePlusNumber = mileagePlusNumber;
                    persistedCSLProfile.CustomerId = customerId;
                }
                if (persistedCSLProfile.Profiles == null)
                {
                    mobProfiles = new List<MOBCPProfile>();
                    persistedCSLProfile.Profiles = mobProfiles;
                }
                else
                {
                    mobProfiles = persistedCSLProfile.Profiles;
                }

                foreach (var profile in profiles)
                {
                    if (profile.Travelers != null && profile.Travelers.Count > 0)
                    {
                        MOBCPProfile mobProfile = new MOBCPProfile();
                        mobProfile.AirportCode = profile.AirportCode;
                        mobProfile.AirportNameLong = profile.AirportNameLong;
                        mobProfile.AirportNameShort = profile.AirportNameShort;
                        mobProfile.Description = profile.Description;
                        mobProfile.Key = profile.Key;
                        mobProfile.LanguageCode = profile.LanguageCode;
                        mobProfile.ProfileId = profile.ProfileId;
                        mobProfile.ProfileMembers = PopulateProfileMembers(profile.ProfileMembers);
                        mobProfile.ProfileOwnerId = profile.ProfileOwnerId;
                        mobProfile.ProfileOwnerKey = profile.ProfileOwnerKey;
                        mobProfile.QuickCustomerId = profile.QuickCustomerId;
                        mobProfile.QuickCustomerKey = profile.QuickCustomerKey;
                        if (_configuration.GetValue<bool>("CorporateConcurBooking"))
                        {
                            mobProfile.CorporateData = _corpProfile.PopulateCorporateData(profile.CorporateData, application);
                        }
                        bool isProfileOwnerTSAFlagOn = false;
                        //List<MOBKVP> mpList = new List<MOBKVP>();
                        var tupleRes = await _mpTraveler.PopulateTravelers(profile.Travelers, mileagePlusNumber, isProfileOwnerTSAFlagOn, false, request, sessionId, getMPSecurityDetails, path);
                        mobProfile.Travelers = tupleRes.mobTravelersOwnerFirstInList;
                        mobProfile.SavedTravelersMPList = tupleRes.savedTravelersMPList;
                        mobProfile.IsProfileOwnerTSAFlagON = tupleRes.isProfileOwnerTSAFlagOn;
                        if (mobProfile != null)
                        {
                            mobProfile.DisclaimerList = await _mpTraveler.GetProfileDisclaimerList();
                        }
                        mobProfiles.Add(mobProfile);
                    }
                }
            }

            return mobProfiles;
        }


        private async Task<Reservation> PersistedReservation(MOBCPProfileRequest request)
        {
            Reservation persistedReservation =
                new Reservation();
            if (request != null)
                //to do persistedReservation = Persist.FilePersist.Load<Reservation>(request.SessionId,persistedReservation.ObjectName);
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
        private List<MOBCPProfileMember> PopulateProfileMembers(List<United.Services.Customer.Common.ProfileMember> profileMembers)
        {
            List<MOBCPProfileMember> mobProfileMembers = null;

            if (profileMembers != null && profileMembers.Count > 0)
            {
                mobProfileMembers = new List<MOBCPProfileMember>();
                foreach (var profileMember in profileMembers)
                {
                    MOBCPProfileMember mobProfileMember = new MOBCPProfileMember();
                    mobProfileMember.CustomerId = profileMember.CustomerId;
                    mobProfileMember.Key = profileMember.Key;
                    mobProfileMember.LanguageCode = profileMember.LanguageCode;
                    mobProfileMember.ProfileId = profileMember.ProfileId;

                    mobProfileMembers.Add(mobProfileMember);
                }
            }

            return mobProfileMembers;
        }
        public async System.Threading.Tasks.Task GetChaseCCStatement(MOBCPProfileRequest req)
        {
            //MOBCCAdStatement adStatement = null;
            try
            {
                if (req.ChaseAdType != CHASEADTYPE.NONE.ToString())
                {
                    Reservation bookingReservation = new Reservation();
                    bookingReservation = await _sessionHelperService.GetSession<Reservation>(req.SessionId, bookingReservation.ObjectName, new List<string> { req.SessionId, bookingReservation.ObjectName }).ConfigureAwait(false);

                    if (bookingReservation != null && !string.IsNullOrEmpty(bookingReservation.PointOfSale) && bookingReservation.PointOfSale.ToUpper().Equals("US") &&
                        bookingReservation.ShopReservationInfo2.ChaseCreditStatement == null && !bookingReservation.IsReshopChange && !bookingReservation.AwardTravel)
                    {
                        if (bookingReservation.ShopReservationInfo2 == null)
                            bookingReservation.ShopReservationInfo2 = new ReservationInfo2();

                        bookingReservation.ShopReservationInfo2.ChaseCreditStatement = BuildChasePromo(req.ChaseAdType.ToString());
                        //adStatement = bookingReservation.ShopReservationInfo2.ChaseCreditStatement;

                        if (_configuration.GetValue<bool>("EnableChaseBannerFromCCE") && GeneralHelper.IsApplicationVersionGreater(req.Application.Id, req.Application.Version.Major, "AndroidChaseCCEPromoVersion", "iPhoneChaseCCEPromoVersion", "", "", true, _configuration))
                        {
                            try
                            {
                                CrediCardAdCCEPromoBanner chaseCCEResponse = await GetChasePromoFromCCE(req.MileagePlusNumber, _configuration.GetValue<string>("ChaseBannerCCERequestComponentToLoad"), _configuration.GetValue<string>("ChaseBannerCCERequestPageToLoad"), (MOBRequest)req, req.SessionId, "GetChaseCCStatement");
                                if (bookingReservation.ShopReservationInfo2.ChaseCreditStatement == null)
                                    bookingReservation.ShopReservationInfo2.ChaseCreditStatement = new MOBCCAdStatement();
                                bookingReservation.ShopReservationInfo2.ChaseCreditStatement.chaseBanner = new CCAdCCEPromoBanner()
                                {
                                    BannerImage = chaseCCEResponse.BannerImage,
                                    MessageKey = chaseCCEResponse.MessageKey,
                                    DisplayPriceCalculation = chaseCCEResponse.DisplayPriceCalculation,
                                    MakeFeedBackCall = chaseCCEResponse.MakeFeedBackCall,

                                    PlacementLandingPageURL = chaseCCEResponse.PlacementLandingPageURL

                                };
                                bookingReservation.ShopReservationInfo2.ChaseCreditStatement.statementCreditDisplayPrice = string.Empty;
                                bookingReservation.ShopReservationInfo2.ChaseCreditStatement.statementCreditDisplayPrice = (Convert.ToDecimal(chaseCCEResponse.StatementCredit)).ToString("C2", CultureInfo.CurrentCulture);
                            }
                            catch (MOBUnitedException coex)
                            {
                                _logger.LogWarning("GetChaseCCStatement {@UnitedException}", JsonConvert.SerializeObject(coex));
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("GetChaseCCStatement {@Exception}", JsonConvert.SerializeObject(ex));
                            }
                        }

                        await _sessionHelperService.SaveSession<Reservation>(bookingReservation, req.SessionId, new List<string> { req.SessionId, bookingReservation.ObjectName }, bookingReservation.ObjectName).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetChaseCCStatement {@Exception}", JsonConvert.SerializeObject(ex));
            }
            //return adStatement;
        }
        public MOBCCAdStatement BuildChasePromo(string adType)
        {
            MOBCCAdStatement CCAdStatement = null;

            if (!adType.ToUpper().Equals(CHASEADTYPE.NONE.ToString()))
            {
                try
                {
                    Dictionary<string, string> imagePaths = GetImageUrl();
                    if (imagePaths.Count > 0)
                    {
                        string chaseDomainURL = _configuration.GetValue<string>("ChaseDomainNameURL");
                        CCAdStatement = new MOBCCAdStatement();

                        CCAdStatement.ccImage = string.Format("{0}/{1}", chaseDomainURL, imagePaths["ccImage"]);

                        CCAdStatement.bannerImage = new Mobile.Model.Shopping.Image();

                        if (adType.ToUpper() == CHASEADTYPE.PREMIER.ToString())
                        {
                            CCAdStatement.bannerImage.PhoneUrl = string.Format("{0}/{1}", chaseDomainURL, imagePaths["PrimePhoneUrl"]);
                            CCAdStatement.bannerImage.TabletUrl = string.Format("{0}/{1}", chaseDomainURL, imagePaths["PrimeTabUrl"]);
                        }
                        else
                        {
                            CCAdStatement.bannerImage.PhoneUrl = string.Format("{0}/{1}", chaseDomainURL, imagePaths["NPrimePhoneUrl"]);
                            CCAdStatement.bannerImage.TabletUrl = string.Format("{0}/{1}", chaseDomainURL, imagePaths["NPrimeTabUrl"]);
                        }

                        CCAdStatement.statementCreditDisplayPrice = (Convert.ToDecimal(_configuration.GetValue<string>("ChaseStatementCredit"))).ToString("C2", CultureInfo.CurrentCulture);
                    }
                }
                catch (Exception e) { throw e; }
            }

            return CCAdStatement;
        }

        public bool EnableYoungAdult(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableYoungAdultBooking") && !isReshop;
        }

        public async Task<CrediCardAdCCEPromoBanner> GetChasePromoFromCCE(string mileagePlusNumber, string componentToLoad, string pageToLoad, MOBRequest mobRequest, string sessionId, string logAction)
        {
            CrediCardAdCCEPromoBanner response = null;
            //build the request 
            United.Service.Presentation.PersonalizationRequestModel.ContextualCommRequest cceRequest = new Service.Presentation.PersonalizationRequestModel.ContextualCommRequest();
            cceRequest.ComponentsToLoad = new System.Collections.ObjectModel.Collection<string> { componentToLoad };
            cceRequest.PageToLoad = pageToLoad;
            cceRequest.MileagePlusId = mileagePlusNumber;
            cceRequest.ChannelType = _configuration.GetValue<string>("ChaseBannerCCERequestChannelName");
            cceRequest.IPCountry = _configuration.GetValue<string>("ChaseBannerCCERequestCountryCode");
            cceRequest.LangCode = _configuration.GetValue<string>("ChaseBannerCCERequestLanguageCode");

            //call CCE 
            var jsonRequest = JsonSerializer.SerializeToJSON(cceRequest);
            var token = await GetSessionToken(sessionId);
            //var jsonResponse = PostAndLog(sessionId, url, jsonRequest, mobRequest, logAction, "GetChasePromoFromCCE");
            var jsonResponse = await _shoppingCcePromoService.ChasePromoFromCCE(token, jsonRequest, sessionId).ConfigureAwait(false);
            var cceResponse = string.IsNullOrEmpty(jsonResponse) ? null
                : JsonSerializer.Deserialize<ContextualCommResponse>(jsonResponse);

            //save the response to persist 
            CCEPromo ccePromoPersist = new CCEPromo();
            ccePromoPersist.ContextualCommRequest = cceRequest;
            ccePromoPersist.ContextualCommResponseJson = jsonResponse;
            await _sessionHelperService.SaveSession<CCEPromo>(ccePromoPersist, sessionId, new List<string> { sessionId, ccePromoPersist.ObjectName }, ccePromoPersist.ObjectName).ConfigureAwait(false);


            //build the mobile response  
            if (cceResponse != null && cceResponse.Components.Count > 0 && cceResponse.Components[0].ContextualElements.Count > 0
                && cceResponse.Components[0].ContextualElements[0].Value != null)
            {

                var cceValuesJson = Newtonsoft.Json.JsonConvert.SerializeObject(cceResponse.Components[0].ContextualElements[0].Value);
                ContextualMessage cceValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ContextualMessage>(cceValuesJson);

                if (cceValues?.Content?.Images?.Count > 1)
                {
                    response = new CrediCardAdCCEPromoBanner();
                    response.MessageKey = cceValues.MessageKey;
                    response.BannerImage = new Mobile.Model.Shopping.Image();
                    response.BannerImage.PhoneUrl = cceValues.Content.Images[0].Url;
                    response.BannerImage.TabletUrl = cceValues.Content.Images[1].Url;

                    string doTheMath = string.Empty;
                    string statementCredit = string.Empty;
                    string offerTrackingId = string.Empty;

                    cceValues.Params.TryGetValue("DoTheMath", out doTheMath);
                    cceValues.Params.TryGetValue("StatementCredit", out statementCredit);
                    cceValues.Params.TryGetValue("OfferTrackingId", out offerTrackingId);

                    response.MakeFeedBackCall = !string.IsNullOrEmpty(offerTrackingId) ? true : false;
                    response.DisplayPriceCalculation = (!string.IsNullOrEmpty(doTheMath) && Convert.ToBoolean(doTheMath)) ? true : false;
                    response.StatementCredit = statementCredit;

                    if (!_configuration.GetValue<bool>("TurnOffCCELinkMOBILE-10054"))
                    {
                        response.PlacementLandingPageURL = cceValues.Content?.Links?.FirstOrDefault()?.Link;
                    }

                }
                else
                {
                    throw new MOBUnitedException("Chase promo content or Images are not found from CCE response");
                }
            }
            else
            {
                throw new MOBUnitedException($"Chase ad not available from CCE for {mileagePlusNumber}");
            }
            return response;
        }
        private Dictionary<string, string> GetImageUrl()
        {
            Dictionary<string, string> dicImageUrl = new Dictionary<string, string>();
            string[] urls = (_configuration.GetValue<string>("ChaseImages") ?? "").Split('^');
            foreach (string url in urls)
            {
                string[] imagepath = url.Split('|');
                dicImageUrl.Add(imagepath[0], imagepath[1]);
            }

            return dicImageUrl;
        }
        private async Task<string> GetSessionToken(string sessionId)
        {
            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            return session.Token;
        }

        #region UCB Migration Mobile Phase 3 Changes
        private async Task<List<MOBCPProfile>> GetProfileV2(MOBCPProfileRequest request, bool getMPSecurityDetails = false)
        {
            if (request == null)
            {
                throw new MOBUnitedException("Profile request cannot be null.");
            }
            var persistedReservation = await PersistedReservation(request);

            List<MOBCPProfile> profiles = null;

            if (!_configuration.GetValue<bool>("YAESubscrptionIssue"))
            {
                if (persistedReservation != null)
                {
                    if (EnableYoungAdult(persistedReservation.IsReshopChange))
                    {
                        if (persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.IsYATravel)
                        {
                            request.ProfileOwnerOnly = true;//For young Adult profile owner only allowed to book ticket.
                        }
                    }
                }
            }

            var response = await GetProfileDetails(request, getMPSecurityDetails, persistedReservation?.ShopReservationInfo?.IsCorporateBooking == true);

            if (response != null && response.Data != null)
            {
                if (_configuration.GetValue<bool>("YAESubscrptionIssue"))
                {
                    if (persistedReservation != null)
                    {
                        if (persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.IsYATravel)
                        {
                            GetProfileOwner(response);
                        }
                    }
                    if (getMPSecurityDetails)
                    {
                        GetProfileOwner(response);
                    }
                }
                profiles = await PopulateProfilesV2(request.SessionId, request.MileagePlusNumber, request.CustomerId, response.Data.Travelers, request, path: request.Path, application: request.Application, isCorporateBooking: persistedReservation?.ShopReservationInfo?.IsCorporateBooking == true, getMPSecurityDetails: getMPSecurityDetails);
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }


            return profiles;
        }
        private static void GetProfileOwner(Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse> response)
        {
            if (response.Data.Travelers != null && response.Data.Travelers.Count > 0)
            {
                TravelerProfileResponse owner = response.Data.Travelers.First(t => t.Profile != null && t.Profile.ProfileOwnerIndicator);
                response.Data.Travelers = new List<TravelerProfileResponse>();
                response.Data.Travelers.Add(owner);
            }
        }
        private async Task<List<MOBCPProfile>> PopulateProfilesV2(string sessionId, string mileagePlusNumber, int customerId, List<TravelerProfileResponse> profilesTravelers, MOBCPProfileRequest request, bool getMPSecurityDetails = false, string path = "", MOBApplication application = null, bool isCorporateBooking = false)
        {
            List<MOBCPProfile> mobProfiles = null;
            if (profilesTravelers != null && profilesTravelers.Count > 0)
            {
                CSLProfile persistedCSLProfile = new CSLProfile();
                persistedCSLProfile = await _sessionHelperService.GetSession<CSLProfile>(sessionId, persistedCSLProfile.ObjectName, new List<string>() { sessionId, persistedCSLProfile.ObjectName }).ConfigureAwait(false);
                if (persistedCSLProfile == null)
                {
                    persistedCSLProfile = new CSLProfile();
                    persistedCSLProfile.SessionId = sessionId;
                    persistedCSLProfile.MileagePlusNumber = mileagePlusNumber;
                    persistedCSLProfile.CustomerId = customerId;
                }
                if (persistedCSLProfile.Profiles == null)
                {
                    mobProfiles = new List<MOBCPProfile>();
                    persistedCSLProfile.Profiles = mobProfiles;
                }
                else
                {
                    mobProfiles = persistedCSLProfile.Profiles;
                }

                TravelerProfileResponse owner = profilesTravelers.First(t => t.Profile?.ProfileOwnerIndicator == true);
                if (owner != null && owner.Profile != null)
                {
                    MOBCPProfile mobProfile = new MOBCPProfile();
                    if (owner.AirPreferences != null)
                    {
                        mobProfile.AirportCode = owner.AirPreferences[0].AirportCode;
                        mobProfile.AirportNameLong = owner.AirPreferences[0].AirportNameLong;
                        mobProfile.AirportNameShort = owner.AirPreferences[0].AirportNameShort;
                    }
                    mobProfile.ProfileId = Convert.ToInt32(owner.Profile.ProfileId);
                    mobProfile.ProfileOwnerId = Convert.ToInt32(owner.Profile.ProfileOwnerId);
                    mobProfile.ProfileOwnerKey = owner.Profile.TravelerKey;
                    if (isCorporateBooking || getMPSecurityDetails)
                    {
                        mobProfile.CorporateData = await _corpProfile.PopulateCorporateData(request);
                    }
                    bool isProfileOwnerTSAFlagOn = false;
                    //List<MOBKVP> mpList = new List<MOBKVP>();
                    var tupleRes = await _mpTraveler.PopulateTravelersV2(profilesTravelers, mileagePlusNumber, isProfileOwnerTSAFlagOn, false, request, request.SessionId, getMPSecurityDetails, path);
                    mobProfile.Travelers = tupleRes.mobTravelersOwnerFirstInList;
                    mobProfile.SavedTravelersMPList = tupleRes.savedTravelersMPList;
                    mobProfile.IsProfileOwnerTSAFlagON = tupleRes.isProfileOwnerTSAFlagOn;
                    if (mobProfile != null)
                    {
                        mobProfile.DisclaimerList = await _mpTraveler.GetProfileDisclaimerList();
                    }
                    mobProfiles.Add(mobProfile);
                }
            }

            return mobProfiles;
        }
        public async Task<Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse>> GetProfileDetails(MOBCPProfileRequest request, bool getMPSecurityDetails = false, bool isCorporateBooking = false)
        {
            Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse> cslTravelersProfileResponse = new Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse>();
            cslTravelersProfileResponse = await MakeProfileTravelerServiceCall(request);
            await _mpTraveler.MakeProfileOwnerServiceCall(request);
            if (!getMPSecurityDetails)
            {
                await _profileCreditCard.MakeProfileCreditCardsServiecall(request);
            }
            if (getMPSecurityDetails || isCorporateBooking)
            {
                await _corpProfile.MakeCorpProfileServiecall(request);
            }
            if (!getMPSecurityDetails && isCorporateBooking)
            {
                await _corpProfile.MakeCorpFopServiceCall(request);
            }
            return cslTravelersProfileResponse;
        }

        public async Task<Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse>> MakeProfileTravelerServiceCall(MOBCPProfileRequest request)
        {
            return await _customerProfileTravelerService.GetProfileTravelerInfo<United.Mobile.Model.CSLModels.CslResponse<TravelersProfileResponse>>(request.Token, request.SessionId, request.MileagePlusNumber).ConfigureAwait(false);
        }
        #endregion 




    }
}
