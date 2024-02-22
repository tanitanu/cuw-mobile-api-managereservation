using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.PriceBreakDown;
using United.Mobile.Model.Shopping.UnfinishedBooking;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.CustomerResponseModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Services.Loyalty.Preferences.Common;
using United.Utility.Helper;
using FlowType = United.Utility.Enum.FlowType;
using MOBBKTraveler = United.Mobile.Model.Shopping.Booking.MOBBKTraveler;

namespace United.Common.Helper
{
    public static class UtilityHelper
    {
        public static string FormatAwardAmountForDisplay(string amt, bool truncate = true)
        {
            string newAmt = amt;
            decimal amount = 0;
            decimal.TryParse(amt, out amount);

            try
            {
                if (amount > -1)
                {
                    if (truncate)
                    {
                        //int newTempAmt = (int)decimal.Ceiling(amount);
                        try
                        {
                            if (amount > 999)
                            {
                                amount = amount / 1000;
                                if (amount % 1 > 0)
                                {
                                    newAmt = string.Format("{0:n1}", amount) + "K miles";
                                }
                                else
                                {
                                    newAmt = string.Format("{0:n0}", amount) + "K miles";
                                }
                            }
                            else
                            {
                                newAmt = string.Format("{0:n0}", amount) + " miles";
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            newAmt = string.Format("{0:n0}", amount) + " miles";
                        }
                        catch { }
                    }
                }
            }
            catch { }

            return newAmt;
        }

        public static string RemoveString(string text, string textToRemove)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(textToRemove))
            {
                return text;
            }

            var length = textToRemove.Length;
            var index = text.IndexOf(textToRemove, StringComparison.InvariantCultureIgnoreCase);
            return index != -1 ? Regex.Replace(text.Remove(index, length).Trim(), @"\s+", " ") : text;
        }

        public static string GetAndroidVersionKey(FlowType flow)
        {
            switch (flow)
            {
                case FlowType.ALL:
                case FlowType.BOOKING:
                case FlowType.BAGGAGECALCULATOR:
                    return "AndroidELFVersion";
                case FlowType.MANAGERES:
                    return "AndroidManageResMinSupportedVersion";
                case FlowType.RESHOP:
                    return "AndroidReshopMinSupportedVersion";
                case FlowType.ERES:
                    return "AndroidERESVersion";
                default:
                    return "AndroidELFVersion";
            }
        }

        public static string GetIphoneVersionKey(FlowType flow)
        {
            switch (flow)
            {
                case FlowType.ALL:
                case FlowType.BOOKING:
                case FlowType.BAGGAGECALCULATOR:
                    return "iPhoneELFVersion";
                case FlowType.MANAGERES:
                    return "iPhoneManageResMinSupportedVersion";
                case FlowType.RESHOP:
                    return "iPhoneReshopMinSupportedVersion";
                case FlowType.ERES:
                    return "iPhoneERESVersion";
                default:
                    return "iPhoneELFVersion";
            }
        }

        public static bool CheckRevenueLowestPriceForAwardSearchEnabled(MOBSHOPShopRequest shopRequest, Mobile.Model.Shopping.ShopResponse shopResponse)
        {
            bool IsRevenueLowestPriceForAwardSearchEnabled = false;
            if (shopRequest.AwardTravel && shopResponse.Availability != null && shopResponse.Availability.FSRAlertMessages == null)
            {
                IsRevenueLowestPriceForAwardSearchEnabled = true;
            }
            return IsRevenueLowestPriceForAwardSearchEnabled;
        }

        public static string GetSSOToken(int id, string deviceId, string major, string transactionId, object p, string sessionId, string mileagePlusAccountNumber)
        {
            string ssoToken = string.Empty;

            ssoToken = null;//TODO Authentication.GetSSOToken(applicationId, deviceId, appVersion, transactionId, webConfigSession, sessionID,  mileagPlusNumber);
            return ssoToken;
        }

        public static void GetDotComRedirectParameters(MOBSHOPShopRequest request, out string fareType, out string fareClass, out string totalPax)
        {
            //faretype 
            fareType = request.FareType == "ff" ? "&ft=1" : request.FareType == "urf" ? "&ft=2" : string.Empty;

            //fare class
            fareClass = (!string.IsNullOrEmpty(request.FareClass)) ? $"&cs={request.FareClass}" : string.Empty;

            //traveler count
            totalPax = BuildPaxInfoDotComRequest(request.TravelerTypes);
        }

        private static string BuildPaxInfoDotComRequest(List<MOBTravelerType> travelerTypes)
        {
            string paxes = "&px=", numOfAdults = string.Empty, numOfSeniors = string.Empty, numOfChildren01 = string.Empty, numOfChildren02 = string.Empty, numOfChildren03 = string.Empty, numOfChildren04 = string.Empty, numOfInfants = string.Empty, numOfLapInfants = string.Empty;

            foreach (MOBTravelerType t in travelerTypes.Where(t => t.TravelerType != null))
            {
                int tType = (int)Enum.Parse(typeof(PAXTYPE), t.TravelerType);

                switch (tType)
                {
                    case (int)PAXTYPE.Adult:
                    default:
                        numOfAdults = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.Senior:
                        numOfSeniors = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.Child2To4:
                        numOfChildren01 = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.Child5To11:
                        numOfChildren02 = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.Child12To14:
                        numOfChildren03 = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.Child15To17:
                        numOfChildren04 = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.InfantSeat:
                        numOfInfants = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;

                    case (int)PAXTYPE.InfantLap:
                        numOfLapInfants = (t.Count > 0) ? $"{t.Count}," : ",";
                        break;
                }
            }

            paxes = $"{paxes}{numOfAdults}{numOfSeniors}{numOfChildren01}{numOfChildren02}{numOfChildren03}{numOfChildren04}{numOfInfants}{numOfLapInfants}";

            if (paxes != null && paxes.ToString().Length > 1 && paxes.ToString().EndsWith(","))
            {
                paxes = paxes.Remove(paxes.Length - 1, 1);
            }
            return paxes;
        }

        public static object FormatDate(string dataTime)
        {
            string formattedDate = string.Empty;

            formattedDate = Convert.ToDateTime(dataTime).ToString("yyyy-MM-dd");

            return formattedDate;
        }

        public static string GetCharacteristicDescription(List<United.Service.Presentation.CommonModel.Characteristic> characteristics, string code)
        {
            string keyDesc = string.Empty;
            if (characteristics.Exists(p => p.Code.Trim() == code && p.Value.ToUpper() == "TRUE"))
            {
                keyDesc = characteristics.First(p => p.Code.Trim() == code && p.Value.ToUpper() == "TRUE").Description;
            }
            return keyDesc;
        }

        public static int GetTravelerCount(List<MOBTravelerType> lst)
        {
            return lst.Where(t => t.TravelerType != null && t.Count > 0).Select(t => t.Count).Sum();
        }

        public static string GetThousandPlaceCommaDelimitedNumberString(string integer)
        {
            try
            {
                int intResult;

                if (!int.TryParse(integer.Trim(), out intResult))
                {
                    return string.Empty;
                }

                return string.Format("{0:n0}", intResult);
            }
            catch { return string.Empty; }
        }

        public static string GetSubTitleForNoResults(MOBSHOPShopRequest request, int travelCount)
        {
            try
            {
                string traveler = travelCount <= 1 ? travelCount + " traveler" : travelCount + " travelers";
                string date = Convert.ToDateTime(request.Trips[0].DepartDate).ToString("ddd MMM d", CultureInfo.CreateSpecificCulture("en-US"));
                string tripCount = request.Trips.Count.ToString();
                string searchType = request.SearchType.Equals("OW", StringComparison.OrdinalIgnoreCase) ? "One-way" : request.SearchType.Equals("RT", StringComparison.OrdinalIgnoreCase) ? "RoundTrip" : "Flight 1 of " + tripCount;
                return searchType + ", " + traveler + ", " + date;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string FormatAllCabinAwardAmountNoMiles(string amt, bool truncate = true)
        {
            string newAmt = amt;
            decimal amount = 0;
            decimal.TryParse(amt, out amount);

            try
            {
                if (amount > -1)
                {
                    if (truncate)
                    {
                        //int newTempAmt = (int)decimal.Ceiling(amount);
                        try
                        {
                            if (amount > 999)
                            {
                                amount = amount / 1000;
                                if (amount % 1 > 0)
                                {
                                    newAmt = string.Format("{0:n1}", amount) + "K";
                                }
                                else
                                {
                                    newAmt = string.Format("{0:n0}", amount) + "K";
                                }
                            }
                            else if (amount == 0)
                            {
                                newAmt = "---";
                            }
                            else
                            {
                                newAmt = string.Format("{0:n0}", amount);
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            newAmt = string.Format("{0:n0}", amount);
                        }
                        catch { }
                    }
                }
            }
            catch { }

            return newAmt;
        }

        public static string CabinButtonTextWithPrice(string price, string newAmt)
        {
            if (!String.IsNullOrEmpty(newAmt))
            {
                if (!string.IsNullOrEmpty(price) && !price.ToUpper().Equals("NOT AVAILABLE"))
                {
                    newAmt = newAmt + " " + price.Replace(" ", string.Empty);
                }
                newAmt = newAmt.Replace(" ", "\n");
            }

            return newAmt;
        }

        public static MOBSHOPShopRequest GetMobShopRequest(ShopRequest cslShopRequest, MOBRequest mobRequest, string sessionId, string logAction)
        {
            MOBSHOPShopRequest mOBSHOPShop = new MOBSHOPShopRequest
            {

                //build application obj
                AccessCode = mobRequest.AccessCode,
                TransactionId = mobRequest.TransactionId,
                Application = mobRequest.Application,
                DeviceId = mobRequest.DeviceId
            };

            //build trip type  
            switch (cslShopRequest.SearchTypeSelection)
            {
                case SearchType.OneWay:
                    mOBSHOPShop.SearchType = "OW";
                    break;
                case SearchType.RoundTrip:
                    mOBSHOPShop.SearchType = "RT";
                    break;
                case SearchType.MultipleDestination:
                    mOBSHOPShop.SearchType = "MD";
                    break;
            }
            //build trip
            mOBSHOPShop.Trips = new List<MOBSHOPTripBase>();
            foreach (var cslTrip in cslShopRequest.Trips)
            {
                MOBSHOPTripBase trip = new MOBSHOPTripBase
                {
                    DepartDate = DateTime.Parse(cslTrip.DepartDate).ToString("MM/dd/yyyy"),
                    Destination = cslTrip.Destination,
                    Origin = cslTrip.Origin
                };
                var cslCabin = cslTrip.Flights?.Select(f => f.Products?.Select(p => p?.ProductType).FirstOrDefault()).FirstOrDefault();

                string cabin = string.Empty;
                switch (cslCabin?.Trim().ToUpper())
                {
                    case "ECONOMY":
                        cabin = "econ";
                        break;
                    case "BUSINESS":
                        cabin = "business";
                        break;
                    case "FIRST":
                        cabin = "first";
                        break;
                    default:
                        cabin = "econ";
                        break;
                }

                trip.Cabin = cabin;

                mOBSHOPShop.Trips.Add(trip);
            }
            //build pax  
            mOBSHOPShop.TravelerTypes = new List<MOBTravelerType>
            {
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Adult), TravelerType = PAXTYPE.Adult.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Senior), TravelerType = PAXTYPE.Senior.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Child01), TravelerType = PAXTYPE.Child2To4.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Child02), TravelerType = PAXTYPE.Child5To11.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Child03), TravelerType = PAXTYPE.Child12To14.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.Child04), TravelerType = PAXTYPE.Child15To17.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.InfantLap), TravelerType = PAXTYPE.InfantLap.ToString() },
                new MOBTravelerType() { Count = cslShopRequest.PaxInfoList.Count(c => c.PaxType == PaxType.InfantSeat), TravelerType = PAXTYPE.InfantSeat.ToString() }
            };

            mOBSHOPShop.IsShareTripSearchAgain = true;

            return mOBSHOPShop;
        }

        public static void GetTravelTypesInfoFromShop(SerializableSavedItinerary ub, MOBSHOPUnfinishedBooking mobEntry)
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

        public static string GetSeachTypeSelection(SearchType searchType)
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

        public static void AssignCovidTestIndicator(MOBSHOPReservation reservation)
        {
            bool hasCovidTestFlight =
                reservation.Trips.Any(
                    t =>
                        t.FlattenedFlights.Any(
                            ff =>
                                ff.Flights.Any(
                                    f =>
                                        f.IsCovidTestFlight)));
            if (hasCovidTestFlight)
            {
                if (reservation.ShopReservationInfo2 == null)
                {
                    reservation.ShopReservationInfo2 = new ReservationInfo2();
                }

                reservation.ShopReservationInfo2.IsCovidTestFlight = true;
            }
        }

        public static List<DisplayTravelType> GetDisplayTravelerTypes(List<DisplayTraveler> displayTravelers)
        {
            List<DisplayTravelType> lst = new List<DisplayTravelType>();
            if (displayTravelers != null && displayTravelers.Count > 0)
            {
                int paxID = 1;
                foreach (var traveler in displayTravelers)
                {
                    for (int i = 0; i < traveler.TravelerCount; i++)
                    {
                        DisplayTravelType t = new DisplayTravelType
                        {
                            TravelerDescription = traveler.PaxTypeDescription,
                            PaxID = paxID++,
                            PaxType = traveler.PaxTypeCode,
                            TravelerType = GetTType(traveler.PaxTypeCode)
                        };

                        lst.Add(t);
                    }
                }
                lst.OrderBy(item => item.PaxID);
            }

            return lst;
        }

        private static PAXTYPE GetTType(string paxTypeCode)
        {
            if (paxTypeCode.ToUpper().Equals("ADT"))
            {
                return PAXTYPE.Adult;
            }

            if (paxTypeCode.ToUpper().Equals("INS"))
            {
                return PAXTYPE.InfantSeat;
            }

            if (paxTypeCode.ToUpper().Equals("C04"))
            {
                return PAXTYPE.Child2To4;
            }

            if (paxTypeCode.ToUpper().Equals("C11"))
            {
                return PAXTYPE.Child5To11;
            }

            if (paxTypeCode.ToUpper().Equals("C14"))
            {
                return PAXTYPE.Child12To14;
            }

            if (paxTypeCode.ToUpper().Equals("C17"))
            {
                return PAXTYPE.Child15To17;
            }

            if (paxTypeCode.ToUpper().Equals("SNR"))
            {
                return PAXTYPE.Senior;
            }

            if (paxTypeCode.ToUpper().Equals("INF"))
            {
                return PAXTYPE.InfantLap;
            }

            return PAXTYPE.Adult;
        }

        public static List<MOBSHOPTax> AddFeesAfterPriceChange(List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> prices)
        {
            List<MOBSHOPTax> taxsAndFees = new List<MOBSHOPTax>();
            CultureInfo ci = null;

            foreach (var price in prices)
            {
                if (ci == null)
                {
                    ci = TopHelper.GetCultureInfo(price.Currency);
                }
                MOBSHOPTax taxNfee = new MOBSHOPTax
                {
                    CurrencyCode = price.Currency,
                    Amount = price.Amount
                };
                taxNfee.DisplayAmount = TopHelper.FormatAmountForDisplay(taxNfee.Amount, ci, false);
                taxNfee.TaxCode = price.Type;
                taxNfee.TaxCodeDescription = price.Description;
                if (taxNfee.TaxCode != "MPF")
                    taxsAndFees.Add(taxNfee);
            }

            return taxsAndFees;
        }

        public static InfoWarningMessages BuildInfoWarningMessages(string message)
        {
            var infoWarningMessages = new InfoWarningMessages
            {
                Order = MOBINFOWARNINGMESSAGEORDER.BEFAREINVERSION.ToString(),
                IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString(),
                Messages = new List<string>
                {
                    message
                }
            };

            return infoWarningMessages;
        }

        public static bool IdentifyBEFareInversion(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse res)
        {
            bool bEFareInversion = false;
            if (res.DisplayCart.DisplayTrips != null)
            {
                foreach (var displaytrips in res.DisplayCart.DisplayTrips)
                {
                    if (displaytrips.Flights != null)
                    {
                        foreach (var flights in displaytrips.Flights)
                        {
                            if (flights.IsSelectedClassChanged)
                            {
                                bEFareInversion = true;
                                break;
                            }
                        }
                    }
                    if (bEFareInversion)
                    {
                        break;
                    }
                }
            }

            return bEFareInversion;
        }

        public static bool IdentifyInhibitWarning(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse response)
        {
            if (response != null && response.Errors != null && response.Errors.Count > 0)
            {
                foreach (var err in response.Errors)
                {
                    if (err != null && !string.IsNullOrEmpty(err.MinorCode) && (err.MinorCode.Trim().Equals("10049")))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<MOBItem> GetCharacteristics(Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation == null || reservation.Characteristic == null || !reservation.Characteristic.Any())
            {
                return null;
            }

            return reservation.Characteristic.Select(c => new MOBItem { Id = c.Code, CurrentValue = c.Value }).ToList();
        }

        public static string GetFareDescription(DisplayPrice price)
        {
            return (price.Description.ToUpper().IndexOf("ADULT") == 0 ? "adult)" : (!price.Description.ToUpper().Contains("TOTAL")) ? price.Description.Replace("(", "").ToLower() : string.Empty);
        }

        public static string BuildYAPriceTypeDescription(string searchType)
        {
            if (searchType.ToUpper().Equals("OW"))
            {
                return "Fare Oneway (1 young adult)";
            }
            else
                if (searchType.ToUpper().Equals("RT"))
            {
                return "Fare Roundtrip (1 young adult)";
            }
            else
                if (searchType.ToUpper().Equals("MD"))
            {
                return "Fare Multipletrip (1 young adult)";
            }
            else
            {
                return "Fare Oneway (1 young adult)";
            }
        }

        public static string BuildPriceTypeDescription(string searchType, string priceDescription, int price, string desc, bool isFareLockViewRes, bool isCorporateFareLock)
        {
            if (!searchType.IsNullOrEmpty() && !priceDescription.IsNullOrEmpty() && !price.IsNullOrEmpty())
            {
                if (isFareLockViewRes)
                {
                    var description = isCorporateFareLock ? "traveler)" : desc;
                    return GetFormatPriceSearch(searchType) + "(" + Convert.ToString(price) + " " + description;
                }
                else
                {
                    if (!priceDescription.ToUpper().Equals("CORPORATE RATE"))
                    {
                        return GetSearchTypeDesc(searchType) + "(" + Convert.ToString(price) + " " + desc;
                    }
                }
            }

            return string.Empty;
        }

        private static string GetFormatPriceSearch(string searchType)
        {
            return !searchType.IsNullOrEmpty() ? "Fare " + searchType + " " : string.Empty;
        }

        public static void BuildTaxesAndFees(Reservation reservation, TripPriceBreakDown priceBreakDownObj, out decimal taxNfeesTotal)
        {
            taxNfeesTotal = 0;
            priceBreakDownObj.PriceBreakDownDetails.TaxAndFees = new List<PriceBreakDown2TextItems>
            {
                new PriceBreakDown2TextItems(), // blank line

                new PriceBreakDown2TextItems() { Text1 = "Taxes and fees:", Text2 = (reservation.Taxes != null && reservation.Taxes[0] != null) ? reservation.Taxes[0].TaxCodeDescription : string.Empty }
            };

            foreach (var tax in reservation.Taxes)
            {
                if (!tax.TaxCode.Equals("PERPERSONTAX"))
                {
                    if (!tax.TaxCode.Equals("TOTALTAX"))
                    {
                        // Row n+ 4 column 0
                        // Row n+ 4 column 1
                        priceBreakDownObj.PriceBreakDownDetails.TaxAndFees.Add(new PriceBreakDown2TextItems() { Text1 = string.Format("{0} {1}", tax.TaxCodeDescription, tax.DisplayAmount) });
                    }
                    if (tax.TaxCode.Equals("TOTALTAX"))
                    {
                        priceBreakDownObj.PriceBreakDownDetails.TaxAndFees.Add(new PriceBreakDown2TextItems()); // blank line
                        priceBreakDownObj.PriceBreakDownDetails.TaxAndFees.Add(new PriceBreakDown2TextItems() { Text1 = tax.TaxCodeDescription, Price1 = tax.DisplayAmount });
                        priceBreakDownObj.PriceBreakDownDetails.TaxAndFees.Add(new PriceBreakDown2TextItems()); // blank line

                        taxNfeesTotal = tax.Amount;
                    }
                }
            }
        }

        public static void BuildSeatPrices(Reservation reservation, TripPriceBreakDown priceBreakDownObj)
        {
            if (reservation.SeatPrices != null && reservation.SeatPrices.Count > 0)
            {
                // double travelTotalPrice = 0.0;
                // Row n+ 6 column 0
                // Row n+ 6 column 1
                var seatPriceSum = reservation.SeatPrices.Sum(a => a.DiscountedTotalPrice);

                priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items() { Text1 = "Economy Plus® Seats", Price3 = string.Format("${0}", seatPriceSum.ToString("F")) });

                string economyPlusSeatText = string.Empty;

                foreach (var seat in reservation.SeatPrices)
                {
                    if (seat.SeatMessage.Trim().Contains("limited recline"))
                    {
                        economyPlusSeatText = seat.SeatMessage;
                    }
                    else
                    {
                        economyPlusSeatText = "Economy Plus Seat";
                    }

                    // Row n+m+ 7 column 0
                    // Row n+ 7 column 1

                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items() { Text1 = string.Format("{0} - {1}", seat.Origin, seat.Destination) });
                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items());//blank

                    // Row n+m+ 8 column 0
                    // Row n+m+ 8 column 1

                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items()
                    {
                        Text1 = string.Format("{0} {1}", seat.NumberOftravelers, (seat.NumberOftravelers == 1) ? "traveler" : "traveler"),
                        Price1 = seat.TotalPriceDisplayValue,
                        Price2 = seat.DiscountedTotalPriceDisplayValue
                    });

                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items() { Text1 = seat.SeatMessage });

                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items());//blank
                }
                priceBreakDownObj.PriceBreakDownSummary.TravelOptions.Add(
                    new PriceBreakDown2Items()
                    {
                        Text1 = economyPlusSeatText,
                        Price1 = string.Format("${0}", seatPriceSum)

                    });
            }
        }

        public static void BuildTravelOptions(Reservation reservation, TripPriceBreakDown priceBreakDownObj)
        {
            if (reservation.TravelOptions != null && reservation.TravelOptions.Count > 0)
            {
                priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.PremiumAccess = new List<PriceBreakDown3Items>();

                foreach (var option in reservation.TravelOptions)
                {
                    if (option.Key.Equals("PAS"))
                    {
                        priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.PremiumAccess.Add(new PriceBreakDown3Items() { Text1 = option.Type, Price2 = option.DisplayAmount });

                        priceBreakDownObj.PriceBreakDownSummary.TravelOptions.Add(new PriceBreakDown2Items() { Text1 = option.Type, Price1 = option.DisplayAmount });
                        // traveOptionSummaryList.Add(new Definition.Shopping.PriceBreakDown.MOBSHOPPriceBreakDown2Items() { Text1 = option.Type, Price1 = option.DisplayAmount });

                        if (option.SubItems != null && option.SubItems.Count > 0)
                        {
                            foreach (var subOption in option.SubItems)
                            {
                                if (!string.IsNullOrEmpty(subOption.Description) && (subOption.ProductId.Equals("PremierAccessSegment")))
                                {
                                    foreach (var subItemMap in option.SubItems)
                                    {
                                        if (subOption.Value != null && subItemMap.Key != null && subItemMap.Key.Equals(subOption.Value))
                                        {
                                            //column 1
                                            priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.PremiumAccess.Add(new PriceBreakDown3Items() { Text1 = subOption.Description, Price2 = subItemMap.DisplayAmount });

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static SerializableDictionary<string, SelectTripRequest> RebuildSelectTripRequestDictionary2(
             int index, SerializableDictionary<string, SelectTripRequest> dictionary)
        {
            SerializableDictionary<string, SelectTripRequest> newDictionary =
                new SerializableDictionary<string, SelectTripRequest>();
            int i = 0;
            foreach (KeyValuePair<string, SelectTripRequest> pair in dictionary)
            {
                if (i == index)
                {
                    break;
                }
                else
                {
                    i = i + 1;
                    newDictionary.Add(pair.Key, pair.Value);
                }
            }
            return newDictionary;
        }

        public static SerializableDictionary<string, SelectTripResponse> RebuildSelectTripResponseDictionary2(
            int index, SerializableDictionary<string, SelectTripResponse> dictionary)
        {
            SerializableDictionary<string, SelectTripResponse> newDictionary =
                new SerializableDictionary<string, SelectTripResponse>();
            int i = 0;
            foreach (KeyValuePair<string, SelectTripResponse> pair in dictionary)
            {
                if (i == index)
                {
                    break;
                }
                else
                {
                    i = i + 1;
                    newDictionary.Add(pair.Key, pair.Value);
                }
            }
            return newDictionary;
        }

        public static SelectTripRequest GetLastSelectedTrip_TripID(SerializableDictionary<string, SelectTripRequest> dictionary)
        {
            SerializableDictionary<string, SelectTripRequest> newDictionary = new SerializableDictionary<string, SelectTripRequest>();
            int i = 0;
            foreach (KeyValuePair<string, SelectTripRequest> pair in dictionary)
            {
                if (i == (dictionary.Count - 1))
                {
                    return pair.Value;
                }
                i = i + 1;
            }
            return null;
        }

        public static string GetPreviousKey(string currentTripId, SerializableDictionary<string, SelectTripRequest> dictionary)
        {
            string prevKey = string.Empty;

            foreach (KeyValuePair<string, SelectTripRequest> pair in dictionary)
            {
                if (pair.Key == currentTripId)
                {
                    break;
                }
                else
                {
                    prevKey = pair.Key;
                }
            }

            return prevKey;
        }

        public static SerializableDictionary<string, SelectTripRequest> RebuildSelectTripRequestDictionary(string currentTripId, SerializableDictionary<string, SelectTripRequest> dictionary)
        {
            SerializableDictionary<string, SelectTripRequest> newDictionary = new SerializableDictionary<string, SelectTripRequest>();

            foreach (KeyValuePair<string, SelectTripRequest> pair in dictionary)
            {
                if (pair.Key == currentTripId)
                {
                    newDictionary.Add(pair.Key, pair.Value);
                    break;
                }
                else
                {
                    newDictionary.Add(pair.Key, pair.Value);
                }
            }

            return newDictionary;
        }

        public static SerializableDictionary<string, SelectTripResponse> RebuildSelectTripResponseDictionary(string currentTripId, SerializableDictionary<string, SelectTripResponse> dictionary)
        {
            SerializableDictionary<string, SelectTripResponse> newDictionary = new SerializableDictionary<string, SelectTripResponse>();

            foreach (KeyValuePair<string, SelectTripResponse> pair in dictionary)
            {
                if (pair.Key == currentTripId)
                {
                    newDictionary.Add(pair.Key, pair.Value);
                    break;
                }
                else
                {
                    newDictionary.Add(pair.Key, pair.Value);
                }
            }

            return newDictionary;
        }

        public static void UpdateCertificateRedeemAmountFromTotalInReserationPrices(MOBSHOPPrice price, double value, bool isRemove = true)
        {
            if (price != null)
            {
                if (isRemove)
                {
                    price.Value -= value;
                }
                else
                {
                    price.Value += value;
                }
                price.Value = Math.Round(price.Value, 2, MidpointRounding.AwayFromZero);
                price.FormattedDisplayValue = (price.Value).ToString("C2", CultureInfo.CurrentCulture);
                price.DisplayValue = string.Format("{0:#,0.00}", price.Value);
            }
        }

        public static void UpdateCertificateRedeemAmountInSCProductPrices(ProdDetail scProduct, double value, bool isRemove = true)
        {
            if (scProduct != null)
            {
                double prodValue = Convert.ToDouble(scProduct.ProdTotalPrice);
                prodValue = Math.Round(prodValue, 2, MidpointRounding.AwayFromZero);
                double prodValueAfterUpdate;
                if (isRemove)
                {
                    prodValueAfterUpdate = prodValue - value;
                }
                else
                {
                    prodValueAfterUpdate = prodValue + value;
                }
                prodValueAfterUpdate = Math.Round(prodValueAfterUpdate, 2, MidpointRounding.AwayFromZero);
                scProduct.ProdTotalPrice = (prodValueAfterUpdate).ToString("N2", CultureInfo.CurrentCulture);
                scProduct.ProdDisplayTotalPrice = (prodValueAfterUpdate).ToString("C2", CultureInfo.CurrentCulture);
            }
        }

        public static MOBSHOPPrice BuildGrandTotalPriceForReservation(double grandtotal)
        {
            grandtotal = Math.Round(grandtotal, 2, MidpointRounding.AwayFromZero);
            MOBSHOPPrice totalPrice = new MOBSHOPPrice
            {
                CurrencyCode = "USD",
                DisplayType = "Grand Total",
                DisplayValue = grandtotal.ToString("N2", CultureInfo.InvariantCulture)
            };
            totalPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(totalPrice.DisplayValue, TopHelper.GetCultureInfo(totalPrice.CurrencyCode), false); //string.Format("${0:c}", totalPrice.DisplayValue);
            double tempDouble1 = 0;
            double.TryParse(totalPrice.DisplayValue.ToString(), out tempDouble1);
            totalPrice.Value = Math.Round(tempDouble1, 2, MidpointRounding.AwayFromZero);
            totalPrice.PriceType = "Grand Total";
            return totalPrice;
        }

        public static void AddGrandTotalIfNotExistInPricesAndUpdateCertificateValue(List<MOBSHOPPrice> prices, MOBFormofPaymentDetails formofPaymentDetails)
        {
            var grandTotalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL"));
            if (grandTotalPrice == null)
            {
                var totalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("TOTAL"));
                grandTotalPrice = BuildGrandTotalPriceForReservation(totalPrice.Value);
                prices.Add(grandTotalPrice);
            }
        }

        public static void ClearUnmatchedCertificatesAfterEditTravelers(MOBShoppingCart shoppingCart, FOPTravelerCertificateResponse persistedTravelCertifcateResponse, List<MOBSHOPPrice> prices)
        {
            List<MOBFOPCertificate> removeCertificates = new List<MOBFOPCertificate>();
            foreach (var certificate in persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails.TravelCertificate.Certificates)
            {
                if (certificate.CertificateTraveler?.TravelerNameIndex == "0")
                {
                    foreach (var sctraveler in shoppingCart.SCTravelers)
                    {
                        var persistResponseTraveler = persistedTravelCertifcateResponse.ShoppingCart.SCTravelers.Find(st => st.PaxID == sctraveler.PaxID);
                        if (persistResponseTraveler == null || IsValuesChangedForSameTraveler(sctraveler, persistResponseTraveler))
                        {
                            removeCertificates.Add(certificate);
                        }
                    }
                }
                else if (shoppingCart.CertificateTravelers != null && !shoppingCart.CertificateTravelers.Exists(ct => ct.PaxId == certificate.CertificateTraveler.PaxId && ct.IsCertificateApplied))
                {
                    removeCertificates.Add(certificate);
                }
                else
                {
                    var scTraveler = shoppingCart.SCTravelers.Find(st => st.PaxID == certificate.CertificateTraveler.PaxId);
                    var persistResponseTraveler = persistedTravelCertifcateResponse.ShoppingCart.SCTravelers.Find(st => st.PaxID == certificate.CertificateTraveler.PaxId);
                    if (scTraveler != null && persistResponseTraveler != null && IsValuesChangedForSameTraveler(scTraveler, persistResponseTraveler))
                    {
                        removeCertificates.Add(certificate);
                    }
                }
            }
            if (removeCertificates.Count > 0)
            {
                foreach (var removeCertificate in removeCertificates)
                {
                    var scRemovedOrEditTraveler = shoppingCart.CertificateTravelers?.Find(ct => ct.PaxId == removeCertificate.CertificateTraveler.PaxId);
                    if (scRemovedOrEditTraveler != null)
                    {
                        scRemovedOrEditTraveler.IsCertificateApplied = false;
                    }
                }
                persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.RemoveAll(c => removeCertificates.Contains(c));
                SelectAllTravelersAndAssignIsCertificateApplied(shoppingCart.CertificateTravelers, persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails.TravelCertificate.Certificates);
            }
            persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers = shoppingCart.CertificateTravelers;
        }

        public static bool IsValuesChangedForSameTraveler(MOBCPTraveler requestTraveler, MOBCPTraveler travelerCSLBeforeRegister)
        {
            return requestTraveler.FirstName != travelerCSLBeforeRegister.FirstName ||
                   requestTraveler.LastName != travelerCSLBeforeRegister.LastName ||
                   requestTraveler.BirthDate != travelerCSLBeforeRegister.BirthDate ||
                   requestTraveler.GenderCode != travelerCSLBeforeRegister.GenderCode ||
                   (requestTraveler.Nationality ?? "") != (travelerCSLBeforeRegister.Nationality ?? "") ||
                   (requestTraveler.CountryOfResidence ?? "") != (travelerCSLBeforeRegister.CountryOfResidence ?? "");
        }

        private static void SelectAllTravelersAndAssignIsCertificateApplied(List<MOBFOPCertificateTraveler> certTravelersCopy, List<MOBFOPCertificate> certificates)
        {
            var allTraveler = certTravelersCopy.Find(ct => ct.TravelerNameIndex == "0");
            if (allTraveler != null)
            {
                allTraveler.IsCertificateApplied = certificates.Count > 0;
            }
        }

        public static void AddAllTravelersOptionInCertificateTravelerList(MOBShoppingCart shoppingCart)
        {
            MOBFOPCertificateTraveler certificateTraveler = new MOBFOPCertificateTraveler
            {
                Name = "Apply to all travelers",
                TravelerNameIndex = "0"
            };

            if (shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates != null)
            {
                certificateTraveler.IsCertificateApplied = shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Count() > 0;
            }
            else
            {
                certificateTraveler.IsCertificateApplied = false;
            }

            if (shoppingCart.Products.Exists(p => p.Code == "RES"))
            {
                var scRESProduct = shoppingCart.Products.Find(p => p.Code == "RES");
                certificateTraveler.IndividualTotalAmount = Math.Round(Convert.ToDouble(scRESProduct.ProdTotalPrice), 2, MidpointRounding.AwayFromZero);
            }
            shoppingCart.CertificateTravelers.Add(certificateTraveler);
        }

        public static double GetTotalPriceForRESProduct(bool isPost, Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart, string flow)
        {
            double totalPrice = 0;
            switch (flow)
            {

                case "POSTBOOKING":
                case "VIEWRES":
                case "CHECKIN":
                    totalPrice = isPost ? flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RES").FirstOrDefault().Characteristics.Where(x => (x.Code != null ? x.Code.ToUpper() == "GrandTotal".ToUpper() : true)).Select(x => Convert.ToDouble(x.Value)).ToList().Sum() :
                                   flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RES").SelectMany(x => x.Characteristics).Where(x => (x.Code != null ? x.Code.ToUpper() == "GrandTotal".ToUpper() : true)).Select(x => Convert.ToDouble(x.Value)).ToList().Sum();
                    break;
                case "BOOKING"://For now booking check ia added here But we need to remove this once CSL issue is fixed.
                case "RESHOP":
                    totalPrice = isPost ? flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RES").FirstOrDefault().Price.Totals.Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() : true)).Select(x => x.Amount).ToList().Sum() :
                               flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RES").SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() : true)).Select(x => x.Amount).ToList().Sum();
                    break;
            }

            return totalPrice;
        }

        public static void AddRequestedCertificatesToFOPTravelerCertificates(List<MOBFOPCertificate> requestedCertificates, List<MOBFOPCertificate> profileTravelerCertificates, MOBFOPTravelCertificate travelerCertificate)
        {
            List<MOBFOPCertificate> requestedCertificatesCopy = new List<MOBFOPCertificate>();
            if (requestedCertificates != null && requestedCertificates.Count > 0)
            {
                requestedCertificatesCopy = requestedCertificates.Clone();
                int recentAssignedIndex = 0;
                foreach (var certificate in requestedCertificates.ToList())
                {
                    if (!certificate.IsRemove && (travelerCertificate.AllowedETCAmount - travelerCertificate.TotalRedeemAmount > 0 || travelerCertificate.Certificates.Exists(x => x.Index == certificate.Index)))
                    {
                        if (certificate.Index == 0)
                        {
                            int profileMaxId = ((profileTravelerCertificates != null && profileTravelerCertificates.Count > 0) ? profileTravelerCertificates.Max(c => c.Index) + 1 : 1);
                            int addedCerrtMaxId = ((travelerCertificate.Certificates != null && travelerCertificate.Certificates.Count > 0) ? travelerCertificate.Certificates.Max(c => c.Index) + 1 : 1);
                            certificate.Index = profileMaxId > addedCerrtMaxId ? profileMaxId : addedCerrtMaxId;
                            certificate.Index = certificate.Index > recentAssignedIndex ? certificate.Index : recentAssignedIndex + 1;
                            recentAssignedIndex = certificate.Index;
                        }
                        else
                        {
                            travelerCertificate.Certificates.RemoveAll(c => c.Index == certificate.Index);
                        }

                        AssignCertificateRedeemAmount(certificate, travelerCertificate.AllowedETCAmount - travelerCertificate.TotalRedeemAmount);
                        travelerCertificate.Certificates.Add(certificate);
                    }
                }
            }

            if (travelerCertificate.Certificates != null && !requestedCertificatesCopy.Any(x => x.Index == 0))
            {
                var existCertificatesInTraveletCertificateObject = travelerCertificate.Certificates.Where(tcc => !requestedCertificates.All(rc => rc.Index == tcc.Index));
                if (existCertificatesInTraveletCertificateObject != null && existCertificatesInTraveletCertificateObject.Count() > 0)
                {
                    foreach (var tCertificate in existCertificatesInTraveletCertificateObject)
                    {
                        if (travelerCertificate.AllowedETCAmount - travelerCertificate.TotalRedeemAmount > 0 && tCertificate.NewValueAfterRedeem > 0)
                        {
                            AssignCertificateRedeemAmount(tCertificate, travelerCertificate.AllowedETCAmount - (travelerCertificate.TotalRedeemAmount - tCertificate.RedeemAmount));
                        }
                    }
                }
            }
        }

        public static void AssignCertificateRedeemAmount(MOBFOPCertificate requestCertificate, double amount)
        {
            if (requestCertificate.CurrentValue >= amount)
            {
                requestCertificate.RedeemAmount = amount;
                requestCertificate.NewValueAfterRedeem = requestCertificate.CurrentValue - requestCertificate.RedeemAmount;
            }
            else
            {
                requestCertificate.RedeemAmount = requestCertificate.CurrentValue;
                requestCertificate.NewValueAfterRedeem = 0;
            }
            requestCertificate.DisplayRedeemAmount = (requestCertificate.RedeemAmount).ToString("C2", CultureInfo.CurrentCulture);
            requestCertificate.DisplayNewValueAfterRedeem = (requestCertificate.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);
        }

        private static string GetSearchTypeDesc(string searchType)
        {
            if (string.IsNullOrEmpty(searchType))
            {
                return string.Empty;
            }

            string searchTypeDesc = string.Empty;
            if (searchType.ToUpper().Equals("OW"))
            {
                searchTypeDesc = "Oneway";
            }
            else if (searchType.ToUpper().Equals("RT"))
            {
                searchTypeDesc = "Roundtrip";
            }
            else if (searchType.ToUpper().Equals("MD"))
            {
                searchTypeDesc = "Multipletrip";
            }
            return (!string.IsNullOrEmpty(searchTypeDesc)) ? "Fare " + searchTypeDesc + " " : string.Empty; //"Fare Oneway "
        }

        public static bool IsCertificatesAppliedforAllIndividualTravelers(MOBShoppingCart shoppingCart)
        {
            int certificateAppliedTravelerCount = 0, travelersCount = 0;
            certificateAppliedTravelerCount = shoppingCart.CertificateTravelers.Where(x => x.TravelerNameIndex != "0" && x.IsCertificateApplied).Count();
            travelersCount = shoppingCart?.SCTravelers != null ? shoppingCart.SCTravelers.Where(sct => sct.IndividualTotalAmount > 0).Count() : 0;
            if (certificateAppliedTravelerCount == travelersCount)
            {
                return true;
            }
            return false;
        }

        public static void AddPCUToRequestWhenPCUSeatIsSelected(SeatChangeState state, ref Collection<FOPProduct> products)
        {
            if (state != null && state.BookingTravelerInfo.Any(t => t.Seats.Any(s => !string.IsNullOrEmpty(s.PcuOfferOptionId) && s.PriceAfterTravelerCompanionRules > 0)))
            {
                if (!products.Any(p => p.Code == "PCU"))
                {
                    products.Add(new FOPProduct { Code = "PCU", ProductDescription = "Premium Cabin Upsell" });
                }
            }
        }

        public static List<MOBMobileCMSContentMessages> GetSDLMessageFromList(List<CMSContentMessage> list, string title)
        {
            List<MOBMobileCMSContentMessages> listOfMessages = new List<MOBMobileCMSContentMessages>();
            list?.Where(l => l.Title.ToUpper().Equals(title.ToUpper()))?.ForEach(i => listOfMessages.Add(new MOBMobileCMSContentMessages()
            {
                Title = i.Title,
                ContentFull = i.ContentFull,
                HeadLine = i.Headline,
                ContentShort = i.ContentShort,
                LocationCode = i.LocationCode
            }));

            return listOfMessages;
        }

        public static string GetProductDescription(Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> travelOptions, string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return string.Empty;
            }

            productCode = productCode.ToUpper().Trim();
            var key = travelOptions != null && travelOptions.Any(d => d.Key == productCode) ? travelOptions.Where(d => d.Key == productCode).Select(d => d.Type).FirstOrDefault().ToString() : string.Empty;
            if (productCode == "TPI")
            {
                return "Travel insurance";
            }

            if (productCode == "FARELOCK")
            {
                return "FareLock";
            }

            if (key == "BE")
            {
                return "Travel Options Bundle";
            }

            return string.Empty;
        }

        public static List<string> GetProductDetailDescrption(IGrouping<String, SubItem> subItem)
        {
            List<string> prodDetailDescription = new List<string>();
            if (subItem != null && subItem.Any(si => si.Product != null))
            {
                foreach (var subProduct in subItem.FirstOrDefault(si => si.Product != null).Product.SubProducts)
                {
                    prodDetailDescription.Add(subProduct.Description);
                }
            }
            return prodDetailDescription;
        }

        public static string GetDisplayOriginalPrice(Decimal price, Decimal originalPrice)
        {
            if (originalPrice > 0)
            {
                return Decimal.Parse(originalPrice.ToString()).ToString("c");
            }

            return Decimal.Parse(price.ToString()).ToString("c");
        }

        public static void UpdateRefundTotal(ProdDetail prod)
        {
            decimal otherPrice = RefundedPriceOrTotalAfterRefund(prod.Segments);
            var chargedTotalPrice = GetChargedAmountFromSegments(prod.Segments);
            prod.ProdTotalPrice = String.Format("{0:0.00}", chargedTotalPrice);
            prod.ProdDisplayTotalPrice = chargedTotalPrice > 0 ? Decimal.Parse(chargedTotalPrice.ToString()).ToString("c") : string.Empty;

            if (prod.Segments != null && prod.Segments.Any() && prod.Segments.TrueForAll(s => s.SubSegmentDetails.Any(sb => sb.IsPurchaseFailure)))
            {
                prod.Segments = null;
                prod.ProdTotalPrice = string.Empty;
                prod.ProdDisplayTotalPrice = string.Empty;
            }

            if (otherPrice > 0)
            {
                prod.ProdOtherPrice = String.Format("{0:0.00}", otherPrice);
                prod.ProdDisplayOtherPrice = Decimal.Parse(otherPrice.ToString()).ToString("c");
            }
        }

        public static decimal GetChargedAmountFromSegments(List<ProductSegmentDetail> segments)
        {
            if (segments == null || !segments.Any())
            {
                return 0;
            }

            return segments.SelectMany(s => s.SubSegmentDetails).ToList().Sum(s => s != null && !string.IsNullOrEmpty(s.Price) ? Convert.ToDecimal(s.Price) : 0);
        }

        private static decimal RefundedPriceOrTotalAfterRefund(List<ProductSegmentDetail> segments)
        {
            if (segments == null || !segments.Any())
            {
                return 0;
            }

            var chargedTotalPrice = GetChargedAmountFromSegments(segments);
            var refundedTotalPrice = segments.SelectMany(s => s.SubSegmentDetails).ToList().Sum(s => s != null && s.IsPurchaseFailure && !string.IsNullOrEmpty(s.Price) ? Convert.ToDecimal(s.Price) : 0);
            if (refundedTotalPrice > 0 && chargedTotalPrice > refundedTotalPrice)
            {
                return chargedTotalPrice - refundedTotalPrice;
            }

            return refundedTotalPrice;
        }

        public static bool IsCheckinFlow(string flowName)
        {
            FlowType flowType;
            if (!Enum.TryParse(flowName, out flowType))
            {
                return false;
            }

            return flowType == FlowType.CHECKIN;
        }

        public static bool IsPurchaseFailed(bool isPCUProduct, string originalSegmentIndex, List<string> refundedSegmentNums)
        {
            if (!isPCUProduct)
            {
                return false;
            }

            if (refundedSegmentNums == null || !refundedSegmentNums.Any() || string.IsNullOrEmpty(originalSegmentIndex))
            {
                return false;
            }

            return refundedSegmentNums.Any(s => s == originalSegmentIndex);
        }

        public static bool ShouldIgnoreAmount(SubItem subItem)
        {
            if (string.IsNullOrEmpty(subItem.Reason))
            {
                return false;
            }

            return subItem.Reason.ToUpper().Equals("INF");
        }

        public static bool IsRefundSuccess(Collection<ShoppingCartItemResponse> items, out List<string> refundedSegments)
        {
            refundedSegments = null;
            var item = items.FirstOrDefault(i => i.Item.Category.Equals("Reservation.Merchandise.PCU"));
            if (item == null)
            {
                return false;
            }

            var productContext = item.Item.ProductContext.FirstOrDefault(p => !p.StartsWith("<"));
            if (productContext == null)
            {
                return false;
            }

            var refundInfo = JsonConvert.DeserializeObject<Collection<Genre>>(productContext);
            if (refundInfo != null && refundInfo.Any() && !refundInfo.Any(g => containsKey(g, "REFUNDFAILED")) &&
                refundInfo.Any(g => containsKey(g, "REFUNDED")))
            {
                refundedSegments = refundInfo.Where(g => containsKey(g, "REFUNDED")).Select(s => s.Value.Split(' ')[1]).Distinct().ToList();
                return true;
            }

            return false;
        }

        private static bool containsKey(Genre g, string key)
        {
            return g != null && !string.IsNullOrEmpty(g.Description) && g.Description.ToUpper().Contains(key);
        }

        public static Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> GetTravelOptionItems(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, string productCode)
        {
            var travelOptions = flightReservationResponse.DisplayCart.TravelOptions.Where(d => d.Key == productCode).ToCollection().Clone();

            //when pcu seat is select, we will have duplicate items in travelOptions we need ignore those items
            if (flightReservationResponse?.DisplayCart?.DisplaySeats?.Any(s => s?.PCUSeat ?? false) ?? false)
            {
                travelOptions.Where(t => t.Key == "PCU")
                                .ForEach(t => t.SubItems.RemoveWhere(sb => sb.Amount == 0 || flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s.PCUSeat && s.OriginalSegmentIndex.ToString() == sb.SegmentNumber && (s.TravelerIndex + 1).ToString() == sb.TravelerRefID)));
                travelOptions.RemoveWhere(t => t.SubItems == null || !t.SubItems.Any());
            }

            return travelOptions;
        }

        public static string GetSegmentDescription(Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> travelOptions)
        {
            if (travelOptions == null)
            {
                return string.Empty;
            }

            var refSegments = travelOptions.SelectMany(a => a.SubItems)?.Select(b => b.Product)?.Select(c => c.SubProducts?.FirstOrDefault().ReferencedSegments.FirstOrDefault());
            return refSegments?.FirstOrDefault().Origin + " - " + refSegments?.FirstOrDefault().Destination;
        }

        public static Dictionary<string, int> GetSeatPriceOrder()
        {
            return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                { "United First® Seats",1 },
                { "United First® Seat",1 },
                { "United Business® Seats",1 },
                { "United Business® Seat",1 },
                { "United Polaris℠ first Seats",1 },
                { "United Polaris℠ first Seat",1 },
                { "United Polaris℠ business Seats",1 },
                { "United Polaris℠ business Seat",1 },
                { "United® Premium Plus Seats",1 },
                { "United® Premium Plus Seat",1 },
                { "Economy Plus Seats", 2 },
                { "Economy Plus Seat", 3 },
                { "Economy Plus Seats (limited recline)", 4 },
                { "Economy Plus Seat (limited recline)", 5 },
                { "Preferred seats", 6 },
                { "Preferred seat", 7 },
                { "Advance Seat Assignments", 8 },
                { "Advance Seat Assignment", 9 },
                { string.Empty, 9 } };
        }

        public static string GetCommonSeatCode(string seatCode)
        {
            if (string.IsNullOrEmpty(seatCode))
            {
                return string.Empty;
            }

            string commonSeatCode = string.Empty;

            switch (seatCode.ToUpper().Trim())
            {
                case "SXZ": //StandardPreferredExitPlus
                case "SZX": //StandardPreferredExit
                case "SBZ": //StandardPreferredBlukheadPlus
                case "SZB": //StandardPreferredBlukhead
                case "SPZ": //StandardPreferredZone
                case "PZA":
                    commonSeatCode = "PZA";
                    break;
                case "SXP": //StandardPrimeExitPlus
                case "SPX": //StandardPrimeExit
                case "SBP": //StandardPrimeBlukheadPlus
                case "SPB": //StandardPrimeBlukhead
                case "SPP": //StandardPrimePlus
                case "PPE": //StandardPrime
                case "BSA":
                case "ASA":
                    commonSeatCode = "ASA";
                    break;
                case "EPL": //EplusPrime
                case "EPU": //EplusPrimePlus
                case "BHS": //BulkheadPrime
                case "BHP": //BulkheadPrimePlus  
                case "PSF": //PrimePlus    
                    commonSeatCode = "EPU";
                    break;
                case "PSL": //Prime                           
                    commonSeatCode = "PSL";
                    break;
                default:
                    return seatCode;
            }
            return commonSeatCode;
        }

        public static string GetFormatedDisplayPriceForSeats(string price)
        {
            if (string.IsNullOrEmpty(price))
            {
                return string.Empty;
            }

            return Decimal.Parse(price).ToString("c");
        }

        public static string GetOriginalTotalSeatPriceForStrikeOff(List<SeatAssignment> seatAssignments, List<Seat> seats, List<MOBBKTraveler> BookingTravelerInfo)
        {
            if (seatAssignments.Any(s => s.PCUSeat))
            {
                return string.Empty;
            }

            var seatsBySeatType = seats.Where(x => x.Origin == seatAssignments[0].DepartureAirportCode && x.Destination == seatAssignments[0].ArrivalAirportCode && x.FlightNumber == seatAssignments[0].FlightNumber && (GetCommonSeatCode(x.ProgramCode) == GetCommonSeatCode(seatAssignments[0].SeatPromotionCode))).ToList().Where(s => s.SeatAssignment != s.OldSeatAssignment && s.OldSeatPrice < s.Price);

            var originalPrice = seatsBySeatType.Sum(s => s.Price);
            var priceAfterCompanionRules = seatsBySeatType.Sum(s => s.PriceAfterTravelerCompanionRules);
            var bookingTravelerInfoSeats = BookingTravelerInfo.Where(t => !t.Seats.IsNullOrEmpty()).SelectMany(t => t.Seats).ToCollection();
            var seatsBySeatTypeCoupon = bookingTravelerInfoSeats.Where(x => x.Origin == seatAssignments[0].DepartureAirportCode && x.Destination == seatAssignments[0].ArrivalAirportCode && x.FlightNumber == seatAssignments[0].FlightNumber && (GetCommonSeatCode(x.ProgramCode) == GetCommonSeatCode(seatAssignments[0].SeatPromotionCode))).ToList();

            if (!seatsBySeatTypeCoupon.IsNullOrEmpty() && seatsBySeatTypeCoupon.Count > 0)
            {
                foreach (var item in seatsBySeatTypeCoupon)
                {
                    if (!item.Adjustments.IsNullOrEmpty() && !item.IsCouponApplied)
                    {
                        var priceAfterCouponApplied = seatsBySeatTypeCoupon.Sum(s => s.PriceAfterCouponApplied);
                        var PriceBeforeCouponApplied = seatsBySeatTypeCoupon.Sum(s => s.PriceBeforeCouponApplied);
                        if (PriceBeforeCouponApplied > priceAfterCouponApplied)
                        {
                            return String.Format("{0:0.00}", PriceBeforeCouponApplied);
                        }
                    }
                    else
                    {
                        if (originalPrice > priceAfterCompanionRules)
                        {
                            return String.Format("{0:0.00}", originalPrice);
                        }

                    }
                }
            }
            else
            {
                if (originalPrice > priceAfterCompanionRules)
                {
                    return String.Format("{0:0.00}", originalPrice);
                }
            }
            return string.Empty;
        }
        public static string GetOriginalTotalSeatPriceForStrikeOff(List<SeatAssignment> seatAssignments, List<Seat> seats)
        {
            if (seatAssignments.Any(s => s.PCUSeat) || seats == null)
            {
                return string.Empty;
            }

            var seatsBySeatType = seats.Where(x => x.Origin == seatAssignments[0].DepartureAirportCode && x.Destination == seatAssignments[0].ArrivalAirportCode && x.FlightNumber == seatAssignments[0].FlightNumber && (GetCommonSeatCode(x.ProgramCode) == seatAssignments[0].SeatPromotionCode)).ToList();

            var originalPrice = seatsBySeatType.Sum(s => s.Price);
            var priceAfterCompanionRules = seatsBySeatType.Sum(s => s.PriceAfterTravelerCompanionRules);
            if (originalPrice > priceAfterCompanionRules)
            {
                return String.Format("{0:0.00}", originalPrice);
            }
            return string.Empty;
        }

        public static string GetSeatTypeForDisplay(SeatAssignment s, TravelOptionsCollection travelOptions)
        {
            if (s == null)
            {
                return string.Empty;
            }

            if (s.PCUSeat)
            {
                return GetCabinNameForPcuSeat(s.TravelerIndex, s.OriginalSegmentIndex, travelOptions);
            }

            return GetCommonSeatCode(s.SeatPromotionCode);
        }

        public static string GetCabinNameForPcuSeat(int travelerIndex, int originalSegmentIndex, TravelOptionsCollection travelOptions)
        {
            if (travelOptions == null || !travelOptions.Any())
            {
                return string.Empty;
            }

            var pcutravelOption = travelOptions.Where(t => t.Key == "PCU").FirstOrDefault();
            if (pcutravelOption == null)
            {
                return string.Empty;
            }

            var subItem = pcutravelOption.SubItems.Where(s => s != null && s.Amount > 0 && s.SegmentNumber == originalSegmentIndex.ToString() && s.TravelerRefID == (travelerIndex + 1).ToString()).FirstOrDefault();
            if (subItem == null)
            {
                return string.Empty;
            }

            return subItem.Description;
        }

        public static string GetSegmentInfo(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, int SegmentNumber, int LegIndex)
        {
            if (LegIndex == 0)
            {
                return flightReservationResponse.Reservation.FlightSegments.Where(k => k.SegmentNumber == Convert.ToInt32(SegmentNumber)).Select(y => y.FlightSegment.DepartureAirport.IATACode + " - " + y.FlightSegment.ArrivalAirport.IATACode).FirstOrDefault().ToString();
            }
            else
            {
                return flightReservationResponse.Reservation.FlightSegments.Where(k => k.SegmentNumber == Convert.ToInt32(SegmentNumber)).Select(x => x.Legs).Select(x => x[LegIndex - 1]).Select(y => y.DepartureAirport.IATACode + " - " + y.ArrivalAirport.IATACode).FirstOrDefault().ToString();
            }
            //return flightReservationResponse.Reservation.FlightSegments.Where(k => k.SegmentNumber == Convert.ToInt32(SegmentNumber)).Select(k => k.Legs).FirstOrDefault().Select(y => y.DepartureAirport.IATACode + " - " + y.ArrivalAirport.IATACode).FirstOrDefault().ToString();

        }

        public static bool IsFailedSeat(SeatAssignment displaySeat, List<SeatDetail> unAssignedSeats)
        {
            if (unAssignedSeats == null || !unAssignedSeats.Any() || displaySeat == null || displaySeat.PCUSeat)
            {
                return false;
            }

            return unAssignedSeats.Any(s => s.DepartureAirport != null && s.ArrivalAirport != null &&
                                            s.DepartureAirport.IATACode == displaySeat.DepartureAirportCode &&
                                            s.ArrivalAirport.IATACode == displaySeat.ArrivalAirportCode &&
                                            s.FlightNumber == displaySeat.FlightNumber &&
                                            s.Seat != null && !string.IsNullOrEmpty(s.Seat.Identifier) &&
                                            s.Seat.Identifier == displaySeat.Seat &&
                                            s.Seat.Price != null && s.Seat.Price.Totals != null && s.Seat.Price.Totals.Any() &&
                                            s.Seat.Price.Totals.FirstOrDefault().Amount == Convert.ToDouble(displaySeat.SeatPrice));

        }

        public static string GetSeatDescription(string seatCode)
        {
            string seatDescription = string.Empty;

            switch (seatCode.ToUpper().Trim())
            {

                case "PZA"://All Preferred Seats
                    seatDescription = "Preferred seat";
                    break;
                case "ASA"://All Advance Seat assignment Seats                                       
                    seatDescription = "Advance seat assignment";
                    break;
                case "EPU": //EplusPrimePlus           
                    seatDescription = "Economy Plus®";
                    break;
                case "PSL": //Prime                            
                    seatDescription = "Economy Plus® (limited recline)";
                    break;
                default:
                    return string.Empty;
            }
            return seatDescription;
        }

        public static List<string> GetPassengerNamesRemove(IGrouping<string, SeatAssignment> t, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse response)
        {
            List<string> passengerNames = new List<string>();
            if (response?.Reservation?.Travelers != null)
            {
                t.ForEach(seat =>
                {
                    var traveler = response.Reservation.Travelers.Where(passenger => passenger.Person != null && passenger.Person.Key == seat.PersonIndex).FirstOrDefault();
                    if (traveler != null)
                    {
                        passengerNames.Add(traveler.Person.GivenName + " " + traveler.Person.Surname);
                    }

                });
            }
            return passengerNames;
        }

        public static List<string> OrderProducts(List<string> productCodes)
        {
            if (productCodes == null || !productCodes.Any())
            {
                return productCodes;
            }

            return productCodes.OrderBy(p => GetProductOrder()[GetProductKeytoOrder(p)]).ToList();
        }

        public static Dictionary<string, int> GetProductOrder()
        {
            return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                { "AAC", 0 },
                { "PAC", 1 },
                { string.Empty, 2 } };
        }

        public static string GetProductKeytoOrder(string productCode)
        {
            productCode = string.IsNullOrEmpty(productCode) ? string.Empty : productCode.ToUpper().Trim();

            if (productCode == "AAC" || productCode == "PAC")
            {
                return productCode;
            }

            return string.Empty;
        }

        public static List<string> GetProductCodes(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, string flow, bool isPost)
        {
            List<string> productCodes = new List<string>();

            if (isPost ? flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Select(x => x.Item).Any(x => x.Product.FirstOrDefault().Code == "FLK")
                        : flightReservationResponse.ShoppingCart.Items.Any(x => x.Product.FirstOrDefault().Code == "FLK"))
            {
                flow = FlowType.FARELOCK.ToString();
            }

            switch (flow)
            {
                case "BOOKING":
                case "RESHOP":
                    productCodes = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !CheckFailedShoppingCartItem(flightReservationResponse, x)).Select(x => x.Product.FirstOrDefault().Code).Distinct().ToList()
                        : flightReservationResponse.ShoppingCart.Items.Select(x => x.Product.FirstOrDefault().Code).ToList();
                    break;
                case "POSTBOOKING":
                    productCodes = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !CheckFailedShoppingCartItem(flightReservationResponse, x)).SelectMany(x => x.Product).Where(x => x.Characteristics != null && (x.Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Code).Distinct().ToList()
                        : flightReservationResponse.ShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Characteristics != null && (x.Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Code).Distinct().ToList();
                    break;
                case "FARELOCK":
                case "VIEWRES_SEATMAP":
                case "VIEWRES":
                case "CHECKIN":
                    productCodes = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => x.Product.FirstOrDefault().Code != "RES").Select(x => x.Product.FirstOrDefault().Code).Distinct().ToList()
                        : flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES").Select(x => x.Product.FirstOrDefault().Code).Distinct().ToList();
                    break;
            }

            return productCodes;
        }

        public static string GetFareLockSegmentDescription(United.Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation != null)
            {
                var traveler = reservation.Travelers.Count.ToString() + (reservation.Travelers.Count() > 1 ? " travelers" : " traveler");
                var JourneyType = GetJourneyTypeDescription(reservation);

                return !JourneyType.IsNullOrEmpty() ? JourneyType + "(" + traveler + ")" : string.Empty;
            }
            return string.Empty;
        }

        public static string GetFareLockPassengerDescription(United.Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation != null)
            {
                var traveler = reservation.Travelers.Count.ToString() + (reservation.Travelers.Count() > 1 ? " travelers" : " traveler");
                var JourneyType = GetJourneyTypeDescription(reservation);

                return !JourneyType.IsNullOrEmpty() ? "<b>" + JourneyType + "(" + traveler + ")</b>" : string.Empty;
            }
            return string.Empty;
        }

        public static string GetJourneyTypeDescription(United.Service.Presentation.ReservationModel.Reservation reservation)
        {
            if (reservation != null)
            {
                var JourneyType = reservation.Type.FirstOrDefault(o => (o.Description != null && o.Key != null && o.Description.Equals("JOURNEY_TYPE", StringComparison.OrdinalIgnoreCase)));
                return JourneyType.IsNullOrEmpty() ? GetTravelType(reservation.FlightSegments) : GetTravelType(JourneyType.Key);
            }
            return string.Empty;
        }

        private static string GetTravelType(Collection<ReservationFlightSegment> FlightSegments)
        {
            var journeytype = string.Empty;

            if (FlightSegments != null && FlightSegments.Any(p => p != null))
            {

                var maxTripNumber = FlightSegments.Max(tq => tq.TripNumber);
                var minTripNumber = FlightSegments.Min(f => f.TripNumber);

                if (maxTripNumber.ToInteger() == 1)
                {
                    journeytype = "Oneway";
                }

                if (maxTripNumber.ToInteger() == 2)
                {

                    var firstTripDepartureAirportCode = FlightSegments.Where(t => t.TripNumber == minTripNumber.ToString()).Select(t => t.FlightSegment.DepartureAirport.IATACode).FirstOrDefault();
                    var firstTripArrivalAirportCode = FlightSegments.Where(t => t.TripNumber == minTripNumber.ToString()).Select(t => t.FlightSegment.ArrivalAirport.IATACode).LastOrDefault();
                    var lastTripArrivalAirportCode = FlightSegments.Where(f => f.TripNumber == maxTripNumber.ToString()).Select(t => t.FlightSegment.ArrivalAirport.IATACode).LastOrDefault();
                    var lastTripDepartureAirportCode = FlightSegments.Where(f => f.TripNumber == maxTripNumber.ToString()).Select(t => t.FlightSegment.DepartureAirport.IATACode).FirstOrDefault();

                    if (firstTripDepartureAirportCode == lastTripArrivalAirportCode && firstTripArrivalAirportCode == lastTripDepartureAirportCode)
                    {
                        journeytype = "Roundtrip";
                    }
                    else
                    {
                        journeytype = "Multicity";
                    }

                }
                if (maxTripNumber.ToInteger() > 2)
                {
                    journeytype = "Multicity";
                }
            }


            return journeytype;
        }

        private static string GetTravelType(string JourneyType)
        {
            string Type = string.Empty;

            if (!string.IsNullOrEmpty(JourneyType))
            {
                switch (JourneyType.ToLower())
                {
                    case "one_way":
                        Type = "Oneway";
                        break;
                    case "round_trip":
                        Type = "Roundtrip";
                        break;
                    case "multi_city":
                    default:
                        return "Multicity";
                }
            }
            return Type;
        }

        public static double GetGrandTotalPriceFareLockShoppingCart(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse)
        {
            double totalPrice = 0.0;
            if (!flightReservationResponse.IsNullOrEmpty() && !flightReservationResponse.DisplayCart.IsNullOrEmpty() && !flightReservationResponse.DisplayCart.DisplayPrices.IsNullOrEmpty())
            {

                return !flightReservationResponse.DisplayCart.GrandTotal.IsNullOrEmpty() ? Convert.ToDouble(flightReservationResponse.DisplayCart.GrandTotal)
                                                                                   : Convert.ToDouble(flightReservationResponse.DisplayCart.DisplayPrices.FirstOrDefault(o => (o.Description != null && o.Description.Equals("GrandTotal", StringComparison.OrdinalIgnoreCase))).Amount);
            }
            return totalPrice;
        }

        public static double GetCloseBookingFee(bool isPost, United.Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart, string flow)
        {
            return isPost ? flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RBF").FirstOrDefault().Price.Totals.FirstOrDefault().Amount :
                                  flightReservationResponseShoppingCart.Items.SelectMany(d => d.Product).Where(d => d.Code == "RBF").SelectMany(x => x.Price.Totals).FirstOrDefault().Amount;
        }

        public static bool CheckFailedShoppingCartItem(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, United.Service.Presentation.InteractionModel.ShoppingCartItem item)
        {
            string productCode = item.Product.Select(z => z.Code).FirstOrDefault().ToString();
            bool isFailed = false;

            switch (productCode)
            {
                case "TPI":
                case "SEATASSIGNMENTS":
                    isFailed = flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Select(x => x.Item).Any(x => x.Product.Select(y => y.Code).FirstOrDefault() == productCode.ToString() && (x.Status.Contains("FAILED")));
                    break;

                default:
                    isFailed = flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Any(x => x.Item.Product.Select(y => y.Code).FirstOrDefault() == productCode.ToString() && (x.Error != null && x.Error.Count > 0));
                    break;
            }

            return isFailed;
        }
        public static int GetTTypeValue(int age, bool iSChildInLap = false)
        {
            if ((18 <= age) && (age <= 64))
            {
                return 0;
            }
            else
            if ((2 <= age) && (age < 5))
            {
                return 5;
            }
            else
            if ((5 <= age) && (age <= 11))
            {
                return 4;
            }
            else
            //if((12 <= age) && (age <= 17))
            //{

            //}
            if ((12 <= age) && (age <= 14))
            {
                return 3;
            }
            else
            if ((15 <= age) && (age <= 17))
            {
                return 2;
            }
            else
            if (65 <= age)
            {
                return 1;
            }
            else
                if (age < 2 && iSChildInLap)
            {
                return 7;
            }
            else if (age < 2)
            {
                return 6;
            }

            return 0;

        }
        public static List<MOBTravelerType> GetTravelTypesFromShop(List<DisplayTraveler> displayTravelers)
        {
            List<MOBTravelerType> lst = new List<MOBTravelerType>();
            if (displayTravelers != null && displayTravelers.Count > 0)
            {
                foreach (var t in displayTravelers.GroupBy(p => p.PaxTypeCode))
                {
                    MOBTravelerType tType = new MOBTravelerType
                    {
                        TravelerType = GetTType(t.Key).ToString(),
                        Count = t.Count()
                    };

                    lst.Add(tType);
                }
            }
            return lst;
        }

        #region SeatMap
        //in shop seat business check 
        public static List<MOBTypeOption> GetExitAdvisory()
        {
            List<MOBTypeOption> exitAdvisory = new List<MOBTypeOption>
            {
                new MOBTypeOption("1", "Be at least 15 years of age and able to perform all of the functions listed below without assistance."),
                new MOBTypeOption("2", "Not be traveling with a customer who requires special care, such as a small child, that would prevent you from performing all of the required functions listed."),
                new MOBTypeOption("3", "Read and speak well enough to understand the instructions, provided by United in printed or graphic form, for opening exits and other emergency procedures and give information during an emergency."),
                new MOBTypeOption("4", "See well enough to perform the emergency exit functions. Persons may wear glasses or contact lenses"),
                new MOBTypeOption("5", "Understand English well enough and hear well enough to understand crewmembers commands. Persons may wear a hearing aid."),
                new MOBTypeOption("6", "Be able to reach the emergency exit quickly and assist customers off an escape slide."),
                new MOBTypeOption("7", "Be able to use both hands, both arms and both legs, as well as maintain balance and be strong and flexible enough to operate the exit and any slide mechanism; open the exit and go quickly through it; stabilize the escape slide; assist others in getting off an escape slide; and clear the exit row of obstructions including window hatches as required. Some window hatches that must be lifted can weigh as much as 58 lbs. (26 kgs).")
            };

            return exitAdvisory;
        }

        public static bool HasAllIBE(List<TripSegment> segments)
        {
            return segments != null && segments.All(s => s.IsIBE);
        }
        public static bool HasAllElfSegments(List<TripSegment> segments)
        {
            return segments != null && segments.All(s => s.IsELF);
        }
        public static bool IsElfSegment(string marketingCarrier, string serviceClass)
        {
            return marketingCarrier == "UA" &&
                   serviceClass == "N";
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
        public static byte[] StringToByteArray(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string GetFormatedDateTime(string flightdate)
        {
            DateTime departureTime;
            if (DateTime.TryParse(flightdate, out departureTime))
            {
                return departureTime.ToString("MM/dd/yyyy");
            }
            return null;
        }

        public static string GetFareBasisCode(Collection<Service.Presentation.PriceModel.Price> prices, int segmentNumber)
        {
            var priceFlightSegment = GetPriceFlightSegment(prices, segmentNumber);
            return priceFlightSegment != null ? priceFlightSegment.FareBasisCode : null;
        }

        public static PriceFlightSegment GetPriceFlightSegment(Collection<Service.Presentation.PriceModel.Price> prices, int segmentNumber)
        {
            if (prices == null || !prices.Any())
            {
                return null;
            }

            return prices.Select(p => p.PriceFlightSegments.FirstOrDefault(s => s.SegmentNumber == segmentNumber)).FirstOrDefault();
        }
        #endregion

        public static bool IsUsed(Collection<Service.Presentation.PriceModel.Price> prices, int segmentNumber)
        {
            var segment = GetPriceFlightSegment(prices, segmentNumber);
            return segment != null && segment.FlightStatuses != null &&
                   segment.FlightStatuses.Any() &&
                   segment.FlightStatuses[0].Code.Equals("UA USED", StringComparison.InvariantCultureIgnoreCase);

        }

        public static bool IsATREEligible(United.Service.Presentation.ReservationResponseModel.ReservationDetail response)
        {
            try
            {
                bool.TryParse(response?.PNRChangeEligibility?.IsATREEligible, out bool isATREEligible);
                return isATREEligible;
            }
            catch { return false; }
        }

        public static string GetCharactersticDescription_New(Collection<Service.Presentation.CommonModel.Characteristic> Characteristic, string code)
        {
            try
            {
                if (Characteristic != null && Characteristic.Any())
                {
                    var selectDesc = Characteristic.FirstOrDefault(x => (string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase)));
                    return (selectDesc != null) ? selectDesc.Description : string.Empty;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static bool GetETicketStatus(United.Service.Presentation.ReservationModel.Reservation cslreservation)
        {
            string noetktedtstatus = "NO E-TKT";
            //string etktedstatus = "ETKT";
            if (cslreservation != null && cslreservation.Status != null)
            {
                if (!string.IsNullOrEmpty(cslreservation.Status.Key) && (!string.IsNullOrWhiteSpace(cslreservation.Status.Key)))
                {
                    return (!string.Equals(cslreservation.Status.Key, noetktedtstatus, StringComparison.OrdinalIgnoreCase));
                }
            }
            return false;
        }

        public static string GetRemarksDescriptionValue(Collection<Remark> remarks, string description)
        {
            if (remarks == null || remarks.Count <= 0)
            {
                return string.Empty;
            }

            var remark = remarks.FirstOrDefault(r => r != null && !string.IsNullOrEmpty(r.Description) && r.Description.Contains(description));
            return remark == null ? string.Empty : description;
        }
        public static bool CheckForCheckinEligible(Collection<ReservationFlightSegment> flightsegments)
        {
            bool isCheckinEligible = false;
            if (flightsegments != null && flightsegments.Any())
            {
                foreach (ReservationFlightSegment segment in flightsegments)
                {
                    isCheckinEligible = CheckForCheckinEligible(segment);
                }
            }
            return isCheckinEligible;
        }
        public static bool CheckUnaccompaniedMinorAvailable(Collection<United.Service.Presentation.ReservationModel.Traveler> traveler)
        {
            bool IsUnaccompaniedMinor = false;
            try
            {
                if (traveler != null && traveler.Any())
                {
                    traveler.ForEach(item =>
                    {
                        if (string.Equals(item.IsUnaccompaniedMinor, "true", StringComparison.OrdinalIgnoreCase))
                        {
                            IsUnaccompaniedMinor = true;
                            return;
                        }
                    });
                }
            }
            catch (Exception)
            { return IsUnaccompaniedMinor; }
            return IsUnaccompaniedMinor;
        }
        public static bool CheckForCheckinEligible(ReservationFlightSegment segment)
        {
            bool isCheckinEligible = false;
            if (segment != null && segment.Characteristic != null && segment.Characteristic.Any())
            {
                string checkineligible = GetCharactersticValue(segment.Characteristic, "CheckinEligibility");
                string checkedin = GetCharactersticValue(segment.Characteristic, "CHECKED-IN");

                if (GetBooleanValueFromString(checkineligible)
                    || GetBooleanValueFromString(checkedin))
                {
                    isCheckinEligible = true;
                }
            }

            return isCheckinEligible;
        }

        public static string GetCharactersticValue(Collection<Service.Presentation.CommonModel.Characteristic> characteristics, string code)
        {
            if (characteristics == null || characteristics.Count <= 0)
            {
                return string.Empty;
            }

            var characteristic = characteristics.FirstOrDefault(c => c != null && c.Code != null
            && !string.IsNullOrEmpty(c.Code) && c.Code.Trim().Equals(code, StringComparison.InvariantCultureIgnoreCase));
            return characteristic == null ? string.Empty : characteristic.Value;
        }
        private static Boolean GetBooleanValueFromString(string inputstring)
        {
            try
            {
                if (string.IsNullOrEmpty(inputstring))
                {
                    return false;
                }

                bool isTrue;
                Boolean.TryParse(inputstring, out isTrue);
                return (isTrue) ? true : false;
            }
            catch { return false; }
        }

        public static string[] SplitConcatenatedString(string value, string splitchar)
        {
            try
            {
                string[] splitSymbol = { splitchar };
                string[] splitString = value.Split(splitSymbol, StringSplitOptions.None);
                return splitString;
            }
            catch { return null; }
        }
        public static bool? GetBooleanFromCharacteristics(Collection<Service.Presentation.CommonModel.Characteristic> characteristic, string key)
        {
            if (characteristic != null && characteristic.Any())
            {
                string stringvalue = GetCharactersticValue(characteristic, key);
                if (!string.IsNullOrEmpty(stringvalue))
                {
                    Boolean.TryParse(stringvalue, out bool boolvalue);
                    return boolvalue;
                }
            }
            return null;
        }
        public static bool CheckInCabinPetAvailable(Collection<Service.Presentation.CommonModel.Service> services)
        {
            string petCode = "PETC";
            try
            {
                if (services != null && services.Any
                    (x => (string.Equals(x.Code, petCode, StringComparison.OrdinalIgnoreCase))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }

        public static void CheckSCChangeRefundEligible(ReservationDetail response, MOBPNR pnr)
        {
            if (response?.Detail?.BookingIndicators != null)
            {
                pnr.IsSCChangeEligible = response.Detail.BookingIndicators.IsSCChangeEligible;
                pnr.IsSCRefundEligible = response.Detail.BookingIndicators.IsSCRefundEligible;
            }
        }

        public static bool CheckTravelWaiverAlertAvailable
          (United.Service.Presentation.ReservationResponseModel.PNRChangeEligibilityResponse changeEligibilityResponse)
        {
            if (changeEligibilityResponse?.Policies?.FirstOrDefault()?.PolicyRule == null)
            {
                return false;
            }

            bool.TryParse(changeEligibilityResponse.IsPolicyEligible, out bool isShowTravelWaiverAlert);
            return isShowTravelWaiverAlert;
        }

        public static bool ShuttleOfferEligibilityCheck(MOBPNR mobpnr, string shuttleOfferAirportCode)
        {
            if (mobpnr != null)
            {
                //Reservation is BE / IBE
                //Reservation is bulk / group                
                //Reservation is NRSA / PS
                //Reservation is UMNR
                //Reservation eTicketed
                //No origin/ destination in EWR
                if (mobpnr.IsIBE || mobpnr.IsIBELite || mobpnr.isELF
                    || mobpnr.isgroup || mobpnr.PsSaTravel
                    || mobpnr.IsUnaccompaniedMinor || mobpnr.IsBulk
                    || !ShuttleOfferGetQualifiedLOF(mobpnr.Segments, shuttleOfferAirportCode)
                    || mobpnr.IsFareLockOrNRSA
                    || !mobpnr.IsETicketed)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ShuttleOfferGetQualifiedLOF(List<MOBPNRSegment> mobpnrsegment, string shuttleOfferAirportCode)
        {
            if (string.IsNullOrEmpty(shuttleOfferAirportCode))
            {
                return false;
            }

            if (mobpnrsegment != null && mobpnrsegment.Any())
            {
                var lastSegmentTripNumber = mobpnrsegment.Last();
                int totalLOF = (lastSegmentTripNumber != null) ? Convert.ToInt32(lastSegmentTripNumber.TripNumber) : 0;
                if (totalLOF > 0)
                {
                    for (int i = 1; i <= totalLOF; i++)
                    {
                        var lofSegments = mobpnrsegment.FindAll
                            (x => string.Equals(x.TripNumber, Convert.ToString(i), StringComparison.OrdinalIgnoreCase));
                        if (lofSegments != null && lofSegments.Any())
                        {
                            var firstSegment = lofSegments.First();
                            var lastSegment = lofSegments.Last();
                            if (firstSegment != null && lastSegment != null)
                            {
                                if (string.Equals(firstSegment.Departure.Code, shuttleOfferAirportCode, StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(lastSegment.Arrival.Code, shuttleOfferAirportCode, StringComparison.OrdinalIgnoreCase))
                                { return true; }
                            }//End of first/last check
                        }//End of LOF segments check
                    }//End of total LOF check
                }
            }
            return false;
        }
        public static bool UpgradeCabinDisplayCheck(MOBPNR mobpnr)
        {
            if (mobpnr != null)
            {
                //Reservation is BE / IBE
                //Reservation is group                
                //Reservation is NRSA / PS                
                if (mobpnr.IsIBE || mobpnr.IsIBELite || mobpnr.isELF
                    || mobpnr.isgroup || mobpnr.IsFareLockOrNRSA
                    || mobpnr.PsSaTravel || !mobpnr.IsETicketed)
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetGenderShortDesciption(string gender)
        {
            if (!string.IsNullOrEmpty(gender))
            {
                if (gender.Trim() == "Male (F)")
                {
                    return "M";
                }
                else if (gender.Trim() == "Female (F)")
                {
                    return "F";
                }
                else if (gender.Trim() == "Unspecified (X)")
                {
                    return "X";
                }
                else if (gender.Trim() == "Undisclosed (U)")
                {
                    return "U";
                }
            }
            return gender;
        }
    }
}
