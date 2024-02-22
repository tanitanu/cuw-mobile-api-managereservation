using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Payment;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ProductRequestModel;
using United.Service.Presentation.ProductResponseModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using Offer = United.Service.Presentation.ProductResponseModel.Offer;
using ProductOffer = United.Service.Presentation.ProductResponseModel.ProductOffer;
using RegisterOfferRequest = United.Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest;
using ServiceResponse = United.Service.Presentation.CommonModel.ServiceResponse;

namespace United.Common.Helper.FOP
{
    public class ProductOffers : IProductOffers
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IPaymentUtility _paymentUtility;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly string _UPGRADEMALL = "UPGRADEMALL";
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IHeaders _headers;
        public ProductOffers(IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IPaymentUtility paymentUtility
            , IShoppingCartService shoppingCartService
            , IDynamoDBService dynamoDBService
            , IShoppingUtility shoppingUtility
            , IHeaders headers
            )
        {
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _paymentUtility = paymentUtility;
            _shoppingCartService = shoppingCartService;
            _dynamoDBService = dynamoDBService;
            _shoppingUtility = shoppingUtility;
            _headers = headers;
        }
        public async Task<string> CreateCart(MOBRequest request, Session session)
        {
            string response = string.Empty;

            try
            {
                string jsonRequest = JsonConvert.SerializeObject(string.Empty);
                response = await _shoppingCartService.CreateCart(session.Token, jsonRequest, session.SessionId).ConfigureAwait(false);
            }
            catch (System.Net.WebException wex)
            {
                response = null;
                var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                throw new System.Exception(errorResponse);
            }
            catch (System.Exception ex)
            {
                response = null;
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                throw new System.Exception(exceptionWrapper.Message.ToString());
            }

            return response;
        }

        public async Task<FlightReservationResponse> RegisterOffers
            (MOBRegisterOfferRequest request, ProductOffer productOffer, ProductOffer productVendorOffer, DynamicOfferDetailResponse pomOffer, ReservationDetail reservationDetail, Session session, Collection<RegisterOfferRequest>
            upgradeCabinRegisterOfferRequest = null, ProductOffer cceDODOfferResponse = null)
        {
            var response = new FlightReservationResponse();

            bool isUpgradeMallRequest
                = (string.Equals(request.Flow, _UPGRADEMALL, StringComparison.OrdinalIgnoreCase));

            if (((productOffer != null || productVendorOffer != null || pomOffer != null || cceDODOfferResponse !=null) && reservationDetail != null)
                || isUpgradeMallRequest)
            {
                var registerOfferRequests
                    = (isUpgradeMallRequest) ? upgradeCabinRegisterOfferRequest
                    : await BuildRegisterOffersRequest(request, productOffer, productVendorOffer, pomOffer, reservationDetail, cceDODOfferResponse);

                if (registerOfferRequests != null && registerOfferRequests.Count > 0)
                {
                    string jsonRequest = JsonConvert.SerializeObject(registerOfferRequests);
                    string path = "/RegisterOffers";

                    if (session == null)
                    {
                        throw new MOBUnitedException("Could not find your session.");
                    }

                    //string jsonResponse = HttpHelper.Post(url, "application/json; charset=utf-8", session.Token, jsonRequest);
                    response = await _shoppingCartService.FareLockReservation<FlightReservationResponse>(session.Token, path, session.SessionId, jsonRequest).ConfigureAwait(false);

                    if (response != null)
                    {
                        if (!(response != null && response.Status.Equals(United.Services.FlightShopping.Common.StatusType.Success) && response.DisplayCart != null))
                        {
                            if (response.Errors != null && response.Errors.Count > 0)
                            {
                                string errorMessage = string.Empty;
                                foreach (var error in response.Errors)
                                {
                                    errorMessage = errorMessage + " " + error.Message;
                                }

                                throw new System.Exception(errorMessage);
                            }
                        }
                    }
                    else
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                    }
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            return response;
        }

        public RegisterOfferRequest GetRegisterOffersRequest(string cartId, string cartKey, string languageCode, string pointOfSale, string productCode, string productId, List<string> productIds, string subProductCode, bool delete, ProductOffer Offer, ReservationDetail reservation)
        {
            RegisterOfferRequest registerOfferRequest = new RegisterOfferRequest();

            registerOfferRequest.Offer = (Offer == null ? null : Offer);
            registerOfferRequest.AutoTicket = false;
            registerOfferRequest.CartId = cartId;
            registerOfferRequest.Characteristics = new System.Collections.ObjectModel.Collection<United.Service.Presentation.CommonModel.Characteristic>() {
                    new Characteristic() { Code = "ManageRes", Value = "true" }};
            registerOfferRequest.CartKey = cartKey;
            registerOfferRequest.CountryCode = pointOfSale;
            registerOfferRequest.Delete = delete;
            registerOfferRequest.LangCode = languageCode;
            registerOfferRequest.ProductCode = productCode;
            registerOfferRequest.ProductId = productId;
            registerOfferRequest.ProductIds = productIds;
            registerOfferRequest.SubProductCode = subProductCode;
            registerOfferRequest.Reservation = (reservation == null ? null : reservation.Detail);

            return registerOfferRequest;
        }

        private async Task<Collection<RegisterOfferRequest>> BuildRegisterOffersRequest(MOBRegisterOfferRequest request, ProductOffer productOffer, ProductOffer productVendorOffer, DynamicOfferDetailResponse pomOffer, ReservationDetail reservation, ProductOffer cceDODOfferResponse)
        {
            var reservationDetail = new ReservationDetail();
            var selectedOffer = new ProductOffer();
            var registerOfferRequests = new Collection<RegisterOfferRequest>();
            int reservationCtr = 0;
            List<MOBItem> selectedProductsAndCount = new List<MOBItem>();

            if (request.MerchandizingOfferDetails.Any(a => a.ProductCode == _configuration.GetValue<string>("InflightMealProductCode")))
            {
                var selectedOffers = await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(request.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { request.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false); //change session

                United.Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest registerOfferRequest = GetInflightMealsRegisterOfferRequest(pomOffer, request, reservation);
                var pastSelections = GetPastRefreshmentSelections(request, pomOffer, selectedOffers);

                var toggleCheck =_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version?.Major);
                if ((!toggleCheck && selectedOffers == null) || (toggleCheck && selectedOffers == null && pastSelections.Count == 0))
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                registerOfferRequest.Products[0].SelectedProductRequests = GetInFlightMealSelectedOffersRequest(selectedOffers, pastSelections, request);
                if (registerOfferRequest.Products[0].SelectedProductRequests != null)
                    registerOfferRequest.Products[0].Ids = registerOfferRequest.Products[0].SelectedProductRequests.Select(a => a.ProductId).ToList();
                registerOfferRequest.WorkFlowType = ConfigUtility.GetWorkFlowType(request.Flow, _configuration.GetValue<string>("InflightMealProductCode"));// todo check for other products thrugh debug till checkout                
                registerOfferRequests.Add(registerOfferRequest);
            }
            else
            {
                ProductOffer productOfferFromCce = await LoadProductOfferFromPersist(request);

                foreach (var merchandizingOfferDetail in request.MerchandizingOfferDetails)
                {
                    selectedOffer = GetSelectedOffer(productOffer, productVendorOffer, productOfferFromCce, merchandizingOfferDetail.ProductCode, cceDODOfferResponse);
                    if (!(selectedOffer.Offers.FirstOrDefault().ProductInformation.ProductDetails.Any(x => x.Product.Code.Equals(merchandizingOfferDetail.ProductCode)))
                    || selectedOffer.Offers.FirstOrDefault().ProductInformation.ProductDetails.Where(x => x.Product.Code.Equals(merchandizingOfferDetail.ProductCode)).Select(c => c.Product.SubProducts.Where(p =>p.Prices!=null).All(x => x.Prices.Count() == 0 || x.Prices == null)).FirstOrDefault())
                        continue;
                    else
                    {
                        Service.Presentation.ProductResponseModel.ProductOffer selOffer = null;
                        if (!(merchandizingOfferDetail.IsOfferRegistered))
                        {
                            selOffer = new ProductOffer();
                            selOffer.Offers = new Collection<Offer>();
                            Offer offer = new Offer();
                            offer.ProductInformation = new ProductInformation();
                            offer.ProductInformation.ProductDetails = new Collection<ProductDetail>();
                            offer.ProductInformation.ProductDetails.Add(selectedOffer.Offers[0].ProductInformation.ProductDetails.Where(x => x.Product.Code.Equals(merchandizingOfferDetail.ProductCode)).FirstOrDefault());
                            selOffer.Offers.Add(offer);
                            selOffer.Travelers = new Collection<ProductTraveler>();
                            selOffer.Travelers = selectedOffer.Travelers;
                            selOffer.Solutions = new Collection<Solution>();
                            selOffer.Solutions = selectedOffer.Solutions;
                            selOffer.Response = new ServiceResponse();
                            selOffer.Response = selectedOffer.Response;
                            selOffer.FlightSegments = new Collection<ProductFlightSegment>();
                            selOffer.FlightSegments = selectedOffer.FlightSegments;
                            if (_configuration.GetValue<bool>("EnableTravelOptionsInViewRes"))
                                selOffer.Requester = selectedOffer?.Requester;
                        }

                        RegisterOfferRequest registerOfferRequest = null;
                        if (merchandizingOfferDetail.IsOfferRegistered || reservationCtr != 0)
                            reservationDetail = null;
                        else
                        {
                            reservationDetail = reservation;
                            reservationCtr++;
                        }

                        registerOfferRequest = GetRegisterOffersRequest(request.CartId, null, null, null, merchandizingOfferDetail.ProductCode, null, merchandizingOfferDetail.ProductIds, merchandizingOfferDetail.SubProductCode, merchandizingOfferDetail.IsOfferRegistered, selOffer, reservationDetail);
                        if (_configuration.GetValue<bool>("EnableCSLCloudMigrationToggle"))
                        {
                            registerOfferRequest.WorkFlowType = ConfigUtility.GetWorkFlowType(request.Flow);
                        }
                        registerOfferRequests.Add(registerOfferRequest);
                    }
                }
            }
            return registerOfferRequests;
        }

        private async Task<ProductOffer> LoadProductOfferFromPersist(MOBRegisterOfferRequest request)
        {
            var persistedProductOfferFromCce = new United.Mobile.Model.ManageRes.GetOffersCce();
            persistedProductOfferFromCce = await _sessionHelperService.GetSession<United.Mobile.Model.ManageRes.GetOffersCce>(request.SessionId, persistedProductOfferFromCce.ObjectName, new List<string> { request.SessionId, persistedProductOfferFromCce.ObjectName }).ConfigureAwait(false);
            var productOfferFromCce = string.IsNullOrEmpty(persistedProductOfferFromCce.OfferResponseJson)
                                            ? null
                                            : JsonConvert.DeserializeObject<ProductOffer>(persistedProductOfferFromCce.OfferResponseJson);

            return productOfferFromCce;
        }

        private ProductOffer GetSelectedOffer(ProductOffer productOffer, ProductOffer productVendorOffer, ProductOffer productOfferFromCce, string code, ProductOffer cceDODOfferResponse)
        {
            if ((cceDODOfferResponse?.Offers?.Any(o => o.ProductInformation?.ProductDetails?.Any(p => p?.Product?.Code == code) ?? false) ?? false))
                return cceDODOfferResponse;
            else if (productOfferFromCce?.Offers?.Any(o => o.ProductInformation?.ProductDetails?.Any(p => p?.Product?.Code == code) ?? false) ?? false)
            {
                if (code == "BEB")
                {
                    var bebProduct = productOfferFromCce?.Offers?.FirstOrDefault()?.ProductInformation?.ProductDetails?.FirstOrDefault(p => p.Product.Code == "BEB").Product;
                    bebProduct.DisplayName = !_configuration.GetValue<bool>("EnableNewBEBContentChange") ? "Switch to Economy" : _configuration.GetValue<string>("BEBuyOutPaymentInformationMessage");
                }

                return productOfferFromCce;
            }
            else if (code.Equals("TPI"))
                return productVendorOffer;
            else
                return productOffer;
        }

        private RegisterOfferRequest GetInflightMealsRegisterOfferRequest(DynamicOfferDetailResponse pomOffer, MOBRegisterOfferRequest request, ReservationDetail reservation)
        {
            RegisterOfferRequest registerOfferRequest = new RegisterOfferRequest();
            registerOfferRequest.Offer = new ProductOffer();
            registerOfferRequest.Offer.Offers = pomOffer.Offers;
            registerOfferRequest.Offer.FlightSegments = pomOffer.FlightSegments;
            registerOfferRequest.Offer.ODOptions = pomOffer.ODOptions;
            registerOfferRequest.Offer.Requester = pomOffer.Requester;
            registerOfferRequest.Offer.Solutions = pomOffer.Solutions;
            registerOfferRequest.Offer.Travelers = pomOffer.Travelers;

            registerOfferRequest.AutoTicket = false;
            registerOfferRequest.CartId = request.CartId;
            registerOfferRequest.CartKey = null;
            registerOfferRequest.CountryCode = null;
            registerOfferRequest.Delete = false;
            registerOfferRequest.LangCode = null;
            registerOfferRequest.ProductCode = request.MerchandizingOfferDetails[0].ProductCode;
            registerOfferRequest.SubProductCode = null;
            registerOfferRequest.Reservation = (reservation == null ? null : reservation.Detail);
            registerOfferRequest.SubProductCode = _configuration.GetValue<string>("InflightMealProductCode");
            registerOfferRequest.WorkFlowType = WorkFlowType.PreOrderMeals;           

            registerOfferRequest.Products = new List<ProductRequest>
                {
                    new ProductRequest
                    {
                        Code =  _configuration.GetValue<string>("InflightMealProductCode"),
                        Ids = new List<string>(),
                        SelectedProductRequests = new List<ProductSelectedRequest>()
                    }
                };

            if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version.Major))
            {
                var list = new Collection<Characteristic>();
                list.Add(new Characteristic() { Code = "ManageRes", Value = "true" });

                var mealType = pomOffer.Offers?.FirstOrDefault()?.ProductInformation?.ProductDetails?.FirstOrDefault()?.Product?.SubProducts?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                var merchandizingOfferDetail = request.MerchandizingOfferDetails.FirstOrDefault();
                var segmentNumber = merchandizingOfferDetail?.ProductIds?.FirstOrDefault();
                var flightSegment = pomOffer.FlightSegments?.FirstOrDefault(x => x.SegmentNumber.ToString() == segmentNumber);

                if (mealType == InflightMealType.Refreshment.ToString() && flightSegment != null)
                {
                    registerOfferRequest.Delete = true;
                    list.Add(new Characteristic() { Code = "SegmentNumber", Value = segmentNumber });
                    list.Add(new Characteristic() { Code = "Pnr", Value = pomOffer.Response.RecordLocator });
                    list.Add(new Characteristic() { Code = "FlightNumber", Value = flightSegment?.FlightNumber });
                    list.Add(new Characteristic() { Code = "DepartureAirport", Value = flightSegment.DepartureAirport.IATACode });
                    list.Add(new Characteristic() { Code = "ArrivalAirport", Value = flightSegment.ArrivalAirport.IATACode });
                    list.Add(new Characteristic() { Code = "DepartureDateTime", Value = flightSegment.DepartureDateTime });
                    list.Add(new Characteristic() { Code = "ClearAll", Value = "true" });
                }
                merchandizingOfferDetail.ProductIds = null;
                registerOfferRequest.Characteristics = list;
            }
            return registerOfferRequest;
        }

        private List<ProductSelectedRequest> GetInFlightMealSelectedOffersRequest(List<MOBInFlightMealsRefreshmentsResponse> selectedOffers, List<United.Service.Presentation.ProductModel.Product> list, MOBRegisterOfferRequest request)
        {
            List<ProductSelectedRequest> selectedProductRequests = new List<ProductSelectedRequest>();
            var ids = new HashSet<String>();

            foreach (var soffer in selectedOffers)
            {
                ids.Add(soffer.Passenger.PassengerId);

                if (soffer.Snacks != null)
                {
                    foreach (var snacks in soffer?.Snacks?.Where(q => q.Quantity > 0))
                    {
                        GetInFlightMealAddSelectedProduct(selectedProductRequests, snacks);
                    }
                }

                if (soffer.Beverages != null)
                {
                    foreach (var beverage in soffer?.Beverages?.Where(q => q.Quantity > 0))
                    {
                        GetInFlightMealAddSelectedProduct(selectedProductRequests, beverage);
                    }
                }

                if (soffer.FreeMeals != null)
                {
                    foreach (var freeMeal in soffer?.FreeMeals?.Where(q => q.Quantity > 0))
                    {
                        GetInFlightMealAddSelectedProduct(selectedProductRequests, freeMeal);
                    }
                }

                if (_configuration.GetValue<bool>("EnableDynamicPOM"))
                {
                    if (soffer.DifferentPlanOptions != null)
                    {
                        foreach (var displayOption in soffer?.DifferentPlanOptions?.Where(q => q.Quantity > 0))
                        {
                            GetInFlightMealAddSelectedProduct(selectedProductRequests, displayOption);
                        }
                    }

                    if (soffer.SpecialMeals != null)
                    {
                        foreach (var specialMeal in soffer?.SpecialMeals?.Where(q => q.Quantity > 0))
                        {
                            GetInFlightMealAddSelectedProduct(selectedProductRequests, specialMeal);
                        }
                    }
                }

                if (_shoppingUtility.EnablePOMPreArrival(request.Application.Id, request.Application.Version.Major))
                {
                    if (soffer.PreArrivalFreeMeals != null)
                    {
                        foreach (var preArrivalMeal in soffer?.PreArrivalFreeMeals?.Where(q => q.Quantity > 0))
                        {
                            GetInFlightMealAddSelectedProduct(selectedProductRequests, preArrivalMeal);
                        }
                    }

                    if (soffer.PreArrivalDifferentPlanOptions != null)
                    {
                        foreach (var preArrivalDiffOption in soffer?.PreArrivalDifferentPlanOptions?.Where(q => q.Quantity > 0))
                        {
                            GetInFlightMealAddSelectedProduct(selectedProductRequests, preArrivalDiffOption);
                        }
                    }
                }
            }

            if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version.Major))
            {
                var pastSelections = list.SelectMany(prod => prod.SubProducts).SelectMany(subProd => subProd.Association.TravelerRefIDs.Where(id => !ids.Contains(id)).Select(tid => CreatePastSelectionRequest(subProd))).ToList();

                if (pastSelections != null && pastSelections.Count > 0)
                {
                    selectedProductRequests.AddRange(pastSelections);
                }
            }
            return selectedProductRequests;
        }

        private void GetInFlightMealAddSelectedProduct(List<ProductSelectedRequest> selectedProductRequests, MOBInFlightMealRefreshment refreshment)
        {
            if (selectedProductRequests.Any(x => x.EddCode == refreshment.MealCode && x.ProductId == refreshment.ProductId))
            {
                selectedProductRequests.Find(x => x.EddCode == refreshment.MealCode && x.ProductId == refreshment.ProductId).Quantity += refreshment.Quantity;
            }
            else
            {
                selectedProductRequests.Add(new ProductSelectedRequest
                {
                    EddCode = refreshment.MealCode,
                    ProductId = refreshment.ProductId,
                    Quantity = refreshment.Quantity
                });
            }
        }

        private ProductSelectedRequest CreatePastSelectionRequest(United.Service.Presentation.ProductModel.SubProduct subProd)
        {
            int quantity = 0;
            if (Int32.TryParse(subProd.Extension?.MealCatalog?.Characteristics?.Where(x => x.Code == "Quantity").FirstOrDefault().Value, out int q))
            {
                quantity = q;
            }

            return new ProductSelectedRequest
            {
                EddCode = subProd.Code,
                ProductId = subProd.Extension?.MealCatalog?.Characteristics?.Where(x => x.Code == "PriceId").FirstOrDefault().Value,
                Quantity = quantity
            };
        }

        private List<United.Service.Presentation.ProductModel.Product> GetPastRefreshmentSelections(MOBRegisterOfferRequest request, Service.Presentation.PersonalizationResponseModel.DynamicOfferDetailResponse pomOffer, List<MOBInFlightMealsRefreshmentsResponse> responses)
        {
            var pastSelections = new List<United.Service.Presentation.ProductModel.Product>();

            if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version.Major))
            {
                var segmentId = responses.FirstOrDefault()?.SegmentId;
                foreach (var offer in pomOffer.Offers)
                {
                    foreach (var productDetail in offer.ProductInformation.ProductDetails.Where(p => p.Product.Code == "PAST_SELECTION"))
                    {
                        if (productDetail.Product != null && productDetail.Product.SubProducts != null && productDetail.Product.SubProducts.Any(x => x.Association != null && x.Association.SegmentRefIDs != null && x.Association.SegmentRefIDs.Contains(segmentId)))
                        {
                            pastSelections.Add(productDetail.Product);
                        }
                    }
                }
            }
            return pastSelections;
        }

    }
}
