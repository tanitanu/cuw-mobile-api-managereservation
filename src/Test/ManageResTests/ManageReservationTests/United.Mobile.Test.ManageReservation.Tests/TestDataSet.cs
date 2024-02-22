using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.ReShop;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.ReservationResponseModel;
using MOBPNRByRecordLocatorResponse = United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse;

namespace United.Mobile.Test.ManageReservation.Tests
{
   public class TestDataSet
    {
        public Object[] set1()
        {
            var mOBConfirmScheduleChangeRequestjson = TestDataGenerator.GetFileContent("MOBConfirmScheduleChangeRequest.json");
            var mOBConfirmScheduleChangeRequest = JsonConvert.DeserializeObject<List<MOBConfirmScheduleChangeRequest>>(mOBConfirmScheduleChangeRequestjson);


            var sessionjson = TestDataGenerator.GetFileContent("SessionData.json");
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            var hashPinValidatejson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidatejson);


            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailjson);

            var pnrByRecordLocatorResponsejson = TestDataGenerator.GetFileContent("pnrByRecordLocatorResponse.json");
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(pnrByRecordLocatorResponsejson);

            return new object[] { mOBConfirmScheduleChangeRequest[0], session, hashPinValidate, reservationDetail[0], pnrByRecordLocatorResponse[0] };
        }

        public Object[] set1_1()
        {
            var mOBConfirmScheduleChangeRequestjson = TestDataGenerator.GetFileContent("MOBConfirmScheduleChangeRequest.json");
            var mOBConfirmScheduleChangeRequest = JsonConvert.DeserializeObject<List<MOBConfirmScheduleChangeRequest>>(mOBConfirmScheduleChangeRequestjson);


            var sessionjson = TestDataGenerator.GetFileContent("SessionData.json");
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            var hashPinValidatejson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidatejson);


            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailjson);

            var pnrByRecordLocatorResponsejson = TestDataGenerator.GetFileContent("pnrByRecordLocatorResponse.json");
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(pnrByRecordLocatorResponsejson);

            return new object[] { mOBConfirmScheduleChangeRequest[1], session, hashPinValidate, reservationDetail[0], pnrByRecordLocatorResponse[0] };
        }

        public Object[] set2()
        {

            var mOBReceiptByEmailRequestjson = TestDataGenerator.GetFileContent(@"NegativeTestCases\MOBReceiptByEmailRequest.json");
            var mOBReceiptByEmailRequest = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(mOBReceiptByEmailRequestjson);

            var commonDefjson = TestDataGenerator.GetFileContent("CommonDef.json");
            var commonDef = JsonConvert.DeserializeObject<CommonDef>(commonDefjson);



            return new object[] { mOBReceiptByEmailRequest[0], commonDef };
        }

        public Object[] set2_1()
        {

            var mOBReceiptByEmailRequestjson = TestDataGenerator.GetFileContent("MOBReceiptByEmailRequest.json");
            var mOBReceiptByEmailRequest = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(mOBReceiptByEmailRequestjson);

            var commonDefjson = TestDataGenerator.GetFileContent("CommonDef.json");
            var commonDef = JsonConvert.DeserializeObject<CommonDef>(commonDefjson);

            //var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            ////var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);
            //var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            return new object[] { mOBReceiptByEmailRequest[0], commonDef };
        }

        public Object[] set3()
        {

            var mOBMileageAndStatusOptionsRequestJson = TestDataGenerator.GetFileContent("MOBMileageAndStatusOptionsRequest.json");
            var mOBMileageAndStatusOptionsRequest = JsonConvert.DeserializeObject<List<MOBMileageAndStatusOptionsRequest>>(mOBMileageAndStatusOptionsRequestJson);

            var sessionjson = TestDataGenerator.GetFileContent("SessionData.json");
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);

            var dOTBaggageInfoResponsejson = TestDataGenerator.GetFileContent("DOTBaggageInfoResponse.json");
            var dOTBaggageInfoResponse = JsonConvert.DeserializeObject<DOTBaggageInfoResponse>(dOTBaggageInfoResponsejson);

            var dOTBaggageInfojson = TestDataGenerator.GetFileContent("DOTBaggageInfo.json");
            var dOTBaggageInfo = JsonConvert.DeserializeObject<DOTBaggageInfo>(dOTBaggageInfojson);

            var getOffersjson = TestDataGenerator.GetFileContent("GetOffers.json");
            var getOffers = JsonConvert.DeserializeObject<List<GetOffers>>(getOffersjson);


            return new object[] { mOBMileageAndStatusOptionsRequest[0], session, reservationDetail[0], dOTBaggageInfoResponse, dOTBaggageInfo, getOffers[0] };
        }

        public Object[] set4()
        {

            var mOBGetActionDetailsForOffersRequestJson = TestDataGenerator.GetFileContent("MOBGetActionDetailsForOffersRequest.json");
            var mOBGetActionDetailsForOffersRequest = JsonConvert.DeserializeObject<List<MOBGetActionDetailsForOffersRequest>>(mOBGetActionDetailsForOffersRequestJson);

            var sessionjson = TestDataGenerator.GetFileContent("SessionData.json");
            var session = JsonConvert.DeserializeObject<Session>(sessionjson);

            var pnrByRecordLocatorResponsejson = TestDataGenerator.GetFileContent("pnrByRecordLocatorResponse.json");
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(pnrByRecordLocatorResponsejson);

            var mOBSeatChangeInitializeResponsejson = TestDataGenerator.GetFileContent("MOBSeatChangeInitializeResponse.json");
            var mOBSeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(mOBSeatChangeInitializeResponsejson);

            return new object[] { mOBGetActionDetailsForOffersRequest[0], session, pnrByRecordLocatorResponse[0], mOBSeatChangeInitializeResponse };
        }
    }
}
