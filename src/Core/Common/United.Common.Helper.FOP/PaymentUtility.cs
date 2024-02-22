using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using CslDataVaultResponse = United.Service.Presentation.PaymentResponseModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using FlightSegment = United.Service.Presentation.SegmentModel.FlightSegment;
using ProfileFOPCreditCardResponse = United.Mobile.Model.Shopping.ProfileFOPCreditCardResponse;
using ProfileResponse = United.Mobile.Model.Shopping.ProfileResponse;
using Reservation = United.Mobile.Model.Shopping.Reservation;

namespace United.Common.Helper.FOP
{
    public class PaymentUtility : IPaymentUtility
    {
        private readonly ICacheLog<PaymentUtility> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IDataVaultService _dataVaultService;
        private string _deviceId = string.Empty;
        private MOBApplication _application = new MOBApplication() { Version = new MOBVersion() };
        private string _token = string.Empty;
        private readonly IHeaders _headers;
        private readonly ICachingService _cachingService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;

        public PaymentUtility(ICacheLog<PaymentUtility> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IProductInfoHelper productInfoHelper
            , IDynamoDBService dynamoDBService
            , IDataVaultService dataVaultService
            , IHeaders headers
            , ICachingService cachingService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _productInfoHelper = productInfoHelper;
            _dynamoDBService = dynamoDBService;
            _dataVaultService = dataVaultService;
            _headers = headers;
            _cachingService = cachingService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
        }

        public bool GetIsMilesFOPEnabled(MOBShoppingCart shoppingCart)
        {
            Int32 j;
            Int32.TryParse(shoppingCart.TotalMiles, out j);
            return j > 0;
        }
        public string FirstLetterToUpperCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length == 1)
                return value[0].ToString().ToUpper();

            return value[0].ToString().ToUpper() + value.Substring(1).ToLower();
        }
        public ULTripInfo GetUpliftTripInfo(ReservationDetail reservationDetail, string totalPrice, List<ProdDetail> products)
        {
            if (!ConfigUtility.HasEligibleProductsForUplift(totalPrice, products))
                return null;

            double.TryParse(totalPrice, out double price);
            new Dictionary<string, string>() { { "ONEWAY", "OW" }, { "ROUNDTRIP", "RT" }, { "MULTICITY", "MD" } }.TryGetValue(ShopStaticUtility.GetJourneyTypeDescription(reservationDetail.Detail)?.ToUpper(), out string tripType);
            return new ULTripInfo
            {
                AirReservations = new List<MOBULAirReservation>
                {
                    new MOBULAirReservation
                    {
                        Itineraries = reservationDetail.Detail.FlightSegments.Select( f =>
                        new MOBULItinerary{
                            ArrivalTime = DateTime.Parse( f.FlightSegment.ArrivalDateTime).ToString("MM/dd/yyyy h:mm tt"),
                            DepartureTime = DateTime.Parse(  f.FlightSegment.DepartureDateTime).ToString("MM/dd/yyyy h:mm tt"),
                            CarrierCode = f.FlightSegment.OperatingAirlineCode,
                            Origin = f.FlightSegment.DepartureAirport.IATACode,
                            OriginDescription = f.FlightSegment.DepartureAirport.Name,
                            Destination = f.FlightSegment.ArrivalAirport.IATACode,
                            DestinationDescription = f.FlightSegment.ArrivalAirport.Name,
                            FareClass = f.FlightSegment.BookingClasses?.FirstOrDefault().Cabin.Name
                        }).ToList(),
                        Pnr = reservationDetail.Detail.ConfirmationID,
                        Price = Convert.ToInt32(price * 100),
                        ReservationType = "standard",
                        TripType = tripType,
                        Origin = reservationDetail.Detail.FlightSegments?.FirstOrDefault()?.FlightSegment?.DepartureAirport?.IATACode,
                        Destination = reservationDetail.Detail.FlightSegments?.LastOrDefault(f => f?.TripNumber == reservationDetail.Detail.FlightSegments?.FirstOrDefault()?.TripNumber)?.FlightSegment?.ArrivalAirport?.IATACode
                    }
                },
                OrderAmount = Convert.ToInt32(price * 100),
                Travelers = reservationDetail.Detail.Travelers.Select((t, index) =>
               new MOBULTraveler
               {
                   Index = index,
                   FirstName = t.Person.GivenName,
                   LastName = t.Person.Surname,
                   DateOfBirth = DateTime.Parse(t.Person.DateOfBirth).ToString("MM/dd/yyyy")
               }).ToList(),
                OrderLines = products.Select(p =>
                new MOBULOrderLine
                {
                    Name = p.ProdDescription,
                    Price = Convert.ToInt32(p.ProdTotalPrice.ToDecimal() * 100)
                }).ToList(),
                Email = reservationDetail.Detail.Travelers.FirstOrDefault()?.Person?.Contact?.Emails?.FirstOrDefault()?.Address,
                PhoneNumber = ExcludeCountryCodeFrmPhoneNumber(reservationDetail.Detail.Travelers.FirstOrDefault()?.Person?.Contact?.PhoneNumbers?.FirstOrDefault()?.PhoneNumber, GetCountryCode(reservationDetail.Detail.Travelers.FirstOrDefault()?.Person?.Contact?.PhoneNumbers?.FirstOrDefault()?.CountryAccessCode))
            };
        }
        public string ExcludeCountryCodeFrmPhoneNumber(string phonenumber, string countrycode)
        {
            try
            {
                Int64 _phonenumber;
                if (!string.IsNullOrEmpty(phonenumber)) phonenumber = phonenumber.Replace(" ", "");
                if (Int64.TryParse(phonenumber, out _phonenumber))
                {
                    if (!string.IsNullOrEmpty(countrycode))
                    {
                        var phonenumbercountrycode = phonenumber.Substring(0, countrycode.Length);
                        if (string.Equals(countrycode, phonenumbercountrycode, StringComparison.OrdinalIgnoreCase))
                        {
                            return phonenumber.Remove(0, countrycode.Length);
                        }
                    }
                }
            }
            catch
            { return string.Empty; }
            return phonenumber;
        }
        public string GetCountryCode(string countryaccesscode)
        {
            var countrycode = string.Empty;
            try
            {
                var _countries = LoadCountries();
                //if (_countries != null && _countries.Any())
                //{
                //    countrycode = _countries.FirstOrDefault(x => (x.Length == 3 && string.Equals
                //    (countryaccesscode, x[0], StringComparison.OrdinalIgnoreCase)))[2];
                //    countrycode = countrycode.Replace(" ", "");
                //}
            }
            catch { return countrycode; }

            return countrycode;
        }
        public List<MOBSHOPPrice> AddGrandTotalIfNotExist(List<MOBSHOPPrice> prices, double amount, string flow)
        {
            if (prices == null)
            {
                prices = new List<MOBSHOPPrice>();
            }
            if (!prices.Exists(p => p.DisplayType == "GRAND TOTAL"))
            {
                prices.Add(ShopStaticUtility.BuildGrandTotalPriceForReservation(amount));
            }
            return prices;
        }
        public bool IsCorporateTraveler(Collection<Characteristic> characteristics)
        {
            if (!characteristics.IsNullOrEmpty())
            {
                return characteristics.Any(c => !c.Code.IsNullOrEmpty() && c.Code.Equals("IsValidCorporateTravel", StringComparison.OrdinalIgnoreCase) &&
                                          !c.Value.IsNullOrEmpty() && c.Value.Equals("True"));
            }
            return false;
        }
        public MOBItem GetFareLockViewResPaymentCaptions(string id, string currentValue)
        {
            if (id.IsNullOrEmpty() || currentValue.IsNullOrEmpty())
                return null;

            var captions = new MOBItem()
            {
                Id = id,
                CurrentValue = currentValue
            };
            return captions;
        }
        public async Task<List<Section>> GetPaymentMessagesForWLPNRViewRes(FlightReservationResponse flightReservationResponse, Collection<ReservationFlightSegment> FlightSegments, string Flow)
        {
            var pcuWaitListManageResMessage = new List<Section>();
            bool isPCUPurchase = Flow.Equals(FlowType.VIEWRES.ToString()) && !flightReservationResponse.IsNull() && !flightReservationResponse.ShoppingCart.IsNull() ? flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES").Select(x => x.Product.FirstOrDefault().Code).Distinct().Any(x => x.Equals("PCU")) : false;
            if (_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isPCUPurchase && IsWaitListPNRFromCharacteristics(FlightSegments))
            {
                var getPCUSegmentFromTravelOptions = getPCUSegments(flightReservationResponse, Flow);
                var segments = getPCUSegmentFromTravelOptions != null ? getPCUSegmentFromTravelOptions.Select(x => x.SegmentNumber).Distinct().ToList() : null;
                var getPCUFlightSegments = segments != null ? FlightSegments.Where(y => y != null && y.FlightSegment != null && HasPCUSelectedSegments(segments, y.FlightSegment.SegmentNumber)).ToCollection() : null;
                var getAllPCUSegmentsIncludingWL = IsSelectedPCUWaitListSegment(getPCUFlightSegments, FlightSegments);
                var isWaitListPCU = IsWaitListPNRFromCharacteristics(getAllPCUSegmentsIncludingWL);
                if (isWaitListPCU)
                {
                    var isUPP = isPCUUPPWaitListSegment(getPCUSegmentFromTravelOptions, FlightSegments);
                    var refundMessages = isUPP ? await _productInfoHelper.GetCaptions("PI_PCUWaitList_RefundMessage_UPPMessage") : await _productInfoHelper.GetCaptions("PI_PCUWaitList_RefundMessage_GenericMessage");
                    pcuWaitListManageResMessage = AssignRefundMessage(refundMessages);
                }
            }
            return pcuWaitListManageResMessage.Any() ? pcuWaitListManageResMessage : null;
        }
        public bool isPCUUPPWaitListSegment(List<SubItem> PCUTravelOptions, Collection<ReservationFlightSegment> FlightSegments)
        {
            var hasPCUUPPSegment = PCUTravelOptions.Where(t => t != null && t.Description != null && t.Description.ToUpper().Equals("UNITED PREMIUM PLUS")).Select(t => t.SegmentNumber).Distinct().ToCollection();
            bool isBusiness = false;
            if (!hasPCUUPPSegment.IsNull() && hasPCUUPPSegment.Any())
            {
                foreach (var segment in hasPCUUPPSegment)
                {
                    var selectedSegment = FlightSegments.Where(y => y != null && y.FlightSegment != null && y.FlightSegment.SegmentNumber == Convert.ToInt32(segment)).FirstOrDefault().TripNumber;
                    var flightSegmentCharacteristics = selectedSegment != null ? FlightSegments.Where(x => !x.TripNumber.IsNullOrEmpty() && !selectedSegment.IsNullOrEmpty() && x.TripNumber == selectedSegment && x.BookingClass != null && x.BookingClass.Code != null && (x.BookingClass.Code.Equals("PZ") || x.BookingClass.Code.Equals("PN"))).Select(t => t.Characteristic).FirstOrDefault() : null;
                    isBusiness = flightSegmentCharacteristics != null && flightSegmentCharacteristics.Any() ? flightSegmentCharacteristics.Any(p => p != null && !p.Code.IsNullOrEmpty() && !p.Value.IsNullOrEmpty() && p.Code.Equals("Waitlisted", StringComparison.OrdinalIgnoreCase) && p.Value.Equals("True", StringComparison.OrdinalIgnoreCase)) : false;
                    if (isBusiness)
                        return isBusiness;
                }
            }
            return isBusiness;
        }
        public static Collection<ReservationFlightSegment> IsSelectedPCUWaitListSegment(Collection<ReservationFlightSegment> PCUFlightSegments, Collection<ReservationFlightSegment> FlightSegments)
        {
            return FlightSegments.Where(x => x != null && x.FlightSegment != null && isFlightMatching(x.FlightSegment.DepartureAirport.IATACode, x.FlightSegment.ArrivalAirport.IATACode, x.FlightSegment.FlightNumber, x.EstimatedDepartureTime, PCUFlightSegments)).ToCollection();
        }
        private static bool isFlightMatching(string origin, string destination, string flightNumber, string flightDate, Collection<ReservationFlightSegment> PCUFlightSegments, BookingClass BookingClass = null, bool isUPP = false)
        {
            return PCUFlightSegments.Any(x => x != null && x.FlightSegment != null && x.FlightSegment.DepartureAirport.IATACode == origin &&
                                     x.FlightSegment.ArrivalAirport.IATACode == destination &&
                                     x.FlightSegment.FlightNumber == flightNumber);
        }
        public List<SubItem> getPCUSegments(FlightReservationResponse flightReservationResponse, string Flow)
        {
            var segments = flightReservationResponse.DisplayCart.TravelOptions.Where(d => d != null && d.Key == "PCU").SelectMany(x => x.SubItems).Where(x => ShopStaticUtility.ShouldIgnoreAmount(x) ? true : x.Amount != 0).ToList();
            if (segments != null && segments.Any())
                segments.OrderBy(x => x.SegmentNumber).GroupBy(x => x.SegmentNumber);
            return segments.Any() ? segments : null;
        }
        private static bool HasPCUSelectedSegments(List<string> segments, int segmentNumber)
        {
            return segments.Any(x => x != null && Convert.ToInt32(x) == segmentNumber);
        }

        public List<Section> AssignRefundMessage(List<MOBItem> refundMessages)
        {
            List<Section> pcuWaitListManageResMessage = new List<Section>();
            if (refundMessages != null && refundMessages.Any())
            {
                var PCUWaitListMessage = new Section()
                {
                    Text1 = refundMessages.Where(x => x.Id == "HEADER").Select(x => x.CurrentValue).FirstOrDefault().ToString(),
                    Text2 = refundMessages.Where(x => x.Id == "BODY").Select(x => x.CurrentValue).FirstOrDefault().ToString()
                };
                pcuWaitListManageResMessage.Add(PCUWaitListMessage);
            }
            return pcuWaitListManageResMessage;
        }
        private bool IsWaitListPNRFromCharacteristics(Collection<ReservationFlightSegment> FlightSegments)
        {
            var flightSegmentCharacteristics = FlightSegments.Where(t => t != null).SelectMany(t => t.Characteristic).ToCollection();
            return flightSegmentCharacteristics.Any(p => p != null && !p.Code.IsNullOrEmpty() && !p.Value.IsNullOrEmpty() && p.Code.Equals("Waitlisted", StringComparison.OrdinalIgnoreCase) && p.Value.Equals("True", StringComparison.OrdinalIgnoreCase));
        }
        public async Task<List<CacheCountry>> LoadCountries()
        {
            List<CacheCountry> Countries = new List<CacheCountry>();
            try
            {
                var CountriesList = await _cachingService.GetCache<string>("CountriesContent", "CountriesContent01").ConfigureAwait(false);

                if (!string.IsNullOrEmpty(CountriesList))
                {
                    return JsonConvert.DeserializeObject<List<CacheCountry>>(CountriesList);
                }
            }
            catch { return Countries; }
            return default;
        }
        public FormofPaymentOption UpliftAsFormOfPayment(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart)
        {
            if (_configuration.GetValue<bool>("EnableUpliftPayment"))
            {
                if (IsEligibileForUplift(reservation, shoppingCart))
                {
                    return new FormofPaymentOption
                    {
                        Category = "UPLIFT",
                        FoPDescription = "Pay Monthly",
                        Code = "UPLIFT",
                        FullName = "Pay Monthly",
                        DeleteOrder = shoppingCart?.FormofPaymentDetails?.FormOfPaymentType?.ToUpper() != MOBFormofPayment.Uplift.ToString().ToUpper()
                    };
                }
            }
            return null;
        }

        private bool IsEligibileForUplift(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart)
        {
            if (shoppingCart?.Flow?.ToUpper() == FlowType.VIEWRES.ToString().ToUpper())
            {
                return HasEligibleProductsForUplift(shoppingCart.TotalPrice, shoppingCart.Products);
            }

            if (!_configuration.GetValue<bool>("EnableUpliftPayment"))
                return false;

            if (reservation == null || reservation.Prices == null || shoppingCart == null || shoppingCart?.Flow != FlowType.BOOKING.ToString())
                return false;

            if (reservation.ShopReservationInfo?.IsCorporateBooking ?? false)
                return false;

            if (shoppingCart.Products?.Any(p => p?.Code == "FLK") ?? false)
                return false;

            if (!_configuration.GetValue<bool>("DisableFixForUpliftFareLockDefect"))
            {
                if (shoppingCart.Products?.Any(p => p?.Code?.ToUpper() == "FARELOCK") ?? false)
                    return false;
            }

            if (reservation.Prices.Any(p => "TOTALPRICEFORUPLIFT".Equals(p.DisplayType, StringComparison.CurrentCultureIgnoreCase) && p.Value >= MinimumPriceForUplift && p.Value <= MaxmimumPriceForUplift) &&
               (shoppingCart?.SCTravelers?.Any(t => t?.TravelerTypeCode == "ADT" || t?.TravelerTypeCode == "SNR") ?? false))
            {
                return true;
            }
            return false;
        }

        private bool HasEligibleProductsForUplift(string totalPrice, List<ProdDetail> products)
        {
            decimal.TryParse(totalPrice, out decimal price);
            if (price >= MinimumPriceForUplift && price <= MaxmimumPriceForUplift)
            {
                var eligibleProductsForUplift = _configuration.GetValue<string>("EligibleProductsForUpliftInViewRes").Split(',');
                if (eligibleProductsForUplift.Any())
                {
                    return products.Any(p => eligibleProductsForUplift.Contains(p.Code));
                }
            }

            return false;
        }
        private int MinimumPriceForUplift
        {
            get
            {
                var minimumAmountForUplift = _configuration.GetValue<string>("MinimumPriceForUplift");
                if (string.IsNullOrEmpty(minimumAmountForUplift))
                    return 300;

                int.TryParse(minimumAmountForUplift, out int upliftMinAmount);
                return upliftMinAmount;
            }
        }

        private int MaxmimumPriceForUplift
        {
            get
            {
                var maximumAmountForUplift = _configuration.GetValue<string>("MaximumPriceForUplift");
                if (string.IsNullOrEmpty(maximumAmountForUplift))
                    return 150000;

                int.TryParse(maximumAmountForUplift, out int upliftMaxAmount);
                return upliftMaxAmount;
            }
        }
        public bool IsCheckFareLockUsingProductCode(MOBShoppingCart shoppingCart)
        {
            return _configuration.GetValue<bool>("EnableFareLockPurchaseViewRes") && shoppingCart.Products.Any(o => (o != null && o.Code != null && o.Code.Equals("FLK_VIEWRES", StringComparison.OrdinalIgnoreCase)));
        }
        public async Task<List<MOBItem>> GetFareLockCaptions(United.Service.Presentation.ReservationModel.Reservation reservation, string email)
        {
            if (reservation.IsNullOrEmpty() || email.IsNullOrEmpty())
                return null;

            var attention = string.Format(_configuration.GetValue<string>("EmailConfirmationMessage"), email);
            var subAttention = await GetFareLockAttentionMessageViewRes(reservation);

            List<MOBItem> captions = new List<MOBItem>();
            if (!attention.IsNullOrEmpty())
            {
                captions.Add(GetFareLockViewResPaymentCaptions("ConfirmationPage_Attention", attention));
                if (!subAttention.IsNullOrEmpty())
                {
                    captions.Add(GetFareLockViewResPaymentCaptions("ConfirmationPage_SubAttention", subAttention));
                }
                var confirmationPageCaptions = await _productInfoHelper.GetCaptions("ConfirmationPage_ViewRes_Captions");
                confirmationPageCaptions.ForEach(p =>
                {
                    if (!p.IsNullOrEmpty())
                    {
                        captions.Add(p);
                    }
                });
                captions.Add(GetFareLockViewResPaymentCaptions("ConfirmationPage_Email", email));
                captions.Add(GetFareLockViewResPaymentCaptions("PaymentPage_ProductCode", "FLK_ViewRes"));
            }

            // To find if PNR is corporate travel
            bool isCorporateFareLock = IsCorporateTraveler(reservation.Characteristic);
            if (isCorporateFareLock)
            {
                var priceText = _configuration.GetValue<string>("CorporateRateText");
                string vendorName = ShopStaticUtility.GetCharactersticValue(reservation.Characteristic, "CorporateTravelVendorName");
                if (!priceText.IsNullOrEmpty() && !vendorName.IsNullOrEmpty())
                {
                    captions.Add(GetFareLockViewResPaymentCaptions("Corporate_PriceBreakDownText", priceText));
                    vendorName = string.Format(_configuration.GetValue<string>("CorporateBookingConfirmationMessage"), vendorName);
                    var companyName = string.Empty;
                    if (!string.IsNullOrEmpty(vendorName)) // Added code to camel case company name as PNR service will return all caps.
                    {
                        var splitCamel = vendorName.TrimEnd().Split(' ');
                        if (!splitCamel.IsNullOrEmpty() && splitCamel.Any())
                        {
                            foreach (var camel in splitCamel)
                            {
                                companyName = companyName.IsNullOrEmpty() ? FirstLetterToUpperCase(camel) : companyName + " " + FirstLetterToUpperCase(camel);
                            }
                        }
                    }
                    captions.Add(GetFareLockViewResPaymentCaptions("Corporate_PNRHeaderText", companyName));
                }
            }

            return captions;
        }

        private async Task<string> GetFareLockAttentionMessageViewRes(Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation.IsNullOrEmpty())
                return null;
            string foreignCountry = string.Empty; bool caExists = false; bool meExists = false; bool internationPNR = false;
            var tripnum = string.Empty;
            reservation.FlightSegments.ForEach(p =>
            {
                if (!p.IsNullOrEmpty() && !p.FlightSegment.IsNullOrEmpty() && !p.FlightSegment.IsInternational.IsNullOrEmpty() && p.FlightSegment.IsInternational.ToUpper().Equals("TRUE"))
                {
                    if (tripnum.IsNullOrEmpty())
                    {
                        tripnum = p.TripNumber;
                    }
                    meExists = p.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode.Equals("MX") ? true : false;
                    caExists = p.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode.Equals("CA") ? true : false;
                    internationPNR = true;
                }
            });

            if (internationPNR)
            {
                var JourneyType = ShopStaticUtility.GetJourneyTypeDescription(reservation);
                var flightSegment = new FlightSegment();
                if (JourneyType.Equals("Multicity") && !tripnum.IsNullOrEmpty())
                {
                    flightSegment = reservation.FlightSegments.FirstOrDefault(p => (!p.IsNullOrEmpty() && !p.FlightSegment.IsNullOrEmpty() && p.TripNumber.Equals(tripnum))).FlightSegment;
                }
                else if (JourneyType.Equals("Roundtrip"))
                {
                    flightSegment = reservation.FlightSegments.FirstOrDefault(p => (!p.IsNullOrEmpty() && !p.FlightSegment.IsNullOrEmpty())).FlightSegment;
                }
                else
                {
                    flightSegment = reservation.FlightSegments.LastOrDefault(p => (!p.IsNullOrEmpty() && !p.FlightSegment.IsNullOrEmpty())).FlightSegment;
                }
                foreignCountry = !flightSegment.IsNullOrEmpty() && !flightSegment.ArrivalAirport.IsNullOrEmpty() && !flightSegment.ArrivalAirport.IATACode.IsNullOrEmpty()
                                 ? await GetAirportName(flightSegment.ArrivalAirport.IATACode) : string.Empty;
            }

            if (!string.IsNullOrEmpty(foreignCountry))
            {
                string docTitles = "'TripAdvisoryForSelectedOriginDestination','TripAdvisoryForCanada','TripAdvisoryForMexico'";
                List<United.Definition.MOBLegalDocument> docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(docTitles, _headers.ContextValues.TransactionId, true);

                if (docs != null && docs.Count > 0)
                {
                    if (caExists && docs.Find(item => item.Title == "TripAdvisoryForCanada") != null)
                    {
                        return docs.Find(item => item.Title == "TripAdvisoryForCanada").LegalDocument;
                    }
                    else if (meExists && docs.Find(item => item.Title == "TripAdvisoryForMexico") != null)
                    {
                        return docs.Find(item => item.Title == "TripAdvisoryForMexico").LegalDocument;
                    }
                    else if (docs.Find(item => item.Title == "TripAdvisoryForSelectedOriginDestination") != null)
                    {
                        return string.Format(docs.Find(item => item.Title == "TripAdvisoryForSelectedOriginDestination").LegalDocument, foreignCountry);
                    }
                }
            }

            return string.Empty;
        }


        public string GetFlightShareMessageViewRes(Service.Presentation.ReservationModel.Reservation reservation)
        {
            string reservationShareMessage = string.Empty;

            if (!reservation.IsNullOrEmpty() && !reservation.FlightSegments.IsNullOrEmpty())
            {
                #region Build Reservation Share Message 
                var originSegment = reservation.FlightSegments.FirstOrDefault(k => !k.IsNullOrEmpty() && !k.FlightSegment.IsNullOrEmpty()).FlightSegment;
                var arrivalSegment = reservation.FlightSegments.LastOrDefault(k => !k.IsNullOrEmpty() && !k.FlightSegment.IsNullOrEmpty()).FlightSegment;
                if (!originSegment.IsNullOrEmpty() && !arrivalSegment.IsNullOrEmpty())
                {
                    var initialOrigin = originSegment.DepartureAirport.IATACode;
                    var finalDestination = arrivalSegment.ArrivalAirport.IATACode;
                    var travelersText = !reservation.Travelers.IsNullOrEmpty() ? (reservation.Travelers.Count.ToString() + " " + (reservation.Travelers.Count > 1 ? "travelers" : "traveler")) : string.Empty;
                    var flightDatesText = string.Empty;
                    if (!originSegment.DepartureDateTime.IsNullOrEmpty() && !arrivalSegment.ArrivalDateTime.IsNullOrEmpty())
                    {
                        flightDatesText = DateTime.Parse(originSegment.DepartureDateTime.Replace("\\", ""), CultureInfo.InvariantCulture).ToString("MMM dd") + (reservation.FlightSegments.Count > 1 ? " - " + DateTime.Parse(arrivalSegment.ArrivalDateTime.Replace("\\", ""), CultureInfo.InvariantCulture).ToString("MMM dd") : "");
                    }
                    var searchType = ShopStaticUtility.GetJourneyTypeDescription(reservation);

                    string flightNumbers = string.Empty, viaAirports = string.Empty, cabinType = string.Empty;
                    var currenttripnum = string.Empty;

                    foreach (var cslsegment in reservation.FlightSegments)
                    {
                        if (!cslsegment.TripNumber.IsNullOrEmpty() && !currenttripnum.Equals(cslsegment.TripNumber))
                        {
                            currenttripnum = cslsegment.TripNumber;
                            var tripAllSegments = reservation.FlightSegments.Where(p => p != null && p.TripNumber != null && p.TripNumber == cslsegment.TripNumber).ToList();
                            if (string.IsNullOrEmpty(cabinType))
                            {
                                var bookingClass = tripAllSegments.FirstOrDefault(k => !k.IsNullOrEmpty() && !k.BookingClass.IsNullOrEmpty()).BookingClass;
                                cabinType = !bookingClass.IsNullOrEmpty() && !bookingClass.Cabin.IsNullOrEmpty() && !bookingClass.Cabin.Name.IsNullOrEmpty()
                                            ? bookingClass.Cabin.Name.ToUpper().Equals("COACH") ? "Economy" : bookingClass.Cabin.Name : string.Empty;
                            }
                            tripAllSegments.ForEach(k =>
                            {
                                if (!k.IsNullOrEmpty() && !k.FlightSegment.IsNullOrEmpty() && !k.FlightSegment.FlightNumber.IsNullOrEmpty()
                                && !k.FlightSegment.DepartureAirport.IsNullOrEmpty() && !k.FlightSegment.ArrivalAirport.IsNullOrEmpty())
                                {
                                    flightNumbers = flightNumbers + "," + k.FlightSegment.FlightNumber;
                                    if (k.FlightSegment.DepartureAirport.IATACode != initialOrigin && k.FlightSegment.ArrivalAirport.IATACode != finalDestination)
                                    {
                                        if (string.IsNullOrEmpty(viaAirports))
                                        {
                                            viaAirports = " via ";
                                        }
                                        viaAirports = viaAirports + k.FlightSegment.ArrivalAirport.IATACode + ",";
                                    }
                                }
                            });
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
                    reservationShareMessage = string.Format(_configuration.GetValue<string>("Booking20ShareMessage"), flightDatesText, travelersText, searchType, cabinType, flightNumbers.Trim(','), initialOrigin, finalDestination, viaAirports.Trim(','));
                    #endregion
                }
            }
            return reservationShareMessage;
        }

        public List<MOBMobileCMSContentMessages> getEtcBalanceAttentionConfirmationMessages(double? balanceAmount)
        {
            MOBMobileCMSContentMessages message = new MOBMobileCMSContentMessages();
            message.ContentFull = String.Format(_configuration.GetValue<string>("ETCBalanceConfirmationMessage"), String.Format("{0:0.00}", balanceAmount));
            message.HeadLine = "Attention";
            return new List<MOBMobileCMSContentMessages> { message };
        }

        public void AssignTotalAndCertificateItemsToPrices(MOBShoppingCart shoppingcart)
        {
            if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres"))
            {
                double total = 0;
                //rebuilding Grandtotal
                total = shoppingcart.Products.Sum(p => Convert.ToDouble(p.ProdTotalPrice));
                shoppingcart.Prices = new List<MOBSHOPPrice>();
                if (shoppingcart.TotalPrice != "")
                {
                    //total = Convert.ToDouble(shoppingcart.TotalPrice);
                    shoppingcart.Prices.Add(ShopStaticUtility.BuildGrandTotalPriceForReservation(total));
                }

                if (shoppingcart?.FormofPaymentDetails?.TravelCertificate?.Certificates != null)
                {
                    MOBSHOPPrice certificatePrice = new MOBSHOPPrice();
                    ConfigUtility.UpdateCertificateAmountInTotalPrices(shoppingcart.Prices, shoppingcart.FormofPaymentDetails.TravelCertificate.TotalRedeemAmount);
                }
                double grandTotal = getGrandTotalFromPrices(shoppingcart.Prices);
                shoppingcart.TotalPrice = string.Format("{0:0.00}", grandTotal);
                shoppingcart.DisplayTotalPrice = Decimal.Parse(grandTotal.ToString()).ToString("c");
            }
        }
        private double getGrandTotalFromPrices(List<MOBSHOPPrice> prices)
        {
            var grandTotal = prices.Find(p => p.DisplayType == "GRAND TOTAL");
            return grandTotal == null ? 0 : grandTotal.Value;
        }
        private MOBVormetricKeys GetPersistentTokenFromCSLDatavaultResponse(string jsonResponseFromCSL, string sessionID)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (!string.IsNullOrEmpty(jsonResponseFromCSL))
            {
                //CslDataVaultResponse response = JsonSerializer.NewtonSoftDeserialize<CslDataVaultResponse>(jsonResponseFromCSL);
                CslDataVaultResponse response = JsonConvert.DeserializeObject<CslDataVaultResponse>(jsonResponseFromCSL);
                if (response != null && response.Responses != null && response.Responses[0].Error == null && response.Responses[0].Message != null && response.Responses[0].Message.Count > 0 && response.Responses[0].Message[0].Code.Trim() == "0")
                {
                    var creditCard = response.Items[0] as Service.Presentation.PaymentModel.CreditCard;
                    vormetricKeys.PersistentToken = creditCard.PersistentToken;
                    vormetricKeys.SecurityCodeToken = creditCard.SecurityCodeToken;
                    vormetricKeys.CardType = creditCard.Code;
                    _logger.LogInformation("GenerateCCTokenWithDataVault - DeSerialized Response:DeSerialized Response", sessionID, _headers.ContextValues.Application.Id, _headers.ContextValues.Application.Version.Major, _headers.ContextValues.DeviceId, response);

                }
                else
                {
                    if (response.Responses[0].Error != null && response.Responses[0].Error.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Responses[0].Error)
                        {
                            errorMessage = errorMessage + " " + error.Text;
                        }
                        if (!_configuration.GetValue<bool>("DisableSoftErrorLogForCCDataVaultRSA_MOBILE20913"))
                            _logger.LogError("GetPersistentTokenUsingAccountNumberToken UnitedException: {@response}", response);
                        else
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

            return vormetricKeys;
        }

        public async Task<MOBVormetricKeys> GetVormetricPersistentTokenForBooking(MOBCreditCard requestCreditCard, string sessionId, string token)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (_configuration.GetValue<bool>("VormetricTokenMigration"))
            {
                Reservation persistReservation = new Reservation();
                persistReservation = await _sessionHelperService.GetSession<Reservation>(_headers.ContextValues.SessionId, persistReservation.ObjectName, new List<string> { _headers.ContextValues.SessionId, persistReservation.ObjectName }).ConfigureAwait(false);
                if (persistReservation == null && string.IsNullOrEmpty(requestCreditCard.Key))
                {
                    ProfileFOPCreditCardResponse profilePersist = new ProfileFOPCreditCardResponse();
                    profilePersist = await _sessionHelperService.GetSession<ProfileFOPCreditCardResponse>(sessionId, profilePersist.ObjectName, new List<string> { sessionId, profilePersist.ObjectName }).ConfigureAwait(false);

                    if (profilePersist != null && profilePersist.Response != null && profilePersist.Response.SelectedCreditCard != null && !string.IsNullOrEmpty(profilePersist.Response.SelectedCreditCard.PersistentToken))
                    {
                        vormetricKeys.PersistentToken = profilePersist.Response.SelectedCreditCard.PersistentToken;
                        vormetricKeys.SecurityCodeToken = profilePersist.Response.SelectedCreditCard.SecurityCodeToken;
                        vormetricKeys.CardType = profilePersist.Response.SelectedCreditCard.CardType;
                    }
                }
                else if (persistReservation.CreditCards != null && persistReservation.CreditCards.Count > 0)   //MOBILE-1193 Booking - Credit Card Checkout with MP Sign IN -Saved FOP
                {
                    if (persistReservation.IsSignedInWithMP) //For MP Sign in
                    {
                        foreach (MOBCreditCard creditCard in persistReservation.CreditCards)
                        {
                            if (requestCreditCard != null && creditCard.Key == requestCreditCard.Key)
                            {
                                vormetricKeys.PersistentToken = creditCard.PersistentToken;
                                vormetricKeys.SecurityCodeToken = creditCard.SecurityCodeToken;
                                vormetricKeys.CardType = creditCard.CardType;
                                break;
                            }
                        }

                        //MOBILE-2490 
                        if (String.IsNullOrEmpty(vormetricKeys.PersistentToken) && !String.IsNullOrEmpty(requestCreditCard.Key))
                        {
                            vormetricKeys = await GetVormetricPersistentTokenFromProfile(requestCreditCard.Key, sessionId, token);
                        }
                    }
                    else if (persistReservation.CreditCards.Exists(cc => cc.AccountNumberToken == requestCreditCard.AccountNumberToken))//MOBILE-1192 Booking - Credit Card Checkout as Guest
                    {
                        var cc = persistReservation.CreditCards.First(c => c.AccountNumberToken == requestCreditCard.AccountNumberToken);
                        vormetricKeys.PersistentToken = cc.PersistentToken;
                        vormetricKeys.SecurityCodeToken = cc.SecurityCodeToken;
                        vormetricKeys.CardType = cc.CardType;
                    }
                }

                if (String.IsNullOrEmpty(vormetricKeys.PersistentToken) && (!string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(_token)) && !string.IsNullOrEmpty(requestCreditCard.AccountNumberToken))
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        token = _token;
                    }
                    vormetricKeys = await GetPersistentTokenUsingAccountNumberToken(requestCreditCard.AccountNumberToken, sessionId, token);
                }

                if (String.IsNullOrEmpty(vormetricKeys.PersistentToken))
                {
                    LogNoPersistentTokenInCSLResponseForVormetricPayment(sessionId);
                }
            }

            return vormetricKeys;
        }

        private void LogNoPersistentTokenInCSLResponseForVormetricPayment(string sessionId, string Message = "Unable to retieve PersistentToken")
        {
            _logger.LogWarning("LogNoPersistentTokenInCSLResponseForVormetricPayment-PERSISTENTTOKENNOTFOUND Error {exception} {sessionid}", Message, sessionId);
            //No need to block the flow as we are calling DataVault for Persistent Token during the final payment
        }

        public async Task<MOBVormetricKeys> GetVormetricPersistentTokenFromProfile(string request_CreditCard_Key, string sessionId, string token)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            ProfileResponse profilePersistCopy = new ProfileResponse();
            profilePersistCopy = await _sessionHelperService.GetSession<ProfileResponse>(sessionId, profilePersistCopy.ObjectName, new List<string> { sessionId, profilePersistCopy.ObjectName }).ConfigureAwait(false);
            if (profilePersistCopy != null && profilePersistCopy.Response != null
                && profilePersistCopy.Response.Profiles != null && profilePersistCopy.Response.Profiles.Count > 0
                && profilePersistCopy.Response.Profiles[0].Travelers != null && profilePersistCopy.Response.Profiles[0].Travelers.Count > 0
                && profilePersistCopy.Response.Profiles[0].Travelers[0].CreditCards != null && profilePersistCopy.Response.Profiles[0].Travelers[0].CreditCards.Count > 0)
            {
                foreach (MOBCreditCard creditCard in profilePersistCopy.Response.Profiles[0].Travelers[0].CreditCards)
                {
                    if (creditCard.Key == request_CreditCard_Key)
                    {
                        vormetricKeys.PersistentToken = creditCard.PersistentToken;
                        vormetricKeys.SecurityCodeToken = creditCard.SecurityCodeToken;
                        vormetricKeys.AccountNumberToken = creditCard.AccountNumberToken;
                        vormetricKeys.CardType = creditCard.CardType;
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(vormetricKeys.PersistentToken) && !string.IsNullOrEmpty(token))
            {
                vormetricKeys = await GetPersistentTokenUsingAccountNumberToken(vormetricKeys.AccountNumberToken, sessionId, token);
            }

            if (String.IsNullOrEmpty(vormetricKeys.PersistentToken))
            {
                LogNoPersistentTokenInCSLResponseForVormetricPayment(sessionId);
            }

            return vormetricKeys;
        }

        public async Task<MOBVormetricKeys> GetVormetricPersistentTokenForViewRes(MOBCreditCard persistedCreditCard, string sessionId, string token)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (_configuration.GetValue<bool>("VormetricTokenMigration"))
            {
                //MOBILE-1243 CFOP MP Sign IN - ViewRes - TPI - CC saved
                ProfileFOPCreditCardResponse profilePersist = new ProfileFOPCreditCardResponse();
                profilePersist = await _sessionHelperService.GetSession<ProfileFOPCreditCardResponse>(sessionId, profilePersist.ObjectName, new List<string> { sessionId, profilePersist.ObjectName }).ConfigureAwait(false);
                if (persistedCreditCard != null && !string.IsNullOrEmpty(persistedCreditCard.Key) &&
                    profilePersist != null &&
                    profilePersist.Response != null &&
                    profilePersist.Response.Profiles != null &&
                    profilePersist.Response.Profiles.Count > 0 &&
                    profilePersist.Response.Profiles[0].Travelers != null &&
                    profilePersist.Response.Profiles[0].Travelers.Count > 0 &&
                    profilePersist.Response.Profiles[0].Travelers[0].CreditCards != null &&
                    profilePersist.Response.Profiles[0].Travelers[0].CreditCards.Count > 0 &&
                        (profilePersist.Response.Profiles[0].Travelers[0].CreditCards.Exists(p => p.Key == persistedCreditCard.Key) ||
                        profilePersist.Response.Profiles[0].Travelers[0].CreditCards.Exists(p => p.AccountNumberToken == persistedCreditCard.AccountNumberToken))

                    )
                {
                    var cc = profilePersist.Response.Profiles[0].Travelers[0].CreditCards.FirstOrDefault(p => p.Key == persistedCreditCard.Key);
                    if (cc == null)
                    {
                        cc = profilePersist.Response.Profiles[0].Travelers[0].CreditCards.FirstOrDefault(p => p.AccountNumberToken == persistedCreditCard.AccountNumberToken);
                    }
                    vormetricKeys.PersistentToken = cc.PersistentToken;
                    vormetricKeys.AccountNumberToken = cc.AccountNumberToken;
                    vormetricKeys.SecurityCodeToken = cc.SecurityCodeToken;
                    vormetricKeys.CardType = cc.CardType;
                }
                else if (!string.IsNullOrEmpty(profilePersist?.Response?.SelectedCreditCard?.PersistentToken) &&
                   (_configuration.GetValue<bool>("DisableCompareAccountTokenNumber") ? true : profilePersist?.Response?.SelectedCreditCard?.AccountNumberToken == persistedCreditCard.AccountNumberToken))
                {
                    vormetricKeys.PersistentToken = profilePersist.Response.SelectedCreditCard.PersistentToken;
                    vormetricKeys.AccountNumberToken = profilePersist.Response.SelectedCreditCard.AccountNumberToken;
                    vormetricKeys.SecurityCodeToken = profilePersist.Response.SelectedCreditCard.SecurityCodeToken;
                    vormetricKeys.CardType = profilePersist.Response.SelectedCreditCard.CardType;
                }
                //MOBILE-1681 CFOP ON MP Sign IN - ViewRes - PCU - MasterPass
                else if (!string.IsNullOrEmpty(profilePersist?.customerProfileResponse?.SelectedCreditCard?.PersistentToken) &&
                    (_configuration.GetValue<bool>("DisableCompareAccountTokenNumber") ? true : profilePersist?.customerProfileResponse?.SelectedCreditCard?.AccountNumberToken == persistedCreditCard.AccountNumberToken))
                {
                    vormetricKeys.PersistentToken = profilePersist.customerProfileResponse.SelectedCreditCard.PersistentToken;
                    vormetricKeys.AccountNumberToken = profilePersist.customerProfileResponse.SelectedCreditCard.AccountNumberToken;
                    vormetricKeys.SecurityCodeToken = profilePersist.customerProfileResponse.SelectedCreditCard.SecurityCodeToken;
                    vormetricKeys.CardType = profilePersist.customerProfileResponse.SelectedCreditCard.CardType;
                }
                else if (persistedCreditCard != null && (!string.IsNullOrEmpty(persistedCreditCard.AccountNumberToken) || !string.IsNullOrEmpty(persistedCreditCard.PersistentToken)))
                {
                    vormetricKeys.PersistentToken = persistedCreditCard.PersistentToken;
                    vormetricKeys.AccountNumberToken = persistedCreditCard.AccountNumberToken;
                    vormetricKeys.SecurityCodeToken = persistedCreditCard.SecurityCodeToken;
                    vormetricKeys.CardType = persistedCreditCard.CardType;
                }
                //MOBILE-1238 CFOP Guest Flow - ViewRes - TPI - CC


                if (String.IsNullOrEmpty(vormetricKeys.PersistentToken) && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(vormetricKeys.AccountNumberToken))
                {
                    vormetricKeys = await GetPersistentTokenUsingAccountNumberToken(vormetricKeys.AccountNumberToken, sessionId, token);
                }

                if (String.IsNullOrEmpty(vormetricKeys.PersistentToken))
                {
                    LogNoPersistentTokenInCSLResponseForVormetricPayment(sessionId);
                }
            }

            return vormetricKeys;
        }
        public async Task<MOBVormetricKeys> GetPersistentTokenUsingAccountNumberToken(string accountNumberToke, string sessionId, string token)
        {
            string url = string.Format("/{0}/RSA", accountNumberToke);

            var cslResponse = await MakeHTTPCallAndLogIt(sessionId, _deviceId, "CSL-ChangeEligibleCheck", _application, token, url, string.Empty, true, false);

            return GetPersistentTokenFromCSLDatavaultResponse(cslResponse, sessionId);
        }
        private async Task<string> MakeHTTPCallAndLogIt(string sessionId, string deviceId, string action, MOBApplication application, string token, string url, string jsonRequest, bool isGetCall, bool isXMLRequest = false)
        {
            string jsonResponse = string.Empty;

            //string paypalCSLCallDurations = string.Empty;
            //string callTime4Tuning = string.Empty;

            #region//****Get Call Duration Code*******
            //Stopwatch cslCallDurationstopwatch1;
            //cslCallDurationstopwatch1 = new Stopwatch();
            //cslCallDurationstopwatch1.Reset();
            //cslCallDurationstopwatch1.Start();
            #endregion

            string applicationRequestType = isXMLRequest ? "xml" : "json";
            if (isGetCall)
            {
                //jsonResponse = HttpHelper.Get(url, "Application/json", token);
                jsonResponse = await _dataVaultService.GetPersistentToken(token, jsonRequest, url, sessionId).ConfigureAwait(false);

            }
            else
            {
                //jsonResponse = HttpHelper.Post(url, "application/" + applicationRequestType + "; charset=utf-8", token, jsonRequest, httpPostTimeOut, httpPostNumberOfRetry);
                jsonResponse = await _dataVaultService.PersistentToken(token, jsonRequest, url, sessionId).ConfigureAwait(false);

            }

            #region
            //if (cslCallDurationstopwatch1.IsRunning)
            //{
            //    cslCallDurationstopwatch1.Stop();
            //}
            //paypalCSLCallDurations = paypalCSLCallDurations + "|2=" + cslCallDurationstopwatch1.ElapsedMilliseconds.ToString() + "|"; // 2 = shopCSLCallDurationstopwatch1
            //callTime4Tuning = "|CSL =" + (cslCallDurationstopwatch1.ElapsedMilliseconds / (double)1000).ToString();
            #endregion

            //logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "Response", application.Id, application.Version.Major, deviceId, jsonResponse, false, false));
            //logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "CSS/CSL-CallDuration", application.Id, application.Version.Major, deviceId, "CSLResponse=" + (cslCallDurationstopwatch1.ElapsedMilliseconds / (double)1000).ToString(), false, false));
            return jsonResponse;
        }
        public async Task<MOBVormetricKeys> AssignPersistentTokenToCC(string accountNumberToken, string persistentToken, string securityCodeToken, string cardType, string sessionId, string action, int appId, string deviceID)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (_configuration.GetValue<bool>("VormetricTokenMigration"))
            {
                if ((string.IsNullOrEmpty(persistentToken) || string.IsNullOrEmpty(cardType)) && !string.IsNullOrEmpty(accountNumberToken) && !string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(accountNumberToken))
                {
                    vormetricKeys = await GetPersistentTokenUsingAccountNumberToken(accountNumberToken, sessionId, accountNumberToken);
                    persistentToken = vormetricKeys.PersistentToken;
                }

                if (!string.IsNullOrEmpty(persistentToken))
                {
                    vormetricKeys.PersistentToken = persistentToken;
                    vormetricKeys.SecurityCodeToken = securityCodeToken;
                    vormetricKeys.CardType = cardType;
                }
                else
                {
                    LogNoPersistentTokenInCSLResponseForVormetricPayment(sessionId);
                }
            }
            else
            {
                persistentToken = string.Empty;
            }

            return vormetricKeys;
        }
        public double getPCUProductPrice(MOBShoppingCart shoppingcart)
        {
            double pcuPrice = 0;
            if (shoppingcart.Products.Where(x => x.Code == "SEATASSIGNMENTS").Any())
            {
                foreach (ProductSegmentDetail segment in shoppingcart.Products.Where(x => x.Code == "SEATASSIGNMENTS").FirstOrDefault().Segments)
                {
                    foreach (ProductSubSegmentDetail subsegment in segment.SubSegmentDetails)
                    {
                        if (IsPCUProduct(subsegment.SeatCode))
                        {
                            pcuPrice += Convert.ToDouble(subsegment.Price);
                        }
                    }
                }
            }
            return pcuPrice;
        }

        private bool IsPCUProduct(string code)
        {
            switch (code.ToUpper().Trim())
            {
                case "UNITED FIRST":
                case "UNITED BUSINESS":
                case "UNITED POLARIS FIRST":
                case "UNITED POLARIS BUSINESS":
                case "UNITED PREMIUM PLUS":
                    return true;

                default: return false;
            }
        }
        private async Task<string> GetAirportName(string airportCode)
        {
            AirportDynamoDB airportDynamoDB = new AirportDynamoDB(_configuration, _dynamoDBService);
            return await airportDynamoDB.GetAirportName(airportCode, _headers.ContextValues.SessionId);
        }

        public bool IncludeTravelBankFOP(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelBankFOP")
                && GeneralHelper.isApplicationVersionGreater
                (appId, appVersion, "AndroidTravelBankFOPVersion", "iPhoneTravelBankFOPVersion", "", "", true, _configuration);
        }

        public async Task<double> GetTravelBankBalance(string sessionId)
        {
            MOBCPTraveler mobCPTraveler = await GetProfileOwnerTravelerCSL(sessionId);
            return mobCPTraveler?.MileagePlus?.TravelBankBalance > 0.00 ? mobCPTraveler.MileagePlus.TravelBankBalance : 0.00;
        }

        public async Task<MOBCPTraveler> GetProfileOwnerTravelerCSL(string sessionID)
        {
            ProfileResponse profilePersist = new ProfileResponse();
            profilePersist = await GetCSLProfileResponseInSession(sessionID);
            if (profilePersist != null && profilePersist.Response != null && profilePersist.Response.Profiles != null)
            {
                foreach (MOBCPTraveler mobCPTraveler in profilePersist.Response.Profiles[0].Travelers)
                {
                    if (mobCPTraveler.IsProfileOwner)
                    {
                        return mobCPTraveler;
                    }
                }
            }
            return null;
        }

        private async Task<ProfileResponse> GetCSLProfileResponseInSession(string sessionId)
        {
            ProfileResponse profile = new ProfileResponse();
            try
            {
                profile = await _sessionHelperService.GetSession<ProfileResponse>(sessionId, new ProfileResponse().ObjectName, new List<string> { sessionId, new ProfileResponse().ObjectName }).ConfigureAwait(false);
            }
            catch (System.Exception)
            {

            }
            return profile;
        }

        public bool IncludeTravelCredit(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelCredit") &&
                   GeneralHelper.isApplicationVersionGreater(appId, appVersion, "AndroidTravelCreditVersion", "iPhoneTravelCreditVersion", "", "", true, _configuration);
        }


        public Dictionary<string, string> GetAdditionalHeadersForMosaic(string flow)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            if (_configuration.GetValue<bool>("EnableAdditionalHeadersForMosaicInRFOP"))
            {
                if (flow?.ToUpper() == FlowType.BOOKING.ToString() || flow?.ToUpper() == FlowType.RESHOP.ToString())
                {
                    headers.Add("ChannelID", _configuration.GetValue<string>("MerchandizeOffersCSLServiceChannelID"));
                    headers.Add("ChannelName", _configuration.GetValue<string>("MerchandizeOffersCSLServiceChannelName"));
                }
                else if (flow?.ToUpper() == FlowType.CHECKIN.ToString() || flow?.ToUpper() == FlowType.CHECKINSDC.ToString())
                {
                    headers.Add("ChannelID", _configuration.GetValue<string>("MerchandizeOffersMCEServiceChannelID"));
                    headers.Add("ChannelName", _configuration.GetValue<string>("MerchandizeOffersMCEServiceChannelName"));
                }
                else
                {
                    headers.Add("ChannelID", _configuration.GetValue<string>("MerchandizeOffersServiceChannelID"));
                    headers.Add("ChannelName", _configuration.GetValue<string>("MerchandizeOffersServiceChannelName"));
                }
            }
            return headers;
        }

        public bool IsBundleFlow(string flow)
        {
            bool isBundleFlow = false;
            if (_configuration.GetValue<bool>("EnableTravelOptionsInViewRes"))
            {
                if (flow?.ToUpper() == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString())
                {
                    isBundleFlow = true;
                }
            }
            return isBundleFlow;
        }

    }
}
