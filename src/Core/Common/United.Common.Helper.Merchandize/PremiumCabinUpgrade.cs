using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Shopping.Pcu;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Cart;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Helper;
using Reservation = United.Service.Presentation.ReservationModel.Reservation;

namespace United.Common.Helper.Merchandize
{
    public class PremiumCabinUpgrade
    {
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IConfiguration _configuration;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IDPService _dPService;
        private readonly IPKDispenserService _pKDispenserService;
        private readonly ICachingService _cachingService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;

        #region Private Members
        private const string CountryCode = "US";
        private const string LangCode = "en-US";
        private const string ProductCodePcu = "PCU";
        private const string CartKeyPcuBookingPath = "ConfirmationPCU";
        private const string ModifyReservationFailed = "90506";
        private const string CreditCardFailure = "90546";
        private const string GenericSeatAssignmentFailed = "90584";
        private const string MerchandiseServiceReturnedError = "90585";
        private string _sessionId;
        private readonly string _flightNumber;
        private readonly string _origin;
        private readonly string _destination;
        private readonly string _emailAddress;
        private readonly MOBCreditCard _creditCard;
        private readonly List<string> _selectedSegmentIds;
        private readonly List<LogEntry> _logEntries;
        private readonly MOBAddress _address;
        private readonly MOBCPPhone _phone;
        private MOBRequest _mobRequest;
        private Reservation _cslReservation;
        public Service.Presentation.ProductResponseModel.ProductOffer MerchProductOffer;
        public string CartId;
        private string _token;
        public bool IsValidOffer;
        public string RecordLocator;
        private double _totalPrice;
        private string _lastNameOfAnyTraveler;
        private int _numberOfPax;
        private List<string> _selectedProductIds;
        private bool _isPostBookingPurchase;
        private List<MOBPASegment> _pcuSegments;
        private List<string> _travelerNames;

        private readonly MOBPayPal _payPal;
        private readonly MOBMasterpass _masterPass;
        private readonly MOBApplePay _applePay;

        #endregion
        public DisplayCartRequest ProductRequest;
        public MOBPremiumCabinUpgrade CabinUpgradeOffer { get; set; }
        private readonly IHeaders _headers;
        public PremiumCabinUpgrade(
            ISessionHelperService sessionHelperService,
            IConfiguration configuration,
            IProductInfoHelper productInfoHelper,
            IDynamoDBService dynamoDBService,
            IDPService dPService,
            IPKDispenserService pKDispenserService,
             ICachingService cachingService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IHeaders headers)
        {

            _sessionHelperService = sessionHelperService;
            _configuration = configuration;
            _productInfoHelper = productInfoHelper;
            _dynamoDBService = dynamoDBService;
            _dPService = dPService;
            _pKDispenserService = pKDispenserService;
            _cachingService = cachingService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
        }
        public async System.Threading.Tasks.Task Initialization(string recordLocator, string sessionId, string cartId, MOBRequest mobRequest, Reservation cslReservation)
        {
            await ClearPcuState(sessionId);
            this.RecordLocator = recordLocator;
            this._sessionId = sessionId;
            this._mobRequest = mobRequest;
            this.CartId = cartId;
            this._isPostBookingPurchase = !string.IsNullOrEmpty(cartId);
            this._cslReservation = _configuration.GetValue<bool>("RevertPassingReservationToGetProductsPCU") ? null : cslReservation;
        }
        private async System.Threading.Tasks.Task ClearPcuState(string sessionId)
        {
            var pcuState = new PcuState();
            //FilePersist.Save(sessionId, pcuState.ObjectName, pcuState);
            await _sessionHelperService.SaveSession(pcuState, sessionId, new List<string> { sessionId, pcuState.ObjectName }, pcuState.ObjectName).ConfigureAwait(false);
        }
        public PremiumCabinUpgrade ValidateOfferResponse()
        {
            IsValidOffer = !string.IsNullOrEmpty(CartId) &&
                            MerchProductOffer != null &&
                            MerchProductOffer.Offers != null &&
                            MerchProductOffer.Offers.Any() &&
                            MerchProductOffer.Offers[0].ProductInformation != null &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails != null &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails.Any() &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product != null &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts != null &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Any() &&
                            MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Any(SubProductWithPrice()) &&
                            MerchProductOffer.Travelers != null &&
                            MerchProductOffer.Travelers.Any() &&
                            MerchProductOffer.Response != null &&
                            MerchProductOffer.Response.RecordLocator != null &&
                            MerchProductOffer.Response.RecordLocator.Equals(RecordLocator, StringComparison.InvariantCultureIgnoreCase) &&
                            (MerchProductOffer.Solutions != null &&
                            MerchProductOffer.Solutions.Any() &&
                            MerchProductOffer.Solutions[0].ODOptions != null &&
                            MerchProductOffer.Solutions[0].ODOptions.Any() ||
                            MerchProductOffer.FlightSegments != null &&
                            MerchProductOffer.FlightSegments.Any());

            return this;
        }
        public async Task<PremiumCabinUpgrade> BuildPremiumCabinUpgrade()
        {
            if (!this.IsValidOffer) return this;

            var premiumCabinUpgrade = new United.Mobile.Model.Shopping.Pcu.MOBPremiumCabinUpgrade();
            premiumCabinUpgrade.CartId = CartId;
            premiumCabinUpgrade.PcuOptions = await BuildPcuOptions();
            premiumCabinUpgrade.ProductCode = MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.Code;
            premiumCabinUpgrade.ProductName = MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.DisplayName;
            if (premiumCabinUpgrade.PcuOptions == null ||
                premiumCabinUpgrade.PcuOptions.EligibleTravelers == null ||
                premiumCabinUpgrade.PcuOptions.EligibleSegments == null ||
                !premiumCabinUpgrade.PcuOptions.EligibleSegments.Any()) return this;

            var amount = premiumCabinUpgrade.PcuOptions.EligibleSegments.Where(s => !string.IsNullOrEmpty(s.FormattedPrice)).Min(s => s.Price);
            if (amount <= 0) return null;

            premiumCabinUpgrade.OfferTile = await BuildOfferTile(amount, "PCU_OfferTile", amount >= MinimumPriceForUplift && (_configuration.GetValue<string>("EligibleProductsForUpliftInViewRes")?.Split(',')?.Contains("PCU") ?? false));
            premiumCabinUpgrade.Captions = await BuildPcuCaptions(premiumCabinUpgrade.PcuOptions.EligibleTravelers.Count > 1);
            premiumCabinUpgrade.MobileCmsContentMessages = await GetTermsAndConditions();
            this.CabinUpgradeOffer = premiumCabinUpgrade;
            return this;
        }
        public async Task<List<MOBMobileCMSContentMessages>> GetTermsAndConditions()
        {
            var cmsContentMessages = new List<MOBMobileCMSContentMessages>();
            var docKeys = "PCU_TnC";
            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(docKeys, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
            if (docs != null && docs.Any())
            {
                foreach (var doc in docs)
                {
                    var cmsContentMessage = new MOBMobileCMSContentMessages();
                    cmsContentMessage.ContentFull = doc.LegalDocument;
                    cmsContentMessage.Title = doc.Title;
                    cmsContentMessages.Add(cmsContentMessage);
                }
            }
            return cmsContentMessages;
        }
        private async Task<List<MOBItem>> BuildPcuCaptions(bool isMultiPax)
        {
            //var captions = new Profile().GetMPPINPWDTitleMessages(new List<string> { "PCU_UpgradeFlight_Captions" });
            var captions = await GetMPPINPWDTitleMessages("PCU_UpgradeFlight_Captions");
            if (_configuration.GetValue<bool>("EnablePcuMultipleUpgradeOptions"))
            {
                if (!isMultiPax && captions != null)
                {
                    captions.RemoveWhere(c => c != null && c.Id == "MultiPaxNoteText");
                }
            }
            return captions;
        }
        public int MinimumPriceForUplift
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
        public async Task<PremiumCabinUpgrade> GetTokenFromSession()
        {
            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(_headers.ContextValues.SessionId, session.ObjectName, new List<string> { _headers.ContextValues.SessionId, session.ObjectName }).ConfigureAwait(false);

            this._token = session.Token;
            return this;
        }
        public async Task<PremiumCabinUpgrade> GetpkDispenserPublicKey(string flow)
        {
            if (!this.IsValidOffer) return this;
            var pkDispense = new PKDispenserPublicKey(_configuration, _cachingService, _dPService, _pKDispenserService, _headers);
            this.CabinUpgradeOffer.PkDispenserPublicKey = await pkDispense.GetCachedOrNewpkDispenserPublicKey(this._mobRequest.Application.Id, this._mobRequest.Application.Version.Major, this._mobRequest.DeviceId, _sessionId, _token, null, flow);

            return this;
        }
        public async System.Threading.Tasks.Task SaveState()
        {
            if (!IsValidOffer) return;

            var pcuState = new PcuState();
            pcuState.CartId = CartId;
            pcuState.EligibleSegments = CabinUpgradeOffer.PcuOptions.EligibleSegments;
            pcuState.RecordLocator = RecordLocator;
            pcuState.LastName = MerchProductOffer.Travelers[0].Surname;
            pcuState.NumberOfPax = MerchProductOffer.Travelers.Count;
            pcuState.IsPostBookingPurchase = _isPostBookingPurchase;
            pcuState.PremiumCabinUpgradeOfferDetail = CabinUpgradeOffer;
            await _sessionHelperService.SaveSession(pcuState, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, pcuState.ObjectName }, pcuState.ObjectName).ConfigureAwait(false);
        }
        private static Func<SubProduct, bool> SubProductWithPrice()
        {
            return sp => sp != null && sp.Prices != null &&
                   sp.Prices.Any(p => p != null &&
                                 p.PaymentOptions != null &&
                                 p.PaymentOptions.Any(po => po != null &&
                                                      po.PriceComponents != null &&
                                                      po.PriceComponents.Any(pc => pc != null && pc.Price != null && pc.Price.Totals != null && pc.Price.Totals.Any() && pc.Price.Totals[0].Amount > 0)));
        }
        private async Task<PcuOptions> BuildPcuOptions()
        {

            var subProducts = MerchProductOffer.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts;
            var flightSegments = MerchProductOffer.FlightSegments ?? MerchProductOffer.Solutions[0].ODOptions.SelectMany(o => o.FlightSegments).ToCollection();

            PcuOptions pcuOptions = new PcuOptions();
            pcuOptions.EligibleTravelers = MerchProductOffer.Travelers.Select(t => string.Format("{0}, {1}", t.Surname, t.GivenName)).ToList();
            pcuOptions.EligibleSegments = new List<PcuSegment>();
            pcuOptions.CurrencyCode = "$";
            var noOfTravelers = MerchProductOffer.Travelers.Count;
            var pcuSegments = new List<PcuSegment>();
            foreach (var flightSegment in flightSegments)
            {
                if (!ValidFlighSegmentData(flightSegment)) continue;
                var offers = subProducts.Where(sp => IsEligibleSegment(flightSegment.ID, sp));

                var segment = new PcuSegment
                {
                    FlightDescription = string.Format("{0}{1} - {2} to {3}", flightSegment.MarketedFlightSegment[0].MarketingAirlineCode, flightSegment.MarketedFlightSegment[0].FlightNumber, flightSegment.DepartureAirport.IATACode, flightSegment.ArrivalAirport.IATACode),
                    SegmentNumber = flightSegment.SegmentNumber,
                    Origin = flightSegment.DepartureAirport.IATACode.ToUpper(),
                    Destination = flightSegment.ArrivalAirport.IATACode.ToUpper(),
                    FlightNumber = flightSegment.MarketedFlightSegment[0].FlightNumber,
                    SegmentDescription = string.Format("{0} - {1}", flightSegment.DepartureAirport.IATACode.ToUpper(), flightSegment.ArrivalAirport.IATACode.ToUpper()),
                    NoOfTravelersText = noOfTravelers + (noOfTravelers == 1 ? " Traveler /" : " Travelers /"),
                    UpgradeOptions = BuildUpgradeOptions(noOfTravelers, flightSegment, offers)
                };
                PopulateMoreFieldsToSupportOlderVerionsOfApps(ref segment);
                pcuSegments.Add(segment);
            }
            if (_configuration.GetValue<bool>("EnablePcuMultipleUpgradeOptions"))
            {
                pcuOptions.CompareOptions =await BuildCompareOptions(pcuSegments);
            }
            if (pcuSegments.Any(s => s.Price > 0))
            {
                if (EnablePCU_NotAvailableVersion(_mobRequest.Application.Id, _mobRequest.Application.Version.Major))
                {
                    pcuOptions.EligibleSegments.AddRange(pcuSegments);
                }
                else
                {
                    pcuSegments.RemoveAll(s => s.Price <= 0);
                    pcuOptions.EligibleSegments.AddRange(pcuSegments);
                }
            }

            return pcuOptions;
        }
        private async Task<List<PcuUpgradeOptionInfo>> BuildCompareOptions(List<PcuSegment> pcuSegments)
        {
            if (pcuSegments == null || !pcuSegments.Any() || !pcuSegments.Any(s => s.UpgradeOptions != null && s.UpgradeOptions.Any()))
                return null;

            var upgradeCabinsOffered = pcuSegments.SelectMany(s => GetOfferedCabinDescriptions(s.UpgradeOptions)).Distinct().ToList();

            var compareOptionsCaptions =await _productInfoHelper.GetCaptions("PCU_COMPAREOPTIONS");
            if (compareOptionsCaptions == null || !compareOptionsCaptions.Any())
                return null;

            compareOptionsCaptions.RemoveWhere(c => !upgradeCabinsOffered.Contains(c.Id, StringComparer.OrdinalIgnoreCase));
            var compareOptions = compareOptionsCaptions.Select(c => AddUpgradeOptionInfo(c.CurrentValue)).ToList();
            return compareOptions;
        }
        private PcuUpgradeOptionInfo AddUpgradeOptionInfo(string dbContent)
        {
            if (string.IsNullOrEmpty(dbContent))
                return null;

            var details = dbContent.Split('|');
            if (details == null || details.Length < 4)
                return null;

            return new PcuUpgradeOptionInfo()
            {
                Product = details[0],
                Header = details[1],
                Body = details[2],
                ImageUrl = details[3]
            };
        }
        private IEnumerable<string> GetOfferedCabinDescriptions(List<PcuUpgradeOption> upgradeOptions)
        {
            if (upgradeOptions == null || !upgradeOptions.Any())
                return new List<string>();

            return upgradeOptions.Select(u => u != null && u.UpgradeOptionDescription != null ? u.UpgradeOptionDescription.Trim().Replace("®", "").Replace("℠", "").Replace(" ", "") : string.Empty);
        }
        private static void PopulateMoreFieldsToSupportOlderVerionsOfApps(ref PcuSegment segment)
        {
            //to support older clients
            if (segment.UpgradeOptions != null && segment.UpgradeOptions.Any())
            {
                var lowestPricedUpgradeOption = segment.UpgradeOptions.Aggregate((currentItem, u) => u.Price != 0 && u.Price < currentItem.Price ? u : currentItem);
                segment.CabinDescription = lowestPricedUpgradeOption.CabinDescriptionForPaymentPage;
                segment.UpgradeDescription = lowestPricedUpgradeOption.UpgradeOptionDescription;
                segment.Price = lowestPricedUpgradeOption.Price;
                segment.FormattedPrice = string.Format("${0}", lowestPricedUpgradeOption.Price);
                segment.TotalPriceForAllTravelers = lowestPricedUpgradeOption.TotalPriceForAllTravelers;
                segment.ProductIds = lowestPricedUpgradeOption.ProductIds;
            }
        }
        private List<PcuUpgradeOption> BuildUpgradeOptions(int noOfTravelers, ProductFlightSegment flightSegment, IEnumerable<SubProduct> offers)
        {
            if (noOfTravelers <= 0 || flightSegment == null || offers == null || !offers.Any())
                return null;

            var options = offers.Where(sp => IsEligibleSegment(flightSegment.ID, sp))
                                .Select(o => BuildUpgradeOption(noOfTravelers, flightSegment.ID, o)).ToList();
            if (!options.Any())
                return null;

            options.RemoveAll(o => o.Price <= 0);
            if (!options.Any())
                return null;

            options = options.OrderBy(o => o.Price).ToList();
            options.Add(new PcuUpgradeOption
            {
                UpgradeOptionDescription = "I don't want to upgrade this flight"
            });

            return options;
        }
        private static double GetAmount(ProductPriceOption priceOption)
        {
            if (priceOption == null ||
                priceOption.PaymentOptions == null ||
                !priceOption.PaymentOptions.Any() ||
                priceOption.PaymentOptions[0] == null ||
                priceOption.PaymentOptions[0].PriceComponents == null ||
                !priceOption.PaymentOptions[0].PriceComponents.Any() ||
                priceOption.PaymentOptions[0].PriceComponents[0].Price == null ||
                priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals == null ||
                !priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals.Any())
                return 0;

            return priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount;
        }
        private PcuUpgradeOption BuildUpgradeOption(int noOfTravelers, string segmentId, SubProduct subProduct)
        {
            var amount = GetAmount(subProduct.Prices[0]);
            var cabinName = GetFormattedCabinName(subProduct.Descriptions[0]);
            return new PcuUpgradeOption()
            {
                OptionId = segmentId + subProduct.Code,
                UpgradeOptionDescription = cabinName,
                CabinDescriptionForPaymentPage = string.Format("{0}:", cabinName),
                FormattedPrice = TopHelper.FormatAmountForDisplay(amount.ToString(), new CultureInfo("en-US")),
                Price = amount,
                TotalPriceForAllTravelers = noOfTravelers * amount,
                ProductIds = subProduct.Prices.Select(p => p.ID).ToList()
            };
        }
        public string GetFormattedCabinName(string cabinName)
        {
            if (!_configuration.GetValue<bool>("EnablePcuMultipleUpgradeOptions"))
            {
                return cabinName;
            }

            if (string.IsNullOrWhiteSpace(cabinName))
                return string.Empty;

            switch (cabinName.ToUpper().Trim())
            {
                case "UNITED FIRST":
                    return "United First®";
                case "UNITED BUSINESS":
                    return "United Business®";
                case "UNITED POLARIS FIRST":
                    return "United Polaris℠ first";
                case "UNITED POLARIS BUSINESS":
                    return "United Polaris℠ business";
                case "UNITED PREMIUM PLUS":
                    return "United® Premium Plus";
                default:
                    return string.Empty;
            }
        }
        private static bool ValidFlighSegmentData(ProductFlightSegment flightSegment)
        {
            return !(flightSegment == null ||
                     flightSegment.DepartureAirport == null || string.IsNullOrEmpty(flightSegment.DepartureAirport.IATACode) ||
                     flightSegment.ArrivalAirport == null || string.IsNullOrEmpty(flightSegment.ArrivalAirport.IATACode) ||
                     flightSegment.MarketedFlightSegment == null || !flightSegment.MarketedFlightSegment.Any() ||
                     string.IsNullOrEmpty(flightSegment.MarketedFlightSegment[0].MarketingAirlineCode) || string.IsNullOrEmpty(flightSegment.MarketedFlightSegment[0].FlightNumber));
        }
        private static bool IsEligibleSegment(string segId, SubProduct subProduct)
        {
            if (string.IsNullOrEmpty(segId) ||
                subProduct == null ||
                subProduct.Extension == null ||
                subProduct.Extension.AdditionalExtensions == null ||
                !subProduct.Extension.AdditionalExtensions.Any() ||
                subProduct.Extension.AdditionalExtensions[0].Assocatiation == null ||
                subProduct.Extension.AdditionalExtensions[0].Assocatiation.SegmentRefIDs == null ||
                !subProduct.Extension.AdditionalExtensions[0].Assocatiation.SegmentRefIDs.Any() ||
                subProduct.Prices == null || !subProduct.Prices.Any())
                return false;

            return segId == subProduct.Extension.AdditionalExtensions[0].Assocatiation.SegmentRefIDs[0];
        }

        public PremiumCabinUpgrade BuildOfferRequest()
        {
            this.ProductRequest = new DisplayCartRequest
            {
                CartId = this.CartId,
                CartKey = this._isPostBookingPurchase ? CartKeyPcuBookingPath : "",
                CountryCode = CountryCode,
                LangCode = LangCode,
                Characteristics = CharacteristicsWithDeviceType(_mobRequest.Application.Id),
                ProductCodes = new List<string> { ProductCodePcu },
                Reservation = GetCslReservation()
            };

            return this;
        }

        private Reservation GetCslReservation()
        {
            return _cslReservation ?? new Reservation { ConfirmationID = this.RecordLocator };
        }

        private static Collection<United.Service.Presentation.CommonModel.Characteristic> CharacteristicsWithDeviceType(int id)
        {
            string deviceType;
            switch (id)
            {
                case 1:
                    deviceType = "iOS";
                    break;
                case 2:
                    deviceType = "Android";
                    break;
                default:
                    return null;
            }

            return new Collection<United.Service.Presentation.CommonModel.Characteristic>
            {
                new United.Service.Presentation.CommonModel.Characteristic {Code = "RTD_DeviceType", Value = deviceType},
            };
        }

        #region profile
        public async Task<List<MOBItem>> GetMPPINPWDTitleMessages(string titleList)
        {
            List<MOBItem> messages = new List<MOBItem>();
            List<MOBLegalDocument> docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(titleList, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
            if (docs != null && docs.Count > 0)
            {
                foreach (MOBLegalDocument doc in docs)
                {
                    MOBItem item = new MOBItem();
                    item.Id = doc.Title;
                    item.CurrentValue = doc.LegalDocument;
                    messages.Add(item);
                }
            }
            return messages;
        }
        #endregion
        #region Utilities
        public bool EnablePCU_NotAvailableVersion(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnablePCU")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPCU_NotAvailableVersion", "iPhonePCU_NotAvailableVersion", "", "", true, _configuration);
        }

        internal async Task<MOBOfferTile> BuildOfferTile(double offerPrice, string captionKey, bool showUplift = false)
        {
            if (offerPrice <= 0 || string.IsNullOrWhiteSpace(captionKey))
                return null;

            var offerTileCaptions = await _productInfoHelper.GetCaptions(captionKey);
            var offerTile = new MOBOfferTile();
            offerTile.Price = (decimal)Math.Round(offerPrice);
            offerTile.ShowUpliftPerMonthPrice = showUplift;
            foreach (var caption in offerTileCaptions)
            {
                switch (caption.Id.ToUpper())
                {
                    case "TITLE":
                        offerTile.Title = caption.CurrentValue;
                        break;
                    case "TEXT1":
                        offerTile.Text1 = caption.CurrentValue;
                        break;
                    case "TEXT2":
                        offerTile.Text2 = caption.CurrentValue;
                        break;
                    case "TEXT3":
                        offerTile.Text3 = caption.CurrentValue;
                        break;
                    case "CURRENCYCODE":
                        offerTile.CurrencyCode = caption.CurrentValue;
                        break;
                }
            }

            return offerTile;
        }
        #endregion

        public PremiumCabinUpgrade GetOffer()
        {
            //todo uncomment the below code
            //var productResponse = _flightShopping.GetProducts(ProductRequest, _mobRequest, _sessionId, GetLogActionName());
            ////var productResponse = _getProductsService.GetProducts(ProductRequest, _mobRequest, _sessionId, GetLogActionName());

            //if (productResponse == null)
            //    return this;

            //this.CartId = productResponse.CartId;
            //this.MerchProductOffer = productResponse.MerchProductOffer;
            //return this;
            return null;
        }

        private string GetLogActionName()
        {
            return _isPostBookingPurchase ? "PCUPostBooking" : "PremiumCabinUpgrade";
        }
    }
}
