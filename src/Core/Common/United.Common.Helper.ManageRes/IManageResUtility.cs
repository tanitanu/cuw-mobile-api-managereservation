using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Service.Presentation.ReservationResponseModel;

namespace United.Common.Helper.ManageRes
{
    public interface IManageResUtility
    {
        MOBPNRByRecordLocatorResponse GetShareReservationInfo(MOBPNRByRecordLocatorResponse response, ReservationDetail cslReservationDetail, string url);
        void OneTimeSCChangeCancelAlert(MOBPNR pnr, int appId, string appVersion);
        bool GetHasScheduledChanged(List<MOBPNRSegment> segments);
        bool IsEnableJSXManageRes(int applicationId, string appVersion);
        MOBPageContent PopulateCtnInfo(MOBPNR pnr);
        bool IsEnableCanadianTravelNumber(int applicationId, string appVersion);      
    }
}
