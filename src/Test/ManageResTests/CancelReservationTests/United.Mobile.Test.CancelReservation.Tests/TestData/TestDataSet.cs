using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using United.Mobile.Model.MPRewards;
using System.Text;
using United.Mobile.Model.ManageRes;
using United.Service.Presentation.ReservationResponseModel;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.AccountManagement;

namespace United.Mobile.Test.CancelReservation.Tests.TestData
{
    public class TestDataSet
    {
        public Object[] set1()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);



            return new object[] { EligibilityResponse[0], requestData[0], CommonDef, reservationDetail[0], hashPinValidate };

        }

        public Object[] set1_1()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { EligibilityResponse[0], requestData[1], CommonDef, reservationDetail[0], hashPinValidate };

        }

        public Object[] set2()
        {

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelAndRefundReservationRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelAndRefundReservationRequest>>(requestDataJson);

            var MOBCancelRefundInfoResponseJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoResponse.json");
            var MOBCancelRefundInfoResponse = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoResponse>>(MOBCancelRefundInfoResponseJson);

            var MOBQuoteRefundResponseJson = TestDataGenerator.GetFileContent("MOBQuoteRefundResponse.json");
            var MOBQuoteRefundResponse = JsonConvert.DeserializeObject<List<MOBQuoteRefundResponse>>(MOBQuoteRefundResponseJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailjson);

            var mOBVormetricKeysjson = TestDataGenerator.GetFileContent("MOBVormetricKeys.json");
            var mOBVormetricKeys = JsonConvert.DeserializeObject<List<MOBVormetricKeys>>(mOBVormetricKeysjson);

            return new object[] { requestData[0], MOBCancelRefundInfoResponse[0], MOBQuoteRefundResponse[0], CommonDef, reservationDetail[0], mOBVormetricKeys[0] };

        }

        public Object[] set2_1()
        {

            var mOBCancelAndRefundReservationRequestJson = TestDataGenerator.GetFileContent("MOBCancelAndRefundReservationRequest.json");
            var mOBCancelAndRefundReservationRequest = JsonConvert.DeserializeObject<List<MOBCancelAndRefundReservationRequest>>(mOBCancelAndRefundReservationRequestJson);

            var MOBCancelRefundInfoResponseJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoResponse.json");
            var MOBCancelRefundInfoResponse = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoResponse>>(MOBCancelRefundInfoResponseJson);

            var MOBQuoteRefundResponseJson = TestDataGenerator.GetFileContent("MOBQuoteRefundResponse.json");
            var MOBQuoteRefundResponse = JsonConvert.DeserializeObject<List<MOBQuoteRefundResponse>>(MOBQuoteRefundResponseJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailjson);

            var mOBVormetricKeysjson = TestDataGenerator.GetFileContent("MOBVormetricKeys.json");
            var mOBVormetricKeys = JsonConvert.DeserializeObject<List<MOBVormetricKeys>>(mOBVormetricKeysjson);


            return new object[] { mOBCancelAndRefundReservationRequest[1], MOBCancelRefundInfoResponse[1], MOBQuoteRefundResponse[1], CommonDef, reservationDetail[0], mOBVormetricKeys[0] };

        }

        public Object[] set3()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { EligibilityResponse[0], requestData[0], CommonDef, reservationDetail[0], hashPinValidate };

        }

        public Object[] set3_1()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { EligibilityResponse[0], requestData[0], CommonDef, reservationDetail[0], hashPinValidate };

        }

        public Object[] set3_2()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { EligibilityResponse[0], requestData[2], CommonDef, reservationDetail[0], hashPinValidate };

        }


        public Object[] set3_3()
        {


            var EligibilityResponseJson = TestDataGenerator.GetFileContent("EligibilityResponse.json");
            var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(EligibilityResponseJson);

            var requestDataJson = TestDataGenerator.GetFileContent("MOBCancelRefundInfoRequest.json");
            var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(requestDataJson);

            var CommonDefJson = TestDataGenerator.GetFileContent("CommonDef.json");
            var CommonDef = JsonConvert.DeserializeObject<CommonDef>(CommonDefJson);

            var reservationDetailJson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<ReservationDetail>>(reservationDetailJson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { EligibilityResponse[0], requestData[3], CommonDef, reservationDetail[0], hashPinValidate };

        }
    }
}
