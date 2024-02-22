using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.AccountManagement;
using MOBPNRByRecordLocatorResponse = United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse;
using United.Service.Presentation.ReservationResponseModel;
using United.Mobile.Model.ReShop;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.ManageRes;

namespace United.Mobile.Test.ManageReservation.Tests.TestData
{
   public class TestDataSet
    {
        public Object[] set1()
        {
            var mOBConfirmScheduleChangeRequestjson = TestDataGenerator.GetFileContent("MOBConfirmScheduleChangeRequest.json");
            var mOBConfirmScheduleChangeRequest = JsonConvert.DeserializeObject<MOBConfirmScheduleChangeRequest>(mOBConfirmScheduleChangeRequestjson);


            var sessionjson = TestDataGenerator.GetFileContent("SessionData.json");
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            var hashPinValidatejson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidatejson);


            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailjson);

            var pnrByRecordLocatorResponsejson = TestDataGenerator.GetFileContent("pnrByRecordLocatorResponse.json");
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(pnrByRecordLocatorResponsejson);

            return new object[] { mOBConfirmScheduleChangeRequest, session, hashPinValidate, reservationDetail[0],pnrByRecordLocatorResponse[0] };
        }

        public Object[] set2()
        {

            var mOBReceiptByEmailRequestjson = TestDataGenerator.GetFileContent("MOBReceiptByEmailRequest.json");
            var mOBReceiptByEmailRequest = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(mOBReceiptByEmailRequestjson);

            var commonDefjson = TestDataGenerator.GetFileContent("CommonDef.json");
            var commonDef = JsonConvert.DeserializeObject<CommonDef>(commonDefjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            //var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            return new object[] { mOBReceiptByEmailRequest[0], commonDef, session };
        }
    }
}
