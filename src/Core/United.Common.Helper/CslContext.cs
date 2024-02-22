using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Services.FlightShopping.Common.Extensions;

namespace United.Common.Helper
{
    public class CslContext : ICslContext
    {
        private readonly string _transactionId;
        private readonly string _token;
        private readonly Session _session;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IDPService _dpTokenService;
        private readonly IConfiguration _configuration;

        public Session Session
        {
            get { return _session; }
        }
        public string TransactionId
        {
            get { return _transactionId; }
        }

        public string Token
        {
            get { return _token; }
        }       

        public CslContext(MOBModifyReservationRequest request
            , ISessionHelperService sessionHelperService
            , IDPService dpTokenService
            , IConfiguration configuration)          
        {
            _sessionHelperService = sessionHelperService;
            _dpTokenService = dpTokenService;
            _configuration = configuration;
            _transactionId = request.TransactionId.Replace("|", "").Replace("-", "");           

            _session = GetSessionWithValidToken(request).Result;

            _token = _dpTokenService.GetAnonymousToken(request.Application.Id,request.DeviceId,_configuration).Result;

        }


        public void Dispose()
        {

        }

        private async Task<Session> GetSessionWithValidToken(MOBModifyReservationRequest request)
        {
            Session mySession = null;

            if (request.SessionId.IsNullOrEmpty())
            {
                mySession = GetNewSessionWithoutToken(request); //we weren't given an existing session, make a new one
            }
            else
            {
                mySession = await _sessionHelperService.GetSession<Session>(request.SessionId, new Session().ObjectName, new List<string> { request.SessionId, new Session().ObjectName }).ConfigureAwait(false); //load session from persist

                if (mySession.IsNullOrEmpty()) //if session doesn't exist then make a new one
                {
                    mySession = GetNewSessionWithoutToken(request);
                }
            }

            //get a fresh token
            mySession.Token = await _dpTokenService.GetAnonymousToken(request.Application.Id, request.DeviceId,_configuration);

            return mySession;
        }

        
        private Session GetNewSessionWithoutToken(MOBRequest request)
        {
            var session = new Session
            {
                DeviceID = request.DeviceId,
                AppID = request.Application.Id,
                SessionId = NewSessionId(),
                CreationTime = DateTime.Now,
                LastSavedTime = DateTime.Now,
                SupressLMXForAppID = false,
                Token = string.Empty
            };

            return session;
        }

        public static string NewSessionId()
        {
            return System.Guid.NewGuid().ToString().Replace("{", "").Replace("-", "").Replace("}", "").ToUpper();
        }
    }
}
