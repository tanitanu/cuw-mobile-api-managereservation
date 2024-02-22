using MerchandizingServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.SeatEngine;
using United.Mobile.DataAccess.ShopSeats;
using United.Mobile.Model.Common;
using United.Mobile.Model.FligtStatus.Internal;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.Pcu;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ReferenceDataRequestModel;
using United.Service.Presentation.ReferenceDataResponseModel;
using United.Service.Presentation.ReservationRequestModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using MOBCabin = United.Mobile.Model.Shopping.MOBCabin;
using Characteristic = United.Mobile.Model.Shopping.MOBCharacteristic;
using FlowType = United.Utility.Enum.FlowType;
using IMerchandizingServices = United.Common.Helper.Merchandize.IMerchandizingServices;
using Location = MerchandizingServices.Location;
using MOBBKEquipmentDisclosure = United.Mobile.Model.Shopping.Booking.MOBBKEquipmentDisclosure;
using MOBBKFlattenedFlight = United.Mobile.Model.Shopping.Booking.MOBBKFlattenedFlight;
using MOBBKFlight = United.Mobile.Model.Shopping.Booking.MOBBKFlight;
using MOBBKGaugeChange = United.Mobile.Model.Shopping.Booking.MOBBKGaugeChange;
using MOBBKTrip = United.Mobile.Model.Shopping.Booking.MOBBKTrip;
using MOBSeatMap = United.Mobile.Model.Shopping.MOBSeatMap;
using ProfileResponse = United.Mobile.Model.Shopping.ProfileResponse;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Seat = United.Mobile.Model.Shopping.Misc.Seat;
using Session = United.Mobile.Model.Common.Session;
using United.Service.Presentation.CommonEnumModel;
using United.Mobile.Model.ManageRes;
using System.Web;
using United.Services.FlightShopping.Common;
using United.Mobile.Model;

namespace United.Common.HelperSeatEngine
{
    public class SeatEngine : ISeatEngine
    {
        private readonly ICacheLog<SeatEngine> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly ISeatMapService _seatMapService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ISeatMapEngine _seatMapEngine;
        private readonly IMerchandizingServices _merchandizingServices;
        private readonly IDPService _dPService;
        private readonly IPNRRetrievalService _pNRRetrievalService;
        private readonly IProductInfoHelper _productInfoHelper;
        private string pcuOfferAmountForthisCabin;
        private string seatMapLegendEntry1;
        private string seatMapLegendEntry2;
        private string cabinName = string.Empty;
        private string pcuOfferOptionId;
        private double pcuOfferPriceForthisCabin = 0;
        private readonly SeatEngineDynamoDB _seatEngineDynamoDB;
        //private readonly ISeatEnginePostService _seatEnginePostService;
        private readonly IComplimentaryUpgradeService _complimentaryUpgradeService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly ICachingService _cachingService;
        private readonly IHeaders _headers;
        private readonly ICabinBrandingService _cabinBrandingService;
        private readonly IFeatureSettings _featureSettings;

        public SeatEngine(ICacheLog<SeatEngine> logger, IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IShoppingSessionHelper shoppingSessionHelper
            , IShoppingUtility shoppingUtility
            , ISeatMapService seatMapService
            , IDynamoDBService dynamoDBService
            , IMerchandizingServices merchandizingServices
            //, ISeatEnginePostService seatEnginePostService
            , IDPService dPService
            , IPNRRetrievalService pNRRetrievalService
            , IProductInfoHelper productInfoHelper
            , IComplimentaryUpgradeService complimentaryUpgradeService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , ICachingService cachingService
            , IHeaders headers
            , ISeatMapEngine seatMapEngine
            , ICabinBrandingService cabinBrandingService
            , IFeatureSettings featureSettings
            )
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _shoppingSessionHelper = shoppingSessionHelper;
            _shoppingUtility = shoppingUtility;
            _seatMapService = seatMapService;
            _dynamoDBService = dynamoDBService;
            _merchandizingServices = merchandizingServices;
            _headers = headers;
            _seatMapEngine = seatMapEngine;
            seatMapLegendEntry1 = _configuration.GetValue<string>("seatMapLegendEntry1");
            seatMapLegendEntry2 = _configuration.GetValue<string>("seatMapLegendEntry2");
            _seatEngineDynamoDB = new SeatEngineDynamoDB(_configuration, _dynamoDBService);
            //_seatEnginePostService = seatEnginePostService;
            _dPService = dPService;
            _pNRRetrievalService = pNRRetrievalService;
            _productInfoHelper = productInfoHelper;
            _complimentaryUpgradeService = complimentaryUpgradeService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            ConfigUtility.UtilityInitialize(_configuration);
            _cachingService = cachingService;
            _cabinBrandingService = cabinBrandingService;
            _featureSettings = featureSettings;
        }


        private async Task<List<MOBSeatMap>> GetSeatMapWithFeesFromCSlResponse(FlightSeatMapDetail seatMapResponse, string bookingCabin, string sessionId, bool isQueryingFirstSegment, bool isSeatMapSupportedOa, int applicationId = -1, string transactionId = "", bool returnPolarisLegendforSeatMap = false, string appVersion = "")
        {
            List<MOBSeatMap> seatMap = new List<MOBSeatMap>();
            MOBSeatMap objMOBSeatMap = new MOBSeatMap();
            bool IsPolarisBranding = (_configuration.GetValue<string>("IsPolarisCabinBrandingON") != null) ? (_configuration.GetValue<bool>("IsPolarisCabinBrandingON")) : false;
            bool isPreferredZoneEnabled = _shoppingUtility.EnablePreferredZone(applicationId, appVersion); // Check Preferred Zone based on App version - Returns true or false // Kiran
            bool isPreferredZoneEnabledAndOlderVersion = !isPreferredZoneEnabled && _configuration.GetValue<bool>("isEnablePreferredZone");
            bool supressLMX = SupressLMX(applicationId);
            List<string> cabinBrandingDescriptions = new List<string>();
            string oASeatMapBannerMessage = string.Empty;
            var persistedReservation = await GetPersitedReservation(sessionId);
            bool disableEplusSeatsForBasicEconomy = _configuration.GetValue<bool>("EnableEPlusSeatsForBasicEconomy") ? false : (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation));
            if (persistedReservation != null && seatMapResponse != null && seatMapResponse.SeatMap != null && seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft != null && seatMapResponse.SeatMap.FlightInfo != null)
            {
                #region
                objMOBSeatMap = new MOBSeatMap();
                objMOBSeatMap.IsOaSeatMap = isSeatMapSupportedOa;
                int numberOfCabins = 0;

                if (seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Cabins.Count >= 1)
                {

                    numberOfCabins = seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Cabins.Count;
                    if (numberOfCabins > 3)
                    {
                        numberOfCabins = 3;
                    }
                    if (!returnPolarisLegendforSeatMap)
                    {
                        objMOBSeatMap.SeatLegendId = await GetSeatMapLegendId(seatMapResponse.SeatMap.FlightInfo.DepartureAirport, seatMapResponse.SeatMap.FlightInfo.ArrivalAirport, numberOfCabins);
                        //POLARIS Cabin Branding SeatMapLegend
                        #region polaris cabin

                        if (string.IsNullOrEmpty(objMOBSeatMap.SeatLegendId))
                        {
                            //POLARIS Cabin Branding SeatMapLegend Booking Path
                            objMOBSeatMap.SeatLegendId = "seatmap_legend5";
                        }
                        #endregion
                    }
                }
                objMOBSeatMap.SeatMapAvailabe = true;
                objMOBSeatMap.FleetType = seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Model != null ? (seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Model.Name != null ? seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Model.Name : "") : "";//Added the fleetname for Windows 8 Seat Map for Gavin and KO they are using the same as GIDS to show seat map seatMapResponse.SeatMapResponse;
                objMOBSeatMap.FlightNumber = Convert.ToInt32(seatMapResponse.SeatMap.SegmentSeatMap[0].SegmentInfo.FlightNumber);
                objMOBSeatMap.FlightDateTime = seatMapResponse.SeatMap.SegmentSeatMap[0].SegmentInfo.DepartureDate;
                if (objMOBSeatMap.Departure == null)
                {
                    objMOBSeatMap.Departure = new MOBAirport();
                }
                if (objMOBSeatMap.Arrival == null)
                {
                    objMOBSeatMap.Arrival = new MOBAirport();
                }
                objMOBSeatMap.Departure.Code = seatMapResponse.SeatMap.SegmentSeatMap[0].SegmentInfo.DepartureAirport;
                objMOBSeatMap.Arrival.Code = seatMapResponse.SeatMap.SegmentSeatMap[0].SegmentInfo.ArrivalAirport;

                objMOBSeatMap.LegId = "";
                int cabinCount = 0;
                var countNoOfFreeSeats = 0;
                int countNoOfPricedSeats = 0;
                foreach (Service.Presentation.CommonModel.AircraftModel.Cabin cabinInfo in seatMapResponse.SeatMap.SegmentSeatMap[0].Aircraft.Cabins)
                {
                    ++cabinCount;
                    MOBCabin tmpCabin = new MOBCabin
                    {
                        COS = objMOBSeatMap.IsOaSeatMap
                                    ? GetOaCabinCOS(cabinInfo) : cabinInfo.Name,
                        Configuration = cabinInfo.Layout
                    };
                    decimal lowestPrice = 0;
                    decimal highestPrice = 0;
                    string currency = string.Empty;

                    foreach (Service.Presentation.CommonModel.AircraftModel.SeatRow row in cabinInfo.SeatRows)
                    {
                        if (row.RowNumber < 1000)
                        {
                            #region
                            MOBRow tmpRow = new MOBRow();
                            tmpRow.Number = row.RowNumber.ToString();
                            if (!objMOBSeatMap.IsOaSeatMap)
                                tmpRow.Wing = CheckSeatRRowType(row.Characteristics, "IsWing");
                            int seatCol = 0;
                            foreach (Service.Presentation.CommonModel.AircraftModel.Seat seat in row.Seats)
                            {
                                #region
                                MOBSeatB tmpSeat = null;
                                string seatVal = tmpCabin.Configuration.Substring(seatCol, 1);
                                if (string.IsNullOrEmpty(seatVal.Trim()))
                                {
                                    tmpSeat = new MOBSeatB();
                                    tmpSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit");  //Code=IsExit
                                    tmpSeat.Number = "";
                                    tmpSeat.Fee = "";
                                    tmpSeat.LimitedRecline = false;
                                    tmpSeat.seatvalue = "-";
                                    tmpRow.Seats.Add(tmpSeat);
                                    seatCol++;
                                }
                                tmpSeat = new MOBSeatB();
                                tmpSeat.Number = seat.Identifier;
                                if (seat.Price != null)
                                {
                                    foreach (Service.Presentation.CommonModel.Charge feeInfo in seat.Price.Totals)
                                        tmpSeat.Fee = feeInfo.Amount.ToString();
                                }
                                else tmpSeat.Fee = "";
                                tmpSeat.LimitedRecline = false;
                                tmpSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit");  //Code=IsExit

                                if (!objMOBSeatMap.IsOaSeatMap)
                                {
                                    if (CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsBulkheadPrime") || (isPreferredZoneEnabled && IsPreferredSeatLimitedRecline(seat.Characteristics))) // As Kent replied email about Limited Reclined codes
                                    {
                                        tmpSeat.LimitedRecline = true;
                                    }
                                    string rearFacingSeat = string.Empty, frontFacingSeat = string.Empty, leftFacingSeat = string.Empty, rightFacingSeat = string.Empty;
                                    tmpSeat.Program = string.Empty;
                                    foreach (Service.Presentation.CommonModel.Characteristic characteristic in seat.Characteristics)
                                    {
                                        if (applicationId == 16)
                                        {
                                            Characteristic c = new Characteristic();
                                            c.Code = characteristic.Code;
                                            c.Value = characteristic.Value;
                                            if (tmpSeat.Characteristics == null)
                                            {
                                                tmpSeat.Characteristics = new List<Characteristic>();
                                            }
                                            tmpSeat.Characteristics.Add(c);
                                        }

                                        GetSeatPositionAccess(ref rearFacingSeat, ref frontFacingSeat, ref leftFacingSeat, ref rightFacingSeat, characteristic);
                                    }
                                    tmpSeat.Program = rearFacingSeat + "|" + frontFacingSeat + "|" + leftFacingSeat + "|" + rightFacingSeat;
                                    tmpSeat.Program = tmpSeat.Program.Trim('|');
                                }

                                bool disableSeats = true;
                                if ((bookingCabin.ToUpper().Equals("FIRST") || bookingCabin.Equals("United First") || bookingCabin.Equals("United Global First") || bookingCabin.ToUpper().Equals(seatMapLegendEntry1.Substring(1).ToUpper())) && (cabinInfo.Name.Equals("United First") || cabinInfo.Name.Equals("United Global First") || cabinInfo.Name.ToUpper().Equals(seatMapLegendEntry1.Substring(1).ToUpper()) || cabinInfo.Name.ToUpper().Equals("FIRST")))
                                {
                                    disableSeats = false;
                                }
                                else if ((bookingCabin.ToUpper().Equals("BUSINESS") || bookingCabin.Equals("BusinessFirst") || bookingCabin.Equals("United Business") || bookingCabin.Equals("United BusinessFirst") || bookingCabin.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper())) && (cabinInfo.Name.Equals("United Business") || cabinInfo.Name.Equals("BusinessFirst") || cabinInfo.Name.Equals("United BusinessFirst") || cabinInfo.Name.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper()) || cabinInfo.Name.ToUpper().Equals("BUSINESS")))
                                {
                                    disableSeats = false;
                                }
                                else if ((bookingCabin.ToUpper().Trim().Equals("COACH") || bookingCabin.Equals("Economy") || bookingCabin.Equals("United Economy") || cabinInfo.Name.Equals("United Premium Plus")) && (cabinInfo.Name.Equals("Economy") || cabinInfo.Name.Equals("United Economy") || objMOBSeatMap.IsOaSeatMap && cabinInfo.Name.Equals("Premium Economy")))
                                {
                                    disableSeats = false;
                                }
                                else if (_shoppingUtility.IsUPPSeatMapSupportedVersion(applicationId, appVersion) && bookingCabin.ToUpper().Trim().Equals("United Premium Plus".ToUpper().Trim()) && cabinInfo.Name.ToUpper().Trim().Equals("United PremiumPlus".ToUpper().Trim()))
                                {
                                    disableSeats = false;
                                    tmpSeat.DisplaySeatFeature = "United Premium Plus";
                                }
                                else if (_shoppingUtility.IsUPPSeatMapSupportedVersion(applicationId, appVersion)
                                            && bookingCabin.ToUpper().Trim().Equals((_configuration.GetValue<string>("PremiumEconomyCabinForOASeatMapEnableToggleText") ?? "").ToUpper().Trim())
                                            && cabinInfo.Name.ToUpper().Trim().Equals((_configuration.GetValue<string>("PremiumEconomyCabinForOASeatMapEnableToggleText") ?? "").ToUpper().Trim()))
                                {
                                    disableSeats = false;
                                }
                                else if (!string.IsNullOrEmpty(seatMapResponse.SeatMap.FlightInfo.OperatingCarrierCode) && !string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapForDeck")) && _configuration.GetValue<string>("SeatMapForDeck").Contains(seatMapResponse.SeatMap.FlightInfo.OperatingCarrierCode) && bookingCabin.Equals("Premium Economy") && cabinInfo.Name.Equals("Economy"))
                                {
                                    disableSeats = false;
                                }
                                bool IsOaPremiumEconomyCabin = cabinInfo.Name.Equals("Premium Economy");
                                bool isEplus = false;
                                if (isPreferredZoneEnabled)
                                {
                                    tmpSeat.seatvalue = objMOBSeatMap.IsOaSeatMap ?
                                                        GetOaSeatType(seat.Characteristics, disableSeats, IsOaPremiumEconomyCabin, out isEplus) :
                                                        GetSeatTypeWithPreferred(seat, disableSeats, applicationId, out isEplus, disableEplusSeatsForBasicEconomy);
                                }
                                else
                                {
                                    tmpSeat.seatvalue = objMOBSeatMap.IsOaSeatMap ?
                                                    GetOaSeatType(seat.Characteristics, disableSeats, IsOaPremiumEconomyCabin, out isEplus) :
                                                    getSeatType(seat.Characteristics, false, disableSeats, applicationId, out isEplus, (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation)));
                                }
                                tmpSeat.IsEPlus = isEplus;
                                #region
                                //if (!objMOBSeatMap.IsOaSeatMap && (cabinInfo.Name.Equals(UAWSSeatEngine.Cabins.Economy) || cabinInfo.Name.Equals("Economy") || cabinInfo.Name.Equals("United Economy")))
                                //{
                                //    if (seat.Characteristics != null && seat.Characteristics.Count > 0)
                                //    {
                                //        tmpSeat.IsEPlus = tmpSeat.IsEPlus || IsEPlusSeat(seat.Characteristics);
                                //        if (disableEplusSeatsForBasicEconomy)
                                //        {
                                //            if (tmpSeat.SeatValue != "X" && tmpSeat.IsEPlus && (persistedReservation.IsELF || Utility.IsIBE(persistedReservation)))
                                //            {
                                //                tmpSeat.SeatValue = "X";
                                //            }
                                //        }
                                //    }
                                //    if (((persistedReservation.IsELF || Utility.IsIBE(persistedReservation)) || persistedReservation.IsSSA && Utility.GetBooleanConfigValue("SSA_1B")) && !tmpSeat.IsEPlus)
                                //    {
                                //        tmpSeat.DisplaySeatFeature = "Economy";
                                //        if (isPreferredZoneEnabled)
                                //        {
                                //            tmpSeat.SeatFeatureList = new List<string>();
                                //            tmpSeat.SeatFeatureList.Add("Advance seat assignment");
                                //            if (tmpSeat.LimitedRecline)
                                //                tmpSeat.SeatFeatureList.Add("Limited recline");
                                //        }
                                //        else
                                //        {
                                //            tmpSeat.SeatFeatureInfo = "Advance seat assignment";
                                //        }
                                //    }
                                //}
                                #endregion
                                #region Get Services and Fees
                                ServicesAndFees tmpServicesAndFees = new ServicesAndFees();
                                if (tmpSeat.ServicesAndFees == null)
                                {
                                    tmpSeat.ServicesAndFees = new List<ServicesAndFees>();
                                }
                                if ((tmpServicesAndFees = GetServicesAndFees(seat)) != null)
                                {
                                    //Mobile-7429 DAA seat icon should not show Square icon for the Bussiness cabin
                                    if (!string.IsNullOrEmpty(_configuration.GetValue<string>("SeatmapSocialDistancingProgram")) && !string.IsNullOrEmpty(tmpSeat.Program))
                                    {
                                        string[] SeatmapSocialDistancingPrograms = _configuration.GetValue<string>("SeatmapSocialDistancingProgram").Split('|');
                                        foreach (string program in SeatmapSocialDistancingPrograms)
                                        {
                                            if (tmpServicesAndFees.Program.Equals(program))
                                            {
                                                tmpServicesAndFees.Program = tmpSeat.Program;
                                                break;
                                            }
                                        }
                                    }
                                    tmpSeat.ServicesAndFees.Add(tmpServicesAndFees);

                                    if (tmpSeat.ServicesAndFees.Count > 0)
                                    {
                                        tmpSeat.IsEPlus = tmpSeat.IsEPlus || CheckEPlusSeatCode(tmpSeat.ServicesAndFees[0].Program);
                                        if (disableEplusSeatsForBasicEconomy)
                                        {
                                            if (tmpSeat.seatvalue != "X" && tmpSeat.IsEPlus && (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation)))
                                            {
                                                tmpSeat.seatvalue = "X";
                                            }
                                        }
                                        if (tmpSeat.IsEPlus)
                                        {
                                            tmpSeat.DisplaySeatFeature = "Economy Plus";
                                            if (isPreferredZoneEnabled)
                                            {
                                                tmpSeat.SeatFeatureList = new List<string>();
                                                tmpSeat.SeatFeatureList.Add("Extra Legroom");
                                                if (tmpSeat.LimitedRecline)
                                                    tmpSeat.SeatFeatureList.Add("Limited recline");
                                                if (!supressLMX)
                                                    tmpSeat.SeatFeatureList.Add("Eligible for PQD");
                                            }
                                            else
                                            {
                                                tmpSeat.SeatFeatureInfo = "";
                                            }
                                        }
                                        if (isPreferredZoneEnabled && tmpSeat.seatvalue == "PZ" ||
                                            isPreferredZoneEnabledAndOlderVersion && tmpSeat.seatvalue == "O" && IsPreferredSeat(seat))
                                        {
                                            tmpSeat.DisplaySeatFeature = "Preferred Seat";
                                            tmpSeat.SeatFeatureList = new List<string>();
                                            tmpSeat.SeatFeatureList.Add("Standard legroom");
                                            tmpSeat.SeatFeatureList.Add("Favorable location in Economy");
                                            if (tmpSeat.LimitedRecline)
                                            {
                                                tmpSeat.SeatFeatureList.Add("Limited recline");
                                            }
                                        }
                                    }

                                    if (string.IsNullOrEmpty(currency))
                                    {
                                        currency = tmpServicesAndFees.Currency;
                                    }
                                    ///225816 - PROD mApps: Remove white dot for Economy Plus recommended seat from interactive seatmap
                                    ///Srini - 01/08/2018
                                    if (!_configuration.GetValue<bool>("BugFixToggleFor17M"))
                                    {
                                        if (lowestPrice == 0 && tmpServicesAndFees.TotalFee != 0)
                                        {
                                            lowestPrice = tmpServicesAndFees.TotalFee;
                                            highestPrice = tmpServicesAndFees.TotalFee;
                                        }

                                        if (lowestPrice > tmpServicesAndFees.TotalFee)
                                        {
                                            lowestPrice = tmpServicesAndFees.TotalFee;
                                        }

                                        if (highestPrice < tmpServicesAndFees.TotalFee)
                                        {
                                            highestPrice = tmpServicesAndFees.TotalFee;
                                        }
                                    }
                                }
                                #endregion

                                CountNoOfFreeSeatsAndPricedSeats(tmpSeat, ref countNoOfFreeSeats, ref countNoOfPricedSeats);

                                tmpRow.Seats.Add(tmpSeat);

                                seatCol++;
                                #endregion
                            }
                            if (tmpRow.Seats == null || tmpRow.Seats.Count != cabinInfo.Layout.Length)
                            {
                                if (objMOBSeatMap.IsOaSeatMap)
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                                throw new MOBUnitedException("Unable to get the seat map for the flight you requested.");
                            }
                            tmpCabin.Rows.Add(tmpRow);
                            #endregion
                        }
                        else if (row.RowNumber >= 1000 && applicationId == 16)
                        {
                            #region
                            MOBRow tmpRow = new MOBRow();
                            tmpRow.Number = row.RowNumber.ToString();
                            if (!objMOBSeatMap.IsOaSeatMap)
                                tmpRow.Wing = CheckSeatRRowType(row.Characteristics, "IsWing");
                            int seatCol = 0;
                            foreach (Service.Presentation.CommonModel.AircraftModel.Seat seat in row.Seats)
                            {
                                #region
                                MOBSeatB tmpSeat = null;
                                string seatVal = tmpCabin.Configuration.Substring(seatCol, 1);
                                if (string.IsNullOrEmpty(seatVal.Trim()))
                                {
                                    tmpSeat = new MOBSeatB();
                                    tmpSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit");  //Code=IsExit
                                    tmpSeat.Number = "";
                                    tmpSeat.Fee = "";
                                    tmpSeat.LimitedRecline = false;
                                    tmpSeat.seatvalue = "-";
                                    tmpRow.Seats.Add(tmpSeat);
                                    seatCol++;
                                }
                                tmpSeat = new MOBSeatB();
                                tmpSeat.Number = seat.Identifier;
                                if (seat.Price != null)
                                {
                                    foreach (Service.Presentation.CommonModel.Charge feeInfo in seat.Price.Totals)
                                        tmpSeat.Fee = feeInfo.Amount.ToString();
                                }
                                else tmpSeat.Fee = "";
                                tmpSeat.LimitedRecline = false;
                                tmpSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit"); //Code=IsExit

                                if (!objMOBSeatMap.IsOaSeatMap)
                                {
                                    if (CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsBulkheadPrime") || (isPreferredZoneEnabled && IsPreferredSeatLimitedRecline(seat.Characteristics))) // As Kent replied email about Limited Reclined codes
                                    {
                                        tmpSeat.LimitedRecline = true;
                                    }

                                    string rearFacingSeat = string.Empty, frontFacingSeat = string.Empty, leftFacingSeat = string.Empty, rightFacingSeat = string.Empty;
                                    tmpSeat.Program = string.Empty;
                                    foreach (Service.Presentation.CommonModel.Characteristic characteristic in seat.Characteristics)
                                    {
                                        if (applicationId == 16)
                                        {
                                            Characteristic c = new Characteristic();
                                            c.Code = characteristic.Code;
                                            c.Value = characteristic.Value;
                                            if (tmpSeat.Characteristics == null)
                                            {
                                                tmpSeat.Characteristics = new List<Characteristic>();
                                            }
                                            tmpSeat.Characteristics.Add(c);
                                        }

                                        rearFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsRearFacing".ToUpper().Trim() ? "FBB" : rearFacingSeat);
                                        frontFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsFrontFacing".ToUpper().Trim() ? "FBF" : frontFacingSeat);
                                        leftFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsLeftFacing".ToUpper().Trim() ? "FBL" : leftFacingSeat);
                                        rightFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsRightFacing".ToUpper().Trim() ? "FBR" : rightFacingSeat);
                                    }
                                    tmpSeat.Program = rearFacingSeat + "|" + frontFacingSeat + "|" + leftFacingSeat + "|" + rightFacingSeat;
                                    tmpSeat.Program = tmpSeat.Program.Trim('|');
                                }

                                bool disableSeats = true;
                                if ((bookingCabin.ToUpper().Equals("FIRST") || bookingCabin.Equals("First") || bookingCabin.Equals("United First") || bookingCabin.Equals("United Global First") || bookingCabin.ToUpper().Equals(seatMapLegendEntry1.Substring(1).ToUpper())) && (cabinInfo.Name.Equals("United First") || cabinInfo.Name.Equals("United Global First") || cabinInfo.Name.ToUpper().Equals(seatMapLegendEntry1.Substring(1).ToUpper()) || cabinInfo.Name.ToUpper().Equals("FIRST")))
                                {
                                    disableSeats = false;
                                }
                                else if ((bookingCabin.ToUpper().Equals("BUSINESS") || bookingCabin.Equals("BusinessFirst") || bookingCabin.Equals("United Business") || bookingCabin.Equals("United BusinessFirst") || bookingCabin.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper())) && (cabinInfo.Name.Equals("United Business") || cabinInfo.Name.Equals("BusinessFirst") || cabinInfo.Name.Equals("United BusinessFirst") || cabinInfo.Name.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper()) || cabinInfo.Name.ToUpper().Equals("BUSINESS")))
                                {
                                    disableSeats = false;
                                }
                                else if ((bookingCabin.ToUpper().Trim().Equals("COACH") || bookingCabin.Equals("Economy") || bookingCabin.Equals("United Economy")) && (cabinInfo.Name.Equals("Economy") || cabinInfo.Name.Equals("United Economy") || objMOBSeatMap.IsOaSeatMap && cabinInfo.Name.Equals("Premium Economy")))
                                {
                                    disableSeats = false;
                                }
                                else if (!string.IsNullOrEmpty(seatMapResponse.SeatMap.FlightInfo.OperatingCarrierCode) && !string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapForDeck")) && _configuration.GetValue<string>("SeatMapForDeck").Contains(seatMapResponse.SeatMap.FlightInfo.OperatingCarrierCode) && bookingCabin.Equals("Premium Economy") && cabinInfo.Name.Equals("Economy"))
                                {
                                    disableSeats = false;
                                }
                                bool IsOaPremiumEconomyCabin = cabinInfo.Name.Equals("Premium Economy");
                                bool isEplus = false;
                                if (isPreferredZoneEnabled)
                                {
                                    tmpSeat.seatvalue = objMOBSeatMap.IsOaSeatMap ?
                                                        GetOaSeatType(seat.Characteristics, disableSeats, IsOaPremiumEconomyCabin, out isEplus) :
                                                        GetSeatTypeWithPreferred(seat, disableSeats, applicationId, out isEplus, disableEplusSeatsForBasicEconomy);
                                }
                                else
                                {
                                    tmpSeat.seatvalue = objMOBSeatMap.IsOaSeatMap ?
                                                        GetOaSeatType(seat.Characteristics, disableSeats, IsOaPremiumEconomyCabin, out isEplus) :
                                                        getSeatType(seat.Characteristics, false, disableSeats, applicationId, out isEplus, (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation)));
                                }
                                tmpSeat.IsEPlus = isEplus;

                                #region Get Services and Fees
                                ServicesAndFees tmpServicesAndFees = new ServicesAndFees();
                                if (tmpSeat.ServicesAndFees == null)
                                {
                                    tmpSeat.ServicesAndFees = new List<ServicesAndFees>();
                                }
                                if ((tmpServicesAndFees = GetServicesAndFees(seat)) != null)
                                {
                                    tmpSeat.ServicesAndFees.Add(tmpServicesAndFees);

                                    if (tmpSeat.ServicesAndFees.Count > 0)
                                    {
                                        tmpSeat.IsEPlus = tmpSeat.IsEPlus || CheckEPlusSeatCode(tmpSeat.ServicesAndFees[0].Program);
                                        if (disableEplusSeatsForBasicEconomy)
                                        {
                                            if (tmpSeat.seatvalue != "X" && tmpSeat.IsEPlus && (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation)))
                                            {
                                                tmpSeat.seatvalue = "X";
                                            }
                                        }
                                        if (tmpSeat.IsEPlus)
                                        {
                                            tmpSeat.DisplaySeatFeature = "Economy Plus";
                                            if (isPreferredZoneEnabled)
                                            {
                                                tmpSeat.SeatFeatureList = new List<string>();
                                                tmpSeat.SeatFeatureList.Add("Extra Legroom");
                                                if (tmpSeat.LimitedRecline)
                                                    tmpSeat.SeatFeatureList.Add("Limited recline");
                                                if (!supressLMX)
                                                    tmpSeat.SeatFeatureList.Add("Eligible for PQD");
                                            }
                                            else
                                            {
                                                tmpSeat.SeatFeatureInfo = "";
                                            }
                                        }
                                        if (isPreferredZoneEnabled && tmpSeat.seatvalue == "PZ" ||
                                            isPreferredZoneEnabledAndOlderVersion && tmpSeat.seatvalue == "O" && IsPreferredSeat(seat))
                                        {
                                            tmpSeat.DisplaySeatFeature = "Preferred Seat";
                                            tmpSeat.SeatFeatureList = new List<string>();
                                            tmpSeat.SeatFeatureList.Add("Standard legroom");
                                            tmpSeat.SeatFeatureList.Add("Favorable location in Economy");
                                            if (tmpSeat.LimitedRecline)
                                            {
                                                tmpSeat.SeatFeatureList.Add("Limited recline");
                                            }
                                        }
                                    }

                                    if (string.IsNullOrEmpty(currency))
                                    {
                                        currency = tmpServicesAndFees.Currency;
                                    }

                                    ///225816 - PROD mApps: Remove white dot for Economy Plus recommended seat from interactive seatmap
                                    ///Srini - 01/08/2018
                                    if (!_configuration.GetValue<bool>("BugFixToggleFor17M"))
                                    {
                                        if (lowestPrice == 0 && tmpServicesAndFees.TotalFee != 0)
                                        {
                                            lowestPrice = tmpServicesAndFees.TotalFee;
                                            highestPrice = tmpServicesAndFees.TotalFee;
                                        }

                                        if (lowestPrice > tmpServicesAndFees.TotalFee)
                                        {
                                            lowestPrice = tmpServicesAndFees.TotalFee;
                                        }

                                        if (highestPrice < tmpServicesAndFees.TotalFee)
                                        {
                                            highestPrice = tmpServicesAndFees.TotalFee;
                                        }
                                    }
                                }
                                #endregion

                                CountNoOfFreeSeatsAndPricedSeats(tmpSeat, ref countNoOfFreeSeats, ref countNoOfPricedSeats);

                                tmpRow.Seats.Add(tmpSeat);

                                seatCol++;
                                #endregion
                            }
                            if (tmpRow.Seats == null || tmpRow.Seats.Count != cabinInfo.Layout.Length)
                            {
                                if (objMOBSeatMap.IsOaSeatMap)
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                                throw new MOBUnitedException("Unable to get the seat map for the flight you requested.");
                            }
                            tmpCabin.Rows.Add(tmpRow);
                            #endregion
                        }
                    }

                    ///225816 - PROD mApps: Remove white dot for Economy Plus recommended seat from interactive seatmap
                    ///Srini - 01/08/2018
                    if (!_configuration.GetValue<bool>("BugFixToggleFor17M"))
                    {
                        if (lowestPrice > 0)
                        {
                            foreach (MOBRow seatRow in tmpCabin.Rows)
                            {
                                foreach (MOBSeatB seat in seatRow.Seats)
                                {
                                    if (seat.ServicesAndFees != null && seat.ServicesAndFees.Count > 0)
                                    {
                                        if (seat.ServicesAndFees[0].TotalFee == lowestPrice)
                                        {
                                            seat.IsLowestEPlus = true;
                                        }
                                        if (seat.ServicesAndFees[0].TotalFee == highestPrice)
                                        {
                                            seat.IsHighestEPlus = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (highestPrice > 0)
                        {
                            string currencySymbol = string.Empty;
                            if (currency == "USD")
                            {
                                currencySymbol = "$";
                            }
                            objMOBSeatMap.EplusPromotionMessage = string.Format(_configuration.GetValue<string>("EPlusPromotionMessage"), currencySymbol + highestPrice);
                        }
                    }
                    tmpCabin.Configuration = tmpCabin.Configuration.Replace(" ", "-");

                    if (!objMOBSeatMap.IsOaSeatMap && cabinCount == 4)
                    {
                        objMOBSeatMap.Cabins.Insert(2, tmpCabin);
                        tmpCabin.COS = "Upper Deck " + tmpCabin.COS;
                    }
                    else
                    {
                        objMOBSeatMap.Cabins.Add(tmpCabin);
                    }
                }
                //should do a version check and change. future version check..... sandeep interline. throw new MOBUnitedException(GetDocumentTextFromDataBase("AC_SEATMAP_Exception_TEXT"));
                //   bool isSupportedVersion = UtilityNew.isApplicationVersionGreater2(request.Application.Id, request.Application.Version.Major, "AndroidOAFlifoMapVersion", "iPhoneOAFlifoMapVersion", "", "");


                if (objMOBSeatMap.IsOaSeatMap && countNoOfFreeSeats == 0)
                {
                    //sandeep
                    //UtilityNew.isApplicationVersionGreaterorEqual "AndroidOaSeatMapExceptionVersion", "iPhoneOaSeatMapExceptionVersion",
                    bool oaExceptionVersionCheck = _shoppingUtility.OaSeatMapExceptionVersion(applicationId, appVersion);

                    if (oaExceptionVersionCheck)
                    {
                        throw new MOBUnitedException(await GetDocumentTextFromDataBase("OA_SEATMAP_Exception_TEXT"));
                    }
                    else
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                    }
                }
                if (_configuration.GetValue<bool>("checkForPAXCount"))
                {
                    string readOnlySeatMapinBookingPathOAAirlines = string.Empty;
                    readOnlySeatMapinBookingPathOAAirlines = _configuration.GetValue<string>("ReadonlySeatMapinBookingPathOAAirlines");


                    if (objMOBSeatMap.IsOaSeatMap && !seatMapResponse.SeatMap.FlightInfo.CarrierCode.Equals("SQ"))
                    {
                        if ((countNoOfFreeSeats <= persistedReservation.NumberOfTravelers))
                        {
                            bool oaExceptionVersionCheck = _shoppingUtility.OaSeatMapExceptionVersion(applicationId, appVersion);

                            if (oaExceptionVersionCheck)
                            {
                                throw new MOBUnitedException(await GetDocumentTextFromDataBase("OA_SEATMAP_Exception_TEXT"));
                            }
                            else
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                            }
                        }
                        else if (!string.IsNullOrEmpty(readOnlySeatMapinBookingPathOAAirlines))
                        {
                            string[] readOnlySeatMapAirlines = { };
                            readOnlySeatMapAirlines = readOnlySeatMapinBookingPathOAAirlines.Split(',');
                            foreach (string airline in readOnlySeatMapAirlines)
                            {
                                if (seatMapResponse.SeatMap.FlightInfo.CarrierCode.ToUpper().Equals(airline.Trim().ToUpper()))
                                {
                                    objMOBSeatMap.IsReadOnlySeatMap = true;
                                    objMOBSeatMap.oASeatMapBannerMessage = _configuration.GetValue<string>("OASeatMapBannerMessage") != null ? _configuration.GetValue<string>("OASeatMapBannerMessage").Trim() : string.Empty;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (objMOBSeatMap.IsOaSeatMap)
                {
                    objMOBSeatMap.SeatLegendId = _configuration.GetValue<string>("SeatMapLegendForOtherAirlines");
                }
                else
                {
                    #region Consuming CabinBranding Service

                    //POLARIS Cabin Branding SeatMap FlightStatus
                    if (IsPolarisBranding)
                    {
                        #region

                        //Generating Token
                        string flifoAuthenticationToken = string.Empty;
                        flifoAuthenticationToken = await _dPService.GetAnonymousToken(applicationId, transactionId, _configuration);

                        //DateTime departureDateTime = DateTime.ParseExact(Convert.ToDateTime(segment.DepartDate).ToShortDateString() + " " + segment.DepartTime, "M/d/yyyy h:mtt", CultureInfo.InvariantCulture);
                        string fDate =
                            DateTime.ParseExact(
                                Convert.ToDateTime(seatMapResponse.SeatMap.FlightInfo.DepartureDate).ToShortDateString(),
                                "M/d/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        //("yyyy-MM-dd");
                        //flightDate.ToString("yyyy-MM-dd");
                        cabinBrandingDescriptions = await GetPolarisCabinBranding(flifoAuthenticationToken,
                            seatMapResponse.SeatMap.FlightInfo.FlightNumber,
                            seatMapResponse.SeatMap.FlightInfo.DepartureAirport, fDate,
                            seatMapResponse.SeatMap.FlightInfo.ArrivalAirport, numberOfCabins.ToString(), "en-US",
                            sessionId, "UA", "UA");

                        if (cabinBrandingDescriptions != null && cabinBrandingDescriptions.Count > 0)
                        {
                            //objMOBSeatMap.SeatLegendId = Utility.GetPolarisCabinBrandingSeatMapLegendId(cabinBrandingDescriptions);
                            if (returnPolarisLegendforSeatMap)
                            {
                                //Adding version changes for Bug 95616 - Ranjit
                                objMOBSeatMap.SeatLegendId =
                                    await GetPolarisSeatMapLegendId(
                                        seatMapResponse.SeatMap.FlightInfo.DepartureAirport,
                                        seatMapResponse.SeatMap.FlightInfo.ArrivalAirport, numberOfCabins,
                                        cabinBrandingDescriptions, applicationId, appVersion);
                            }
                            foreach (MOBCabin mc in objMOBSeatMap.Cabins)
                            {

                                if (objMOBSeatMap.Cabins.Count > 3 && mc.COS.ToUpper().Contains("UPPER DECK"))
                                {
                                    objMOBSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                    objMOBSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                                    objMOBSeatMap.Cabins[2].COS = "Upper Deck " +
                                                                  cabinBrandingDescriptions[1].ToString();
                                    objMOBSeatMap.Cabins[3].COS = cabinBrandingDescriptions[2].ToString();

                                }
                                else if (objMOBSeatMap.Cabins.Count == 3)
                                {
                                    objMOBSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                    objMOBSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                                    objMOBSeatMap.Cabins[2].COS = cabinBrandingDescriptions[2].ToString();
                                }
                                else if (objMOBSeatMap.Cabins.Count == 2)
                                {
                                    objMOBSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                    objMOBSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                                }
                                else
                                {
                                    objMOBSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                }
                            }
                        }
                        else
                        {
                            throw new MOBUnitedException("Cabin Branding Service returned null");
                        }

                        #endregion
                    }
                    else if (returnPolarisLegendforSeatMap)
                    {
                        objMOBSeatMap.SeatLegendId =
                           await GetPolarisSeatMapLegendIdPolarisOff(
                                seatMapResponse.SeatMap.FlightInfo.DepartureAirport,
                                seatMapResponse.SeatMap.FlightInfo.ArrivalAirport, numberOfCabins);
                    }

                    #endregion Consuming CabinBranding Service
                }
                bool isInCheckInWindow = IsInCheckInWindow(seatMapResponse);

                if (_configuration.GetValue<bool>("EnableSocialDistanceMessagingForSeatMap") && isQueryingFirstSegment)
                {
                    objMOBSeatMap.ShowInfoMessageOnSeatMap = _configuration.GetValue<string>("SocialDistanceSeatDisplayMessageDetailBody") + _configuration.GetValue<string>("SocialDistanceSeatMapMessagePopup");
                }
                else
                {
                    objMOBSeatMap.ShowInfoMessageOnSeatMap = objMOBSeatMap.IsOaSeatMap ?
                                                         await ShowOaSeatMapAvailabilityDisclaimerText() :
                                                         await ShowNoFreeSeatsAvailableMessage(persistedReservation, countNoOfFreeSeats, countNoOfPricedSeats, isInCheckInWindow, isQueryingFirstSegment);
                    if (_configuration.GetValue<bool>("DisableFreeSeatMessageChanges") == false && objMOBSeatMap.IsOaSeatMap == false
                         && string.IsNullOrEmpty(objMOBSeatMap.ShowInfoMessageOnSeatMap) == false)
                        objMOBSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("NoFreeSeatAvailableMessageHeader");
                }
                EconomySeatsForBUSService(objMOBSeatMap, true);
                seatMap.Add(objMOBSeatMap);
                #endregion
            }
            else
            {
                string errorMessage = isSeatMapSupportedOa
                                        ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines")
                                        : "Unable to get the seat map for the flight you requested.";
                throw new MOBUnitedException(errorMessage);
            }

            return seatMap;
        }

        private async Task<Reservation> GetPersitedReservation(string sessionId)
        {
            try
            {
                var persistedReservation = await _sessionHelperService.GetSession<Reservation>(sessionId, new Reservation().ObjectName, new List<string> { sessionId, new Reservation().ObjectName }).ConfigureAwait(false);
                return persistedReservation;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetOaCabinCOS(Service.Presentation.CommonModel.AircraftModel.Cabin cabinInfo)
        {
            bool isUpperDeck = false;
            bool.TryParse(cabinInfo.IsUpperDeck, out isUpperDeck);
            return isUpperDeck ?
                    "Upper Deck " + cabinInfo.Name :
                    cabinInfo.Name;
        }

        private bool CheckSeatRRowType(Collection<Service.Presentation.CommonModel.Characteristic> seatCharacteristics, string code)
        {
            bool seatRRowType = false;
            if (seatCharacteristics != null && seatCharacteristics.Count > 0)
            {
                foreach (Service.Presentation.CommonModel.Characteristic objChar in seatCharacteristics)
                {
                    if (objChar.Code != null && objChar.Code.ToUpper().Trim() == code.ToUpper().Trim())
                    {
                        seatRRowType = true;
                        break;
                    }
                }
            }
            return seatRRowType;
        }

        private static void GetSeatPositionAccess(ref string rearFacingSeat, ref string frontFacingSeat, ref string leftFacingSeat, ref string rightFacingSeat, Service.Presentation.CommonModel.Characteristic characteristic)
        {
            //RearFacing Non-DAA:
            if (characteristic.Code.ToUpper().Trim() == "IsRearFacing".ToUpper().Trim())
            {
                rearFacingSeat = "FBB";
            }

            //FrontFAcing Non DAA FBF ; DAA DAFL|DAFR
            if (characteristic.Code.ToUpper().Trim() == "IsFrontFacing".ToUpper().Trim())
            {
                frontFacingSeat = "FBF";
            }
            else if (characteristic.Code.ToUpper().Trim() == "IsDAAFrontFacingLeftAccess".ToUpper().Trim())
            {
                frontFacingSeat = "DAFL";
            }
            else if (characteristic.Code.ToUpper().Trim() == "IsDAAFrontFacingRightAccess".ToUpper().Trim())
            {
                frontFacingSeat = "DAFR";
            }

            //LeftFacing Non-DAA: FBL; DAA:DAL
            if (characteristic.Code.ToUpper().Trim() == "IsLeftFacing".ToUpper().Trim())
            {
                leftFacingSeat = "FBL";
            }
            else if (characteristic.Code.ToUpper().Trim() == "IsDAALeftAngle".ToUpper().Trim())
            {
                leftFacingSeat = "DAL";
            }

            //Right Facing Non-DAA: FBR; DAA: DAR
            if (characteristic.Code.ToUpper().Trim() == "IsRightFacing".ToUpper().Trim())
            {
                rightFacingSeat = "FBR";
            }
            else if (characteristic.Code.ToUpper().Trim() == "IsDAARightAngle".ToUpper().Trim())
            {
                rightFacingSeat = "DAR";
            }
            else if (characteristic.Code.ToUpper().Trim() == "IsDAAFrontFacingRightAccessMiddle".ToUpper().Trim())
            {
                rightFacingSeat = "DAFRM";
            }

            //rearFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsRearFacing".ToUpper().Trim() ? "FBB" : rearFacingSeat);
            //frontFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsFrontFacing".ToUpper().Trim() ? "FBF" : frontFacingSeat);
            //leftFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsLeftFacing".ToUpper().Trim() ? "FBL" : leftFacingSeat);
            //rightFacingSeat = (characteristic.Code.ToUpper().Trim() == "IsRightFacing".ToUpper().Trim() ? "FBR" : rightFacingSeat);
        }

        private string GetOaSeatType(Collection<Service.Presentation.CommonModel.Characteristic> seatCharacteristics, bool disableSeats, bool isOaPremiumEconomyCabin, out bool isEplus)
        {
            isEplus = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (CheckOaSeatRRowType(seatCharacteristics, "IsLavatory") || CheckOaSeatRRowType(seatCharacteristics, "IsCloset") || CheckOaSeatRRowType(seatCharacteristics, "IsGalley") || CheckOaSeatRRowType(seatCharacteristics, "IsStairs") || CheckOaSeatRRowType(seatCharacteristics, "IsSpace") || CheckOaSeatRRowType(seatCharacteristics, "IsInoperativeSeat"))
            {
                sb.Append("-");
            }
            else if (CheckOaSeatRRowType(seatCharacteristics, "IsUnusableSeat") || CheckOaSeatRRowType(seatCharacteristics, "IsPermBlockedSeat"))
            {
                sb.Append("X");
            }
            else if (CheckOaSeatRRowType(seatCharacteristics, "IsAvailableSeat"))
            {
                sb.Append(isOaPremiumEconomyCabin ? "P" : "O");
            }
            else
            {
                sb.Append("X");
            }

            string aType = sb.ToString();
            if (aType != "-" && disableSeats)
            {
                sb = new System.Text.StringBuilder();
                sb.Append("X");
            }

            if (sb.ToString() != "-")
            {
                isEplus = isOaPremiumEconomyCabin;
            }
            return sb.ToString();
        }

        private string GetSeatTypeWithPreferred(Service.Presentation.CommonModel.AircraftModel.Seat seat, bool disableSeats, int applicationId, out bool isEplus, bool disableEplus = false)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            isEplus = false;

            //if (seatType.Lavatory || seatType.Closet || seatType.Galley  || seatType.Stairs  || seatType.Unusable || seatType.Inoperative|| seatType.Space)
            if (CheckSeatRRowType(seat.Characteristics, "IsLavatory") || CheckSeatRRowType(seat.Characteristics, "IsCloset") || CheckSeatRRowType(seat.Characteristics, "IsGalley") || CheckSeatRRowType(seat.Characteristics, "IsStairs") || CheckSeatRRowType(seat.Characteristics, "IsSpace"))
            {
                sb.Append("-");
            }
            //Bug-87823- Added below condition to show the "IsUnusableSeat" seat as blocked in UI- 01/04/2017 -Vijayan
            else if (CheckSeatRRowType(seat.Characteristics, "IsUnusableSeat"))
            {
                sb.Append("X");
            }
            else if (CheckSeatRRowType(seat.Characteristics, "IsInoperativeSeat"))
            {
                if (applicationId == 16)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append("-");
                }
            }
            else if (CheckSeatRRowType(seat.Characteristics, "IsPermBlockedSeat"))
            {
                sb.Append("X");
            }
            else if (CheckSeatRRowType(seat.Characteristics, "IsBulkheadPrime") ||
                      CheckSeatRRowType(seat.Characteristics, "IsBulkheadPrimePlus") ||
                      CheckSeatRRowType(seat.Characteristics, "IsPrimeSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsPrimePlusSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsPrimeLegZoneSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsPrimePlusLegRoomSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusBulkheadPrimeSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusBulkheadPrimePlusSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusExitPrimeSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusExitPrimePlusSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusPrimeSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusPrimePlusSeat"))
            {
                isEplus = true;
                if (!CheckSeatRRowType(seat.Characteristics, "IsAvailableSeat") || disableEplus)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append("P");
                }
            }
            else if (IsPreferredSeat(seat))
            {
                if (CheckSeatRRowType(seat.Characteristics, "IsAvailableSeat") || CheckSeatRRowType(seat.Characteristics, "IsPermBlockedSeat") &&
                      CheckSeatRRowType(seat.Characteristics, "IsAvailableSeat"))
                {
                    sb.Append("PZ");
                }
                else
                {
                    sb.Append("X");
                }
            }
            else if (CheckSeatRRowType(seat.Characteristics, "IsAvailableSeat") ||
                      CheckSeatRRowType(seat.Characteristics, "IsPermBlockedSeat") &&
                      CheckSeatRRowType(seat.Characteristics, "IsAvailableSeat")
                    )
            {
                sb.Append("O");
            }
            else
            {
                sb.Append("X");
            }

            string aType = sb.ToString();
            //if (disableSeats) QC defect 1618 
            if (aType != "-" && disableSeats)
            {
                if (string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin))
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append("X");
                }
            }

            return sb.ToString();
        }

        private bool CheckOaSeatRRowType(Collection<Service.Presentation.CommonModel.Characteristic> seatCharacteristics, string code)
        {
            bool seatRRowType = false;
            if (seatCharacteristics != null && seatCharacteristics.Count > 0)
            {
                foreach (Service.Presentation.CommonModel.Characteristic objChar in seatCharacteristics)
                {
                    if (objChar.Code != null && objChar.Code.ToUpper().Trim() == code.ToUpper().Trim())
                    {
                        Boolean.TryParse(objChar.Value, out seatRRowType);
                        break;
                    }
                }
            }
            return seatRRowType;
        }

        private bool IsPreferredSeat(Service.Presentation.CommonModel.AircraftModel.Seat seat)
        {
            return seat.Price != null && !string.IsNullOrEmpty(seat.Price.PromotionCode) && IsPreferredSeat(seat.Characteristics, seat.Price.PromotionCode);
        }
        private bool IsPreferredSeat(Collection<Service.Presentation.CommonModel.Characteristic> characteristics, string program)
        {
            return IsPreferredSeatProgramCode(program) && IsPreferredSeatCharacteristics(characteristics);
        }
        public bool IsPreferredSeatProgramCode(string program)
        {
            string preferredProgramCodes = _configuration.GetValue<string>("PreferredSeatProgramCodes");

            if (!string.IsNullOrEmpty(preferredProgramCodes) && !string.IsNullOrEmpty(program))
            {
                string[] codes = preferredProgramCodes.Split('|');

                foreach (string code in codes)
                {
                    if (code.Equals(program))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsPreferredSeatCharacteristics(Collection<Service.Presentation.CommonModel.Characteristic> characteristics)
        {
            if (characteristics != null && characteristics.Count > 0)
            {
                string preferredSeatChar = _configuration.GetValue<string>("PreferredSeatBooleanCharacteristic") ?? string.Empty;
                var preferredSeatCharList = preferredSeatChar.Split('|');
                foreach (Service.Presentation.CommonModel.Characteristic characteristic in characteristics)
                {
                    if (characteristic != null && !string.IsNullOrEmpty(characteristic.Code) && !string.IsNullOrEmpty(characteristic.Value))
                    {
                        if (preferredSeatCharList.Any(s => s.Trim().Equals(characteristic.Code, StringComparison.OrdinalIgnoreCase)) && characteristic.Value.ToUpper().Trim() == "TRUE" ||
                            (characteristic.Code.Equals("DisplaySeatType") && IsPreferredSeatDisplayType(characteristic.Value)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsPreferredSeatDisplayType(string displaySeatType)
        {
            string seatTypes = _configuration.GetValue<string>("PreferredSeatSharesSeatTypes") ?? string.Empty;
            var seatTypesList = seatTypes.Split('|');
            if (!string.IsNullOrEmpty(seatTypes) && !string.IsNullOrEmpty(displaySeatType))
            {
                if (seatTypesList.Any(s => s.Trim().Equals(displaySeatType, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPreferredSeatLimitedRecline(Collection<Service.Presentation.CommonModel.Characteristic> characteristics)
        {
            if (characteristics != null && characteristics.Count > 0)
            {
                string seatTypes = _configuration.GetValue<string>("PreferredSeatLimitedRecline") ?? string.Empty;
                var seatTypesList = seatTypes.Split('|');
                foreach (Service.Presentation.CommonModel.Characteristic characteristic in characteristics)
                {
                    if (characteristic != null && !string.IsNullOrEmpty(characteristic.Code) && !string.IsNullOrEmpty(characteristic.Value))
                    {
                        if ((characteristic.Code.Equals("DisplaySeatType") && seatTypesList.Any(s => s.Trim().Equals(characteristic.Value, StringComparison.OrdinalIgnoreCase))))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private string getSeatType(Collection<Service.Presentation.CommonModel.Characteristic> seatCharacteristics, bool blockPrimeSeat, bool disableSeats, int applicationId, out bool isEplus, bool disableEplus = false)
        {
            StringBuilder sb = new StringBuilder();
            isEplus = false;
            //if (seatType.Lavatory || seatType.Closet || seatType.Galley  || seatType.Stairs  || seatType.Unusable || seatType.Inoperative|| seatType.Space)
            if (CheckSeatRRowType(seatCharacteristics, "IsLavatory") || CheckSeatRRowType(seatCharacteristics, "IsCloset") || CheckSeatRRowType(seatCharacteristics, "IsGalley") || CheckSeatRRowType(seatCharacteristics, "IsStairs") || CheckSeatRRowType(seatCharacteristics, "IsSpace"))
            {
                sb.Append("-");
            }
            //Bug-87823- Added below condition to show the "IsUnusableSeat" seat as blocked in UI- 01/04/2017 -Vijayan
            else if (CheckSeatRRowType(seatCharacteristics, "IsUnusableSeat"))
            {
                sb.Append("X");
            }
            else if (CheckSeatRRowType(seatCharacteristics, "IsInoperativeSeat"))
            {
                if (applicationId == 16)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append("-");
                }
            }
            else if (CheckSeatRRowType(seatCharacteristics, "IsPermBlockedSeat"))
            {
                sb.Append("X");
            }
            else if (CheckSeatRRowType(seatCharacteristics, "IsBulkheadPrime") ||
                      CheckSeatRRowType(seatCharacteristics, "IsBulkheadPrimePlus") ||
                      CheckSeatRRowType(seatCharacteristics, "IsPrimeSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsPrimePlusSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsPrimeLegZoneSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsPrimePlusLegRoomSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusBulkheadPrimeSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusBulkheadPrimePlusSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusExitPrimeSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusExitPrimePlusSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusPrimeSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsEconomyPlusPrimePlusSeat"))
            {
                isEplus = true;
                if (!CheckSeatRRowType(seatCharacteristics, "IsAvailableSeat") || blockPrimeSeat || disableEplus)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append("P");
                }
            }
            else if (CheckSeatRRowType(seatCharacteristics, "IsAvailableSeat") ||
                      CheckSeatRRowType(seatCharacteristics, "IsPermBlockedSeat") &&
                      CheckSeatRRowType(seatCharacteristics, "IsAvailableSeat")
                    )
            {
                sb.Append("O");
            }
            else
            {
                sb.Append("X");
            }

            string aType = sb.ToString();
            //if (disableSeats) QC defect 1618 
            if (aType != "-" && disableSeats)
            {
                if (string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin))
                {
                    sb = new StringBuilder();
                    sb.Append("X");
                }
            }

            return sb.ToString();
        }

        private ServicesAndFees GetServicesAndFees(Service.Presentation.CommonModel.AircraftModel.Seat seat)
        {
            ServicesAndFees serviceAndFee = null;
            if (seat.Price != null)
            {
                serviceAndFee = new ServicesAndFees();
                if (seat.Characteristics != null)
                {
                    foreach (Service.Presentation.CommonModel.Characteristic seatAttr in seat.Characteristics)
                    {
                        switch (seatAttr.Code)
                        {
                            case "SeatSection":
                                {
                                    break;
                                }
                            case "SeatLocation":
                                {
                                    break;
                                }
                            case "IsHeld":
                                {
                                    break;
                                }
                            case "IsOccupiedSeat":
                                {
                                    serviceAndFee.Available = true;
                                    if (seatAttr.Value.ToUpper().Equals("TRUE"))
                                    {
                                        serviceAndFee.Available = false;
                                    }

                                    break;
                                }
                            case "IsAvailableSeat":
                                {
                                    serviceAndFee.Available = true;
                                    break;
                                }
                            case "IsAdvanced":
                                {
                                    break;
                                }
                            case "ColSpan":
                                {
                                    break;
                                }
                            case "RowSpan":
                                {
                                    break;
                                }
                            case "DisplaySeatType":
                                {
                                    serviceAndFee.SeatFeature = seatAttr.Value;
                                    break;
                                }
                            case "SharesSeatType":
                                {
                                    break;
                                }
                                #region
                                //case "IsBulkheadPrimePlus":
                                //{
                                //    //serviceAndFee.DisplaySeatFeature = "Bulkhead Prime Plus";
                                //    serviceAndFee.DisplaySeatFeature = "Economy Plus";
                                //    break;
                                //}
                                //case "IsPrimeSeat":
                                //{
                                //    //serviceAndFee.DisplaySeatFeature = "Prime Seat";
                                //    serviceAndFee.DisplaySeatFeature = "Economy Plus";
                                //    break;
                                //}
                                //case "IsPrimePlusSeat":
                                //{
                                //    //serviceAndFee.DisplaySeatFeature = "Prime Plus Seat";
                                //    serviceAndFee.DisplaySeatFeature = "Economy Plus";
                                //    break;
                                //}
                                //case "IsEconomyPlusBulkheadPrimePlusSeat":
                                //{
                                //    //serviceAndFee.DisplaySeatFeature = "Economy Plus Bulkhead Prime Plus Seat";
                                //    serviceAndFee.DisplaySeatFeature = "Economy Plus";
                                //    break;
                                //}
                                //case "IsEconomyPlusPrimePlusSeat":
                                //{
                                //    //serviceAndFee.DisplaySeatFeature = "Economy Plus Prime Plus Seat";
                                //    serviceAndFee.DisplaySeatFeature = "Economy Plus";
                                //    break;
                                //}
                                #endregion
                        }
                    }
                }

                serviceAndFee.AgentDutyCode = string.Empty;
                serviceAndFee.AgentId = string.Empty;
                serviceAndFee.AgentTripleA = string.Empty;
                serviceAndFee.BaseFee = Convert.ToDecimal(seat.Price.BasePrice.Amount);
                serviceAndFee.Currency = seat.Price.BasePrice.Currency.Code;
                serviceAndFee.TotalFee = Convert.ToDecimal(seat.Price.Totals[0].Amount);
                //serviceAndFee.EliteStatus = seat.Characteristics;
                //serviceAndFee.FeeWaiveType = seatService.FeeWaiveType;
                //serviceAndFee.IsDefault = seatService.IsDefault;
                //serviceAndFee.IsFeeOverriden = seatService.IsFeeOverriden;
                //serviceAndFee.OverrideReason = seatService.OverrideReason;
                serviceAndFee.Program = seat.Price.PromotionCode;
                serviceAndFee.SeatNumber = seat.Identifier;
                if (seat.Price.Taxes != null)
                {
                    if (seat.Price.Taxes.Count > 0)
                    {
                        serviceAndFee.Tax = Convert.ToDecimal(seat.Price.Taxes[0].Amount);
                    }
                }
                //if (Utility.IsMilesFOPEnabled())
                //{
                //    serviceAndFee.TotalMiles = Convert.ToInt32(ConfigurationManager.AppSettings["milesFOP"]);
                //    serviceAndFee.DisplayTotalMiles = Utility.formatAwardAmountForDisplay(ConfigurationManager.AppSettings["milesFOP"], false);
                //}
            }

            return serviceAndFee;
        }

        private bool CheckEPlusSeatCode(string program)
        {
            if (_configuration.GetValue<string>("EPlusSeatProgramCodes") != null)
            {
                string[] codes = _configuration.GetValue<string>("EPlusSeatProgramCodes").Split('|');
                foreach (string code in codes)
                {
                    if (code.Equals(program))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<List<string>> GetPolarisCabinBranding(string authenticationToken, string flightNumber, string departureAirportCode, string flightDate, string arrivalAirportCode, string cabinCount, string languageCode, string sessionId, string operatingCarrier = "UA", string marketingCarrier = "UA")
        {
            List<string> Cabins = null;
            CabinRequest cabinRequest = new CabinRequest();
            CabinResponse cabinResponse = new CabinResponse();

            //Buiding the cabinRequest
            //cabinRequest.CabinCount = cabinCount;
            cabinRequest.CabinCount = cabinCount;
            cabinRequest.DestinationAirportCode = arrivalAirportCode;
            cabinRequest.FlightDate = flightDate;//DateTime.Parse(request.FlightDate).ToString("yyyy-mm-dd");
            cabinRequest.FlightNumber = flightNumber;
            cabinRequest.LanguageCode = languageCode;
            cabinRequest.MarketingCarrier = marketingCarrier;
            cabinRequest.OperatingCarrier = operatingCarrier;
            cabinRequest.OriginAirportCode = departureAirportCode;
            //cabinRequest.ServiceClass = request.

            try
            {
                string path = "CabinBranding";
                string jsonRequest = JsonConvert.SerializeObject(cabinRequest);
                string jsonResponse = await _cabinBrandingService.CabinBranding(authenticationToken, jsonRequest, sessionId, path);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var response = JsonConvert.DeserializeObject<CabinResponse>(jsonResponse);
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        throw new MOBUnitedException("Errors in the CabinBranding Response");
                    }
                    if (response != null && response.Cabins != null && response.Cabins.Count > 0)
                    {
                        Cabins = new List<string>();
                        foreach (var cabin in response.Cabins)
                        {
                            string aCabin = cabin.Description;
                            Cabins.Add(aCabin);
                        }
                    }
                    else
                    {
                        throw new MOBUnitedException("United Data Services not available.");
                    }
                }
                else
                {
                    throw new MOBUnitedException("United Data Services not available.");
                }

            }
            // Added as part of SeatMap- Cabin Branding Service Logging
            catch (WebException exx)
            {
                var exReader = new StreamReader(exx.Response.GetResponseStream()).ReadToEnd().Trim();

                // Added as part of Task - 283491 GetPolarisCabinBranding Exceptions
                if (_configuration.GetValue<bool>("BugFixToggleForExceptionAnalysis") && !string.IsNullOrEmpty(exReader) &&
                    (exReader.StartsWith("{") && exReader.EndsWith("}")))
                {
                    var exceptionDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBFlightStatusError>(exReader);
                    if (exceptionDetails != null && exceptionDetails.Errors != null)
                    {
                        foreach (var error in exceptionDetails.Errors)
                        {
                            if (!string.IsNullOrEmpty(error.MinorCode) && error.MinorCode.Trim().Equals("90830"))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                            }
                        }

                    }

                }
                _logger.LogError("GetPolarisCabinBranding - Exception {ErrorMessageResponse} and {SessionId}", exx.Response, sessionId);

                throw exx;

            }
            catch (System.Exception ex) {

                ExceptionWrapper exceptionWrapper = new ExceptionWrapper(ex);
                _logger.LogError("GetPolarisCabinBranding - Exception {@Exception}", JsonConvert.SerializeObject(exceptionWrapper));
                throw ex;
            }

            return Cabins;
        }

        private bool IsInCheckInWindow(FlightSeatMapDetail seatMapResponse)
        {
            try
            {
                return Convert.ToBoolean(seatMapResponse.IsInCheckInWindow);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool EnoughFreeSeats(int travelerCount, int noOfFreeEplusEligible, int countNoOfFreeSeats, int noOfPricedSeats)
        {
            if (countNoOfFreeSeats >= travelerCount)
                return true;

            var noOfTravelersAfterPickingFreeSeats = travelerCount - countNoOfFreeSeats;
            if ((noOfPricedSeats >= noOfTravelersAfterPickingFreeSeats) && (noOfFreeEplusEligible >= noOfTravelersAfterPickingFreeSeats))
                return true;

            return false;
        }

        public async Task<string> ShowNoFreeSeatsAvailableMessage(Reservation persistedReservation, int noOfFreeSeats, int noOfPricedSeats, bool isInCheckInWindow, bool isFirstSegment)
        {
            if (!_configuration.GetValue<bool>("EnableSSA")) return string.Empty;

            if (persistedReservation != null || persistedReservation.TravelersCSL != null
                || !persistedReservation.IsSSA || ((persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation)) && !isFirstSegment))
                return string.Empty;

            if (persistedReservation.IsELF || _shoppingUtility.IsIBE(persistedReservation))
            {
                if (!_configuration.GetValue<bool>("DisableBESeatBundlesChange"))
                {
                    //suppress seats available for purchase message when any bundle is selected for BE Booking
                    if (persistedReservation.TravelOptions?.Any(t => t?.Key?.Equals("BUNDLES", StringComparison.InvariantCultureIgnoreCase) ?? false) ?? false)
                        return string.Empty;
                }
                return await GetDocumentTextFromDataBase("SSA_ELF_PURCHASE_SEATS_MESSAGE");
            }

            int noOfFreeEplusEligible = GetNoOfFreeEplusSeatsEligible(persistedReservation, isInCheckInWindow);

            if (EnoughFreeSeats(persistedReservation.TravelersCSL.Count, noOfFreeEplusEligible, noOfFreeSeats, noOfPricedSeats))
                return string.Empty;

            return await GetDocumentTextFromDataBase("SSA_NO_FREE_SEATS_MESSAGE");
        }
        private int GetNoOfFreeEplusSeatsEligible(Reservation persistedReservation, bool isInCheckInWindow)
        {
            if (persistedReservation != null)
                return 0;

            if (persistedReservation.AboveGoldMembers > 0)
                return persistedReservation.AboveGoldMembers + 8;

            if (isInCheckInWindow)
                return persistedReservation.NoOfFreeEplusWithSubscriptions
                       + (persistedReservation.GoldMembers * 2)
                       + (persistedReservation.SilverMembers * 2);

            return persistedReservation.NoOfFreeEplusWithSubscriptions + persistedReservation.GoldMembers * 2;
        }

        public async Task<List<MOBSeatMap>> GetSeatMapDetail(string sessionId, string destination, string origin, int applicationId, string appVersion, string deviceId, bool returnPolarisLegendforSeatMap)
        {
            string transactionId = sessionId;
            MOBSHOPFlight segment = new MOBSHOPFlight();

            Reservation persistedReservation = await _sessionHelperService.GetSession<Reservation>(sessionId, new Reservation().ObjectName, new List<string> { sessionId, new Reservation().ObjectName }).ConfigureAwait(false);
            bool isFirstSegmentInReservation = false;
            string cabin = string.Empty;
            string COS = string.Empty;
            string fareBasisCode = string.Empty;
            string bundleCode = string.Empty;

            int countSegment = 0;
            foreach (MOBSHOPTrip flightAvailabilityTrip in persistedReservation.Trips)
            {
                #region
                foreach (MOBSHOPFlattenedFlight flightAvailabilitySegment in flightAvailabilityTrip.FlattenedFlights)
                {
                    foreach (MOBSHOPFlight ts in flightAvailabilitySegment.Flights)
                    {
                        countSegment++;

                        if (ts.Origin.ToUpper() == origin.ToUpper() && ts.Destination.ToUpper() == destination.ToUpper())
                        {
                            if (countSegment == 1)
                            {
                                isFirstSegmentInReservation = true;
                            }
                            COS = ts.ServiceClass;
                            cabin = ts.ServiceClassDescription;
                            if (string.IsNullOrEmpty(cabin))
                            {
                                cabin = ts.Cabin;
                            }

                            if (_configuration.GetValue<bool>("EnableIBE"))
                            {
                                bundleCode = GetFareBasicCodeFromBundles(persistedReservation.TravelOptions, ts.TripIndex, null, destination, origin);
                                fareBasisCode = ts.FareBasisCode;
                            }
                            else
                            {
                                fareBasisCode = GetFareBasicCodeFromBundles(persistedReservation.TravelOptions, ts.TripIndex, ts.FareBasisCode, destination, origin);
                            }

                            segment = ts;

                            if (!ShowSeatMapForCarriers(ts.OperatingCarrier))
                            {
                                bool allowSeatMapForSupportedOaOperatingCarrier =
                                    (_shoppingUtility.OaSeatMapSupportedVersion(applicationId, appVersion, ts.OperatingCarrier, ts.MarketingCarrier));
                                if (persistedReservation.IsReshopChange || !allowSeatMapForSupportedOaOperatingCarrier) // Throw Seat Map Advance Seat Not Avaiable exception for all OA Operating Flights which we are not supporting in booking path and for all OA Operiting carrriers in reshop as in reshop we will not support any oa seat map.d
                                {
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            try
            {
                Service.Presentation.FlightRequestModel.SeatMap request = new Service.Presentation.FlightRequestModel.SeatMap();
                bool isSeatMapSupportedOa = _shoppingUtility.IsSeatMapSupportedOa(segment.OperatingCarrier, segment.MarketingCarrier);
                int flightNum = 0;
                Int32.TryParse(segment.FlightNumber, out flightNum);
                request.FlightNumber = flightNum;
                request.MarketingCarrier = segment.MarketingCarrier;
                request.OperatingCarrier = !string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapForACSubsidiary")) ? _configuration.GetValue<string>("SeatMapForACSubsidiary").Contains(segment.OperatingCarrier) ? "AC" : segment.OperatingCarrier : segment.OperatingCarrier;
                request.DepartureAirport = origin;
                request.ArrivalAirport = destination;
                request.FlightDate = segment.DepartureDateTime;
                request.CabinType = "ALL";
                request.Characteristics = GetTourCode(persistedReservation.ShopReservationInfo2);
                SeatRuleParameter seatRule = new SeatRuleParameter();
                seatRule.Flight = new Service.Presentation.CommonModel.FlightProfile();
                seatRule.Flight.FlightNumber = flightNum.ToString();
                seatRule.Flight.DepartureAirport = origin;
                seatRule.Flight.DepartureDate = segment.DepartureDateTime;
                //Added as part of the changes for the exception 284001:Select seatsFormatException
                if (!string.IsNullOrEmpty(segment.DestinationDate))
                {
                    seatRule.Flight.ArrivalDate = Convert.ToDateTime(segment.DestinationDate).ToString("yyyy-MM-dd");
                }

                if (_configuration.GetValue<bool>("EnableIBE"))
                {
                    if (segment.ShoppingProducts != null && segment.ShoppingProducts.Any())
                        seatRule.ProductCode = segment.ShoppingProducts[0].ProductCode;

                    seatRule.FareBasisCode = fareBasisCode;
                    seatRule.BundleCode = bundleCode;
                }
                else
                {
                    seatRule.FareBasisCode = fareBasisCode;
                }

                seatRule.Flight.ArrivalAirport = destination;
                seatRule.Flight.OperatingCarrierCode = segment.OperatingCarrier;
                seatRule.Segment = origin + destination;
                if (segment.OperatingCarrier.ToUpper().Trim() == "CO")
                    seatRule.AirCarrierType = "1";   //1 – CO plane ; 2 – Express/Connection
                else
                    seatRule.AirCarrierType = "2";
                //seatRule.CabinType = cabin;

                if (isSeatMapSupportedOa && segment.MarketingCarrier != "UA")
                {
                    request.OperatingFlightNumber = flightNum;
                    seatRule.Flight.OperatingFlightNumber = flightNum;
                }
                //Bug 190803 fix verified with Venkat & Mahi
                seatRule.ClassOfService = segment.ServiceClass;
                //seatRule.EliteStatus = "0";                
                seatRule.IsChaseCardMember = "false";
                seatRule.IsInCabinPet = "false";
                seatRule.IsLapChild = "false";

                if (_shoppingUtility.EnableTravelerTypes(applicationId, appVersion, persistedReservation.IsReshopChange) &&
                    persistedReservation != null && persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelerTypes != null
                    && persistedReservation.ShopReservationInfo2.TravelerTypes.Count > 0)
                {
                    if (persistedReservation.ShopReservationInfo2.displayTravelTypes.Any(t => t.TravelerType.ToString().Equals(PAXTYPE.InfantLap.ToString()) || t.TravelerType.ToString().Equals(PAXTYPE.InfantSeat.ToString())))
                        seatRule.IsLapChild = "true";

                    seatRule.PassengerCount = persistedReservation.ShopReservationInfo2.displayTravelTypes.Where(t => !t.PaxType.ToUpper().Equals("INF")).Count();
                }
                else
                {
                    seatRule.PassengerCount = persistedReservation.TravelersCSL.Count;
                }
                seatRule.IsOxygen = "false";
                seatRule.IsServiceAnimal = "false";
                seatRule.IsUnaccompaniedMinor = "false";
                seatRule.LanguageCode = "";
                seatRule.PassengerClass = string.Empty;
                seatRule.hasSSR = "false";
                request.Rules = new Collection<SeatRuleParameter>();
                request.Rules.Add(seatRule);

                request.Travelers = new Collection<ProductTraveler>();

                #region Award Travel Companion EPlus Seats Fix

                ShoppingResponse shop = new ShoppingResponse();
                shop = await _sessionHelperService.GetSession<ShoppingResponse>(sessionId, shop.ObjectName, new List<string> { sessionId, shop.ObjectName }).ConfigureAwait(false);

                if (!persistedReservation.IsReshopChange)
                {
                    if (shop != null)
                    {
                        if (shop.Request.AwardTravel && !string.IsNullOrEmpty(shop.Request.MileagePlusAccountNumber))
                        {
                            ProfileResponse profile = new ProfileResponse();
                            profile = await _sessionHelperService.GetSession<ProfileResponse>(sessionId, profile.ObjectName, new List<string> { sessionId, profile.ObjectName }).ConfigureAwait(false);

                            ProductTraveler travelerInformation = new ProductTraveler();
                            travelerInformation.Characteristics = new Collection<Service.Presentation.CommonModel.Characteristic>();

                            Service.Presentation.CommonModel.Characteristic travelerCharacteristic1 = new Service.Presentation.CommonModel.Characteristic();
                            travelerCharacteristic1.Code = "SponsorAccount";
                            travelerCharacteristic1.Value = profile.Response.Profiles[0].Travelers.Find(item => item.IsProfileOwner).MileagePlus.MileagePlusId;
                            travelerInformation.Characteristics.Add(travelerCharacteristic1);

                            Service.Presentation.CommonModel.Characteristic travelerCharacteristic2 = new Service.Presentation.CommonModel.Characteristic();
                            travelerCharacteristic2.Code = "SponsorEliteStatus";
                            travelerCharacteristic2.Value = profile.Response.Profiles[0].Travelers.Find(item => item.IsProfileOwner).MileagePlus.CurrentEliteLevel.ToString();
                            travelerInformation.Characteristics.Add(travelerCharacteristic2);

                            request.Travelers.Add(travelerInformation);
                        }
                    }
                }
                #endregion

                #region
                if (persistedReservation.TravelersCSL != null && persistedReservation.TravelersCSL.Count > 0)
                {
                    MOBSHOPReservation reservation = new MOBSHOPReservation(_configuration, _cachingService);
                    foreach (string travelerKey in persistedReservation.TravelerKeys)
                    {
                        MOBCPTraveler bookingTravelerInfo = persistedReservation.TravelersCSL[travelerKey];

                        if (_shoppingUtility.EnableTravelerTypes(applicationId, appVersion, persistedReservation.IsReshopChange) &&
                            persistedReservation.ShopReservationInfo2 != null && persistedReservation.ShopReservationInfo2.TravelerTypes != null && persistedReservation.ShopReservationInfo2.TravelerTypes.Count > 0)
                        {
                            if (bookingTravelerInfo.TravelerTypeCode.ToUpper().Equals("INF"))
                                continue;
                        }

                        ProductTraveler TravelerInformations = new ProductTraveler();

                        TravelerInformations.GivenName = bookingTravelerInfo.FirstName;
                        TravelerInformations.Surname = bookingTravelerInfo.LastName;
                        TravelerInformations.IsSelected = "true";
                        TravelerInformations.IsEPlusSubscriber = "false";
                        if (bookingTravelerInfo.AirRewardPrograms != null)
                        {
                            TravelerInformations.LoyaltyProgramProfile = new Service.Presentation.CommonModel.LoyaltyProgramProfile();
                            TravelerInformations.LoyaltyProgramProfile.LoyaltyProgramCarrierCode = string.Empty;
                            TravelerInformations.LoyaltyProgramProfile.LoyaltyProgramMemberID = string.Empty;
                            if (bookingTravelerInfo.AirRewardPrograms.Count > 0)
                            {
                                foreach (var loyaltyProfile in bookingTravelerInfo.AirRewardPrograms)
                                {
                                    RewardProgram shopreward = null;
                                    if (reservation != null && reservation.RewardPrograms != null)
                                    {
                                        shopreward = reservation.RewardPrograms.Find(p => p.ProgramID == loyaltyProfile.ProgramId);
                                    }

                                    if (loyaltyProfile.CarrierCode.Trim().ToUpper() == "UA" || (string.IsNullOrEmpty(loyaltyProfile.CarrierCode) && shopreward != null && shopreward.Type == "UA"))
                                    {
                                        TravelerInformations.LoyaltyProgramProfile.LoyaltyProgramMemberID = loyaltyProfile.MemberId;
                                        TravelerInformations.LoyaltyProgramProfile.LoyaltyProgramCarrierCode = (string.IsNullOrEmpty(loyaltyProfile.CarrierCode) ? shopreward.Type.Trim().ToUpper() : loyaltyProfile.CarrierCode.Trim().ToUpper());
                                        //TravelerInformations.LoyaltyProgramProfile.LoyaltyProgramMemberTierLevel = bookingTravelerInfo.LoyaltyProgramProfile.MemberTierLevel;                 
                                    }
                                }
                            }
                        }
                        request.Travelers.Add(TravelerInformations);
                    }
                }
                #endregion

                string jsonRequest = JsonConvert.SerializeObject(request);

                // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<United.Service.Presentation.FlightRequestModel.SeatMap>(transactionId, "Request for Seat Engine Seat Map Service", "Request", applicationId, appVersion, deviceId, request));
                //string seatMapToken = Authentication.GetToken(System.Configuration.ConfigurationManager.AppSettings["AccessKey - CSLAuthenticate"]); //GetSeatMapSecurityToken();
                Session session = new Session();
                session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
                if (session == null)
                {
                    throw new MOBUnitedException("Could not find your booking session.");
                }
                //string url = "http://unitedservicesstage.ual.com/7.2/flight/seatmap/GetSeatMapDetail";
                string actionName = persistedReservation.IsSSA ?
                            ("GetSeatMapDetailWithFare") :
                             ("GetSeatMapDetail");
                FlightSeatMapDetail response = await _seatMapService.SeatEngine<FlightSeatMapDetail>(session.Token, actionName, jsonRequest, sessionId);

                if (!string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapForDeck")) && _configuration.GetValue<string>("SeatMapForDeck").Contains(request.OperatingCarrier))
                {
                    response.SeatMap.FlightInfo.OperatingCarrierCode = request.OperatingCarrier;
                }
                //Adding version changes for Bug 95616 - Ranjit
                var seatMaps = await GetSeatMapWithFeesFromCSlResponse(response, cabin, sessionId, isFirstSegmentInReservation, isSeatMapSupportedOa, applicationId, transactionId, returnPolarisLegendforSeatMap, appVersion);
                bool tomToggle = _configuration.GetValue<bool>("EnableProjectTOM");
                bool isBus = (seatMaps[0].FleetType.Length >= 2 && seatMaps[0].FleetType.Substring(0, 2).ToUpper().Equals("BU"));

                if (seatMaps != null && seatMaps.Count == 1 && seatMaps[0].IsOaSeatMap && (!tomToggle || (tomToggle && !isBus)))
                {
                    seatMaps[0].OperatedByText = GetOperatedByText(segment.MarketingCarrier, segment.FlightNumber, segment.OperatingCarrierDescription);
                }

                return seatMaps;
            }
            catch (System.Exception ex)
            {
                //string err = ex.Message;
                throw ex;
            }
        }

        private Collection<Service.Presentation.CommonModel.Characteristic> GetTourCode(ReservationInfo2 shopReservationInfo2)
        {
            if (!_configuration.GetValue<bool>("SendTourCodeToSeatEngine"))
                return null;

            if (shopReservationInfo2 == null || shopReservationInfo2.Characteristics == null || !shopReservationInfo2.Characteristics.Any())
                return null;

            var tourCode = shopReservationInfo2.Characteristics.Where(c => c != null && c.Id != null && c.Id.Equals("tourboxcode", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (tourCode == null || string.IsNullOrEmpty(tourCode.CurrentValue))
                return null;

            return new Collection<Service.Presentation.CommonModel.Characteristic> { new Service.Presentation.CommonModel.Characteristic { Code = "tourboxcode", Value = tourCode.CurrentValue.Trim() } };
        }

        #region AM
        public bool ValidateResponse(MOBSeatChangeInitializeResponse response)
        {
            if (_configuration.GetValue<bool>("TurnOffValidateInitializeSeatChangeResponse"))
                return true;

            if (response.SelectedTrips.IsNullOrEmpty())
                return false;

            if (response.SelectedTrips.Any(s => ValidFlattenFlights(s?.FlattenedFlights)))
            {
                return true;
            }

            return false;
        }

        private bool ValidFlattenFlights(List<MOBBKFlattenedFlight> flattenedFlights)
        {
            return flattenedFlights?.Any(f => !f.Flights.IsNullOrEmpty()) ?? false;
        }

        public async Task<Mobile.Model.MPRewards.SeatEngine> GetFlightReservationCSL_CFOP(MOBSeatChangeInitializeRequest request, Mobile.Model.MPRewards.SeatEngine seatEngine, bool isVerticalSeatMapEnabled)
        {
            string token = string.Empty;

            bool isMilesFOPEnabled = ConfigUtility.IsMFOPCatalogEnabled(request.CatalogValues);

            ReservationDetail response = new ReservationDetail();

            response = await _sessionHelperService.GetSession<ReservationDetail>(request.SessionId, response.GetType().FullName, new List<string> { request.SessionId, response.GetType().FullName }).ConfigureAwait(false);
            if (response == null)
            {
                var tupleResponse = await GetPnrDetailsFromCSL(request.TransactionId, request.RecordLocator, request.LastName, request.Application.Id, request.Application.Version.Major, "GetFlightReservationCSL", token);
                var jsonResponse = tupleResponse.Item1;
                token = tupleResponse.token;
                if (string.IsNullOrEmpty(jsonResponse)) return default;
                response = DataContextJsonSerializer.DeserializeJsonDataContract<ReservationDetail>(jsonResponse);
            }

            if (response != null && (response.Error == null || response.Error.Count == 0))
            {
                if (response.Detail != null)
                {
                    bool isBundleSeatMap = _shoppingUtility.IsEnableTravelOptionsInViewRes(request.Application.Id, request?.Application?.Version?.Major) && FlowType.VIEWRES_BUNDLES_SEATMAP.ToString().Equals(request.Flow);
                    MOBShoppingCart persistShoppingCart = new MOBShoppingCart();
                    if (isBundleSeatMap)
                    {
                        persistShoppingCart = await _sessionHelperService.GetSession<MOBShoppingCart>(request.SessionId, persistShoppingCart.ObjectName, new List<string> { request.SessionId, persistShoppingCart.ObjectName }).ConfigureAwait(false);
                    }

                    bool checkInSegmentSeatChangeFixEnabled = await _featureSettings.GetFeatureSettingValue("EnableVSMCheckedInSegmentFix").ConfigureAwait(false);
                    bool unETicketedCheckInPNRSeatChangeEnabled = await _featureSettings.GetFeatureSettingValue("EnableUnETicketedCheckInPNRSeatChange").ConfigureAwait(false);
                    bool checkInSegmentChangeSeatLinkFixEnabled = await _featureSettings.GetFeatureSettingValue("EnableCheckInSegmentChangeSeatLinkFix").ConfigureAwait(false);

                    bool isETicketed = ShopStaticUtility.GetETicketStatus(response.Detail);

                    var seatengine = new United.Mobile.Model.MPRewards.SeatEngine();
                    seatEngine.PNRCreationDate = GeneralHelper.FormatDatetime(Convert.ToDateTime(response.Detail.CreateDate).ToString("yyyyMMdd hh:mm tt"), request.LanguageCode);
                    seatEngine.SelectedTrips = PopulateFlightAddTripResponseCSL(response.Detail, unETicketedCheckInPNRSeatChangeEnabled, ref isVerticalSeatMapEnabled, isETicketed, checkInSegmentSeatChangeFixEnabled, checkInSegmentChangeSeatLinkFixEnabled, request.Application.Version.Major, request.Application.Id, isBundleSeatMap, persistShoppingCart);
                    seatEngine.Seats = PopulateSeatsCSL(response.Detail, isMilesFOPEnabled);
                    seatEngine.Segments = await PopulateTripSegmentsCSL_CFOP(response, request, isBundleSeatMap, persistShoppingCart, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentChangeSeatLinkFixEnabled);
                    seatEngine.BookingTravelerInfo = PopulateBookingTravelerInfoCSL(response, GetSeatFocusShareIndex(request.SeatFocusRequest));
                    PopulateSeatsInTravelers(ref seatEngine);
                    seatEngine.RecordLocator = response.Detail?.ConfirmationID;
                    seatEngine.LastName = response.Detail?.Travelers?.FirstOrDefault()?.Person?.Surname;
                    seatEngine.PointOfSale = response.Detail?.PointOfSale?.Country?.CountryCode;
                    seatEngine.IsVerticalSeatMapEnabled = isVerticalSeatMapEnabled;
                    return seatEngine;
                }
                else
                {
                    throw new MOBUnitedException("We are unable to retrieve the latest information for this itinerary.");
                }
            }
            else
            {
                if (response != null && response.Error != null && response.Error.Count > 0)
                {
                    var errorMsg = response.Error.Aggregate(string.Empty, (current, e) => current + " " + e.Description);
                    if (!string.IsNullOrEmpty(errorMsg) &&
                        errorMsg.IndexOf("The last name you entered, does not match the name we have on file.") != -1)
                    {
                        throw new MOBUnitedException("The confirmation number and last name do not match our records. Please try again.");
                    }
                }
                else
                {
                    if (response != null) throw new MOBUnitedException(response.Error.ToString());
                }
            }
            return default;
        }

        private void PopulateSeatsInTravelers(ref Mobile.Model.MPRewards.SeatEngine seatEngine)
        {
            foreach (MOBBKTraveler traveler in seatEngine.BookingTravelerInfo)
            {
                if (seatEngine.Seats != null)
                {
                    IEnumerable<Seat> seatList = from s in seatEngine.Seats
                                                 where s.TravelerSharesIndex == traveler.SHARESPosition
                                                 select s;
                    if (seatList.Count() > 0)
                    {
                        List<Seat> travelerSeats = new List<Seat>();
                        travelerSeats = seatList.ToList();
                        travelerSeats = travelerSeats.OrderBy(s => s.Key).ToList();
                        traveler.Seats = travelerSeats;
                    }
                }
                int seatKey = 0;
                List<Seat> allSeats = new List<Seat>();
                foreach (var trip in seatEngine.SelectedTrips)
                {
                    foreach (var flattenedFlight in trip.FlattenedFlights)
                    {
                        foreach (var flight in flattenedFlight.Flights)
                        {
                            bool seatFound = false;
                            Seat aSeat = null;
                            if (traveler.Seats != null)
                            {
                                foreach (var seat in traveler.Seats)
                                {
                                    //Bug 181260 - Modified the below condition to get the correct seat for the segments- Vijayan
                                    if (seat.Origin.Equals(flight.Origin)
                                        && seat.Destination.Equals(flight.Destination)
                                        && Convert.ToDateTime(seat.DepartureDate).ToString("MMddyyy") == Convert.ToDateTime(flight.DepartDate).ToString("MMddyyy")
                                        && seat.FlightNumber.Equals(flight.FlightNumber)
                                        )
                                    {
                                        aSeat = seat;
                                        seatFound = true;
                                        break;
                                    }
                                }
                            }
                            if (seatFound)
                            {
                                allSeats.Add(aSeat);
                            }
                            else
                            {
                                aSeat = new Seat();
                                aSeat.Origin = flight.Origin;
                                aSeat.Destination = flight.Destination;
                                aSeat.FlightNumber = flight.FlightNumber;
                                aSeat.DepartureDate = flight.DepartureDateTime;
                                aSeat.TravelerSharesIndex = traveler.SHARESPosition;
                                aSeat.OldSeatAssignment = string.Empty;
                                aSeat.Key = seatKey;
                                aSeat.OldSeatType = "ALL";
                                allSeats.Add(aSeat);
                            }
                            ++seatKey;
                        }
                    }
                }

                traveler.Seats = allSeats;
            }
        }

        private string GetSeatFocusShareIndex(MOBSeatFocus seatFocusRequest)
        {
            if (!_configuration.GetValue<bool>("IsSeatNumberClickableEnabled"))
                return string.Empty;

            return seatFocusRequest?.SharesIndex ?? string.Empty;
        }

        private List<MOBBKTraveler> PopulateBookingTravelerInfoCSL(ReservationDetail response, string seatFocusShareIndex)
        {
            if (response.Detail.Travelers == null || !response.Detail.Travelers.Any()) return null;
            int numberOfPax = 0;
            var bookingTravelerInfo = new List<MOBBKTraveler>();

            bool isSeatFocusShareIndexEnabled = _configuration.GetValue<bool>("IsSeatNumberClickableEnabled");

            foreach (var traveler in response.Detail.Travelers)
            {
                //Bug 244886 :XML to CSL Migration-mApp: Lap Infant name is listed in the Seat map Travelers list and Unable to Change the seats for the adult with lap infant
                if (traveler.Person != null && !string.IsNullOrWhiteSpace(traveler.Person.Type) && traveler.Person.Type.ToUpper().Contains("INF"))
                {
                    continue;
                }

                MOBBKTraveler bti = new MOBBKTraveler();
                bti.Person = new United.Mobile.Model.Shopping.Booking.MOBBKPerson();
                // bti.Person.TravelerName = new MOBName();
                // bti.Key = traveler.Person.Key; ; need to check from csl
                bti.Person.GivenName = traveler.Person.GivenName;
                bti.Person.MiddleName = traveler.Person.MiddleName;
                bti.Person.Surname = traveler.Person.Surname;
                bti.Person.Suffix = traveler.Person.Suffix;
                bti.Person.Title = traveler.Person.Title;
                bti.Person.DateOfBirth = traveler.Person.DateOfBirth;
                bti.TravelerTypeCode = traveler.Person.Type; //GetCharactersticValue(traveler.Characteristics, "TravelerTypeCode")
                bti.SHARESPosition = traveler.Person.Key;//GetCharactersticValue(traveler.Characteristics, "SHARESPosition"); //New Method
                bti.LoyaltyProgramProfile = GetAirRewardProgramCSL(traveler); // New Method - mapping done
                bti.Ticket = GetTicketCSL(bti.Person.GivenName, bti.Person.Surname, traveler); // New Method

                if (isSeatFocusShareIndexEnabled)
                {
                    if (!string.IsNullOrEmpty(seatFocusShareIndex) && seatFocusShareIndex == traveler.Person.Key)
                    {
                        bti.ShowSeatFocus = true;
                    }
                }

                bookingTravelerInfo.Add(bti);

                numberOfPax = numberOfPax + 1;
            }
            return bookingTravelerInfo;
        }

        private Ticket GetTicketCSL(string firstName, string lastName, Service.Presentation.ReservationModel.Traveler objTraveler)
        {
            Ticket ticket = null;
            //if (objTraveler != null && objTicket.Length > 0 && !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            if (objTraveler != null && !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                if (objTraveler.Tickets != null)
                {
                    foreach (var t in objTraveler.Tickets)
                    {
                        if (!string.IsNullOrEmpty(t.Issuer) && string.Format("{0}/{1}", lastName.Trim().ToUpper(), firstName.Trim().ToUpper()).IndexOf(t.Issuer.Trim().ToUpper()) != -1)
                        {
                            ticket = new Ticket();
                            //ticket.CurrencyOfIssuance = t
                            //ticket.Destination = t.;
                            //ticket.FareBasis = t.fare;
                            //ticket.FlightDate = t.FlightDate;
                            //ticket.GiftSequence = t.GiftSequence;
                            //ticket.IsActive = t.a;
                            //ticket.IsBulkTicket = t.BulkTicket;
                            //ticket.IsGiftTicket = t.TicketGiftTicket;
                            //ticket.IssuedBy = t.IssuedBy;
                            //ticket.IssuedTo = t.IssuedTo;
                            ticket.Number = t.DocumentID;
                            //ticket.Origin = t.Origin;
                            ticket.Sequence = t.SequenceNumber;
                            ticket.Status = t.Status.ToString();
                            // ticket.TotalFare = t.TotalFare;

                            ////ToDo - Need to verify the below properties  -- May 2
                            //ticket.CurrencyOfIssuance = t.Characteristic.Where(x => x.Code == "CurrencyOfIssuance" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.Destination = t.Characteristic.Where(x => x.Code == "Destination" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.FareBasis = t.Characteristic.Where(x => x.Code == "FareBasis" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.FlightDate = t.Characteristic.Where(x => x.Code == "FlightDate" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.GiftSequence = Convert.ToInt32(t.Characteristic.Where(x => x.Code == "GiftSequence" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            //ticket.IsActive = Convert.ToBoolean(t.Characteristic.Where(x => x.Code == "Active" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            //ticket.IsBulkTicket = Convert.ToBoolean(t.Characteristic.Where(x => x.Code == "BulkTicket" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            //ticket.IsGiftTicket = Convert.ToBoolean(t.Characteristic.Where(x => x.Code == "TicketGiftTicket" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            //ticket.IssuedBy = t.Characteristic.Where(x => x.Code == "IssuedBy" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.IssuedTo = t.Characteristic.Where(x => x.Code == "IssuedTo" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.Number = t.Characteristic.Where(x => x.Code == "TicketNumber" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.Origin = t.Characteristic.Where(x => x.Code == "Origin" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.Sequence = Convert.ToInt32(t.Characteristic.Where(x => x.Code == "TicketSequence" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            //ticket.Status = t.Characteristic.Where(x => x.Code == "Status" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value;
                            //ticket.TotalFare = Convert.ToDecimal(t.Characteristic.Where(x => x.Code == "TotalFare" && !string.IsNullOrEmpty((x.Value).Trim())).First().Value);
                            break;
                        }
                    }
                }

                //}
            }
            return ticket;
        }

        private MOBBKLoyaltyProgramProfile GetAirRewardProgramCSL(Service.Presentation.ReservationModel.Traveler traveler)
        {
            MOBBKLoyaltyProgramProfile airRewardProgram = null;

            if (traveler != null && traveler.LoyaltyProgramProfile != null)
            {
                if (!string.IsNullOrEmpty(traveler.LoyaltyProgramProfile.LoyaltyProgramCarrierCode) &&
                   traveler.LoyaltyProgramProfile.LoyaltyProgramCarrierCode.Equals("UA"))
                {
                    airRewardProgram = new MOBBKLoyaltyProgramProfile();
                    airRewardProgram.MPEliteLevel = (int)traveler.LoyaltyProgramProfile.LoyaltyProgramMemberTierDescription;
                    airRewardProgram.ProgramId = traveler.LoyaltyProgramProfile.LoyaltyProgramID;
                    //airRewardProgram.ProgramName = need to check
                    airRewardProgram.MemberId = traveler.LoyaltyProgramProfile.LoyaltyProgramMemberID;
                    airRewardProgram.CarrierCode = traveler.LoyaltyProgramProfile.LoyaltyProgramCarrierCode;
                }
            }
            return airRewardProgram;
        }

        private async Task<List<TripSegment>> PopulateTripSegmentsCSL_CFOP(ReservationDetail response, MOBSeatChangeInitializeRequest request, bool isBundleSeatMap, MOBShoppingCart persistShoppingCart, bool unETicketedCheckInPNRSeatChangeEnabled, bool isETicketed, bool isVerticalSeatMapEnabled, bool checkInSegmentSeatChangeLinkFixEnabled)
        {
            if (response == null || response.Detail.FlightSegments == null || !response.Detail.FlightSegments.Any())
                return null;

            bool isOaSupportedVersion = GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request?.Application?.Version?.Major, "AndroidOaSeatMapVersion", "iPhoneOaSeatMapVersion", "", "", true, _configuration);
            var tripSegments = new List<TripSegment>();
            int segmentIndex = 1;
            foreach (var segment in response.Detail.FlightSegments)
            {
                if (isVerticalSeatMapEnabled && !ShowSeatChange(segment, response.Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentSeatChangeLinkFixEnabled))
                {
                    continue;
                }

                bool segmentPast = false;
                if (response.Detail.Prices != null && response.Detail.Prices.Count > 0)
                {
                    foreach (PriceFlightSegment pSegment in response.Detail.Prices[0].PriceFlightSegments)
                    {
                        if (pSegment.SegmentNumber == segment.SegmentNumber && pSegment.FlightStatuses != null && pSegment.FlightStatuses.Count > 0)
                        {
                            if (pSegment.FlightStatuses[0].Code.ToUpper() == "UA USED")
                            {
                                segmentPast = true;
                                segmentIndex++;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(segment.FlightSegment.DepartureDateTime).AddHours(12) < DateTime.Now)
                {
                    continue;
                }
                var actionCode = segment.FlightSegment.FlightSegmentType.Substring(0, 2);

                if ((actionCode.ToUpper().Trim() == "HK" || actionCode.ToUpper().Trim() == "DK" || actionCode.ToUpper().Trim() == "KL" || actionCode.ToUpper().Trim() == "RR" || actionCode.ToUpper().Trim() == "TK") && !segmentPast)
                {

                    bool isGaugeChange = string.IsNullOrEmpty(segment.FlightSegment.IsChangeOfGauge) ? false : Convert.ToBoolean(segment.FlightSegment.IsChangeOfGauge);
                    if (isGaugeChange && segment.FlightSegment.NumberofStops > 0 && segment.Legs != null)
                    {
                        int legIndex = 1;
                        foreach (PersonFlightSegment stopSegment in segment.Legs)
                        {
                            //GaugeChanged not required since data is present in legs
                            TripSegment tripSegment = new TripSegment();

                            tripSegment.Arrival = new MOBAirport();
                            tripSegment.Arrival.Code = stopSegment.ArrivalAirport.IATACode;
                            tripSegment.Arrival.Name = stopSegment.ArrivalAirport.Name;
                            tripSegment.Arrival.City = GetCityNameFromAirportDescription(tripSegment.Arrival.Name);

                            tripSegment.Equipment = new United.Mobile.Model.Shopping.Aircraft();
                            tripSegment.Equipment.Code = stopSegment.Equipment.Model.Fleet;
                            tripSegment.Equipment.ShortName = stopSegment.Equipment.Model.Description;
                            tripSegment.Equipment.LongName = stopSegment.Equipment.Model.Description;

                            tripSegment.CodeshareFlightNumber = string.Empty;
                            tripSegment.CrossFleetCOFlightNumber = string.Empty;
                            tripSegment.CSCC = string.Empty;
                            tripSegment.CSFN = string.Empty;
                            tripSegment.Departure = new MOBAirport();
                            tripSegment.Departure.Code = stopSegment.DepartureAirport.IATACode;
                            tripSegment.Departure.Name = stopSegment.DepartureAirport.Name;
                            tripSegment.Departure.City = GetCityNameFromAirportDescription(tripSegment.Departure.Name);
                            tripSegment.FlightNumber = stopSegment.FlightNumber;
                            tripSegment.IsCrossFleet = false;
                            if (stopSegment.MarketedFlightSegment != null && stopSegment.MarketedFlightSegment.Count > 0)
                            {
                                tripSegment.MarketingCarrier = stopSegment.MarketedFlightSegment[0].MarketingAirlineCode;
                            }
                            else  //Added below condition to take MarketingCarrier code from segment if stopsegment is null
                            {
                                if (segment.FlightSegment.MarketedFlightSegment != null && segment.FlightSegment.MarketedFlightSegment.Count > 0)
                                {
                                    tripSegment.MarketingCarrier = segment.FlightSegment.MarketedFlightSegment[0].MarketingAirlineCode;
                                }
                            }
                            if (!_configuration.GetValue<bool>("DisableVIEWRESDecouplingSeatChangeIn24hrWindow"))
                            {
                                tripSegment.IsCheckedIn = _shoppingUtility.IsCheckedIn(segment);
                                if (tripSegment.IsCheckedIn)
                                {
                                    tripSegment.IsAllPaxCheckedIn = _shoppingUtility.IsAllPaxCheckedIn(response, segment, tripSegment.IsCheckedIn);
                                }
                            }

                            tripSegment.OperatingCarrier = stopSegment.OperatingAirlineCode;
                            tripSegment.OperatingCarrierDescription = stopSegment.OperatingAirlineName;
                            tripSegment.ScheduledDepartureDate = stopSegment.DepartureDateTime;
                            tripSegment.ScheduledDepartureDateFormated = Convert.ToDateTime(stopSegment.DepartureDateTime).ToString("MMM., dd, yyyy");
                            // Commented below line  and changed the condition to get the serviceclass and its description.
                            tripSegment.ServiceClass = ((stopSegment.BookingClasses == null || stopSegment.BookingClasses.Count.Equals(0)) || string.IsNullOrEmpty(stopSegment.BookingClasses[0].Cabin.Name)) ? segment.FlightSegment.BookingClasses[0].Code : stopSegment.BookingClasses[0].Cabin.Name;
                            tripSegment.ServiceClassDescription = ((stopSegment.BookingClasses == null || stopSegment.BookingClasses.Count.Equals(0)) || string.IsNullOrEmpty(stopSegment.BookingClasses[0].Cabin.Description)) ? segment.FlightSegment.BookingClasses[0].Cabin.Name : stopSegment.BookingClasses[0].Cabin.Description;
                            tripSegment.COGStop = true;
                            tripSegment.SegmentIndex = segmentIndex;
                            tripSegment.LegIndex = legIndex;
                            tripSegment.OriginalSegmentNumber = segment.SegmentNumber;
                            tripSegment.ProductCode = GetCharactersticValue(stopSegment.Characteristic, "ProductCode");
                            tripSegment.IsIBE = _shoppingUtility.IsIBEFullFare(tripSegment.ProductCode);
                            tripSegment.FareBasisCode = ShopStaticUtility.GetFareBasisCode(response.Detail.Prices, stopSegment.SegmentNumber);
                            tripSegment.BundleProductCode = isBundleSeatMap ? GetBundleCode(persistShoppingCart?.Products, segment.SegmentNumber) : string.Empty;
                            tripSegment.ShowSeatChange = ShowSeatChange(segment, response.Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentSeatChangeLinkFixEnabled);
                            tripSegments.Add(tripSegment);
                            legIndex++;
                            segmentIndex++;
                        }
                    }
                    else
                    {

                        TripSegment tripSegment = new TripSegment();
                        tripSegment.CodeshareFlightNumber = string.Empty;
                        tripSegment.CrossFleetCOFlightNumber = string.Empty;
                        tripSegment.CSCC = string.Empty;
                        tripSegment.CSFN = string.Empty;

                        tripSegment.Departure = new MOBAirport();
                        tripSegment.Departure.Code = segment.FlightSegment.DepartureAirport.IATACode;
                        tripSegment.Departure.Name = segment.FlightSegment.DepartureAirport.Name;
                        tripSegment.Departure.City = GetCityNameFromAirportDescription(tripSegment.Departure.Name);
                        tripSegment.Equipment = new United.Mobile.Model.Shopping.Aircraft();
                        tripSegment.Equipment.Code = segment.FlightSegment.Equipment.Model.Fleet;
                        tripSegment.Equipment.ShortName = segment.FlightSegment.Equipment.Model.Description;
                        tripSegment.Equipment.LongName = segment.FlightSegment.Equipment.Model.Description;
                        tripSegment.FlightNumber = segment.FlightSegment.FlightNumber;
                        tripSegment.IsCrossFleet = false;
                        if (segment.FlightSegment.MarketedFlightSegment != null && segment.FlightSegment.MarketedFlightSegment.Count > 0)
                        {
                            tripSegment.MarketingCarrier = segment.FlightSegment.MarketedFlightSegment[0].MarketingAirlineCode;

                        }
                        tripSegment.OperatingCarrier = segment.FlightSegment.OperatingAirlineCode;
                        tripSegment.OperatingCarrierDescription = segment.FlightSegment.OperatingAirlineName;
                        tripSegment.ScheduledDepartureDate = segment.FlightSegment.DepartureDateTime;
                        tripSegment.ScheduledDepartureDateFormated = Convert.ToDateTime(segment.FlightSegment.DepartureDateTime).ToString("MMM., dd, yyyy");
                        tripSegment.ServiceClassDescription = segment.FlightSegment.BookingClasses[0].Cabin.Name;
                        tripSegment.ServiceClass = segment.FlightSegment.BookingClasses[0].Code;

                        tripSegment.CarrierCode = segment.FlightSegment.OperatingAirlineCode;

                        tripSegment.Arrival = new MOBAirport();
                        tripSegment.Arrival.Code = segment.FlightSegment.ArrivalAirport.IATACode;
                        tripSegment.Arrival.Name = segment.FlightSegment.ArrivalAirport.Name;
                        tripSegment.Arrival.City = GetCityNameFromAirportDescription(tripSegment.Arrival.Name);

                        if (!_shoppingUtility.EnableOAMessageUpdate(request.Application.Id, request.Application.Version.Major) && segment.FlightSegment.OperatingAirlineCode != null
                        && !ShowSeatMapForCarriers(segment.FlightSegment.OperatingAirlineCode.Trim())
                        && !(isOaSupportedVersion && _shoppingUtility.IsSeatMapSupportedOa(segment.FlightSegment.OperatingAirlineCode, segment.FlightSegment.MarketedFlightSegment[0] != null ? segment.FlightSegment.MarketedFlightSegment[0].MarketingAirlineCode : string.Empty)))
                        {
                            tripSegment.IsCheckInWindow = true;
                            tripSegment.CheckInWindowText = _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines");
                        }
                        else if (IsInChecKInWindow(segment.EstimatedDepartureUTCTime))
                        {
                            tripSegment.IsCheckInWindow = true;

                            if (_shoppingUtility.EnableNewChangeSeatCheckinWindowMsg(request.Application.Id, request?.Application?.Version?.Major))
                            {
                                if (unETicketedCheckInPNRSeatChangeEnabled && !isETicketed)
                                {
                                    tripSegment.IsCheckInWindow = false;
                                }
                                else
                                {                                    
                                    tripSegment.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("NewChangeSeatCheckinWindowMsg"));
                                    tripSegment.ContinueButtonText = Convert.ToString(_configuration.GetValue<string>("ChangeSeatContinueButtonText"));
                                }
                            }
                            else
                                tripSegment.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("CheckInWindowTextMessage"));
                        }
                        if (!_configuration.GetValue<bool>("DisableVIEWRESDecouplingSeatChangeIn24hrWindow"))
                        {
                            tripSegment.IsCheckedIn = _shoppingUtility.IsCheckedIn(segment);
                            if (tripSegment.IsCheckedIn)
                            {
                                tripSegment.IsAllPaxCheckedIn = _shoppingUtility.IsAllPaxCheckedIn(response, segment, tripSegment.IsCheckedIn); ;
                            }
                        }
                        tripSegment.SegmentIndex = segmentIndex;
                        tripSegment.OriginalSegmentNumber = segment.SegmentNumber;
                        tripSegment.ProductCode = GetCharactersticValue(segment.FlightSegment.Characteristic, "ProductCode");
                        tripSegment.IsIBE = _shoppingUtility.IsIBEFullFare(tripSegment.ProductCode);
                        tripSegment.FareBasisCode = ShopStaticUtility.GetFareBasisCode(response.Detail.Prices, segment.SegmentNumber);
                        tripSegment.BundleProductCode = isBundleSeatMap ? GetBundleCode(persistShoppingCart?.Products, segment.SegmentNumber) : string.Empty;
                        tripSegment.ShowSeatChange = ShowSeatChange(segment, response.Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentSeatChangeLinkFixEnabled);
                        tripSegments.Add(tripSegment);
                        segmentIndex++;
                    }
                }
            }

            if (isBundleSeatMap)
                tripSegments.RemoveAll(x => string.IsNullOrEmpty(x.BundleProductCode));
            return tripSegments;
        }

        private string GetBundleCode(List<ProdDetail> products, int segmentNumber)
        {
            return products?.FirstOrDefault(p => p?.Segments?.Any(s => s.SegmentId?.Split(',')?.Any(id => id == segmentNumber.ToString()) ?? false) ?? false)?.Code;
        }

        private List<Seat> PopulateSeatsCSL(Service.Presentation.ReservationModel.Reservation Detail, bool isMilesFOPEnabled)
        {
            List<Seat> seats = null;
            int seatKeyCounter = 0;
            if (Detail != null && Detail.FlightSegments != null && Detail.FlightSegments.Count > 0)
            {
                foreach (ReservationFlightSegment segment in Detail.FlightSegments)
                {
                    bool segmentPast = false;
                    if (Detail.Prices != null && Detail.Prices.Count > 0)
                    {
                        foreach (PriceFlightSegment pSegment in Detail.Prices[0].PriceFlightSegments)
                        {
                            if (pSegment.SegmentNumber == segment.SegmentNumber && pSegment.FlightStatuses != null && pSegment.FlightStatuses.Count > 0)
                            {
                                if (pSegment.FlightStatuses[0].Code.ToUpper() == "UA USED")
                                {
                                    segmentPast = true;
                                }
                            }
                        }
                    }

                    var segmentType = segment.FlightSegment.FlightSegmentType.Substring(0, 2);//per csl need to get first 2 characters 

                    if ((segmentType.ToUpper().Trim() == "HK" || segmentType.ToUpper().Trim() == "DK"
                         || segmentType.ToUpper().Trim() == "KL" || segmentType.ToUpper().Trim() == "RR"
                         || segmentType.ToUpper().Trim() == "TK") && !segmentPast)
                    {
                        for (int i = 0; i < Detail.Travelers.Count(); i++)
                        {
                            if (seats == null)
                            {
                                seats = new List<Seat>();
                            }
                            bool isGaugeChange = string.IsNullOrEmpty(segment.FlightSegment.IsChangeOfGauge) ? false : Convert.ToBoolean(segment.FlightSegment.IsChangeOfGauge);

                            if (isGaugeChange && segment.Legs != null && segment.Legs.Count > 0)
                            {
                                foreach (United.Service.Presentation.SegmentModel.PersonFlightSegment stopSegment in segment.Legs)
                                {
                                    // if (stopSegment.GaugeChanged)

                                    Seat seat = new Seat();
                                    seat.Key = seatKeyCounter;
                                    seatKeyCounter = seatKeyCounter + 1;
                                    seat.TravelerSharesIndex = Detail.Travelers[i].Person.Key;
                                    seat.Origin = stopSegment.DepartureAirport.IATACode;
                                    seat.FlightNumber = stopSegment.FlightNumber;
                                    seat.DepartureDate = stopSegment.DepartureDateTime;

                                    if (stopSegment.CurrentSeats != null)
                                    {
                                        foreach (PersonSeat currentseat in stopSegment.CurrentSeats)
                                        {
                                            if (currentseat.ReservationNameIndex.ToUpper().Trim() == seat.TravelerSharesIndex.ToUpper().Trim())
                                            {
                                                decimal price = 0;

                                                seat.OldSeatAssignment = string.IsNullOrEmpty(currentseat.Seat.Identifier) ? string.Empty : currentseat.Seat.Identifier.Replace("*", "");
                                                seat.OldSeatType = _configuration.GetValue<string>("DefaultSeatTypeIfNULL");
                                                seat.OldSeatProgramCode = GetCharactersticValue(currentseat.Seat.Characteristics, "ProgramCode");
                                                Decimal.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out price);
                                                seat.OldSeatPrice = price;

                                                seat.OldSeatCurrency = GetCharactersticValue(currentseat.Seat.Characteristics, "Currency");

                                                if (isMilesFOPEnabled && string.Equals(seat.OldSeatCurrency, "ML1", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    int oldSeatMiles = 0;
                                                    seat.OldSeatPrice = 0;
                                                    int.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out oldSeatMiles);
                                                    seat.OldSeatMiles = oldSeatMiles;
                                                    seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), false);
                                                }

                                                if (_shoppingUtility.IsMilesFOPEnabled())
                                                {
                                                    seat.OldSeatMiles = Convert.ToInt32(_configuration.GetValue<string>("milesFOP"));
                                                    seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false);
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(seat.OldSeatAssignment) && seat.OldSeatAssignment.StartsWith("0"))
                                    {
                                        seat.OldSeatAssignment = seat.OldSeatAssignment.TrimStart('0');
                                    }
                                    seat.Destination = stopSegment.ArrivalAirport.IATACode;
                                    seats.Add(seat);
                                }
                            }
                            else
                            {
                                //Bug 244886 :XML to CSL Migration-mApp: Lap Infant name is listed in the Seat map Travelers list and Unable to Change the seats for the adult with lap infant
                                if (Detail.Travelers[i].Person != null && !string.IsNullOrWhiteSpace(Detail.Travelers[i].Person.Type) && Detail.Travelers[i].Person.Type.ToUpper().Contains("INF"))
                                {
                                    continue;
                                }
                                Seat seat = new Seat();
                                seat.Key = seatKeyCounter;
                                seatKeyCounter = seatKeyCounter + 1;
                                seat.TravelerSharesIndex = Detail.Travelers[i].Person.Key;
                                seat.Origin = segment.FlightSegment.DepartureAirport.IATACode;
                                seat.FlightNumber = segment.FlightSegment.FlightNumber;
                                seat.DepartureDate = segment.FlightSegment.DepartureDateTime;
                                seat.OldSeatAssignment = string.Empty;
                                seat.OldSeatType = _configuration.GetValue<string>("DefaultSeatTypeIfNULL");

                                bool isCSLPNRSeatmap244678BugFixON = Convert.ToBoolean(_configuration.GetValue<string>("isCSLPNRSeatmap244678BugFixON") ?? "false");
                                if (segment.CurrentSeats != null && segment.CurrentSeats.Count > 0)
                                {
                                    foreach (PersonSeat currentseat in segment.CurrentSeats)
                                    {
                                        if (currentseat.ReservationNameIndex.ToUpper().Trim() == seat.TravelerSharesIndex.ToUpper().Trim())
                                        {
                                            decimal price = 0;
                                            seat.OldSeatAssignment = string.IsNullOrEmpty(currentseat.Seat.Identifier) ? string.Empty : currentseat.Seat.Identifier.Replace("*", "").ToUpper();
                                            seat.OldSeatType = _configuration.GetValue<string>("DefaultSeatTypeIfNULL");
                                            seat.OldSeatProgramCode = GetCharactersticValue(currentseat.Seat.Characteristics, "ProgramCode");
                                            Decimal.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out price);
                                            seat.OldSeatPrice = price;

                                            seat.OldSeatCurrency = GetCharactersticValue(currentseat.Seat.Characteristics, "Currency");

                                            if (isMilesFOPEnabled && string.Equals(seat.OldSeatCurrency, "ML1", StringComparison.OrdinalIgnoreCase))
                                            {
                                                int oldSeatMiles = 0;
                                                seat.OldSeatPrice = 0;                                                
                                                int.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out oldSeatMiles);
                                                seat.OldSeatMiles = oldSeatMiles;
                                                seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), false);
                                            }

                                            if (_shoppingUtility.IsMilesFOPEnabled())
                                            {
                                                seat.OldSeatMiles = Convert.ToInt32(_configuration.GetValue<string>("milesFOP"));
                                                seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false);
                                            }
                                            break;
                                        }
                                    }
                                }
                                // Bug-244678 - Added below condition to display the correct seats from legs for THRU flight - Vijayan
                                else if (isCSLPNRSeatmap244678BugFixON && segment.Legs != null && segment.Legs.Count > 0)
                                {
                                    foreach (United.Service.Presentation.SegmentModel.PersonFlightSegment stopSegment in segment.Legs)
                                    {
                                        if (stopSegment.CurrentSeats != null && stopSegment.CurrentSeats.Count > 0)
                                        {
                                            foreach (PersonSeat currentseat in stopSegment.CurrentSeats)
                                            {
                                                if (currentseat.ReservationNameIndex.ToUpper().Trim() == seat.TravelerSharesIndex.ToUpper().Trim())
                                                {
                                                    decimal price = 0;
                                                    seat.OldSeatAssignment = string.IsNullOrEmpty(currentseat.Seat.Identifier) ? string.Empty : currentseat.Seat.Identifier.Replace("*", "");
                                                    seat.OldSeatType = _configuration.GetValue<string>("DefaultSeatTypeIfNULL");
                                                    seat.OldSeatProgramCode = GetCharactersticValue(currentseat.Seat.Characteristics, "ProgramCode");
                                                    Decimal.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out price);
                                                    seat.OldSeatPrice = price;

                                                    seat.OldSeatCurrency = GetCharactersticValue(currentseat.Seat.Characteristics, "Currency");

                                                    if (isMilesFOPEnabled && string.Equals(seat.OldSeatCurrency, "ML1", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        int oldSeatMiles = 0;
                                                        seat.OldSeatPrice = 0;
                                                        int.TryParse(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), out oldSeatMiles);
                                                        seat.OldSeatMiles = oldSeatMiles;
                                                        seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(GetCharactersticValue(currentseat.Seat.Characteristics, "Price"), false);
                                                    }

                                                    if (_shoppingUtility.IsMilesFOPEnabled())
                                                    {
                                                        seat.OldSeatMiles = Convert.ToInt32(_configuration.GetValue<string>("milesFOP"));
                                                        seat.DisplayOldSeatMiles = ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (segment.Seat != null)
                                {
                                    seat.OldSeatAssignment = string.IsNullOrEmpty(segment.Seat.Identifier) ? string.Empty : segment.Seat.Identifier.Replace("*", "");
                                    seat.OldSeatType = _configuration.GetValue<string>("DefaultSeatTypeIfNULL");
                                }

                                if (!string.IsNullOrEmpty(seat.OldSeatAssignment) && seat.OldSeatAssignment.StartsWith("0"))
                                {
                                    seat.OldSeatAssignment = seat.OldSeatAssignment.TrimStart('0');
                                }

                                seat.Destination = segment.FlightSegment.ArrivalAirport.IATACode;

                                seats.Add(seat);
                            }

                        }
                    }
                }
            }
            return seats;
        }

        private List<MOBBKTrip> PopulateFlightAddTripResponseCSL(Service.Presentation.ReservationModel.Reservation Detail, bool unETicketedCheckInPNRSeatChangeEnabled, ref bool isVerticalSeatMapEnabled, bool isETicketed, bool checkInSegmentSeatChangeFixEnabled, bool checkInSegmentSeatChangeLinkFixEnabled, string appversion, int appId = 0, bool isBundleSeatMap = false, MOBShoppingCart persistShoppingCart = null)
        {
            if (_configuration.GetValue<bool>("TurnOffChangeSeatsTripNumberFixToHandleFlownSegments"))
            {
                return PopulateFlightAddTripResponseCSL_OLD(Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, appversion, appId);
            }

            List<MOBBKTrip> selectedTrips = new List<MOBBKTrip>();

            //int totaltrips = Detail.FlightSegments.Select(o => o.TripNumber).Distinct().Count();
            var tripNumbers = Detail.FlightSegments.Select(o => Convert.ToInt32(o.TripNumber)).Distinct();

            //MOBILE-39120 Check if all the flight segments have been checked in or checkin triggered or checkin eligible or departed and if true, reset VerticalSeatmap flag to false to naviate to multisegment screen.
            if (isVerticalSeatMapEnabled && checkInSegmentSeatChangeFixEnabled && CheckIfAllSegmentsCheckedInOrDeparted(Detail, isETicketed))
            {
                isVerticalSeatMapEnabled = false;
            }

            //for (int i = 1; i <= totaltrips; i++)
            foreach (var i in tripNumbers)
            {
                MOBBKTrip selectedTrip = new MOBBKTrip();

                selectedTrip.FlattenedFlights = new List<MOBBKFlattenedFlight>();

                var totalTripSegments = Detail.FlightSegments.Where(o => o.TripNumber == i.ToString());

                foreach (United.Service.Presentation.SegmentModel.ReservationFlightSegment segment in Detail.FlightSegments)
                {
                    MOBBKFlattenedFlight flattenedFlight = new MOBBKFlattenedFlight();
                    flattenedFlight.Flights = new List<MOBBKFlight>();
                    flattenedFlight.FlightId = segment.SegmentNumber.ToString();
                    flattenedFlight.TripId = segment.TripNumber;
                    if (isBundleSeatMap && string.IsNullOrEmpty(GetBundleCode(persistShoppingCart?.Products, segment.SegmentNumber)))
                        continue;
                    bool isGaugeChange = string.IsNullOrEmpty(segment.FlightSegment.IsChangeOfGauge) ? false : Convert.ToBoolean(segment.FlightSegment.IsChangeOfGauge);

                    if (!string.IsNullOrEmpty(segment.TripNumber) && Convert.ToInt32(segment.TripNumber) == i)
                    {
                        if (isGaugeChange && segment.Legs != null && segment.Legs.Count > 0) // for gauge changes
                        {
                            foreach (PersonFlightSegment seg in segment.Legs)
                            {
                                MOBBKFlight Legflight = PopulateFlightAvailabilityStopSegments(seg, segment, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, appversion, appId);

                                if (Legflight != null)
                                {
                                    Legflight.ShowSeatChange = ShowSeatChange(segment, Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentSeatChangeLinkFixEnabled);
                                    //Legflight.ChangeOfGauge = isGaugeChange;
                                    Legflight.FareBasisCode = ShopStaticUtility.GetFareBasisCode(Detail.Prices, segment.SegmentNumber);
                                    flattenedFlight.Flights.Add(Legflight);
                                }
                            }
                        }
                        else
                        {
                            MOBBKFlight flight = PopulateFlightAvailabilitySegmentsCSL(Detail, segment, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, appversion, appId);
                            if (flight != null)
                            {
                                flight.ShowSeatChange = ShowSeatChange(segment, Detail, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, isVerticalSeatMapEnabled, checkInSegmentSeatChangeLinkFixEnabled);
                                flattenedFlight.Flights.Add(flight);
                            }
                        }

                        selectedTrip.FlattenedFlights.Add(flattenedFlight);

                        if (segment.SegmentNumber == totalTripSegments.Min(x => x.SegmentNumber))
                        {
                            selectedTrip.Origin = segment.FlightSegment.DepartureAirport.IATACode;
                        }

                        if (segment.SegmentNumber == totalTripSegments.Max(x => x.SegmentNumber))
                        {
                            selectedTrip.Destination = segment.FlightSegment.ArrivalAirport.IATACode;
                        }
                    }
                }

                if (checkInSegmentSeatChangeFixEnabled)
                {
                    if (isVerticalSeatMapEnabled && selectedTrip != null && selectedTrip.FlattenedFlights != null && selectedTrip.FlattenedFlights.Count > 0)
                    {
                        foreach (MOBBKFlattenedFlight ff in selectedTrip.FlattenedFlights)
                        {
                            if (ff.Flights != null)
                            {
                                ff.Flights.RemoveAll(f => !f.ShowSeatChange);
                            }
                        }

                        selectedTrip.FlattenedFlights.RemoveAll(ff => ff.Flights == null || ff.Flights.Count == 0);
                    }
                }
                else
                {
                    if (isVerticalSeatMapEnabled && selectedTrip != null && selectedTrip.FlattenedFlights != null && selectedTrip.FlattenedFlights.Count > 0
                        && selectedTrip.FlattenedFlights[0].Flights.Any(f => !f.ShowSeatChange))
                    {
                        continue;
                    }
                }

                selectedTrips.Add(selectedTrip);
            }

            return selectedTrips;
        }

        private bool CheckIfAllSegmentsCheckedInOrDeparted(Service.Presentation.ReservationModel.Reservation Detail, bool isETicketed)
        {
            return (Detail.FlightSegments.All(segment =>
                               (!string.IsNullOrEmpty(GetCharactersticValue(segment.Characteristic, "CheckedIn")) && Convert.ToBoolean(GetCharactersticValue(segment.Characteristic, "CheckedIn")))
                           || (!string.IsNullOrEmpty(GetCharactersticValue(segment.Characteristic, "CheckinTriggered")) && Convert.ToBoolean(GetCharactersticValue(segment.Characteristic, "CheckinTriggered")) && isETicketed)
                           || TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(segment.EstimatedDepartureTime)) < DateTime.Now.ToUniversalTime()));
        }        

        private MOBBKFlight PopulateFlightAvailabilityStopSegments
          (PersonFlightSegment leg, United.Service.Presentation.SegmentModel.ReservationFlightSegment segment, bool unETicketedCheckInPNRSeatChangeEnabled, bool isETicketed, string appVersion = "", int appId = 0)
        {
            string airportName = string.Empty;
            string cityName = string.Empty;
            MOBBKFlight flight = new MOBBKFlight();
            if (leg != null)
            {
                flight.FlightId = segment.SegmentNumber.ToString();
                flight.FlightNumber = leg.FlightNumber;

                flight.DepartureDateTime = leg.DepartureDateTime;
                flight.DepartTime = FormatTime(leg.DepartureDateTime);
                flight.DepartDate = FormatDate(leg.DepartureDateTime);
                flight.Origin = leg.DepartureAirport.IATACode;
                _shoppingUtility.GetAirportCityName(flight.Origin, ref airportName, ref cityName);
                flight.OriginDescription = airportName;

                flight.DestinationDate = FormatDate(leg.ArrivalDateTime);
                flight.DestinationTime = FormatTime(leg.ArrivalDateTime);
                flight.Destination = leg.ArrivalAirport.IATACode;
                _shoppingUtility.GetAirportCityName(flight.Destination, ref airportName, ref cityName);
                flight.DestinationDescription = airportName;

                if (leg.BookingClasses != null && leg.BookingClasses.Count > 0)                
                {
                    flight.ServiceClass = leg.BookingClasses[0].Code;
                    flight.ServiceClassDescription = leg.BookingClasses[0].Cabin.Name;
                }
                else if (segment.BookingClass != null)
                {
                    flight.ServiceClass = segment.BookingClass.Cabin.Description;
                    flight.ServiceClassDescription = segment.BookingClass.Cabin.Name;
                }

                if (leg.MarketedFlightSegment != null && leg.MarketedFlightSegment.Count > 0)
                {
                    flight.MarketingCarrier = leg.MarketedFlightSegment[0].MarketingAirlineCode;
                }
                else if (segment.FlightSegment != null && segment.FlightSegment.MarketedFlightSegment != null && segment.FlightSegment.MarketedFlightSegment.Any())
                {
                    flight.MarketingCarrier = segment.FlightSegment.MarketedFlightSegment[0].MarketingAirlineCode;
                }

                flight.OperatingCarrier = leg.OperatingAirlineCode;
                flight.OperatingCarrierDescription = leg.OperatingAirlineName;
                //string ischeckinwindow = GetCharactersticValue(leg.Characteristic, "CheckinTriggered");
                //flight.IsCheckInWindow = string.IsNullOrEmpty(ischeckinwindow) ? false : Convert.ToBoolean(ischeckinwindow);
                //bug id 245559 code changes mApp: View Reservation- 'Seat map not available at this time.' message displayed when we tap on 9B segment in the seat map segments screen, DOTCOM message is different
                if (!_configuration.GetValue<bool>("EnableOAMsgUpdateFix") && !ShowSeatMapForCarriers(flight.OperatingCarrier) && !_shoppingUtility.IsSeatMapSupportedOa(flight.OperatingCarrier, flight.MarketingCarrier) && _configuration.GetValue<bool>("BugFixToggleFor18E"))
                {
                    flight.IsCheckInWindow = true;
                    flight.CheckInWindowText = _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines").ToString();
                }
                else if (IsInChecKInWindow(segment.EstimatedDepartureUTCTime))
                {
                    flight.IsCheckInWindow = true;
                    if (_shoppingUtility.EnableNewChangeSeatCheckinWindowMsg(appId, appVersion))
                    {
                        if (unETicketedCheckInPNRSeatChangeEnabled && !isETicketed)
                        {
                            flight.IsCheckInWindow = false;
                        }
                        else
                        {
                            flight.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("NewChangeSeatCheckinWindowMsg"));
                            flight.ContinueButtonText = Convert.ToString(_configuration.GetValue<string>("ChangeSeatContinueButtonText"));
                        }
                    }
                    else
                        flight.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("CheckInWindowTextMessage"));
                }

                flight.Meal = GetCharactersticValue(leg.Characteristic, "MealDescription");

                if (_configuration.GetValue<bool>("EnableMRCOGSeatMapFix"))
                {
                    if (leg.Equipment != null && leg.Equipment.Model != null)
                    {
                        flight.EquipmentDisclosures = PopulateEquipmentDisclosures(leg.Equipment.Model.Fleet, leg.Equipment.Model.Description, false, false, false, false);
                    }
                }

                flight.GaugeChanges = PopulateGaugeChangeCSL(leg.Equipment, leg.ArrivalAirport);
                flight.ProductCode = GetCharactersticValue(segment.FlightSegment.Characteristic, "ProductCode");
                flight.IsIBE = _shoppingUtility.IsIBEFullFare(flight.ProductCode);
            }

            return flight;
        }
        private List<MOBBKTrip> PopulateFlightAddTripResponseCSL_OLD(Service.Presentation.ReservationModel.Reservation Detail, bool unETicketedCheckInPNRSeatChangeEnabled, bool isETicketed, string appversion, int appId = 0)
        {
            List<MOBBKTrip> selectedTrips = new List<MOBBKTrip>();

            int totaltrips = Detail.FlightSegments.Select(o => o.TripNumber).Distinct().Count();

            for (int i = 1; i <= totaltrips; i++)
            {
                MOBBKTrip selectedTrip = new MOBBKTrip();

                selectedTrip.FlattenedFlights = new List<MOBBKFlattenedFlight>();

                var totalTripSegments = Detail.FlightSegments.Where(o => o.TripNumber == i.ToString());

                foreach (ReservationFlightSegment segment in Detail.FlightSegments)
                {
                    MOBBKFlattenedFlight flattenedFlight = new MOBBKFlattenedFlight();
                    flattenedFlight.Flights = new List<MOBBKFlight>();
                    flattenedFlight.FlightId = segment.SegmentNumber.ToString();
                    flattenedFlight.TripId = segment.TripNumber;
                    bool isGaugeChange = string.IsNullOrEmpty(segment.FlightSegment.IsChangeOfGauge) ? false : Convert.ToBoolean(segment.FlightSegment.IsChangeOfGauge);

                    if (!string.IsNullOrEmpty(segment.TripNumber) && Convert.ToInt32(segment.TripNumber) == i)
                    {
                        if (isGaugeChange && segment.Legs != null && segment.Legs.Count > 0) // for gauge changes
                        {
                            foreach (PersonFlightSegment seg in segment.Legs)
                            {
                                MOBBKFlight Legflight = PopulateFlightAvailabilityStopSegments(seg, segment, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, appversion, appId);

                                if (Legflight != null)
                                {
                                    //Legflight.ChangeOfGauge = isGaugeChange;
                                    Legflight.FareBasisCode = ShopStaticUtility.GetFareBasisCode(Detail.Prices, segment.SegmentNumber);
                                    flattenedFlight.Flights.Add(Legflight);
                                }
                            }
                        }
                        else
                        {
                            MOBBKFlight flight = PopulateFlightAvailabilitySegmentsCSL(Detail, segment, unETicketedCheckInPNRSeatChangeEnabled, isETicketed, appversion, appId);
                            if (flight != null)
                            {
                                flattenedFlight.Flights.Add(flight);
                            }

                        }

                        selectedTrip.FlattenedFlights.Add(flattenedFlight);

                        if (segment.SegmentNumber == totalTripSegments.Min(x => x.SegmentNumber))
                        {
                            selectedTrip.Origin = segment.FlightSegment.DepartureAirport.IATACode;
                        }

                        if (segment.SegmentNumber == totalTripSegments.Max(x => x.SegmentNumber))
                        {
                            selectedTrip.Destination = segment.FlightSegment.ArrivalAirport.IATACode;
                        }
                    }
                }

                selectedTrips.Add(selectedTrip);
            }

            return selectedTrips;
        }

        private List<MOBBKGaugeChange> PopulateGaugeChangeCSL(Service.Presentation.CommonModel.AircraftModel.Aircraft eqipment, Service.Presentation.CommonModel.AirportModel.Airport legArrival)
        {
            List<MOBBKGaugeChange> bookingGuageChanges = null;

            if (eqipment != null && eqipment.Model != null)
            {
                bookingGuageChanges = new List<MOBBKGaugeChange>();

                MOBBKGaugeChange gaugeChange = new MOBBKGaugeChange();
                gaugeChange.DecodedDestination = legArrival.IATACountryCode.Name;
                gaugeChange.Destination = legArrival.IATACode;
                gaugeChange.Equipment = eqipment.Model.Fleet;
                gaugeChange.EquipmentDescription = eqipment.Model.Description;
                bookingGuageChanges.Add(gaugeChange);
            }

            return bookingGuageChanges;
        }

        private MOBBKFlight PopulateFlightAvailabilitySegmentsCSL
            (Service.Presentation.ReservationModel.Reservation detail, ReservationFlightSegment segment, bool unETicketedCheckInPNRSeatChangeEnabled, bool isETicketed, string appversion, int appId = 0)
        {
            bool segmentPast = false;
            string airportName = string.Empty;
            string cityName = string.Empty;
            MOBBKFlight flight = null;
            //Bug 208440 - detail.Prices.Count check added for below condition - j.Srinivas
            if (detail != null && detail.Prices != null && detail.Prices.Count > 0 && detail.Prices[0] != null && detail.Prices[0].PriceFlightSegments != null)
            {
                foreach (PriceFlightSegment pSegment in detail.Prices[0].PriceFlightSegments)
                {
                    if (pSegment.SegmentNumber == segment.SegmentNumber && pSegment.FlightStatuses != null && pSegment.FlightStatuses.Count > 0)
                    {
                        if (pSegment.FlightStatuses[0].Code.ToUpper() == "UA USED")
                        {
                            segmentPast = true;
                        }
                    }
                }
            }
            //Bug 207261: iOS-ViewRes flow: App got crashed, when we tap on change seats for two segments(UA+Code share) PNR in Viewres screen - kirti
            if (segment != null && segment.FlightSegment != null)
            {
                var segmentTyep = segment.FlightSegment.FlightSegmentType.Substring(0, 2);

                if ((segmentTyep.ToUpper().Trim() == "HK" || segmentTyep.ToUpper().Trim() == "DK"
                    || segmentTyep.ToUpper().Trim() == "KL" || segmentTyep.ToUpper().Trim() == "RR"
                    || segmentTyep.ToUpper().Trim() == "TK") && !segmentPast)
                {
                    flight = new MOBBKFlight();
                    flight.FlightId = segment.FlightSegment.SegmentNumber.ToString();
                    flight.FlightNumber = segment.FlightSegment.FlightNumber;

                    flight.DepartureDateTime = segment.FlightSegment.DepartureDateTime;
                    flight.DepartTime = FormatTime(segment.FlightSegment.DepartureDateTime);
                    flight.DepartDate = FormatDate(segment.FlightSegment.DepartureDateTime);
                    flight.Origin = segment.FlightSegment.DepartureAirport.IATACode;
                    _shoppingUtility.GetAirportCityName(flight.Origin, ref airportName, ref cityName);
                    flight.OriginDescription = airportName;

                    flight.DestinationDate = FormatDate(segment.FlightSegment.ArrivalDateTime);
                    flight.DestinationTime = FormatTime(segment.FlightSegment.ArrivalDateTime);
                    flight.Destination = segment.FlightSegment.ArrivalAirport.IATACode;
                    _shoppingUtility.GetAirportCityName(flight.Destination, ref airportName, ref cityName);
                    flight.DestinationDescription = airportName;

                    if (segment.FlightSegment.BookingClasses != null && segment.FlightSegment.BookingClasses.Count > 0)
                    {
                        flight.ServiceClass = segment.FlightSegment.BookingClasses[0].Code;
                        flight.ServiceClassDescription = segment.FlightSegment.BookingClasses[0].Cabin.Name;
                    }

                    if (segment.FlightSegment.MarketedFlightSegment != null && segment.FlightSegment.MarketedFlightSegment.Count > 0)
                    {
                        flight.MarketingCarrier = segment.FlightSegment.MarketedFlightSegment[0].MarketingAirlineCode;
                    }

                    flight.OperatingCarrier = segment.FlightSegment.OperatingAirlineCode;
                    flight.OperatingCarrierDescription = segment.FlightSegment.OperatingAirlineName;
                    flight.TotalTravelTime = GetTravelTime(appversion, segment);

                    flight.ChangeOfGauge = string.IsNullOrEmpty(segment.FlightSegment.IsChangeOfGauge) ? false : Convert.ToBoolean(segment.FlightSegment.IsChangeOfGauge);
                    //string ischeckinwindow = GetCharactersticValue(segment.FlightSegment.Characteristic, "CheckinTriggered");
                    //flight.IsCheckInWindow = string.IsNullOrEmpty(ischeckinwindow) ? false : Convert.ToBoolean(ischeckinwindow);
                    //bug id 245559 code changes mApp: View Reservation- 'Seat map not available at this time.' message displayed when we tap on 9B segment in the seat map segments screen, DOTCOM message is different
                    if (!_configuration.GetValue<bool>("EnableOAMsgUpdateFix") && !ShowSeatMapForCarriers(flight.OperatingCarrier) && !_shoppingUtility.IsSeatMapSupportedOa(flight.OperatingCarrier, flight.MarketingCarrier) && _configuration.GetValue<bool>("BugFixToggleFor18E"))
                    {
                        flight.IsCheckInWindow = true;
                        flight.CheckInWindowText = _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines").ToString();
                    }
                    else if (IsInChecKInWindow(segment.EstimatedDepartureUTCTime))
                    {
                        flight.IsCheckInWindow = true;
                        if (_shoppingUtility.EnableNewChangeSeatCheckinWindowMsg(appId, appversion))
                        {
                            if (unETicketedCheckInPNRSeatChangeEnabled && !isETicketed)
                            {
                                flight.IsCheckInWindow = false;
                            }
                            else
                            {
                                flight.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("NewChangeSeatCheckinWindowMsg"));
                                flight.ContinueButtonText = Convert.ToString(_configuration.GetValue<string>("ChangeSeatContinueButtonText"));
                            }
                        }
                        else
                            flight.CheckInWindowText = Convert.ToString(_configuration.GetValue<string>("CheckInWindowTextMessage"));
                    }

                    flight.Meal = GetCharactersticValue(segment.Characteristic, "MealDescription");
                    if (segment.FlightSegment.Equipment != null && segment.FlightSegment.Equipment.Model != null)
                    {
                        flight.EquipmentDisclosures = PopulateEquipmentDisclosures(segment.FlightSegment.Equipment.Model.Fleet, segment.FlightSegment.Equipment.Model.Description, false, false, false, false);
                    }
                    flight.ProductCode = GetCharactersticValue(segment.FlightSegment.Characteristic, "ProductCode");
                    flight.IsIBE = _shoppingUtility.IsIBEFullFare(flight.ProductCode);
                    flight.FareBasisCode = ShopStaticUtility.GetFareBasisCode(detail.Prices, segment.FlightSegment.SegmentNumber);
                }
            }

            return flight;
        }

        private string GetTravelTime(string appVersion, ReservationFlightSegment segment)
        {
            string flightTime = string.Empty;

            if (segment.FlightSegment.JourneyDuration.Hours > 0)
            {
                if (!string.IsNullOrEmpty(appVersion) && appVersion.ToUpper().Equals("2.1.8I"))
                {
                    flightTime = "0 HR " + segment.FlightSegment.JourneyDuration.Hours;
                }
                else if (segment.FlightSegment.JourneyDuration.Hours > 0) // Madhavi added this condition not to dispaly 0H 
                {
                    flightTime = segment.FlightSegment.JourneyDuration.Hours + " HR";
                }
            }
            if (segment.FlightSegment.JourneyDuration.Minutes > 0)
            {
                if (!string.IsNullOrEmpty(appVersion) && appVersion.ToUpper().Equals("2.1.8I"))
                {
                    flightTime = flightTime + " " + segment.FlightSegment.JourneyDuration.Minutes +
                                            " 0 MN";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(flightTime))
                    {
                        flightTime = segment.FlightSegment.JourneyDuration.Minutes + " MN";
                    }
                    else
                    {
                        flightTime = flightTime + " " + segment.FlightSegment.JourneyDuration.Minutes + " MN";
                    }
                }
            }
            return flightTime;
        }

        public bool ShowSeatMapForCarriers(string operatingCarrier)
        {
            if (_configuration.GetValue<string>("ShowSeatMapAirlines") != null)
            {
                string[] carriers = _configuration.GetValue<string>("ShowSeatMapAirlines").Split(',');
                foreach (string carrier in carriers)
                {
                    if (operatingCarrier != null && carrier.ToUpper().Trim().Equals(operatingCarrier.ToUpper().Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsInChecKInWindow(string departTimeString)
        {
            DateTime departDateTime;
            DateTime.TryParse(departTimeString, out departDateTime);
            return departDateTime > DateTime.UtcNow && departDateTime < DateTime.UtcNow.AddHours(24);
        }

        public MOBBKEquipmentDisclosure PopulateEquipmentDisclosures(string equipmentType, string equipmentDescription, bool isSingleCabin, bool noBoardingAssistance, bool nonJetEquipment, bool wheelchairsNotAllowed)
        {
            MOBBKEquipmentDisclosure equipmentDisclosure = new MOBBKEquipmentDisclosure();
            equipmentDisclosure.EquipmentType = equipmentType;
            equipmentDisclosure.EquipmentDescription = equipmentDescription;
            equipmentDisclosure.IsSingleCabin = isSingleCabin;
            equipmentDisclosure.NoBoardingAssistance = noBoardingAssistance;
            equipmentDisclosure.NonJetEquipment = nonJetEquipment;
            equipmentDisclosure.WheelchairsNotAllowed = wheelchairsNotAllowed;

            return equipmentDisclosure;
        }

        public string GetCharactersticValue(System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic> Characteristic, string code)
        {
            string value = string.Empty;

            if (Characteristic != null && Characteristic.Count > 0)
            {
                try
                {
                    for (int i = 0; i < Characteristic.Count; i++)
                    {
                        if (Characteristic[i] != null && Characteristic[i].Code != null && Characteristic[i].Code.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                        {
                            value = Characteristic[i].Value;

                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    value = string.Empty;
                }
            }

            return value;
        }

        private string FormatDate(string dateTimeString)
        {
            string result = string.Empty;

            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(dateTimeString, out dateTime);
            if (dateTime.Ticks != 0)
            {
                if (dateTime.ToString("MMMM").Length > 3)
                {
                    result = string.Format("{0:ddd., MMM. d, yyyy}", dateTime);
                }
                else
                {
                    result = string.Format("{0:ddd., MMM d, yyyy}", dateTime);
                }
            }

            return result;
        }

        private string FormatTime(string dateTimeString)
        {
            string result = string.Empty;

            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(dateTimeString, out dateTime);
            if (dateTime.Ticks != 0)
            {
                result = dateTime.ToString("h:mm tt");
            }

            return result;
        }

        public async Task<(string jsonResponse, string token)> GetPnrDetailsFromCSL(string transactionId, string recordLocator, string lastName, int applicationId, string appVersion, string actionName, string token, bool usedRecall = false)
        {
            var request = new RetrievePNRSummaryRequest();

            if (!usedRecall)
            {
                request.Channel = _configuration.GetValue<string>("ChannelName");
                request.IsIncludeETicketSDS = _configuration.GetValue<string>("IsIncludeETicketSDS");
                request.IsIncludeFlightRange = _configuration.GetValue<string>("IsIncludeFlightRange");
                request.IsIncludeFlightStatus = _configuration.GetValue<string>("IsIncludeFlightStatus");
                request.IncludeManageResDetails = _configuration.GetValue<string>("IncludeManageResDetails");
                request.IsUpgradeDetails = _configuration.GetValue<string>("IsUpgradeDetails");
                if (_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes"))
                {
                    request.IsUpgradeDetailsWithEMD = _configuration.GetValue<string>("IsUpgradeDetailsWithEMD");
                }
                request.IsIncludePNRChangeEligibility = _configuration.GetValue<string>("IsIncludePNRChangeEligibility");
                request.IsIncludeLMX = _configuration.GetValue<string>("IsIncludeLMX");
                request.IsIncludePNRDB = _configuration.GetValue<string>("IsIncludePNRDB");
                request.IsIncludeSegmentDuration = _configuration.GetValue<string>("IsIncludeSegmentDuration");
                request.ConfirmationID = recordLocator.ToUpper();
                request.LastName = lastName;
                request.PNRType = string.Empty; //per csl to get the data from cache use PNRType=”CACHED”
                request.FilterHours = _configuration.GetValue<string>("FilterHours");
                request.IsIncludeChangeFee = _configuration.GetValue<bool>("IsIncludeChangeFee");
                if (_configuration.GetValue<bool>("IsIncludeTravelWaiverDetail"))
                {
                    request.IsIncludeTravelWaiverDetail = _configuration.GetValue<bool>("IsIncludeTravelWaiverDetail");
                }
            }
            else
            {
                request.Channel = _configuration.GetValue<string>("ChannelName");
                request.IsIncludeETicketSDS = _configuration.GetValue<string>("IsIncludeETicketSDS");
                request.IsIncludeFlightRange = _configuration.GetValue<string>("IsIncludeFlightRange");
                request.IsIncludeFlightStatus = _configuration.GetValue<string>("IsIncludeFlightStatus");
                request.IsIncludeLMX = _configuration.GetValue<string>("IsIncludeLMX");
                request.IsIncludePNRDB = _configuration.GetValue<string>("IsIncludePNRDB");
                request.IsIncludeSegmentDuration = _configuration.GetValue<string>("IsIncludeSegmentDuration");
                request.ConfirmationID = recordLocator.ToUpper();
                request.LastName = lastName;
                request.PNRType = string.Empty; //per csl to get the data from cache use PNRType=”CACHED”                
            }

            var jsonResponse = await RetrievePnrDetailsFromCsl(applicationId, transactionId, request);
            return (jsonResponse, token);
        }

        private async Task<string> RetrievePnrDetailsFromCsl(int applicationId, string TransactionId, RetrievePNRSummaryRequest request)
        {
            string sessionId = null;

            var jsonRequest = System.Text.Json.JsonSerializer.Serialize<RetrievePNRSummaryRequest>(request);

            string token = await _dPService.GetAnonymousToken(applicationId, _headers.ContextValues.DeviceId, _configuration);

            var jsonResponse = string.Empty;

            jsonResponse = await _pNRRetrievalService.PNRRetrieval(token, jsonRequest, sessionId, "/PNRRetrieval");

            return jsonResponse;
        }

        public void CheckSegmentToRaiseExceptionForElf(List<TripSegment> segments)
        {
            if (segments != null && segments.Any())
            {
                var elfSegments = segments.Where(p => p.IsELF);
                if (elfSegments.Count() == segments.Count)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("ELFManageResAdvisoryMsg"));
                }
            }
        }

        public bool HasEconomySegment(List<MOBBKTrip> trips)
        {
            bool result = false;

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    if (trip.FlattenedFlights != null)
                    {
                        foreach (var ff in trip.FlattenedFlights)
                        {
                            if (ff.Flights != null)
                            {
                                foreach (var flight in ff.Flights)
                                {
                                    if (!string.IsNullOrEmpty(flight.ServiceClassDescription) &&
                                        (flight.ServiceClassDescription.ToUpper().Trim().Equals("ECONOMY") ||
                                         flight.ServiceClassDescription.ToUpper().Trim().Equals("UNITED ECONOMY") ||
                                         flight.ServiceClassDescription.ToUpper().Trim().Equals("COACH")
                                        ))
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private string GetEplusSubscriptionMessage(string travelerName, int subscribeCompanionCount, string regionType)
        {
            string ePLUSMessage = string.Empty;
            var regionInfo = !string.IsNullOrEmpty(regionType) && !regionType.Equals("GLOBAL", StringComparison.OrdinalIgnoreCase) ? "(" + regionType + " flights)" : string.Empty;

            if (subscribeCompanionCount == 1)
            {
                ePLUSMessage = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForGeneralEplusRegionWithOneCompanionManageRes"), travelerName, regionInfo);
            }
            else
            {
                ePLUSMessage = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForGeneralEplusRegionWithZeroCompanionManageRes"), travelerName, regionInfo);
            }

            return ePLUSMessage;
        }

        public async Task<(int, MOBSeatChangeInitializeResponse response, int ePlusSubscriberCount)> PopulateEPlusSubscriberSeatMessage(MOBSeatChangeInitializeResponse response, int applicationID, string sessionID, int ePlusSubscriberCount, bool isVerticalSeatMapEnabled, bool isEnablePreferredZoneSubscriptionMessages = false)
        {
            // Booking and View Res only Care about Gold Level Status for Free EPlus Seats because silver will get only on Check In path.
            int goldMemberCount = 0;
            int silverMemberCount = 0;
            int subscribeCompanionCount = 0;
            int totalEPlusCompanionsEligible = 0;
            int totalSubscribeCompanionCount = 0;
            bool hasEliteAboveGold = false;
            string ePlusSubscriptionMessage = string.Empty;
            List<string> ePlusSubscriberNames = new List<string>();
            bool isOneOfIsGlobalSubscriber = false;
            string ePlusMsgTitle = string.Empty;
            string ePlusMsg = string.Empty;
            bool showEPUSubscriptionMessage = false;
            Dictionary<string, int> subscribeRegions = new Dictionary<string, int>();
            string regionType = string.Empty, region = string.Empty;

            if (response != null && response.BookingTravlerInfo != null && response.BookingTravlerInfo.Count > 0)
            {
                try
                {
                    #region
                    StringBuilder names = new StringBuilder();
                    foreach (var traveler in response.BookingTravlerInfo)
                    {
                        #region Getting Traveler Details based on elite level
                        if (traveler.LoyaltyProgramProfile != null)
                        {
                            string travelerName = traveler.Person.GivenName + " " + traveler.Person.Surname;
                            if (traveler.LoyaltyProgramProfile.MPEliteLevel == 1)
                            {
                                silverMemberCount++;
                                travelerName = names.Length != 0 ? (", " + travelerName) : travelerName;
                                names.Append(travelerName);
                            }
                            else if (traveler.LoyaltyProgramProfile.MPEliteLevel == 2)
                            {
                                goldMemberCount++;
                                travelerName = names.Length != 0 ? (", " + travelerName) : travelerName;
                                names.Append(travelerName);
                            }
                            else if (traveler.LoyaltyProgramProfile.MPEliteLevel > 2)
                            {
                                hasEliteAboveGold = true;
                                break;
                            }

                            if (traveler.LoyaltyProgramProfile.CarrierCode == "UA")
                            {
                                MOBUASubscriptions objUASubscriptions = null;
                                bool isEPlusSubscriber = false;
                                var tupleResponse = await GetEPlusSubscriptionMessage(traveler.LoyaltyProgramProfile.MemberId, applicationID, sessionID, traveler.Person.GivenName + " " + traveler.Person.Surname, objUASubscriptions, isEPlusSubscriber, isOneOfIsGlobalSubscriber, subscribeCompanionCount, regionType, isEnablePreferredZoneSubscriptionMessages);
                                ePlusSubscriptionMessage = ePlusSubscriptionMessage + " " + tupleResponse.Item1.Trim();
                                objUASubscriptions = tupleResponse.objUASubscriptions;
                                isEPlusSubscriber = tupleResponse.isEPlusSubscriber;
                                isOneOfIsGlobalSubscriber = tupleResponse.isOneOfIsGlobalSubscriber;
                                subscribeCompanionCount = tupleResponse.subscribeCompanionCount;
                                regionType = tupleResponse.regionType;

                                //traveler.EPlusSubscriptions = objUASubscriptions;
                                traveler.IsEPlusSubscriber = isEPlusSubscriber;
                                if (isEPlusSubscriber)
                                {
                                    //Calculate total number of EPlus Companions eligible to get free Eplus Seats
                                    totalEPlusCompanionsEligible = totalEPlusCompanionsEligible + 1;
                                    totalEPlusCompanionsEligible = totalEPlusCompanionsEligible + subscribeCompanionCount;

                                    ePlusSubscriberNames.Add(traveler.Person.GivenName + " " + traveler.Person.Surname);
                                    ePlusSubscriberCount = ePlusSubscriberCount + 1;

                                    totalSubscribeCompanionCount += subscribeCompanionCount;

                                    if (subscribeRegions.Count == 0)
                                    {
                                        subscribeRegions.Add(regionType.ToUpper(), subscribeCompanionCount);
                                        region = regionType;
                                    }
                                    else
                                    {
                                        if (subscribeRegions.ContainsKey(regionType.ToUpper()))
                                        {
                                            subscribeRegions[regionType.ToUpper()] += subscribeCompanionCount;
                                        }
                                        else subscribeRegions.Add(regionType.ToUpper(), subscribeCompanionCount);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    if (!hasEliteAboveGold && (ePlusSubscriberCount > 0 || goldMemberCount > 0 || silverMemberCount > 0))
                    {
                        showEPUSubscriptionMessage = await ShowEPUSubscriptionMessage(response.BookingTravlerInfo, response.SelectedTrips);
                    }

                    if (isEnablePreferredZoneSubscriptionMessages && showEPUSubscriptionMessage && !hasEliteAboveGold)
                    {
                        #region
                        string _eplusSubscriberNames = string.Empty;
                        if (names.Length > 0)
                        {
                            _eplusSubscriberNames = names.ToString().Split(',')[0].Trim();
                        }
                        else
                        {
                            foreach (string eachName in ePlusSubscriberNames)
                            {
                                _eplusSubscriberNames = string.IsNullOrEmpty(_eplusSubscriberNames) ? eachName : _eplusSubscriberNames;
                                break;
                            }
                        }

                        if (goldMemberCount > 0)
                        {
                            ePlusMsg = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForGoldManageRes"), _eplusSubscriberNames);
                        }
                        else if (silverMemberCount > 0)
                        {
                            if ((subscribeRegions != null && subscribeRegions.Any() && subscribeRegions.Keys.ToList()[0].ToUpper() != "GLOBAL"))
                            {
                                regionType = !string.IsNullOrEmpty(region) ? "(" + region + " flights)" : string.Empty;
                            }
                            else { regionType = string.Empty; }

                            if (ePlusSubscriberCount > 0 && totalSubscribeCompanionCount == 1)
                            {
                                ePlusMsg = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForSilverEplusWithOneCompanionManageRes"), _eplusSubscriberNames, regionType);
                            }
                            else if (ePlusSubscriberCount > 0 && totalSubscribeCompanionCount == 0)
                            {
                                ePlusMsg = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForSilverEplusWithZeroCompanionManageRes"), _eplusSubscriberNames, regionType);
                            }
                            else
                            {
                                ePlusMsg = string.Format(_configuration.GetValue<string>("NEWEPAEPlusSeatMessageForSilverManageRes"), _eplusSubscriberNames);
                            }
                        }
                        else if (ePlusSubscriberCount == 1)
                        {
                            ePlusMsg = ePlusSubscriptionMessage.Trim();
                        }
                        #endregion
                    }

                    #endregion
                }
                catch (Exception ex)
                { }

                if (response != null && response.SelectedTrips != null && response.SelectedTrips.Any())
                {
                    //Populate EPlus message in segments
                    foreach (MOBBKTrip trip in response.SelectedTrips)
                    {
                        foreach (MOBBKFlattenedFlight ff in trip.FlattenedFlights)
                        {
                            foreach (MOBBKFlight flight in ff.Flights)
                            {
                                if (ShopStaticUtility.IsElfSegment(flight.MarketingCarrier, flight.ServiceClass) || flight.IsIBE || _shoppingUtility.IsSeatMapSupportedOa(flight.OperatingCarrier, flight.MarketingCarrier))
                                {
                                    flight.ShowEPAMessage = false;
                                    flight.EPAMessageTitle = "";
                                    flight.EPAMessage = "";
                                }
                                else if (!string.IsNullOrEmpty(ePlusMsg))
                                {
                                    flight.ShowEPAMessage = true;
                                    flight.EPAMessageTitle = isVerticalSeatMapEnabled ? _configuration.GetValue<string>("EPlusSubscriberMessageTitle") : ePlusMsgTitle;
                                    flight.EPAMessage = ePlusMsg;
                                }
                            }
                        }
                    }
                }
            }

            return ((hasEliteAboveGold ? 9 : totalEPlusCompanionsEligible + goldMemberCount * 2), response, ePlusSubscriberCount);
        }

        public List<MOBSeatMap> GetSeatMapWithPreAssignedSeats(List<MOBSeatMap> seatMap, List<Seat> existingSeats, bool isPreferredZoneEnabled)
        {
            try
            {
                if (existingSeats != null && existingSeats.Count() > 0 && seatMap != null)
                {
                    for (int co1 = 0; co1 < seatMap.Count(); co1++)
                    {
                        for (int co2 = 0; co2 < seatMap[co1].Cabins.Count(); co2++)
                        {
                            for (int co3 = 0; co3 < seatMap[co1].Cabins[co2].Rows.Count(); co3++)
                            {
                                for (int co4 = 0; co4 < seatMap[co1].Cabins[co2].Rows[co3].Seats.Count(); co4++)
                                {
                                    foreach (Seat seat in existingSeats)
                                    {
                                        if (seat.OldSeatAssignment != null && seat.OldSeatAssignment.ToUpper().Trim() == seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].Number.ToUpper().Trim() && seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].Number.ToUpper().Trim() != string.Empty)
                                        {
                                            //if (_configuration.GetValue<bool>("EnableCSL30ManageResSelectSeatMap"))
                                            //{
                                                seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].seatvalue = GetSeatValueForPreAssignedSeat(seatMap[co1].Cabins[co2].Rows[co3].Seats[co4]);
                                            //}
                                            /*else
                                            {
                                                if (isPreferredZoneEnabled && IsPreferredSeat(seatMap[co1].Cabins[co2].Rows[co3].Seats[co4]))
                                                {
                                                    seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].seatvalue = "PZ";
                                                }
                                                else
                                                {
                                                    seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].seatvalue = seatMap[co1].Cabins[co2].Rows[co3].Seats[co4].IsEPlus ? "P" : "O";
                                                }
                                            }*/
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            { }

            return seatMap;
        }

        public List<MOBBKTraveler> GetBookingTravelerInfoWithSeatLocation(List<MOBSeatMap> seatMaps, List<MOBBKTraveler> bookingTravelerInfo)
        {
            try
            {
                if (bookingTravelerInfo != null && bookingTravelerInfo.Count > 0 && seatMaps != null)
                {
                    MOBSeatMap seatMap = seatMaps.FirstOrDefault();

                    var seats = (from traveler in bookingTravelerInfo
                                 where traveler.Seats != null && traveler.Seats.Count > 0
                                 from seat in traveler.Seats
                                 where seat != null && !string.IsNullOrEmpty(seat.OldSeatAssignment)
                                      && seat.FlightNumber == seatMap.FlightNumber.ToString()
                                      && Convert.ToDateTime(seat.DepartureDate).ToString("MMddyyy") == Convert.ToDateTime(seatMap.FlightDateTime).ToString("MMddyyy")
                                      && seat.Origin == seatMap.Departure.Code
                                      && seat.Destination == seatMap.Arrival.Code
                                 select seat).ToList();


                    foreach (Seat seat in seats)
                    {
                        MOBSeatB mobSeatB = (from cabin in seatMap.Cabins
                                             from row in cabin.Rows
                                             from se in row.Seats
                                             where se.Number.ToUpper().Trim() != string.Empty && se.Number.ToUpper().Trim() == seat.OldSeatAssignment.ToUpper().Trim()
                                             select se).FirstOrDefault();

                        seat.OldSeatLocation = mobSeatB != null ? mobSeatB.Location : null;

                    }
                }
            }
            catch (System.Exception)
            { }

            return bookingTravelerInfo;
        }

        private string GetSeatValueForPreAssignedSeat(MOBSeatB seat)
        {
            string seatValue = string.Empty;
            if (seat != null)
            {
                seatValue = seat.ServicesAndFees != null && seat.ServicesAndFees.Any() && !seat.ServicesAndFees[0].SeatFeature.IsNullOrEmpty()
                    && IsPreferredSeat(seat) ? "PZ" : seat.IsEPlus ? "P" : "O";
            }

            return seatValue;
        }

        private async Task<bool> ShowEPUSubscriptionMessage(List<MOBBKTraveler> bookingTravelerList, List<MOBBKTrip> trips)
        {
            #region Get Segments
            int segmentID = 0;
            List<FlightSegmentType> flightSegmentTypeList = new List<FlightSegmentType>();
            List<TravelerType> travelTypeList = new List<TravelerType>();
            foreach (MOBBKTrip ts in trips)
            {
                foreach (MOBBKFlattenedFlight ff in ts.FlattenedFlights)
                {
                    foreach (MOBBKFlight flight in ff.Flights)
                    {
                        #region
                        segmentID = segmentID + 1;
                        FlightSegmentType flightSegmentType = new FlightSegmentType();
                        flightSegmentType.DepartureAirport = new Location();
                        flightSegmentType.ArrivalAirport = new Location();
                        Location dep = new Location();
                        dep.LocationCode = flight.Origin;
                        flightSegmentType.DepartureAirport = dep;
                        Location arr = new Location();
                        arr.LocationCode = flight.Destination;
                        flightSegmentType.ArrivalAirport = arr;
                        //flightSegmentType.DepartureDateTime = DateTime.ParseExact(flight.DepartDate + " " + flight.DepartTime, "ddd., MMM. dd, yyyy h:m tt", CultureInfo.InvariantCulture);
                        flightSegmentType.DepartureDateTime = DateTime.ParseExact(Convert.ToDateTime(flight.DepartDate).ToShortDateString() + " " + flight.DepartTime, "M/d/yyyy h:m tt", CultureInfo.InvariantCulture);
                        flightSegmentType.DepartureDateTimeSpecified = true;
                        flightSegmentType.OperatingAirline = flight.MarketingCarrier;
                        flightSegmentType.MarketingAirline = flight.OperatingCarrier;
                        flightSegmentType.SegmentNumber = segmentID.ToString();
                        flightSegmentType.CabinCode = FlightSegmentTypeCabinCode.Economy;
                        flightSegmentType.ClassOfService = "W";
                        flightSegmentType.Id = segmentID.ToString();
                        flightSegmentTypeList.Add(flightSegmentType);
                        #endregion
                    }
                }
            }
            #endregion
            #region Get Travelers
            int travelerID = 0;
            foreach (MOBBKTraveler bookingTraveler in bookingTravelerList)
            {
                #region
                travelerID = travelerID + 1;
                TravelerType travelerType = new TravelerType();
                travelerType.Id = travelerID.ToString();// "1";
                travelerType.GivenName = bookingTraveler.Person.GivenName;
                travelerType.Surname = bookingTraveler.Person.Surname;
                travelerType.TicketingDate = DateTime.Now;
                travelerType.TicketingDateSpecified = true; // Need to set to true if Sending Ticketing Date default is false so even if ticketing date is send with false the request won't serialize the ticketing date.
                if (bookingTraveler.LoyaltyProgramProfile != null && bookingTraveler.LoyaltyProgramProfile.CarrierCode == "UA")
                {
                    #region
                    CustomerLoyaltyType[] customerLoyaltyTypeList = new CustomerLoyaltyType[1];
                    customerLoyaltyTypeList[0] = new CustomerLoyaltyType();
                    customerLoyaltyTypeList[0].ProgramId = "UA";
                    customerLoyaltyTypeList[0].MemberShipId = bookingTraveler.LoyaltyProgramProfile.MemberId;
                    travelerType.Loyalty = customerLoyaltyTypeList;
                    #endregion
                }
                travelTypeList.Add(travelerType);
                #endregion
            }

            #endregion
            var tupleRes = await _merchandizingServices.GetEPlusSubscriptionsForBookingSelectedTravelers(flightSegmentTypeList, travelTypeList, trips);
            trips = tupleRes.trips;
            return tupleRes.showEPAMsg;

        }

        public string GetOperatedByText(string marketingCarrier, string flightNumber, string operatingCarrierDescription)
        {
            if (string.IsNullOrEmpty(marketingCarrier) ||
                string.IsNullOrEmpty(flightNumber) ||
                string.IsNullOrEmpty(operatingCarrierDescription))
                return string.Empty;
            operatingCarrierDescription = ShopStaticUtility.RemoveString(operatingCarrierDescription, "Limited");
            return marketingCarrier + flightNumber + " operated by " + operatingCarrierDescription;
        }

        public bool IsMatchedFlight(TripSegment segment, OfferRequestData flightDetails, List<TripSegment> segments)
        {
            if (segment == null || flightDetails == null || segment == null || !segments.Any())
                return false;

            if (segment.FlightNumber == flightDetails.FlightNumber && segment.Departure.Code == flightDetails.Origin && segment.Arrival.Code == flightDetails.Destination)
                return true;

            //thru flight
            if (segments.Where(s => s.FlightNumber == segment.FlightNumber && s.FlightNumber == flightDetails.FlightNumber).Count() == 1)
                return true;

            return false;
        }

        public bool IsMatchedFlight(TripSegment segment, MOBSeatFocus seatFocusSegment, List<TripSegment> segments)
        {
            if (segment == null || seatFocusSegment == null || segment == null || !segments.Any())
                return false;

            if (segment.FlightNumber == seatFocusSegment.FlightNumber && segment.Departure.Code == seatFocusSegment.Origin && segment.Arrival.Code == seatFocusSegment.Destination)
                return true;

            //thru or COG flight
            if (segments.Where(s => s.FlightNumber == seatFocusSegment.FlightNumber).Count() > 1)
            {
                if (segment.Departure.Code == seatFocusSegment.Origin || segment.Arrival.Code == seatFocusSegment.Destination)
                    return true;
            }

            return false;
        }

        private string GetCityNameFromAirportDescription(string airportDescription)
        {
            string cityName = string.Empty;

            if (!string.IsNullOrEmpty(airportDescription))
            {
                int pos = airportDescription.IndexOf('(');
                cityName = airportDescription.Substring(0, pos);
            }

            if (!string.IsNullOrEmpty(cityName))
            {
                cityName = cityName.Trim();
            }

            return cityName;
        }

        public void PopulateEPAEPlusSeatMessage(ref MOBSeatChangeInitializeResponse response, int noFreeSeatCompanionCount, ref bool doNotShowEPlusSubscriptionMessage)
        {
            if (response != null && response.BookingTravlerInfo != null && response.BookingTravlerInfo.Count > 0)
            {
                int goldMemberCount = 0;
                string epaMsgTitle = string.Empty; string epaMsg = string.Empty;

                foreach (var traveler in response.BookingTravlerInfo)
                {
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.MPEliteLevel == 2)
                    {
                        goldMemberCount = goldMemberCount + 1;
                    }
                }

                int count = 0;
                StringBuilder names = new StringBuilder();
                foreach (var traveler in response.BookingTravlerInfo)
                {
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.MPEliteLevel == 2)
                    {
                        if (count == 0)
                        {
                            names.Append(traveler.Person.GivenName + " " + traveler.Person.Surname);
                        }
                        else if (count > 0 && count < goldMemberCount - 1)
                        {
                            names.Append(", " + traveler.Person.GivenName + " " + traveler.Person.Surname);
                        }
                        else
                        {
                            names.Append(" and " + traveler.Person.GivenName + " " + traveler.Person.Surname);
                        }

                        ++count;
                    }
                }

                if (noFreeSeatCompanionCount > 0)
                {

                    if (goldMemberCount == 1)
                    {
                        doNotShowEPlusSubscriptionMessage = false;
                        epaMsgTitle = _configuration.GetValue<string>("EPAEPlusSeatMessageTitle3");
                        epaMsg = string.Format(_configuration.GetValue<string>("EPAEPlusSeatMessage3"), names.ToString());
                    }
                    else
                    {
                        doNotShowEPlusSubscriptionMessage = false;
                        epaMsgTitle = _configuration.GetValue<string>("EPAEPlusSeatMessageTitle4");
                        epaMsg = string.Format(_configuration.GetValue<string>("EPAEPlusSeatMessage4"), names.ToString());
                    }

                    foreach (MOBBKTrip trip in response.SelectedTrips)
                    {
                        foreach (MOBBKFlattenedFlight ff in trip.FlattenedFlights)
                        {
                            foreach (MOBBKFlight flight in ff.Flights)
                            {
                                if (ShopStaticUtility.IsElfSegment(flight.MarketingCarrier, flight.ServiceClass) || flight.IsIBE || _shoppingUtility.IsSeatMapSupportedOa(flight.OperatingCarrier, flight.MarketingCarrier))
                                {
                                    flight.ShowEPAMessage = false;
                                    flight.EPAMessageTitle = "";
                                    flight.EPAMessage = "";
                                }
                                else if (flight.ShowEPAMessage)
                                {
                                    flight.EPAMessageTitle = epaMsgTitle;
                                    flight.EPAMessage = epaMsg;
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task<bool> IsEnablePCUSeatMapPurchaseManageRes(int appId, string appVersion, int numberOfTravelers, List<MOBItem> catalogs = null)
        {
            if (ConfigUtility.CheckClientCatalogForEnablingFeature("PCUSeatsForMultiPaxClientCatalog", catalogs)) return true;

            try
            {
                if (await new CatalogHelper(_configuration, _sessionHelperService, _dynamoDBService, _headers).GetBooleanValueFromCatalogCache("PCUOnSeatMapUpgradeAndAssignSeatCatalogSwitch", appId))
                {
                    if (!string.IsNullOrEmpty(appVersion) && appId != -1 && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnablePCUSelectedSeatPurchaseViewResVersion", "iPhoneEnablePCUSelectedSeatPurchaseViewResVersion", "", "", true, _configuration))
                    {
                        string noOfPCUTravelers = _configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes");
                        return noOfPCUTravelers.IsNullOrEmpty() ? false : numberOfTravelers <= Convert.ToInt32(noOfPCUTravelers);
                    }
                }
            }
            catch (Exception ex)
            {
                string enterlog = string.IsNullOrEmpty(ex.StackTrace) ? ex.Message : ex.StackTrace;
                _logger.LogError("IsEnablePCUSeatMapPurchaseManageRes {@Exception}", enterlog);
            }
            return false;
        }

        private async Task<string> GetSeatMapLegendId(string from, string to, int numberOfCabins)
        {
            string seatMapLegendId = string.Empty;
            var listOfCabinIds = new List<ComplimentaryUpgradeCabin>();
            try
            {
                listOfCabinIds = await _seatEngineDynamoDB.GetSeatMapLegendId<List<ComplimentaryUpgradeCabin>>(from, to, numberOfCabins, _headers.ContextValues.SessionId).ConfigureAwait(false);
                foreach (var Ids in listOfCabinIds)
                {
                    int secondCabinBrandingId = Ids.SecondCabinBrandingId != null ? 0 : Convert.ToInt32(Ids.SecondCabinBrandingId);
                    int thirdCabinBrandingId = Ids.ThirdCabinBrandingId != null ? 0 : Convert.ToInt32(Ids.ThirdCabinBrandingId);

                    if (thirdCabinBrandingId == 0)
                    {
                        if (secondCabinBrandingId == 1)
                        {
                            seatMapLegendId = "seatmap_legend5";
                        }
                        else if (secondCabinBrandingId == 2)
                        {
                            seatMapLegendId = "seatmap_legend4";
                        }
                        else if (secondCabinBrandingId == 3)
                        {
                            seatMapLegendId = "seatmap_legend3";
                        }
                    }
                    else if (thirdCabinBrandingId == 1)
                    {
                        seatMapLegendId = "seatmap_legend2";
                    }
                    else if (thirdCabinBrandingId == 4)
                    {
                        seatMapLegendId = "seatmap_legend1";
                    }
                }

            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
            }
            return seatMapLegendId;
        }

        public void EconomySeatsForBUSService(MOBSeatMap seats, bool operated = false)
        {
            if (_configuration.GetValue<bool>("EnableProjectTOM") && seats != null && seats.FleetType.Length > 1 && seats.FleetType.Substring(0, 2).ToUpper().Equals("BU"))
            //if (seats != null && seats.FleetType.Substring(0, 2).ToUpper().Equals("BU"))
            {
                string seatMapLegendEntry = (_configuration.GetValue<string>("seatMapLegendEntry").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry").ToString() : "";
                string seatMapLegendKey = (_configuration.GetValue<string>("seatMapLegendKey").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendKey").ToString() : "";
                seats.SeatLegendId = seatMapLegendKey + "|" + seatMapLegendEntry;

                //seats.SeatLegendId = "seatmap_legendTOM|Available|Unavailable";
                seats.IsOaSeatMap = true;
                seats.Cabins[0].COS = string.Empty;
                seats.IsReadOnlySeatMap = !operated;
                seats.OperatedByText = operated ? _configuration.GetValue<string>("ProjectTOMOperatedByText").ToString() : "";

                foreach (var cabin in seats.Cabins)
                {
                    foreach (var row in cabin.Rows)
                    {
                        row.Number = row.Number.PadLeft(2, '0');
                        row.Wing = false;
                    }
                }
            }
        }

        public int GetNoFreeSeatCompanionCount(List<MOBBKTraveler> travelers, List<MOBBKTrip> trips)
        {
            int noFreeSeatCompanionCount = 0;

            int goldMemberCount = 0;
            if (travelers != null && travelers.Count > 0)
            {
                foreach (var traveler in travelers)
                {
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.MPEliteLevel == 2)
                    {
                        goldMemberCount = goldMemberCount + 1;
                    }
                }

                if (goldMemberCount > 0)
                {
                    noFreeSeatCompanionCount = travelers.Count - 2 * goldMemberCount;
                }

                if (!HasEconomySegment(trips))
                {
                    noFreeSeatCompanionCount = 0;
                }
            }

            return noFreeSeatCompanionCount;
        }

        public async Task<(int, MOBSeatChangeInitializeResponse response, int ePlusSubscriberCount, bool hasEliteAboveGold, bool doNotShowEPlusSubscriptionMessage, bool showEPUSubscriptionMessage)> PopulateEPlusSubscriberAndMPMemeberSeatMessage(MOBSeatChangeInitializeResponse response, int applicationID, string sessionID, int ePlusSubscriberCount, bool hasEliteAboveGold, bool doNotShowEPlusSubscriptionMessage, bool showEPUSubscriptionMessage)
        {
            // Booking and View Res only Care about Gold Level Status for Free EPlus Seats because silver will get only on Check In path.
            int goldMemberCount = 0;
            int subscribeCompanionCount = 0;
            int totalEPlusCompanionsEligible = 0;
            hasEliteAboveGold = false;
            string ePlusSubscriptionMessage = string.Empty;
            List<string> ePlusSubscriberNames = new List<string>();
            bool isOneOfIsGlobalSubscriber = false;
            string ePlusMsgTitle = string.Empty;
            string ePlusMsg = string.Empty;
            string regionType = string.Empty;

            if (response != null && response.BookingTravlerInfo != null && response.BookingTravlerInfo.Count > 0)
            {
                #region
                StringBuilder names = new StringBuilder();
                foreach (var traveler in response.BookingTravlerInfo)
                {
                    #region
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.MPEliteLevel == 2)
                    {
                        if (goldMemberCount == 0)
                        {
                            names.Append(traveler.Person.GivenName + " " + traveler.Person.Surname);
                        }
                        else if (goldMemberCount > 0)
                        {
                            names.Append(", " + traveler.Person.GivenName + " " + traveler.Person.Surname);
                        }
                        ++goldMemberCount;
                    }
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.MPEliteLevel > 2)
                    {
                        hasEliteAboveGold = true;
                        break;
                    }
                    if (traveler.LoyaltyProgramProfile != null && traveler.LoyaltyProgramProfile.CarrierCode == "UA")
                    {
                        #region

                        MOBUASubscriptions objUASubscriptions = null;
                        bool isEPlusSubscriber = false;
                        var tupleResponse = await GetEPlusSubscriptionMessage(traveler.LoyaltyProgramProfile.MemberId, applicationID, sessionID, traveler.Person.GivenName + " " + traveler.Person.Surname, objUASubscriptions, isEPlusSubscriber, isOneOfIsGlobalSubscriber, subscribeCompanionCount, regionType);
                        ePlusSubscriptionMessage = ePlusSubscriptionMessage + " " + tupleResponse.Item1.Trim();
                        objUASubscriptions = tupleResponse.objUASubscriptions;
                        isEPlusSubscriber = tupleResponse.isEPlusSubscriber;
                        isOneOfIsGlobalSubscriber = tupleResponse.isOneOfIsGlobalSubscriber;
                        subscribeCompanionCount = tupleResponse.subscribeCompanionCount;
                        regionType = tupleResponse.regionType;

                        //traveler.EPlusSubscriptions = objUASubscriptions;
                        traveler.IsEPlusSubscriber = isEPlusSubscriber;
                        if (isEPlusSubscriber)
                        {
                            //Calculate total number of EPlus Companions eligible to get free Eplus Seats
                            totalEPlusCompanionsEligible = totalEPlusCompanionsEligible + 1;
                            totalEPlusCompanionsEligible = totalEPlusCompanionsEligible + subscribeCompanionCount;

                            ePlusSubscriberNames.Add(traveler.Person.GivenName + " " + traveler.Person.Surname);
                            ePlusSubscriberCount = ePlusSubscriberCount + 1;
                        }

                        #endregion
                    }
                    #endregion
                }
                if (ePlusSubscriberCount > 0)
                {
                    showEPUSubscriptionMessage = await ShowEPUSubscriptionMessage(response.BookingTravlerInfo, response.SelectedTrips);
                }

                //if (hasEliteAboveGold || totalEPlusCompanionsEligible >= response.BookingTravelerInfo.Count())
                if (hasEliteAboveGold || !showEPUSubscriptionMessage || (showEPUSubscriptionMessage && response.BookingTravlerInfo.Count() <= (goldMemberCount * 2)))
                {
                    //response.BookingTravelerInfo[0].EPAMessageTitle = string.Empty;
                    //response.BookingTravelerInfo[0].EPAMessage = string.Empty;
                    doNotShowEPlusSubscriptionMessage = true;
                    showEPUSubscriptionMessage = false;
                }
                else
                {
                    if (goldMemberCount > 0 && ePlusSubscriberCount > 0)
                    {
                        #region
                        string _eplusSubscriberNames = string.Empty;
                        foreach (string eachName in ePlusSubscriberNames)
                        {
                            _eplusSubscriberNames = _eplusSubscriberNames + "," + eachName;
                        }
                        if (isOneOfIsGlobalSubscriber)
                        {
                            ePlusMsgTitle = _configuration.GetValue<string>("EPlusSubscriberMessageTitle");
                            ePlusMsg = string.Format(_configuration.GetValue<string>("EPlusMessageForGLOBALGoldMemberAndSubscriberPLUS"), names.ToString(), _eplusSubscriberNames.Trim().Trim(','));
                        }
                        else
                        {
                            ePlusMsgTitle = _configuration.GetValue<string>("EPlusSubscriberMessageTitle");
                            ePlusMsg = string.Format(_configuration.GetValue<string>("EPlusMessageForGoldMemberAndSubscriberPLUS"), names.ToString(), _eplusSubscriberNames.Trim().Trim(','));
                        }
                        #endregion
                    }
                    else if (ePlusSubscriberCount > 1 && goldMemberCount == 0)
                    {
                        #region
                        string[] _eplusSubscriberNamesList = ePlusSubscriberNames.ToArray();
                        string _eplusSubscriberNames = string.Empty;
                        for (int i = 1; i < _eplusSubscriberNamesList.Length; i++)
                        {
                            _eplusSubscriberNames = _eplusSubscriberNames + "," + _eplusSubscriberNamesList[i].ToString();
                        }
                        if (isOneOfIsGlobalSubscriber)
                        {
                            ePlusMsgTitle = _configuration.GetValue<string>("EPlusSubscriberMessageTitle");
                            ePlusMsg = string.Format(_configuration.GetValue<string>("EPlusMessageForGLOBALMultipleSubscriberPLUS"), _eplusSubscriberNamesList[0].ToString(), _eplusSubscriberNames.Trim().Trim(','));
                        }
                        else
                        {
                            ePlusMsgTitle = _configuration.GetValue<string>("EPlusSubscriberMessageTitle");
                            ePlusMsg = string.Format(_configuration.GetValue<string>("EPlusMessageForMultipleSubscriberPLUS"), _eplusSubscriberNamesList[0].ToString(), _eplusSubscriberNames.Trim().Trim(','));
                        }
                        #endregion
                    }
                    else if (ePlusSubscriberCount == 1 && goldMemberCount == 0)
                    {
                        ePlusMsgTitle = _configuration.GetValue<string>("EPlusSubscriberMessageTitle").ToString().Trim();
                        ePlusMsg = ePlusSubscriptionMessage.Trim();
                    }

                    //Populate EPlus message in segments
                    foreach (MOBBKTrip trip in response.SelectedTrips)
                    {
                        foreach (MOBBKFlattenedFlight ff in trip.FlattenedFlights)
                        {
                            foreach (MOBBKFlight flight in ff.Flights)
                            {
                                if (ShopStaticUtility.IsElfSegment(flight.MarketingCarrier, flight.ServiceClass) || flight.IsIBE || _shoppingUtility.IsSeatMapSupportedOa(flight.OperatingCarrier, flight.MarketingCarrier))
                                {
                                    flight.ShowEPAMessage = false;
                                    flight.EPAMessageTitle = "";
                                    flight.EPAMessage = "";
                                }
                                else if (flight.ShowEPAMessage)
                                {
                                    flight.EPAMessageTitle = ePlusMsgTitle;
                                    flight.EPAMessage = ePlusMsg;
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            return ((hasEliteAboveGold ? 9 : totalEPlusCompanionsEligible + goldMemberCount * 2), response, ePlusSubscriberCount, hasEliteAboveGold, doNotShowEPlusSubscriptionMessage, showEPUSubscriptionMessage);
        }

        //objMOBSeatMap.ShowInfoMessageOnSeatMap = _seatEngine.ShowNoFreeSeatsAvailableMessage(persistedReservation, countNoOfFreeSeats, countNoOfPricedSeats, isInCheckInWindow, isFirstSegmentInReservation);

        public async Task<string> ShowNoFreeSeatsAvailableMessage(int noOfTravelersWithoutSeat, int noOfFreeEplusEligibleRemaining, int noOfFreeSeats,
            int noOfPricedSeats, bool isBasicEconomy)
        {
            if (!_configuration.GetValue<bool>("EnableSSA")) return string.Empty;

            return _shoppingUtility.EnableIBEFull() && isBasicEconomy
                    ? await SeatmapMessageForBasicEconomy(noOfTravelersWithoutSeat, noOfPricedSeats)
                    : await SeatmapMessageForSeatAvailability(noOfTravelersWithoutSeat, noOfFreeEplusEligibleRemaining, noOfFreeSeats, noOfPricedSeats);
        }

        private async Task<string> SeatmapMessageForBasicEconomy(int noOfTravelersWithoutSeat, int noOfPricedSeats)
        {
            if (HaveEnoughSeatsToChange(noOfTravelersWithoutSeat, noOfPricedSeats))
            {
                return await GetDocumentTextFromDataBase("MR_ASA_SEATS_AVAILABLE_FOR_PURCHASE");
            }

            return noOfTravelersWithoutSeat == 0 ? string.Empty : await GetDocumentTextFromDataBase("MR_ASA_SEATS_NOT_AVAILABLE");
        }

        private async Task<string> SeatmapMessageForSeatAvailability(int noOfTravelersWithoutSeat, int noOfFreeEplusEligibleRemaining, int noOfFreeSeats, int noOfPricedSeats)
        {
            return EnoughFreeSeats(noOfTravelersWithoutSeat, noOfFreeEplusEligibleRemaining, noOfFreeSeats, noOfPricedSeats)
                    ? string.Empty
                    : await GetDocumentTextFromDataBase("SSA_NO_FREE_SEATS_MESSAGE");
        }

        private bool HaveEnoughSeatsToChange(int noOfTravelersWithoutSeat, int noOfPricedSeats)
        {
            return noOfTravelersWithoutSeat == 0 && noOfPricedSeats > 0 || noOfTravelersWithoutSeat != 0 && noOfPricedSeats - noOfTravelersWithoutSeat >= 0;
        }

        private bool IsPreferredSeat(MOBSeatB seat)
        {
            if (seat != null && seat.ServicesAndFees != null && seat.ServicesAndFees.Any())
            {
                return IsPreferredSeat(seat.ServicesAndFees[0].SeatFeature, seat.ServicesAndFees[0].Program);
            }
            return false;
        }

        public bool IsPreferredSeat(string DisplaySeatType, string program)
        {
            return IsPreferredSeatProgramCode(program) && IsPreferredSeatDisplayType(DisplaySeatType);
        }

        public int GetNoOfFreeEplusEligibleRemaining(List<MOBBKTraveler> travelers, string orgin, string destination, int totalEplusEligible, bool isElf, out int noOfTravelersWithNoSeat)
        {
            noOfTravelersWithNoSeat = 0;
            if (travelers.IsNullOrEmpty() || orgin.IsNullOrEmpty() || destination.IsNullOrEmpty())
                return 0;

            noOfTravelersWithNoSeat = travelers.Count;
            int remainingEplusCount = isElf ? 0 : totalEplusEligible;
            foreach (var traveler in travelers)
            {
                bool alwaysFreeEplus = (!traveler.LoyaltyProgramProfile.IsNullOrEmpty() && traveler.LoyaltyProgramProfile.MPEliteLevel >= 2)
                                       || traveler.IsEPlusSubscriber;

                var seatList = traveler.Seats.Where(s => s.Origin.ToUpper().Equals(orgin.ToUpper()) &&
                                                         s.Destination.ToUpper().Equals(destination.ToUpper()) &&
                                                         !s.OldSeatAssignment.IsNullOrEmpty()).ToList();
                foreach (var seat in seatList)
                {
                    if (noOfTravelersWithNoSeat > 0)
                        noOfTravelersWithNoSeat--;

                    if (alwaysFreeEplus && remainingEplusCount > 0)
                    {
                        remainingEplusCount--;
                    }
                    else if (seat.OldSeatPrice == 0 && remainingEplusCount > 0)
                    {
                        if (IsEPlusSeat(seat.OldSeatType)
                            || IsEPlusSeat(seat.OldSeatProgramCode)
                            || _shoppingUtility.IsEMinusSeat(seat.OldSeatProgramCode))
                            remainingEplusCount--;
                    }
                }
            }

            return remainingEplusCount;
        }

        private bool IsEPlusSeat(string seatType)
        {
            if (seatType.IsNullOrEmpty())
                return false;

            string ePlusSeatTypes = _configuration.GetValue<string>("EPlusSeatSharesSeatTypes");
            if (ePlusSeatTypes.IsNullOrEmpty()) return false;

            return ePlusSeatTypes.IndexOf(seatType + "|", StringComparison.Ordinal) != -1;
        }

        private bool ShowSeatChange(United.Service.Presentation.SegmentModel.ReservationFlightSegment segment, Service.Presentation.ReservationModel.Reservation Detail, bool unETicketedCheckInPNRSeatChangeEnabled, bool isETicketed, bool isVerticalSeatMapEnabled, bool checkInSegmentSeatChangeLinkFixEnabled)
        {
            if (!_configuration.GetValue<bool>("DisableShowSeatChangeButton"))
            {
                try
                {
                    bool departed = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(segment.EstimatedDepartureTime)) < DateTime.Now.ToUniversalTime();

                    if (!isVerticalSeatMapEnabled && checkInSegmentSeatChangeLinkFixEnabled)
                    {
                        if (departed
                            || Detail.Type.ToList().Exists(p => string.Equals(p.Description, "GROUP", StringComparison.OrdinalIgnoreCase))
                            || (Detail.Travelers != null && Detail.Travelers.Count > 9)
                            || (Detail?.Prices != null && Detail?.Prices.Count > 0 && string.Equals(Detail?.Prices[0]?.PriceFlightSegments?.FirstOrDefault
                                    (x => x.SegmentNumber == segment.SegmentNumber)?.FlightStatuses[0]?.Code, "UA USED", StringComparison.OrdinalIgnoreCase)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (departed
                            || Detail.Type.ToList().Exists(p => string.Equals(p.Description, "GROUP", StringComparison.OrdinalIgnoreCase))
                            || (Detail.Travelers != null && Detail.Travelers.Count > 9)
                            || (Convert.ToBoolean(GetCharactersticValue(segment.Characteristic, "CheckinTriggered")) && (!unETicketedCheckInPNRSeatChangeEnabled || isETicketed))
                            || Convert.ToBoolean(GetCharactersticValue(segment.Characteristic, "CheckedIn"))
                            || (Detail?.Prices != null && string.Equals(Detail?.Prices[0]?.PriceFlightSegments?.FirstOrDefault
                                    (x => x.SegmentNumber == segment.SegmentNumber)?.FlightStatuses[0]?.Code, "UA USED", StringComparison.OrdinalIgnoreCase)))
                        {
                            return false;
                        }
                    }
                }
                catch { }
            }
            return true;
        }

        public async Task<(string, MOBUASubscriptions objUASubscriptions, bool isEPlusSubscriber, bool isOneOfIsGlobalSubscriber, int subscribeCompanionCount, string regionType)> GetEPlusSubscriptionMessage(string mpAccountNumber, int applicationID, string sessionID, string travelerName, MOBUASubscriptions objUASubscriptions, bool isEPlusSubscriber, bool isOneOfIsGlobalSubscriber, int subscribeCompanionCount, string regionType, bool isEnablePreferredZoneSubscriptionMessages = false)
        {
            objUASubscriptions = await _merchandizingServices.GetEPlusSubscriptions(mpAccountNumber, applicationID, sessionID).ConfigureAwait(false);
            string ePLUSMessage = string.Empty; regionType = string.Empty;
            //int subscribeCompanionCount = 0;
            isEPlusSubscriber = false;
            if (objUASubscriptions != null && objUASubscriptions.SubscriptionTypes != null && objUASubscriptions.SubscriptionTypes.Count() > 0)
            {
                #region
                isEPlusSubscriber = true;
                foreach (MOBUASubscription objUASubscription in objUASubscriptions.SubscriptionTypes)
                {
                    foreach (MOBItem item in objUASubscription.Items)
                    {
                        #region
                        if (item.Id.Trim().ToUpper() == "EplusSubscribeRegion".ToUpper())
                        {
                            regionType = item.CurrentValue.Trim();
                        }
                        else if (item.Id.Trim().ToUpper() == "EPlusSubscribeCompanionCount".ToUpper())
                        {
                            subscribeCompanionCount = Convert.ToInt32(item.CurrentValue.Trim());
                        }
                        #endregion
                    }
                }
                #region
                if (isEnablePreferredZoneSubscriptionMessages)
                {
                    isOneOfIsGlobalSubscriber = regionType.Equals("GLOBAL", StringComparison.OrdinalIgnoreCase);
                    ePLUSMessage = GetEplusSubscriptionMessage(travelerName, subscribeCompanionCount, regionType);
                }
                #endregion
                #region
                else
                {
                    if (regionType.Trim().ToUpper() == "GLOBAL" && subscribeCompanionCount == 0)
                    {
                        isOneOfIsGlobalSubscriber = true;
                        if (_configuration.GetValue<string>("EPlusMessageForIndividualGLOBALSubscriber") != null)
                        {
                            ePLUSMessage = String.Format(_configuration.GetValue<string>("EPlusMessageForIndividualGLOBALSubscriber").ToString(), travelerName);
                        }
                    }
                    else if (regionType.Trim().ToUpper() == "GLOBAL" && subscribeCompanionCount > 0)
                    {
                        isOneOfIsGlobalSubscriber = true;
                        if (_configuration.GetValue<string>("EPlusMessageForIndividualGLOBALSubscriber") != null)
                        {
                            ePLUSMessage = String.Format(_configuration.GetValue<string>("EPlusMessageForGLOBALSubscriberPLUS").ToString(), travelerName, subscribeCompanionCount.ToString() + (subscribeCompanionCount == 1 ? " companion " : " companions "));
                        }
                    }
                    else
                    {
                        if (subscribeCompanionCount == 0)
                        {
                            if (_configuration.GetValue<string>("EPlusMessageForIndividualSubscriber") != null)
                            {
                                ePLUSMessage = String.Format(_configuration.GetValue<string>("EPlusMessageForIndividualSubscriber").ToString(), travelerName, regionType);
                            }
                        }
                        else if (subscribeCompanionCount > 0)
                        {
                            if (_configuration.GetValue<string>("EPlusMessageForSubscriberPLUS") != null)
                            {
                                ePLUSMessage = String.Format(_configuration.GetValue<string>("EPlusMessageForSubscriberPLUS").ToString(), travelerName, subscribeCompanionCount.ToString() + (subscribeCompanionCount == 1 ? " companion " : " companions "), regionType);
                            }
                        }
                    }
                }
                #endregion
                #endregion
            }
            return (ePLUSMessage, objUASubscriptions, isEPlusSubscriber, isOneOfIsGlobalSubscriber, subscribeCompanionCount, regionType);
        }

        #endregion

        public async Task<string> GetPolarisSeatMapLegendId(string from, string to, int numberOfCabins, List<string> polarisCabinBrandingDescriptions, int applicationId = -1, string appVersion = "", bool isBERecommendedSeatsAvailable = false)
        {
            string seatMapLegendId = string.Empty;

            //POLARIS Cabin Branding SeatMapLegend Booking Path
            string seatMapLegendEntry1 = (_configuration.GetValue<string>("seatMapLegendEntry1") != null) ? _configuration.GetValue<string>("seatMapLegendEntry1") : "";
            string seatMapLegendEntry2 = (_configuration.GetValue<string>("seatMapLegendEntry2") != null) ? _configuration.GetValue<string>("seatMapLegendEntry2") : "";
            string seatMapLegendEntry3 = (_configuration.GetValue<string>("seatMapLegendEntry3") != null) ? _configuration.GetValue<string>("seatMapLegendEntry3") : "";
            string seatMapLegendEntry4 = (_configuration.GetValue<string>("seatMapLegendEntry4") != null) ? _configuration.GetValue<string>("seatMapLegendEntry4") : "";

            string seatMapLegendEntry5 = (_configuration.GetValue<string>("seatMapLegendEntry5") != null) ? _configuration.GetValue<string>("seatMapLegendEntry5") : "";
            string seatMapLegendEntry6 = (_configuration.GetValue<string>("seatMapLegendEntry6") != null) ? _configuration.GetValue<string>("seatMapLegendEntry6") : "";
            string seatMapLegendEntry7 = (_configuration.GetValue<string>("seatMapLegendEntry7") != null) ? _configuration.GetValue<string>("seatMapLegendEntry7") : "";
            string seatMapLegendEntry8 = (_configuration.GetValue<string>("seatMapLegendEntry8") != null) ? _configuration.GetValue<string>("seatMapLegendEntry8") : "";
            string seatMapLegendEntry9 = (_configuration.GetValue<string>("seatMapLegendEntry9") != null) ? _configuration.GetValue<string>("seatMapLegendEntry9") : "";
            string seatMapLegendEntry14 = string.Empty;
            string seatMapLegendEntry15 = string.Empty;
            string legendForPZA = string.Empty;
            string legendForUPP = string.Empty;
            string legendForFreeFamilySeating = string.Empty;
            // Preferred Seat //AB-223
            bool isPreferredZoneEnabled = ConfigUtility.EnablePreferredZone(applicationId, appVersion); // Check if preferred seat
            if (isPreferredZoneEnabled)
            {
                seatMapLegendEntry14 = _configuration.GetValue<string>("seatMapLegendEntry14");
                legendForPZA = "_PZA";
            }
            if (isBERecommendedSeatsAvailable)
            {
                seatMapLegendEntry15 = _configuration.GetValue<string>("seatMapLegendEntry15");
                legendForFreeFamilySeating = "_FFS";
            }
            if (ConfigUtility.IsUPPSeatMapSupportedVersion(applicationId, appVersion) && numberOfCabins == 3 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Any(p => p.ToUpper() == "UNITED PREMIUM PLUS"))
            {
                legendForUPP = "_UPP";
                seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris Business|United Premium Plus|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
            }
            else
            {
                if (_configuration.GetValue<bool>("IsEnableUPPForTwoCabinFlight")
                    && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, "AndroidUPPForTwoCabinFlightVersion", "iOSUPPForTwoCabinFlightVersion") && numberOfCabins == 2 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Any(p => p.ToUpper() == "UNITED PREMIUM PLUS"))
                {
                    legendForUPP = "_UPP";
                }
                if (_configuration.GetValue<bool>("DisableComplimentaryUpgradeOnpremSqlService"))
                {

                    if (numberOfCabins == 1)
                    {
                        seatMapLegendId = "seatmap_legend6" + legendForPZA + legendForFreeFamilySeating + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                    else if (numberOfCabins == 3)
                    {
                        seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                    else//If number of cabin==2 or by default assiging legend5
                    {
                        seatMapLegendId = "seatmap_legend5" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                }
                else
                {
                    var listOfCabinIds = await _complimentaryUpgradeService.GetComplimentaryUpgradeOfferedFlagByCabinCount(from, to, numberOfCabins, _headers.ContextValues.SessionId, _headers.ContextValues.TransactionId).ConfigureAwait(false);

                    if (listOfCabinIds != null)
                    {
                        foreach (var Ids in listOfCabinIds)
                        {
                            //verification needed
                            int secondCabinBrandingId = Ids.SecondCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(Ids.SecondCabinBrandingId);
                            int thirdCabinBrandingId = Ids.ThirdCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(Ids.ThirdCabinBrandingId);

                            //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                            //Added the code to check the flag for Preferred Zone and app version > 2.1.60  
                            if (thirdCabinBrandingId == 0)
                            {
                                if (secondCabinBrandingId == 1)
                                {
                                    seatMapLegendId = "seatmap_legend5" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9; // Sample Value Could be Ex: seatmap_legend5|First|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                                }
                                else if (secondCabinBrandingId == 2)
                                {
                                    seatMapLegendId = "seatmap_legend4" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend4|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                                }
                                else if (secondCabinBrandingId == 3)
                                {
                                    seatMapLegendId = "seatmap_legend3" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend3|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend4|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                                }
                            }
                            else if (thirdCabinBrandingId == 1)
                            {
                                seatMapLegendId = "seatmap_legend2" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Economy|Occupied Seat|Exit Row 
                            }
                            else if (thirdCabinBrandingId == 4)
                            {
                                seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(seatMapLegendId))
                    {
                        #region Adding Preferred Seat Legend ID
                        //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                        //Added the code to check the flag for Preferred Zone and app version > 2.1.60
                        //Changes added on 09/24/2018                
                        //Bug 213002 mAPP: Seat Map- Blank Legend is displayed for One Cabin Flights
                        //Bug 102152
                        if (!string.IsNullOrEmpty(appVersion) &&
                        GeneralHelper.isApplicationVersionGreater(applicationId, appVersion, "AndroidFirstCabinVersion", "iPhoneFirstCabinVersion", "", "", true, _configuration)
                        && numberOfCabins == 1 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0 &&
                        !string.IsNullOrEmpty(polarisCabinBrandingDescriptions[0].ToString().Trim()))
                        {
                            seatMapLegendId = "seatmap_legend6" + legendForPZA + legendForFreeFamilySeating + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                        else
                        {
                            seatMapLegendId = "seatmap_legend5" + legendForPZA + legendForFreeFamilySeating + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry15 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                    }
                }
                #region
                //Database database = DatabaseFactory.CreateDatabase("ConnectionString - DB_Flightrequest");
                //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_GetComplimentary_Upgrade_Offered_flag_By_Cabin_Count");
                //database.AddInParameter(dbCommand, "@Origin", DbType.String, from);
                //database.AddInParameter(dbCommand, "@destination", DbType.String, to);
                //database.AddInParameter(dbCommand, "@numberOfCabins", DbType.Int32, numberOfCabins);

                //try
                //{
                //    using (IDataReader dataReader = database.ExecuteReader(dbCommand))
                //    {
                //        while (dataReader.Read())
                //        {
                //            #region
                //            int secondCabinBrandingId = dataReader["SecondCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["SecondCabinBrandingId"]);
                //            int thirdCabinBrandingId = dataReader["ThirdCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["ThirdCabinBrandingId"]);

                //            //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                //            //Added the code to check the flag for Preferred Zone and app version > 2.1.60                                           
                //            if (thirdCabinBrandingId == 0)
                //            {
                //                if (secondCabinBrandingId == 1)
                //                {
                //                    seatMapLegendId = "seatmap_legend5" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9; // Sample Value Could be Ex: seatmap_legend5|First|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                //                }
                //                else if (secondCabinBrandingId == 2)
                //                {
                //                    seatMapLegendId = "seatmap_legend4" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend4|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                //                }
                //                else if (secondCabinBrandingId == 3)
                //                {
                //                    seatMapLegendId = "seatmap_legend3" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend3|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend4|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                //                }
                //            }
                //            else if (thirdCabinBrandingId == 1)
                //            {
                //                seatMapLegendId = "seatmap_legend2" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Economy|Occupied Seat|Exit Row 
                //            }
                //            else if (thirdCabinBrandingId == 4)
                //            {
                //                seatMapLegendId = "seatmap_legend1" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                //            }
                //            #endregion
                //        }
                //    }
                //}
                //catch (System.Exception ex)
                //{
                //    Console.Write(ex.Message);
                //}
                //if (string.IsNullOrEmpty(seatMapLegendId))
                //{
                //    #region Adding Preferred Seat Legend ID
                //    //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                //    //Added the code to check the flag for Preferred Zone and app version > 2.1.60
                //    //Changes added on 09/24/2018                
                //    //Bug 213002 mAPP: Seat Map- Blank Legend is displayed for One Cabin Flights
                //    //Bug 102152
                //    if (!string.IsNullOrEmpty(appVersion) &&
                //        GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "AndroidFirstCabinVersion", "iPhoneFirstCabinVersion", "", "", true)
                //        && numberOfCabins == 1 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0 &&
                //        !string.IsNullOrEmpty(polarisCabinBrandingDescriptions[0].ToString().Trim()))
                //    {
                //        seatMapLegendId = "seatmap_legend6" + legendForPZA + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                //    }
                //    else
                //    {
                //        seatMapLegendId = "seatmap_legend5" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                //    }
                //    #endregion
                //}
                #endregion
                #endregion
            }
            return seatMapLegendId;
        }

        public async Task<string> GetPolarisSeatMapLegendIdPolarisOff(string from, string to, int numberOfCabins)
        {
            #region
            string seatMapLegendId = string.Empty;

            List<CabinBrand> lstCabinBrand = new List<CabinBrand>();
            try
            {
                lstCabinBrand = await _complimentaryUpgradeService.GetComplimentaryUpgradeOfferedFlagByCabinCount(from, to, numberOfCabins, _headers.ContextValues.SessionId, _headers.ContextValues.TransactionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("OnPremSQLService-GetComplimentaryUpgradeOfferedFlagByCabinCount Error {@Exception}", JsonConvert.SerializeObject(ex));
            }

            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - DB_Flightrequest");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_GetComplimentary_Upgrade_Offered_flag_By_Cabin_Count");
            //database.AddInParameter(dbCommand, "@Origin", DbType.String, from);
            //database.AddInParameter(dbCommand, "@destination", DbType.String, to);
            //database.AddInParameter(dbCommand, "@numberOfCabins", DbType.Int32, numberOfCabins);

            //POLARIS Cabin Branding SeatMapLegend Booking Path
            string seatMapLegendEntry1 = (_configuration.GetValue<string>("seatMapLegendEntry10").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry10").ToString() : "";
            string seatMapLegendEntry2 = (_configuration.GetValue<string>("seatMapLegendEntry11").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry11").ToString() : "";
            string seatMapLegendEntry3 = (_configuration.GetValue<string>("seatMapLegendEntry12").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry12").ToString() : "";
            string seatMapLegendEntry4 = (_configuration.GetValue<string>("seatMapLegendEntry13").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry13").ToString() : "";

            string seatMapLegendEntry5 = (_configuration.GetValue<string>("seatMapLegendEntry5").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry5").ToString() : "";
            string seatMapLegendEntry6 = (_configuration.GetValue<string>("seatMapLegendEntry6").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry6").ToString() : "";
            string seatMapLegendEntry7 = (_configuration.GetValue<string>("seatMapLegendEntry7").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry7").ToString() : "";
            string seatMapLegendEntry8 = (_configuration.GetValue<string>("seatMapLegendEntry8").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry8").ToString() : "";
            string seatMapLegendEntry9 = (_configuration.GetValue<string>("seatMapLegendEntry9").ToString() != null) ? _configuration.GetValue<string>("seatMapLegendEntry9").ToString() : "";

            try
            {
                if (lstCabinBrand.Count > 0)
                {
                    foreach (var cb in lstCabinBrand)
                    {
                        #region
                        int secondCabinBrandingId = cb.SecondCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(cb.SecondCabinBrandingId);
                        int thirdCabinBrandingId = cb.ThirdCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(cb.ThirdCabinBrandingId);

                        if (thirdCabinBrandingId == 0)
                        {
                            if (secondCabinBrandingId == 1)
                            {
                                seatMapLegendId = "seatmap_legend5" + seatMapLegendEntry3 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                            }
                            else if (secondCabinBrandingId == 2)
                            {
                                seatMapLegendId = "seatmap_legend4" + seatMapLegendEntry4 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                            }
                            else if (secondCabinBrandingId == 3)
                            {
                                seatMapLegendId = "seatmap_legend3" + seatMapLegendEntry2 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                            }
                        }
                        else if (thirdCabinBrandingId == 1)
                        {
                            seatMapLegendId = "seatmap_legend2" + seatMapLegendEntry3 + seatMapLegendEntry4 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                        else if (thirdCabinBrandingId == 4)
                        {
                            seatMapLegendId = "seatmap_legend1" + seatMapLegendEntry1 + seatMapLegendEntry2 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                        #endregion
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetPolarisSeatMapLegendIdPolarisOff Error {@Error} ", JsonConvert.SerializeObject(ex));
            }
            if (string.IsNullOrEmpty(seatMapLegendId))
            {
                seatMapLegendId = "seatmap_legend5" + seatMapLegendEntry3 + seatMapLegendEntry6 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
            }

            return seatMapLegendId;

            #endregion
        }

        public async Task<bool> HasPCUOfferState(MOBSeatChangeInitializeRequest request)
        {
            var pcuState = new PcuState();
            pcuState = await _sessionHelperService.GetSession<PcuState>(request.SessionId, pcuState.ObjectName, new List<string> { request.SessionId, pcuState.ObjectName }).ConfigureAwait(false);

            if (pcuState == null
                || pcuState.PremiumCabinUpgradeOfferDetail == null
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions == null
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleSegments == null
                || !pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleSegments.Any()
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleTravelers == null
                || !pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleTravelers.Any())
            {
                return false;
            }
            return true;
        }

        public TravelOptionsRequest BuildTravelOptionsRequest(MOBSeatChangeInitializeRequest request)
        {
            TravelOptionsRequest travelOptionsRequest = new TravelOptionsRequest();
            travelOptionsRequest.SessionId = request?.SessionId;
            travelOptionsRequest.Flow = request?.Flow;
            travelOptionsRequest.Application = request?.Application;
            travelOptionsRequest.DeviceId = request?.DeviceId;
            travelOptionsRequest.ProductCode = "PCU";
            travelOptionsRequest.ProductName = "PCU";
            travelOptionsRequest.RecordLocator = request?.RecordLocator;
            travelOptionsRequest.LastName = request?.LastName;
            return travelOptionsRequest;
        }
        public bool SupressLMX(int appId)
        {
            bool supressLMX = false;
            bool.TryParse(_configuration.GetValue<string>("SupressLMX"), out supressLMX); // ["SupressLMX"] = true to make all Apps Turn off. ["SupressLMX"] = false then will check for each app as below.
            if (!supressLMX && _configuration.GetValue<string>("AppIDSToSupressLMX") != null && _configuration.GetValue<string>("AppIDSToSupressLMX").Trim() != "")
            {
                string appIDS = _configuration.GetValue<string>("AppIDSToSupressLMX"); // AppIDSToSupressLMX = ~1~2~3~ or ~1~ or empty to allow lmx to all apps
                supressLMX = appIDS.Contains("~" + appId.ToString() + "~");
            }
            return supressLMX;
        }

        public void CountNoOfFreeSeatsAndPricedSeats(MOBSeatB seat, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats)
        {
            if (seat.IsNullOrEmpty() || seat.seatvalue.IsNullOrEmpty() ||
                 seat.seatvalue == "-" || seat.seatvalue.ToUpper() == "X" || seat.IsPcuOfferEligible)
                return;

            if (seat.ServicesAndFees.IsNullOrEmpty())
            {
                countNoOfFreeSeats++;
            }
            else if (seat.ServicesAndFees[0].Available && seat.ServicesAndFees[0].TotalFee <= 0)
            {
                countNoOfFreeSeats++;
            }
            else if (seat.ServicesAndFees[0].Available)
            {
                countNoOfPricedSeats++;
            }
        }
        
        public async Task<string> ShowOaSeatMapAvailabilityDisclaimerText()
        {
            return await GetDocumentTextFromDataBase("OA_SEATMAP_DISCLAIMER_TEXT");
        }
        public async Task<string> GetDocumentTextFromDataBase(string title)
        {
            var messagesFromDb = await GetMPPINPWDTitleMessages(title).ConfigureAwait(false);
            return messagesFromDb != null && messagesFromDb.Any() ? messagesFromDb[0].CurrentValue : string.Empty;
        }
        private async Task<List<MOBItem>> GetMPPINPWDTitleMessages(string titleList)
        {
            List<MOBItem> messages = new List<MOBItem>();
            List<United.Definition.MOBLegalDocument> docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(titleList, _headers.ContextValues.TransactionId, (!_configuration.GetValue<bool>("DisableManageRes23C_P3")) ? true : false);
            if (docs != null && docs.Count > 0)
            {
                foreach (United.Definition.MOBLegalDocument doc in docs)
                {
                    MOBItem item = new MOBItem();
                    item.Id = doc.Title;
                    item.CurrentValue = doc.LegalDocument;
                    messages.Add(item);
                }
            }
            return messages;
        }
        public string GetFareBasicCodeFromBundles(List<TravelOption> travelOptions, int tripId, string defaultFareBasisCode, string destination, string origin)
        {
            // string strSegmentname = origin + " - " + destination;
            if (travelOptions == null || travelOptions.Count <= 0)
                return defaultFareBasisCode;

            foreach (var travelOption in travelOptions)
            {
                foreach (var bundleCode in travelOption.BundleCode)
                {
                    if (bundleCode.AssociatedTripIndex == tripId && !string.IsNullOrEmpty(bundleCode.ProductKey))
                    {
                        return bundleCode.ProductKey;
                    }
                }
            }

            return defaultFareBasisCode;
        }

        /* DeadCode Removed

        public async Task<List<MOBSeatMap>> GetSeatMapForRecordLocatorWithLastNameCSL(string sessionId,
             string recordLocator, int segmentIndex, string languageCode, string bookingCabin,
             bool cogStop, string origin, string flightnumber, string MarketingCarrier, string OperatingCarrier,
             string flightdate, string destination, string appVersion, bool isOaSeatMapSegment, bool isBasicEconomy,
             int noOfTravelersWithNoSeat1, int noOfFreeEplusEligibleRemaining, List<TripSegment> tripSegments, string deviceId, int applicationId = -1, bool returnPolarisLegendforSeatMap = false)
        {
            //need to get all seat maps if the flight is a through flight
            if (cogStop)
            {
                segmentIndex = 0;
            }

            if (!_shoppingUtility.EnableAirCanada(applicationId, appVersion) && OperatingCarrier != null
                  && OperatingCarrier.Trim().ToUpper() == "AC")
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines") != null ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines").ToString() : string.Empty);
            }

            var request = new Service.Presentation.FlightRequestModel.SeatMap();

            request.ConfirmationID = recordLocator;
            request.ArrivalAirport = destination;
            request.DepartureAirport = origin;
            request.FlightNumber = Convert.ToInt32(flightnumber);
            request.MarketingCarrier = MarketingCarrier;
            request.OperatingCarrier = !string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapForACSubsidiary")) ? _configuration.GetValue<string>("SeatMapForACSubsidiary").Contains(OperatingCarrier) ? "AC" : OperatingCarrier : OperatingCarrier;
            request.LanguageCode = languageCode;
            request.CabinType = "ALL";
            request.FlightDate = ShopStaticUtility.GetFormatedDateTime(flightdate);
            request.SegmentNumber = segmentIndex;
            request.Rules = BuildSeatMapRequestWithSegmentsCSL(tripSegments, segmentIndex);

            string _jsonRequest = JsonConvert.SerializeObject(request);
            var session = new Session();
            await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            string url = string.Empty;

            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("CSLService-ViewReservationChangeseats")))
            {
                url = _shoppingUtility.EnableSSA(applicationId, appVersion) ?
                     string.Format("{0}GetSeatMapDetailWithFare", _configuration.GetValue<string>("CSLService-ViewReservationChangeseats")) :
                     string.Format("{0}GetSeatMapDetail", _configuration.GetValue<string>("CSLService-ViewReservationChangeseats"));
            }

            // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatMapForRecordLocatorWithLastNameCSL", "Url", applicationId, appVersion, deviceId, url)); //CSL URL log
            // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatMapForRecordLocatorWithLastNameCSL", "CSL Request", applicationId, appVersion, deviceId, _jsonRequest)); //CSL request log
            // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatMapForRecordLocatorWithLastNameCSL", "CSL Token", applicationId, appVersion, deviceId, session.Token));//CSL Token log

            var _jsonResponse = await _seatEnginePostService.SeatEnginePostNew(sessionId, url, "application/xml;", session.Token, _jsonRequest).ConfigureAwait(false);

            // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetSeatMapForRecordLocatorWithLastNameCSL", "CSL Response", applicationId, appVersion, deviceId, _jsonResponse));//CSL response log

            List<MOBSeatMap> seatMaps = null;
            if (!string.IsNullOrEmpty(_jsonResponse))
            {
                FlightSeatMapDetail response = new FlightSeatMapDetail();
                response = JsonConvert.DeserializeObject<FlightSeatMapDetail>(_jsonResponse);

                if (response.SeatMap != null && response.SeatMap.SegmentSeatMap != null && response.SeatMap.SegmentSeatMap.Count > 0)
                {
                    seatMaps = new List<MOBSeatMap>();
                    foreach (Service.Presentation.CommonModel.AircraftModel.SeatMap segmentseatmap in response.SeatMap.SegmentSeatMap)
                    {
                        if (segmentseatmap.SegmentInfo.ArrivalAirport == destination && segmentseatmap.SegmentInfo.DepartureAirport == origin)
                        {
                            MOBSeatMap aSeatMap = await GetSeatMapCSL(segmentseatmap, response.Travelers.Count, bookingCabin, cogStop, sessionId, isBasicEconomy, noOfTravelersWithNoSeat1, noOfFreeEplusEligibleRemaining, isOaSeatMapSegment, session.Token, returnPolarisLegendforSeatMap, applicationId, appVersion);
                            seatMaps.Add(aSeatMap);
                        }
                    }
                }
            }
            else if (isOaSeatMapSegment)
            {
                if (_shoppingUtility.EnableAirCanada(applicationId, appVersion) && OperatingCarrier != null
                    && OperatingCarrier.Trim().ToUpper() == "AC")
                {
                    if (_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation") != null) // Using this "SeatMapUnavailableAC_Managereservation" message as a Toggle and also Message if HSG PNR Not Ticketed not working as expected so we can return the Common Message sent by Nora for AC Seat Map failure for Both Non ETicketed and for all other seat map exceptions.
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation").ToString());
                    }
                    //TO DO
                    //if (response.SeatMapResponse != null && response.SeatMapResponse.Errors != null
                    //&& CheckIsPNRNotETicketedErrorMessage(response.SeatMapResponse.Errors))
                    //{
                    //    throw new MOBUnitedException(_configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation"] != null ? _configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation"].ToString() : string.Empty);
                    //}
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation") != null ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation").ToString() : string.Empty);
                }
            }

            // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<List<MOBSeatMap>>(sessionId, "Response for SeatMap", "CUSTOMER_SEATMAP_RESPONSE", seatMaps));

            return seatMaps;
        }

        private Collection<SeatRuleParameter> BuildSeatMapRequestWithSegmentsCSL(List<TripSegment> tripSegments, int segmentIndex)
        {
            if (tripSegments == null)
                return null;

            var segmentsCopy = tripSegments.Clone();
            if (segmentIndex > 0)
            {
                segmentsCopy = tripSegments.FindAll(t => t.SegmentIndex == segmentIndex);
            }

            return BuildSeatMapRequestWithSegmentsCSL(segmentsCopy);
        }

        private Collection<SeatRuleParameter> BuildSeatMapRequestWithSegmentsCSL(List<TripSegment> segments)
        {
            if (segments == null)
                return null;

            var rules = new Collection<SeatRuleParameter>();
            foreach (var segment in segments)
            {
                var seatRule = new SeatRuleParameter();
                seatRule.Flight = new Service.Presentation.CommonModel.FlightProfile();
                seatRule.Flight.FlightNumber = segment.FlightNumber;
                seatRule.Flight.OperatingCarrierCode = segment.OperatingCarrier;
                seatRule.ProductCode = segment.ProductCode;
                seatRule.FareBasisCode = segment.FareBasisCode;
                //Need to add segment index and get the property from seats team.
                seatRule.Segment = segment.Departure.Code + segment.Arrival.Code;
                seatRule.Flight.DepartureAirport = segment.Departure.Code;
                seatRule.Flight.ArrivalAirport = segment.Arrival.Code;
                rules.Add(seatRule);
            }

            return rules;
        }

        public async Task<MOBSeatMap> GetSeatMapCSL(Service.Presentation.CommonModel.AircraftModel.SeatMap segmentSeatMap, int numberOfTravelers, string bookingCabin, bool cogStop, string sessionId, bool isBasicEconomy, int noOfTravelersWithNoSeat, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, string token = "", bool returnPolarisLegendforSeatMap = false, int applicationId = -1, string appVersion = "")
        {
            int countNoOfFreeSeats = 0;
            int countNoOfPricedSeats = 0;
            MOBSeatMap aSeatMap = new MOBSeatMap();
            aSeatMap.SeatMapAvailabe = true;
            aSeatMap.FlightNumber = Convert.ToInt32(segmentSeatMap.SegmentInfo.FlightNumber);
            DateTime departureTime;
            if (DateTime.TryParse(segmentSeatMap.SegmentInfo.DepartureDate, out departureTime))
            {
                aSeatMap.FlightDateTime = departureTime.ToString("MM/dd/yyyy hh:mm tt");
            }

            aSeatMap.Departure = new MOBAirport();
            aSeatMap.Departure.Code = segmentSeatMap.SegmentInfo.DepartureAirport;
            aSeatMap.Arrival = new MOBAirport();
            aSeatMap.Arrival.Code = segmentSeatMap.SegmentInfo.ArrivalAirport;
            aSeatMap.IsOaSeatMap = isOaSeatMapSegment;
            int numberOfCabins = segmentSeatMap.Aircraft.Cabins.Count > 3 ? 3 : segmentSeatMap.Aircraft.Cabins.Count;

            //POLARIS Cabin Branding SeatMapLegend
            bool IsPolarisBranding = _configuration.GetValue<bool>("IsPolarisCabinBrandingON");
            //Preferred seat ManageRes
            bool isPreferredZoneEnabled = _shoppingUtility.EnablePreferredZone(applicationId, appVersion); // Check Preferred Zone based on App version - Returns true or false // Kiran
            if (aSeatMap.IsOaSeatMap)
            {
                //POLARIS ON/OFF OLD CLIENT
                aSeatMap.SeatLegendId = (!returnPolarisLegendforSeatMap)
                                        ? await this.GetSeatMapLegendId(segmentSeatMap.SegmentInfo.DepartureAirport, segmentSeatMap.SegmentInfo.ArrivalAirport, numberOfCabins)
                                        : "";

            }

            aSeatMap.FleetType = segmentSeatMap.Aircraft.Model != null ? (segmentSeatMap.Aircraft.Model.Name != null ? segmentSeatMap.Aircraft.Model.Name : "") : "";
            aSeatMap.LegId = string.Empty;
            int cabinCount = 0;
            List<PcuUpgradeOption> upgradeOffers = null;
            if (_shoppingUtility.EnablePcuDeepLinkInSeatMap(applicationId, appVersion))
            {
                var pcu = await new Helper.SeatEngine.PremiumCabinUpgrade(_sessionHelperService, sessionId, segmentSeatMap.SegmentInfo.FlightNumber.ToString(), segmentSeatMap.SegmentInfo.DepartureAirport, segmentSeatMap.SegmentInfo.ArrivalAirport).LoadOfferStateforSeatMap();
                upgradeOffers = pcu.GetUpgradeOptionsForSeatMap();
                aSeatMap.Captions = await GetPcuCaptions(pcu.GetTravelerNames(), pcu.RecordLocator);
            }
          
            bool isEnablePCUSeatMapPurchaseManageRes = await IsEnablePCUSeatMapPurchaseManageRes(applicationId, appVersion, numberOfTravelers); /// Checking the version for enabling PCU seat map without deeplink

            foreach (var cabinInfo in segmentSeatMap.Aircraft.Cabins)
            {
                ++cabinCount;
                bool firstCabin = (cabinCount == 1);
                bool disableSeats = ShouldSeatsBeDisabled(cabinInfo.Name, bookingCabin, segmentSeatMap.Aircraft.Cabins.Count, applicationId, appVersion, isOaSeatMapSegment);

                SetPcuOfferAmountAndCabinNameForthisCabinCSL(cabinInfo.Name, upgradeOffers, isEnablePCUSeatMapPurchaseManageRes);

                MOBCabin aCabin = GetCabinCSL(cabinInfo, aSeatMap.SeatLegendId, firstCabin, disableSeats, cogStop, isBasicEconomy, isOaSeatMapSegment, ref countNoOfFreeSeats, ref countNoOfPricedSeats, isPreferredZoneEnabled, applicationId);

                if (cabinCount == 4)
                {
                    aSeatMap.Cabins.Insert(2, aCabin);
                    aCabin.COS = "Upper Deck " + aCabin.COS;
                }
                else
                {
                    aSeatMap.Cabins.Add(aCabin);
                }
            }

            if (aSeatMap.IsOaSeatMap && countNoOfFreeSeats == 0)
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
            }

            if (_configuration.GetValue<bool>("checkForPAXCount") && aSeatMap.IsOaSeatMap)
            {
                if (countNoOfFreeSeats <= numberOfTravelers)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
                }
            }

            if (aSeatMap.IsOaSeatMap)
            {
                aSeatMap.SeatLegendId = _configuration.GetValue<string>("SeatMapLegendForOtherAirlines");
            }
            else
            {
                #region Consuming CabinBranding Service
                //POLARIS Cabin Branding SeatMap FlightStatus
                if (IsPolarisBranding)
                {
                    #region
                    string fDate = departureTime.ToString(); //flightDate.ToString("yyyy-MM-dd");
                    var cabinBrandingDescriptions = await GetPolarisCabinBranding(token, segmentSeatMap.SegmentInfo.FlightNumber.ToString(), segmentSeatMap.SegmentInfo.DepartureAirport, fDate, segmentSeatMap.SegmentInfo.ArrivalAirport, numberOfCabins.ToString(), "en-US", sessionId, "UA", "UA");
                    if (cabinBrandingDescriptions != null && cabinBrandingDescriptions.Count > 0)
                    {
                        //aSeatMap.SeatLegendId = Utility.GetPolarisCabinBrandingSeatMapLegendId(cabinBrandingDescriptions);
                        //POLARIS ON NEW CLIENT  - SeatMapLegendID with Pipe and Polaris Names
                        if (returnPolarisLegendforSeatMap)
                        {
                            aSeatMap.SeatLegendId = await GetPolarisSeatMapLegendId(segmentSeatMap.SegmentInfo.DepartureAirport, segmentSeatMap.SegmentInfo.ArrivalAirport, numberOfCabins, cabinBrandingDescriptions, applicationId, appVersion);
                        }
                        foreach (MOBCabin mc in aSeatMap.Cabins)
                        {
                            if (aSeatMap.Cabins.Count > 3 && mc.COS.ToUpper().Contains("UPPER DECK"))
                            {
                                aSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                aSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                                aSeatMap.Cabins[2].COS = "Upper Deck " + cabinBrandingDescriptions[1].ToString();
                                aSeatMap.Cabins[3].COS = cabinBrandingDescriptions[2].ToString();
                            }
                            else if (aSeatMap.Cabins.Count == 3)
                            {
                                aSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                aSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                                aSeatMap.Cabins[2].COS = cabinBrandingDescriptions[2].ToString();
                            }
                            else if (aSeatMap.Cabins.Count == 2)
                            {
                                aSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                                aSeatMap.Cabins[1].COS = cabinBrandingDescriptions[1].ToString();
                            }
                            else
                            {
                                aSeatMap.Cabins[0].COS = cabinBrandingDescriptions[0].ToString();
                            }
                        }
                    }
                    else
                    {
                        throw new MOBUnitedException("Cabin Branding Service returned null");
                    }
                    #endregion
                }
                else if (returnPolarisLegendforSeatMap)
                {
                    //POLARIS OFF - NEW CLIENT - SeatMapLegendID with Pipe and Non-Polaris branding
                    aSeatMap.SeatLegendId = await GetPolarisSeatMapLegendIdPolarisOff(segmentSeatMap.SegmentInfo.DepartureAirport, segmentSeatMap.SegmentInfo.ArrivalAirport, numberOfCabins);
                }
                #endregion Consuming CabinBranding Service

                string[] erjFleetTypes = _configuration.GetValue<string>("ERJSeatFixFleetTypes").Split('|');
                string[] erjSeats = _configuration.GetValue<string>("ERJSeatFix").Split('|');
                string seatFix1 = string.Empty; string seatFix2 = string.Empty;

                for (int k = 0; k < erjFleetTypes.Length; k++)
                {
                    if (aSeatMap.FleetType == erjFleetTypes[k])
                    {
                        seatFix1 = erjSeats[k].Split(',')[0];
                        seatFix2 = erjSeats[k].Split(',')[1];
                        foreach (MOBCabin cabin in aSeatMap.Cabins)
                        {
                            for (int i = 0; i < cabin.Rows.Count; i++)
                            {
                                for (int j = 0; j < cabin.Rows[i].Seats.Count; j++)
                                {
                                    if (cabin.Rows[i].Seats[j].Number == seatFix1 || cabin.Rows[i].Seats[j].Number == seatFix2)
                                    {
                                        cabin.Rows[i].Seats[j].seatvalue = "-";

                                    }
                                    if (i == 0)
                                    {
                                        cabin.Rows[i].Seats[j].Exit = false;
                                    }
                                }
                                break;
                            }

                        }
                    }
                }
            }
            aSeatMap.ShowInfoMessageOnSeatMap = aSeatMap.IsOaSeatMap ?
                                                _configuration.GetValue<string>("ShowFreeSeatsMessageForOtherAilines") :
                                                await ShowNoFreeSeatsAvailableMessage(noOfTravelersWithNoSeat, noOfFreeEplusEligibleRemaining, countNoOfFreeSeats, countNoOfPricedSeats, isBasicEconomy);
            if (_configuration.GetValue<bool>("DisableFreeSeatMessageChanges") == false && aSeatMap.IsOaSeatMap == false
                         && string.IsNullOrEmpty(aSeatMap.ShowInfoMessageOnSeatMap) == false)
                aSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("NoFreeSeatAvailableMessageHeader");
            EconomySeatsForBUSService(aSeatMap);

            return aSeatMap;
        }

        private async Task<List<MOBItem>> GetPcuCaptions(string travelerNames, string recordLocator)
        {
            var pcuCaptions = await _productInfoHelper.GetCaptions("PCU_IN_SEATMAP_PRODUCTPAGE");
            if (pcuCaptions == null || !pcuCaptions.Any() || string.IsNullOrEmpty(travelerNames))
                return null;

            pcuCaptions.Add(new MOBItem { Id = "PremiumSeatTravelerNames", CurrentValue = travelerNames });
            pcuCaptions.Add(new MOBItem { Id = "RecordLocator", CurrentValue = recordLocator });
            return pcuCaptions;
        }

        private bool ShouldSeatsBeDisabled(string cabinName, string bookingCabin, int cabinCount, int applicationId, string appVersion, bool isOaSeatMapSegment)
        {
            cabinName = string.IsNullOrEmpty(cabinName) ? "" : cabinName.ToUpper().Trim();
            bool disableSeats = true;
            if (cabinName.Contains("FIRST") && (bookingCabin.Equals("First") || bookingCabin.Equals("United First") || bookingCabin.Equals("United Global First") || bookingCabin.ToUpper().Equals(seatMapLegendEntry1.Substring(1).ToUpper())))
            {
                disableSeats = false;
            }
            else if (cabinName.Contains("BUSINESS") && (bookingCabin.Equals("Business") || bookingCabin.Equals("United Business") || bookingCabin.Equals("BusinessFirst") || bookingCabin.Equals("United BusinessFirst") || bookingCabin.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper())))
            {
                disableSeats = false;
            }
            else if ((cabinName.Contains("ECONOMY") || isOaSeatMapSegment && cabinName.Equals("PREMIUM ECONOMY")) && (bookingCabin.Equals("Coach") || bookingCabin.Equals("Economy") || bookingCabin.Equals("United Economy")))
            {
                disableSeats = false;
            }
            else if ((cabinName.Contains("UNITED PREMIUM PLUS") || cabinName.Contains("UNITED PREMIUMPLUS")) && bookingCabin.ToUpper().Trim().Equals("UNITED PREMIUM PLUS"))
            {
                disableSeats = false;
            }
            else if (_shoppingUtility.IsUPPSeatMapSupportedVersion(applicationId, appVersion)
                      && cabinName.Contains("PREMIUM ECONOMY")
                      && bookingCabin.ToUpper().Equals((_configuration.GetValue<string>("MRESPremiumEconomyCabinForOASeatMapEnableToggleText") ?? "").ToUpper()))
            {
                disableSeats = false;
            }

            if (cabinCount == 2)
            {
                if (cabinName.Contains("FIRST") && (bookingCabin.Equals("Business") || bookingCabin.Equals("United Business") || bookingCabin.Equals("BusinessFirst") || bookingCabin.Equals("United BusinessFirst") || bookingCabin.ToUpper().Equals(seatMapLegendEntry2.Substring(1).ToUpper())))
                {
                    disableSeats = false;
                }
            }

            return disableSeats;
        }

        private void SetPcuOfferAmountAndCabinNameForthisCabinCSL(string seatmapCabin, List<PcuUpgradeOption> upgradeOffers, bool isEnablePCUSeatMapPurchaseManageRes)
        {
            //reset the values
            pcuOfferAmountForthisCabin = string.Empty;
            cabinName = string.Empty;

            if (upgradeOffers == null || !upgradeOffers.Any() || seatmapCabin.ToUpper().Trim().Contains("ECONOMY"))
                return;

            var upgradeOffer = upgradeOffers.FirstOrDefault(u => IsCabinMatchedCSL(u.UpgradeOptionDescription, seatmapCabin));
            if (upgradeOffer == null)
                return;

            pcuOfferAmountForthisCabin = string.Format("{0}.00", upgradeOffer.FormattedPrice);
            pcuOfferPriceForthisCabin = isEnablePCUSeatMapPurchaseManageRes ? upgradeOffer.Price : 0;
            cabinName = upgradeOffer.UpgradeOptionDescription;
            pcuOfferOptionId = _configuration.GetValue<bool>("TurnOff_DefaultSelectionForUpgradeOptions") ? string.Empty : upgradeOffer.OptionId;
        }

        private MOBCabin GetCabinCSL(Service.Presentation.CommonModel.AircraftModel.Cabin cabinInfo, string seatLegendId, bool firstCabin, bool disableSeats, bool cogStop, bool disableEplusSeats, bool isOaSeatMapSegment, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats, bool isPreferredZoneEnabled, int applicationId)
        {
            MOBCabin aCabin = new MOBCabin();
            aCabin.COS = isOaSeatMapSegment ? GetOaCabinCOS(cabinInfo) : cabinInfo.Name;
            aCabin.Configuration = cabinInfo.Layout;
            //aCabin.COS = isOaSeatMapSegment ?
            //            GetOaCabinCOS(cabinInfo) :
            //            GetCabinCOSCSL(cabinInfo, seatLegendId, firstCabin); //ToDo 
            bool isOaPremiumEconomyCabin = cabinInfo.Name.Equals("Premium Economy");
            foreach (United.Service.Presentation.CommonModel.AircraftModel.SeatRow row in cabinInfo.SeatRows)
            {
                MOBRow aRow = GetSeatRowCSL(row, aCabin.Configuration, disableSeats, cogStop, disableEplusSeats, isOaSeatMapSegment, isOaPremiumEconomyCabin, ref countNoOfFreeSeats, ref countNoOfPricedSeats, isPreferredZoneEnabled, applicationId);
                if (aRow != null)
                {
                    if (aRow.Seats == null || aRow.Seats.Count != cabinInfo.Layout.Length)
                    {
                        if (isOaSeatMapSegment)
                            throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                        throw new MOBUnitedException(_configuration.GetValue<string>("GenericExceptionMessage"));
                    }
                    aCabin.Rows.Add(aRow);
                }
            }
            aCabin.Configuration = aCabin.Configuration.Replace(" ", "-");

            return aCabin;
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

        private MOBRow GetSeatRowCSL(Service.Presentation.CommonModel.AircraftModel.SeatRow row, string configuration, bool disableSeats, bool cogStop, bool disableEplusSeats, bool isOaSeatMapSegment, bool isOaPremiumEconomyCabin, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats, bool isPreferredZoneEnabled, int applicationId)
        {
            MOBRow aRow = null;

            int rowNumber = row != null ? row.RowNumber : 0;

            if (row != null && rowNumber < 1000)
            {
                aRow = new MOBRow();
                aRow.Number = row.RowNumber.ToString();
                if (!isOaSeatMapSegment)
                    aRow.Wing = CheckSeatRRowType(row.Characteristics, "IsWing");
                int seatCol = 0;
                foreach (Service.Presentation.CommonModel.AircraftModel.Seat seat in row.Seats)
                {
                    MOBSeatB aSeat = null;
                    string seatVal = configuration.Substring(seatCol, 1);
                    if (string.IsNullOrEmpty(seatVal) || seatVal.Equals(" "))
                    {
                        aSeat = new MOBSeatB();
                        aSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit");
                        aSeat.Number = string.Empty;
                        aSeat.Fee = string.Empty;
                        aSeat.LimitedRecline = false;
                        aSeat.seatvalue = "-";
                        aRow.Seats.Add(aSeat);
                        seatCol++;
                    }

                    aSeat = new MOBSeatB();
                    aSeat.Exit = CheckSeatRRowType(seat.Characteristics, "IsExit");
                    aSeat.Number = seat.Identifier;
                    aSeat.Fee = string.Empty;
                    aSeat.LimitedRecline = !isOaSeatMapSegment && (CheckSeatRRowType(seat.Characteristics, "IsEconomyPlusPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsPrimeSeat") || CheckSeatRRowType(seat.Characteristics, "IsBulkheadPrime") || (isPreferredZoneEnabled && IsPreferredSeatLimitedRecline(seat.Characteristics)));
                    bool isEplus = false;
                    if (isPreferredZoneEnabled)
                    {
                        aSeat.seatvalue = isOaSeatMapSegment ?
                                            GetOaSeatType(seat.Characteristics, disableSeats, isOaPremiumEconomyCabin, out isEplus) :
                                            GetSeatTypeWithPreferred(seat, disableSeats, applicationId, out isEplus, disableEplusSeats);
                    }
                    else
                    {
                        aSeat.seatvalue = isOaSeatMapSegment ?
                                          GetOaSeatType(seat.Characteristics, disableSeats, isOaPremiumEconomyCabin, out isEplus) :
                                          getSeatType(seat.Characteristics, false, disableSeats, applicationId, out isEplus, disableEplusSeats);
                    }

                    aSeat.IsEPlus = isEplus;

                    if (!isOaSeatMapSegment && seat.Characteristics != null)
                    {
                        string rearFacingSeat = string.Empty, frontFacingSeat = string.Empty, leftFacingSeat = string.Empty, rightFacingSeat = string.Empty;
                        foreach (var characteristic in seat.Characteristics)
                        {
                            if (applicationId == 16)
                            {
                                Characteristic c = new Characteristic();
                                c.Code = characteristic.Code;
                                c.Value = characteristic.Value;
                                if (aSeat.Characteristics == null)
                                {
                                    aSeat.Characteristics = new List<Characteristic>();
                                }
                                aSeat.Characteristics.Add(c);
                            }

                            GetSeatPositionAccess(ref rearFacingSeat, ref frontFacingSeat, ref leftFacingSeat, ref rightFacingSeat, characteristic);
                        }
                        aSeat.Program = rearFacingSeat + "|" + frontFacingSeat + "|" + leftFacingSeat + "|" + rightFacingSeat;
                        aSeat.Program = aSeat.Program.Trim('|');
                    }

                    aSeat.ServicesAndFees = GetServicesAndFeesCSL(seat, aSeat.Program);
                    if (!isOaSeatMapSegment && aSeat.ServicesAndFees != null && aSeat.ServicesAndFees.Count > 0)
                    {
                        aSeat.IsEPlus = aSeat.IsEPlus || CheckEPlusSeatCode(aSeat.ServicesAndFees[0].Program);
                        if (aSeat.IsEPlus && aSeat.seatvalue == "O")
                        {
                            aSeat.seatvalue = disableEplusSeats ? "X" : "P";
                        }
                    }
                    aSeat.PcuOfferPrice = GetPcuOfferPrice(aSeat);
                    aSeat.IsPcuOfferEligible = IsPcuOfferred(aSeat);
                    aSeat.PcuOfferOptionId = GetPcuOfferOptionId(aSeat);
                    aSeat.DisplaySeatFeature = GetDisplaySeatFeature(isOaSeatMapSegment, isPreferredZoneEnabled, aSeat);
                    CountNoOfFreeSeatsAndPricedSeats(aSeat, ref countNoOfFreeSeats, ref countNoOfPricedSeats);

                    aRow.Seats.Add(aSeat);
                    seatCol++;
                }
            }

            return aRow;
        }

        private List<ServicesAndFees> GetServicesAndFeesCSL(Service.Presentation.CommonModel.AircraftModel.Seat seat, string Program)
        {
            List<ServicesAndFees> serviceAndFees = null;
            ServicesAndFees serviceAndFee = null;
            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin > 0)
            {
                serviceAndFee = new ServicesAndFees();
                serviceAndFee.SeatNumber = seat.Identifier;
                serviceAndFee.Program = Program; // aSeat.Program will be empty and to assign program code for higher cabins to upgrade seat
                serviceAndFee.TotalFee = Convert.ToDecimal(pcuOfferPriceForthisCabin.ToString("0.00"));
                if (_shoppingUtility.IsMilesFOPEnabled())
                {
                    serviceAndFee.TotalMiles = 0;
                    serviceAndFee.DisplayTotalMiles = string.Empty;
                }
            }
            else if (seat.Price != null)
            {
                serviceAndFee = new ServicesAndFees();
                if (seat.Characteristics != null)
                {
                    foreach (Service.Presentation.CommonModel.Characteristic seatAttr in seat.Characteristics)
                    {
                        switch (seatAttr.Code)
                        {
                            case "SeatSection":
                                {
                                    break;
                                }
                            case "SeatLocation":
                                {
                                    break;
                                }
                            case "IsHeld":
                                {
                                    break;
                                }
                            case "IsOccupiedSeat":
                                {
                                    serviceAndFee.Available = true;
                                    if (seatAttr.Value.ToUpper().Equals("TRUE"))
                                    {
                                        serviceAndFee.Available = false;
                                    }

                                    break;
                                }
                            case "IsAvailableSeat":
                                {
                                    serviceAndFee.Available = true;
                                    break;
                                }
                            case "IsAdvanced":
                                {
                                    break;
                                }
                            case "ColSpan":
                                {
                                    break;
                                }
                            case "RowSpan":
                                {
                                    break;
                                }
                            case "DisplaySeatType":
                                {
                                    serviceAndFee.SeatFeature = seatAttr.Value;
                                    break;
                                }
                            case "SharesSeatType":
                                {
                                    break;
                                }
                        }
                    }
                }
                //if (Utility.IsMilesFOPEnabled())
                //{
                //    serviceAndFee.TotalMiles = Convert.ToInt32(_configuration.GetValue<string>("milesFOP"]);
                //    serviceAndFee.DisplayTotalMiles = Utility.formatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"], false);
                //}

                serviceAndFee.AgentDutyCode = string.Empty;
                serviceAndFee.AgentId = string.Empty;
                serviceAndFee.AgentTripleA = string.Empty;

                serviceAndFee.BaseFee = Convert.ToDecimal(seat.Price.BasePrice.Amount.ToString("0.00"));
                serviceAndFee.Currency = seat.Price.BasePrice.Currency.Code;
                serviceAndFee.TotalFee = Convert.ToDecimal(seat.Price.Totals[0].Amount.ToString("0.00"));
                //serviceAndFee.EliteStatus = seat.Characteristics;               
                serviceAndFee.Program = seat.Price.PromotionCode;
                serviceAndFee.SeatNumber = seat.Identifier;
                if (seat.Price.Taxes != null)
                {
                    if (seat.Price.Taxes.Count > 0)
                    {
                        serviceAndFee.Tax = Convert.ToDecimal(seat.Price.Taxes[0].Amount);
                    }
                }
            }
            if (serviceAndFee != null)
            {
                serviceAndFees = new List<ServicesAndFees>();
                serviceAndFees.Add(serviceAndFee);
            }

            return serviceAndFees;
        }

        private string GetPcuOfferPrice(MOBSeatB aSeat)
        {
            return aSeat.seatvalue == "O" ? pcuOfferAmountForthisCabin : null;
        }

        private bool IsPcuOfferred(MOBSeatB aSeat)
        {
            return aSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin == 0;
        }

        private string GetPcuOfferOptionId(MOBSeatB aSeat)
        {
            return aSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) ? pcuOfferOptionId : null;
        }

        private string GetDisplaySeatFeature(bool isOaSeatMapSegment, bool isPreferredZoneEnabled, MOBSeatB aSeat)
        {
            if (isOaSeatMapSegment)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && aSeat.seatvalue == "O" && !string.IsNullOrEmpty(cabinName))
                return cabinName;

            if (aSeat.IsEPlus)
                return "Economy Plus";

            if (isPreferredZoneEnabled && aSeat.seatvalue == "PZ")
                return "Preferred Seat";

            return string.Empty;
        }

        */

    }
}
