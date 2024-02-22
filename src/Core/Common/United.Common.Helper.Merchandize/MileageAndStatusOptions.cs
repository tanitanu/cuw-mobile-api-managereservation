using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MemberProfile;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ProductResponseModel;
using United.Services.Customer.Profile.Common;
using United.Utility.Helper;
using MOBOfferTile = United.Mobile.Model.Common.MOBOfferTile;

namespace United.Common.Helper.Merchandize
{
    public class MileageAndStatusOptions
    {
        private readonly ProductOffer _offerResponse;
        private readonly string _sessionId;
        private readonly MOBRequest _mobRequest;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IConfiguration _configuration;
        public MOBOfferTile OfferTile;
        private MOBAccelerators accelerator;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IHeaders _headers;
        private readonly ILoyaltyMemberProfileService _loyaltyMemberProfileService;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly IDynamoDBService _dynamoDBService;

        public MileageAndStatusOptions(ProductOffer offerResponse, string sessionId, MOBRequest mobRequest,
            ISessionHelperService sessionHelperService, IConfiguration configuration, IProductInfoHelper productInfoHelper,
            ILegalDocumentsForTitlesService legalDocumentsForTitlesService, IHeaders headers, ILoyaltyMemberProfileService loyaltyMemberProfileService,
            ICustomerProfileService customerProfileService, IDynamoDBService dynamoDBService)
        {
            _offerResponse = offerResponse;
            _sessionId = sessionId;
            _mobRequest = mobRequest;
            _sessionHelperService = sessionHelperService;
            _configuration = configuration;
            _productInfoHelper = productInfoHelper;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
            _loyaltyMemberProfileService = loyaltyMemberProfileService;
            _customerProfileService = customerProfileService;
            _dynamoDBService = dynamoDBService;
        }
        public MileageAndStatusOptions(ProductOffer offerResponse
            , string sessionId
            , IProductInfoHelper productInfoHelper)
        {
            _offerResponse = offerResponse;
            _sessionId = sessionId;
            _productInfoHelper = productInfoHelper;
        }
        public async Task<MileageAndStatusOptions> AddMiles(string token)
        {
            if (accelerator == null || accelerator.EligibleTravelers == null || !accelerator.EligibleTravelers.Any())
                return this;

            foreach (var item in accelerator.EligibleTravelers)
            {
                string currentMiles = null;
                string premierMiles = null;
                (currentMiles, premierMiles) = await GetMileagePlusDetails(token, item.MileagePlusNumber, currentMiles, premierMiles);
                item.MileagePlusNumber = null;
                item.CurrentMiles = currentMiles;
                item.PremierMiles = IsPremierMilesOffered(item.Offers) ? premierMiles : string.Empty;
            }

            //Parallel.ForEach(accelerator.EligibleTravelers, async t =>
            //{
            //    string currentMiles = null;
            //    string premierMiles = null;
            //    (currentMiles, premierMiles) = await GetMileagePlusDetails(session.Token, t.MileagePlusNumber, currentMiles, premierMiles);
            //    t.MileagePlusNumber = null;
            //    t.CurrentMiles = currentMiles;
            //    t.PremierMiles = IsPremierMilesOffered(t.Offers) ? premierMiles : string.Empty;
            //});
            return this;
        }
        public MileageAndStatusOptions AddTravelers()
        {
            var travelers = new List<MOBAcceleratorTraveler>();
            foreach (var t in _offerResponse.Travelers)
            {
                var ac = new MOBAcceleratorTraveler
                {
                    Id = t.ID,
                    Name = FirstLetterToUpperCase(t.GivenName) + " " + FirstLetterToUpperCase(t.Surname),
                    MileagePlusNumber = t.LoyaltyProgramProfile != null ? t.LoyaltyProgramProfile.LoyaltyProgramMemberID : string.Empty,
                    Offers = BuildOfferForTraveler(t.ID, _offerResponse.Offers.FirstOrDefault().ProductInformation.ProductDetails)
                };
                if (ac.Offers != null && ac.Offers.Any())
                {
                    travelers.Add(ac);
                }
            }
            if (travelers == null || !travelers.Any())
                return this;

            accelerator = new MOBAccelerators();
            accelerator.EligibleTravelers = travelers;
            return this;
        }
        public MileageAndStatusOptions RemoveMiles()
        {
            if (accelerator == null || !accelerator.EligibleTravelers.Any(t => string.IsNullOrEmpty(t.CurrentMiles)))
                return this;

            accelerator.EligibleTravelers.ForEach(t => ClearMiles(ref t));
            return this;
        }
        public MOBAccelerators GetAccelerators()
        {
            return accelerator;
        }
        public async Task<MileageAndStatusOptions> AddCaptions()
        {
            if (accelerator == null)
                return this;

            accelerator.Captions = _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ? await _productInfoHelper.GetCaptions("PPR_AAPA_CAPTIONS_PRODUCTPAGE") : await _productInfoHelper.GetCaptions("AAPA_CAPTIONS_PRODUCTPAGE");
            if (accelerator.EligibleTravelers.Any(t => !string.IsNullOrEmpty(t.CurrentMiles)) && accelerator.Captions != null && accelerator.Captions.Any())
            {
                accelerator.Captions.RemoveAll(c => !string.IsNullOrEmpty(c.Id) &&
                                                    (c.Id.Equals("ProductPageMessageForLoyalityInfoMissing", StringComparison.OrdinalIgnoreCase) ||
                                                     c.Id.Equals("PickTravelersMessageForLoyalityInfoMissing", StringComparison.OrdinalIgnoreCase)));
            }
            return this;
        }
        public async Task<MileageAndStatusOptions> AddTermsAndCondtions()
        {
            if (accelerator == null)
                return this;

            var hasPremierAcceleratorOffer = accelerator.EligibleTravelers.Any(t => IsPremierMilesOffered(t.Offers));
            accelerator.AwardAcceleratorTnCs = _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ? await GetAcceleratorMessages("PPR_AAPA_AWARD_ACCELERATOR_MESSAGES") : await GetAcceleratorMessages("AAPA_AWARD_ACCELERATOR_MESSAGES");
            accelerator.PremierAcceleratorTnCs = hasPremierAcceleratorOffer ? _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ? await GetAcceleratorMessages("PPR_AAPA_PREMIER_ACCELERATOR_MESSAGES") : await GetAcceleratorMessages("AAPA_PREMIER_ACCELERATOR_MESSAGES") : null;
            accelerator.TermsAndConditions = await GetTermsAndConditions(hasPremierAcceleratorOffer);

            return this;
        }
        private async Task<List<MOBMobileCMSContentMessages>> GetTermsAndConditions(bool hasPremierAccelerator)
        {

            var dbKey = _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ? hasPremierAccelerator ? "PPR_AAPA_TERMS_AND_CONDITIONS_AA_PA_MP"
                                              : "PPR_AAPA_TERMS_AND_CONDITIONS_AA_MP" : hasPremierAccelerator ? "AAPA_TERMS_AND_CONDITIONS_AA_PA_MP"
                                              : "AAPA_TERMS_AND_CONDITIONS_AA_MP";

            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(dbKey, _headers.ContextValues.SessionId, true).ConfigureAwait(false);

            if (docs == null || !docs.Any())
                return null;

            var tncs = new List<MOBMobileCMSContentMessages>();
            foreach (var doc in docs)
            {
                var tnc = new MOBMobileCMSContentMessages
                {
                    Title = "Terms and conditions",
                    ContentFull = doc.LegalDocument,
                    ContentShort = _configuration.GetValue<string>("PaymentTnCMessage").ToString(),
                    HeadLine = doc.Title
                };
                tncs.Add(tnc);
            }

            return tncs;
        }
        private async Task<List<MOBMobileCMSContentMessages>> GetAcceleratorMessages(string key)
        {
            var docs = await _productInfoHelper.GetCaptions(key);
            if (docs == null || !docs.Any())
                return null;

            var cmsContentMessage = new MOBMobileCMSContentMessages();
            foreach (var doc in docs)
            {
                switch (doc.Id)
                {
                    case "PageTitle":
                        cmsContentMessage.Title = doc.CurrentValue;
                        break;
                    case "ShortDescription":
                        cmsContentMessage.ContentShort = doc.CurrentValue;
                        break;
                    case "DetailTitle":
                        cmsContentMessage.HeadLine = doc.CurrentValue;
                        break;
                    case "DetailDescription":
                        cmsContentMessage.ContentFull = doc.CurrentValue;
                        break;
                }
            }
            return new List<MOBMobileCMSContentMessages>() { cmsContentMessage };
        }
        private void ClearMiles(ref MOBAcceleratorTraveler t)
        {
            t.MileagePlusNumber = null;
            t.CurrentMiles = null;
            t.PremierMiles = null;
        }
        private async Task<(string, string)> GetMileagePlusDetails(string token, string mileagPlusNumber, string currentMiles, string premierMiles)
        {
            if (_configuration.GetValue<bool>("AccountStatusAwsUCB"))
            {
                (currentMiles, premierMiles) = await GetMileagePlusDetailsV2(token, mileagPlusNumber, currentMiles, premierMiles);
                return (currentMiles, premierMiles);
            }

            try
            {
                var jsonResponse = await _customerProfileService.GetAccountStatus<string>(mileagPlusNumber, token, _headers.ContextValues.SessionId);

                var response = JsonConvert.DeserializeObject<AccountStatusCompositeDataModel>(jsonResponse);

                currentMiles = string.Format("Current award miles: <b>{0:n0}</b>", response.MileagePlusService.MileagePlus.AccountBalance);
                premierMiles =
                    _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ?
                    string.Format("PQF: <b>{0:n0}</b> | PQP: <b>{1}</b>", response.MileagePlusService.MileagePlus.PremierQualifyingFlights, response.MileagePlusService.MileagePlus.PremierQualifyingPoints) :
                    string.Format("PQM: <b>{0:n0}</b> | PQS: <b>{1}</b> | PQD: <b>{2}</b>", response.MileagePlusService.MileagePlus.EliteMileageBalance, response.MileagePlusService.MileagePlus.EliteSegmentBalance, TopHelper.FormatAmountForDisplay(response.MileagePlusService.MileagePlus.CurrentYearMoneySpent.ToString(), TopHelper.GetCultureInfo("USD")));
            }
            catch (Exception ex)
            {
                currentMiles = null;
                premierMiles = null;
            }
            return (currentMiles, premierMiles);

        }
        private async Task<(string, string)> GetMileagePlusDetailsV2(string token, string mileagPlusNumber, string currentMiles, string premierMiles)
        {
            try
            {
                var response = await _loyaltyMemberProfileService.GetAccountMemberProfile<CslResponse<MemberProfileResponse>>(token, mileagPlusNumber, _headers.ContextValues.SessionId);

                if (response.Errors != null && response.Errors.Any())
                {
                    currentMiles = null;
                    premierMiles = null;
                }
                else
                {
                    var accountBalance = response.Data?.Balances?.Where(x => x.Currency == "RDM").FirstOrDefault()?.Amount;
                    var premierQualifyingFlights = response.Data?.PremierQualifyingMetrics?.Where(x => x.ProgramCurrency == "PQF").FirstOrDefault()?.Balance;
                    var premierQualifyingPoints = response.Data?.PremierQualifyingMetrics?.Where(x => x.ProgramCurrency == "PQP").FirstOrDefault()?.Balance;
                    var currentYearMoneySpent = response.Data?.CurrentYearMoneySpent;

                    currentMiles = string.Format("Current award miles: <b>{0:n0}</b>", accountBalance ?? 0);
                    premierMiles =
                        _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ?
                        string.Format("PQF: <b>{0:n0}</b> | PQP: <b>{1}</b>", premierQualifyingFlights, premierQualifyingPoints) :
                        string.Format("PQM: <b>{0:n0}</b> | PQS: <b>{1}</b> | PQD: <b>{2}</b>", premierQualifyingPoints, premierQualifyingFlights,
                        TopHelper.FormatAmountForDisplay(currentYearMoneySpent.ToString(), TopHelper.GetCultureInfo("USD")));
                }


            }
            catch (Exception ex)
            {
                currentMiles = null;
                premierMiles = null;
            }
            return (currentMiles, premierMiles);
        }
        private bool IsPremierMilesOffered(List<MOBAcceleratorOffer> offers)
        {
            return offers != null &&
                   offers.Any(o => o != null &&
                              o.AwardMilesOffer != null &&
                              o.PremierAcceleratorOffer != null &&
                              !string.IsNullOrEmpty(o.PremierAcceleratorOffer.PremierMilesOffer));
        }
        private List<MOBAcceleratorOffer> BuildOfferForTraveler(string travelerRefId, Collection<ProductDetail> productDetails)
        {
            var AACprices = GetAwardAcceleratorPrices(travelerRefId, productDetails);
            var PACprices = GetPremierAcceleratortPrices(travelerRefId, productDetails);
            var offers = new List<MOBAcceleratorOffer>();
            foreach (var aa in AACprices)
            {
                var offer = new MOBAcceleratorOffer();
                offer.ProductCode = "AAC";
                offer.ProductId = aa.ID;
                offer.Price = GetOfferAmount(aa);
                offer.FormattedPrice = TopHelper.FormatAmountForDisplay(offer.Price.ToString(), TopHelper.GetCultureInfo(GetCurrencyCode(aa)));
                offer.AwardMilesOffer = string.Format("{0} award miles", GetOfferedMiles(aa));
                offer.SelectedAwardMilesText = string.Format("{0} miles | {1}", GetOfferedMiles(aa), offer.FormattedPrice);
                offer.SubProductCode = GetSubProductCode(productDetails, "AAC", aa.ID);
                offer.PremierAcceleratorOffer = BuildPremierAcceleratorOffer(productDetails, PACprices, aa);
                offers.Add(offer);
            }
            return offers;
        }
        private IEnumerable<ProductPriceOption> GetPremierAcceleratortPrices(string travelerRefId, Collection<ProductDetail> productDetails)
        {
            return productDetails.FirstOrDefault(p => p.Product.Code == "PAC").Product.SubProducts.Where(sp => sp.Prices != null).SelectMany(sp => sp.Prices.Where(p => p.Association?.TravelerRefIDs != null && p.Association.TravelerRefIDs.Contains(travelerRefId)));
        }
        private IEnumerable<ProductPriceOption> GetAwardAcceleratorPrices(string travelerRefId, Collection<ProductDetail> productDetails)
        {
            return productDetails.FirstOrDefault(p => p.Product.Code == "AAC").Product.SubProducts.Where(sp => sp.Prices != null).SelectMany(sp => sp.Prices.Where(p => p.Association?.TravelerRefIDs != null && p.Association.TravelerRefIDs.Contains(travelerRefId)));
        }
        private MOBPremierAcceleratorOffer BuildPremierAcceleratorOffer(Collection<ProductDetail> productDetails, IEnumerable<ProductPriceOption> PACprices, ProductPriceOption aa)
        {
            if (PACprices != null && PACprices.Any())
            {
                var id = GetRelatedPremierAcceleratorId(aa, PACprices).ID;
                return new MOBPremierAcceleratorOffer()
                {
                    Price = GetOfferAmount(GetRelatedPremierAcceleratorId(aa, PACprices)),
                    ProductCode = "PAC",
                    ProductId = id,
                    SubProductCode = GetSubProductCode(productDetails, "PAC", id),
                    PremierMilesOffer = _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ?
                    string.Format("Add Premier Accelerator℠ {0} for {1}", GetPremierAcceleratorPoints(GetRelatedPremierAcceleratorId(aa, PACprices)), TopHelper.FormatAmountForDisplay(GetOfferAmount(GetRelatedPremierAcceleratorId(aa, PACprices)).ToString(), TopHelper.GetCultureInfo(GetCurrencyCode(aa)))) :
                    string.Format("Add Premier Accelerator℠ for {0} more", TopHelper.FormatAmountForDisplay(GetOfferAmount(GetRelatedPremierAcceleratorId(aa, PACprices)).ToString(), TopHelper.GetCultureInfo(GetCurrencyCode(aa))))
                };
            }
            return null;
        }
        private string GetPremierAcceleratorPoints(ProductPriceOption p)
        {
            if (p == null && p.PaymentOptions == null && !p.PaymentOptions.Any())
            {
                return string.Empty;
            }
            decimal premierMiles = 0;
            string premierAccelertor = string.Empty;
            United.Service.Presentation.CommonModel.Characteristic characteristic = p.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Characteristics.FirstOrDefault();
            decimal.TryParse(characteristic.Value, out premierMiles);
            premierAccelertor = string.Format("{0:n0}", premierMiles) + " " + characteristic.Description;
            return premierAccelertor;
        }
        private ProductPriceOption GetRelatedPremierAcceleratorId(ProductPriceOption p, IEnumerable<ProductPriceOption> PACprice)
        {
            return PACprice.FirstOrDefault(pac => pac.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Characteristics.FirstOrDefault().Code == p.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Characteristics.FirstOrDefault().Code);
        }
        private string GetSubProductCode(Collection<ProductDetail> productDetails, string productCode, string productId)
        {
            return productDetails.FirstOrDefault(p => p.Product.Code == productCode).Product.SubProducts.FirstOrDefault(sp => sp.Prices != null && sp.Prices.Any(p => p.ID.Equals(productId))).SubGroupCode;
        }
        private string GetOfferedMiles(ProductPriceOption p)
        {
            var miles = Convert.ToDecimal(p.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Characteristics.FirstOrDefault().Value);
            return string.Format("{0:n0}", miles);
        }
        private double GetOfferAmount(ProductPriceOption p)
        {
            return p.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals.FirstOrDefault().Amount;
        }
        private string GetCurrencyCode(ProductPriceOption p)
        {
            return p.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals.FirstOrDefault().Currency.Code;
        }
        private string FirstLetterToUpperCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length == 1)
                return value[0].ToString().ToUpper();

            return value[0].ToString().ToUpper() + value.Substring(1).ToLower();
        }
        public async Task<MileageAndStatusOptions> BuildOfferTile()
        {
            if (_offerResponse == null || _offerResponse.Offers == null || !_offerResponse.Offers.Any())
                return this;

            var productDetail = _offerResponse.Offers.FirstOrDefault().ProductInformation.ProductDetails.FirstOrDefault(p => p != null && p.Product != null && p.Product.Code == "AAC");
            if (productDetail != null)
            {
                var subproductWithPrices = productDetail.Product.SubProducts.Where(sp => sp.Prices != null &&
                                                                                         sp.Prices.Any() &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions != null &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.Any() &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents != null &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents.Any() &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price != null &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals != null &&
                                                                                         sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals.FirstOrDefault() != null);
                if (subproductWithPrices != null && subproductWithPrices.Any())
                {
                    var price = subproductWithPrices.Min(sp => sp.Prices.FirstOrDefault().PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals.FirstOrDefault().Amount);
                    OfferTile = await BuildOfferTile(price, "AA_OfferTile");
                }
            }
            return this;
        }

        private async Task<MOBOfferTile> BuildOfferTile(double offerPrice, string captionKey, bool showUplift = false)
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



    }

}
