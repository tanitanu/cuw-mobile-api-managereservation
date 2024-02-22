using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Service.Presentation.SecurityRequestModel;
using United.Service.Presentation.SecurityResponseModel;
using United.Services.ProfileValidation.Common;
using United.Utility.Helper;

namespace United.Common.Helper.Profile
{
    public class SecurityQuestion : ISecurityQuestion
    {
        private readonly ICacheLog<SecurityQuestion> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICachingService _cachingService;
        private readonly IDPService _dPService;
        private readonly IMPSecurityQuestionsService _mPSecurityQuestionsService;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IHeaders _headers;

        public SecurityQuestion(ICacheLog<SecurityQuestion> logger
            , IConfiguration configuration
            , ICachingService cachingService
            , IDPService dPService
            , IDynamoDBService dynamoDBService
            , IMPSecurityQuestionsService mPSecurityQuestionsService
            , IUtilitiesService utilitiesService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IHeaders headers

        )
        {
            _logger = logger;
            _configuration = configuration;
            _cachingService = cachingService;
            _dPService = dPService;
            _mPSecurityQuestionsService = mPSecurityQuestionsService;
            _utilitiesService = utilitiesService;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
        }

        public async Task<List<Securityquestion>> GetMPPINPWDSecurityQuestions(string token, string sessionId, int appId, string appVersion, string deviceId)
        {
            List<Securityquestion> securityQuestions = new List<Securityquestion>();
            #region
            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices") }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            Service.Presentation.SecurityRequestModel.SecurityQuestionsRequest _SecurityQuestionsRequest = new Service.Presentation.SecurityRequestModel.SecurityQuestionsRequest
            {
                Tokens = dbTokens,
            };
            string jsonRequest = JsonConvert.SerializeObject(_SecurityQuestionsRequest);

            string jsonResponse = await _mPSecurityQuestionsService.GetMPPINPWDSecurityQuestions(token, jsonRequest, sessionId);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Service.Presentation.SecurityResponseModel.SecurityQuestionsResponse>(jsonResponse);

                if (response != null && string.IsNullOrEmpty(response.ExceptionCode))
                {
                    _logger.LogInformation("GetMPPINPWDSecurityQuestions - Response for GetAllSecurityQuestions {Response} {TransactionId}", jsonResponse, _headers.ContextValues.TransactionId);
                }
                else
                {
                    string exceptionmessage = string.IsNullOrEmpty(response.ExceptionCode) ? response.ExceptionCode : "Unable to Validate Device.";
                    _logger.LogError("GetMPPINPWDSecurityQuestions - Response for GetAllSecurityQuestions {exception} {TransactionId}", JsonConvert.SerializeObject(exceptionmessage), _headers.ContextValues.TransactionId);
                    throw new MOBUnitedException(exceptionmessage);
                }
                securityQuestions = ConvertToCSSSecurityList(response.SecurityQuestions);
            }
            #endregion

            return securityQuestions;
        }

        private List<Securityquestion> ConvertToCSSSecurityList(Collection<Service.Presentation.SecurityModel.Question> _CSLQuestionsCollection)
        {
            List<Securityquestion> _CSSQuestionsList = new List<Securityquestion>();
            List<Mobile.Model.Common.Answer> _CSSAnsList = new List<Mobile.Model.Common.Answer>();
            //int QuestionID = 1;
            //int AnsID = 1;
            foreach (var _CSLQst in _CSLQuestionsCollection)
            {
                if (_CSLQst != null && _CSLQst.Answers != null && _CSLQst.Answers.Count > 0)
                {
                    _CSSAnsList = new List<Mobile.Model.Common.Answer>();
                    //AnsID = 1;
                    foreach (var _CSLAns in _CSLQst.Answers)
                    {
                        Mobile.Model.Common.Answer _CSSAns = new Mobile.Model.Common.Answer
                        {
                            //AnswerId = AnsID,
                            AnswerKey = _CSLAns.AnswerKey,
                            AnswerText = _CSLAns.AnswerText,
                            QuestionKey = _CSLAns.QuestionKey,
                            //QuestionId = QuestionID
                        };
                        //++AnsID;
                        _CSSAnsList.Add(_CSSAns);
                    }
                    Securityquestion _CSSQtn = new Securityquestion
                    {
                        //QuestionId = QuestionID,
                        QuestionKey = _CSLQst.QuestionKey,
                        QuestionText = _CSLQst.QuestionText,
                        Used = _CSLQst.IsUsed,
                        Answers = _CSSAnsList
                    };
                    _CSSQuestionsList.Add(_CSSQtn);
                    //++QuestionID;
                }
            }

            return _CSSQuestionsList;
        }

        public async Task<List<Securityquestion>> GetMPPinPwdSavedSecurityQuestions(string token, int customerId, string mileagePlusNumber, string sessionId, int appId, string appVersion, string deviceId)
        {
            List<Securityquestion> securityQuestions = null;

            //string url = string.Format("{0}/GetSavedSecurityQuestions", _configuration.GetValue<string>("CslSecureProfileURL"));
            string path = string.Format("/GetSavedSecurityQuestions");

            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString() }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            Service.Presentation.SecurityRequestModel.SavedSecurityQuestionsRequest _SavedSecurityQuestionsRequest = new Service.Presentation.SecurityRequestModel.SavedSecurityQuestionsRequest
            {
                CustomerId = customerId,
                MileagePlusId = mileagePlusNumber,
                Tokens = dbTokens
            };

            string jsonRequest = JsonConvert.SerializeObject(_SavedSecurityQuestionsRequest);

            var response = await _mPSecurityQuestionsService.ValidateSecurityAnswer<Service.Presentation.SecurityResponseModel.SavedSecurityQuestionsResponse>(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (response != null)
            {
                securityQuestions = ConvertToCSSSecurityList(response.Questions);
            }

            return securityQuestions;
        }

        public async Task<bool> ValidateSecurityAnswer(string questionKey, string answerKey, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId)
        {
            bool isAnswerValidated = false;

            //string url = string.Format("{0}/ValidateSecurityAnswer", _configuration.GetValue<string>("CslSecureProfileURL"));
            string path = string.Format("/ValidateSecurityAnswer");

            //< add key = "CslSecureProfileURL" value = "https://csmc.stage.api.united.com/8.0/security/SecureProfile/api" />

            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString() }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            Service.Presentation.SecurityRequestModel.ValidateSecurityAnswerRequest _ValidateSecurityAnswerRequest = new Service.Presentation.SecurityRequestModel.ValidateSecurityAnswerRequest
            {
                AnswerKey = answerKey,
                MileagePlusId = mileagePlusNumber,
                QuestionKey = questionKey,
                Tokens = dbTokens,
                //tobechanged csstodp
                IsFirstAnswer = false
            };

            string jsonRequest = DataContextJsonSerializer.Serialize<ValidateSecurityAnswerRequest>(_ValidateSecurityAnswerRequest);

            var response =await _mPSecurityQuestionsService.ValidateSecurityAnswer<ValidateSecurityAnswerResponse>(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (response != null)
            {
                isAnswerValidated = response.IsCorrectAnswer;
            }

            return isAnswerValidated;
        }

        public async Task<bool> MPPinPwdValidatePassowrd(string token, string langCode, string password, string mpdId, string username, string email, string sessionId, int appId, string appVersion, string deviceId)
        {
            bool isPasswordValidated = false;

            United.Services.ProfileValidation.Common.ValidatePasswordRequest ProfileValidationReq = new ValidatePasswordRequest
            {
                DataSetting = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString(),
                Email = email,
                LangCode = langCode,
                MileagePlusId = mpdId,
                Password = password,
                UserName = username
            };


            //string url = string.Format("{0}/profilevalidation/api/ValidatePassword", _configuration.GetValue<string>("PINPWD_ServiceEndPointBaseUrl - CSLUtilities"));
            string path = string.Format("/profilevalidation/api/ValidatePassword");

            string jsonRequest = DataContextJsonSerializer.Serialize<United.Services.ProfileValidation.Common.ValidatePasswordRequest>(ProfileValidationReq);

            string jsonResponse = await _utilitiesService.ValidateMPNames(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jsonResponse))
            {

                United.Services.ProfileValidation.Common.ValidatePasswordResponse response = DataContextJsonSerializer.Deserialize<United.Services.ProfileValidation.Common.ValidatePasswordResponse>(jsonResponse);

                if (response != null && response.Status.Equals(United.Services.ProfileValidation.Common.Constants.StatusType.Success) && response.ValidationResult == true)
                {
                    isPasswordValidated = true;
                }
                else
                {
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in response.Errors)
                        {
                            if (!string.IsNullOrEmpty(error.UserFriendlyMessage))
                            {
                                errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                            }
                            else
                            {
                                errorMessage = errorMessage + " " + error.Message;
                            }
                        }
                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        string exceptionMessage = _configuration.GetValue<string>("UnableToValidatePasswordErrorMessage");
                        //if (ConfigurationManager.AppSettings["ReturnActualExceptionMessageBackForTesting"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["ReturnActualExceptionMessageBackForTesting"].ToString()))
                        //{
                        //    exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL MPPinPwdValidatePassowrd";
                        //}
                        throw new MOBUnitedException(exceptionMessage);
                    }
                }
            }

            return isPasswordValidated;
        }

        public async Task<bool> MPPinPwdUpdateCustomerPassword(string token, string oldPassword, string newPassword, int customerId, string mileagePlusNumber, string langCode, string sessionId, int appId, string appVersion, string deviceId)
        {
            bool isPasswordUpdated = false;

            // string url = string.Format("{0}/UpdateCustomerPassword", _configuration.GetValue<string>("CslSecureProfileURL"));
            string path = "/UpdateCustomerPassword";

            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString() }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            Service.Presentation.SecurityRequestModel.UpdateCustomerPasswordRequest _UpdateCustomerPasswordRequest = new Service.Presentation.SecurityRequestModel.UpdateCustomerPasswordRequest
            {

                CustomerId = customerId,
                MileagePlusId = mileagePlusNumber,
                Tokens = dbTokens,
                NewPassword = newPassword,
                OldPassword = oldPassword,
                WhoModifiedId = mileagePlusNumber
            };

            string jsonRequest = DataContextJsonSerializer.Serialize<Service.Presentation.SecurityRequestModel.UpdateCustomerPasswordRequest>(_UpdateCustomerPasswordRequest);

            var response = await _mPSecurityQuestionsService.ValidateSecurityAnswer<UpdateCustomerPasswordResponse>(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (response != null)
            {
                isPasswordUpdated = Convert.ToBoolean(response.Result);
            }

            return isPasswordUpdated;
        }


        public async Task<bool> UnLockCustomerAccount(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId)
        {
            bool isAccountUnLocked = false;

            //string url = string.Format("{0}/UnLockCustomerAccount", _configuration.GetValue<string>("CslSecureProfileURL"));
            string path = string.Format("/UnLockCustomerAccount");

            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString() }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            UnLockCustomerAccountRequest _UnLockCustomerAccountRequest = new UnLockCustomerAccountRequest
            {
                CustomerId = customerId,
                MileagePlusId = mileagePlusNumber,
                Tokens = dbTokens,
                UpdateId = customerId.ToString(),
            };

            string jsonRequest = DataContextJsonSerializer.Serialize<UnLockCustomerAccountRequest>(_UnLockCustomerAccountRequest);

            var response =await _mPSecurityQuestionsService.ValidateSecurityAnswer<UnLockCustomerAccountResponse>(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (response != null)
            {
                isAccountUnLocked = response.Success;
            }

            return isAccountUnLocked;
        }

        public async Task<bool> LockCustomerAccount(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId)
        {
            return await LockCustomerAccountWithSendEmailFlag(customerId, mileagePlusNumber, token, langCode, sessionId, appId, appVersion, deviceId, true);
        }

        public async Task<bool> LockCustomerAccountWithSendEmailFlag(int customerId, string mileagePlusNumber, string token, string langCode, string sessionId, int appId, string appVersion, string deviceId, bool sendEmail)
        {
            bool isAccountLocked = false;
            string path = "/LockCustomerAccount";
            Collection<Service.Presentation.CommonModel.Genre> dbTokens = new Collection<Service.Presentation.CommonModel.Genre> { new Service.Presentation.CommonModel.Genre { Key = "DBEnvironment", Value = _configuration.GetValue<string>("CustomerDBDataSettingForCSLServices").ToString() }, new Service.Presentation.CommonModel.Genre { Key = "LangCode", Value = "en-US" } };

            Service.Presentation.SecurityRequestModel.LockCustomerAccountRequest _LockCustomerAccountRequest = new Service.Presentation.SecurityRequestModel.LockCustomerAccountRequest
            {
                CustomerId = customerId,
                MileagePlusId = mileagePlusNumber,
                Tokens = dbTokens,
                IsSendEmail = sendEmail,
                UpdateId = customerId.ToString(),
            };

            string jsonRequest = DataContextJsonSerializer.Serialize<Service.Presentation.SecurityRequestModel.LockCustomerAccountRequest>(_LockCustomerAccountRequest);

            _logger.LogInformation("LockCustomerAccountWithSendEmailFlag {@clientRequest} {SessionId}", JsonConvert.SerializeObject(jsonRequest), sessionId);

            var jsonResponse =await _mPSecurityQuestionsService.ValidateSecurityAnswer<LockCustomerAccountResponse>(token, jsonRequest, sessionId, path).ConfigureAwait(false);

            if (jsonResponse != null)
            {
                isAccountLocked = jsonResponse.Success;
            }
            _logger.LogInformation("LockCustomerAccountWithSendEmailFlag {response} {SessionId}", JsonConvert.SerializeObject(jsonResponse), sessionId);
            return isAccountLocked;
        }
    }
}
