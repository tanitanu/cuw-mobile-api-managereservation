using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper.Shopping;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Common;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.ManageRes;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using Genre = United.Service.Presentation.CommonModel.Genre;

namespace United.Common.Helper.ManageRes
{
    public class ManageResUtility : IManageResUtility
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IHeaders _headers;
        private readonly ICacheLog _logger;

        public ManageResUtility(IConfiguration configuration
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IDynamoDBService dynamoDBService
            , IHeaders headers
            , ICacheLog logger
            )
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
             _logger = logger;

        }
        public bool IsVersionEnableMP2015LMXCallOrFareLock(string appVersion, string configVersion)
        {
            try
            {
                if (configVersion.IndexOf(appVersion) != -1)
                {
                    return true;
                }
                else
                {
                    Regex regex = new Regex("[0-9.]");
                    appVersion = string.Join("", regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                    configVersion = string.Join("", regex.Matches(configVersion).Cast<Match>().Select(match => match.Value).ToArray());

                    string[] version1Arr = appVersion.Trim().Split('.');
                    string[] version2Arr = configVersion.Trim().Split('.');

                    if (Convert.ToInt32(version1Arr[0]) > Convert.ToInt32(version2Arr[0]))
                    {
                        return true;
                    }
                    else if (Convert.ToInt32(version1Arr[0]) == Convert.ToInt32(version2Arr[0]))
                    {
                        if (Convert.ToInt32(version1Arr[1]) > Convert.ToInt32(version2Arr[1]))
                        {
                            return true;
                        }
                        else if (Convert.ToInt32(version1Arr[1]) == Convert.ToInt32(version2Arr[1]))
                        {
                            if (Convert.ToInt32(version1Arr[2]) > Convert.ToInt32(version2Arr[2]))
                            {
                                return true;
                            }
                            else if (Convert.ToInt32(version1Arr[2]) == Convert.ToInt32(version2Arr[2]))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public string SpecialcharacterFilterInPNRLastname(string stringTofilter)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringTofilter))
                {
                    Regex regex = new Regex(_configuration.GetValue<string>("SpecialcharactersFilterInPNRLastname"));
                    return regex.Replace(stringTofilter, string.Empty);
                }
                else
                    return stringTofilter;
            }
            catch (Exception ex) { return stringTofilter; }
        }

        public bool EnableActiveFutureFlightCreditPNR(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneActiveFutureFlightCreditPNRVersion", "AndroidActiveFutureFlightCreditPNRVersion", "", "", true, _configuration);
        }

        public string GetCurrencyCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "";

            string currencyCodeMappings = _configuration.GetValue<string>("CurrencyCodeMappings");

            var ccMap = currencyCodeMappings.Split('|');
            string currencyCode = string.Empty;

            foreach (var item in ccMap)
            {
                if (item.Split('=')[0].Trim() == code.Trim())
                {
                    currencyCode = item.Split('=')[1].Trim();
                    break;
                }
            }
            if (string.IsNullOrEmpty(currencyCode))
                return code;
            else
                return currencyCode;
        }
        public string GetCurrencyAmount(double value = 0, string code = "USD", int decimalPlace = 2, string languageCode = "")
        {

            string isNegative = value < 0 ? "- " : "";
            double amount = Math.Abs(value);

            if (string.IsNullOrEmpty(code))
                code = "USD";

            string currencyCode = GetCurrencyCode(code);

            //Handle the currency code which is not in the app setting key - CurrencyCodeMappings
            if (string.IsNullOrEmpty(currencyCode))
                currencyCode = code;

            double.TryParse(amount.ToString(), out double total);
            string currencyAmount = "";

            if (string.Equals(currencyCode, "Miles", StringComparison.OrdinalIgnoreCase))
            {
                currencyAmount = string.Format("{0} {1}", total.ToString("#,##0"), currencyCode);
            }
            else if (languageCode == "")
            {
                currencyAmount = string.Format("{0}{1}{2}", isNegative, currencyCode, total.ToString("N" + decimalPlace));
            }
            else
            {
                CultureInfo locCutlure = new CultureInfo(languageCode);
                Thread.CurrentThread.CurrentCulture = locCutlure;
                NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                LocalFormat.CurrencySymbol = currencyCode;
                LocalFormat.CurrencyDecimalDigits = 2;

                currencyAmount = string.Format("{0}{1}", isNegative, amount.ToString("c", LocalFormat));

            }

            return currencyAmount;
        }
        public bool EnableFareLockPurchaseViewRes(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableFareLockPurchaseViewRes")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidFareLockPurchaseViewResVersion", "iPhoneFareLockPurchaseViewResVersion", "", "", true, _configuration);
            }
            return false;
        }
        public int fareLockDate(string fareLockCreatedDate, string fareLockExpireDate)
        {
            int days = 0;
            if (!string.IsNullOrEmpty(fareLockCreatedDate) && !string.IsNullOrEmpty(fareLockExpireDate))
            {
                DateTime farelockExpiryDateObj;
                DateTime.TryParse(fareLockExpireDate, out farelockExpiryDateObj);
                DateTime fareLockCreatedDateObj;
                DateTime.TryParse(fareLockCreatedDate, out fareLockCreatedDateObj);
                double day = (farelockExpiryDateObj - fareLockCreatedDateObj).TotalDays;
                days = day != null ? Convert.ToInt32(day) : 0;
                return days;
            }
            return days;
        }
        public string TicketingCountryCode(United.Service.Presentation.CommonModel.PointOfSale pointOfSale)
        {
            return pointOfSale != null && pointOfSale.Country != null ? pointOfSale.Country.CountryCode : string.Empty;
        }
        public bool IsRevenue(Collection<Genre> types)
        {
            return types != null && types.Any(IsRevenue);
        }
        private bool IsRevenue(Genre type)
        {
            return type.Description != null && type.Key != null && type.Description.ToUpper().Trim().Equals("ITIN_TYPE") && type.Key.ToUpper().Trim().Equals("REVENUE");
        }
        public Boolean CheckEmpPassengerIndicator
          (Service.Presentation.ValueDocumentModel.ValueDocument ticket)
        {
            string passengerindicatorcharkey = "PassengerIndicator";
            string passengerindicatorconfigkey = "emppassengerindicator";
            try
            {
                if (ticket == null) return false;
                var passengerindicator = GetCharactersticValue(ticket.Characteristic, passengerindicatorcharkey);
                var emppassengerindicator = GetListFrmPipelineSeptdConfigString(passengerindicatorconfigkey);
                if (string.IsNullOrEmpty(passengerindicator)
                    || emppassengerindicator == null
                    || !emppassengerindicator.Any()) return false;

                return emppassengerindicator.Contains(passengerindicator);
            }
            catch { return false; }
        }
        public string GetCharactersticValue(Collection<Service.Presentation.CommonModel.Characteristic> characteristics, string code)
        {
            if (characteristics == null || characteristics.Count <= 0) return string.Empty;
            var characteristic = characteristics.FirstOrDefault(c => c != null && c.Code != null
            && !string.IsNullOrEmpty(c.Code) && c.Code.Trim().Equals(code, StringComparison.InvariantCultureIgnoreCase));
            return characteristic == null ? string.Empty : characteristic.Value;
        }
        public List<string> GetListFrmPipelineSeptdConfigString(string configkey)
        {
            try
            {
                var retstrarray = new List<string>();
                var configstring = _configuration.GetValue<string>(configkey);
                if (!string.IsNullOrEmpty(configstring))
                {
                    string[] strarray = configstring.Split('|');
                    if (strarray.Any())
                    {
                        strarray.ToList().ForEach(str =>
                        {
                            if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                                retstrarray.Add(str.Trim());
                        });
                    }
                }
                return (retstrarray.Any()) ? retstrarray : null;
            }
            catch { return null; }
        }
        public async Task ValidateTripsandSegments(Collection<ReservationFlightSegment> flightSegments, string recordLocator, string lastName, string jsonPayLoad, int applicationId, string deviceId, string appVersion, string guid)
        {
            try
            {
                if (flightSegments.Any(sg => sg.SegmentNumber <= 0) || flightSegments.Any(sg => Convert.ToInt32(sg.TripNumber) <= 0))
                {
                    await System.Threading.Tasks.Task.Factory.StartNew(async() => await InsertPayLoad(recordLocator, lastName, jsonPayLoad, applicationId, deviceId, appVersion, guid));
                    // InsertPayLoad(recordLocator, lastName, jsonPayLoad, applicationId, deviceId, appVersion, guid);
                }
            }
            catch
            {

            }
        }

        public async Task InsertPayLoad(string recordLocator, string lastName, string jsonPayLoad, int applicationId, string deviceId, string appVersion, string guid)
        {
            var fitBitDynanmoDB = new FitbitDynamoDB(_configuration, _dynamoDBService);
            var savePayload = new Payload()
            {
                recordLocator = recordLocator,
                lastName = lastName,
                jsonPayLoad = jsonPayLoad,
                applicationId = applicationId,
                deviceId = deviceId,
                appVersion = appVersion,
                guid = guid
            };
            var key = recordLocator + "::" + deviceId;
            var returnValue = await fitBitDynanmoDB.InsertPayLoad<Payload>(savePayload, key, string.Empty).ConfigureAwait(false);
        }

        public void GetCheckInEligibilityStatusFromCSLPnrReservation(Collection<United.Service.Presentation.CommonEnumModel.CheckinStatus> checkinEligibilityList, ref MOBPNR pnr)
        {
            bool SegmentFlownCheckToggle = Convert.ToBoolean(_configuration.GetValue<string>("SegmentFlownCheckToggle") ?? "false");

            pnr.CheckInStatus = "0";
            pnr.IrrOps = false;
            pnr.IrrOpsViewed = false;

            if (checkinEligibilityList != null && (pnr.Segments != null && pnr.Segments.Count > 0))
            {
                int hours = Convert.ToInt32(_configuration.GetValue<string>("PNRStatusLeadHours")) * -1;
                bool isNotFlownSegmentExist = IsNotFlownSegmentExist(pnr.Segments, hours, SegmentFlownCheckToggle);

                if ((!SegmentFlownCheckToggle && Convert.ToDateTime((pnr.Segments[0].ScheduledDepartureDateTimeGMT)).AddHours(hours) < DateTime.UtcNow) ||
                    (SegmentFlownCheckToggle && isNotFlownSegmentExist))
                {
                    foreach (var checkinEligibility in checkinEligibilityList)
                    {
                        if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.AlreadyCheckedin)
                        {
                            pnr.CheckInStatus = "2"; //"AlreadyCheckedin";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.CheckinEligible)
                        {
                            pnr.CheckInStatus = "1"; //"CheckInEligible";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.IRROPS)
                        {
                            pnr.IrrOps = true; //"IRROPS";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.IRROPS_VIEWED)
                        {
                            pnr.IrrOpsViewed = true; //"IRROPS_VIEWED";
                        }
                    }
                }
            }
        }
        private bool IsNotFlownSegmentExist(List<MOBPNRSegment> segments, int hours, bool SegmentFlownCheckToggle)
        {
            bool isNotFlownSegmentExist = false;
            try
            {
                if (SegmentFlownCheckToggle)
                {
                    string segmentTicketCouponStatusCodes = (_configuration.GetValue<string>("SegmentTicketCouponStatusCodes") ?? "");
                    isNotFlownSegmentExist = segments.Exists(segment => (string.IsNullOrEmpty(segment.TicketCouponStatus) ||
                                                                            (segmentTicketCouponStatusCodes != string.Empty &&
                                                                             !string.IsNullOrEmpty(segment.TicketCouponStatus) &&
                                                                             !segmentTicketCouponStatusCodes.Contains(segment.TicketCouponStatus)
                                                                            )
                                                                        ) &&
                                                                        Convert.ToDateTime((segment.ScheduledDepartureDateTimeGMT)).AddHours(hours) < DateTime.UtcNow);
                }
            }
            catch
            {
                isNotFlownSegmentExist = false;
            }
            return isNotFlownSegmentExist;
        }
        public bool IsElfSegment(MOBPNRSegment pnrSegment)
        {
            if (pnrSegment == null) return false;
            return pnrSegment.MarketingCarrier != null &&
                IsElfSegment(pnrSegment.MarketingCarrier.Code, pnrSegment.ClassOfService);
        }
        private bool IsElfSegment(string marketingCarrier, string serviceClass)
        {
            return marketingCarrier == "UA" &&
                   serviceClass == "N";
        }
        public string ConvertListToString(List<string> inputList)
        {
            try
            {
                if (inputList != null && inputList.Any())
                    inputList.RemoveAll(x => (string.IsNullOrWhiteSpace(x) || string.IsNullOrEmpty(x)));

                if (inputList != null && inputList.Any())
                {
                    if (inputList.Count == 1) return inputList[0];
                    else if (inputList.Count == 2)
                        return string.Join(", ", inputList.Take(inputList.Count() - 1)) + " and " + inputList.Last();
                    else
                        return string.Join(", ", inputList.Take(inputList.Count() - 1)) + ", and " + inputList.Last();
                }
                else
                    return string.Empty;
            }
            catch (Exception ex) { return string.Empty; }
        }
        public bool IsIBEFullFare(string productCode)
        {
            var iBEFullProductCodes = _configuration.GetValue<string>("IBEFullShoppingProductCodes");
            return _configuration.GetValue<bool>("EnablePBE") && !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBEFullProductCodes) &&
                   iBEFullProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        public string[] SplitConcatenatedConfigValue(string configkey, string splitchar)
        {
            try
            {
                string[] splitSymbol = { splitchar };
                string[] splitString = _configuration.GetValue<string>(configkey)
                    .Split(splitSymbol, StringSplitOptions.None);
                return splitString;
            }
            catch { return null; }
        }

        public bool EnableUMNRInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneUMNRInformationVersion", "AndroidUMNRInformationVersion", "", "", true, _configuration);
        }

        public MOBPNRAdvisory PopulateTRCAdvisoryContent(string displaycontent)
        {
            try
            {
                string[] stringarray
                    = SplitConcatenatedConfigValue("ManageResTRCContent", "||");

                if (stringarray == null || !stringarray.Any()) return null;

                MOBPNRAdvisory content = new MOBPNRAdvisory();

                stringarray.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] lineitem = ShopStaticUtility.SplitConcatenatedString(item, "|");

                        if (lineitem?.Length > 1)
                        {
                            switch (lineitem[0])
                            {
                                case "Header":
                                    content.Header = lineitem[1];
                                    break;
                                case "Body":
                                    content.Body = lineitem[1];
                                    break;
                                case "ButtonText":
                                    content.Buttontext = lineitem[1];
                                    break;
                            }
                        }
                    }
                });
                return content;
            }
            catch { return null; }
        }

        public bool EnablePetInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhonePetInformationVersion", "AndroidPetInformationVersion", "", "", true, _configuration);
        }

        public bool IncludeReshopFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableReshopFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }
        public bool CheckMax737WaiverFlight
          (PNRChangeEligibilityResponse changeEligibilityResponse)
        {
            if (changeEligibilityResponse == null
                || changeEligibilityResponse.Policies == null
                || !changeEligibilityResponse.Policies.Any()) return false;

            foreach (var policies in changeEligibilityResponse.Policies)
            {
                var max737flightnames = GetListFrmPipelineSeptdConfigString("max737flightnames");
                string flightname = (!string.IsNullOrEmpty(policies.Name)) ? policies.Name.ToUpper() : string.Empty;
                if (max737flightnames != null && max737flightnames.Any() && !string.IsNullOrEmpty(flightname))
                {
                    foreach (string name in max737flightnames)
                    {
                        if (flightname.Contains(name))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public async Task<List<MOBItem>> GetDBDisplayContent(string contentname)
        {
            List<MOBItem> mobcontentitem;
            if (string.IsNullOrEmpty(contentname)) return null;
            try
            {
                var messageitems = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(contentname, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
                if (messageitems != null && messageitems.Any())
                {
                    mobcontentitem = new List<MOBItem>();
                    foreach (MOBLegalDocument doc in messageitems)
                    {
                        mobcontentitem.Add(new MOBItem() { Id = doc.Title, CurrentValue = doc.LegalDocument });
                    }
                    return mobcontentitem;
                }
            }
            catch (Exception ex) { return null; }
            return null;
        }

        public void OneTimeSCChangeCancelAlert(MOBPNR pnr, int appId, string appVersion)
        {
            try
            {
                //Utility.isApplicationVersionGreaterorEqual(appId, appVersion, "iPhone_OneTimeSCChangeCancelAlertVersion", "Android_OneTimeSCChangeCancelAlertVersion");
                if (pnr.IsSCChangeEligible)
                {
                    string[] onetimecontent;
                    List<MOBItem> buttonItems = new List<MOBItem>();
                    var scheduleChangeConfigValue = _configuration.GetValue<bool>("EnableScheduleChangeAlertNavigation");
                    var isNewVersion = GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("iPhoneSCChangeCancelAlertNavigationVersion"), _configuration.GetValue<string>("AndroidSCChangeCancelAlertNavigationVersion"));
                    if (pnr.IsSCRefundEligible)
                        onetimecontent = (scheduleChangeConfigValue && isNewVersion) ? SplitConcatenatedConfigValue("ResDtlSCOneTimeChangeCancel_Content", "||") : SplitConcatenatedConfigValue("ResDtlSCOneTimeChangeCancel", "||");
                    else
                        onetimecontent = (scheduleChangeConfigValue && isNewVersion) ? SplitConcatenatedConfigValue("ResDtlSCOneTimeChange_Content", "||") : SplitConcatenatedConfigValue("ResDtlSCOneTimeChange", "||");
                    if (onetimecontent?.Length >= 2)
                    {
                        if (scheduleChangeConfigValue && isNewVersion)
                        {
                            buttonItems.AddRange(new List<MOBItem>
                            {
                                new MOBItem {
                                    Id = Convert.ToString(MOBDisplayType.MAPPCHANGE),
                                    CurrentValue = onetimecontent[2]
                            },
                                new MOBItem {
                                    Id = Convert.ToString(MOBDisplayType.MAPPCANCEL),
                                    CurrentValue = onetimecontent[3]
                            }
                        });
                        }

                        MOBPNRAdvisory sconetimechangecanceladvisory = new MOBPNRAdvisory
                        {
                            ContentType = ContentType.SCHEDULECHANGE,
                            AdvisoryType = AdvisoryType.INFORMATION,
                            Header = onetimecontent[0],
                            Body = onetimecontent[1],
                            IsBodyAsHtml = true,
                            IsDefaultOpen = true,
                            ButtonItems = (scheduleChangeConfigValue && isNewVersion) ? buttonItems : null,
                        };
                        pnr.AdvisoryInfo = (pnr.AdvisoryInfo == null) ? new List<MOBPNRAdvisory>() : pnr.AdvisoryInfo;
                        pnr.AdvisoryInfo.Add(sconetimechangecanceladvisory);
                    }
                }
            }
            catch { }
        }

        public bool GetHasScheduledChanged(List<MOBPNRSegment> segments)
        {
            if (segments != null && segments.Count > 0)
            {
                foreach (var segment in segments)
                {
                    if (_configuration.GetValue<string>("ScheduleChangeCodes") != null)
                    {
                        string[] schChangeCodes = _configuration.GetValue<string>("ScheduleChangeCodes").Split(',');
                        foreach (string schChangeCode in schChangeCodes)
                        {
                            if (segment.ActionCode.Contains(schChangeCode))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool GetHasScheduledChangedV2(List<MOBPNRSegment> segments, ref bool isScheduleChangeSegmentWithNoProtection)
        {
            bool isYesProtection = false;
            bool isScheduleChangeSegment = false;
            try
            {
                if (segments != null && segments.Count > 0)
                {
                    string scheduleChangeCodes = _configuration.GetValue<string>("ScheduleChangeCodes");

                    foreach (var segment in segments)
                    {
                        if (!string.IsNullOrEmpty(scheduleChangeCodes) && !string.IsNullOrEmpty(segment.ActionCode))
                        {
                            if (scheduleChangeCodes.IndexOf(segment.ActionCode.Substring(0, 2), StringComparison.Ordinal) > -1)
                            {
                                isScheduleChangeSegment = true;
                                if (!string.Equals(segment?.NoProtection, "true", StringComparison.OrdinalIgnoreCase)
                                    && segment.HasPreviousSegmentDetails)
                                {
                                    isYesProtection = true;
                                }
                            }
                        }
                    }
                    isScheduleChangeSegmentWithNoProtection = isScheduleChangeSegment && !isYesProtection;
                }
            }
            catch { return false; }
            return isYesProtection;
        }

        public Boolean CheckIfTicketedByUA(ReservationDetail response)
        {
            if (response?.Detail?.Characteristic == null) return false;
            string configbookingsource = _configuration.GetValue<string>("PNRUABookingSource");
            var charbookingsource = ShopStaticUtility.GetCharactersticDescription_New(response.Detail.Characteristic, "Booking Source");
            if (string.IsNullOrEmpty(configbookingsource) || string.IsNullOrEmpty(charbookingsource)) return false;
            return (configbookingsource.IndexOf(charbookingsource, StringComparison.OrdinalIgnoreCase) > -1);
        }
        public bool EnableConsolidatedAdvisoryMessage(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneConsolidatedAdvisoryMessageVersion", "AndroidConsolidatedAdvisoryMessageVersion", "", "", true, _configuration);
        }
        public bool CheckTravelWaiverAlertAvailable
         (PNRChangeEligibilityResponse changeEligibilityResponse)
        {
            if (changeEligibilityResponse?.Policies?.FirstOrDefault()?.PolicyRule == null) return false;
            bool.TryParse(changeEligibilityResponse.IsPolicyEligible, out bool isShowTravelWaiverAlert);
            return isShowTravelWaiverAlert;
        }
        public async Task<MOBShuttleOffer> GetEWRShuttleOfferInformation()
        {
            MOBShuttleOffer shuttleOffer = new MOBShuttleOffer();
            string offerCode = "offerCode";
            string offerText1 = "offerText1";
            string offerText2 = "offerText2";
            string currencyCode = "currencyCode";
            string offerPrice = "offerPrice";
            string offerText3 = "offerText3";
            string offerTileImageName = "offerTileImageName";
            string eligibleAirport = "eligibleAirport";
            string shuttleStation = "shuttleStation";
            string formattedPriceText = "Check price";
            string shuttleStationDescription = "shuttleStationDescription";
            string childWarningMessage = "childWarningMessage";
            string childCutoffAge = "childCutoffAge";
            try
            {
                //var offerItems = await _documentLibraryDynamoDB.GetNewLegalDocumentsForTitles(new List<string> { "EWRNYY_ShuttleOffer_Content" }, _headers.ContextValues.SessionId).ConfigureAwait(false);
                var offerItems = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles("EWRNYY_ShuttleOffer_Content", _headers.ContextValues.SessionId, true).ConfigureAwait(false);

                if (offerItems != null && offerItems.Any())
                {
                    shuttleOffer.OfferCode = GetFirstOrDefaultShuttleOfferValue(offerItems, offerCode);
                    shuttleOffer.Text1 = GetFirstOrDefaultShuttleOfferValue(offerItems, offerText1);
                    shuttleOffer.Title = GetFirstOrDefaultShuttleOfferValue(offerItems, offerText2);
                    shuttleOffer.Price = Convert.ToDecimal(GetFirstOrDefaultShuttleOfferValue(offerItems, offerPrice));
                    shuttleOffer.CurrencyCode = GetFirstOrDefaultShuttleOfferValue(offerItems, currencyCode);

                    if (shuttleOffer.Price > 0)
                    {
                        shuttleOffer.FormattedPrice = string.Format("{1}{0:0.00}", Convert.ToDecimal(shuttleOffer.Price), shuttleOffer.CurrencyCode);
                        shuttleOffer.Text2 = GetFirstOrDefaultShuttleOfferValue(offerItems, offerText3);
                    }
                    else
                    {
                        shuttleOffer.FormattedPrice = formattedPriceText;
                        shuttleOffer.Text2 = string.Empty;
                    }

                    shuttleOffer.OfferTileImageName = GetFirstOrDefaultShuttleOfferValue(offerItems, offerTileImageName);
                    shuttleOffer.EligibleAirport = GetFirstOrDefaultShuttleOfferValue(offerItems, eligibleAirport);
                    shuttleOffer.ShuttleStation = GetFirstOrDefaultShuttleOfferValue(offerItems, shuttleStation);
                    shuttleOffer.ShuttleStationDescription = GetFirstOrDefaultShuttleOfferValue(offerItems, shuttleStationDescription);
                    shuttleOffer.ChildCutoffAge = GetFirstOrDefaultShuttleOfferValue(offerItems, childCutoffAge);
                    shuttleOffer.ChildWarningMessage = GetFirstOrDefaultShuttleOfferValue(offerItems, childWarningMessage);
                }
            }
            catch (Exception ex) { return null; }
            return shuttleOffer;
        }
        private string GetFirstOrDefaultShuttleOfferValue(List<MOBLegalDocument> documents, string keyname)
        {
            try
            {
                return (documents.FirstOrDefault
                         (x => string.Equals(x.Title, keyname, StringComparison.OrdinalIgnoreCase)) != null)
                         ? documents.FirstOrDefault(x => string.Equals(x.Title, keyname, StringComparison.OrdinalIgnoreCase)).LegalDocument
                         : string.Empty;
            }
            catch (Exception ex) { return string.Empty; }
        }
        public bool EnableVBQEarnedMiles(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneVBQEarnedMilesVersion", "AndroidVBQEarnedMilesVersion", "", "", true, _configuration);
        }
        public bool EnableSSA(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSSA")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidSSAVersion", "iPhoneSSAVersion", "", "", true, _configuration);
        }
        public bool IsEnableCanadianTravelNumber(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableCanadianTravelNumber")
                    && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("IPhone_CanadianTravelNumber_AppVersion"), _configuration.GetValue<string>("Android_CanadianTravelNumber_AppVersion"));
        }
        public async Task<List<MOBItem>> GetCaptions(string key)
        {
            return !string.IsNullOrEmpty(key) ?await GetCaptions( key , true) : null;
        }
        /// <summary>
        /// KeyList requires mutiple doc IDs to be passed based upon docTitles
        /// Cannot use dynamodb since it's not fetching legaldocument based on doctitle rather docIDs--Dayakar working on it.
        /// </summary>
        /// <param name="keyList"></param>
        /// <param name="isTnC"></param>
        /// <returns></returns>
        private async Task<List<MOBItem>> GetCaptions(string keyList, bool isTnC)
        {
            //var docs = await _documentLibraryDynamoDB.GetNewLegalDocumentsForTitles(keyList, isTnC.ToString()).ConfigureAwait(false);

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
        public bool IsIBELiteFare(string productCode)
        {
            var iBELiteProductCodes = _configuration.GetValue<string>("IBELiteShoppingProductCodes");
            return !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBELiteProductCodes) &&
                   iBELiteProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }
        public bool IsELFFare(string productCode)
        {
            return _configuration.GetValue<bool>("EnableIBE") && !string.IsNullOrWhiteSpace(productCode) &&
                   "ELF" == productCode.Trim().ToUpper();
        }

        public async Task<string> GetGMTTime(string localTime, string airportCode, string sessionId)
        {

            string gmtTime = localTime;

            DateTime dateTime = new DateTime(0);
            if (DateTime.TryParse(localTime, out dateTime) && airportCode != null && airportCode.Trim().Length == 3)
            {

                long dateTime1 = 0L;
                long dateTime2 = 0L;
                long dateTime3 = 0L;
                try
                {
                    var fitBitDynanmoDB = new FitbitDynamoDB(_configuration, _dynamoDBService);
                    var gmtTimeValue = await fitBitDynanmoDB.GetGMTTime<GMTTime>(dateTime.Year, airportCode.Trim().ToUpper(), sessionId).ConfigureAwait(false);

                    //IDataReader dataReader = null;
                    //using (dataReader = database.ExecuteReader(dbCommand))
                    //{
                    //    while (dataReader.Read())
                    //    {
                    //        dateTime1 = Convert.ToInt64(dataReader["DateTime_1"]);
                    //        dateTime2 = Convert.ToInt64(dataReader["DateTime_2"]);
                    //        dateTime3 = Convert.ToInt64(dataReader["DateTime_3"]);
                    //    }
                    //}

                    long time = Convert.ToInt64(dateTime.Year.ToString() + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00"));
                    bool dayLightSavingTime = false;
                    if (time >= dateTime2 && time <= dateTime3)
                    {
                        dayLightSavingTime = true;
                    }

                    int offsetMunite = 0;
                    //database = DatabaseFactory.CreateDatabase("ConnectionString - GMTConversion");
                    //dbCommand = (DbCommand)database.GetStoredProcCommand("sp_GMT_City");

                    //database.AddInParameter(dbCommand, "@StationCode", DbType.String, airportCode.Trim().ToUpper());
                    //database.AddInParameter(dbCommand, "@Carrier", DbType.String, "CO");

                    offsetMunite = await fitBitDynanmoDB.GetGMTCity<int>(airportCode.Trim().ToUpper(), sessionId).ConfigureAwait(false);

                    //using (dataReader = database.ExecuteReader(dbCommand))
                    //{
                    //    while (dataReader.Read())
                    //    {
                    //        if (dayLightSavingTime)
                    //        {
                    //            offsetMunite = Convert.ToInt32(dataReader["DaySavTime"]);
                    //        }
                    //        else
                    //        {
                    //            offsetMunite = Convert.ToInt32(dataReader["StandardTime"]);
                    //        }
                    //    }
                    //}

                    dateTime = dateTime.AddMinutes(-offsetMunite);

                    gmtTime = dateTime.ToString("MM/dd/yyyy hh:mm tt");

                }
                catch (System.Exception) { }
            }
            return gmtTime;
        }
        public List<MOBItem> GetTripNamesFromSegment(List<MOBPNRSegment> mobpnrsegment)
        {
            List<MOBItem> tripNames = new List<MOBItem>();
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
                                tripNames.Add(new MOBItem
                                {
                                    Id = Convert.ToString(lastSegment.TripNumber),
                                    CurrentValue = string.Format("{0} - {1}", firstSegment.Departure.Code, lastSegment.Arrival.Code)
                                });
                            }
                        } //lofSegments
                    } //for loop
                } //totalLOF
            } //mobpnrsegment
            return tripNames;
        }
        public bool IsSeatMapSupportedOa(string operatingCarrier, string MarketingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier)) return false;
            var seatMapSupportedOa = _configuration.GetValue<string>("SeatMapSupportedOtherAirlines");
            if (string.IsNullOrEmpty(seatMapSupportedOa)) return false;

            var seatMapEnabledOa = seatMapSupportedOa.Split(',');
            if (seatMapEnabledOa.Any(s => s == operatingCarrier.ToUpper().Trim()))
                return true;
            else if (_configuration.GetValue<string>("SeatMapSupportedOtherAirlinesMarketedBy") != null)
            {
                return _configuration.GetValue<string>("SeatMapSupportedOtherAirlinesMarketedBy").Split(',').ToList().Any(m => m == MarketingCarrier + "-" + operatingCarrier);
            }
            return false;

        }

        public string GetEarnedMilesFromCharacteristics
         (Collection<Characteristic> characteristic, string code, string key)
        {
            try
            {
                if (characteristic == null || !characteristic.Any() || string.IsNullOrEmpty(code)) return string.Empty;
                var objData = (string.IsNullOrEmpty(key))
                   ? characteristic.FirstOrDefault
                   (x => (x.Status != null && string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase) && x.Status.Key == null))
                       : characteristic.FirstOrDefault
                       (x => (x.Status != null && string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase) && x.Status.Key == key));
                if (objData != null) return objData.Value;
            }
            catch { return string.Empty; }
            return string.Empty;
        }
        public string GetFormattedMiles(string miles)
        {
            decimal result;
            try
            {
                decimal.TryParse(miles, out result);
                return (result > 0) ? result.ToString("N", System.Globalization.CultureInfo.InvariantCulture).Replace(".00", "") : "--";
                //return (result > 0) ? result.ToString("N", System.Globalization.CultureInfo.CurrentCulture).Replace(".00", "") : "--";

            }
            catch { return "--"; }
        }
        public string GetFormattedCurrency(string amount)
        {
            decimal result;
            try
            {
                decimal.TryParse(amount, out result);
                return (result > 0) ? result.ToString("C", System.Globalization.CultureInfo.CurrentCulture).Replace(".00", "") : "--";
            }
            catch { return "--"; }
        }
        public string GetFormattedSegment(string segment)
        {
            decimal result;
            try
            {
                decimal.TryParse(segment, out result);
                return (result > 0) ? result.ToString("0.####") : "--";
            }
            catch { return "--"; }
        }

        public MOBPageContent PopulateCtnInfo(MOBPNR pnr)
        {
            try
            {
                MOBPageContent ctnInfo = new MOBPageContent();
                ctnInfo.id = MOBPageId.CANADIANTRAVELNUMBER;
                string[] stringarray = SplitConcatenatedConfigValue("CTNTooltipContent", "||");
                stringarray.ForEach(i =>
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        string[] lineitem = ShopStaticUtility.SplitConcatenatedString(i, "|");

                        if (lineitem?.Length > 1)
                        {
                            switch (lineitem[0])
                            {
                                case "Header":
                                    ctnInfo.displayHeader = lineitem[1];
                                    break;
                                case "Body":
                                    ctnInfo.displayBody = lineitem[1];
                                    break;
                            }
                        }
                    }
                });

                string[] ctnErrorMsg = SplitConcatenatedConfigValue("ValidationMsgCanadianTravelNumber", "||");
                if (ctnErrorMsg != null && ctnErrorMsg.Length > 1)
                {

                    pnr.TravelerInfo.messages = (pnr.TravelerInfo.messages == null) ? new List<MOBItem>() : pnr.TravelerInfo.messages;
                    pnr.TravelerInfo.messages.Add(new MOBItem { Id = ctnErrorMsg[0], CurrentValue = ctnErrorMsg[1] });
                }
                return ctnInfo;
            }
            catch { return null; }

        }

        public MOBPNRByRecordLocatorResponse GetShareReservationInfo(MOBPNRByRecordLocatorResponse response, ReservationDetail cslReservationDetail, string url)
        {
            MOBShareReservationInfo shareReservationInfo = new MOBShareReservationInfo();
            var baseUrl = _configuration.GetValue<string>("TripDetailRedirect3dot0BaseUrl");
            var urlPattern = _configuration.GetValue<string>("TripDetailRedirect3dot0UrlPattern");
            string languagecode = "en/US";

            try
            {
                string[] stringarray = SplitConcatenatedConfigValue("ShareReservationDisplayCaption", "||");
                stringarray.ForEach(i =>
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        string[] lineitem = ShopStaticUtility.SplitConcatenatedString(i, "|");

                        if (lineitem?.Length > 1)
                        {
                            switch (lineitem[0])
                            {
                                case "Header":
                                    shareReservationInfo.DisplayHeader = lineitem[1];
                                    break;
                                case "Body":
                                    shareReservationInfo.DisplayBody = lineitem[1].Replace("@", Environment.NewLine);
                                    break;
                            }
                        }
                    }
                });

                shareReservationInfo.DisplayLink = string.Format
                    (urlPattern, baseUrl, languagecode, response.RecordLocator, response.LastName, "").TrimEnd('?');

                shareReservationInfo.DisplayOption = GetShareHtmlItems(response, cslReservationDetail, url);
                response.ShareReservationInfo = shareReservationInfo;
                return response;
            }
            catch
            {
                return null;
            }

        }

        public List<MOBHtmlItem> GetShareHtmlItems(MOBPNRByRecordLocatorResponse response, ReservationDetail cslReservationDetail, string url)
        {
            List<MOBHtmlItem> items = new List<MOBHtmlItem>();
            MOBHtmlItem item;

            string[] stringarray = SplitConcatenatedConfigValue("ShareReservationDisplayOption", "||");

            stringarray.ForEach(i =>
            {
                if (!string.IsNullOrEmpty(i))
                {
                    string[] lineitem = ShopStaticUtility.SplitConcatenatedString(i, "|");

                    if (lineitem?.Length > 1)
                    {
                        switch (lineitem[0])
                        {
                            case "Option1":
                                item = new MOBHtmlItem
                                {
                                    DisplayKey = DisplayType.LINK,
                                    DisplayText = lineitem[1],
                                    DisplayLink = url,
                                };
                                items.Add(item);
                                break;
                            case "Option2":
                                item = new MOBHtmlItem
                                {
                                    DisplayKey = DisplayType.DATA,
                                    DisplayText = lineitem[1],
                                    DisplayData = BuildShareReservationTripSegmentData
                                    (cslReservationDetail.Detail.FlightSegments, cslReservationDetail)
                                };
                                items.Add(item);
                                break;
                            case "Option3":
                                item = new MOBHtmlItem
                                {
                                    DisplayKey = DisplayType.NONE,
                                    DisplayText = lineitem[1],
                                };
                                items.Add(item);
                                break;
                        }
                    }
                }
            });
            return items;
        }

        public string BuildShareReservationTripSegmentData(Collection<ReservationFlightSegment> flightsegments, ReservationDetail cslReservationDetail = null, string cslJourneyType = "")
        {
            StringBuilder strBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(cslJourneyType))
            {
                var type = cslReservationDetail.Detail.Type;
                if (type != null && type.Any())
                {
                    var journeyTypeObj = type.FirstOrDefault(t => t.Description.Equals("JOURNEY_TYPE", StringComparison.InvariantCultureIgnoreCase));
                    cslJourneyType = (journeyTypeObj != null) ? journeyTypeObj.Key : string.Empty;
                }
            }

            string journeyType = (string.Equals("MULTI_CITY", cslJourneyType, StringComparison.OrdinalIgnoreCase)) ? "Multi City"
                   : (string.Equals("ROUND_TRIP", cslJourneyType, StringComparison.OrdinalIgnoreCase)) ? "Round Trip"
                   : "One Way";

            if (flightsegments != null && flightsegments.Any())
            {
                flightsegments = flightsegments.OrderBy(x => x.TripNumber).ToCollection();

                int mintripnumber = Convert.ToInt32(flightsegments.Select(o => o.TripNumber).First());
                int maxtripnumber = Convert.ToInt32(flightsegments.Select(o => o.TripNumber).Last());

                for (int i = mintripnumber; i <= maxtripnumber; i++)
                {
                    MOBTrip pnrTrip = new MOBTrip();

                    var totalTripSegments = flightsegments.Where(o => o.TripNumber == i.ToString());

                    pnrTrip.Index = i;

                    if (totalTripSegments != null && totalTripSegments.Any())
                    {
                        int minsegmantnumber = Convert.ToInt32(totalTripSegments.Select(o => o.SegmentNumber).First());
                        int maxsegmentnumber = Convert.ToInt32(totalTripSegments.Select(o => o.SegmentNumber).Last());
                        string tripDeparture = string.Empty;
                        string tripArrival = string.Empty;
                        bool isTripAdded = false;

                        foreach (United.Service.Presentation.SegmentModel.ReservationFlightSegment segment in totalTripSegments)
                        {

                            if (segment != null)
                            {
                                if (segment.SegmentNumber == minsegmantnumber)
                                {
                                    var address1 = segment.FlightSegment?.DepartureAirport?.Address;
                                    if (address1 != null) tripDeparture = address1.Name;
                                }

                                if (segment.SegmentNumber == maxsegmentnumber)
                                {
                                    var address2 = segment.FlightSegment?.ArrivalAirport?.Address;
                                    if (address2 != null) tripArrival = address2.Name;
                                }

                                if (!isTripAdded)
                                {
                                    //string sss = ;                        
                                    strBuilder.Append(string.Format($"{journeyType} from {tripDeparture} to {tripArrival}" + Environment.NewLine + Environment.NewLine));
                                    isTripAdded = true;
                                }

                                strBuilder.Append("Duration : " + segment.FlightSegment.ScheduledFlightDuration + Environment.NewLine);
                                strBuilder.Append("Depart — " + segment.FlightSegment.DepartureAirport.Name + Environment.NewLine);
                                strBuilder.Append(segment.EstimatedDepartureTime + Environment.NewLine);
                                strBuilder.Append("Arrive — " + segment.FlightSegment.ArrivalAirport.Name + Environment.NewLine);
                                strBuilder.Append(segment.EstimatedArrivalTime + Environment.NewLine);
                                strBuilder.Append("Operated by : " + segment.FlightSegment.OperatingAirlineName + Environment.NewLine + Environment.NewLine);
                            }
                        }//Seg
                    }
                }//trip
            }
            return strBuilder.ToString();
        }
        public string GetCharacteristicValue(List<Characteristic> characteristics, string code)
        {
            string keyValue = string.Empty;
            if (characteristics.Exists(p => p.Code?.Trim() == code))
            {
                keyValue = characteristics.First(p => p.Code.Trim() == code).Value;
            }
            return keyValue;
        }
        public bool IsEnableJSXManageRes(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("enableMANAGERESJSXChanges") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("AndroidAppLatestAppVersionSupportHTMLContentMANAGERESAlertMessage"), _configuration.GetValue<string>( "iPhoneAppLatestAppVersionSupportHTMLContentMANAGERESAlertMessage"));
        }
        public bool IsEnableTravelOptionsInViewRes(int applicationId, string appVersion, List<MOBItem> catalogItems)
        {
            return _configuration.GetValue<bool>("EnableTravelOptionsInViewRes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Android"), _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Iphone")) && catalogItems !=null &&
                              catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableBundlesInManageRes).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableBundlesInManageRes).ToString())?.CurrentValue == "1";
        }
        public bool IsEnableSAFInViewRes(int applicationId, string appVersion, List<MOBItem> catalogItems)
        {
            return _configuration.GetValue<bool>("EnableSAFInViewRes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("EnableSAFInViewRes_AppVersion_Android"), _configuration.GetValue<string>("EnableSAFInViewRes_AppVersion_Iphone")) && catalogItems != null &&
                              catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableSAFInViewres).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableSAFInViewres).ToString())?.CurrentValue == "1";
        }
    }
}

