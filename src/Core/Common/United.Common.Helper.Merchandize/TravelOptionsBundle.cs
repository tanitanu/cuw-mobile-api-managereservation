using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.SegmentModel;

namespace United.Common.Helper.Merchandize
{
   public class TravelOptionsBundle
    {
        private readonly DynamicOfferDetailResponse _offerResponse;
        private readonly Dictionary<string, SDLResult> _sdlBundleContent;
        public MOBTravelOptionsBundle BundleOffer { get; set; }

        public TravelOptionsBundle(DynamicOfferDetailResponse offerResponse)
        {
            _offerResponse = offerResponse;
            _sdlBundleContent = GetBundleContent(offerResponse);
        }

        /// <summary>
        /// This method extracts the SDL content for the bundles offered to extract the SDL content for these codes
        /// </summary>
        /// <param name="offerResponse"></param>
        /// <returns>Dictionary with bundle code as key and respective content as a value</returns>
        private Dictionary<string, SDLResult> GetBundleContent(DynamicOfferDetailResponse offerResponse)
        {
            if (offerResponse?.ResponseData == null)
                return null;

            var codes = offerResponse?.Offers
                                     ?.SelectMany(o => o?.ProductInformation?.ProductDetails
                                                                            ?.Where(p => p.Product?.SubProducts?.Any(sp => sp.GroupCode == "BE") ?? false)
                                                                            ?.Select(p => p.Product.Code))
                                     ?.TakeWhile(code => !string.IsNullOrEmpty(code))
                                     ?.ToList()
                                     ?.Distinct();

            SDLContentResponseData sdlData = offerResponse?.ResponseData?.ToObject<SDLContentResponseData>();
            return sdlData?.Results
                          ?.Where(r => codes.Contains(r.Code))
                          ?.GroupBy(r => r.Code)
                          ?.ToDictionary(r => r.Key, r => r.FirstOrDefault());
        }

        public TravelOptionsBundle BuildTravelOptions()
        {
            if (_offerResponse == null || _offerResponse.Offers == null || !_offerResponse.Offers.Any())
                return this;

            decimal lowestPrice = GetOfferPrice();
            if (lowestPrice <= 0)
                return this;

            BundleOffer = new MOBTravelOptionsBundle
            {
                NumberOfTravelers = _offerResponse?.Travelers?.Count() ?? 0,
                OfferTile = BuildOfferTile(lowestPrice),
                Products = GetProducts(),
                TermsAndCondition = GetTermsAndConditions()
            };

            return this;
        }

        /// <summary>
        /// This method extracts Terms And Conditions for bundles from SDL Content response
        /// </summary>
        /// <returns>MOBMobileCMSContentMessages T&C's for Travel Option bundles</returns>
        private MOBMobileCMSContentMessages GetTermsAndConditions()
        {
            SDLContentResponseData sdlData = _offerResponse.ResponseData.ToObject<SDLContentResponseData>();
            var termsAndConditions = sdlData?.Body
                                            ?.FirstOrDefault(b => b.name.Equals("BUNDLE", StringComparison.OrdinalIgnoreCase))
                                            ?.content
                                            ?.FirstOrDefault(c => c.name.Equals("Bundles-Terms-And-Conditions", StringComparison.OrdinalIgnoreCase))
                                            ?.content
                                            ?.body;

            if (string.IsNullOrEmpty(termsAndConditions))
                return null;


            return new MOBMobileCMSContentMessages
            {
                Title = "Terms and Conditions",
                HeadLine = "Travel Options bundle terms and conditions",
                ContentFull = termsAndConditions
            };
        }

        private List<MOBBundleProduct> GetProducts()
        {

            var subProducts = _offerResponse?.Offers?.FirstOrDefault()
                                                    ?.ProductInformation
                                                    ?.ProductDetails
                                                    ?.SelectMany(p => p?.Product?.SubProducts ?? null)
                                                    ?.TakeWhile(sp => GetAmount(sp?.Prices?.FirstOrDefault()) > 0);

            return subProducts?.GroupBy(sp => sp.Code)?.Select((g, index) => BuildBundleProduct(g?.ToList(), index))?.ToList();
        }

        private MOBBundleProduct BuildBundleProduct(List<SubProduct> subProducts, int index)
        {
            return new MOBBundleProduct
            {
                ProductCode = subProducts?.FirstOrDefault().Code,
                ProductID = subProducts?.FirstOrDefault()?.Prices?.FirstOrDefault()?.ID,
                ProductIDs = GetProductIds(subProducts),
                Tile = BuildBundleTile(subProducts, index),
                Detail = BuildBundleDetails(subProducts, index),
                ProductIndex = index
            };
        }

        private MOBBundleDetail BuildBundleDetails(List<SubProduct> subProducts, int index)
        {
            return new MOBBundleDetail
            {
                OfferTitle = $"{_sdlBundleContent[subProducts?.FirstOrDefault()?.Code].Name} includes the following additions to your trip:",
                OfferDetails = _sdlBundleContent[subProducts?.FirstOrDefault()?.Code].Products.Select(p => new MOBBundleOfferDetail
                {
                    OfferDetailHeader = p.Name,
                    OfferDetailDescription = p.Description,
                    OfferDetailWarningMessage = ""
                })?.ToList(),
                OfferTrips = subProducts?.SelectMany(sp => sp?.Association?.ODMappings?.Select(od => BuildOfferTrips(od.RefID, sp, _offerResponse?.Solutions?.FirstOrDefault()?.ODOptions?.FirstOrDefault(od1 => !string.IsNullOrEmpty(od1?.ID) && od1?.ID == od.RefID)?.FlightSegments)))?.ToList(),
            };
        }

        private MOBBundleOfferTrip BuildOfferTrips(string refID, SubProduct sp, Collection<ProductFlightSegment> flightSegments)
        {
            var price = Convert.ToInt16(GetAmount(sp?.Prices?.FirstOrDefault(p => p?.Association?.ODMappings?.Any(od => !string.IsNullOrEmpty(od?.RefID) && od.RefID == refID) ?? false)));
            return new MOBBundleOfferTrip
            {
                OriginDestination = $"{flightSegments?.FirstOrDefault()?.DepartureAirport?.IATACode} - {flightSegments?.LastOrDefault()?.ArrivalAirport?.IATACode} | ${price}/person",
                TripId = refID,
                TripProductID = sp?.Prices?.Where(p => p?.Association?.ODMappings?.Any(od => !string.IsNullOrEmpty(od?.RefID) && od.RefID == refID) ?? false)?.FirstOrDefault()?.ID,
                TripProductIDs = GetProductIds(sp?.Prices?.Where(p => p?.Association?.ODMappings?.Any(od => !string.IsNullOrEmpty(od?.RefID) && od.RefID == refID) ?? false)?.ToList()),
                Price = price
            };
        }

        private MOBBundleTile BuildBundleTile(List<SubProduct> subProducts, int index)
        {
            return new MOBBundleTile
            {
                OfferTitle = _sdlBundleContent[subProducts?.FirstOrDefault()?.Code].Name,
                OfferDescription = _sdlBundleContent[subProducts?.FirstOrDefault()?.Code].Products.Select(d => d.Name).ToList(),
                PriceText = $"+${GetLowestAmount(subProducts)}/person",
                OfferPrice = $"${GetLowestAmount(subProducts)}"
            };
        }

        private double GetLowestAmount(List<SubProduct> subProducts)
        {
            return subProducts?.Select(sp => GetAmount(sp?.Prices?.FirstOrDefault()))?.Min() ?? 0;
        }

        private List<string> GetProductIds(List<ProductPriceOption> prices) => prices?.Select(p => p?.ID)?.TakeWhile(id => !string.IsNullOrEmpty(id))?.ToList();

        private List<string> GetProductIds(List<SubProduct> subProducts) => subProducts?.SelectMany(sp => GetProductIds(sp?.Prices?.ToList()))?.TakeWhile(id => !string.IsNullOrEmpty(id))?.ToList();

        private double GetAmount(ProductPriceOption price)
        {
            return price?.PaymentOptions?.FirstOrDefault()?.PriceComponents?.FirstOrDefault()?.Price?.Totals?.FirstOrDefault()?.Amount ?? 0;
        }

        private decimal GetOfferPrice()
        {
            if (_offerResponse == null || _offerResponse.Offers == null || !_offerResponse.Offers.Any())
                return 0;

            var subProducts = _offerResponse.Offers?.FirstOrDefault()?.ProductInformation?.ProductDetails?.SelectMany(p => p?.Product?.SubProducts ?? null)?.TakeWhile(sp => sp != null);

            var lowestPrice = subProducts?.Select(sp => sp?.Prices?.FirstOrDefault())
                                         ?.Select(p => p?.PaymentOptions?.FirstOrDefault())
                                         ?.Select(po => po?.PriceComponents?.FirstOrDefault())
                                         ?.Select(pc => pc?.Price?.Totals?.FirstOrDefault()?.Amount)
                                         ?.TakeWhile(a => a > 0)
                                         ?.Min() ?? 0;
            return (decimal)Math.Round(lowestPrice);
        }

        private United.Mobile.Model.Common.MOBOfferTile BuildOfferTile(decimal offerPrice)
        {
            if (_offerResponse == null || _offerResponse.Offers == null || !_offerResponse.Offers.Any())
                return null;


            if (offerPrice > 0)
            {
                return new United.Mobile.Model.Common.MOBOfferTile
                {
                    Price = offerPrice,
                    ShowUpliftPerMonthPrice = false,
                    Title = "Bundles",
                    Text1 = "United Travel Options",
                    Text2 = "per person",
                    Text3 = "From",
                    CurrencyCode = "$",
                };
            }

            return null;
        }
    }
}
