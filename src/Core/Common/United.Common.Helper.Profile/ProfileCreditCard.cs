using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Service.Presentation.CommonModel;
using United.Utility.Helper;
using CslDataVaultRequest = United.Service.Presentation.PaymentRequestModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using CslDataVaultResponse = United.Service.Presentation.PaymentResponseModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using United.Mobile.DataAccess.Customer;
using United.CorporateDirect.Models.CustomerProfile;
using System.Threading.Tasks;
using United.Mobile.Model.CSLModels;

namespace United.Common.Helper.Profile
{
    public class ProfileCreditCard : IProfileCreditCard
    {
        private readonly ICacheLog<ProfileCreditCard> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ICachingService _cachingService;
        private readonly IDataVaultService _dataVaultService;
        private readonly ICustomerProfileCreditCardsService _customerProfileCreditCardsService;
        private readonly ICustomerCorporateProfileService _customerCorporateProfileService;
        private string _deviceId = string.Empty;

        private MOBApplication _application = new MOBApplication() { Version = new MOBVersion() };

        public ProfileCreditCard(ICacheLog<ProfileCreditCard> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , ICachingService cachingService
            , IDataVaultService dataVaultService
            , ICustomerProfileCreditCardsService customerProfileCreditCardsService
            , ICustomerCorporateProfileService customerCorporateProfileService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _cachingService = cachingService;
            _dataVaultService = dataVaultService;
            _customerProfileCreditCardsService = customerProfileCreditCardsService;
            _customerCorporateProfileService= customerCorporateProfileService;
        }

        #region Methods

        public async Task<List<MOBCreditCard>> PopulateCorporateCreditCards(List<Services.Customer.Common.CreditCard> creditCards, bool isGetCreditCardDetailsCall, List<MOBAddress> addresses, Reservation persistedReservation, string sessionId)
        {
            #region

            List<MOBCreditCard> mobCreditCards = new List<MOBCreditCard>();
            var mobGhostCardFirstInList = new List<MOBCreditCard>();
            MOBCreditCard ghostCreditCard = null;
            bool isGhostCard = false;
            bool isValidForTPI = false;
            if (creditCards != null && creditCards.Count > 0)
            {
                #region
                foreach (Services.Customer.Common.CreditCard creditCard in creditCards)
                {
                    MOBCreditCard cc = new MOBCreditCard
                    {
                        Message = IsValidCreditCardMessage(creditCard),
                        AddressKey = creditCard.AddressKey,
                        Key = creditCard.Key
                    };
                    if (_configuration.GetValue<bool>("WorkAround4DataVaultUntilClientChange"))
                    {
                        cc.Key = creditCard.Key + "~" + creditCard.AccountNumberToken;
                    }
                    cc.CardType = creditCard.Code;
                    //switch (creditCard.CCTypeDescription.ToLower())
                    //{
                    //    case "diners club":
                    //        cc.CardTypeDescription = "Diners Club Card";
                    //        break;
                    //    case "uatp (formerly Air Travel Card)":
                    //        cc.CardTypeDescription = "UATP";
                    //        break;
                    //    default:
                    //        cc.CardTypeDescription = creditCard.CCTypeDescription;
                    //        break;
                    //}
                    cc.CardTypeDescription = creditCard.CCTypeDescription;
                    cc.Description = creditCard.CustomDescription;
                    cc.ExpireMonth = creditCard.ExpMonth.ToString();
                    cc.ExpireYear = creditCard.ExpYear.ToString();
                    cc.IsPrimary = creditCard.IsPrimary;

                    //Wade 11/03/2014
                    cc.UnencryptedCardNumber = "XXXXXXXXXXXX" + creditCard.AccountNumberLastFourDigits;
                    //**NOTE**: IF "XXXXXXXXXXXX" is updated to any other format need to fix this same at GetUpdateCreditCardRequest() as we trying to check if CC updated by checking "XXXXXXXXXXXX" exists in the UnencryptedCardNumber if exists CC Number not updated.

                    cc.DisplayCardNumber = cc.UnencryptedCardNumber;

                    cc.cIDCVV2 = creditCard.SecurityCode;

                    if (creditCard.Payor != null)
                    {
                        cc.cCName = creditCard.Payor.GivenName;
                    }
                    if (isGetCreditCardDetailsCall)
                    {
                        cc.UnencryptedCardNumber = creditCard.AccountNumber;
                    }
                    cc.AccountNumberToken = creditCard.AccountNumberToken;
                    MOBVormetricKeys vormetricKeys = await AssignPersistentTokenToCC(creditCard.AccountNumberToken, creditCard.PersistentToken, creditCard.SecurityCodeToken, creditCard.Code, sessionId, "PopulateCorporateCreditCards", 0, "");
                    if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                    {
                        cc.PersistentToken = vormetricKeys.PersistentToken;
                        cc.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                    }
                    if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cc.CardType))
                    {
                        cc.CardType = vormetricKeys.CardType;
                    }
                    cc.IsCorporate = creditCard.IsCorporate;
                    cc.IsMandatory = creditCard.IsMandatory;
                    if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                    {
                        cc.IsValidForTPIPurchase = IsValidFOPForTPIpayment(creditCard.Code);
                    }
                    //Not assigning the cc.EncryptedCardNumber = creditCard.AccountNumberEncrypted; because client will send back to us and while updating we will call DataVault and it fails with AppId
                    if (addresses != null)
                    {
                        foreach (var address in addresses)
                        {
                            if (address.Key.ToUpper().Trim() == cc.AddressKey.ToUpper().Trim() && !cc.IsCorporate && !cc.IsMandatory)
                            {
                                mobCreditCards.Add(cc);
                            }
                        }
                    }
                    //Mandatory Ghost Cards - If Present then only one card should be displayed to the client and no option to add CC / select other FOPs
                    if (creditCard.IsCorporate && creditCard.IsMandatory)
                    {
                        ghostCreditCard = cc;
                        isGhostCard = true;
                        if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                        {
                            isValidForTPI = cc.IsValidForTPIPurchase;
                        }
                        break;
                    }
                    //Non Mandatory Ghost cards - If Present client can select/Add/Edit other cards and will be first in the list
                    if (cc.IsCorporate && !cc.IsMandatory)
                    {
                        mobGhostCardFirstInList.Add(cc);
                        isGhostCard = true;
                        if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                        {
                            isValidForTPI = cc.IsValidForTPIPurchase;
                        }
                    }
                }
                #endregion
            }
            if (ghostCreditCard != null)
            {
                //In this case only Ghost card will be in the list
                mobGhostCardFirstInList.Add(ghostCreditCard);
            }
            else
            {
                mobGhostCardFirstInList.AddRange(mobCreditCards);
            }
            await GhostCardValidationForTPI(persistedReservation, ghostCreditCard, isGhostCard, isValidForTPI);
            #endregion

            return mobGhostCardFirstInList;
        }

        public bool IsValidFOPForTPIpayment(string cardType)
        {
            return !string.IsNullOrEmpty(cardType) &&
                (cardType.ToUpper().Trim() == "VI" || cardType.ToUpper().Trim() == "MC" || cardType.ToUpper().Trim() == "AX" || cardType.ToUpper().Trim() == "DS");
        }

        private async System.Threading.Tasks.Task GhostCardValidationForTPI(Reservation persistedReservation, MOBCreditCard ghostCreditCard, bool isGhostCard, bool isValidForTPI)
        {
            if (isGhostCard)
            {
                if (persistedReservation != null && persistedReservation.ShopReservationInfo != null)
                {
                    //If ghost card has invalid FOP for TPI purchase, we should not show TPI 
                    if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch") && !isValidForTPI)
                    {
                        persistedReservation.ShopReservationInfo.IsGhostCardValidForTPIPurchase = false;
                    }

                    if (ghostCreditCard != null)
                    {
                        persistedReservation.ShopReservationInfo.CanHideSelectFOPOptionsAndAddCreditCard = true;
                    }
                    await SavePersistedReservation(persistedReservation);
                }
            }
        }

        public async Task<List<MOBCreditCard>> PopulateCreditCards(List<Services.Customer.Common.CreditCard> creditCards, bool isGetCreditCardDetailsCall, List<Mobile.Model.Common.MOBAddress> addresses,string sessionId)
        {          
            #region

            List<MOBCreditCard> mobCreditCards = new List<MOBCreditCard>();
            if (creditCards != null && creditCards.Count > 0)
            {
                #region
                foreach (Services.Customer.Common.CreditCard creditCard in creditCards)
                {
                    //if(!IsValidCreditCard(creditCard))
                    //{
                    //    continue;
                    //}
                    if (creditCard.IsCorporate)
                        continue;

                    MOBCreditCard cc = new MOBCreditCard();
                    cc.Message = IsValidCreditCardMessage(creditCard);
                    cc.AddressKey = creditCard.AddressKey;
                    cc.Key = creditCard.Key;
                    if (_configuration.GetValue<bool>("WorkAround4DataVaultUntilClientChange"))
                    {
                        cc.Key = creditCard.Key + "~" + creditCard.AccountNumberToken;
                    }
                    cc.CardType = creditCard.Code;
                    //switch (creditCard.CCTypeDescription.ToLower())
                    //{
                    //    case "diners club":
                    //        cc.CardTypeDescription = "Diners Club Card";
                    //        break;
                    //    case "uatp (formerly air travel card)":
                    //        cc.CardTypeDescription = "UATP";
                    //        break;
                    //    default:
                    //        cc.CardTypeDescription = creditCard.CCTypeDescription;
                    //        break;
                    //}
                    cc.CardTypeDescription = creditCard.CCTypeDescription;
                    cc.Description = creditCard.CustomDescription;
                    cc.ExpireMonth = creditCard.ExpMonth.ToString();
                    cc.ExpireYear = creditCard.ExpYear.ToString();
                    cc.IsPrimary = creditCard.IsPrimary;
                    //if (creditCard.AccountNumber.Length == 15)
                    //    cc.UnencryptedCardNumber = "XXXXXXXXXXX" + creditCard.AccountNumber.Substring(creditCard.AccountNumber.Length - 4, 4);
                    //else
                    //cc.UnencryptedCardNumber = "XXXXXXXXXXXX" + creditCard.AccountNumber.Substring(creditCard.AccountNumber.Length - 4, 4);
                    //updated due to CSL no longer providing the account number.
                    //Wade 11/03/2014
                    cc.UnencryptedCardNumber = "XXXXXXXXXXXX" + creditCard.AccountNumberLastFourDigits;
                    //**NOTE**: IF "XXXXXXXXXXXX" is updated to any other format need to fix this same at GetUpdateCreditCardRequest() as we trying to check if CC updated by checking "XXXXXXXXXXXX" exists in the UnencryptedCardNumber if exists CC Number not updated.

                    cc.DisplayCardNumber = cc.UnencryptedCardNumber;
                    cc.EncryptedCardNumber = creditCard.AccountNumberEncrypted;
                    cc.cIDCVV2 = creditCard.SecurityCode;
                    //cc.CCName = creditCard.Name;
                    if (creditCard.Payor != null)
                    {
                        cc.cCName = creditCard.Payor.GivenName;
                    }
                    if (isGetCreditCardDetailsCall)
                    {
                        cc.UnencryptedCardNumber = creditCard.AccountNumber;
                    }
                    cc.AccountNumberToken = creditCard.AccountNumberToken;
                    MOBVormetricKeys vormetricKeys = await AssignPersistentTokenToCC(creditCard.AccountNumberToken, creditCard.PersistentToken, creditCard.SecurityCodeToken, creditCard.Code, sessionId, "PopulateCreditCards", 0, "");
                    if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                    {
                        cc.PersistentToken = vormetricKeys.PersistentToken;
                        //cc.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                    }

                    if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cc.CardType))
                    {
                        cc.CardType = vormetricKeys.CardType;
                    }

                    if (_configuration.GetValue<bool>("CFOPViewRes_ExcludeCorporateCard"))
                        cc.IsCorporate = creditCard.IsCorporate;

                    if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                    {
                        cc.IsValidForTPIPurchase = IsValidFOPForTPIpayment(creditCard.Code);
                    }
                    if (addresses != null)
                    {
                        foreach (var address in addresses)
                        {
                            if (address.Key.ToUpper().Trim() == cc.AddressKey.ToUpper().Trim())
                            {
                                mobCreditCards.Add(cc);
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            return mobCreditCards;
        }

        private string IsValidCreditCardMessage(Services.Customer.Common.CreditCard creditCard)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(creditCard.AddressKey))
            {
                message = _configuration.GetValue<string>("NoAddressAssociatedWithTheSavedCreditCardMessage");
            }
            if (creditCard.ExpYear < DateTime.Today.Year)
            {
                message = message + _configuration.GetValue<string>("CreditCardDateExpiredMessage");
            }
            else if (creditCard.ExpYear == DateTime.Today.Year)
            {
                if (creditCard.ExpMonth < DateTime.Today.Month)
                {
                    message = message + _configuration.GetValue<string>("CreditCardDateExpiredMessage");
                }
            }
            return message;
        }

        private async System.Threading.Tasks.Task SavePersistedReservation(Reservation persistedReservation)
        {
            await _sessionHelperService.SaveSession<Reservation>(persistedReservation, persistedReservation.SessionId, new List<string> { persistedReservation.SessionId, persistedReservation.ObjectName }, persistedReservation.ObjectName);
        }

        private async Task<MOBVormetricKeys> AssignPersistentTokenToCC(string accountNumberToken, string persistentToken, string securityCodeToken, string cardType, string sessionId, string action, int appId, string deviceID)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (_configuration.GetValue<bool>("VormetricTokenMigration"))
            {
                Session session = new Session();
                session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string>() { sessionId, session.ObjectName }).ConfigureAwait(false);
                if ((string.IsNullOrEmpty(persistentToken) || string.IsNullOrEmpty(cardType)) && !string.IsNullOrEmpty(accountNumberToken) && !string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(session?.Token))
                {
                    vormetricKeys = await GetPersistentTokenUsingAccountNumberToken(accountNumberToken, sessionId, session.Token);
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

        private void LogNoPersistentTokenInCSLResponseForVormetricPayment(string sessionId, string Message = "Unable to retieve PersistentToken")
        {
            _logger.LogWarning("{PERSISTENTTOKENNOTFOUND}",Message);
        }

        private async Task<MOBVormetricKeys> GetPersistentTokenUsingAccountNumberToken(string accountNumberToke, string sessionId, string token)
        {
            string url = string.Format("/{0}/RSA", accountNumberToke);

            var cslResponse = await MakeHTTPCallAndLogIt(sessionId, _deviceId, "CSL-ChangeEligibleCheck", _application, token, url, string.Empty, true, false);

            return GetPersistentTokenFromCSLDatavaultResponse(cslResponse, sessionId);
        }

        private MOBVormetricKeys GetPersistentTokenFromCSLDatavaultResponse(string jsonResponseFromCSL, string sessionID)
        {
            MOBVormetricKeys vormetricKeys = new MOBVormetricKeys();
            if (!string.IsNullOrEmpty(jsonResponseFromCSL))
            {
                CslDataVaultResponse response = DataContextJsonSerializer.DeserializeJsonDataContract<CslDataVaultResponse>(jsonResponseFromCSL);
                if (response != null && response.Responses != null && response.Responses[0].Error == null && response.Responses[0].Message != null && response.Responses[0].Message.Count > 0 && response.Responses[0].Message[0].Code.Trim() == "0")
                {
                    var creditCard = response.Items[0] as Service.Presentation.PaymentModel.CreditCard;
                    vormetricKeys.PersistentToken = creditCard.PersistentToken;
                    vormetricKeys.SecurityCodeToken = creditCard.SecurityCodeToken;
                    vormetricKeys.CardType = creditCard.Code;
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
                           _logger.LogWarning("GetPersistentTokenUsingAccountNumberToken response {sessionId}",response,sessionID);
                        else
                            throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage").ToString();
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

        private async Task<string> MakeHTTPCallAndLogIt(string sessionId, string deviceId, string action, MOBApplication application, string token, string url, string jsonRequest, bool isGetCall, bool isXMLRequest = false)
        {
            string jsonResponse = string.Empty;

            string paypalCSLCallDurations = string.Empty;
            string callTime4Tuning = string.Empty;

            #region//****Get Call Duration Code*******
            Stopwatch cslCallDurationstopwatch1;
            cslCallDurationstopwatch1 = new Stopwatch();
            cslCallDurationstopwatch1.Reset();
            cslCallDurationstopwatch1.Start();
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
            if (cslCallDurationstopwatch1.IsRunning)
            {
                cslCallDurationstopwatch1.Stop();
            }
            paypalCSLCallDurations = paypalCSLCallDurations + "|2=" + cslCallDurationstopwatch1.ElapsedMilliseconds.ToString() + "|"; // 2 = shopCSLCallDurationstopwatch1
            callTime4Tuning = "|CSL =" + (cslCallDurationstopwatch1.ElapsedMilliseconds / (double)1000).ToString();
            #endregion

            //logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "Response", application.Id, application.Version.Major, deviceId, jsonResponse, false, false));
            //logEntries.Add(LogEntry.GetLogEntry<string>(sessionId, action, "CSS/CSL-CallDuration", application.Id, application.Version.Major, deviceId, "CSLResponse=" + (cslCallDurationstopwatch1.ElapsedMilliseconds / (double)1000).ToString(), false, false));
            return jsonResponse;
        }


        public async Task<(bool returnVaue, string ccDataVaultToken)> GenerateCCTokenWithDataVault(MOBCreditCard creditCardDetails, string sessionID, string token, MOBApplication applicationDetails, string deviceID, string ccDataVaultToken)
        {
            bool generatedCCTokenWithDataVault = false;
            //if (creditCardDetails.UnencryptedCardNumber == null || (creditCardDetails.UnencryptedCardNumber != null && !creditCardDetails.UnencryptedCardNumber.Contains("XXXXXXXXXXXX")))
            if (!string.IsNullOrEmpty(creditCardDetails.EncryptedCardNumber)) // expecting Client will send only Encrypted Card Number only if the user input is a clear text CC number either for insert CC or update CC not the CC details like CVV number update or expiration date upate no need to call data vault for this type of updates only data vault will be called for CC number update to get the CC token back
            {
                #region
                CslDataVaultRequest dataVaultRequest = await GetDataValutRequest(creditCardDetails, sessionID, applicationDetails.Id);
                string jsonRequest = DataContextJsonSerializer.Serialize<CslDataVaultRequest>(dataVaultRequest);
                string jsonResponse = await _dataVaultService.GetCCTokenWithDataVault(token, jsonRequest, sessionID).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    CslDataVaultResponse response = DataContextJsonSerializer.NewtonSoftDeserialize<CslDataVaultResponse>(jsonResponse);

                    if (response != null && response.Responses != null && response.Responses[0].Error == null && response.Responses[0].Message != null && response.Responses[0].Message.Count > 0 && response.Responses[0].Message[0].Code.Trim() == "0")
                    {
                        generatedCCTokenWithDataVault = true;                   
                        if (_configuration.GetValue<bool>("VormetricTokenMigration") && String.IsNullOrEmpty(creditCardDetails.CardType))
                        {
                            CslDataVaultResponse response1 = DataContextJsonSerializer.DeserializeJsonDataContract<CslDataVaultResponse>(jsonResponse);
                            var creditCard = response1.Items[0] as Service.Presentation.PaymentModel.CreditCard;
                            creditCardDetails.CardType = creditCard.Code;                          
                        }
                        else if (!_configuration.GetValue<bool>("DisableCheckForUnionPayFOP_MOBILE13762") && !string.IsNullOrEmpty(creditCardDetails?.CardType))
                        {
                            CslDataVaultResponse response1 = DataContextJsonSerializer.DeserializeJsonDataContract<CslDataVaultResponse>(jsonResponse);
                            var creditCard = response1.Items[0] as Service.Presentation.PaymentModel.CreditCard;
                            string[] checkForUnionPayFOP = _configuration.GetValue<string>("CheckForUnionPayFOP")?.Split('|');
                            if (creditCard?.Code == checkForUnionPayFOP?[0])
                            {
                                creditCardDetails.CardType = creditCard.Code;
                                creditCardDetails.CardTypeDescription = checkForUnionPayFOP?[1];
                            }
                        }
                        foreach (var item in response.Items)
                        {
                            ccDataVaultToken = item.AccountNumberToken;
                            break;
                        }

                    }
                    else
                    {
                        if (response.Responses[0].Error != null && response.Responses[0].Error.Count > 0)
                        {
                            string errorMessage = string.Empty;
                            string errorCode = string.Empty;

                            if (_configuration.GetValue<bool>("EnableBacklogIssueFixes"))
                            {
                                //Datavault returns REST (Dollar Ding) Error - Show appropriate error message //Richa
                                string[] dvErrorCodeAndMessages = _configuration.GetValue<string>("HandleDataVaultErrorCodeAndMessages").Split('|');
                                foreach (var error in response.Responses[0].Error)
                                {
                                    if (dvErrorCodeAndMessages != null)
                                    {
                                        string errorTextFromConfig = dvErrorCodeAndMessages.Any(row => row.Contains(error.Code)) ?
                                                dvErrorCodeAndMessages[Array.FindIndex(dvErrorCodeAndMessages, row => row.Contains(error.Code))].Split('=')[1] :
                                                error.Text;
                                        errorMessage = errorMessage + " " + errorTextFromConfig;
                                        errorCode = error.Code;
                                    }
                                    else
                                    {
                                        errorMessage = errorMessage + " " + error.Text;
                                    }
                                }

                            }
                            else
                            {
                                foreach (var error in response.Responses[0].Error)
                                {
                                    errorMessage = errorMessage + " " + error.Text;
                                }
                            }
                            throw new MOBUnitedException(errorCode, errorMessage);
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
                else
                {
                    string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                    if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                    {
                        exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  GenerateCCTokenWithDataVault(MOBUpdateTravelerRequest request, ref string ccDataVaultToken)";
                    }
                    throw new MOBUnitedException(exceptionMessage);
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(creditCardDetails.AccountNumberToken.Trim()))
            {
                ccDataVaultToken = creditCardDetails.AccountNumberToken;
                generatedCCTokenWithDataVault = true;
            }
            return (generatedCCTokenWithDataVault, ccDataVaultToken);
        }

        public async Task<bool> GenerateCCTokenWithDataVault(MOBCreditCard creditCardDetails, string sessionID, string token, MOBApplication applicationDetails, string deviceID)
        {
            bool generatedCCTokenWithDataVault = false;
            if (!string.IsNullOrEmpty(creditCardDetails.EncryptedCardNumber)) // expecting Client will send only Encrypted Card Number only if the user input is a clear text CC number either for insert CC or update CC not the CC details like CVV number update or expiration date upate no need to call data vault for this type of updates only data vault will be called for CC number update to get the CC token back
            {
                #region
                CslDataVaultRequest dataVaultRequest = await GetDataValutRequest(creditCardDetails, sessionID, applicationDetails.Id);
                string jsonRequest = DataContextJsonSerializer.Serialize<CslDataVaultRequest>(dataVaultRequest);
                var jsonResponse = await _dataVaultService.GetCCTokenWithDataVault(token, jsonRequest, sessionID).ConfigureAwait(false);
                if (jsonResponse != null)
                {
                    CslDataVaultResponse response = DataContextJsonSerializer.DeserializeJsonDataContract<CslDataVaultResponse>(jsonResponse);
                    
                    if (response != null && response.Responses != null && response.Responses[0].Error == null && response.Responses[0].Message != null && response.Responses[0].Message.Count > 0 && response.Responses[0].Message[0].Code.Trim() == "0")
                    {
                        generatedCCTokenWithDataVault = true;
                        var creditCard = response.Items[0] as Service.Presentation.PaymentModel.CreditCard;
                        creditCardDetails.AccountNumberToken = creditCard.AccountNumberToken;
                        creditCardDetails.PersistentToken = creditCard.PersistentToken;
                        creditCardDetails.SecurityCodeToken = creditCard.SecurityCodeToken;
                        if (String.IsNullOrEmpty(creditCardDetails.CardType))
                        {
                            creditCardDetails.CardType = creditCard.Code;
                        }
                        else if (!_configuration.GetValue<bool>("DisableCheckForUnionPayFOP_MOBILE13762"))
                        {
                            string[] checkForUnionPayFOP = _configuration.GetValue<string>("CheckForUnionPayFOP")?.Split('|');
                            if (creditCard?.Code == checkForUnionPayFOP?[0])
                            {
                                creditCardDetails.CardType = creditCard.Code;
                                creditCardDetails.CardTypeDescription = checkForUnionPayFOP?[1];
                            }
                        }
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
                            throw new MOBUnitedException(errorMessage);
                        }
                        else
                        {
                            string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                            if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                            {
                                exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL InsertTravelerCreditCard(MOBUpdateTravelerRequest request)";
                            }
                            throw new MOBUnitedException(exceptionMessage);
                        }
                    }
                }
                else
                {
                    string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                    if (_configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                    {
                        exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  GenerateCCTokenWithDataVault(MOBUpdateTravelerRequest request, ref string ccDataVaultToken)";
                    }
                    throw new MOBUnitedException(exceptionMessage);
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(creditCardDetails.AccountNumberToken.Trim()))
            {
                generatedCCTokenWithDataVault = true;
            }
            return generatedCCTokenWithDataVault;
        }
        private async Task<CslDataVaultRequest> GetDataValutRequest(MOBCreditCard creditCardDetails, string sessionID, int appId)
        {
            #region
            var dataVaultRequest = new CslDataVaultRequest
            {
                Items = new System.Collections.ObjectModel.Collection<United.Service.Presentation.PaymentModel.Payment>(),
                Types = new Collection<United.Service.Presentation.CommonModel.Characteristic>(),
                CallingService = new United.Service.Presentation.CommonModel.ServiceClient { Requestor = new Requestor { AgentAAA = "WEB", ApplicationSource = "mobile services" } }
            };
            United.Services.Customer.Common.InsertCreditCardRequest creditCardInsertRequest = new Services.Customer.Common.InsertCreditCardRequest();
            if (creditCardDetails != null)
            {
                var cc = new United.Service.Presentation.PaymentModel.CreditCard();
                cc.ExpirationDate = creditCardDetails.ExpireMonth + "/" + (creditCardDetails.ExpireYear.Trim().Length == 2 ? creditCardDetails.ExpireYear.Trim() : creditCardDetails.ExpireYear.Trim().Substring(2, 2).ToString()); //"05/17";
                if (!_configuration.GetValue<bool>("DisableUATBCvvCodeNullCheckAndAssignEmptyString") &&
                   string.IsNullOrEmpty(creditCardDetails.cIDCVV2))
                {
                    cc.SecurityCode = "";
                }
                else
                {
                    cc.SecurityCode = creditCardDetails.cIDCVV2.Trim(); //"1234";
                }
                cc.Code = creditCardDetails.CardType;  //"VI";
                cc.Name = creditCardDetails.cCName; //"Test Testing";
                if (!string.IsNullOrEmpty(creditCardDetails.EncryptedCardNumber))
                {
                    dataVaultRequest.Types = new System.Collections.ObjectModel.Collection<Characteristic>();
                    dataVaultRequest.Types.Add(new Characteristic { Code = "ENCRYPTION", Value = "PKI" });
                    cc.AccountNumberEncrypted = creditCardDetails.EncryptedCardNumber;

                    if (_configuration.GetValue<bool>("EnablePKDispenserKeyRotationAndOAEPPadding") && creditCardDetails.IsOAEPPaddingCatalogEnabled)
                    {
                        // var obj = Persist.FilePersist.Load<United.Service.Presentation.SecurityResponseModel.PKDispenserKey>(United.Mobile.DAL.Utility.GetNewPublicKeyPersistSessionStaticGUID(appId), "pkDispenserPublicKey");                       
                        string key = string.Format(_configuration.GetValue<string>("PKDispenserKeyTokenKeyFormat"), appId);
                        var pKDispenserKey = await _cachingService.GetCache<string>(key, "TID1").ConfigureAwait(false);//same name as couchbase
                        United.Service.Presentation.SecurityResponseModel.PKDispenserKey obj = JsonConvert.DeserializeObject<United.Service.Presentation.SecurityResponseModel.PKDispenserKey>(pKDispenserKey);
                        dataVaultRequest.Types.Add(new Characteristic { Code = "KID", Value = obj.Kid });
                        dataVaultRequest.Types.Add(new Characteristic { Code = "OAEP", Value = "TRUE" });
                    }
                }
                else
                {
                    cc.AccountNumber = creditCardDetails.UnencryptedCardNumber; //"4000000000000002";
                }
                if (_configuration.GetValue<bool>("PassMobileSessionIDInsteadOfDifferntGuidEveryTime"))
                {
                    cc.OperationID = sessionID; // This one we can pass the session id which we using in bookign path.
                }
                else if (_configuration.GetValue<bool>("EDDtoEMDToggle"))
                {
                    cc.OperationID = Guid.NewGuid().ToString(); // This one we can pass the session id which we using in bookign path.
                }
                else
                {
                    cc.OperationID = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                }
                ///93875 - iOS Android UnionPay bookings are not throwing error for bin ranges that are not 16 digit credit cards
                ///Srini - 02/13/2018
                if (_configuration.GetValue<bool>("DataVaultRequestAddDollarDingToggle"))
                {
                    cc.PinEntryCapability = Service.Presentation.CommonEnumModel.PosTerminalPinEntryCapability.PinNotSupported;
                    cc.TerminalAttended = Service.Presentation.CommonEnumModel.PosTerminalAttended.Unattended;
                    dataVaultRequest.PointOfSaleCountryCode = "US";
                    dataVaultRequest.Types.Add(new Characteristic { Code = "DOLLAR_DING", Value = "TRUE" });
                }
                dataVaultRequest.Items.Add(cc);
            }
            return dataVaultRequest;
            #endregion
        }

        #endregion

        #region UCB Migration mobile phase3
        public async Task<List<MOBCreditCard>> PopulateCreditCards(bool isGetCreditCardDetailsCall, List<Mobile.Model.Common.MOBAddress> addresses, MOBCPProfileRequest request)
        {

            var response = await _sessionHelperService.GetSession<CreditCardDataReponseModel>(request.SessionId, ObjectNames.CSLProfileCreditCardsResponse, new List<string> { request.SessionId, ObjectNames.CSLProfileCreditCardsResponse }).ConfigureAwait(false); 

            if (response != null)
            {
                var creditCards = response.CreditCards;
                List<MOBCreditCard> mobCreditCards = new List<MOBCreditCard>();
                if (creditCards != null && creditCards.Count > 0)
                {
                    #region
                    foreach (United.Mobile.Model.CSLModels.ProfileCreditCardItem creditCard in creditCards)
                    {                     
                        MOBCreditCard cc = new MOBCreditCard();
                        cc.Message = IsValidCreditCardMessage(creditCard.AddressKey,creditCard.ExpYear,creditCard.ExpMonth);
                        cc.AddressKey = creditCard.AddressKey;
                        cc.Key = creditCard.Key;
                        if (_configuration.GetValue<bool>("WorkAround4DataVaultUntilClientChange"))
                        {
                            cc.Key = creditCard.Key + "~" + creditCard.AccountNumberToken;
                        }
                        cc.CardType = creditCard.Code;

                        cc.CardTypeDescription = creditCard.CCTypeDescription;
                        cc.Description = creditCard.CustomDescription;
                        cc.ExpireMonth = creditCard.ExpMonth.ToString();
                        cc.ExpireYear = creditCard.ExpYear.ToString();
                        cc.IsPrimary = creditCard.IsPrimary;

                        //updated due to CSL no longer providing the account number.
                        //Wade 11/03/2014
                        cc.UnencryptedCardNumber = "XXXXXXXXXXXX" + creditCard.AccountNumberLastFourDigits;
                        //**NOTE**: IF "XXXXXXXXXXXX" is updated to any other format need to fix this same at GetUpdateCreditCardRequest() as we trying to check if CC updated by checking "XXXXXXXXXXXX" exists in the UnencryptedCardNumber if exists CC Number not updated.

                        cc.DisplayCardNumber = cc.UnencryptedCardNumber;
                     
                        if (creditCard.Payor != null)
                        {
                            cc.cCName = creditCard.Payor.GivenName;
                        }                  
                        cc.AccountNumberToken = creditCard.AccountNumberToken;
                        MOBVormetricKeys vormetricKeys = await AssignPersistentTokenToCC(creditCard.AccountNumberToken, creditCard.PersistentToken, /*creditCard.SecurityCodeToken*/ "", creditCard.Code, request.SessionId, "PopulateCreditCards", 0, "");//SecurityCodeToken will never be sent by service confirmed with service team
                        if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                        {
                            cc.PersistentToken = vormetricKeys.PersistentToken;
                         
                        }
                        if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cc.CardType))
                        {
                            cc.CardType = vormetricKeys.CardType;
                        }                 
                        if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                        {
                            cc.IsValidForTPIPurchase = IsValidFOPForTPIpayment(creditCard.Code);
                        }
                        if (addresses != null)
                        {
                            foreach (var address in addresses)
                            {
                                if (address.Key.ToUpper().Trim() == cc.AddressKey.ToUpper().Trim())
                                {
                                    mobCreditCards.Add(cc);
                                }
                            }
                        }
                    }
                    #endregion
                }
                return mobCreditCards;
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }               
        }
        private string IsValidCreditCardMessage(string addressKey,int expYear,int expMonth)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(addressKey))
            {
                message = _configuration.GetValue<string>("NoAddressAssociatedWithTheSavedCreditCardMessage");
            }
            if (expYear < DateTime.Today.Year)
            {
                message = message + _configuration.GetValue<string>("CreditCardDateExpiredMessage");
            }
            else if (expYear == DateTime.Today.Year)
            {
                if (expMonth < DateTime.Today.Month)
                {
                    message = message + _configuration.GetValue<string>("CreditCardDateExpiredMessage");
                }
            }
            return message;
        }
        public async Task<List<MOBCreditCard>> PopulateCorporateCreditCards(bool isGetCreditCardDetailsCall, List<MOBAddress> addresses, Reservation persistedReservation, MOBCPProfileRequest request)
        {
            var response = await _sessionHelperService.GetSession<United.CorporateDirect.Models.CustomerProfile.CorpFopResponse>(request.SessionId, ObjectNames.CSLCorFopResponse, new List<string> { request.SessionId, ObjectNames.CSLCorFopResponse }).ConfigureAwait(false);

            #region

            List<MOBCreditCard> mobCreditCards = new List<MOBCreditCard>();
            var mobGhostCardFirstInList = new List<MOBCreditCard>();
            MOBCreditCard ghostCreditCard = null;
            bool isGhostCard = false;
            bool isValidForTPI = false;
            if (response != null && (response.Errors == null|| response.Errors.Count==0))
            {
                var creditCards = response.CreditCards;
                if (creditCards != null && creditCards.Count > 0)
                {
                    #region
                    foreach (CorporateDirect.Models.CustomerProfile.CorporateCreditCard creditCard in creditCards)
                    {
                        MOBCreditCard cc = new MOBCreditCard
                        {
                            Message = IsValidCreditCardMessage("", creditCard.ExpYear, creditCard.ExpMonth),
                             AddressKey = addresses != null ? addresses[0].Key : string.Empty,//Getprofile service was assigning the first address(Confirmed with service team) ..Corporate direct service wont have addresskey..So,implemented how getprofile is doing
                            Key = creditCard.Key
                        };
                        if (_configuration.GetValue<bool>("WorkAround4DataVaultUntilClientChange"))
                        {
                            cc.Key = creditCard.Key + "~" + creditCard.AccountNumberToken;
                        }
                        cc.CardType = creditCard.Code;
                        cc.CardTypeDescription = creditCard.CCTypeDescription;
                        cc.Description = creditCard.CustomDescription;
                        cc.ExpireMonth = creditCard.ExpMonth.ToString();
                        cc.ExpireYear = creditCard.ExpYear.ToString();
                        cc.IsPrimary = creditCard.IsPrimary;

                        //Wade 11/03/2014
                        cc.UnencryptedCardNumber = "XXXXXXXXXXXX" + creditCard.AccountNumberLastFourDigits;
                        //**NOTE**: IF "XXXXXXXXXXXX" is updated to any other format need to fix this same at GetUpdateCreditCardRequest() as we trying to check if CC updated by checking "XXXXXXXXXXXX" exists in the UnencryptedCardNumber if exists CC Number not updated.

                        cc.DisplayCardNumber = cc.UnencryptedCardNumber;

           

                        if (creditCard.Payor != null)
                        {
                            cc.cCName = creditCard.Payor.GivenName;
                        }
                        if (isGetCreditCardDetailsCall)
                        {
                            cc.UnencryptedCardNumber = creditCard.AccountNumber;
                        }
                        cc.AccountNumberToken = creditCard.AccountNumberToken;
                        MOBVormetricKeys vormetricKeys = await AssignPersistentTokenToCC(creditCard.AccountNumberToken, creditCard.PersistentToken, /*creditCard.SecurityCodeToken*/"", creditCard.Code, request.SessionId, "PopulateCorporateCreditCards", 0, "");//SecurityCodeToken will never be sent by service confirmed with them
                        if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                        {
                            cc.PersistentToken = vormetricKeys.PersistentToken;
                            cc.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                        }
                        if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cc.CardType))
                        {
                            cc.CardType = vormetricKeys.CardType;
                        }
                        cc.IsCorporate = creditCard.IsCorporate;
                        cc.IsMandatory = creditCard.IsMandatory;
                        if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                        {
                            cc.IsValidForTPIPurchase = IsValidFOPForTPIpayment(creditCard.Code);
                        }
                        //Not assigning the cc.EncryptedCardNumber = creditCard.AccountNumberEncrypted; because client will send back to us and while updating we will call DataVault and it fails with AppId
                        if (addresses != null)
                        {
                            foreach (var address in addresses)
                            {
                                if (address.Key.ToUpper().Trim() == cc.AddressKey.ToUpper().Trim() && !cc.IsCorporate && !cc.IsMandatory)
                                {
                                    mobCreditCards.Add(cc);
                                }
                            }
                        }
                        //Mandatory Ghost Cards - If Present then only one card should be displayed to the client and no option to add CC / select other FOPs
                        if (creditCard.IsCorporate && creditCard.IsMandatory)
                        {
                            ghostCreditCard = cc;
                            isGhostCard = true;
                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                isValidForTPI = cc.IsValidForTPIPurchase;
                            }
                            break;
                        }
                        //Non Mandatory Ghost cards - If Present client can select/Add/Edit other cards and will be first in the list
                        if (cc.IsCorporate && !cc.IsMandatory)
                        {
                            mobGhostCardFirstInList.Add(cc);
                            isGhostCard = true;
                            if (_configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch"))
                            {
                                isValidForTPI = cc.IsValidForTPIPurchase;
                            }
                        }
                    }
                    #endregion
                }
                if (ghostCreditCard != null)
                {
                    //In this case only Ghost card will be in the list
                    mobGhostCardFirstInList.Add(ghostCreditCard);
                }
                else
                {
                    mobGhostCardFirstInList.AddRange(mobCreditCards);
                }
                await GhostCardValidationForTPI(persistedReservation, ghostCreditCard, isGhostCard, isValidForTPI);
            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            #endregion

            return mobGhostCardFirstInList;
        }
        public async System.Threading.Tasks.Task MakeProfileCreditCardsServiecall(MOBCPProfileRequest request)
        {
            var response = await _customerProfileCreditCardsService.GetProfileCreditCards<United.Mobile.Model.CSLModels.CslResponse<United.Mobile.Model.CSLModels.CreditCardDataReponseModel>>(request.Token, request.SessionId, request.MileagePlusNumber).ConfigureAwait(false);
            if (response != null && response.Data != null)
            {
                await _sessionHelperService.SaveSession<CreditCardDataReponseModel>(response.Data, request.SessionId, new List<string> { request.SessionId, ObjectNames.CSLProfileCreditCardsResponse }, ObjectNames.CSLProfileCreditCardsResponse);

            }
            else
            {
                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }
        #endregion
    }
}
