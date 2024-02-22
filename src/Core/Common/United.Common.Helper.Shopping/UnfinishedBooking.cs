using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.FlightShopping;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.MerchandizeService;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Profile;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.SSR;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.UnfinishedBooking;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.ReferenceDataRequestModel;
using United.Service.Presentation.ReferenceDataResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.Customer.Preferences.Common;
using United.Services.FlightShopping.Common;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Services.FlightShopping.Common.LMX;
using United.Utility.Enum;
using United.Utility.Helper;
using Aircraft = United.Services.FlightShopping.Common.Aircraft;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using EquipmentDisclosure = United.Services.FlightShopping.Common.EquipmentDisclosure;
using GetSavedItineraryDataModel = United.Services.Customer.Preferences.Common.GetSavedItineraryDataModel;
using InsertSavedItineraryData = United.Services.Customer.Preferences.Common.InsertSavedItineraryData;
using MOBSHOPFlattenedFlight = United.Mobile.Model.Shopping.MOBSHOPFlattenedFlight;
using MOBSHOPTax = United.Mobile.Model.Shopping.MOBSHOPTax;
using Product = United.Services.FlightShopping.Common.Product;
using SavedItineraryDataModel = United.Services.Customer.Preferences.Common.SavedItineraryDataModel;
using SerializableSavedItinerary = United.Services.Customer.Preferences.Common.SerializableSavedItinerary;
using SubCommonData = United.Services.Customer.Preferences.Common.SubCommonData;
using Trip = United.Services.FlightShopping.Common.Trip;
namespace United.Common.Helper.Shopping
{
    public class UnfinishedBooking : IUnfinishedBooking
    {
        private readonly ICacheLog<UnfinishedBooking> _logger;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IConfiguration _configuration;
        private readonly IFlightShoppingService _flightShoppingService;
        private readonly ICustomerPreferencesService _customerPreferencesService;
        private readonly IDPService _dPService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOmniCart _omniCart;
        private readonly ITravelerCSL _travelerCSL;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IReferencedataService _referencedataService;
        private readonly IPurchaseMerchandizingService _purchaseMerchandizingService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ICachingService _cachingService;
        private AirportDetailsList airportsList;
        private MOBSHOPUnfinishedBookingComparer comparer = new MOBSHOPUnfinishedBookingComparer();
        private string shopPinDownActionName = "shoppindown";
        private string shopPinDownErrorSeparator = ",";
        private string shopPinDownErrorMajorAndMinorCodesSeparator = "^";
        private string savedUnfinishedBookingActionName = "SavedItinerary";
        private string savedUnfinishedBookingAugumentName = "CustomerId";
        private readonly IPNRRetrievalService _pNRRetrievalService;
        private readonly IPKDispenserService _pKDispenserService;
        private string CURRENCY_TYPE_MILES = "miles";
        private string PRICING_TYPE_CLOSE_IN_FEE = "CLOSEINFEE";
        private readonly IFFCShoppingcs _ffcShoppingcs;
        private readonly IHeaders _headers;

        public UnfinishedBooking(ICacheLog<UnfinishedBooking> logger
            , ISessionHelperService sessionHelperService
            , IShoppingUtility shoppingUtility
            , IConfiguration configuration
            , IFlightShoppingService flightShoppingService
            , ICustomerPreferencesService customerPreferencesService
            , IDPService dPService
            , IShoppingCartService shoppingCartService
            , IOmniCart omniCart,
             ITravelerCSL travelerCSL
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IReferencedataService referencedataService
            , IPurchaseMerchandizingService purchaseMerchandizingService
            , IPNRRetrievalService pNRRetrievalService
            , IPKDispenserService pKDispenserService
            , IDynamoDBService dynamoDBService
            , ICachingService cachingService
            , IFFCShoppingcs ffcShoppingcs
            , IHeaders headers)
        {
            _logger = logger;
            _sessionHelperService = sessionHelperService;
            _shoppingUtility = shoppingUtility;
            _configuration = configuration;
            _flightShoppingService = flightShoppingService;
            _customerPreferencesService = customerPreferencesService;
            _dPService = dPService;
            _shoppingCartService = shoppingCartService;
            _omniCart = omniCart;
            _travelerCSL = travelerCSL;
            airportsList = null;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _referencedataService = referencedataService;
            _purchaseMerchandizingService = purchaseMerchandizingService;
            _pNRRetrievalService = pNRRetrievalService;
            _pKDispenserService = pKDispenserService;
            _dynamoDBService = dynamoDBService;
            _cachingService = cachingService;
            _ffcShoppingcs = ffcShoppingcs;
            _headers = headers;
        }

        public string GetFlightShareMessage(MOBSHOPReservation reservation, string cabinType)
        {
            #region Build Reservation Share Message 
            string flightDatesText = DateTime.Parse(reservation.Trips[0].DepartDate.Replace("\\", ""), CultureInfo.InvariantCulture).ToString("MMM dd") + (reservation.Trips.Count == 1 ? "" : (" - " + (DateTime.Parse(reservation.Trips[reservation.Trips.Count - 1].ArrivalDate.Replace("\\", ""), CultureInfo.InvariantCulture).ToString("MMM dd"))));
            string travelersText = reservation.NumberOfTravelers.ToString() + " " + (reservation.NumberOfTravelers > 1 ? "travelers" : "traveler");
            string searchType = string.Empty, flightNumbers = string.Empty, viaAirports = string.Empty;
            string initialOrigin = reservation.Trips[0].Origin.ToUpper().Trim();
            string finalDestination = reservation.Trips[reservation.Trips.Count - 1].Destination.ToUpper().Trim();

            switch (reservation.SearchType.ToUpper().Trim())
            {
                case "OW":
                    searchType = "one way";
                    break;
                case "RT":
                    searchType = "roundtrip";
                    break;
                case "MD":
                    searchType = "multiple destinations";
                    break;
                default:
                    break;
            }
            foreach (MOBSHOPTrip trip in reservation.Trips)
            {
                foreach (MOBSHOPFlattenedFlight flattenedFlight in trip.FlattenedFlights)
                {
                    if (string.IsNullOrEmpty(cabinType))
                    {
                        cabinType = flattenedFlight.Flights[0].Cabin.ToUpper().Trim() == "COACH" ? "Economy" : flattenedFlight.Flights[0].Cabin;
                    }
                    foreach (MOBSHOPFlight flight in flattenedFlight.Flights)
                    {
                        flightNumbers = flightNumbers + "," + flight.FlightNumber;
                        if (flight.Destination.ToUpper().Trim() != initialOrigin && flight.Destination.ToUpper().Trim() != finalDestination)
                        {
                            if (string.IsNullOrEmpty(viaAirports))
                            {
                                viaAirports = " via ";
                            }
                            viaAirports = viaAirports + flight.Destination + ",";
                        }
                    }
                }
            }
            if (flightNumbers.Trim(',').Split(',').Count() > 1)
            {
                flightNumbers = "Flights " + flightNumbers.Trim(',');
            }
            else
            {
                flightNumbers = "Flight " + flightNumbers.Trim(',');
            }
            string reservationShareMessage = string.Format(_configuration.GetValue<string>("Booking20ShareMessage"), flightDatesText, travelersText, searchType, cabinType, flightNumbers.Trim(','), initialOrigin, finalDestination, viaAirports.Trim(','));
            reservation.FlightShareMessage = reservationShareMessage;
            #endregion
            return reservationShareMessage;
        }


        public bool IsOneFlexibleSegmentExist(List<MOBSHOPTrip> trips)
        {
            bool isFlexibleSegment = true;
            if (trips != null)
            {
                foreach (MOBSHOPTrip trip in trips)
                {
                    #region
                    if (trip.FlattenedFlights != null)
                    {
                        foreach (MOBSHOPFlattenedFlight flattenedFlight in trip.FlattenedFlights)
                        {
                            if (flattenedFlight.Flights != null && flattenedFlight.Flights.Count > 0)
                            {
                                foreach (MOBSHOPFlight flight in flattenedFlight.Flights)
                                {
                                    //TFS 53620:Booking - Certain Flights From IAH- ANC Are Displaying An Error Message
                                    if (flight.ShoppingProducts != null && flight.ShoppingProducts.Count > 0)
                                    {
                                        foreach (MOBSHOPShoppingProduct product in flight.ShoppingProducts)
                                        {
                                            if (!product.Type.ToUpper().Trim().Contains("FLEXIBLE"))
                                            {
                                                isFlexibleSegment = false;
                                                break;
                                            }

                                        }
                                    }
                                    if (!isFlexibleSegment)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (!isFlexibleSegment) { break; }
                        }
                    }
                    if (!isFlexibleSegment) { break; }
                    #endregion
                }
            }
            return isFlexibleSegment;
        }

        public async Task<MOBResReservation> PopulateReservation(Session session, Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation == null)
                return null;

            var mobReservation = new MOBResReservation
            {
                FlightSegments = PopulateReservationFlightSegments(reservation.FlightSegments),
                Travelers = PopulateReservationTravelers(reservation.Travelers),
                IsRefundable = reservation.Characteristic.Any(c => c.Code.ToUpper().Trim() == "REFUNDABLE" && United.Utility.Helper.SafeConverter.ToBoolean(c.Value)),
            };
            mobReservation.ISInternational = mobReservation.FlightSegments.Any(item => item.FlightSegment.IsInternational.ToUpper().Trim() == "TRUE");

            var persistedCSLReservation = new CSLReservation(_configuration, _cachingService) { SessionId = session.SessionId, Reservation = mobReservation };
            await _sessionHelperService.SaveSession(persistedCSLReservation, session.SessionId, new List<string> { session.SessionId, persistedCSLReservation.ObjectName }, persistedCSLReservation.ObjectName).ConfigureAwait(false);

            return mobReservation;
        }

        private List<MOBResTraveler> PopulateReservationTravelers(IEnumerable<Service.Presentation.ReservationModel.Traveler> travelers)
        {
            if (travelers == null)
                return null;

            var mobTravelers = new List<MOBResTraveler>();
            foreach (var traveler in travelers)
            {
                MOBResTraveler mobTraveler = new MOBResTraveler();
                if (traveler.Person != null)
                {
                    mobTraveler.Person = new MOBPerPerson();
                    mobTraveler.Person.ChildIndicator = traveler.Person.ChildIndicator;
                    mobTraveler.Person.CustomerId = traveler.Person.CustomerID;
                    mobTraveler.Person.DateOfBirth = traveler.Person.DateOfBirth;
                    mobTraveler.Person.Title = traveler.Person.Title;
                    mobTraveler.Person.GivenName = traveler.Person.GivenName;
                    mobTraveler.Person.MiddleName = traveler.Person.MiddleName;
                    mobTraveler.Person.Surname = traveler.Person.Surname;
                    mobTraveler.Person.Suffix = traveler.Person.Suffix;
                    mobTraveler.Person.Suffix = traveler.Person.Sex;
                    if (traveler.Person.Documents != null)
                    {
                        mobTraveler.Person.Documents = new List<MOBPerDocument>();
                        foreach (var dcoument in traveler.Person.Documents)
                        {
                            MOBPerDocument mobDocument = new MOBPerDocument();
                            mobDocument.DocumentId = dcoument.DocumentID;
                            mobDocument.KnownTravelerNumber = dcoument.KnownTravelerNumber;
                            mobDocument.RedressNumber = dcoument.RedressNumber;
                            mobTraveler.Person.Documents.Add(mobDocument);
                        }
                    }
                }

                if (traveler.LoyaltyProgramProfile != null)
                {
                    mobTraveler.LoyaltyProgramProfile = new MOBComLoyaltyProgramProfile();
                    mobTraveler.LoyaltyProgramProfile.CarrierCode = traveler.LoyaltyProgramProfile.LoyaltyProgramCarrierCode;
                    mobTraveler.LoyaltyProgramProfile.MemberId = traveler.LoyaltyProgramProfile.LoyaltyProgramMemberID;
                }

                mobTravelers.Add(mobTraveler);
            }

            return mobTravelers;
        }

        private List<MOBSegReservationFlightSegment> PopulateReservationFlightSegments(IEnumerable<ReservationFlightSegment> flightSegments)
        {
            if (flightSegments == null)
                return null;

            return flightSegments.Select(s => new MOBSegReservationFlightSegment { FlightSegment = PopulateFlightSegment(s.FlightSegment) }).ToList();
        }

        private SegFlightSegment PopulateFlightSegment(Service.Presentation.SegmentModel.FlightSegment segment)
        {
            if (segment == null)
                return null;

            return new SegFlightSegment
            {
                ArrivalAirport = PopulateAirport(segment.ArrivalAirport),
                BookingClasses = PopulateBookingClasses(segment.BookingClasses),
                DepartureAirport = PopulateAirport(segment.DepartureAirport),
                DepartureDateTime = segment.DepartureDateTime,
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                OperatingAirlineName = segment.OperatingAirlineName,
                IsInternational = segment.IsInternational
            };
        }

        private List<ComBookingClass> PopulateBookingClasses(IEnumerable<BookingClass> bookingClasses)
        {
            if (bookingClasses == null || !bookingClasses.Any())
                return null;

            return bookingClasses.Select(c => new ComBookingClass { Code = c.Code }).ToList();
        }

        private MOBTMAAirport PopulateAirport(Service.Presentation.CommonModel.AirportModel.Airport arrivalAirport)
        {
            if (arrivalAirport == null)
                return null;

            return new MOBTMAAirport { IATACode = arrivalAirport.IATACode, Name = arrivalAirport.Name, ShortName = arrivalAirport.ShortName };
        }

        public void AssignMissingPropertiesfromRegisterFlightsResponse(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReserationResponse, United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse registerFlightsResponse)
        {
            if (!registerFlightsResponse.IsNullOrEmpty())
            {
                registerFlightsResponse.FareLockResponse = flightReserationResponse?.FareLockResponse;
                registerFlightsResponse.Upsells = flightReserationResponse?.Upsells;
                registerFlightsResponse.LastBBXSolutionSetId = flightReserationResponse?.LastBBXSolutionSetId;
            }
        }


        public List<MOBSHOPTax> GetTaxAndFees(List<DisplayPrice> prices, int numPax, bool isReshopChange = false)
        {
            List<MOBSHOPTax> taxsAndFees = new List<MOBSHOPTax>();
            CultureInfo ci = null;
            decimal taxTotal = 0.0M;
            bool isEnableOmniCartMVP2Changes = _configuration.GetValue<bool>("EnableOmniCartMVP2Changes");

            foreach (var price in prices)
            {
                if (price.SubItems != null && price.SubItems.Count > 0
                    && price.Type.Trim().ToUpper() != "RBF" // Added by Hasnan - # 167553 - 10/04/2017
                   )
                {
                    foreach (var subItem in price.SubItems)
                    {
                        if (ci == null)
                        {
                            ci = TopHelper.GetCultureInfo(subItem.Currency);
                        }

                        MOBSHOPTax taxNfee = new MOBSHOPTax();
                        taxNfee.CurrencyCode = subItem.Currency;
                        taxNfee.Amount = subItem.Amount;
                        taxNfee.DisplayAmount = TopHelper.FormatAmountForDisplay(taxNfee.Amount, ci, false);
                        taxNfee.TaxCode = subItem.Type;
                        taxNfee.TaxCodeDescription = subItem.Description;
                        taxsAndFees.Add(taxNfee);

                        taxTotal += taxNfee.Amount;
                    }
                }
                else if (price.Type.Trim().ToUpper() == "RBF") //Reward Booking Fee
                {
                    if (ci == null)
                    {
                        ci = TopHelper.GetCultureInfo(price.Currency);
                    }
                    MOBSHOPTax taxNfee = new MOBSHOPTax();
                    taxNfee.CurrencyCode = price.Currency;
                    taxNfee.Amount = price.Amount / numPax;
                    taxNfee.DisplayAmount = TopHelper.FormatAmountForDisplay(taxNfee.Amount, ci, false);
                    taxNfee.TaxCode = price.Type;
                    taxNfee.TaxCodeDescription = price.Description;
                    taxsAndFees.Add(taxNfee);

                    taxTotal += taxNfee.Amount;
                }
            }

            if (taxsAndFees != null && taxsAndFees.Count > 0)
            {
                //add new label as first item for UI
                MOBSHOPTax tnf = new MOBSHOPTax();
                tnf.CurrencyCode = taxsAndFees[0].CurrencyCode;
                tnf.Amount = taxTotal;
                tnf.DisplayAmount = TopHelper.FormatAmountForDisplay(tnf.Amount, ci, false);
                tnf.TaxCode = "PERPERSONTAX";
                tnf.TaxCodeDescription = string.Format("{0} adult{1}: {2}{3}", numPax, numPax > 1 ? "s" : "", tnf.DisplayAmount, isEnableOmniCartMVP2Changes ? "/person" : " per person");
                taxsAndFees.Insert(0, tnf);

                //add grand total for all taxes
                MOBSHOPTax tnfTotal = new MOBSHOPTax();
                tnfTotal.CurrencyCode = taxsAndFees[0].CurrencyCode;
                tnfTotal.Amount = taxTotal * numPax;
                tnfTotal.DisplayAmount = TopHelper.FormatAmountForDisplay(tnfTotal.Amount, ci, false);
                tnfTotal.TaxCode = "TOTALTAX";
                tnfTotal.TaxCodeDescription = "Taxes and Fees Total";
                taxsAndFees.Add(tnfTotal);

            }

            return taxsAndFees;
        }

        public List<Mobile.Model.Shopping.TravelOption> GetTravelOptions(DisplayCart displayCart)
        {
            List<Mobile.Model.Shopping.TravelOption> travelOptions = null;
            if (displayCart != null && displayCart.TravelOptions != null && displayCart.TravelOptions.Count > 0)
            {
                CultureInfo ci = null;
                travelOptions = new List<Mobile.Model.Shopping.TravelOption>();
                bool addTripInsuranceInTravelOption =
                    !_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch")
                    && Convert.ToBoolean(_configuration.GetValue<string>("ShowTripInsuranceSwitch") ?? "false");
                foreach (var anOption in displayCart.TravelOptions)
                {
                    //wade - added check for farelock as we were bypassing it
                    if (!anOption.Type.Equals("Premium Access") && !anOption.Key.Trim().ToUpper().Contains("FARELOCK") && !(addTripInsuranceInTravelOption && anOption.Key.Trim().ToUpper().Contains("TPI")))
                    {
                        continue;
                    }
                    if (ci == null)
                    {
                        ci = TopHelper.GetCultureInfo(anOption.Currency);
                    }

                    Mobile.Model.Shopping.TravelOption travelOption = new Mobile.Model.Shopping.TravelOption();
                    travelOption.Amount = (double)anOption.Amount;

                    travelOption.DisplayAmount = TopHelper.FormatAmountForDisplay(anOption.Amount, ci, false);

                    //??
                    if (anOption.Key.Trim().ToUpper().Contains("FARELOCK") || (addTripInsuranceInTravelOption && anOption.Key.Trim().ToUpper().Contains("TPI")))
                        travelOption.DisplayButtonAmount = TopHelper.FormatAmountForDisplay(anOption.Amount, ci, false);
                    else
                        travelOption.DisplayButtonAmount = TopHelper.FormatAmountForDisplay(anOption.Amount, ci, true);

                    travelOption.CurrencyCode = anOption.Currency;
                    travelOption.Deleted = anOption.Deleted;
                    travelOption.Description = anOption.Description;
                    travelOption.Key = anOption.Key;
                    travelOption.ProductId = anOption.ProductId;
                    travelOption.SubItems = GetTravelOptionSubItems(anOption.SubItems);
                    if (!string.IsNullOrEmpty(anOption.Type))
                    {
                        travelOption.Type = anOption.Type.Equals("Premium Access") ? "Premier Access" : anOption.Type;
                    }
                    travelOptions.Add(travelOption);
                }
            }

            return travelOptions;
        }

        private List<TravelOptionSubItem> GetTravelOptionSubItems(SubitemsCollection subitemsCollection)
        {
            List<TravelOptionSubItem> subItems = null;

            if (subitemsCollection != null && subitemsCollection.Count > 0)
            {
                CultureInfo ci = null;
                subItems = new List<TravelOptionSubItem>();

                foreach (var item in subitemsCollection)
                {
                    if (ci == null)
                    {
                        ci = TopHelper.GetCultureInfo(item.Currency);
                    }

                    TravelOptionSubItem subItem = new TravelOptionSubItem();
                    subItem.Amount = (double)item.Amount;
                    subItem.DisplayAmount = TopHelper.FormatAmountForDisplay(item.Amount, ci, false);
                    subItem.CurrencyCode = item.Currency;
                    subItem.Description = item.Description;
                    subItem.Key = item.Key;
                    subItem.ProductId = item.Type;
                    subItem.Value = item.Value;
                    subItems.Add(subItem);
                }
            }

            return subItems;
        }


        public bool EnableAdvanceSearchCouponBooking(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAdvanceSearchCouponBookingVersion"), _configuration.GetValue<string>("iPhoneAdvanceSearchCouponBookingVersion"));
        }
        public MOBPromoCodeDetails AddAFSPromoCodeDetails(DisplayCart displayCart)
        {
            MOBPromoCodeDetails promoDetails = new MOBPromoCodeDetails();
            promoDetails.PromoCodes = new List<MOBPromoCode>();
            if (isAFSCouponApplied(displayCart))
            {
                var promoOffer = displayCart.SpecialPricingInfo.MerchOfferCoupon;
                promoDetails.PromoCodes.Add(new MOBPromoCode
                {
                    PromoCode = !_configuration.GetValue<bool>("DisableHandlingCaseSenstivity") ? promoOffer.PromoCode.ToUpper().Trim() : promoOffer.PromoCode.Trim(),
                    AlertMessage = promoOffer.Description,
                    IsSuccess = true,
                    TermsandConditions = new MOBMobileCMSContentMessages
                    {
                        HeadLine = _configuration.GetValue<string>("PromoCodeTermsandConditionsTitle"),
                        Title = _configuration.GetValue<string>("PromoCodeTermsandConditionsTitle")
                    },
                    Product = promoOffer.Product
                });
            }
            return promoDetails;
        }
        private bool isAFSCouponApplied(DisplayCart displayCart)
        {
            if (displayCart != null && displayCart.SpecialPricingInfo != null && displayCart.SpecialPricingInfo.MerchOfferCoupon != null && !string.IsNullOrEmpty(displayCart.SpecialPricingInfo.MerchOfferCoupon.PromoCode) && displayCart.SpecialPricingInfo.MerchOfferCoupon.IsCouponEligible.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private RegisterFlightsRequest BuildRegisterFlightRequest(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, string flow, MOBRequest mobRequest)
        {
            RegisterFlightsRequest request = new RegisterFlightsRequest();
            request.CartId = flightReservationResponse.CartId;
            request.CartInfo = flightReservationResponse.DisplayCart;
            request.CountryCode = flightReservationResponse.DisplayCart.CountryCode;//TODO:Check this is populated all the time.
            request.Reservation = flightReservationResponse.Reservation;
            request.DeviceID = mobRequest.DeviceId;
            request.Upsells = flightReservationResponse.Upsells;
            request.MerchOffers = flightReservationResponse.MerchOffers;
            request.LoyaltyUpgradeOffers = flightReservationResponse.LoyaltyUpgradeOffers;
            request.WorkFlowType = _shoppingUtility.GetWorkFlowType(flow);
            return request;
        }

        public List<MOBSHOPPrice> GetPrices(List<DisplayPrice> displayPrices)
        {
            if (displayPrices == null)
                return null;

            var bookingPrices = new List<MOBSHOPPrice>();
            CultureInfo ci = null;
            foreach (var price in displayPrices)
            {
                if (ci == null)
                {
                    ci = TopHelper.GetCultureInfo(price.Currency);
                }

                MOBSHOPPrice bookingPrice = new MOBSHOPPrice();
                bookingPrice.CurrencyCode = price.Currency;
                bookingPrice.DisplayType = price.Type;
                bookingPrice.Status = price.Status;
                bookingPrice.Waived = price.Waived;
                bookingPrice.DisplayValue = string.Format("{0:#,0.00}", price.Amount);

                double tempDouble = 0;
                double.TryParse(price.Amount.ToString(), out tempDouble);
                bookingPrice.Value = Math.Round(tempDouble, 2, MidpointRounding.AwayFromZero);
                bookingPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(price.Amount, ci, false);
                bookingPrices.Add(bookingPrice);
            }

            return bookingPrices;
        }

        public async Task<United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse> GetShopPinDown(Session session, string appVersion, ShopRequest shopRequest)
        {
            if (_configuration.GetValue<bool>("EnableOmniChannelCartMVP1"))
            {
                shopPinDownActionName = "ShopPinDownV2";
            }
            string jsonRequest = JsonConvert.SerializeObject(shopRequest);
            var jsonResponse = await _flightShoppingService.GetShopPinDown(session.Token, shopPinDownActionName, jsonRequest, _headers.ContextValues.TransactionId).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var response = JsonConvert.DeserializeObject<United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse>(jsonResponse);
                if (response != null && !response.Status.Equals(StatusType.Success))
                {
                    if (response != null && response.Errors != null && response.Errors.Count > 0)
                    {
                        var builtErrMsg = string.Join(shopPinDownErrorSeparator, response.Errors.Select(err => string.Format("{0}{1}{2}", err.MajorCode, shopPinDownErrorMajorAndMinorCodesSeparator, err.MinorCode)));

                        throw new Exception(builtErrMsg);
                    }

                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                return response;
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

        }

        public ShopRequest BuildShopPinDownDetailsRequest(MOBSHOPSelectUnfinishedBookingRequest request, string cartId = "")
        {

            var shopRequest = (_shoppingUtility.EnableTravelerTypes(request.Application.Id, request.Application.Version.Major) && request.SelectedUnfinishBooking.TravelerTypes != null) ?
                            BuildShopPinDownRequest(request.SelectedUnfinishBooking, request.MileagePlusAccountNumber, request.LanguageCode, request.Application.Id, request.Application.Version.Major, true) :
                            BuildShopPinDownRequest(request.SelectedUnfinishBooking, request.MileagePlusAccountNumber, request.LanguageCode);


            shopRequest.DisablePricingBySlice = _shoppingUtility.EnableRoundTripPricing(request.Application.Id, request.Application.Version.Major);
            shopRequest.DecodesOnTimePerfRequested = _configuration.GetValue<bool>("DecodesOnTimePerformance");
            shopRequest.DecodesRequested = _configuration.GetValue<bool>("DecodesRequested");
            shopRequest.IncludeAmenities = _configuration.GetValue<bool>("IncludeAmenities");
            shopRequest.CartId = cartId;
            if (_configuration.GetValue<bool>("EnableOmniChannelCartMVP1"))
            {
                shopRequest.DeviceId = request.DeviceId;
                shopRequest.LoyaltyPerson = new Service.Presentation.PersonModel.LoyaltyPerson();
                shopRequest.LoyaltyPerson.LoyaltyProgramMemberID = request.MileagePlusAccountNumber;
            }
            return shopRequest;
        }

        public ShopRequest BuildShopPinDownRequest(MOBSHOPUnfinishedBooking unfinishedBooking, string mpNumber, string languageCode, int appID = -1, string appVer = "", bool isCatalogOnForTravelerTypes = false)
        {
            var shopRequest = new ShopRequest
            {
                AccessCode = _configuration.GetValue<string>("AccessCode - CSLShopping"),
                ChannelType = _configuration.GetValue<string>("Shopping - ChannelType"),
                CountryCode = unfinishedBooking.CountryCode,
                InclCancelledFlights = false,
                InclOAMain = true,
                InclStarMain = true,
                InclUACodeshares = true,
                InclUAMain = true,
                InclUARegionals = true,
                InitialShop = true,
                LangCode = languageCode,
                StopsInclusive = true,
                LoyaltyId = mpNumber,
                RememberedLoyaltyId = mpNumber,
                TrueAvailability = true,
                UpgradeComplimentaryRequested = true,
                SearchTypeSelection = GetSearchType(unfinishedBooking.SearchType),
                Trips = GetTripsForShopPinDown(unfinishedBooking),
            };

            Func<PaxType, int, PaxInfo> getPax = (type, subtractYear) => new PaxInfo { PaxType = type, DateOfBirth = DateTime.Today.AddYears(subtractYear).ToShortDateString() };
            shopRequest.PaxInfoList = new List<PaxInfo>();

            if (_shoppingUtility.EnableTravelerTypes(appID, appVer) && isCatalogOnForTravelerTypes)
            {
                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Adult.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Adult.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Adult.ToString()).Count).Select(x => getPax(PaxType.Adult, -20)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Senior.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Senior.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Senior.ToString()).Count).Select(x => getPax(PaxType.Senior, -67)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child15To17.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child15To17.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child15To17.ToString()).Count).Select(x => getPax(PaxType.Child05, -16)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child12To14.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To14.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To14.ToString()).Count).Select(x => getPax(PaxType.Child04, -13)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child5To11.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).Count).Select(x => getPax(PaxType.Child02, -8)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child2To4.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).Count).Select(x => getPax(PaxType.Child01, -3)));

                /*
                 * commented as we are not using the below code
                 * if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child12To17.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To17.ToString()).Count > 0)
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To17.ToString()).Count).Select(x => getPax(PaxType.Child03, -15))); */

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).Count).Select(x => getPax(PaxType.InfantSeat, -1)));

                if (unfinishedBooking.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantLap.ToString()) && unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).Count > 0)
                    shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).Count).Select(x => getPax(PaxType.InfantLap, -1)));
            }
            else
            {
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfAdults).Select(x => getPax(PaxType.Adult, -20)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfSeniors).Select(x => getPax(PaxType.Senior, -67)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfChildren2To4).Select(x => getPax(PaxType.Child01, -3)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfChildren5To11).Select(x => getPax(PaxType.Child02, -8)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfChildren12To17).Select(x => getPax(PaxType.Child03, -15)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfInfantOnLap).Select(x => getPax(PaxType.InfantLap, -1)));
                shopRequest.PaxInfoList.AddRange(Enumerable.Range(1, unfinishedBooking.NumberOfInfantWithSeat).Select(x => getPax(PaxType.InfantSeat, -1)));
            }

            return shopRequest;
        }

        private List<Trip> GetTripsForShopPinDown(MOBSHOPUnfinishedBooking unfinishedBooking)
        {
            List<Trip> trips = new List<Trip>();

            foreach (var mobTrip in unfinishedBooking.Trips)
            {
                var trip = new Trip
                {
                    DepartDate = mobTrip.DepartDate,
                    DepartTime = mobTrip.DepartTime,
                    Destination = mobTrip.Destination,
                    Origin = mobTrip.Origin,
                    FlightCount = mobTrip.Flights.Count(),
                    Flights = mobTrip.Flights.Select(MapToFlightShoppingFlight).ToList(),
                    SearchFiltersIn = new SearchFilterInfo(),
                    SearchFiltersOut = new SearchFilterInfo(),
                };

                // MB-3052 Combine COG flights for price variance in saved trips
                _shoppingUtility.GetFlattedFlightsForCOGorThruFlights(trip);
                trips.Add(trip);
            }
            return trips;
        }
        private Flight MapToFlightShoppingFlight(MOBSHOPUnfinishedBookingFlight ubFlight)
        {
            var flight = new Flight
            {
                Aircraft = new Aircraft(),
                EquipmentDisclosures = new EquipmentDisclosure(),
                FlightInfo = new FlightInfo(),
                BookingCode = ubFlight.BookingCode,
                DepartDateTime = ubFlight.DepartDateTime,
                Destination = ubFlight.Destination,
                Origin = ubFlight.Origin,
                FlightNumber = ubFlight.FlightNumber,
                MarketingCarrier = ubFlight.MarketingCarrier,
            };
            if (ubFlight.Products != null && ubFlight.Products.Count > 0)
            {
                flight.Products = new ProductCollection();

                foreach (var product in ubFlight.Products)
                {
                    if (product != null && !String.IsNullOrEmpty(product.ProductType))
                    {
                        flight.Products.Add(MapToFlightShoppingFlightProduct(product));
                    }
                }
            }

            if (ubFlight.Connections == null)
                return flight;

            ubFlight.Connections.ForEach(x => flight.Connections.Add(MapToFlightShoppingFlight(x)));

            return flight;
        }

        private United.Services.FlightShopping.Common.Product MapToFlightShoppingFlightProduct(MOBSHOPUnfinishedBookingFlightProduct ubFlightProduct)
        {
            if (ubFlightProduct != null)
            {
                var product = new United.Services.FlightShopping.Common.Product
                {
                    BookingCode = ubFlightProduct.BookingCode,
                    ProductType = ubFlightProduct.ProductType,
                    TripIndex = ubFlightProduct.TripIndex,
                };
                if (ubFlightProduct.Prices?.Count > 0)
                {
                    product.Prices = new List<PricingItem>();
                    foreach (var price in ubFlightProduct.Prices)
                    {
                        if (price != null && !String.IsNullOrEmpty(price.PricingType))
                        {
                            product.Prices.Add(MapToFlightShoppingFlightProductPrice(price));
                        }
                    }
                }
                return product;
            }
            return null;
        }

        private PricingItem MapToFlightShoppingFlightProductPrice(MOBSHOPUnfinishedBookingProductPrice ubProductPrice)
        {
            if (ubProductPrice != null)
            {
                PricingItem price = new PricingItem
                {
                    Amount = ubProductPrice.Amount,
                    AmountAllPax = ubProductPrice.AmountAllPax,
                    AmountBase = ubProductPrice.AmountBase,
                    Currency = ubProductPrice.Currency,
                    CurrencyAllPax = ubProductPrice.CurrencyAllPax,
                    OfferID = ubProductPrice.OfferID,
                    PricingType = ubProductPrice.PricingType,
                    Selected = ubProductPrice.Selected,
                    MerchPriceDetail = new MerchPriceDetail { EDDCode = ubProductPrice.MerchPriceDetail?.EddCode, ProductCode = ubProductPrice.MerchPriceDetail?.ProductCode }
                };
                if (ubProductPrice.SegmentMappings?.Count > 0)
                {
                    price.SegmentMappings = new List<SegmentMapping>();
                    foreach (var segmentMapping in ubProductPrice.SegmentMappings)
                    {
                        if (String.IsNullOrEmpty(segmentMapping?.SegmentRefID))
                        {
                            price.SegmentMappings.Add(MapToFlightShoppingProductSegmentMapping(segmentMapping));
                        }
                    }
                }
                return price;
            }
            return null;
        }

        private SegmentMapping MapToFlightShoppingProductSegmentMapping(MOBSHOPUnfinishedBookingProductSegmentMapping ubSegmentMapping)
        {
            if (ubSegmentMapping != null && !String.IsNullOrEmpty(ubSegmentMapping.SegmentRefID))
            {
                SegmentMapping segmentMapping = new SegmentMapping
                {
                    Origin = ubSegmentMapping.Origin,
                    Destination = ubSegmentMapping.Destination,
                    BBxHash = ubSegmentMapping.BBxHash,
                    UpgradeStatus = ubSegmentMapping.UpgradeStatus,
                    UpgradeTo = ubSegmentMapping.UpgradeTo,
                    FlightNumber = ubSegmentMapping.FlightNumber,
                    CabinDescription = ubSegmentMapping.CabinDescription,
                    SegmentRefID = ubSegmentMapping.SegmentRefID
                };
                return segmentMapping;
            }
            return null;
        }

        private SearchType GetSearchType(string searchTypeSelection)
        {
            SearchType searchType = SearchType.ValueNotSet;

            if (string.IsNullOrEmpty(searchTypeSelection))
                return searchType;

            switch (searchTypeSelection.Trim().ToUpper())
            {
                case "OW":
                    searchType = SearchType.OneWay;
                    break;
                case "RT":
                    searchType = SearchType.RoundTrip;
                    break;
                case "MD":
                    searchType = SearchType.MultipleDestination;
                    break;
                default:
                    searchType = SearchType.ValueNotSet;
                    break;
            }

            return searchType;
        }

        //added
        public async Task<bool> SaveAnUnfinishedBooking(Session session, MOBRequest request, MOBSHOPUnfinishedBooking ub)
        {
            var savedUBs = await GetSavedUnfinishedBookingEntries(session, request, ub.TravelerTypes != null);
            MOBSHOPUnfinishedBooking foundUB = null;
            if (savedUBs != null && savedUBs.Any() && (foundUB = savedUBs.FirstOrDefault(x => comparer.Equals(x, ub))) != null)
            {
                ub.Id = foundUB.Id;
                return await UpdateAnUnfinishedBooking(session, request, ub);
            }

            return await InsertAnUnfinishedBooking(session, request, ub);
        }

        public async Task<List<MOBSHOPUnfinishedBooking>> GetSavedUnfinishedBookingEntries(Session session, MOBRequest request, bool isCatalogOnForTravelerTypes = false)
        {
            if (session.CustomerID <= 0)
                return new List<MOBSHOPUnfinishedBooking>();

            string cslActionName = "SavedItinerary(Get)";

            string token = await _dPService.GetAnonymousToken(_headers.ContextValues.Application.Id, _headers.ContextValues.DeviceId, _configuration);
            var csLCallDurationstopwatch = new Stopwatch();
            csLCallDurationstopwatch.Start();

            var response = await _customerPreferencesService.GetCustomerPrefernce<SavedItineraryDataModel>(token, cslActionName, savedUnfinishedBookingActionName, savedUnfinishedBookingAugumentName, session.CustomerID, session.SessionId);

            if (csLCallDurationstopwatch.IsRunning)
            {
                csLCallDurationstopwatch.Stop();
            }

            if (response != null)
            {

                if (response != null && response.Status.Equals(PreferencesConstants.StatusType.Success))
                {
                    List<MOBSHOPUnfinishedBooking> unfinishedBookings = new List<MOBSHOPUnfinishedBooking>();
                    if (response.SavedItineraryList != null)
                    {
                        unfinishedBookings = response.SavedItineraryList.Select(x => (_shoppingUtility.EnableTravelerTypes(request.Application.Id, request.Application.Version.Major) && isCatalogOnForTravelerTypes) ? MapToMOBSHOPUnfinishedBookingTtypes(x, request)
                        : MapToMOBSHOPUnfinishedBooking(x, request)).ToList();
                    }
                    return unfinishedBookings;
                }
                else
                {
                    if (response != null && response.Errors != null && response.Errors.Count > 0)
                    {
                        throw new Exception(string.Join(" ", response.Errors.Select(err => err.Message)));
                    }

                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }

        private MOBSHOPUnfinishedBooking MapToMOBSHOPUnfinishedBooking(GetSavedItineraryDataModel cslEntry, MOBRequest request)
        {
            var ub = cslEntry.SavedItinerary;
            var mobEntry = new MOBSHOPUnfinishedBooking
            {
                SearchExecutionDate = new[] { cslEntry.InsertTimestamp, cslEntry.UpdateTimestamp }.FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                NumberOfAdults = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.Adult),
                NumberOfSeniors = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.Senior),
                NumberOfChildren2To4 = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.Child01),
                NumberOfChildren5To11 = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.Child02),
                NumberOfChildren12To17 = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.Child03),
                NumberOfInfantOnLap = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.InfantLap),
                NumberOfInfantWithSeat = ub.PaxInfoList.Count(px => (PaxType)px.PaxType == PaxType.InfantSeat),
                CountryCode = ub.CountryCode,
                SearchType = GetSeachTypeSelection((SearchType)ub.SearchTypeSelection),
                Trips = ub.Trips.Select(MapToMOBSHOPUnfinishedBookingTrip).ToList(),
                Id = ub.AccessCode
            };

            if (_shoppingUtility.EnableSavedTripShowChannelTypes(request.Application.Id, request.Application.Version.Major)) // Map channel
                mobEntry.ChannelType = ub.ChannelType;

            return mobEntry;
        }

        private MOBSHOPUnfinishedBookingTrip MapToMOBSHOPUnfinishedBookingTrip(SerializableSavedItinerary.Trip csTrip)
        {
            return new MOBSHOPUnfinishedBookingTrip
            {
                DepartDate = csTrip.DepartDate,
                DepartTime = csTrip.DepartTime,
                Destination = csTrip.Destination,
                Origin = csTrip.Origin,
                Flights = csTrip.Flights.Select(MapToMOBSHOPUnfinishedBookingFlight).ToList()
            };
        }

        private MOBSHOPUnfinishedBookingFlight MapToMOBSHOPUnfinishedBookingFlight(SerializableSavedItinerary.Flight cslFlight)
        {
            var ubMOBFlight = new MOBSHOPUnfinishedBookingFlight
            {
                BookingCode = cslFlight.BookingCode,
                DepartDateTime = cslFlight.DepartDateTime,
                Origin = cslFlight.Origin,
                Destination = cslFlight.Destination,
                FlightNumber = cslFlight.FlightNumber,
                MarketingCarrier = cslFlight.MarketingCarrier,
                ProductType = cslFlight.ProductType,
            };

            if (ubMOBFlight.Connections == null)
                return ubMOBFlight;

            cslFlight.Connections.ForEach(x => ubMOBFlight.Connections.Add(MapToMOBSHOPUnfinishedBookingFlight(x)));

            return ubMOBFlight;
        }

        private string GetSeachTypeSelection(SearchType searchType)
        {
            var result = string.Empty;
            try
            {
                return new Dictionary<SearchType, string>
                {
                    {SearchType.OneWay, "OW"},
                    {SearchType.RoundTrip, "RT"},
                    {SearchType.MultipleDestination, "MD"},
                    {SearchType.ValueNotSet, string.Empty},
                }[searchType];
            }
            catch { return result; }
        }

        private MOBSHOPUnfinishedBooking MapToMOBSHOPUnfinishedBookingTtypes(United.Services.Customer.Preferences.Common.GetSavedItineraryDataModel cslEntry, MOBRequest request)
        {
            var ub = cslEntry.SavedItinerary;
            MOBSHOPUnfinishedBooking mobEntry = new MOBSHOPUnfinishedBooking();

            mobEntry.SearchExecutionDate = new[] { cslEntry.InsertTimestamp, cslEntry.UpdateTimestamp }.FirstOrDefault(x => !string.IsNullOrEmpty(x));
            mobEntry.TravelerTypes = new List<MOBTravelerType>();
            GetTravelTypesFromShop(ub, mobEntry);
            mobEntry.CountryCode = ub.CountryCode;
            mobEntry.SearchType = GetSeachTypeSelection((SearchType)ub.SearchTypeSelection);
            mobEntry.Trips = ub.Trips.Select(MapToMOBSHOPUnfinishedBookingTrip).ToList();
            mobEntry.Id = ub.AccessCode;
            if (_shoppingUtility.EnableSavedTripShowChannelTypes(request.Application.Id, request.Application.Version.Major)) // Map channel
                mobEntry.ChannelType = ub.ChannelType;

            return mobEntry;
        }

        private static void GetTravelTypesFromShop(SerializableSavedItinerary ub, MOBSHOPUnfinishedBooking mobEntry)
        {
            foreach (var t in ub.PaxInfoList.GroupBy(p => p.PaxType))
            {
                switch ((int)t.Key)
                {
                    case (int)PaxType.Adult:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Adult.ToString() });
                        break;

                    case (int)PaxType.Senior:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Senior.ToString() });
                        break;

                    case (int)PaxType.Child01:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Child2To4.ToString() });
                        break;

                    case (int)PaxType.Child02:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Child5To11.ToString() });
                        break;

                    case (int)PaxType.Child03:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Child12To17.ToString() });
                        break;

                    case (int)PaxType.Child04:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Child12To14.ToString() });
                        break;

                    case (int)PaxType.Child05:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Child15To17.ToString() });
                        break;

                    case (int)PaxType.InfantSeat:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.InfantSeat.ToString() });
                        break;

                    case (int)PaxType.InfantLap:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.InfantLap.ToString() });
                        break;
                    default:
                        mobEntry.TravelerTypes.Add(new MOBTravelerType() { Count = t.Count(), TravelerType = PAXTYPE.Adult.ToString() });
                        break;
                }
            }
        }

        public async Task<bool> UpdateAnUnfinishedBooking(Session session, MOBRequest request, MOBSHOPUnfinishedBooking ubTobeUpdated)
        {
            string cslActionName = "SavedItinerary(Put)";

            var cslRequest = new United.Services.Customer.Preferences.Common.UpdateSavedItineraryData
            {
                UpdateID = session.DeviceID,
                UpdateSavedItinerary = (_shoppingUtility.EnableTravelerTypes(request.Application.Id, request.Application.Version.Major) && ubTobeUpdated.TravelerTypes != null) ?
                MapToSerializableSavedItineraryTtypes(ubTobeUpdated, request.LanguageCode, session.MileagPlusNumber) : MapToSerializableSavedItinerary(ubTobeUpdated, request.LanguageCode, session.MileagPlusNumber)
            };

            string jsonRequest = JsonConvert.SerializeObject(cslRequest);

            string token = await _dPService.GetAnonymousToken(_headers.ContextValues.Application.Id, _headers.ContextValues.DeviceId, _configuration).ConfigureAwait(false);

            var cSLCallDurationstopwatch = new Stopwatch();
            cSLCallDurationstopwatch.Start();

            var response = await _customerPreferencesService.GetUnfinishedCustomerPrefernce<SubCommonData>(token, cslActionName, savedUnfinishedBookingActionName, savedUnfinishedBookingAugumentName, session.CustomerID, session.SessionId, jsonRequest).ConfigureAwait(false);
            if (cSLCallDurationstopwatch.IsRunning)
            {
                cSLCallDurationstopwatch.Stop();
            }

            if (response != null)
            {

                if (response != null && !response.Status.Equals(PreferencesConstants.StatusType.Success))
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        throw new Exception(string.Join(" ", response.Errors.Select(err => err.Message)));
                    }

                    throw new MOBUnitedException("Failed to update an unfinished booking.");
                }
            }
            else
            {
                throw new MOBUnitedException("Failed to update an unfinished booking.");
            }

            return true;

        }

        private async Task<bool> InsertAnUnfinishedBooking(Session session, MOBRequest request, MOBSHOPUnfinishedBooking ubTobeInserted)
        {
            string cslActionName = "SavedItinerary(Post)";

            string token = await _dPService.GetAnonymousToken(_headers.ContextValues.Application.Id, _headers.ContextValues.DeviceId, _configuration).ConfigureAwait(false);


            var cslRequest = new InsertSavedItineraryData
            {
                InsertID = session.DeviceID,
                InsertSavedItinerary = (_shoppingUtility.EnableTravelerTypes(request.Application.Id, request.Application.Version.Major) && ubTobeInserted.TravelerTypes != null) ?
                MapToSerializableSavedItineraryTtypes(ubTobeInserted, request.LanguageCode, session.MileagPlusNumber) : MapToSerializableSavedItinerary(ubTobeInserted, request.LanguageCode, session.MileagPlusNumber)
            };

            //// Ensure access code is empty for the new one to be inserted. Only the existing one has access code
            //cslRequest.InsertSavedItinerary.AccessCode = null;

            string jsonRequest = JsonConvert.SerializeObject(cslRequest);

            var cSLCallDurationstopwatch = new Stopwatch();
            cSLCallDurationstopwatch.Start();

            var response = await _customerPreferencesService.GetUnfinishedCustomerPrefernce<SubCommonData>(token, cslActionName, savedUnfinishedBookingActionName, savedUnfinishedBookingAugumentName, session.CustomerID, session.SessionId, jsonRequest).ConfigureAwait(false);

            if (cSLCallDurationstopwatch.IsRunning)
            {
                cSLCallDurationstopwatch.Stop();
            }

            if (response != null)
            {

                if (response != null && !response.Status.Equals(PreferencesConstants.StatusType.Success))
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        throw new Exception(string.Join(" ", response.Errors.Select(err => err.Message)));
                    }

                    throw new MOBUnitedException("Failed to insert an unfinished booking.");
                }
            }
            else
            {
                throw new MOBUnitedException("Failed to insert an unfinished booking.");
            }

            return true;

        }

        private SerializableSavedItinerary MapToSerializableSavedItineraryTtypes(MOBSHOPUnfinishedBooking ub, string languageCode, string mpNumber)
        {
            var cslUB = new SerializableSavedItinerary
            {
                AccessCode = ub.Id,
                AwardTravel = false,
                ChannelType = _configuration.GetValue<string>("Shopping - ChannelType"),
                CountryCode = ub.CountryCode,
                InitialShop = true,
                LangCode = languageCode,
                LoyaltyId = mpNumber,
                NGRP = false,
                SearchTypeSelection = (SerializableSavedItinerary.SearchType)GetSearchType(ub.SearchType),
                Trips = MapToListOfCSLUnfinishedBookingTrips(ub.Trips),
                TrueAvailability = true
            };

            Func<PaxType, int, SerializableSavedItinerary.PaxInfo> getPax = (type, subtractYear) => new SerializableSavedItinerary.PaxInfo { PaxType = (SerializableSavedItinerary.PaxType)type, DateOfBirth = DateTime.Today.AddYears(subtractYear).ToShortDateString() };
            cslUB.PaxInfoList = new List<SerializableSavedItinerary.PaxInfo>();
            GetPaxInfoUnfinishBooking(ub, cslUB, getPax);

            return cslUB;
        }

        private List<SerializableSavedItinerary.Trip> MapToListOfCSLUnfinishedBookingTrips(List<MOBSHOPUnfinishedBookingTrip> trips)
        {
            if (trips == null)
                return null;

            var clsTrips = new List<SerializableSavedItinerary.Trip>();
            if (!trips.Any())
                return clsTrips;

            foreach (var mobTrip in trips)
            {
                var trip = new SerializableSavedItinerary.Trip
                {
                    DepartDate = mobTrip.DepartDate,
                    DepartTime = mobTrip.DepartTime,
                    Destination = mobTrip.Destination,
                    Origin = mobTrip.Origin,
                    FlightCount = mobTrip.Flights.Count(),
                    Flights = mobTrip.Flights.Select(MapToCSLUnfinishedBookingFlight).ToList(),
                };

                clsTrips.Add(trip);
            }

            return clsTrips;
        }

        private SerializableSavedItinerary.Flight MapToCSLUnfinishedBookingFlight(MOBSHOPUnfinishedBookingFlight ubFlight)
        {
            var flight = new SerializableSavedItinerary.Flight
            {
                BookingCode = ubFlight.BookingCode,
                DepartDateTime = ubFlight.DepartDateTime,
                Destination = ubFlight.Destination,
                Origin = ubFlight.Origin,
                FlightNumber = ubFlight.FlightNumber,
                MarketingCarrier = ubFlight.MarketingCarrier,
                ProductType = ubFlight.ProductType
            };

            if (ubFlight.Connections == null)
                return flight;

            ubFlight.Connections.ForEach(x => flight.Connections.Add(MapToCSLUnfinishedBookingFlight(x)));

            return flight;
        }

        private SerializableSavedItinerary MapToSerializableSavedItinerary(MOBSHOPUnfinishedBooking ub, string languageCode, string mpNumber)
        {
            var cslUB = new SerializableSavedItinerary
            {
                AccessCode = ub.Id,
                AwardTravel = false,
                ChannelType = _configuration.GetValue<string>("Shopping - ChannelType"),
                CountryCode = ub.CountryCode,
                InitialShop = true,
                LangCode = languageCode,
                LoyaltyId = mpNumber,
                NGRP = false,
                SearchTypeSelection = (SerializableSavedItinerary.SearchType)GetSearchType(ub.SearchType),
                Trips = MapToListOfCSLUnfinishedBookingTrips(ub.Trips),
                TrueAvailability = true
            };

            Func<PaxType, int, SerializableSavedItinerary.PaxInfo> getPax = (type, subtractYear) => new SerializableSavedItinerary.PaxInfo { PaxType = (SerializableSavedItinerary.PaxType)type, DateOfBirth = DateTime.Today.AddYears(subtractYear).ToShortDateString() };
            cslUB.PaxInfoList = new List<SerializableSavedItinerary.PaxInfo>();
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfAdults).Select(x => getPax(PaxType.Adult, -20)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfSeniors).Select(x => getPax(PaxType.Senior, -67)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfChildren2To4).Select(x => getPax(PaxType.Child01, -3)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfChildren5To11).Select(x => getPax(PaxType.Child02, -8)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfChildren12To17).Select(x => getPax(PaxType.Child03, -15)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfInfantOnLap).Select(x => getPax(PaxType.InfantLap, -1)));
            cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.NumberOfInfantWithSeat).Select(x => getPax(PaxType.InfantSeat, -1)));

            return cslUB;
        }

        private void GetPaxInfoUnfinishBooking(MOBSHOPUnfinishedBooking ub, SerializableSavedItinerary cslUB, Func<PaxType, int, SerializableSavedItinerary.PaxInfo> getPax)
        {
            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Adult.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Adult.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Adult.ToString()).Count).Select(x => getPax(PaxType.Adult, -20)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Senior.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Senior.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Senior.ToString()).Count).Select(x => getPax(PaxType.Senior, -67)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child2To4.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).Count).Select(x => getPax(PaxType.Child01, -3)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child5To11.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).Count).Select(x => getPax(PaxType.Child02, -8)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child12To17.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To17.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To17.ToString()).Count).Select(x => getPax(PaxType.Child03, -15)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child12To14.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To14.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child12To14.ToString()).Count).Select(x => getPax(PaxType.Child04, -13)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child15To17.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child15To17.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.Child15To17.ToString()).Count).Select(x => getPax(PaxType.Child05, -16)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantLap.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).Count).Select(x => getPax(PaxType.InfantLap, -1)));

            if (ub.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()) && ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).Count > 0)
                cslUB.PaxInfoList.AddRange(Enumerable.Range(1, ub.TravelerTypes.First(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).Count).Select(x => getPax(PaxType.InfantSeat, -1)));
        }
        #region UnfinishedBooking methods


        public async Task<TravelSpecialNeeds> GetItineraryAvailableSpecialNeeds(Session session, int appId, string appVersion, string deviceId, IEnumerable<ReservationFlightSegment> segments, string languageCode
            , MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null)
        {
            MultiCallResponse flightshoppingReferenceData = null;
            IEnumerable<ReservationFlightSegment> pnrOfferedMeals = null;
            var offersSSR = new TravelSpecialNeeds();

            try
            {
                //Parallel.Invoke(() => flightshoppingReferenceData = GetSpecialNeedsReferenceDataFromFlightShopping(session, appId, appVersion, deviceId, languageCode),
                //                () => pnrOfferedMeals = GetOfferedMealsForItineraryFromPNRManagement(session, appId, appVersion, deviceId, segments));
                flightshoppingReferenceData = await GetSpecialNeedsReferenceDataFromFlightShopping(session, appId, appVersion, deviceId, languageCode);
                pnrOfferedMeals = await GetOfferedMealsForItineraryFromPNRManagement(session, appId, appVersion, deviceId, segments);
            }
            catch (Exception) // 'System.ArgumentException' is thrown when any action in the actions array throws an exception.
            {
                if (flightshoppingReferenceData == null) // unable to get reference data, POPULATE DEFAULT SPECIAL REQUESTS
                {
                    offersSSR.ServiceAnimalsMessages = new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSR_RefDataServiceFailure_ServiceAnimalMassage") } };

                    flightshoppingReferenceData = GetMultiCallResponseWithDefaultSpecialRequests();
                }
                else if (pnrOfferedMeals == null) // unable to get market restriction meals, POPULATE DEFAULT MEALS
                {
                    pnrOfferedMeals = PopulateSegmentsWithDefaultMeals(segments);
                }
            }

            //Parallel.Invoke(() => offersSSR.SpecialMeals = GetOfferedMealsForItinerary(pnrOfferedMeals, flightshoppingReferenceData),
            //                () => offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData),
            //                () => offersSSR.SpecialRequests = GetOfferedSpecialRequests(flightshoppingReferenceData),
            //                () => offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData),
            //                () => offersSSR.ServiceAnimals = GetOfferedServiceAnimals(flightshoppingReferenceData, segments, appId, appVersion));

            offersSSR.SpecialMeals = GetOfferedMealsForItinerary(pnrOfferedMeals, flightshoppingReferenceData);
            offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData);
            offersSSR.SpecialRequests = await GetOfferedSpecialRequests(flightshoppingReferenceData, reservation, selectRequest, session);
            offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData);
            offersSSR.ServiceAnimals = GetOfferedServiceAnimals(flightshoppingReferenceData, segments, appId, appVersion);

            _logger.LogInformation("{@SpecialMeals}", JsonConvert.SerializeObject(offersSSR.SpecialMeals));

            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("RemoveEmotionalSupportServiceAnimalOption_EffectiveDateTime"))
                && Convert.ToDateTime(_configuration.GetValue<string>("RemoveEmotionalSupportServiceAnimalOption_EffectiveDateTime")) <= DateTime.Now
                && offersSSR.ServiceAnimals != null && offersSSR.ServiceAnimals.Any())
            {
                offersSSR.ServiceAnimals.Remove(offersSSR.ServiceAnimals.FirstOrDefault(x => x.Code == "ESAN" && x.Value == "6"));
            }
            if (IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion)
                 && offersSSR?.ServiceAnimals != null && offersSSR.ServiceAnimals.Any(x => x.Code == "ESAN" && x.Value == "5"))
            {
                offersSSR.ServiceAnimals.Remove(offersSSR.ServiceAnimals.FirstOrDefault(x => x.Code == "ESAN" && x.Value == "5"));
            }

            await AddServiceAnimalsMessageSection(offersSSR, appId, appVersion, session, deviceId);

            if (offersSSR.ServiceAnimalsMessages == null || !offersSSR.ServiceAnimalsMessages.Any())
                offersSSR.ServiceAnimalsMessages = GetServiceAnimalsMessages(offersSSR.ServiceAnimals);
            if (_configuration.GetValue<bool>("EnableAirlinesFareComparison") && session.CatalogItems != null && session.CatalogItems.Count > 0 &&
                   session.CatalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableNewPartnerAirlines).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableNewPartnerAirlines).ToString())?.CurrentValue == "1"
                  && reservation.Trips?.FirstOrDefault()?.FlattenedFlights?.FirstOrDefault()?.Flights?.FirstOrDefault().OperatingCarrier != null
                    && _configuration.GetValue<string>("SupportedAirlinesFareComparison").Contains(reservation.Trips?.FirstOrDefault()?.FlattenedFlights?.FirstOrDefault()?.Flights?.FirstOrDefault().OperatingCarrier.ToUpper())
                   )
            {

                offersSSR.SpecialNeedsAlertMessages = new Mobile.Model.Common.MOBAlertMessages
                {
                    HeaderMessage = _configuration.GetValue<string>("PartnerAirlinesSpecialTravelNeedsHeader"),
                    IsDefaultOption = true,
                    MessageType = MOBINFOWARNINGMESSAGEICON.WARNING.ToString(),
                    AlertMessages = new List<MOBSection>
                    {
                        new MOBSection
                        {
                            MessageType = MOBINFOWARNINGMESSAGEICON.WARNING.ToString(),
                            Text1 = _configuration.GetValue<string>("PartnerAirlinesSpecialTravelNeedsMessage"),
                            Order = "1"
                        }
                    }
                };
            }

            return offersSSR;
        }
        private IEnumerable<ReservationFlightSegment> PopulateSegmentsWithDefaultMeals(IEnumerable<ReservationFlightSegment> segments)
        {
            var pnrOfferedMeals = GetOfferedMealsForItineraryFromPNRManagementRequest(segments);
            pnrOfferedMeals.Where(x => x.FlightSegment != null && x.FlightSegment.IsInternational.Equals("True", StringComparison.OrdinalIgnoreCase))
                           .ToList()
                           .ForEach(x => x.FlightSegment.Characteristic = new Collection<Service.Presentation.CommonModel.Characteristic> { new Service.Presentation.CommonModel.Characteristic {
                                       Code = "SPML",
                                       Description = "Default meals when service is down",
                                       Value = _configuration.GetValue<string>("SSR_DefaultMealCodes")
                                   } });

            return pnrOfferedMeals;
        }

        private MultiCallResponse GetMultiCallResponseWithDefaultSpecialRequests()
        {
            try
            {
                return new MultiCallResponse
                {
                    SpecialRequestResponses = new Collection<SpecialRequestResponse>
                    {
                        new SpecialRequestResponse
                        {
                            SpecialRequests = new Collection<Service.Presentation.CommonModel.Characteristic> (_configuration.GetValue<string>("SSR_DefaultSpecialRequests")
                                                                                                .Split('|')
                                                                                                .Select(request => request.Split('^'))
                                                                                                .Select(request => new Service.Presentation.CommonModel.Characteristic
                                                                                                {
                                                                                                    Code = request[0],
                                                                                                    Description = request[1],
                                                                                                    Genre = new Service.Presentation.CommonModel.Genre { Description = request[2]},
                                                                                                    Value = request[3]
                                                                                                })
                                                                                                .ToList())
                        }
                    }
                };
            }
            catch
            {
                return null;
            }
        }

        private async Task<IEnumerable<ReservationFlightSegment>> GetOfferedMealsForItineraryFromPNRManagement(Session session, int appId, string appVersion, string deviceId, IEnumerable<ReservationFlightSegment> segments)
        {
            string cslActionName = "/SpecialMeals/FlightSegments";

            string jsonRequest = JsonConvert.SerializeObject(GetOfferedMealsForItineraryFromPNRManagementRequest(segments));

            string token = await _dPService.GetAnonymousToken(_headers.ContextValues.Application.Id, _headers.ContextValues.DeviceId, _configuration).ConfigureAwait(false);

            var cslCallDurationstopwatch = new Stopwatch();
            cslCallDurationstopwatch.Start();

            var response = await _pNRRetrievalService.GetOfferedMealsForItinerary<List<ReservationFlightSegment>>(token, cslActionName, jsonRequest, session.SessionId).ConfigureAwait(false);

            if (cslCallDurationstopwatch.IsRunning)
            {
                cslCallDurationstopwatch.Stop();
            }

            if (response != null)
            {
                if (response == null)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                if (response.Count > 0)
                {
                    await _sessionHelperService.SaveSession<List<ReservationFlightSegment>>(response, session.SessionId, new List<string> { session.SessionId, new ReservationFlightSegment().GetType().FullName }, new ReservationFlightSegment().GetType().FullName).ConfigureAwait(false);//, response[0].GetType().FullName);
                }

                return response;
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }
        private IEnumerable<ReservationFlightSegment> GetOfferedMealsForItineraryFromPNRManagementRequest(IEnumerable<ReservationFlightSegment> segments)
        {
            if (segments == null || !segments.Any())
                return new List<ReservationFlightSegment>();

            return segments.Select(segment => new ReservationFlightSegment
            {
                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment
                {
                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport { IATACode = segment.FlightSegment.ArrivalAirport.IATACode },
                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport { IATACode = segment.FlightSegment.DepartureAirport.IATACode },
                    DepartureDateTime = segment.FlightSegment.DepartureDateTime,
                    FlightNumber = segment.FlightSegment.FlightNumber,
                    InstantUpgradable = false,
                    IsInternational = segment.FlightSegment.IsInternational,
                    OperatingAirlineCode = segment.FlightSegment.OperatingAirlineCode,
                    UpgradeEligibilityStatus = Service.Presentation.CommonEnumModel.UpgradeEligibilityStatus.Unknown,
                    UpgradeVisibilityType = Service.Presentation.CommonEnumModel.UpgradeVisibilityType.None,
                    BookingClasses = new Collection<BookingClass>(segment.FlightSegment.BookingClasses.Where(y => y != null && y.Cabin != null).Select(y => new BookingClass { Cabin = new Service.Presentation.CommonModel.AircraftModel.Cabin { Name = y.Cabin.Name }, Code = y.Code }).ToList())
                }
            }).ToList();
        }
        private async Task<MOBMobileCMSContentMessages> GetCMSContentMessageByKey(string Key, MOBRequest request, Session session)
        {

            CSLContentMessagesResponse cmsResponse = new CSLContentMessagesResponse();
            MOBMobileCMSContentMessages cmsMessage = null;
            List<CMSContentMessage> cmsMessages = null;
            try
            {
                var cmsContentCache = await _cachingService.GetCache<string>(_configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID") + "MOBCSLContentMessagesResponse", request.TransactionId);
                try
                {
                    if (!string.IsNullOrEmpty(cmsContentCache))
                        cmsResponse = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(cmsContentCache);
                }
                catch { cmsContentCache = null; }

                if (string.IsNullOrEmpty(cmsContentCache) || Convert.ToBoolean(cmsResponse.Status) == false || cmsResponse.Messages == null)
                    cmsResponse = await _travelerCSL.GetBookingRTICMSContentMessages(request, session);

                cmsMessages = (cmsResponse != null && cmsResponse.Messages != null && cmsResponse.Messages.Count > 0) ? cmsResponse.Messages : null;
                if (cmsMessages != null)
                {
                    var message = cmsMessages.Find(m => m.Title.Equals(Key));
                    if (message != null)
                    {
                        cmsMessage = new MOBMobileCMSContentMessages()
                        {
                            HeadLine = message.Headline,
                            ContentFull = message.ContentFull,
                            ContentShort = message.ContentShort
                        };
                    }
                }
            }
            catch (Exception)
            { }
            return cmsMessage;
        }

        public bool IsEnableWheelchairLinkUpdate(Session session)
        {
            return _configuration.GetValue<bool>("EnableWheelchairLinkUpdate") &&
                   session.CatalogItems != null &&
                   session.CatalogItems.Count > 0 &&
                   session.CatalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableWheelchairLinkUpdate).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableWheelchairLinkUpdate).ToString())?.CurrentValue == "1";
        }

        private async Task<List<TravelSpecialNeed>> GetOfferedSpecialRequests(MultiCallResponse flightshoppingReferenceData, MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null, Session session = null)
        {
            if (flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialRequestResponses == null || !flightshoppingReferenceData.SpecialRequestResponses.Any()
                || flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests == null || !flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests.Any())
                return null;

            var specialRequests = new List<TravelSpecialNeed>();
            var specialNeedType = TravelSpecialNeedType.SpecialRequest.ToString();
            TravelSpecialNeed createdWheelChairItem = null;
            Func<string, string, string, TravelSpecialNeed> createSpecialNeedItem = (code, value, desc)
                => new TravelSpecialNeed { Code = code, Value = value, DisplayDescription = desc, RegisterServiceDescription = desc, Type = specialNeedType };


            foreach (var specialRequest in flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests.Where(x => x.Genre != null && !string.IsNullOrWhiteSpace(x.Genre.Description) && !string.IsNullOrWhiteSpace(x.Code)))
            {
                if (specialRequest.Genre.Description.Equals("General"))
                {
                    var sr = createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description);

                    if (specialRequest.Code.StartsWith("DPNA", StringComparison.OrdinalIgnoreCase)) // add info message for DPNA_1, and DPNA_2 request
                        sr.Messages = new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSR_DPNA_Message") } };
                    await SetTaskTrainedServiceAnimalMessage(specialRequest, sr, reservation, selectRequest, null, selectRequest, session);
                    if (sr.Code != "OTHS")
                        specialRequests.Add(sr);
                }
                else if (specialRequest.Genre.Description.Equals("WheelchairReason"))
                {
                    if (createdWheelChairItem == null)
                    {
                        createdWheelChairItem = createSpecialNeedItem(_configuration.GetValue<string>("SSRWheelChairDescription"), null, _configuration.GetValue<string>("SSRWheelChairDescription"));
                        createdWheelChairItem.SubOptionHeader = _configuration.GetValue<string>("SSR_WheelChairSubOptionHeader");
                        // MOBILE-23726
                        if (IsEnableWheelchairLinkUpdate(session))
                        {
                            var sdlKeyForWheelchairLink = _configuration.GetValue<string>("FSRSpecialTravelNeedsWheelchairLinkKey");
                            MOBMobileCMSContentMessages message = null;
                            if (!string.IsNullOrEmpty(sdlKeyForWheelchairLink))
                            {
                                message = await GetCMSContentMessageByKey(sdlKeyForWheelchairLink, selectRequest, session);
                            }
                            createdWheelChairItem.InformationLink = message?.ContentFull ?? (_configuration.GetValue<string>("WheelchairLinkUpdateFallback") ?? "");
                        }

                        specialRequests.Add(createdWheelChairItem);
                    }

                    var wheelChairSubItem = createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description);

                    if (createdWheelChairItem.SubOptions == null)
                    {
                        createdWheelChairItem.SubOptions = new List<TravelSpecialNeed> { wheelChairSubItem };
                    }
                    else
                    {
                        createdWheelChairItem.SubOptions.Add(wheelChairSubItem);
                    }
                }
                else if (specialRequest.Genre.Description.Equals("WheelchairType"))
                {
                    specialRequests.Add(createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description));
                }
            }

            return specialRequests;
        }

        private async System.Threading.Tasks.Task SetTaskTrainedServiceAnimalMessage(Characteristic specialRequest, TravelSpecialNeed sr, MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null,
           TraceSwitch _traceSwitch = null, MOBRequest request = null, Session session = null)
        {
            if (selectRequest != null && reservation != null && _shoppingUtility.IsServiceAnimalEnhancementEnabled(selectRequest.Application.Id, selectRequest.Application.Version.Major, selectRequest.CatalogItems) && specialRequest.Code.StartsWith(_configuration.GetValue<string>("TasktrainedServiceAnimalCODE"), StringComparison.OrdinalIgnoreCase)) // add info message for Task-trained service animal
            {

                List<CMSContentMessage> lstMessages = await _ffcShoppingcs.GetSDLContentByGroupName(request, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_BookingRTI_Messages"), _configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID"));

                //First 2 lines only if International , HNL or GUM 
                if (reservation?.ISInternational == true
                    || (!string.IsNullOrWhiteSpace(reservation?.Trips?.FirstOrDefault()?.Origin) && _configuration.GetValue<string>("DisableServiceAnimalAirportCodes")?.Contains(reservation?.Trips?.FirstOrDefault()?.Origin) == true)
                    || (!string.IsNullOrWhiteSpace(reservation?.Trips?.FirstOrDefault()?.Destination) && _configuration.GetValue<string>("DisableServiceAnimalAirportCodes")?.Contains(reservation?.Trips?.FirstOrDefault()?.Destination) == true))
                {
                    sr.IsDisabled = true;
                    sr.Messages = new List<MOBItem> { new MOBItem {
                                Id = "ESAN_SUBTITLE",
                                CurrentValue =_shoppingUtility.GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Content_INT_MOB")  } };
                }
                sr.SubOptions = new List<TravelSpecialNeed>();

                sr.SubOptions.Add(new TravelSpecialNeed
                {
                    Value = sr.Value,
                    Code = "SVAN",
                    Type = TravelSpecialNeedType.ServiceAnimalType.ToString(),
                    DisplayDescription = _shoppingUtility.GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Content2_MOB"),
                    RegisterServiceDescription = _shoppingUtility.GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB")
                });
                if (sr.Messages == null) sr.Messages = new List<MOBItem>();
                ServiceAnimalDetailsScreenMessages(sr.Messages);

                //TODO, ONCE FS changes are ready we do not need below
                sr.Code = "SVAN";
                sr.DisplayDescription = _shoppingUtility.GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB");
                sr.RegisterServiceDescription = _shoppingUtility.GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB");
            }
        }


        private void ServiceAnimalDetailsScreenMessages(List<MOBItem> messages)
        {

            messages.Add(new MOBItem
            {
                Id = "ESAN_HDR",
                CurrentValue = "Additional step required before traveling with a service dog. \n <br /> <br /> Please complete the service dog request form located in Trip Details before arriving at the airport.",
            });
            messages.Add(new MOBItem
            {
                Id = "ESAN_FTR",
                CurrentValue = "\n<br /><br />We no longer accept emotional support animals due to new Department of Transportation regulations.\n<br /><br /><a href=\"https://www.united.com/ual/en/US/fly/travel/special-needs/disabilities/assistance-animals.html\">Review our service animal policy</a>",
            });
        }

        private List<MOBItem> GetSpecialMealsMessages(IEnumerable<ReservationFlightSegment> allSegmentsWithMeals, MultiCallResponse flightshoppingReferenceData)
        {
            Func<List<MOBItem>> GetMealUnavailableMsg = () => new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSRItinerarySpecialMealsNotAvailableMsg"), "") } };

            if (allSegmentsWithMeals == null || !allSegmentsWithMeals.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialMealResponses == null || !flightshoppingReferenceData.SpecialMealResponses.Any()
                || flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals == null || !flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Any())
            {
                return GetMealUnavailableMsg();
            }

            // all meals from reference data
            var allRefMeals = flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Select(x => x.Type.Key);
            if (allRefMeals == null || !allRefMeals.Any())
                return GetMealUnavailableMsg();

            var segmentsHaveMeals = allSegmentsWithMeals.Where(seg => seg != null && seg.FlightSegment != null && seg.FlightSegment.Characteristic != null && seg.FlightSegment.Characteristic.Any()
                                                   && seg.FlightSegment.Characteristic[0] != null
                                                   && seg.FlightSegment.Characteristic.Exists(x => x.Code.Equals("SPML", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(x.Value)))
                                                   .Select(seg => new
                                                   {
                                                       segment = string.Join(" - ", seg.FlightSegment.DepartureAirport.IATACode, seg.FlightSegment.ArrivalAirport.IATACode),
                                                       meals = string.IsNullOrWhiteSpace(seg.FlightSegment.Characteristic[0].Value) ? new HashSet<string>() : new HashSet<string>(seg.FlightSegment.Characteristic[0].Value.Split('|', ' ').Intersect(allRefMeals))
                                                   })
                                                   .Where(seg => seg.meals != null && seg.meals.Any())
                                                   .Select(seg => seg.segment)
                                                   .ToList();

            if (segmentsHaveMeals == null || !segmentsHaveMeals.Any())
            {
                return GetMealUnavailableMsg();
            }

            if (segmentsHaveMeals.Count < allSegmentsWithMeals.Count())
            {
                var segments = segmentsHaveMeals.Count > 1 ? string.Join(", ", segmentsHaveMeals.Take(segmentsHaveMeals.Count - 1)) + " and " + segmentsHaveMeals.Last() : segmentsHaveMeals.First();
                return new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSR_MarketMealRestrictionMessage"), segments) } };
            }

            return null;
        }

        private List<TravelSpecialNeed> GetOfferedMealsForItinerary(IEnumerable<ReservationFlightSegment> allSegmentsWithMeals, MultiCallResponse flightshoppingReferenceData)
        {
            if (allSegmentsWithMeals == null || !allSegmentsWithMeals.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialMealResponses == null || !flightshoppingReferenceData.SpecialMealResponses.Any()
                || flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals == null || !flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Any())
                return null;

            Func<IEnumerable<string>, List<MOBItem>> generateMsg = flightSegments =>
            {
                var segments = flightSegments.Count() > 1 ? string.Join(", ", flightSegments.Take(flightSegments.Count() - 1)) + " and " + flightSegments.Last() : flightSegments.First();
                return new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSR_MealRestrictionMessage"), segments) } };
            };

            // all meals from reference data
            var allRefMeals = flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.ToDictionary(x => x.Type.Key, x => string.Join("^", x.Value[0], x.Description));
            if (allRefMeals == null || !allRefMeals.Any())
                return null;

            // Dictionary whose keys are segments (orig - dest) and values are list of all meals that are available for each segment
            // These contain only the segments that offer meals
            // These meals also need to exist in reference data table 
            var segmentAndMealsMap = allSegmentsWithMeals.Where(seg => seg != null && seg.FlightSegment != null && seg.FlightSegment.Characteristic != null && seg.FlightSegment.Characteristic.Any()
                                                   && seg.FlightSegment.Characteristic[0] != null
                                                   && seg.FlightSegment.Characteristic.Exists(x => x.Code.Equals("SPML", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(x.Value))) // get all segments that offer meals
                                                   .Select(seg => new // project them
                                                   {
                                                       segment = string.Join(" - ", seg.FlightSegment.DepartureAirport.IATACode, seg.FlightSegment.ArrivalAirport.IATACode), // IAH - NRT if going from IAH to NRT 
                                                       meals = string.IsNullOrWhiteSpace(seg.FlightSegment.Characteristic[0].Value) ? null : new HashSet<string>(seg.FlightSegment.Characteristic[0].Value.Split('|', ' ').Intersect(allRefMeals.Keys)) // List of all meal codes that offer on the segment
                                                   })
                                                   .Where(segment => segment.meals != null && segment.meals.Any()) // filter out the segments that don't offer meals
                                                   .GroupBy(seg => seg.segment) // handle same market exist twice for MD
                                                   .Select(grp => grp.First()) // handle same market exist twice for MD 
                                                   .ToDictionary(seg => seg.segment, seg => seg.meals); // tranform them to dictionary of segment and meals

            if (segmentAndMealsMap == null || !segmentAndMealsMap.Any())
                return null;

            // Get common meals that offers on all segments after filtering out all segments that don't offer meals
            var mealsThatAvailableOnAllSegments = segmentAndMealsMap.Values.Skip(1)
                                                                            .Aggregate(new HashSet<string>(segmentAndMealsMap.Values.First()), (current, next) => { current.IntersectWith(next); return current; });

            // Filter out the common meals
            if (mealsThatAvailableOnAllSegments != null && mealsThatAvailableOnAllSegments.Any())
            {
                segmentAndMealsMap.Values.ToList().ForEach(x => x.RemoveWhere(item => mealsThatAvailableOnAllSegments.Contains(item)));
            }

            // Add the non-common meals, these will have message
            var results = segmentAndMealsMap.Where(kv => kv.Value != null && kv.Value.Any())
                                .SelectMany(item => item.Value.Select(x => new { mealCode = x, segment = item.Key }))
                                .GroupBy(x => x.mealCode, x => x.segment)
                                .ToDictionary(x => x.Key, x => x.ToList())
                                .Select(kv => new TravelSpecialNeed
                                {
                                    Code = kv.Key,
                                    Value = allRefMeals[kv.Key].Split('^')[0],
                                    DisplayDescription = allRefMeals[kv.Key].Split('^')[1],
                                    RegisterServiceDescription = allRefMeals[kv.Key].Split('^')[1],
                                    Type = TravelSpecialNeedType.SpecialMeal.ToString(),
                                    Messages = mealsThatAvailableOnAllSegments.Any() ? generateMsg(kv.Value) : null
                                })
                                .ToList();

            // Add the common meals, these don't have messages
            if (mealsThatAvailableOnAllSegments.Any())
            {
                results.AddRange(mealsThatAvailableOnAllSegments.Select(m => new TravelSpecialNeed
                {
                    Code = m,
                    Value = allRefMeals[m].Split('^')[0],
                    DisplayDescription = allRefMeals[m].Split('^')[1],
                    RegisterServiceDescription = allRefMeals[m].Split('^')[1],
                    Type = TravelSpecialNeedType.SpecialMeal.ToString()
                }));
            }

            return results == null || !results.Any() ? null : results; // return null if empty
        }

        private List<TravelSpecialNeed> GetOfferedServiceAnimals(MultiCallResponse flightshoppingReferenceData, IEnumerable<ReservationFlightSegment> segments, int appId, string appVersion)
        {
            if (!_configuration.GetValue<bool>("EnableServiceAnimalEnhancements") ||
                (!IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion) &&
               !_configuration.GetValue<bool>("ShowServiceAnimalInTravelNeeds")))
                return null;

            if (segments == null || !segments.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.ServiceAnimalResponses == null || !flightshoppingReferenceData.ServiceAnimalResponses.Any()
                || flightshoppingReferenceData.ServiceAnimalResponses[0].Animals == null || !flightshoppingReferenceData.ServiceAnimalResponses[0].Animals.Any()
                || flightshoppingReferenceData.ServiceAnimalTypeResponses == null || !flightshoppingReferenceData.ServiceAnimalTypeResponses.Any()
                || flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types == null || !flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types.Any())
                return null;

            if (!DoesItineraryHaveServiceAnimal(segments))
                return null;

            var SSRAnimalValueCodeDesc = _configuration.GetValue<string>("SSRAnimalValueCodeDesc").Split('|').ToDictionary(x => x.Split('^')[0], x => x.Split('^')[1]);
            var SSRAnimalTypeValueCodeDesc = _configuration.GetValue<string>("SSRAnimalTypeValueCodeDesc").Split('|').ToDictionary(x => x.Split('^')[0], x => x.Split('^')[1]);

            Func<string, string, string, string, string, TravelSpecialNeed> createSpecialNeed = (code, value, desc, RegisterServiceDesc, type)
                => new TravelSpecialNeed { Code = code, Value = value, DisplayDescription = desc, RegisterServiceDescription = RegisterServiceDesc, Type = type };

            List<TravelSpecialNeed> animals = flightshoppingReferenceData.ServiceAnimalResponses[0].Animals
                                                .Where(x => !string.IsNullOrWhiteSpace(x.Description))
                                                .Select(x => createSpecialNeed(SSRAnimalValueCodeDesc[x.Value], x.Value, x.Description, x.Description, TravelSpecialNeedType.ServiceAnimal.ToString())).ToList();

            Func<United.Service.Presentation.CommonModel.Characteristic, TravelSpecialNeed> createServiceAnimalTypeItem = animalType =>
            {
                var type = createSpecialNeed(SSRAnimalTypeValueCodeDesc[animalType.Value], animalType.Value,
                                             animalType.Description, animalType.Description.EndsWith("animal", StringComparison.OrdinalIgnoreCase) ? null : !_configuration.GetValue<bool>("DisableTaskServiceAnimalDescriptionFix") ? animalType.Description : "Dog", TravelSpecialNeedType.ServiceAnimalType.ToString());
                type.SubOptions = animalType.Description.EndsWith("animal", StringComparison.OrdinalIgnoreCase) ? animals : null;
                return type;
            };

            return flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types.Where(x => !string.IsNullOrWhiteSpace(x.Description))
                                                                                  .Select(createServiceAnimalTypeItem).ToList();
        }
        private bool DoesItineraryHaveServiceAnimal(IEnumerable<ReservationFlightSegment> segments)
        {
            var statesDoNotAllowServiceAnimal = new HashSet<string>(_configuration.GetValue<string>("SSRStatesDoNotAllowServiceAnimal").Split('|'));
            foreach (var segment in segments)
            {
                if (segment == null || segment.FlightSegment == null || segment.FlightSegment.ArrivalAirport == null || segment.FlightSegment.DepartureAirport == null
                    || segment.FlightSegment.ArrivalAirport.IATACountryCode == null || segment.FlightSegment.DepartureAirport.IATACountryCode == null
                    || string.IsNullOrWhiteSpace(segment.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode) || string.IsNullOrWhiteSpace(segment.FlightSegment.DepartureAirport.IATACountryCode.CountryCode)
                    || segment.FlightSegment.ArrivalAirport.StateProvince == null || segment.FlightSegment.DepartureAirport.StateProvince == null
                    || string.IsNullOrWhiteSpace(segment.FlightSegment.ArrivalAirport.StateProvince.StateProvinceCode) || string.IsNullOrWhiteSpace(segment.FlightSegment.DepartureAirport.StateProvince.StateProvinceCode)

                    || !segment.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode.Equals("US") || !segment.FlightSegment.DepartureAirport.IATACountryCode.CountryCode.Equals("US") // is international
                    || statesDoNotAllowServiceAnimal.Contains(segment.FlightSegment.ArrivalAirport.StateProvince.StateProvinceCode) // touches states that not allow service animal
                    || statesDoNotAllowServiceAnimal.Contains(segment.FlightSegment.DepartureAirport.StateProvince.StateProvinceCode)) // touches states that not allow service animal
                    return false;
            }

            return true;
        }

        private async System.Threading.Tasks.Task AddServiceAnimalsMessageSection(TravelSpecialNeeds offersSSR, int appId, string appVersion, Session session, string deviceId)
        {
            if (IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion))
            {
                if (offersSSR?.ServiceAnimals == null) offersSSR.ServiceAnimals = new List<TravelSpecialNeed>();
                MOBRequest request = new MOBRequest();
                request.Application = new MOBApplication();
                request.Application.Id = appId;
                request.Application.Version = new MOBVersion();
                request.Application.Version.Major = appVersion;
                request.DeviceId = deviceId;
                request.TransactionId = _headers.ContextValues.TransactionId;
                CSLContentMessagesResponse content = new CSLContentMessagesResponse();

                var cmsCacheResponse = await _cachingService.GetCache<string>(_configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID") + "MOBCSLContentMessagesResponse", _headers.ContextValues.TransactionId).ConfigureAwait(false);

                try
                {
                    if (!string.IsNullOrEmpty(cmsCacheResponse))
                        content = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(cmsCacheResponse);
                }

                catch { cmsCacheResponse = null; }

                if (string.IsNullOrEmpty(cmsCacheResponse))
                    content = await _travelerCSL.GetBookingRTICMSContentMessages(request, session);//, LogEntries);

                string emotionalSupportAssistantContent = (content?.Messages?.FirstOrDefault(m => !string.IsNullOrEmpty(m.Title) && m.Title.Equals("TravelNeeds_TaskTrainedDog_Screen_Content_MOB"))?.ContentFull) ?? "";
                string emotionalSupportAssistantCodeVale = _configuration.GetValue<string>("TravelSpecialNeedInfoCodeValue");

                if (!string.IsNullOrEmpty(emotionalSupportAssistantContent) && !string.IsNullOrEmpty(emotionalSupportAssistantCodeVale))
                {
                    var codeValue = emotionalSupportAssistantCodeVale.Split('#');
                    offersSSR.ServiceAnimals.Add(new TravelSpecialNeed
                    {
                        Code = codeValue[0],
                        Value = codeValue[1],
                        DisplayDescription = "",
                        Type = TravelSpecialNeedType.TravelSpecialNeedInfo.ToString(),
                        Messages = new List<MOBItem>
                        {
                            new MOBItem {
                                CurrentValue = emotionalSupportAssistantContent
                            }
                        }
                    });
                }
            }

            else if (_configuration.GetValue<bool>("EnableTravelSpecialNeedInfo")
                && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("TravelSpecialNeedInfo_Supported_AppVestion_Android"), _configuration.GetValue<string>("TravelSpecialNeedInfo_Supported_AppVestion_iOS"))
                && offersSSR.ServiceAnimals != null && offersSSR.ServiceAnimals.Any())
            {
                string emotionalSupportAssistantHeading = _configuration.GetValue<string>("TravelSpecialNeedInfoHeading");
                string emotionalSupportAssistantContent = _configuration.GetValue<string>("TravelSpecialNeedInfoContent");
                string emotionalSupportAssistantCodeVale = _configuration.GetValue<string>("TravelSpecialNeedInfoCodeValue");

                if (!string.IsNullOrEmpty(emotionalSupportAssistantHeading) &&
                    !string.IsNullOrEmpty(emotionalSupportAssistantContent) &&
                    !string.IsNullOrEmpty(emotionalSupportAssistantCodeVale))
                {
                    var codeValue = emotionalSupportAssistantCodeVale.Split('#');

                    offersSSR.ServiceAnimals.Add(new TravelSpecialNeed
                    {
                        Code = codeValue[0],
                        Value = codeValue[1],
                        DisplayDescription = emotionalSupportAssistantHeading,
                        Type = TravelSpecialNeedType.TravelSpecialNeedInfo.ToString(),
                        Messages = new List<MOBItem>
                        {
                            new MOBItem {
                                CurrentValue = emotionalSupportAssistantContent
                            }
                        }
                    });
                }
            }
        }
        private bool IsTaskTrainedServiceDogSupportedAppVersion(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("TravelSpecialNeedInfo_TaskTrainedServiceDog_Supported_AppVestion_Android"), _configuration.GetValue<string>("TravelSpecialNeedInfo_TaskTrainedServiceDog_Supported_AppVestion_iOS"));
        }
        private List<MOBItem> GetServiceAnimalsMessages(List<TravelSpecialNeed> serviceAnimals)
        {
            if (serviceAnimals != null && serviceAnimals.Any())
                return null;

            return new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSRItineraryServiceAnimalNotAvailableMsg") } };
        }
        public async Task<MultiCallResponse> GetSpecialNeedsReferenceDataFromFlightShopping(Session session, int appId, string appVersion, string deviceId, string languageCode)
        {
            string cslActionName = "MultiCall";

            string jsonRequest = JsonConvert.SerializeObject(GetFlightShoppingMulticallRequest(languageCode));

            string token = await _dPService.GetAnonymousToken(_headers.ContextValues.Application.Id, _headers.ContextValues.DeviceId, _configuration).ConfigureAwait(false);

            var cslCallDurationstopwatch = new Stopwatch();
            cslCallDurationstopwatch.Start();

            var response = await _referencedataService.GetSpecialNeedsInfo<MultiCallResponse>(cslActionName, jsonRequest, token, session.SessionId).ConfigureAwait(false);


            if (cslCallDurationstopwatch.IsRunning)
            {
                cslCallDurationstopwatch.Stop();
            }

            if (response != null)
            {
                if (response == null || response.SpecialRequestResponses == null || response.ServiceAnimalResponses == null || response.SpecialMealResponses == null || response.SpecialRequestResponses == null)
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));

                return response;
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }
        private MultiCallRequest GetFlightShoppingMulticallRequest(string languageCode)
        {
            var request = new MultiCallRequest
            {
                ServiceAnimalRequests = new Collection<ServiceAnimalRequest> { new ServiceAnimalRequest { LanguageCode = languageCode } },
                ServiceAnimalTypeRequests = new Collection<ServiceAnimalTypeRequest> { new ServiceAnimalTypeRequest { LanguageCode = languageCode } },
                SpecialMealRequests = new Collection<SpecialMealRequest> { new SpecialMealRequest { LanguageCode = languageCode } },
                SpecialRequestRequests = new Collection<SpecialRequestRequest> { new SpecialRequestRequest { LanguageCode = languageCode/*, Channel = ConfigurationManager.AppSettings["Shopping - ChannelType")*/ } },
            };

            return request;
        }
        #endregion

        #region-HelperClass
        public class MOBSHOPUnfinishedBookingComparer : IEqualityComparer<MOBSHOPUnfinishedBooking>
        {
            private bool _compareArrivalDateTime;
            private bool _compareIsElf;

            public MOBSHOPUnfinishedBookingComparer(bool compareArrivalDateTime = false, bool compareIsElf = false)
            {
                _compareIsElf = compareIsElf;
                _compareArrivalDateTime = compareArrivalDateTime;
            }
            public bool Equals(MOBSHOPUnfinishedBooking x, MOBSHOPUnfinishedBooking y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }

                if (x.TravelerTypes == null && y.TravelerTypes == null)
                {
                    if (x.SearchType != y.SearchType
                        || x.NumberOfAdults != y.NumberOfAdults
                        || x.NumberOfSeniors != y.NumberOfSeniors
                        || x.NumberOfChildren2To4 != y.NumberOfChildren2To4
                        || x.NumberOfChildren5To11 != y.NumberOfChildren5To11
                        || x.NumberOfChildren12To17 != y.NumberOfChildren12To17
                        || x.NumberOfInfantOnLap != y.NumberOfInfantOnLap
                        || x.NumberOfInfantWithSeat != y.NumberOfInfantWithSeat
                        || x.CountryCode != y.CountryCode)
                    {
                        return false;
                    }
                }

                if (x.SearchType != y.SearchType)
                    return false;

                if (x.TravelerTypes != null && y.TravelerTypes != null)
                {
                    if (x.TravelerTypes.Where(t => t.Count > 0).ToList().Count != y.TravelerTypes.Where(t => t.Count > 0).ToList().Count
                        || x.TravelerTypes.Where(t => t.Count > 0).ToList().Except(y.TravelerTypes.Where(t => t.Count > 0).ToList(), new MOBTravelerTypeComparer()).Count() != 0
                        || y.TravelerTypes.Where(t => t.Count > 0).ToList().Except(x.TravelerTypes.Where(t => t.Count > 0).ToList(), new MOBTravelerTypeComparer()).Count() != 0
                        || x.CountryCode != y.CountryCode)
                    {
                        return false;
                    }
                }

                if (x.TravelerTypes == null && (x.NumberOfAdults > 0 || x.NumberOfSeniors > 0) && y.TravelerTypes != null)
                {
                    //if (y.TravelerTypes.Any(t => t.TravelerType.ToUpper() != PAXTYPE.Adult.ToString().ToUpper() && t.Count > 0))
                    //    return false;

                    if (x.NumberOfAdults > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Adult.ToString()) || x.NumberOfAdults != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Adult.ToString()).First().Count))
                        return false;

                    if (x.NumberOfChildren2To4 > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child2To4.ToString()) || x.NumberOfChildren2To4 != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).First().Count))
                        return false;

                    if (x.NumberOfChildren5To11 > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child5To11.ToString()) || x.NumberOfChildren5To11 != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).First().Count))
                        return false;

                    if (x.NumberOfChildren12To17 > 0 && (!y.TravelerTypes.Any(p => p.TravelerType == PAXTYPE.Child12To14.ToString() || p.TravelerType == PAXTYPE.Child15To17.ToString()) ||
                        x.NumberOfChildren12To17 != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child12To14.ToString() || t.TravelerType == PAXTYPE.Child15To17.ToString()).Sum(t => t.Count)))
                        return false;

                    if (x.NumberOfInfantWithSeat > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()) || x.NumberOfInfantWithSeat != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).First().Count))
                        return false;

                    if (x.NumberOfInfantOnLap > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantLap.ToString()) || x.NumberOfInfantOnLap != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).First().Count))
                        return false;

                    if (x.NumberOfSeniors > 0 && (!y.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Senior.ToString()) && x.NumberOfSeniors != y.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Senior.ToString()).First().Count))
                        return false;
                }

                if (x.TravelerTypes != null && y.TravelerTypes == null && (y.NumberOfAdults > 0 || y.NumberOfSeniors > 0))
                {
                    //if (x.TravelerTypes.Any(t => t.TravelerType.ToUpper() != PAXTYPE.Adult.ToString().ToUpper() && t.Count > 0))
                    //    return false;

                    if (y.NumberOfAdults > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Adult.ToString()) || y.NumberOfAdults != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Adult.ToString()).First().Count))
                        return false;

                    if (y.NumberOfChildren2To4 > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child2To4.ToString()) || y.NumberOfChildren2To4 != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child2To4.ToString()).First().Count))
                        return false;

                    if (y.NumberOfChildren5To11 > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child5To11.ToString()) || y.NumberOfChildren5To11 != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child5To11.ToString()).First().Count))
                        return false;

                    if (y.NumberOfChildren12To17 > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Child12To14.ToString() || t.TravelerType == PAXTYPE.Child15To17.ToString()) || y.NumberOfChildren12To17 != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Child12To14.ToString() || t.TravelerType == PAXTYPE.Child15To17.ToString()).Sum(t => t.Count)))
                        return false;

                    if (y.NumberOfInfantWithSeat > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()) || y.NumberOfInfantWithSeat != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.InfantSeat.ToString()).First().Count))
                        return false;

                    if (y.NumberOfInfantOnLap > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.InfantLap.ToString()) || y.NumberOfInfantOnLap != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.InfantLap.ToString()).First().Count))
                        return false;

                    if (y.NumberOfSeniors > 0 && (!x.TravelerTypes.Any(t => t.TravelerType == PAXTYPE.Senior.ToString()) || y.NumberOfSeniors != x.TravelerTypes.Where(t => t.TravelerType == PAXTYPE.Senior.ToString()).First().Count))
                        return false;
                }

                if (_compareIsElf && (x.IsELF != y.IsELF))
                {
                    return false;
                }

                if (x.Trips == null && y.Trips == null)
                    return true;

                if (x.Trips == null)
                    return !y.Trips.Any();

                if (y.Trips == null)
                    return !x.Trips.Any();

                //if (_configuration.GetValue<bool>("GetUnfinishedBookingsArgumentOutOfRangeExceptionFix"))
                //{
                //    if (x.Trips.Count() != y.Trips.Count())
                //        return false;
                //}
                //else
                //{
                //    if (x.Trips.Count() != x.Trips.Count)
                //        return false;
                //}

                for (int i = 0; i < x.Trips.Count; i++)
                {
                    var xtrip = x.Trips[i];
                    var ytrip = y.Trips[i];
                    if (xtrip == null && ytrip == null)
                        return true;

                    if (xtrip.Destination != ytrip.Destination
                        || xtrip.Origin != ytrip.Origin
                        || !SameDate(xtrip.DepartDate, ytrip.DepartDate)
                        || !SameTime(xtrip.DepartTime, ytrip.DepartTime)
                        || !SameDate(xtrip.DepartDateTimeGMT, ytrip.DepartDateTimeGMT)
                        || xtrip != null && ytrip == null
                        || xtrip == null && ytrip != null)
                    {
                        return false;
                    }

                    if (_compareArrivalDateTime && (!SameDate(xtrip.ArrivalDate, ytrip.ArrivalDate) || !SameTime(xtrip.ArrivalTime, ytrip.ArrivalTime)))
                    {
                        return false;
                    }

                    if (xtrip.Flights == null && ytrip.Flights == null)
                        return true;

                    if (xtrip.Flights == null)
                        return !ytrip.Flights.Any();

                    if (ytrip.Flights == null)
                        return !xtrip.Flights.Any();

                    if (xtrip.Flights.Count() != ytrip.Flights.Count)
                        return false;

                    for (int j = 0; j < xtrip.Flights.Count(); j++)
                    {
                        if (!AreSameFlights(xtrip.Flights[j], ytrip.Flights[j]))
                            return false;
                    }
                }

                return true;
            }

            private bool AreSameFlights(MOBSHOPUnfinishedBookingFlight f1, MOBSHOPUnfinishedBookingFlight f2)
            {
                if (f1 == null && f2 == null)
                    return true;

                if (f1.BookingCode != f2.BookingCode
                    || !SameDate(f1.DepartDateTime, f2.DepartDateTime)
                    || f1.Origin != f2.Origin
                    || f1.Destination != f2.Destination
                    || f1.FlightNumber != f2.FlightNumber
                    || f1.MarketingCarrier != f2.MarketingCarrier
                    || f1.ProductType != f2.ProductType)
                {
                    return false;
                }

                if (f1.Connections == null && f2.Connections == null)
                    return true;

                if (f1.Connections == null)
                    return !f2.Connections.Any();

                if (f2.Connections == null)
                    return !f1.Connections.Any();

                if (f1.Connections.Count() != f2.Connections.Count)
                    return false;

                for (int i = 0; i < f1.Connections.Count(); i++)
                {
                    if (!AreSameFlights(f1.Connections[i], f2.Connections[i]))
                        return false;
                }

                return true;
            }

            private bool SameDate(string date1, string date2)
            {
                if (string.IsNullOrEmpty(date1) && string.IsNullOrEmpty(date2))
                    return true;

                if (!string.IsNullOrEmpty(date1) && string.IsNullOrEmpty(date2) || string.IsNullOrEmpty(date1) && !string.IsNullOrEmpty(date2))
                    return false;

                if (date1 == date2)
                    return true;

                DateTime d1;
                DateTime d2;
                var d1Parsed = DateTime.TryParse(date1, out d1);
                var d2Parsed = DateTime.TryParse(date2, out d2);


                if (!(d1Parsed && d2Parsed))
                    return false;

                return d1 == d2;
            }

            private bool SameTime(string time1, string time2)
            {
                if (string.IsNullOrEmpty(time1) && string.IsNullOrEmpty(time2))
                    return true;

                if (!string.IsNullOrEmpty(time1) && string.IsNullOrEmpty(time2) || string.IsNullOrEmpty(time1) && !string.IsNullOrEmpty(time2))
                    return false;

                if (time1 == time2)
                    return true;

                DateTime d1;
                DateTime d2;

                var now = DateTime.Now.ToShortDateString();
                var d1Parsed = DateTime.TryParse(now + " " + time1, out d1);
                var d2Parsed = DateTime.TryParse(now + " " + time2, out d2);

                if (!(d1Parsed && d2Parsed))
                    return false;

                return d1 == d2;
            }

            public int GetHashCode(MOBSHOPUnfinishedBooking obj)
            {
                return -32000;
            }
        }
        public class MOBTravelerTypeComparer : IEqualityComparer<MOBTravelerType>
        {
            public bool Equals(MOBTravelerType x, MOBTravelerType y)
            {
                if (Object.ReferenceEquals(x, y)) return true;

                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                return x.TravelerType.Equals(y.TravelerType) && x.Count == y.Count;
            }

            public int GetHashCode(MOBTravelerType tType)
            {

                if (Object.ReferenceEquals(tType, null)) return 0;

                return tType.Count ^ tType.TravelerType.GetHashCode();
            }
        }

        private void PopulateFlightAmenities(Collection<AmenitiesProfile> amenityFlights, ref List<Flight> flights)
        {
            if (amenityFlights != null && amenityFlights.Count > 0)
            {
                try
                {
                    foreach (Flight flight in flights)
                    {
                        Flight tempFlight = flight;
                        GetAmenitiesForFlight(amenityFlights, ref tempFlight);
                        flight.Amenities = tempFlight.Amenities;

                        if (flight.Connections != null && flight.Connections.Count > 0)
                        {
                            List<Flight> tempFlights = flight.Connections;
                            PopulateFlightAmenities(amenityFlights, ref tempFlights);
                            flight.Connections = tempFlights;
                        }
                        if (flight.StopInfos != null && flight.StopInfos.Count > 0)
                        {
                            List<Flight> tempFlights = flight.StopInfos;
                            PopulateFlightAmenities(amenityFlights, ref tempFlights);
                            flight.StopInfos = tempFlights;
                        }
                    }
                }
                catch { }
            }
        }

        private void GetAmenitiesForFlight(Collection<AmenitiesProfile> amenityFlights, ref Flight flight)
        {
            foreach (AmenitiesProfile amenityFlight in amenityFlights)
            {
                if (flight.FlightNumber == amenityFlight.Key)
                {
                    //update flight amenities
                    flight.Amenities = amenityFlight.Amenities;
                    return;
                }
            }
        }


        private void PopulateLMX(List<United.Services.FlightShopping.Common.LMX.LmxFlight> lmxFlights, ref List<Flight> flights)
        {
            if (lmxFlights != null && lmxFlights.Count > 0)
            {
                try
                {
                    for (int i = 0; i < flights.Count; i++)
                    {
                        Flight tempFlight = flights[i];
                        GetLMXForFlight(lmxFlights, ref tempFlight);
                        flights[i].Products = tempFlight.Products;

                        if (flights[i].Connections != null && flights[i].Connections.Count > 0)
                        {
                            List<Flight> tempFlights = flights[i].Connections;
                            PopulateLMX(lmxFlights, ref tempFlights);
                            flights[i].Connections = tempFlights;
                        }
                        if (flights[i].StopInfos != null && flights[i].StopInfos.Count > 0)
                        {
                            List<Flight> tempFlights = flights[i].StopInfos;
                            PopulateLMX(lmxFlights, ref tempFlights);
                            flights[i].StopInfos = tempFlights;
                        }
                    }
                }
                catch { }
            }
        }
        private void GetLMXForFlight(List<United.Services.FlightShopping.Common.LMX.LmxFlight> lmxFlights, ref Flight flight)
        {
            foreach (United.Services.FlightShopping.Common.LMX.LmxFlight lmxFlight in lmxFlights)
            {
                if (flight.Hash == lmxFlight.Hash)
                {
                    //overwrite the products with new LMX versions
                    for (int i = 0; i < flight.Products.Count; i++)
                    {
                        Product tempProduct = flight.Products[i];
                        GetLMXForProduct(lmxFlight.Products, ref tempProduct);
                        flight.Products[i] = tempProduct;
                    }
                    return;
                }
            }
        }

        private void GetLMXForProduct(List<LmxProduct> productCollection, ref Product tempProduct)
        {
            foreach (LmxProduct p in productCollection)
            {
                if (p.ProductId == tempProduct.ProductId)
                {
                    tempProduct.LmxLoyaltyTiers = p.LmxLoyaltyTiers;
                }
            }
        }

        public static bool MatchServiceClassRequested(string requestedCabin, string fareClass, string prodType, List<Mobile.Model.Shopping.MOBSHOPShoppingProduct> columnInfo, bool isELFFareDisplayAtFSR = true)
        {
            var match = false;
            if (!string.IsNullOrEmpty(requestedCabin))
            {
                requestedCabin = requestedCabin.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(fareClass))
            {
                fareClass = fareClass.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(fareClass) && prodType.ToUpper().Contains("SPECIFIED"))
            {
                match = true;
            }
            else
            {
                if (string.IsNullOrEmpty(fareClass))
                {
                    switch (requestedCabin)
                    {
                        case "ECON":
                        case "ECONOMY":
                            //Removed FLEXIBLE & UNRESTRICTED as it is not taking ECO-FLEXIBLE as selected when Economy is not available.
                            match = (prodType.ToUpper().Contains("ECON") || (isELFFareDisplayAtFSR && prodType.ToUpper().Contains("ECO-BASIC")) || prodType.ToUpper().Contains("ECO-PREMIUM")) && !prodType.ToUpper().Contains("FLEXIBLE") && !prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "ECONOMY-FLEXIBLE":
                            match = (prodType.ToUpper().Contains("ECON") || prodType.ToUpper().Contains("ECO-PREMIUM")) && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "ECONOMY-UNRESTRICTED":
                            match = (prodType.ToUpper().Contains("ECON") || prodType.ToUpper().Contains("ECO-PREMIUM")) && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "BUSINESS":
                        case "BUSINESSFIRST":
                            match = prodType.ToUpper().Contains("BUS") && !prodType.ToUpper().Contains("FLEXIBLE") && !prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "BUSINESS-FLEXIBLE":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "BUSINESS-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "BUSINESSFIRST-FLEXIBLE":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "BUSINESSFIRST-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "FIRST":
                            match = prodType.ToUpper().Contains("FIRST") && !prodType.ToUpper().Contains("FLEXIBLE") && !prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "FIRST-FLEXIBLE":
                            match = prodType.ToUpper().Contains("FIRST") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "FIRST-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("FIRST") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "AWARDECONOMY":
                            match = prodType.ToUpper().Contains("ECON") && !prodType.ToUpper().Contains("FLEXIBLE") && !prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "AWARDECONOMY-FLEXIBLE":
                            match = prodType.ToUpper().Contains("ECON") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "AWARDECONOMY-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("ECON") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "AWARDBUSINESS":
                        case "AWARDBUSINESSFIRST":
                            {
                                var cabinName = GetCabinNameFromColumn(prodType, columnInfo, string.Empty);
                                match = cabinName.ToUpper().Contains("BUSINESS");
                                break;
                            }
                        case "AWARDBUSINESS-FLEXIBLE":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "AWARDBUSINESS-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "AWARDBUSINESSFIRST-FLEXIBLE":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "AWARDBUSINESSFIRST-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("BUS") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        case "AWARDFIRST":
                            {
                                var cabinName = GetCabinNameFromColumn(prodType, columnInfo, string.Empty);
                                match = cabinName.ToUpper().Contains("FIRST");
                                break;
                            }
                        case "AWARDFIRST-FLEXIBLE":
                            match = prodType.ToUpper().Contains("FIRST") && prodType.ToUpper().Contains("FLEXIBLE");
                            break;
                        case "AWARDFIRST-UNRESTRICTED":
                            match = prodType.ToUpper().Contains("FIRST") && prodType.ToUpper().Contains("UNRESTRICTED");
                            break;
                        default:
                            break;
                    }
                }
            }

            return match;
        }

        private static string GetCabinNameFromColumn(string type, List<MOBSHOPShoppingProduct> columnInfo, string defaultCabin)
        {
            type = type.IsNullOrEmpty() ? string.Empty : type.ToUpper().Trim();

            string cabin = "Economy";
            if (columnInfo != null && columnInfo.Count > 0)
            {
                foreach (MOBSHOPShoppingProduct prod in columnInfo)
                {
                    if (!prod.Type.IsNullOrEmpty() && type == prod.Type.ToUpper().Trim())
                    {
                        cabin = prod.LongCabin;
                        break;
                    }
                }
            }
            else
            {
                cabin = defaultCabin;
            }
            return cabin;
        }

        #endregion
    }
}
