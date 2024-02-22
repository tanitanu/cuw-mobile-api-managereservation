using Amazon.Runtime.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper;
using United.Common.Helper.Shopping;
using United.Definition.SeatCSL30;
using United.Mobile.DataAccess.CMSContent;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ShopSeats;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.ShopSeats;
using United.Service.Presentation.ReferenceDataModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using MOBSeatMap = United.Mobile.Model.Shopping.MOBSeatMap;
using PcuUpgradeOption = United.Mobile.Model.Shopping.Pcu.PcuUpgradeOption;
using Seat = United.Definition.SeatCSL30.Seat;
using SeatMapRequest = United.Definition.SeatCSL30.SeatMapRequest;

namespace United.Common.HelperSeatEngine
{
    public class SeatMapCSL30 : ISeatMapCSL30
    {
        private readonly ICacheLog<SeatMapCSL30> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ISeatMapEngine _seatMapEngine;
        private readonly IDPService _dPService;
        private readonly ISeatMapCSL30Service _seatMapCSL30service;
        private readonly ICMSContentService _iCMSContentService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ISeatEngine _seatEngine;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly ICachingService _cachingService;
        private readonly IHeaders _headers;
        private readonly IFeatureSettings _featureSettings;

        public SeatMapCSL30(ICacheLog<SeatMapCSL30> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , ISeatMapCSL30Service seatMapCSL30service
            , IDynamoDBService dynamoDBService
            , ISeatEngine seatEngine
            , ICMSContentService cMSContentService
            , IDPService dPService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IShoppingUtility shoppingUtility
            , ICachingService cachingService
            , IHeaders headers
            , ISeatMapEngine seatMapEngine
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _dynamoDBService = dynamoDBService;
            _seatMapCSL30service = seatMapCSL30service;
            _seatEngine = seatEngine;
            _iCMSContentService = cMSContentService;
            _dPService = dPService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
            _seatMapEngine = seatMapEngine;
            ConfigUtility.UtilityInitialize(_configuration);
            _shoppingUtility = shoppingUtility;
            _cachingService = cachingService;
            _featureSettings = featureSettings;
        }



        private SeatMapRequest BuildSeatMapRequest
           (string flow, string languageCode, string channelId, string channelName,
           string recordLocator, bool isAwardReservation, bool isIBE, string cartId = null)
        {
            SeatMapRequest request = new SeatMapRequest();

            if (!string.IsNullOrEmpty(recordLocator))
            {
                request.RecordLocator = recordLocator;
            }

            request.ChannelId = channelId;
            request.ChannelName = channelName;
            request.CartId = cartId;
            request.IsAwardReservation = isAwardReservation;
            request.ProductCode = isIBE
                ? _configuration.GetValue<string>("IBEProductDescription") : string.Empty;

            request.IsFrontCabin = true;
            request.IsUppCabin = true;
            request.Travelers = null;
            return request;
        }

        
        private async Task<(MOBSeatMap, int countNoOfFreeSeats, int countNoOfPricedSeats)> GetSeatMapCSL(United.Definition.SeatCSL30.SeatMap seatMapResponse, string sessionId, bool isELF, bool isIBE, bool isOaSeatMapSegment, int segmentIndex, string flow, int appId, string appVersion, bool isEnablePcuDeepLinkInSeatMap, bool isEnablePCUSeatMapPurchaseManageRes, int countNoOfFreeSeats, int countNoOfPricedSeats, string bookingCabin, bool cogStop, bool isSeatMFOPEnabled, bool isVerticalSeatMapEnabled, int ePlusSubscriberCount, bool isBERecommendedSeatsAvailable, Session session, bool isOANoSeatMapAvailableNewMessageEnabled)
        {
            MOBSeatMap objMOBSeatMap = new MOBSeatMap();
            List<string> cabinBrandingDescriptions = new List<string>();
            United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 objMOBSeatMapCSL30 = new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30();

            objMOBSeatMap.SeatMapAvailabe = true;
            objMOBSeatMap.FlightNumber = objMOBSeatMapCSL30.FlightNumber = seatMapResponse.FlightInfo.MarketingFlightNumber;
            objMOBSeatMap.FlightDateTime = objMOBSeatMapCSL30.FlightDateTime = seatMapResponse.FlightInfo.DepartureDate.ToString("MM/dd/yyyy hh:mm tt");
            objMOBSeatMap.Arrival = new MOBAirport { Code = seatMapResponse.FlightInfo.ArrivalAirport };
            objMOBSeatMap.Departure = new MOBAirport { Code = seatMapResponse.FlightInfo.DepartureAirport };
            objMOBSeatMap.IsOaSeatMap = objMOBSeatMapCSL30.IsOaSeatMap = isOaSeatMapSegment;
            objMOBSeatMap.FleetType = !string.IsNullOrEmpty(seatMapResponse.AircraftInfo.Icr) ? seatMapResponse.AircraftInfo.Icr : string.Empty;
            // SupressLMX only for booking path
            bool supressLMX = _seatEngine.SupressLMX(appId);

            // New model to save in persist
            objMOBSeatMapCSL30.ArrivalCode = seatMapResponse.FlightInfo.ArrivalAirport;
            objMOBSeatMapCSL30.DepartureCode = seatMapResponse.FlightInfo.DepartureAirport;
            objMOBSeatMapCSL30.MarketingCarrierCode = seatMapResponse.FlightInfo.MarketingCarrierCode;
            objMOBSeatMapCSL30.OperatingCarrierCode = seatMapResponse.FlightInfo.OperatingCarrierCode;
            objMOBSeatMapCSL30.Flow = flow;
            objMOBSeatMapCSL30.SegmentNumber = segmentIndex;

            List<Mobile.Model.ShopSeats.MOBSeatCSL30> listMOBSeatCSL30 = new List<Mobile.Model.ShopSeats.MOBSeatCSL30>();

            bool isTravelerPricingEnabled = await _featureSettings.GetFeatureSettingValue("EnableMRSeatmapTierPricing").ConfigureAwait(false);

            objMOBSeatMap.LegId = string.Empty;
            int cabinCount = 0;

            /// Only in ManageRes -- code for PCU
            /// This code will not execute for booking as isEnablePcuDeepLinkInSeatMap returned as false in booking path
            List<PcuUpgradeOption> upgradeOffers = null;
            if (isEnablePcuDeepLinkInSeatMap)
            {
                var pcu = await new Helper.SeatEngine.PremiumCabinUpgrade(_sessionHelperService, sessionId, objMOBSeatMap.FlightNumber.ToString(), seatMapResponse.FlightInfo.DepartureAirport, seatMapResponse.FlightInfo.ArrivalAirport).LoadOfferStateforSeatMap();
                upgradeOffers = pcu.GetUpgradeOptionsForSeatMap();
                objMOBSeatMap.Captions = (isEnablePCUSeatMapPurchaseManageRes && (seatMapResponse?.Travelers?.Count ?? 0) > 1) ? await GetPcuCaptions(sessionId) : await GetPcuCaptions(pcu.GetTravelerNames(), pcu.RecordLocator);
            }

            bool hasDAASeat = false;
            bool hasFBBSeat = false;

            List<string> monuments = new List<string>();

            int numberOfCabins = seatMapResponse.Cabins.Count;
            bool isUpsellFlow = isVerticalSeatMapEnabled && await _shoppingUtility.IsEnableUpselltoUpsellInManageRes(appId, appVersion).ConfigureAwait(false) && ((seatMapResponse?.Travelers?.Count ?? 0) > 0) && seatMapResponse.Travelers.Any(x => x.TravelerPurchase != null && !string.IsNullOrEmpty(x.TravelerPurchase.OperationType) && x.TravelerPurchase.OperationType.Equals("Exchange", StringComparison.OrdinalIgnoreCase));

            List<CMSContentMessage> lstMessages = null;
            List<MOBMobileCMSContentMessages> seatmapUpsellMessage = new List<MOBMobileCMSContentMessages>();
            List<MOBMobileCMSContentMessages> seatmapTravelerMessage = new List<MOBMobileCMSContentMessages>();

            if (isUpsellFlow)
            {
                lstMessages = await GetSDLContentByGroupName(session.SessionId, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_MANAGERESOffers_Messages"), "ManageReservation_Offers_CMSContentMessagesCached_StaticGUID");

                seatmapUpsellMessage = _shoppingUtility.GetSDLMessageFromList(lstMessages, "UpselltoUpsell.SeatMapMessage");
                seatmapTravelerMessage = _shoppingUtility.GetSDLMessageFromList(lstMessages, "UpselltoUpsell.SeatMapLabels");

                if(objMOBSeatMap.AlertMessages == null)
                    objMOBSeatMap.AlertMessages = new List<InfoWarningMessages>();
                
                objMOBSeatMap.AlertMessages.Add(BuildUpsellSeatMapMessage(seatmapUpsellMessage));
                objMOBSeatMap.SeatPurchasedText = seatmapTravelerMessage?.FirstOrDefault()?.ContentFull;
            }
            int prevRowNumber = 0;
            foreach (United.Definition.SeatCSL30.Cabin cabin in seatMapResponse.Cabins)
            {
                ++cabinCount;
                bool firstCabin = (cabinCount == 1);
                Mobile.Model.Shopping.MOBCabin tmpCabin = new Mobile.Model.Shopping.MOBCabin();

                bool disableSeats = true;
                if (cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)
                    || cabin.CabinBrand.Equals("Business", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United Premium Plus", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase)
                    || cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Economy", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Coach", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("First", StringComparison.OrdinalIgnoreCase))
                {
                    if (cabin.CabinBrand.Equals(bookingCabin, StringComparison.OrdinalIgnoreCase))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("Economy", StringComparison.OrdinalIgnoreCase))
                    && (bookingCabin.Equals("Coach", StringComparison.OrdinalIgnoreCase)
                    || bookingCabin.Equals("Economy", StringComparison.OrdinalIgnoreCase)
                    || bookingCabin.Equals("United Economy", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("Business", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase))
                        && (bookingCabin.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("Business", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("First", StringComparison.OrdinalIgnoreCase))
                        && (bookingCabin.Equals("United First", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("First", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("United Global First", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                }

                /// Only in MR Path -- Code for PCU
                /// For MR isEnablePCUSeatMapPurchaseManageRes will be true
                double pcuOfferPriceForthisCabin = 0;
                string pcuOfferAmountForthisCabin = string.Empty;
                string cabinName = string.Empty;
                string pcuOfferOptionId = string.Empty;
                var upgradeOffer = isEnablePcuDeepLinkInSeatMap && upgradeOffers != null && upgradeOffers.Any() && !cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) ? upgradeOffers.FirstOrDefault(u => IsCabinMatchedCSL(u.UpgradeOptionDescription, cabin.CabinBrand)) : null;
                if (!upgradeOffer.IsNullOrEmpty())
                {
                    pcuOfferAmountForthisCabin = string.Format("{0}.00", upgradeOffer.FormattedPrice);
                    pcuOfferPriceForthisCabin = isEnablePCUSeatMapPurchaseManageRes ? upgradeOffer.Price : 0;
                    cabinName = upgradeOffer.UpgradeOptionDescription;
                    pcuOfferOptionId = _configuration.GetValue<bool>("TurnOff_DefaultSelectionForUpgradeOptions") ? string.Empty : upgradeOffer.OptionId;
                    tmpCabin.PcuOptionId = upgradeOffer.OptionId;
                    tmpCabin.HasAvailableSeats = cabin.AvailableSeats >= seatMapResponse.Travelers.Count;
                    tmpCabin.HasEnoughPcuSeats = !upgradeOffer.OptionId.IsNullOrEmpty() && tmpCabin.HasAvailableSeats;
                }
                else
                {
                    tmpCabin.HasAvailableSeats = cabin.AvailableSeats >= seatMapResponse.Travelers.Count;
                }
                ///  End
                tmpCabin.COS = isOaSeatMapSegment && cabin.IsUpperDeck ? "Upper Deck " + cabin.CabinBrand : cabin.CabinBrand;
                tmpCabin.Configuration = cabin.Layout;
                /// Checking with azhar as this is for other airlines and checking with cabin name.
                var isOaPremiumEconomyCabin = cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase);
                cabinBrandingDescriptions.Add(cabin.CabinBrand);

                if (isVerticalSeatMapEnabled)
                {
                    tmpCabin.FrontMonuments = GetMonuments(cabin, 0, 999, ref monuments);
                }


                foreach (United.Definition.SeatCSL30.Row row in cabin.Rows)
                {
                    if (!row.IsNullOrEmpty() && row.Number < 1000)
                    {
                        Mobile.Model.Shopping.MOBRow tmpRow = new Mobile.Model.Shopping.MOBRow();
                        tmpRow.Number = row.Number.ToString();
                        tmpRow.Wing = !isOaSeatMapSegment && row.Wing;
                        MonumentRow monumentRow = null;

                        if (isVerticalSeatMapEnabled)
                        {
                            LoadMonuments(row, prevRowNumber, cabin, ref tmpCabin);
                            monumentRow = cabin.MonumentRows.FirstOrDefault(x => x.VerticalGridNumber == row.VerticalGridNumber);

                            if (monumentRow != null && monumentRow.Monuments != null)
                            {
                                foreach (var monument in monumentRow.Monuments)
                                {
                                    if (!monuments.Contains(monument.ItemType))
                                    {
                                        monuments.Add(monument.ItemType);
                                    }
                                }
                            }
                        }

                        var cabinColumnCount = cabin.ColumnCount == 0 ? cabin.Layout.Length : cabin.ColumnCount;

                        for (int i = 1; i <= cabinColumnCount; i++)
                        {
                            MOBSeatB tmpSeat = null;
                            MOBSeatCSL30 objMOBSeatCSL30 = new MOBSeatCSL30();
                            var seat = row.Seats.FirstOrDefault(x => x.HorizontalGridNumber == i);
                            var monumentSeat = (!monumentRow.IsNullOrEmpty()) ? monumentRow.Monuments.FirstOrDefault(x => x.HorizontalGridNumber == i) : null;

                            if (!seat.IsNullOrEmpty())
                            {
                                // Build seatmap response for client
                                tmpSeat = new MOBSeatB();
                                tmpSeat.Exit = seat.IsExit;
                                tmpSeat.Fee = string.Empty; // Need to find and assign
                                tmpSeat.Number = seat.Number;

                                if (_configuration.GetValue<bool>("EnableLimitedReclineAllProducts"))
                                {
                                    tmpSeat.LimitedRecline = !isOaSeatMapSegment && IsLimitedRecline(seat);
                                }
                                else
                                {
                                    tmpSeat.LimitedRecline = !isOaSeatMapSegment && !string.IsNullOrEmpty(seat.DisplaySeatCategory)
                                    && isLimitedRecline(seat.DisplaySeatCategory);
                                }

                                // Need to revisit this code// checking only for united economy might includ UPP
                                if (!string.IsNullOrEmpty(seat.SeatType) && !isOaSeatMapSegment
                                    && (cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                                    || cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase)
                                    || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)))
                                {
                                    tmpSeat.Program = GetSeatPositionAccessFromCSL30SeatMap(seat.SeatType, ref hasDAASeat, ref hasFBBSeat);

                                    if (isVerticalSeatMapEnabled)
                                    {
                                        hasFBBSeat = seat.SeatType.Equals(SeatType.FBLEFT.ToString()) || seat.SeatType.Equals(SeatType.FBRIGHT.ToString()) || seat.SeatType.Equals(SeatType.FBFRONT.ToString()) || seat.SeatType.Equals(SeatType.FBBACK.ToString());
                                        hasDAASeat = seat.SeatType.Equals(SeatType.DAAFRONTL.ToString()) || seat.SeatType.Equals(SeatType.DAAFRONTR.ToString()) || seat.SeatType.Equals(SeatType.DAAFRONTRM.ToString()) || seat.SeatType.Equals(SeatType.DAALEFT.ToString()) || seat.SeatType.Equals(SeatType.DAARIGHT.ToString());
                                    }
                                }

                                tmpSeat.IsEPlus = !string.IsNullOrEmpty(seat.SeatType)
                                                                && seat.SeatType.Equals(SeatType.BLUE.ToString(), StringComparison.OrdinalIgnoreCase);
                                bool isBasicEconomy = isELF || isIBE;
                                bool disableEplusSeats = false; bool isEconomyCabinWithAdvanceSeats = false;
                                if (_configuration.GetValue<bool>("EnableEPlusSeatsForBasicEconomy"))
                                {
                                    isEconomyCabinWithAdvanceSeats = cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) && !tmpSeat.IsEPlus && isBasicEconomy;
                                }
                                else
                                {
                                    disableEplusSeats = tmpSeat.IsEPlus && isBasicEconomy;
                                    isEconomyCabinWithAdvanceSeats = cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) && disableEplusSeats;
                                }

                                tmpSeat.seatvalue = GetSeatValueFromCSL30SeatMap(seat, disableEplusSeats, disableSeats, null, isOaSeatMapSegment, isOaPremiumEconomyCabin, pcuOfferAmountForthisCabin, cogStop);

                                var tier = seatMapResponse.Tiers.FirstOrDefault(x => !x.IsNullOrEmpty() && x.Id == Convert.ToInt32(seat.Tier));
                                tmpSeat.ServicesAndFees = GetServicesAndFees(seat, pcuOfferAmountForthisCabin, pcuOfferPriceForthisCabin, tmpSeat.Program, tier, isVerticalSeatMapEnabled, isSeatMFOPEnabled);

                                bool isAdvanceSearchCouponApplied = EnableAdvanceSearchCouponBooking(appId, appVersion);
                                bool isCouponApplied = isAdvanceSearchCouponApplied ? tier?.Pricing != null && tier.Pricing.Any(x => x != null && !string.IsNullOrEmpty(x.CouponCode)) : false;
                                bool isFamilySeat = false;

                                if (isBERecommendedSeatsAvailable)
                                {
                                    if (seatMapResponse.TransactionIdentifiers.AdjacentSeatsList?.Contains(seat.Number) == true)
                                    {
                                        tmpSeat.FamilySeatingText = "Free family seating";
                                        isFamilySeat = true;
                                    }
                                }

                                bool disablePCUOfferPriceForBundles = _shoppingUtility.IsEnableTravelOptionsInViewRes(appId, appVersion) && flow == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString();
                                if (!disablePCUOfferPriceForBundles)
                                {
                                    tmpSeat.PcuOfferPrice = tmpSeat.seatvalue == "O" ? pcuOfferAmountForthisCabin : null;
                                    tmpSeat.IsPcuOfferEligible = tmpSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin == 0;
                                    tmpSeat.PcuOfferOptionId = tmpSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) ? pcuOfferOptionId : null;
                                }
                                tmpSeat.DisplaySeatFeature = GetDisplaySeatFeature(isOaSeatMapSegment, tmpSeat.seatvalue, pcuOfferAmountForthisCabin, cabinName, isEconomyCabinWithAdvanceSeats, cabin.CabinBrand, tmpSeat.IsEPlus, isFamilySeat, seat.OldSellableSeatCategory).Trim();
                                tmpSeat.DisplaySeatFeatureWithSymbols = GetDisplaySeatFeatureWithSymbols(isOaSeatMapSegment, tmpSeat.seatvalue, pcuOfferAmountForthisCabin, cabinName, isEconomyCabinWithAdvanceSeats, cabin.CabinBrand, tmpSeat.IsEPlus, isFamilySeat, seat.OldSellableSeatCategory).Trim();
                                tmpSeat.IsNoUnderSeatStorage = (_shoppingUtility.IsEnableBulkheadNoUnderSeatStorage(appId, appVersion) && seat.HasNoUnderSeatStorage);
                                tmpSeat.SeatFeatureList = GetSeatFeatureList(tmpSeat.seatvalue, supressLMX, tmpSeat.LimitedRecline, tmpSeat.IsEPlus, isEconomyCabinWithAdvanceSeats, cabin.CabinBrand, isCouponApplied, tmpSeat.Exit, isFamilySeat, seat.OldSellableSeatCategory, hasNoUnderSeatStorageAndBulkHead: tmpSeat.IsNoUnderSeatStorage);

                                if (isVerticalSeatMapEnabled)
                                {
                                    tmpSeat.DoorExit = seat.IsDoorExit;
                                    tmpSeat.MonumentType = seat.ItemType;
                                    tmpSeat.HorizontalSpan = "1";
                                    tmpSeat.Location = seat.Location;
                                }

                                if (isTravelerPricingEnabled)
                                {
                                    tmpSeat.TravelerPricing = GetTravelerPricing(pcuOfferAmountForthisCabin, pcuOfferPriceForthisCabin, tier, isSeatMFOPEnabled, isVerticalSeatMapEnabled, isUpsellFlow, seatmapTravelerMessage);
                                }

                                _seatEngine.CountNoOfFreeSeatsAndPricedSeats(tmpSeat, ref countNoOfFreeSeats, ref countNoOfPricedSeats);

                                // Seatmap response with traveler pricing to save in persisit
                                // This is backend model, Client is not using it.
                                // This code needs to be modified when client is changing.
                                objMOBSeatCSL30.Number = seat.Number;
                                objMOBSeatCSL30.Tier = seat.Tier;
                                objMOBSeatCSL30.TotalFee = tmpSeat.ServicesAndFees?.Count != 0 ? tmpSeat.ServicesAndFees[0].TotalFee : 0;
                                objMOBSeatCSL30.EDoc = seat.EDoc;
                                objMOBSeatCSL30.SeatType = seat.SeatType.ToUpper();
                                objMOBSeatCSL30.DisplaySeatCategory = seat.DisplaySeatCategory;
                                objMOBSeatCSL30.IsAvailable = seat.IsAvailable;
                                objMOBSeatCSL30.Pricing = new List<United.Mobile.Model.ShopSeats.TierPricingCSL30>();
                                if (!tier.IsNullOrEmpty() && tier.Pricing != null)
                                {
                                    foreach (var pricing in tier.Pricing)
                                    {
                                        var travelerPricing = new United.Mobile.Model.ShopSeats.TierPricingCSL30()
                                        {
                                            TravelerId = pricing.TravelerId,
                                            TotalPrice = pricing.TotalPrice,
                                            TravelerIndex = seatMapResponse.Travelers.FirstOrDefault(x => !x.IsNullOrEmpty() && x.Id == pricing.TravelerId).TravelerIndex,
                                            CouponCode = isAdvanceSearchCouponApplied ? pricing.CouponCode : string.Empty,
                                            OriginalPrice = isAdvanceSearchCouponApplied ? pricing.OriginalPrice : string.Empty,
                                            CurrencyCode = pricing.CurrencyCode
                                        };
                                        objMOBSeatCSL30.Pricing.Add(travelerPricing);
                                    }
                                }

                                listMOBSeatCSL30.Add(objMOBSeatCSL30);
                                tmpRow.Seats.Add(tmpSeat);
                            }
                            else
                            {
                                //get monumemt seat and loop based on span - build this empty seat.
                                //monumentseat.HorizontalSpan
                                if (!isVerticalSeatMapEnabled || (objMOBSeatMap.IsOaSeatMap && monumentSeat.IsNullOrEmpty()))
                                {

                                    tmpSeat = new MOBSeatB
                                    {
                                        Number = string.Empty,
                                        Fee = string.Empty,
                                        LimitedRecline = false,
                                        seatvalue = "-",
                                        Exit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsExit : false,
                                        DoorExit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsDoorExit : false,
                                    };

                                    if (cabin.Layout[i-1].Equals(' '))
                                    {
                                        tmpSeat.MonumentType = "AISLE";
                                    }

                                    tmpRow.Seats.Add(tmpSeat);
                                }
                                else
                                {
                                    if (!monumentSeat.IsNullOrEmpty() && !string.IsNullOrEmpty(monumentSeat.ItemType))
                                    {
                                        tmpSeat = new MOBSeatB
                                        {
                                            Number = string.Empty,
                                            Fee = "",
                                            LimitedRecline = false,
                                            seatvalue = "-",
                                            Exit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsExit : false,
                                            MonumentType = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.ItemType : string.Empty,
                                            HorizontalSpan = (!monumentSeat.IsNullOrEmpty()) ? Convert.ToString(monumentSeat.HorizontalSpan) : string.Empty,
                                            DoorExit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsDoorExit : false
                                        };
                                    }
                                    if (!tmpSeat.IsNullOrEmpty() && !string.IsNullOrEmpty(tmpSeat.MonumentType))
                                        tmpRow.Seats.Add(tmpSeat);

                                    if ((!tmpSeat.IsNullOrEmpty() && Convert.ToInt32(tmpSeat.HorizontalSpan) > 1))
                                    {
                                        int hrzlSpan = Convert.ToInt32(tmpSeat.HorizontalSpan);
                                        for (int count = 1; count < hrzlSpan; count++)
                                        {
                                            tmpSeat = new MOBSeatB
                                            {
                                                Number = string.Empty,
                                                Fee = "",
                                                LimitedRecline = false,
                                                seatvalue = "-",
                                                Exit = false,
                                            };
                                            tmpRow.Seats.Add(tmpSeat);
                                        }
                                    }
                                }
                            }
                        }

                        if (tmpRow != null)
                        {
                            if (tmpRow.Seats == null || tmpRow.Seats.Count != cabin.Layout.Length)
                            {
                                if (isOaSeatMapSegment)
                                {
                                    string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                                    throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                                }

                                throw new MOBUnitedException(_configuration.GetValue<string>("GenericExceptionMessage"));
                            }
                        }

                        if (row.Number < 1000)
                            tmpCabin.Rows.Add(tmpRow);
                        prevRowNumber = row.VerticalGridNumber;
                    }
                }
                tmpCabin.Configuration = tmpCabin.Configuration.Replace(" ", "-");

                if (isVerticalSeatMapEnabled)
                {
                    List<Definition.SeatCSL30.MonumentRow> monumentsAtCabinEnd = cabin.MonumentRows.Where(x => x.VerticalGridNumber > prevRowNumber).ToList();

                    if (!monumentsAtCabinEnd.IsNull() && monumentsAtCabinEnd.Count > 0)
                    {
                        List<MOBRow> monumentsRowsToAdd = GetMonuments(cabin, monumentsAtCabinEnd[0].VerticalGridNumber, monumentsAtCabinEnd[monumentsAtCabinEnd.Count - 1].VerticalGridNumber, ref monuments);
                        tmpCabin.Rows.AddRange(monumentsRowsToAdd);
                    }
                }
                objMOBSeatMap.Cabins.Add(tmpCabin);
            }

            objMOBSeatMap.SeatLegendId = objMOBSeatMap.IsOaSeatMap ? _configuration.GetValue<string>("SeatMapLegendForOtherAirlines") :
                                                                     await _seatEngine.GetPolarisSeatMapLegendId(seatMapResponse.FlightInfo.DepartureAirport, seatMapResponse.FlightInfo.ArrivalAirport, numberOfCabins, cabinBrandingDescriptions, appId, appVersion, isBERecommendedSeatsAvailable);
            if (isVerticalSeatMapEnabled)
            {
                objMOBSeatMap.CabinLegends = objMOBSeatMap.IsOaSeatMap ?
                                              await GetOACabinLegends(objMOBSeatMap.Cabins, cabinBrandingDescriptions)
                                            : await GetCabinLegends(cabinBrandingDescriptions, objMOBSeatMap.Cabins, bookingCabin, ePlusSubscriberCount, segmentIndex, isELF, isIBE, isBERecommendedSeatsAvailable, hasDAASeat, hasFBBSeat);

                objMOBSeatMap.MonumentLegends = await GetMonumentLegends(monuments);
                objMOBSeatMap.Tiers = await GetTiers(seatMapResponse.Tiers);
            }

            objMOBSeatMapCSL30.Seat = listMOBSeatCSL30 != null ? listMOBSeatCSL30 : null;
            await SaveCSL30SeatMapPersist(sessionId, objMOBSeatMapCSL30);

            return (objMOBSeatMap, countNoOfFreeSeats, countNoOfPricedSeats);
        }

        private InfoWarningMessages BuildUpsellSeatMapMessage(List<MOBMobileCMSContentMessages> seatmapUpsellMessage)
        {
            InfoWarningMessages upsellSeatMapMessage = new InfoWarningMessages();
            if (seatmapUpsellMessage != null)
            {
                upsellSeatMapMessage.HeaderMessage = seatmapUpsellMessage?.FirstOrDefault()?.ContentFull;
                upsellSeatMapMessage.IsExpandByDefault = true;
                upsellSeatMapMessage.IsCollapsable = true;
                upsellSeatMapMessage.IconType = INFOWARNINGMESSAGEICON.INFORMATION.ToString();
                upsellSeatMapMessage.Order = INFOWARNINGMESSAGEORDER.UPSELLSEATMAPMESSAGE.ToString();
            }

            return upsellSeatMapMessage;
        }

        private List<TravelerPricing> GetTravelerPricing(string pcuOfferAmountForthisCabin, double pcuOfferPriceForthisCabin, Tier tier, bool isSeatMFOPEnabled, bool isVerticalSeatMapEnabled, bool isUpsellFlow, List<MOBMobileCMSContentMessages> lstMessages)
        {
            List<TravelerPricing> travelerPricingList = tier?.Pricing?.GroupBy(p => p.TravelerId)
                                                        .Select(travelerPricing => new TravelerPricing
                                                        {
                                                            TravelerId = travelerPricing.Key,
                                                            DisplayFeesWithMiles = GetTravelerFees(travelerPricing.ToList(), pcuOfferAmountForthisCabin, pcuOfferPriceForthisCabin, isSeatMFOPEnabled, isVerticalSeatMapEnabled),
                                                            PriceDifferenceLabel = GetUpsellPriceLabelText(isUpsellFlow, travelerPricing.ToList(), lstMessages)
                                                        }).ToList();

            return travelerPricingList;
        }

      
        private string GetUpsellPriceLabelText(bool isUpsellFlow, List<Pricing> travelerPricing, List<MOBMobileCMSContentMessages> lstMessages)
        {
            if (isUpsellFlow)
            {
                var travelerPrice = travelerPricing?.FirstOrDefault();
                if (!string.IsNullOrEmpty(travelerPrice?.InitialPrice) && travelerPrice?.TotalPrice > 0 && lstMessages?.Count > 0)
                    return lstMessages?.FirstOrDefault()?.ContentShort;
            }

            return string.Empty;
        }

        private string GetTravelerFees(List<Pricing> travelerPricing, string pcuOfferAmountForthisCabin, double pcuOfferPriceForthisCabin, bool isSeatMFOPEnabled, bool isVerticalSeatMapEnabled)
        {
            string displayFeesWithMiles;

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin > 0) // Amount will be there only if the no of travelers are eligible and Appversion and AppId matches
            {
                displayFeesWithMiles = isVerticalSeatMapEnabled ? displayFeesWithMiles = string.Concat("$", pcuOfferPriceForthisCabin.ToString("0.00")) : string.Concat("$", pcuOfferPriceForthisCabin.ToString());
            }
            else
            {
                decimal USDFee = travelerPricing?.FirstOrDefault(t => t?.CurrencyCode == "USD")?.TotalPrice ?? 0;

                displayFeesWithMiles = USDFee != 0 ? string.Concat("$", USDFee) : string.Empty;

                if (isSeatMFOPEnabled)
                {
                    decimal MilesFee = travelerPricing?.FirstOrDefault(t => t?.CurrencyCode == "Miles")?.TotalPrice ?? 0;

                    if (!string.IsNullOrEmpty(displayFeesWithMiles) && MilesFee != 0)
                    {
                        displayFeesWithMiles = string.Concat(displayFeesWithMiles, " or ", GetDisplayMilesFee(MilesFee), " miles");
                    }
                }
            }

            return displayFeesWithMiles;
        }

        private async Task<List<MOBTier>> GetTiers(ICollection<Tier> tiers)
        {
            List<MOBTier> mobTiers = new List<MOBTier>();

            foreach (Tier tier in tiers)
            {
                MOBTier mobTier = new MOBTier();
                mobTier.Id = tier.Id;
                mobTier.Pricing = GetTierPricing(tier.Pricing);
                mobTiers.Add(mobTier);
            }

            return mobTiers;
        }

        private List<MOBPricing> GetTierPricing(ICollection<Pricing> tierPricing)
        {
            List<MOBPricing> MOBPricingList = new List<MOBPricing>();

            foreach (Pricing pricing in tierPricing)
            {
                MOBPricing mobPricing = new MOBPricing();
                mobPricing.TravelerId = pricing.TravelerId;
                mobPricing.TotalPrice = pricing.TotalPrice;
                mobPricing.OriginalPrice = pricing.OriginalPrice;
                mobPricing.CouponCode = pricing.CouponCode;
                mobPricing.Eligibility = pricing.Eligibility;
                mobPricing.CurrencyCode = pricing.CurrencyCode;
                MOBPricingList.Add(mobPricing);
            }

            return MOBPricingList;
        }

        private List<MOBRow> GetMonuments(United.Definition.SeatCSL30.Cabin cabin, int startVal, int endVal, ref List<string> availableMonuments)
        {
            List<MOBRow> monumentRows = new List<MOBRow>();
            if (cabin.IsNullOrEmpty() || cabin.MonumentRows.IsNullOrEmpty()) return monumentRows;
            var selMonumentRows = cabin.MonumentRows.Where(x => x.VerticalGridNumber >= startVal && x.VerticalGridNumber <= endVal);
            foreach (United.Definition.SeatCSL30.MonumentRow monumentRow in selMonumentRows)
            {
                MOBRow tmpRow = null;

                if (monumentRow != null)
                {
                    tmpRow = new MOBRow();
                    tmpRow.Number = string.Empty;
                    tmpRow.Wing = false;

                    for (int i = 1; i <= cabin.ColumnCount; i++)
                    {
                        var monumentSeat = (!monumentRow.IsNullOrEmpty()) ? monumentRow.Monuments.FirstOrDefault(x => x.HorizontalGridNumber == i) : null;
                        MOBSeatB tmpMonumentSeat = new MOBSeatB
                        {
                            Number = string.Empty,
                            Fee = "",
                            LimitedRecline = false,
                            seatvalue = "-",
                            Exit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsExit : false,
                            DoorExit = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.IsDoorExit : false,
                            MonumentType = (!monumentSeat.IsNullOrEmpty()) ? monumentSeat.ItemType : string.Empty,
                            HorizontalSpan = (!monumentSeat.IsNullOrEmpty()) ? Convert.ToString(monumentSeat.HorizontalSpan) : string.Empty
                        };

                        if (!monumentSeat.IsNullOrEmpty())
                        {
                            if (!availableMonuments.Contains(monumentSeat.ItemType))
                            {
                                availableMonuments.Add(monumentSeat.ItemType);
                            }
                        }

                        if (!tmpMonumentSeat.IsNullOrEmpty())
                            tmpRow.Seats.Add(tmpMonumentSeat);
                    }

                    monumentRows.Add(tmpRow);

                }
            }
            return monumentRows;
        }

        private void LoadMonuments(United.Definition.SeatCSL30.Row row, int prevRowNumber, United.Definition.SeatCSL30.Cabin cabin, ref MOBCabin mobCabin)
        {
            int missingCount = 0;
            MOBRow tmpRow = null;
            if (cabin.IsNullOrEmpty() || cabin.MonumentRows.IsNullOrEmpty()) return;
            if (prevRowNumber == 0) prevRowNumber = row.VerticalGridNumber + 1;
            missingCount = row.VerticalGridNumber - prevRowNumber;
            if (missingCount > 1)
            {
                prevRowNumber++;
                for (int missingNumber = prevRowNumber; missingNumber < row.VerticalGridNumber; missingNumber++)
                {
                    var monumentMissingRow = cabin.MonumentRows.FirstOrDefault(x => x.VerticalGridNumber == missingNumber);
                    if (monumentMissingRow != null)
                    {
                        tmpRow = new MOBRow();
                        tmpRow.Number = string.Empty;
                        tmpRow.Wing = false;

                        for (int i = 1; i <= cabin.ColumnCount; i++)
                        {
                            var monumentseat = (!monumentMissingRow.IsNullOrEmpty()) ? monumentMissingRow.Monuments.FirstOrDefault(x => x.HorizontalGridNumber == i) : null;
                            MOBSeatB tmpMonumentSeat = new MOBSeatB
                            {
                                Number = string.Empty,
                                Fee = "",
                                LimitedRecline = false,
                                seatvalue = "-",
                                Exit = (!monumentseat.IsNullOrEmpty()) ? monumentseat.IsExit : false,
                                DoorExit = (!monumentseat.IsNullOrEmpty()) ? monumentseat.IsDoorExit : false,
                                MonumentType = (!monumentseat.IsNullOrEmpty()) ? monumentseat.ItemType : string.Empty,
                                HorizontalSpan = (!monumentseat.IsNullOrEmpty()) ? Convert.ToString(monumentseat.HorizontalSpan) : string.Empty
                            };
                            tmpRow.Seats.Add(tmpMonumentSeat);
                        }
                        if (row.Number < 1000)
                            mobCabin.Rows.Add(tmpRow);
                    }

                }

            }
        }

        private bool isLimitedRecline(string displaySeatCategory)
        {
            if (!string.IsNullOrEmpty(displaySeatCategory))
            {
                var limitedReclineCategory = _configuration.GetValue<string>("SelectSeatsLimitedReclineForCSL30").Split('|');
                if (!limitedReclineCategory.IsNullOrEmpty() && limitedReclineCategory.Any())
                {
                    return limitedReclineCategory.Any(x => !x.IsNullOrEmpty() && x.Trim().Equals(displaySeatCategory, StringComparison.OrdinalIgnoreCase));
                }
            }
            return false;
        }

        public string GetSeatPositionAccessFromCSL30SeatMap(string seatType, ref bool hasDAASeat, ref bool hasFBBSeat)
        {
            string seatPositionProgram = string.Empty;
            if (seatType.Equals(SeatType.FBLEFT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBL);
            }
            else if (seatType.Equals(SeatType.FBRIGHT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBR);
            }
            else if (seatType.Equals(SeatType.FBFRONT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBF);
            }
            else if (seatType.Equals(SeatType.FBBACK.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBB);
            }
            else if (seatType.Equals(SeatType.DAAFRONTL.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFL);
            }
            else if (seatType.Equals(SeatType.DAAFRONTR.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFR);
            }
            else if (seatType.Equals(SeatType.DAALEFT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAL);
            }
            else if (seatType.Equals(SeatType.DAARIGHT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAR);
            }
            else if (seatType.Equals(SeatType.DAAFRONTRM.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFRM);
            }
            return seatPositionProgram;
        }

        public string GetSeatValueFromCSL30SeatMap(Definition.SeatCSL30.Seat seat, bool disableEplus, bool disableSeats, MOBApplication application, bool isOaSeatMapSegment, bool isOaPremiumEconomyCabin, string pcuOfferAmountForthisCabin, bool cogStop)
        {
            string seatValue = string.Empty;
            if (seat != null && !string.IsNullOrEmpty(seat.SeatType))
            {
                if (seat.IsInoperative || seat.IsPermanentBlocked || seat.IsBlocked)
                {
                    seatValue = "X";
                }
                else if (seat.SeatType.Equals(SeatType.BLUE.ToString(), StringComparison.OrdinalIgnoreCase) || isOaPremiumEconomyCabin)
                {
                    seatValue = seat.IsAvailable && (disableEplus || cogStop) ? "X" : seat.IsAvailable ? "P" : "X";
                }
                else if (seat.SeatType.Equals(SeatType.PREF.ToString(), StringComparison.OrdinalIgnoreCase))//TODO version check
                {
                    seatValue = seat.IsAvailable ? "PZ" : "X";
                }
                else if (seat.SeatType.Equals(SeatType.STANDARD.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    seatValue = seat.IsAvailable ? "O" : "X";
                }
                else
                {
                    seatValue = seat.IsAvailable ? "O" : "X";
                }
            }
            return string.IsNullOrEmpty(seatValue) || (!string.IsNullOrEmpty(seatValue) && disableSeats && string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin)) ? "X" : seatValue;
        }

        private List<ServicesAndFees> GetServicesAndFees(Definition.SeatCSL30.Seat seat, string pcuOfferAmountForthisCabin, double pcuOfferPriceForthisCabin, string program, Tier tier, bool isVerticalSeatMapEnabled, bool isSeatMFOPEnabled = false)
        {
            decimal milesAmount = 0;
            List<ServicesAndFees> servicesAndFees = new List<ServicesAndFees>();

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin > 0) // Amount will be there only if the no of travelers are eligible and Appversion and AppId matches
            {
                ServicesAndFees serviceAndFee = new ServicesAndFees();
                serviceAndFee.Available = seat.IsAvailable;
                serviceAndFee.SeatFeature = seat.DisplaySeatCategory;
                serviceAndFee.SeatNumber = seat.Number;
                serviceAndFee.Program = program; // aSeat.Program will be empty and to assign program code for higher cabins to upgrade seat
                serviceAndFee.TotalFee = Convert.ToDecimal(pcuOfferPriceForthisCabin);
                serviceAndFee.Currency = "USD";


                if (isSeatMFOPEnabled)
                {
                    serviceAndFee.DisplayFeesWithMiles = serviceAndFee.TotalFee != 0 ?
                                                         isVerticalSeatMapEnabled ?
                                                             string.Concat("$", serviceAndFee.TotalFee.ToString("0.00")) : 
                                                             string.Concat("$", serviceAndFee.TotalFee.ToString())
                                                         : string.Empty;
                }

                servicesAndFees.Add(serviceAndFee);
            }
            else if (seat.ServicesAndFees != null && seat.ServicesAndFees.Any())
            {
                foreach (SeatService seatService in seat.ServicesAndFees)
                {
                    if (!string.IsNullOrEmpty(seatService.Currency) && seatService.Currency.ToUpper() == "MILES")
                    {
                        milesAmount = seatService.TotalFee;
                        continue;
                    }

                    ServicesAndFees serviceAndFee = new ServicesAndFees();
                    serviceAndFee.AgentDutyCode = seatService.AgentDutyCode ?? string.Empty;
                    serviceAndFee.AgentId = seatService.AgentId ?? string.Empty;
                    serviceAndFee.AgentTripleA = seatService.AgentTripleA ?? string.Empty;
                    serviceAndFee.Available = seat.IsAvailable;
                    serviceAndFee.BaseFee = seatService.BaseFee;
                    serviceAndFee.Currency = seatService.Currency;
                    serviceAndFee.EliteStatus = seatService.EliteStatus.ToString();
                    serviceAndFee.FeeWaiveType = seatService.FeeWaiveType ?? string.Empty;
                    serviceAndFee.IsDefault = seatService.IsDefault;
                    serviceAndFee.OverrideReason = seatService.OverrideReason ?? string.Empty;
                    serviceAndFee.Program = seat.EDoc;
                    serviceAndFee.SeatFeature = seat.DisplaySeatCategory?.ToUpper();
                    serviceAndFee.SeatNumber = seat.Number;
                    serviceAndFee.Tax = seatService.Tax;
                    serviceAndFee.TotalFee = seatService.TotalFee;
                    serviceAndFee.OriginalPrice = serviceAndFee.OriginalPrice;
                    serviceAndFee.CouponCode = serviceAndFee.CouponCode;
                    servicesAndFees.Add(serviceAndFee);
                }
            }

            if (isSeatMFOPEnabled)
            {
                if (servicesAndFees != null && servicesAndFees.Count > 0)
                {
                    servicesAndFees[0].MilesFee = Convert.ToInt32(milesAmount);
                    servicesAndFees[0].DisplayMilesFee = GetDisplayMilesFee(milesAmount);

                    string displayFeesWithMiles = servicesAndFees[0].TotalFee != 0 ?
                                                                isVerticalSeatMapEnabled ?
                                                                    string.Concat("$", servicesAndFees[0].TotalFee.ToString("0.00"))
                                                                    : string.Concat("$", servicesAndFees[0].TotalFee.ToString())
                                                                : string.Empty;

                    if (!string.IsNullOrEmpty(displayFeesWithMiles)
                        && servicesAndFees[0].MilesFee != 0)
                    {
                        displayFeesWithMiles = string.Concat(displayFeesWithMiles, " or ", servicesAndFees[0].DisplayMilesFee, " miles");
                    }

                    servicesAndFees[0].DisplayFeesWithMiles = displayFeesWithMiles;
                }
            }

            return servicesAndFees;
        }

        private string GetDisplaySeatFeature(bool isOaSeatMapSegment, string seatValue, string pcuOfferAmountForthisCabin, string pcuCabinName, bool isEconomyCabinWithAdvanceSeats, string cabinName, bool isEplus, bool isFamilySeat, string oldSellableSeatCategory)
        {
            if (isOaSeatMapSegment)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && seatValue.Equals("O") && !string.IsNullOrEmpty(pcuCabinName))
                return pcuCabinName;

            if (cabinName.Equals("United Premium Plus", StringComparison.OrdinalIgnoreCase))
                return "United Premium Plus";

            if (seatValue.Equals("P") || isEplus)
                return "Economy Plus";

            if (seatValue.Equals("PZ"))
                return "Preferred Seat";

            if (isFamilySeat && !string.IsNullOrEmpty(oldSellableSeatCategory))
                return oldSellableSeatCategory?.ToUpper().Equals("PREFERRED ZONE") == true ? "Preferred Seat" : oldSellableSeatCategory;

            if (isEconomyCabinWithAdvanceSeats)
                return "Economy";

            return cabinName;
        }

        private string GetDisplaySeatFeatureWithSymbols(bool isOaSeatMapSegment, string seatValue, string pcuOfferAmountForthisCabin, string pcuCabinName, bool isEconomyCabinWithAdvanceSeats, string cabinName, bool isEplus, bool isFamilySeat, string oldSellableSeatCategory)
        {
            if (isOaSeatMapSegment)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && seatValue.Equals("O") && !string.IsNullOrEmpty(pcuCabinName))
            {
                var name = pcuCabinName.Contains("®") ? pcuCabinName : pcuCabinName + "®";
                if (name.Contains("Polaris", StringComparison.OrdinalIgnoreCase)) name = name.Replace("®", "").Replace("Polaris", "Polaris®");
                if (name.Contains("Premium Plus", StringComparison.OrdinalIgnoreCase)) name = name.Replace("®", "").Replace("Premium Plus", "Premium Plus℠");
                return name;
            }

            if (cabinName.Contains("Premium Plus", StringComparison.OrdinalIgnoreCase))
                return "United Premium Plus℠";

            if (seatValue.Equals("P") || isEplus)
                return "Economy Plus®";

            if (seatValue.Equals("PZ"))
                return "Preferred Seat";

            if (isFamilySeat && !string.IsNullOrEmpty(oldSellableSeatCategory) && oldSellableSeatCategory?.ToUpper().Equals("PREFERRED ZONE") == true)
                return "Preferred Seat";

            if (isEconomyCabinWithAdvanceSeats)
                return "Economy®";

            return cabinName.Contains("®") ? cabinName : cabinName + "®";
        }

        private List<string> GetSeatFeatureList(string seatValue, bool supressLMX, bool limitedRecline, bool isEplus, bool isEconomyCabinWithAdvanceSeats, string cabinName, bool isCouponApplied, bool isExit, bool isFamilySeat, string oldSellableSeatCategory, bool hasNoUnderSeatStorageAndBulkHead = false)
        {
            List<string> seatFeatures = new List<string>();
            bool enableLimitReclineAllProducts = _configuration.GetValue<bool>("EnableLimitedReclineAllProducts");

            var isPreferred = false;
            var isEconomy = cabinName.Equals("United Economy", StringComparison.OrdinalIgnoreCase);
            var isPremium = cabinName.Equals("United Premium Plus", StringComparison.OrdinalIgnoreCase);
            var isFirst = cabinName.Equals("United First", StringComparison.OrdinalIgnoreCase);
            var isBusiness = cabinName.Equals("United Business", StringComparison.OrdinalIgnoreCase);
            var isPolaris = cabinName.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase);

            if (seatValue.Equals("P") || isEplus)
            {
                seatFeatures.Add("Extra legroom");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                    {
                        if (isExit)
                            seatFeatures.Add(_configuration.GetValue<string>("ExitNoOrLimitedReclineMessage"));
                        else
                            seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                    }
                }
                if (!supressLMX)
                    seatFeatures.Add("Eligible for PQD");
            }
            else if (seatValue.Equals("PZ") || (isFamilySeat && !string.IsNullOrEmpty(oldSellableSeatCategory) && oldSellableSeatCategory.ToUpper().Equals("PREFERRED ZONE")))
            {
                isPreferred = true;
                seatFeatures.Add("Standard legroom");
                seatFeatures.Add("Favorable location in Economy");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                }
            }
            else if (isFamilySeat && !string.IsNullOrEmpty(oldSellableSeatCategory) && oldSellableSeatCategory.ToUpper().Equals("ECONOMY"))
            {
                seatFeatures.Add("Standard legroom");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                }
            }
            else if (isEconomyCabinWithAdvanceSeats)
            {
                seatFeatures.Add("Advance seat assignment");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                }
            }
            else if (isEconomy && !isEplus && !isPreferred && !isEconomyCabinWithAdvanceSeats)
            {
                seatFeatures.Add("Standard legroom");
            }
            else if (isPremium)
            {
                seatFeatures.Add("Spacious seating with more width and legroom");
            }
            else if (isFirst || isBusiness)
            {
                seatFeatures.Add("Extra wide seats with added legroom");
            }
            else if (isPolaris)
            {
                seatFeatures.Add("Lie-flat seating");
            }

            if (enableLimitReclineAllProducts)
            {
                if (limitedRecline)
                {
                    if (isExit)
                        seatFeatures.Add(_configuration.GetValue<string>("ExitNoOrLimitedReclineMessage"));
                    else
                        seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                }
            }
            if (hasNoUnderSeatStorageAndBulkHead)
            {
                seatFeatures.Add(_configuration.GetValue<string>("BulkSeatNoUnderSeatStorageText"));
            }
            if (isCouponApplied)
            {
                seatFeatures.Add("Discounted price");
            }

            return seatFeatures;
        }

        private string GetDisplayMilesFee(decimal miles)
        {
            if (miles >= 100000000)
                return (miles / 1000000).ToString("#,0M");

            if (miles >= 10000000)
                return (miles / 1000000).ToString("0.#") + "M";

            if (miles >= 100000)
                return (miles / 1000).ToString("#,0k");

            if (miles >= 1000)
                return (miles / 1000).ToString("0.#") + "k";

            return miles.ToString("#,0");
        }

        private async Task SaveCSL30SeatMapPersist(string sessionId, United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 seatMap)
        {
            List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30> csl30SeatMaps = new List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>();
            csl30SeatMaps.Add(seatMap);
            var persistedCSL30SeatMaps = new List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>();
            persistedCSL30SeatMaps = await _sessionHelperService.GetSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(sessionId, new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName, new List<string> { sessionId, new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }).ConfigureAwait(false); //change session

            // to Append current CSL30 seatmap & maintain all segments seatmaps without duplicity
            if (!persistedCSL30SeatMaps.IsNullOrEmpty())
            {
                foreach (United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 pCSL30SeatMap in persistedCSL30SeatMaps)
                {
                    if (!(pCSL30SeatMap.DepartureCode.Equals(seatMap.DepartureCode, StringComparison.OrdinalIgnoreCase) && pCSL30SeatMap.ArrivalCode.Equals(seatMap.ArrivalCode, StringComparison.OrdinalIgnoreCase) && pCSL30SeatMap.FlightNumber == seatMap.FlightNumber && pCSL30SeatMap.FlightDateTime == seatMap.FlightDateTime))
                    {
                        csl30SeatMaps.Add(pCSL30SeatMap);
                    }
                }
                await _sessionHelperService.SaveSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(csl30SeatMaps, sessionId, new List<string> { sessionId, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName).ConfigureAwait(false);//(new United.Mobile.Model.ShopSeats.SeatMapCSL30()).GetType().FullName,
            }
            else
            {
                await _sessionHelperService.SaveSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(csl30SeatMaps, sessionId, new List<string> { sessionId, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName).ConfigureAwait(false);
            }
        }

        private async Task<string> GetSelectSeatMapResponse(string url, string sessionId, string cslRequest, string token, string channelId, string channelName, bool isOANoSeatMapAvailableNewMessageEnabled, Session session, string path = "", bool isOperatedByUA = true, int appID = -1, string appVersion = "")
        {
            try
            {
                return await _seatMapCSL30service.GetSeatMapDeatils(token, sessionId, cslRequest, channelId, channelName, path).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string exReader = ExceptionMessages(ex);

                string seatMapUnavailable = string.Empty;
                if (!string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapUnavailable-MinorDescription")))
                {
                    seatMapUnavailable = _configuration.GetValue<string>("SeatMapUnavailable-MinorDescription");
                    string[] seatMapUnavailableMinorDescription = seatMapUnavailable.Split('|');

                    if (!string.IsNullOrEmpty(exReader))
                    {
                        foreach (string minorDescription in seatMapUnavailableMinorDescription)
                        {
                            if (_shoppingUtility.EnableOAMessageUpdate(appID, appVersion) && !isOperatedByUA)
                            {
                                if (exReader.Contains(minorDescription))
                                {
                                    string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage"));
                                    throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                                }
                                else
                                {
                                    string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage"));
                                    throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                                }
                            }
                            else if (exReader.Contains(minorDescription))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("OASeatMapUnavailableMessage"));
                            }
                        }
                    }
                }
                throw new Exception(exReader);
            }
        }

        private string ExceptionMessages(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return ex.Message + " | " + ExceptionMessages(ex.InnerException);
        }


        public async Task<List<MOBSeatMap>> GetCSL30SeatMapForRecordLocatorWithLastName(string sessionId, string recordLocator, int segmentIndex, string languageCode, string bookingCabin, string lastName, bool cogStop, string origin, string destination, int applicationId, string appVersion, bool isELF, bool isIBE, int noOfTravelersWithNoSeat1, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, List<TripSegment> tripSegments, string operatingCarrierCode, string deviceId, List<MOBBKTraveler> BookingTravelerInfo, string flow, bool isVerticalSeatMapEnabled, bool isOANoSeatMapAvailableNewMessageEnabled, int ePlusSubscriberCount = 0, MOBTravelerSignInData travelerSignInData = null, bool isSeatMapSignInUserDataChangeEnabled = false, bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false, bool isSeatFocusEnabled = false, List<MOBItem> catalog = null, string cartId = null)
        {
            if (!ConfigUtility.EnableAirCanada(applicationId, appVersion) && operatingCarrierCode != null
                    && operatingCarrierCode.Trim().ToUpper() == "AC")
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines") != null ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines").ToString() : string.Empty);
            }
            if (ConfigUtility.EnableLufthansa(operatingCarrierCode))
            {
                if (!ConfigUtility.EnableLufthansaForHigherVersion(operatingCarrierCode, applicationId, appVersion))
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                }
            }

            bool isDeepLink = false;
            if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && !string.IsNullOrEmpty(operatingCarrierCode))
            {
                isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog);
            }

            string[] channelInfo = _configuration.GetValue<string>("CSL30MMRChannelInfo").Split('|');
            var request = BuildSeatMapRequest(flow, languageCode, channelInfo[0], channelInfo[1], recordLocator, false, isIBE, cartId); //BuildSeatMapRequest(flow, languageCode, tripSegments, segmentIndex, channelInfo[0], channelInfo[1], recordLocator);

            bool isSeatMapTravelerDetailsEnabled = await _featureSettings.GetFeatureSettingValue("EnableTravelerInfoinSeatmapRequest").ConfigureAwait(false);

            if (isSeatMapTravelerDetailsEnabled)
            {
                request.Travelers = BuildTravelersDetails(BookingTravelerInfo, applicationId, appVersion);
            }

            request.FlightSegments = BuildFlightSegmentsForManageRes(tripSegments, segmentIndex, origin, destination, isSeatFocusEnabled);
            request.BundleCode = tripSegments?.FirstOrDefault(t => t?.OriginalSegmentNumber == segmentIndex)?.BundleProductCode;
            if (_configuration.GetValue<bool>("EnablePBE"))
            {
                string productCode = isIBE ? tripSegments?.FirstOrDefault()?.ProductCode : string.Empty;
                if (isIBE && !string.IsNullOrEmpty(productCode))
                {
                    request.ProductCode = productCode;
                }
            }

            if (isSeatMapSignInUserDataChangeEnabled)
            {
                request.TravelerSignInData = new TravelerSignInData()
                {
                    SignedInMPLevel = GetMPLevel(travelerSignInData?.SignedInEliteStatusCode),
                    SignedInRPC = travelerSignInData?.SignedInRPC
                };
            }

            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            string url = string.Empty;
            string cslRequest = DataContextJsonSerializer.Serialize<United.Definition.SeatCSL30.SeatMapRequest>(request);


            var cslstrResponse = await GetSelectSeatMapResponse(url, sessionId, cslRequest, session.Token, channelInfo[0], channelInfo[1], isOANoSeatMapAvailableNewMessageEnabled, session);

            _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {ApplicationId} {ApplicationVersion} {DeviceId} {CSLResponse} and {SessionId}", applicationId, appVersion, deviceId, cslstrResponse, sessionId);

            List<MOBSeatMap> seatMaps = null;
            United.Definition.SeatCSL30.SeatMap response = new United.Definition.SeatCSL30.SeatMap();
            if (!string.IsNullOrEmpty(cslstrResponse))
            {
                response = JsonConvert.DeserializeObject<United.Definition.SeatCSL30.SeatMap>(cslstrResponse);
            }

            if (response != null && response.FlightInfo != null && response.Cabins != null && response.Cabins.Count > 0 && response.Errors.IsNullOrEmpty())
            {
                seatMaps = new List<MOBSeatMap>();

                MOBSeatMap aSeatMap = await BuildSeatMapCSL30(response, response.Travelers.Count, bookingCabin, cogStop, sessionId, isELF, isIBE, noOfTravelersWithNoSeat1, noOfFreeEplusEligibleRemaining, isOaSeatMapSegment, segmentIndex, flow, isVerticalSeatMapEnabled, session, isOANoSeatMapAvailableNewMessageEnabled, ePlusSubscriberCount, "", applicationId, appVersion, isOneofTheSegmentSeatMapShownForMultiTripPNRMRes, operatingCarrierCode, catalog);
                seatMaps.Add(aSeatMap);
            }
            else if (isOaSeatMapSegment || (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && isDeepLink))
            {
                // Not throwing error for OA. Sent message in object instead.
                if (!_shoppingUtility.EnableOAMsgUpdateFixViewRes(applicationId, appVersion))
                {
                    if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion))
                    {
                        if (!isDeepLink)
                        {
                            string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage"));
                            throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                        }
                    }
                    else
                    {
                        if (ConfigUtility.EnableAirCanada(applicationId, appVersion) && operatingCarrierCode != null && operatingCarrierCode.Trim().ToUpper() == "AC")
                        {
                            if (!ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog))
                            {
                                if (_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation") != null)
                                {
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation").ToString());
                                }
                                string seatMapError = _configuration.GetValue<string>("AirCanadaSeatmapError");
                                if (response != null && response.Errors != null)
                                {
                                    if (response.Errors.Any(x => !x.IsNullOrEmpty() && !x.Message.IsNullOrEmpty() && x.Message.Equals(seatMapError, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        throw new MOBUnitedException(_configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation") != null ? _configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation").ToString() : string.Empty);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog))
                            {
                                string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? 
                                    await GetOANoSeatMapAvailableNewMessage(session) :
                                    (_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation") != null ?
                                        _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation") 
                                        : string.Empty);
                                
                                throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                            }
                        }
                    }
                }
            }
            else
            {
                string errorMessage = string.Empty;
                if (response != null && response.Errors != null && response.Errors.Any())
                {
                    foreach (var error in response.Errors)
                    {
                        errorMessage = errorMessage + " " + error.Message;
                    }
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {MOBUnitedException} and {SessionId}", applicationId, appVersion, string.Empty, errorMessage, sessionId);

                    if (errorMessage.Contains("BUS"))
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SelectSeats_BusServiceError"));
                    }
                }
            }

            _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {Client-SeatMapResponse} and {SessionId}", seatMaps, sessionId);
            return seatMaps;
        }

        private string GetMPLevel(string signedInEliteStatusCode)
        {
            string signedInMPLevel = null;

            switch (signedInEliteStatusCode)
            {
                case "0":
                    signedInMPLevel = "Member";
                    break;
                case "1":
                    signedInMPLevel = "Silver";
                    break;
                case "2":
                    signedInMPLevel = "Gold";
                    break;
                case "3":
                    signedInMPLevel = "Platinum";
                    break;
                case "4":
                    signedInMPLevel = "1K";
                    break;
                case "5":
                    signedInMPLevel = "GlobalServices";
                    break;
                default:
                    break;
            }

            return signedInMPLevel;
        }

        private ICollection<Traveler2> BuildTravelersDetails(List<MOBBKTraveler> MOBBKtravelers, int applicationId, string appVersion)
        {
            List<Traveler2> travelers = new List<Traveler2>();

            int i = 1;

            foreach (MOBBKTraveler MOBBKTraveler in MOBBKtravelers)
            {
                Traveler2 traveler = new Traveler2();
                traveler.Id = i;
                traveler.TravelerIndex = MOBBKTraveler.SHARESPosition;
                traveler.FirstName = MOBBKTraveler.Person.GivenName;
                traveler.LastName = MOBBKTraveler.Person.Surname;
                traveler.DateOfBirth = MOBBKTraveler.Person.DateOfBirth;
                traveler.Type = MOBBKTraveler.TravelerTypeCode;
                traveler.PassengerTypeCode = MOBBKTraveler.TravelerTypeCode;
                traveler.LoyaltyProfiles = new List<LoyaltyProfile>();

                if (MOBBKTraveler.LoyaltyProgramProfile != null)
                {
                    LoyaltyProfile loyaltyProfile = new LoyaltyProfile();
                    loyaltyProfile.ProgramId = MOBBKTraveler.LoyaltyProgramProfile.CarrierCode;
                    loyaltyProfile.MemberShipId = MOBBKTraveler.LoyaltyProgramProfile.MemberId;

                    traveler.LoyaltyProfiles.Add(loyaltyProfile);
                }

                travelers.Add(traveler);
                i++;
            }

            return travelers;
        }


        private Collection<United.Definition.SeatCSL30.FlightSegments> BuildFlightSegmentsForManageRes(List<TripSegment> tripSegments, int segmentIndex, string origin, string destination, bool isSeatFocusEnabled)
        {
            var flightSegments = new Collection<United.Definition.SeatCSL30.FlightSegments>();
            var segmentsCopy = tripSegments.Clone();
            if (segmentIndex > 0)
            {
                if (isSeatFocusEnabled)
                {
                    segmentsCopy = tripSegments.FindAll(t => !t.IsNullOrEmpty() && t.OriginalSegmentNumber == segmentIndex);
                }
                else
                {
                    segmentsCopy = tripSegments.FindAll(t => !t.IsNullOrEmpty() && t.SegmentIndex == segmentIndex);
                }
            }

            if (!segmentsCopy.IsNullOrEmpty())
            {
                int id = 1;
                foreach (var segment in segmentsCopy)
                {
                    if (!segment.IsNullOrEmpty() && !segment.Arrival.IsNullOrEmpty() && !segment.Departure.IsNullOrEmpty())
                    {
                        if (segment.COGStop)
                        {
                            if (segment.Departure.Code == origin && segment.Arrival.Code == destination)
                            {
                                var flightSegment = BuildFlightSegmentsForSeatMap(segment.Departure.Code, segment.Arrival.Code, segment.IsCheckInWindow, segment.ServiceClass, segment.ScheduledDepartureDate, string.Empty, segment.FareBasisCode, Convert.ToInt32(segment.FlightNumber), segment.MarketingCarrier, segment.OperatingCarrier, segment.SegmentIndex, "true", id);
                                id++;
                                flightSegments.Add(flightSegment);
                            }
                        }
                        else
                        {
                            var flightSegment = BuildFlightSegmentsForSeatMap(segment.Departure.Code, segment.Arrival.Code, segment.IsCheckInWindow, segment.ServiceClass, segment.ScheduledDepartureDate, string.Empty, segment.FareBasisCode, Convert.ToInt32(segment.FlightNumber), segment.MarketingCarrier, segment.OperatingCarrier, segment.SegmentIndex, "true", id);
                            id++;
                            flightSegments.Add(flightSegment);
                        }
                    }
                }
            }

            return flightSegments;
        }
        private United.Definition.SeatCSL30.FlightSegments BuildFlightSegmentsForSeatMap(string origin, string destination, bool isCheckinEligible, string classOfService, string depatureDate, string arrivalDate, string fareBasisCode, int flightNumber, string marketingCarrier, string operatingCarrier, int segmentNumber, string pricing, int id)
        {
            var flightSegment = new United.Definition.SeatCSL30.FlightSegments();
            flightSegment.ArrivalAirport = new Definition.SeatCSL30.Airport { IataCode = destination }; ;
            flightSegment.CheckInSegment = isCheckinEligible;
            flightSegment.ClassOfService = classOfService;
            flightSegment.DepartureAirport = new Definition.SeatCSL30.Airport { IataCode = origin };
            flightSegment.DepartureDateTime = depatureDate;
            if (!string.IsNullOrEmpty(arrivalDate))
            {
                flightSegment.ArrivalDateTime = arrivalDate;
            }
            flightSegment.FarebasisCode = fareBasisCode;
            flightSegment.FlightNumber = flightNumber;
            flightSegment.Id = id;
            flightSegment.OperatingFlightNumber = flightNumber;
            flightSegment.OperatingAirlineCode = operatingCarrier;
            flightSegment.MarketingAirlineCode = marketingCarrier;
            flightSegment.SegmentNumber = segmentNumber;
            flightSegment.Pricing = pricing;
            return flightSegment;
        }
        public async Task<MOBSeatMap> BuildSeatMapCSL30(United.Definition.SeatCSL30.SeatMap seatMapResponse, int numberOfTravelers, string bookingCabin, bool cogStop, string sessionId, bool isELF, bool isIBE, int noOfTravelersWithNoSeat, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, int segmentIndex, string flow, bool isVerticalSeatMapEnabled, Session session, bool isOANoSeatMapAvailableNewMessageEnabled,  int ePlusSubscriberCount = 0, string token = "", int appId = -1, string appVersion = "", bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false, string operatingCarrierCode = "", List<MOBItem> catalog = null)
        {
            int countNoOfFreeSeats = 0;
            int countNoOfPricedSeats = 0;
            MOBSeatMap objMOBSeatMap = new MOBSeatMap();


            bool isEnablePcuDeepLinkInSeatMap = ConfigUtility.EnablePcuDeepLinkInSeatMap(appId, appVersion);
            bool isEnablePCUSeatMapPurchaseManageRes = await _seatEngine.IsEnablePCUSeatMapPurchaseManageRes(appId, appVersion, numberOfTravelers, catalog);
            bool isMFOPEnabled = ConfigUtility.IsMFOPCatalogEnabled(catalog);

            bool isBEFamilySeatingIconEnabled = await _featureSettings.GetFeatureSettingValue("EnableFamilySeatingIconInMR").ConfigureAwait(false)
                                             && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableBEFamilySeatingIconAppVersion", "iOSEnableBEFamilySeatingIconAppVersion", "", "", true, _configuration);

            bool isBERecommendedSeatsAvailable = isBEFamilySeatingIconEnabled
                                                    && (isELF || isIBE)
                                                    && seatMapResponse.TransactionIdentifiers != null
                                                    && seatMapResponse.TransactionIdentifiers.IsAdjacentPreferredSeatsModified
                                                    && !string.IsNullOrEmpty(seatMapResponse.TransactionIdentifiers.AdjacentSeatsList);

            var tupleResponse = await GetSeatMapCSL(seatMapResponse, sessionId, isELF, isIBE, isOaSeatMapSegment, segmentIndex, flow, appId, appVersion, isEnablePcuDeepLinkInSeatMap, isEnablePCUSeatMapPurchaseManageRes, countNoOfFreeSeats, countNoOfPricedSeats, bookingCabin, cogStop, isMFOPEnabled, isVerticalSeatMapEnabled, ePlusSubscriberCount, isBERecommendedSeatsAvailable, session, isOANoSeatMapAvailableNewMessageEnabled);

            objMOBSeatMap = tupleResponse.Item1;
            countNoOfFreeSeats = tupleResponse.countNoOfFreeSeats;
            countNoOfPricedSeats = tupleResponse.countNoOfPricedSeats;

            bool isDeepLinkSupportedAirline = _configuration.GetValue<bool>("EnableRedesignForInterlineDeepLink") && ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode) && ConfigUtility.CheckClientCatalogForEnablingFeature("InterlineDeepLinkRedesignClientCatalog", catalog);
            if (_shoppingUtility.EnableOAMessageUpdate(appId, appVersion))
            {
                if ((_shoppingUtility.EnableOAMsgUpdateFixViewRes(appId, appVersion) && objMOBSeatMap.IsOaSeatMap && (countNoOfFreeSeats == 0 || countNoOfFreeSeats <= numberOfTravelers)))
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = true;
                }
                // Not throwing error for OA. Sent message in object instead. 
                if (!_shoppingUtility.EnableOAMsgUpdateFixViewRes(appId, appVersion) && objMOBSeatMap.IsOaSeatMap && (countNoOfFreeSeats == 0 || countNoOfFreeSeats <= numberOfTravelers))
                {
                    string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage"));
                    throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                }
            }
            else
            {
                if (objMOBSeatMap.IsOaSeatMap && countNoOfFreeSeats == 0)
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = isDeepLinkSupportedAirline ? true : false;
                    if (!isDeepLinkSupportedAirline)
                    {
                        string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
                        throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                    }
                }

                if (Convert.ToBoolean(_configuration.GetValue<string>("checkForPAXCount")) && objMOBSeatMap.IsOaSeatMap)
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = isDeepLinkSupportedAirline ? true : false;
                    if (countNoOfFreeSeats <= numberOfTravelers)
                    {
                        string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessage(session) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
                        throw new MOBUnitedException(OANoSeatMapAvailableMessage);
                    }
                }
            }

            bool recommendedSeatsAvailable = ConfigUtility.IsRecommendedSeatingSupportedVersion(appId, appVersion)
                                            && seatMapResponse.TransactionIdentifiers != null
                                                && seatMapResponse.TransactionIdentifiers.IsAdjacentPreferredSeatsModified
                                                && !string.IsNullOrEmpty(seatMapResponse.TransactionIdentifiers.AdjacentSeatsList);

            if (recommendedSeatsAvailable)
            {
                objMOBSeatMap.IsAdjacentPreferredSeatsModified = seatMapResponse.TransactionIdentifiers.IsAdjacentPreferredSeatsModified;
                objMOBSeatMap.NumberOfPreferredSeatsOpened = seatMapResponse.TransactionIdentifiers.NumberOfPreferredSeatsOpened;
                objMOBSeatMap.AdjacentSeatsList = seatMapResponse.TransactionIdentifiers.AdjacentSeatsList;
            }

            if (_configuration.GetValue<bool>("EnableSocialDistanceMessagingForSeatMap") && !isOneofTheSegmentSeatMapShownForMultiTripPNRMRes)
            {
                objMOBSeatMap.ShowInfoMessageOnSeatMap = _configuration.GetValue<string>("SocialDistanceSeatDisplayMessageDetailBody") + _configuration.GetValue<string>("SocialDistanceSeatMapMessagePopup");
            }
            else if (recommendedSeatsAvailable && !isELF && !isIBE)
            {
                objMOBSeatMap.ShowInfoMessageOnSeatMap = _configuration.GetValue<string>("FamilySeatingMessageBody") + _configuration.GetValue<string>("FamilySeatingMessagePopup");
                objMOBSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("FamilySeatingMessageTitle");
            }
            else
            {
                if (isBERecommendedSeatsAvailable)
                {
                    objMOBSeatMap.ShowInfoMessageOnSeatMap = _configuration.GetValue<string>("NewBEFamilySeatingMessageBody") + _configuration.GetValue<string>("FamilySeatingMessagePopup");
                    objMOBSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("FamilySeatingMessageTitle");
                }
                else
                {
                    objMOBSeatMap.ShowInfoMessageOnSeatMap = objMOBSeatMap.IsOaSeatMap ?
                        _shoppingUtility.EnableOAMessageUpdate(appId, appVersion) ?
                            _configuration.GetValue<string>("SeatMapMessageForEligibleOA") :
                                                    _configuration.GetValue<string>("ShowFreeSeatsMessageForOtherAilines") :
                                                   await _seatEngine.ShowNoFreeSeatsAvailableMessage(noOfTravelersWithNoSeat, noOfFreeEplusEligibleRemaining, countNoOfFreeSeats, countNoOfPricedSeats, (isELF || isIBE));
                    if (_configuration.GetValue<bool>("DisableFreeSeatMessageChanges") == false && objMOBSeatMap.IsOaSeatMap == false
                             && string.IsNullOrEmpty(objMOBSeatMap.ShowInfoMessageOnSeatMap) == false)
                        objMOBSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("NoFreeSeatAvailableMessageHeader");
                }
            }

            _seatEngine.EconomySeatsForBUSService(objMOBSeatMap);
            return objMOBSeatMap;
        }
        public async Task<(decimal lowestEplusPrice, decimal lowestEMinusPrice, string currencyCode)> GetSeatPriceForOfferTile(MOBPNR pnr, United.Service.Presentation.ReservationModel.Reservation cslReservation, string token, MOBRequest mobRequest, string sessionId, Session session)
        {
            bool showEminusOffer = false;
            decimal lowestEplusPrice = 0;
            decimal lowestEMinusPrice = 0;
            string currencyCode = string.Empty;

            var hasNonElfSegments = pnr.Segments.Any(s => !(s.IsElf || s.IsIBE));
            bool hasAllEPlusSeats = false;

            if (!hasNonElfSegments)
            {
                var noOfTravelers = 0;
                int.TryParse(pnr.NumberOfPassengers, out noOfTravelers);
                showEminusOffer = pnr.Segments.Any(seg => (seg.IsElf || seg.IsIBE)
                                                       && (seg.Seats.IsNullOrEmpty() || !seg.Seats.IsNullOrEmpty() && seg.Seats.Count >= noOfTravelers && seg.Seats.Any(s => s.Price.Equals(0))));
            }
            else
            {
                bool doNotShowEplusForAlreadyOfferedToggle = _configuration.GetValue<bool>("DoNotShowEplusForAlreadyPurchasedToggle");

                if (doNotShowEplusForAlreadyOfferedToggle)
                {
                    foreach (var seat in pnr.Segments.SelectMany(seg => seg.Seats.Select(seat => seat)))
                    {
                        if (!ConfigUtility.CheckEPlusSeatCode(seat.ProgramCode))
                        {
                            hasAllEPlusSeats = false;
                            break;
                        }
                        else
                        {
                            hasAllEPlusSeats = true;
                        }
                    }
                }
            }

            if (showEminusOffer || (hasNonElfSegments && !hasAllEPlusSeats))
            {
                bool isOANoSeatMapAvailableNewMessageEnabled = await _featureSettings.GetFeatureSettingValue("EnableNewOASeatMapUnavailableMessage");

                if (!_configuration.GetValue<bool>("LogSeatEngineAWSCallsTiming"))
                {
                    #region with out logs
                    // Get channel Name from config
                    string[] channelInfo = _configuration.GetValue<string>("CSL30MBEChannelInfo").Split('|');
                    // Build seatmap request, Common for Offertile and SelectSeats
                    // FlightSegments object will vary from Booking, ManageRes and Offer Tiles.
                    // Creating seperate flight segments for each flow
                    var request = BuildSeatMapRequest(FlowType.VIEWRES.ToString(), string.Empty, channelInfo?[0], channelInfo?[1], pnr.RecordLocator, false, pnr.IsIBE);
                    if (ConfigUtility.EnablePBE() && pnr.IsIBE)
                    {
                        request.ProductCode = pnr.ProductCode;
                    }
                    request.FlightSegments = BuildFlightSegmentsForOfferTile(pnr, cslReservation);
                    string cslRequest = JsonConvert.SerializeObject(request);
                    string url = string.Empty;

                    if (request.FlightSegments == null || !request.FlightSegments.Any())
                    {
                        return (lowestEplusPrice, lowestEMinusPrice, currencyCode);
                    }

                    // Get seatmap response, Common for booking, manageres and offertile.


                    string cslstrResponse = await GetSelectSeatMapResponse(url, sessionId, cslRequest, token, channelInfo?[0], channelInfo?[1], isOANoSeatMapAvailableNewMessageEnabled, session, "/GetSeatOfferTiles");

                    United.Definition.SeatCSL30.OfferTiles response = new United.Definition.SeatCSL30.OfferTiles();
                    if (!string.IsNullOrEmpty(cslstrResponse))
                    {
                        response = JsonConvert.DeserializeObject<United.Definition.SeatCSL30.OfferTiles>(cslstrResponse);
                    }
                    if (response != null && response.Errors != null && !response.Errors.Any())
                    {
                        currencyCode = response.CurrencyCode.IsNullOrEmpty() ? "USD" : response.CurrencyCode.ToUpper();

                        if (showEminusOffer)
                            lowestEMinusPrice = response.SeatOfferTiles;
                        else if (hasNonElfSegments)
                            lowestEplusPrice = response.SeatOfferTiles;
                    }
                    else
                    {
                        string errorMessage = string.Empty;
                        //error handling
                        if (response != null && response.Errors != null && response.Errors.Any())
                        {
                            foreach (var error in response.Errors)
                            {
                                errorMessage = errorMessage + " " + error.Message;
                            }
                        }
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            //  LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatPriceForOfferTile", "MOBUnitedException", mobRequest.Application.Id, mobRequest.Application?.Version?.Major, mobRequest.DeviceId, errorMessage));
                        }
                    }

                    // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatPriceForOfferTile", "Client-Response", mobRequest.Application.Id, mobRequest.Application?.Version?.Major, mobRequest.DeviceId, "Lowest Eplus Price : " + lowestEplusPrice.ToString() + " Lowest Eminus Price: " + lowestEMinusPrice.ToString() + " CurrencyCode: " + currencyCode)); //CSL30 response log
                    #endregion
                }
                else
                {
                    #region with logs - MOBILE-9558 To log time taken for SeatEngine AWS Call
                    // Get channel Name from config
                    string[] channelInfo = _configuration.GetValue<string>("CSL30MBEChannelInfo").Split('|');
                    // Build seatmap request, Common for Offertile and SelectSeats
                    // FlightSegments object will vary from Booking, ManageRes and Offer Tiles.
                    // Creating seperate flight segments for each flow
                    var request = BuildSeatMapRequest(FlowType.VIEWRES.ToString(), string.Empty, channelInfo?[0], channelInfo?[1], pnr.RecordLocator, false, pnr.IsIBE);
                    if (ConfigUtility.EnablePBE() && pnr.IsIBE)
                    {
                        request.ProductCode = pnr.ProductCode;
                    }
                    request.FlightSegments = BuildFlightSegmentsForOfferTile(pnr, cslReservation);

                    string cslRequest = JsonConvert.SerializeObject(request);
                    string url = string.Empty;

                    if (request.FlightSegments == null || !request.FlightSegments.Any())
                    {
                        return (lowestEplusPrice, lowestEMinusPrice, currencyCode);
                    }

                    string cslstrResponse;
                    Stopwatch stopwatch = new Stopwatch();
                    try
                    {
                        stopwatch.Start();
                        // Get seatmap response, Common for booking, manageres and offertile.
                        cslstrResponse = await GetSelectSeatMapResponse(url, sessionId, cslRequest, token, channelInfo?[0], channelInfo?[1], isOANoSeatMapAvailableNewMessageEnabled, session, "/GetSeatOfferTiles");
                    }
                    finally
                    {
                        stopwatch.Stop();
                        String logMachinename = _configuration.GetValue<string>("MachineNameForLogSeatEngineAWSCallsTiming");

                        if (string.IsNullOrEmpty(logMachinename))
                        {
                            // LogEntries.Add(LogEntry.GetLogEntry<string>(sessionId, "GetSeatPriceForOfferTile", "MobileRESTCallDuration", mobRequest.Application.Id, mobRequest.Application?.Version?.Major, mobRequest.DeviceId, (stopwatch.ElapsedMilliseconds / (double)1000).ToString()));
                        }
                        else if (!string.IsNullOrEmpty(System.Environment.MachineName) && logMachinename.Contains(System.Environment.MachineName))
                        {
                            // LogEntries.Add(LogEntry.GetLogEntry<string>(sessionId, "GetSeatPriceForOfferTile", "MobileRESTCallDuration", mobRequest.Application.Id, mobRequest.Application?.Version?.Major, mobRequest.DeviceId, (stopwatch.ElapsedMilliseconds / (double)1000).ToString()));
                        }

                    }

                    United.Definition.SeatCSL30.OfferTiles response = new United.Definition.SeatCSL30.OfferTiles();
                    if (!string.IsNullOrEmpty(cslstrResponse))
                    {
                        response = JsonConvert.DeserializeObject<United.Definition.SeatCSL30.OfferTiles>(cslstrResponse);
                    }
                    if (response != null && response.Errors != null && !response.Errors.Any())
                    {
                        currencyCode = response.CurrencyCode.IsNullOrEmpty() ? "USD" : response.CurrencyCode.ToUpper();

                        if (showEminusOffer)
                            lowestEMinusPrice = response.SeatOfferTiles;
                        else if (hasNonElfSegments)
                            lowestEplusPrice = response.SeatOfferTiles;
                    }
                    else
                    {
                        string errorMessage = string.Empty;
                        //error handling
                        if (response != null && response.Errors != null && response.Errors.Any())
                        {
                            foreach (var error in response.Errors)
                            {
                                errorMessage = errorMessage + " " + error.Message;
                            }
                        }
                        if (!string.IsNullOrEmpty(errorMessage))
                        {

                            _logger.LogWarning("GetSeatPriceForOfferTile {unitedexception}", errorMessage);

                        }
                    }

                    #endregion
                }
            }
            return (lowestEplusPrice, lowestEMinusPrice, currencyCode);
        }

        private ICollection<FlightSegments> BuildFlightSegmentsForOfferTile(MOBPNR pnr, United.Service.Presentation.ReservationModel.Reservation cslReservation)
        {
            if (pnr?.Segments == null)
                return null;

            var flightSegments = new Collection<United.Definition.SeatCSL30.FlightSegments>();
            string[] isSeatMapSupportedSegmentCodes = _configuration.GetValue<string>("SeatMapOfferSegmentCodes").Split(',');
            int id = 1;
            foreach (var segment in pnr.Segments)
            {
                if (!segment.IsNullOrEmpty() && !segment.ActionCode.IsNullOrEmpty() && !isSeatMapSupportedSegmentCodes.IsNullOrEmpty() && isSeatMapSupportedSegmentCodes.Any(x => !x.IsNullOrEmpty() && segment.ActionCode.Contains(x)))
                {
                    if (segment.IsChangeOfGuage && segment?.Stops?.Count > 0)
                    {
                        foreach (var stopSegment in segment.Stops)
                        {
                            if (segment.MarketingCarrier.Code.Equals("UA", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!stopSegment.IsNullOrEmpty() && !stopSegment.MarketingCarrier.IsNullOrEmpty() && !stopSegment.OperationoperatingCarrier.IsNullOrEmpty())
                                {
                                    var flightSegment = BuildFlightSegmentsForSeatMap(stopSegment.Departure?.Code, stopSegment.Arrival?.Code, pnr.IsCheckinEligible, stopSegment.ClassOfService, stopSegment.ScheduledDepartureDateTime, stopSegment.ScheduledArrivalDateTime, stopSegment.FareBasisCode, Convert.ToInt32(stopSegment.FlightNumber), stopSegment.MarketingCarrier.Code, stopSegment.OperationoperatingCarrier.Code, stopSegment.SegmentNumber, "true", id);
                                    id++;
                                    flightSegments.Add(flightSegment);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!segment.IsNullOrEmpty() && !segment.MarketingCarrier.IsNullOrEmpty() && !segment.OperationoperatingCarrier.IsNullOrEmpty())
                        {
                            if (segment.MarketingCarrier.Code.Equals("UA", StringComparison.OrdinalIgnoreCase))
                            {
                                var flightSegment = BuildFlightSegmentsForSeatMap(segment.Departure?.Code, segment.Arrival?.Code, pnr.IsCheckinEligible, segment.ClassOfService, segment.ScheduledDepartureDateTime, segment.ScheduledArrivalDateTime, segment.FareBasisCode, Convert.ToInt32(segment.FlightNumber), segment.MarketingCarrier.Code, segment.OperationoperatingCarrier.Code, segment.SegmentNumber, "true", id);
                                id++;
                                flightSegments.Add(flightSegment);
                            }
                        }
                    }
                }
            }
            return flightSegments;
        }

        public bool EnableAdvanceSearchCouponBooking(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAdvanceSearchCouponBookingVersion"), _configuration.GetValue<string>("iPhoneAdvanceSearchCouponBookingVersion"));
        }

        private bool IsLimitedRecline(Seat seat)
        {
            return new string[] { "LIMITED", "NO" }.Contains(seat?.ReclineType?.ToUpper());
        }

        public InterLineDeepLink GetInterlineRedirectLink(List<MOBBKTraveler> bookingTravelerInfo, string pointOfSale, MOBSeatChangeSelectRequest request, string recordLocator, string lastName, List<MOBItem> catalog, string operatingCarrier, string origin, string destination, string departDate)
        {
            InterLineDeepLink OADeepLinkContent = null;
            if (ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrier) && ConfigUtility.IsEligibleCarrierAndAPPVersion(operatingCarrier, request.Application.Id, request?.Application?.Version?.Major, catalog))
            {
                string carrierAdvisoryMessage = string.Empty;
                string deepLinkURL = ConfigUtility.CreateDeepLinkURLForOtherAirlinesManageRes(recordLocator, lastName, pointOfSale, request.LanguageCode, operatingCarrier, out carrierAdvisoryMessage);

                OADeepLinkContent = new InterLineDeepLink();

                OADeepLinkContent.ShowInterlineAdvisoryMessage = true;
                string depTimeFormatted = Convert.ToDateTime(departDate).ToString("ddd, MMM dd");
                OADeepLinkContent.InterlineAdvisoryTitle = $"{depTimeFormatted} {origin} - {destination}";
                OADeepLinkContent.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("InterlineDeepLinkRedesignMessageTitle");
                OADeepLinkContent.InterlineAdvisoryDeepLinkURL = deepLinkURL;
                OADeepLinkContent.InterlineAdvisoryMessage = carrierAdvisoryMessage;
            }
            return OADeepLinkContent;
        }



        public void GetInterlineRedirectLink(List<TripSegment> segments, string pointOfSale, MOBRequest mobRequest, string recordLocator, string lastname, List<MOBItem> catalog)
        {
            foreach (var segment in segments)
            {
                if (ConfigUtility.IsEligibleCarrierAndAPPVersion(segment.OperatingCarrier, mobRequest.Application.Id, mobRequest?.Application?.Version?.Major, catalog))
                {
                    string carrierAdvisoryMessage = string.Empty;
                    string deepLinkURL = ConfigUtility.CreateDeepLinkURLForOtherAirlinesManageRes(recordLocator, lastname, pointOfSale, mobRequest.LanguageCode, segment.OperatingCarrier, out carrierAdvisoryMessage);

                    segment.ShowInterlineAdvisoryMessage = !string.IsNullOrEmpty(deepLinkURL) ? true : false;

                    segment.InterlineAdvisoryMessage = carrierAdvisoryMessage;
                    segment.InterlineAdvisoryDeepLinkURL = deepLinkURL;
                    string depTimeFormatted = Convert.ToDateTime(segment.ScheduledDepartureDate).ToString("ddd, MMM dd");
                    segment.InterlineAdvisoryTitle = $"{depTimeFormatted} {segment.Departure.Code} - {segment.Arrival.Code}";
                    segment.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("InterlineDeepLinkRedesignMessageTitle");
                }
            }
        }

        public async System.Threading.Tasks.Task<string> GetOANoSeatMapAvailableNewMessageBodyFromSDL(Session session)
        {
            string OANoSeatMapAvailableNewMessage = null;

            List<CMSContentMessage> lstMessages = await GetSDLContentByGroupName(session.SessionId, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_VIEWRESSeatMap_Messages"), "ViewRes_SeatMap_CMSContentMessagesCached_StaticGUID");

            if (lstMessages != null)
            {
                var contentMsg = lstMessages.FirstOrDefault(msg => msg.Title == "MR.OANoSeatMapAvailable.NewMessage");

                 OANoSeatMapAvailableNewMessage = string.IsNullOrEmpty(contentMsg?.ContentFull) ? _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody"): contentMsg?.ContentFull;
            }

            return OANoSeatMapAvailableNewMessage;
        }

        public async System.Threading.Tasks.Task<string> GetOANoSeatMapAvailableNewMessage(Session session)
        {
            string OANoSeatMapAvailableNewMessageBody = await GetOANoSeatMapAvailableNewMessageBodyFromSDL(session);
            return string.Concat(OANoSeatMapAvailableNewMessageBody, _configuration.GetValue<string>("OANoSeatMapAvailableMessageTitle"));
        }

        public async Task GetOANoSeatAvailableMessage(List<TripSegment> segments, bool isOANoSeatMapAvailableNewMessageEnabled, Session session)
        {
            if (segments != null && segments.Count > 0)
            {
                foreach (var segment in segments)
                {
                    segment.ShowInterlineAdvisoryMessage = true;
                    segment.InterlineAdvisoryDeepLinkURL = string.Empty;
                    segment.InterlineAdvisoryMessage = isOANoSeatMapAvailableNewMessageEnabled ? await GetOANoSeatMapAvailableNewMessageBodyFromSDL(session) : _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody");
                    string depTimeFormatted = Convert.ToDateTime(segment.ScheduledDepartureDate).ToString("ddd, MMM dd");
                    segment.InterlineAdvisoryTitle = $"{depTimeFormatted} {segment.Departure.Code} - {segment.Arrival.Code}";
                    segment.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("OANoSeatMapAvailableMessageTitle");
                }
            }
        }

        private async Task<List<MOBLegend>> GetOACabinLegends(List<MOBCabin> cabins, List<string> polarisCabinBrandingDescriptions)
        {
            var priceRange = from c in cabins
                             from r in c.Rows
                             from s in r.Seats
                             select new MOBPriceRange()
                             {
                                 SeatLegend = c.COS.Replace("Upper Deck ", ""),
                                 SeatFeature = s.DisplaySeatFeature,
                                 SeatPrice = s.ServicesAndFees != null && s.ServicesAndFees.Count > 0 ? s.ServicesAndFees.Min(f => decimal.Truncate(f.TotalFee)) : 0
                             };

            List<MOBLegend> cabinLegends = new List<MOBLegend>();

            if (polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0)
            {
                foreach (string cabinDescription in polarisCabinBrandingDescriptions)
                {
                    switch (cabinDescription.ToUpper())
                    {
                        case "FIRST":
                            cabinLegends.Add(new MOBLegend() { Name = "First", Image = "OAFirst", Details = "From $" + priceRange.Where(l => (l.SeatLegend == "First" || l.SeatFeature == "First") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() });
                            break;

                        case "BUSINESS":
                            cabinLegends.Add(new MOBLegend() { Name = "Business", Image = "OABusiness", Details = "From $" + priceRange.Where(l => (l.SeatLegend == "Business" || l.SeatFeature == "Business") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() });
                            break;

                        case "PREMIUM ECONOMY":
                            cabinLegends.Add(new MOBLegend() { Name = "Premium Economy", Image = "OAPremiumEconomy", Details = "From $" + priceRange.Where(l => (l.SeatLegend == "Premium Economy" || l.SeatFeature == "Premium Economy") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() });
                            break;

                        case "ECONOMY":
                            cabinLegends.Add(new MOBLegend() { Name = "Economy", Image = "UnitedEconomy", Details = "From $" + priceRange.Where(l => (l.SeatLegend == "Economy" || l.SeatFeature == "Economy") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() });
                            break;
                    }
                }
            }

            var legends = cabinLegends.Where(l => l.Details.ToLower() == "from $0");
            if (legends != null && legends.Count() > 0) foreach (var legend in legends) legend.Details = "";

            return cabinLegends;
        }

        public async Task<List<MOBLegend>> GetCabinLegends(List<string> polarisCabinBrandingDescriptions, List<MOBCabin> cabins, string bookingCabin, int ePlusSubscriberCount, int segmentIndex, bool isELF, bool isIBE, bool isBERecommendedSeatsAvailable, bool hasDAASeat = false, bool hasFBBSeat = false)
        {
            List<MOBLegend> cabinLegends = new List<MOBLegend>();
            var isSameCabin = false;
            var hasAvailableSeats = false;
            var hasEnoughPcuSeats = false;
            var isBE = isELF || isIBE;
            var IsEPlus = ePlusSubscriberCount > 0;
            var isEconomy = bookingCabin.Contains("Economy", StringComparison.OrdinalIgnoreCase) || bookingCabin.Contains("United Economy", StringComparison.OrdinalIgnoreCase);
            var isPremium = bookingCabin.Contains("United Premium Plus", StringComparison.OrdinalIgnoreCase);
            var isUpp = bookingCabin.Contains("United Polaris Business", StringComparison.OrdinalIgnoreCase) || bookingCabin.Contains("United First", StringComparison.OrdinalIgnoreCase);

            var priceRange = from c in cabins
                             from r in c.Rows
                             from s in r.Seats
                             select new MOBPriceRange()
                             {
                                 SeatLegend = c.COS,
                                 SeatFeature = s.DisplaySeatFeature,
                                 SeatPrice = s.ServicesAndFees != null && s.ServicesAndFees.Count > 0 ? s.ServicesAndFees.Min(f => decimal.Truncate(f.TotalFee)) : 0,
                                 PcuOfferPrice = !string.IsNullOrWhiteSpace(s.PcuOfferPrice) ? decimal.Truncate(decimal.Parse(s.PcuOfferPrice.Replace("$", ""))) : 0,
                                 IsPcuOfferEligible = s.IsPcuOfferEligible,
                                 HasAvailableSeats = c.HasAvailableSeats,
                                 HasEnoughPcuSeats = c.HasEnoughPcuSeats
                             };

            if (polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0)
            {
                var name = string.Empty;
                var details = string.Empty;
                var pcuOfferPrice = string.Empty;

                foreach (string cabinDescription in polarisCabinBrandingDescriptions)
                {
                    switch (cabinDescription.ToUpper())
                    {
                        case "UNITED GLOBAL FIRST":

                        case "UNITED BUSINESS FIRST":

                        case "UNITED POLARIS FIRST":

                        case "UNITED POLARIS BUSINESS":

                        case "UNITED FIRST":

                        case "UNITED BUSINESS":

                            isSameCabin = cabinDescription.Contains(bookingCabin, StringComparison.OrdinalIgnoreCase);
                            hasAvailableSeats = cabins.Where(c => c.COS == cabinDescription).Select(h => h.HasAvailableSeats).FirstOrDefault();
                            hasEnoughPcuSeats = cabins.Where(c => c.COS == cabinDescription).Select(h => h.HasEnoughPcuSeats).FirstOrDefault();
                            pcuOfferPrice = priceRange.Where(l => (l.SeatLegend == cabinDescription || l.SeatFeature == cabinDescription) && (decimal?)l.PcuOfferPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => p.PcuOfferPrice).ToString();
                            details = "From $" + priceRange.Where(l => (l.SeatLegend == cabinDescription || l.SeatFeature == cabinDescription) && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString();

                            if (isSameCabin)
                            {
                                if (hasAvailableSeats)
                                {
                                    details = "Included";
                                }
                                else
                                {
                                    details = "Unavailable";
                                }
                            }
                            else if (isBE)
                            {
                                details = "Unavailable";
                            }
                            else if (isEconomy || isPremium)
                            {
                                if (hasEnoughPcuSeats && !string.IsNullOrWhiteSpace(pcuOfferPrice))
                                {
                                    details = "Upgrade for $" + pcuOfferPrice;
                                }
                                else
                                {
                                    details = "Unavailable";
                                }
                            }

                            name = cabinDescription.Contains("®") ? cabinDescription : cabinDescription + "®";
                            if (name.Contains("Polaris")) name = name.Replace("®", "").Replace("Polaris", "Polaris®");

                            cabinLegends.Add(new MOBLegend() { Name = name, Image = hasDAASeat ? "DAFL" : hasFBBSeat ? "FBB" : "UnitedFirst", Details = details });
                            break;

                        case "UNITED PREMIUM PLUS":

                            isSameCabin = cabinDescription.Contains(bookingCabin, StringComparison.OrdinalIgnoreCase);
                            hasAvailableSeats = cabins.Where(c => c.COS == cabinDescription).Select(h => h.HasAvailableSeats).FirstOrDefault();
                            hasEnoughPcuSeats = cabins.Where(c => c.COS == cabinDescription).Select(h => h.HasEnoughPcuSeats).FirstOrDefault();
                            pcuOfferPrice = priceRange.Where(l => (l.SeatLegend == cabinDescription || l.SeatFeature == cabinDescription) && (decimal?)l.PcuOfferPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => p.PcuOfferPrice).ToString();
                            details = "From $" + priceRange.Where(l => (l.SeatLegend == cabinDescription || l.SeatFeature == cabinDescription) && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString();

                            if (isSameCabin)
                            {
                                if (hasAvailableSeats)
                                {
                                    details = "Included";
                                }
                                else
                                {
                                    details = "Unavailable";
                                }
                            }
                            else if (isBE || isUpp)
                            {
                                details = "Unavailable";
                            }
                            else if (isEconomy)
                            {
                                if (hasEnoughPcuSeats && !string.IsNullOrWhiteSpace(pcuOfferPrice))
                                {
                                    details = "Upgrade for $" + pcuOfferPrice;
                                }
                                else
                                {
                                    details = "Unavailable";
                                }
                            }

                            name = cabinDescription.Contains("℠") ? cabinDescription : cabinDescription + "℠";
                            cabinLegends.Add(new MOBLegend() { Name = name.Replace("®", ""), Image = "UPP", Details = details });
                            break;
                    }
                }
            }

            MOBLegend economyPlusLegend = new MOBLegend() { Name = "Economy Plus®", Image = "EPU", Details = isBE || isPremium || isUpp ? "Unavailable" : IsEPlus ? "Included" : "From $" + priceRange.Where(l => (l.SeatLegend == "Economy Plus" || l.SeatFeature == "Economy Plus") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() };
            MOBLegend PZALegend = new MOBLegend() { Name = "Preferred Seat", Image = "PZA", Details = isPremium || isUpp ? "Unavailable" : IsEPlus ? "Included" : "From $" + priceRange.Where(l => (l.SeatLegend == "Preferred Seat" || l.SeatFeature == "Preferred Seat") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() };
            MOBLegend economyLegend = new MOBLegend() { Name = "United Economy®", Image = "UnitedEconomy", Details = isPremium || isUpp ? "Unavailable" : IsEPlus || (isEconomy && !isBE) ? "Included" : "From $" + priceRange.Where(l => (l.SeatLegend == "Economy" || l.SeatLegend == "United Economy" || l.SeatFeature == "Economy" || l.SeatFeature == "United Economy") && (decimal?)l.SeatPrice > 0).DefaultIfEmpty(new MOBPriceRange()).Distinct().Min(p => (decimal?)p.SeatPrice).ToString() };
          
            cabinLegends.Add(economyPlusLegend);
            cabinLegends.Add(PZALegend);
            cabinLegends.Add(economyLegend);

            if (isBERecommendedSeatsAvailable)
            {
                MOBLegend familySeatingLegend = new MOBLegend() { Name = "Free family seating", Image = "FreeFamilySeating", Details = "Included" };
                cabinLegends.Add(familySeatingLegend);
            }

            var legends = cabinLegends.Where(l => l.Details.ToLower() == "from $0" || l.Details.ToLower() == "upgrade for $0");
            if (legends != null && legends.Count() > 0) foreach (var legend in legends) legend.Details = "";

            return cabinLegends;
        }

        public async Task<List<MOBLegend>> GetMonumentLegends(List<string> monuments)
        {
            #region

            List<MOBLegend> monumentLegends = new List<MOBLegend>();

            monumentLegends.Add(new MOBLegend("Unavailable seat", "OCC", string.Empty));
            monumentLegends.Add(new MOBLegend("Exit", "EXIT", string.Empty));

            foreach (string monument in monuments)
            {
                switch (monument)
                {
                    case "LAV":
                        monumentLegends.Add(new MOBLegend("Lavatory", "LAV", string.Empty));
                        break;

                    case "A LAV":
                        monumentLegends.Insert(3, new MOBLegend("Accessible lavatory", "ALAV", string.Empty));
                        break;

                    case "GALLEY":
                        monumentLegends.Add(new MOBLegend("Galley", "GAL", string.Empty));
                        break;

                    case "CLOSET":
                        monumentLegends.Add(new MOBLegend("Closet", "CLO", string.Empty));
                        break;

                    default:
                        break;
                }
            }

            return monumentLegends;

            #endregion
        }

        public async Task<List<MOBItem>> GetPcuCaptions(string sessionId)
        {
            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            MOBRequest request = new MOBRequest();
            request.TransactionId = session.SessionId;
            var key = "SeatMapCaptions_SDLGroupName";
            var groupName = _configuration.GetValue<string>(key);

            var sdlContentMessages = await GetSDLContentByGroupName(request.TransactionId, session.SessionId, session.Token, groupName, key);
            if (sdlContentMessages == null || !sdlContentMessages.Any() || !sdlContentMessages.Any(doc => doc.GroupName.Contains("MOBILE:SEATMAP")))
                return null;

            var pcuCaptions = new List<MOBItem>();
            pcuCaptions.AddRange(
                sdlContentMessages.Where(doc => doc.GroupName.Contains("MOBILE:SEATMAP")).Select(doc => new MOBItem
                {
                    Id = doc.Title,
                    CurrentValue = doc.ContentFull
                }));
            return pcuCaptions;
        }
        public async Task<List<CMSContentMessage>> GetSDLContentByGroupName(string transactionId, string sessionId, string token, string groupName, string docNameConfigEntry, bool useCache = false)
        {
            CSLContentMessagesResponse response = null;

            try
            {
                var getSDL = await _cachingService.GetCache<string>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", transactionId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(getSDL))
                {
                    response = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(getSDL);
                }
                if (response != null && response.Messages != null) { return response.Messages; }
            }
            catch { }

            MOBCSLContentMessagesRequest sdlReqeust = new MOBCSLContentMessagesRequest
            {
                Lang = "en",
                Pos = "us",
                Channel = "mobileapp",
                Listname = new List<string>(),
                LocationCodes = new List<string>(),
                Groupname = groupName,
                Usecache = useCache
            };

            string jsonRequest = JsonConvert.SerializeObject(sdlReqeust);

            response = await _iCMSContentService.GetSDLContentByGroupName<CSLContentMessagesResponse>(token, "message", jsonRequest, sessionId).ConfigureAwait(false);

            if (response == null)
            {
                _logger.LogError("GetSDLContentByGroupName Failed to deserialize CSL response");
                return null;
            }

            if (response.Errors.Count > 0)
            {
                string errorMsg = String.Join(" ", response.Errors.Select(x => x.Message));
                _logger.LogError("GetSDLContentByGroupName {@CSLCallError}", errorMsg);
                return null;
            }

            if (response != null && (Convert.ToBoolean(response.Status) && response.Messages != null))
            {
                if (!_configuration.GetValue<bool>("DisableSDLEmptyTitleFix"))
                {
                    response.Messages = response.Messages.Where(l => l.Title != null).ToList();
                }
                var saveSDL = await _cachingService.SaveCache<CSLContentMessagesResponse>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", response, transactionId, new TimeSpan(1, 30, 0)).ConfigureAwait(false);

            }
            return response.Messages;
        }

        public bool IsCabinMatchedCSL(string pcuCabin, string seatmapCabin)
        {
            if (string.IsNullOrEmpty(pcuCabin) || string.IsNullOrEmpty(seatmapCabin))
                return false;

            pcuCabin = pcuCabin.Trim().Replace("®", "").Replace("℠", "").ToUpper();
            seatmapCabin = seatmapCabin.ToUpper().Trim();

            if (pcuCabin.Equals(seatmapCabin, StringComparison.OrdinalIgnoreCase))
                return true;

            var possiblefirstCabins = new List<string> { "FIRST", "UNITED FIRST", "UNITED GLOBAL FIRST", "UNITED POLARIS FIRST" };
            if (possiblefirstCabins.Contains(seatmapCabin) && possiblefirstCabins.Contains(pcuCabin))
                return true;

            var possibleBusinessCabins = new List<string> { "UNITED BUSINESS", "UNITED BUSINESS", "UNITED POLARIS BUSINESS", "BUSINESSFIRST", "UNITED BUSINESSFIRST" };
            if (possibleBusinessCabins.Contains(seatmapCabin) && possibleBusinessCabins.Contains(pcuCabin))
                return true;

            var possibleUppCabins = new List<string> { "UNITED PREMIUM PLUS", "UNITED PREMIUMPLUS" };
            if (possibleUppCabins.Contains(seatmapCabin) && possibleUppCabins.Contains(pcuCabin))
                return true;

            return false;
        }

        public async Task<List<MOBItem>> GetPcuCaptions(string travelerNames, string recordLocator)
        {
            var pcuCaptions = await GetCaptions("PCU_IN_SEATMAP_PRODUCTPAGE");
            if (pcuCaptions == null || !pcuCaptions.Any() || string.IsNullOrEmpty(travelerNames))
                return null;

            pcuCaptions.Add(new MOBItem { Id = "PremiumSeatTravelerNames", CurrentValue = travelerNames });
            pcuCaptions.Add(new MOBItem { Id = "RecordLocator", CurrentValue = recordLocator });
            return pcuCaptions;
        }
        private async Task<List<MOBItem>> GetCaptions(string key)
        {
            return !string.IsNullOrEmpty(key) ? await GetCaptions(key, true) : null;
        }
        private async Task<List<MOBItem>> GetCaptions(string keyList, bool isTnC)
        {
            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(keyList, _headers.ContextValues.TransactionId, isTnC).ConfigureAwait(false);
            if (docs == null || !docs.Any()) return null;

            var captions = new List<MOBItem>();

            captions.AddRange(
                docs.Select(doc => new MOBItem
                {
                    Id = doc.Title,
                    CurrentValue = doc.LegalDocument
                }));
            return captions;
        }
    }
}
