using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Service.Presentation.PersonalizationResponseModel;

namespace United.Mobile.Test.PreOrderMeals.Tests
{
    public class TestDataSet
    {
        public Object[] set1()
        {

            var mOBInFlightMealsOfferRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferRequest.json");
            var mOBInFlightMealsOfferRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsOfferRequest>>(mOBInFlightMealsOfferRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);

            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);

            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);

            var mOBInFlightMealsOfferResponsejson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferResponse.json");
            var mOBInFlightMealsOfferResponse = JsonConvert.DeserializeObject<MOBInFlightMealsOfferResponse>(mOBInFlightMealsOfferResponsejson);

            return new object[] { mOBInFlightMealsOfferRequest[0], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], mOBInFlightMealsOfferResponse };
        }

        public Object[] set1_1()
        {

            var mOBInFlightMealsOfferRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferRequest.json");
            var mOBInFlightMealsOfferRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsOfferRequest>>(mOBInFlightMealsOfferRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);

            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);

            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);

            var mOBInFlightMealsOfferResponsejson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferResponse.json");
            var mOBInFlightMealsOfferResponse = JsonConvert.DeserializeObject<MOBInFlightMealsOfferResponse>(mOBInFlightMealsOfferResponsejson);

            return new object[] { mOBInFlightMealsOfferRequest[1], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], mOBInFlightMealsOfferResponse };
        }

        public Object[] set1_2()
        {
            var mOBInFlightMealsOfferRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferRequest.json");
            var mOBInFlightMealsOfferRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsOfferRequest>>(mOBInFlightMealsOfferRequestjson);
            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);
            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);
            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);
            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);
            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);
            var mOBInFlightMealsOfferResponsejson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferResponse.json");
            var mOBInFlightMealsOfferResponse = JsonConvert.DeserializeObject<MOBInFlightMealsOfferResponse>(mOBInFlightMealsOfferResponsejson);
            return new object[] { mOBInFlightMealsOfferRequest[2], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], mOBInFlightMealsOfferResponse };
        }

        public Object[] set2()
        {

            var mOBInFlightMealsRefreshmentsRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsRefreshmentsRequest.json");
            var mOBInFlightMealsRefreshmentsRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsRefreshmentsRequest>>(mOBInFlightMealsRefreshmentsRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);

            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);

            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);

            var checkOutResponsejson = TestDataGenerator.GetFileContent("CheckOutResponse.json");
            var checkOutResponse = JsonConvert.DeserializeObject<List<CheckOutResponse>>(checkOutResponsejson);

           var getOffersCcejson = TestDataGenerator.GetFileContent("GetOffersCce.json");
            var getOffersCce = JsonConvert.DeserializeObject<GetOffersCce>(getOffersCcejson);

            var mOBInFlightMealsRefreshmentsResponsesjson = TestDataGenerator.GetFileContent("MOBInFlightMealsRefreshmentsResponse.json");
            var mOBInFlightMealsRefreshmentsResponses = JsonConvert.DeserializeObject<List<List<MOBInFlightMealsRefreshmentsResponse>>>(mOBInFlightMealsRefreshmentsResponsesjson);


            return new object[] { mOBInFlightMealsRefreshmentsRequest[0], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], checkOutResponse[0], getOffersCce, mOBInFlightMealsRefreshmentsResponses[0] };
        }
        public Object[] set2_1()
        {
            var mOBInFlightMealsRefreshmentsRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsRefreshmentsRequest.json");
            var mOBInFlightMealsRefreshmentsRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsRefreshmentsRequest>>(mOBInFlightMealsRefreshmentsRequestjson);
            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);
            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);
            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);
            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);
            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);
            var checkOutResponsejson = TestDataGenerator.GetFileContent("CheckOutResponse.json");
            var checkOutResponse = JsonConvert.DeserializeObject<List<CheckOutResponse>>(checkOutResponsejson);
            var getOffersCcejson = TestDataGenerator.GetFileContent("GetOffersCce.json");
            var getOffersCce = JsonConvert.DeserializeObject<GetOffersCce>(getOffersCcejson);
            var mOBInFlightMealsRefreshmentsResponsesjson = TestDataGenerator.GetFileContent("MOBInFlightMealsRefreshmentsResponse.json");
            var mOBInFlightMealsRefreshmentsResponses = JsonConvert.DeserializeObject<List<List<MOBInFlightMealsRefreshmentsResponse>>>(mOBInFlightMealsRefreshmentsResponsesjson);
            return new object[] { mOBInFlightMealsRefreshmentsRequest[2], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], checkOutResponse[0], getOffersCce, mOBInFlightMealsRefreshmentsResponses[0] };
        }
        public Object[] set3()
        {

            var mOBInFlightMealsOfferRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferRequest.json");
            var mOBInFlightMealsOfferRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsOfferRequest>>(mOBInFlightMealsOfferRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);

            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);

            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);

            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);

            var checkOutResponsejson = TestDataGenerator.GetFileContent("CheckOutResponse.json");
            var checkOutResponse = JsonConvert.DeserializeObject<List<CheckOutResponse>>(checkOutResponsejson);

            //var getOffersCcejson = TestDataGenerator.GetFileContent("GetOffersCce.json");
            //var getOffersCce = JsonConvert.DeserializeObject<GetOffersCce>(getOffersCcejson);

            var mOBInFlightMealsOfferResponsejson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferResponse.json");
            var mOBInFlightMealsOfferResponse = JsonConvert.DeserializeObject<MOBInFlightMealsOfferResponse>(mOBInFlightMealsOfferResponsejson);

            return new object[] { mOBInFlightMealsOfferRequest[0], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], checkOutResponse[0], mOBInFlightMealsOfferResponse };
        }
        public Object[] set3_1()
        {
            var mOBInFlightMealsOfferRequestjson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferRequest.json");
            var mOBInFlightMealsOfferRequest = JsonConvert.DeserializeObject<List<MOBInFlightMealsOfferRequest>>(mOBInFlightMealsOfferRequestjson);
            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);
            var cMSContentMessageJson = TestDataGenerator.GetFileContent("CMSContentMessage.json");
            var cMSContentMessage = JsonConvert.DeserializeObject<List<List<CMSContentMessage>>>(cMSContentMessageJson);
            var reservationDetailjson = TestDataGenerator.GetFileContent("ReservationDetail.json");
            var reservationDetail = JsonConvert.DeserializeObject<List<United.Service.Presentation.ReservationResponseModel.ReservationDetail>>(reservationDetailjson);
            var mOBPNRjson = TestDataGenerator.GetFileContent("MOBPNR.json");
            var mOBPNR = JsonConvert.DeserializeObject<MOBPNR>(mOBPNRjson);
            var dynamicOfferDetailResponsejson = TestDataGenerator.GetFileContent("DynamicOfferDetailResponse.json");
            var dynamicOfferDetailResponse = JsonConvert.DeserializeObject<List<DynamicOfferDetailResponse>>(dynamicOfferDetailResponsejson);
            var checkOutResponsejson = TestDataGenerator.GetFileContent("CheckOutResponse.json");
            var checkOutResponse = JsonConvert.DeserializeObject<List<CheckOutResponse>>(checkOutResponsejson);
            //var getOffersCcejson = TestDataGenerator.GetFileContent("GetOffersCce.json");
            //var getOffersCce = JsonConvert.DeserializeObject<GetOffersCce>(getOffersCcejson);
            var mOBInFlightMealsOfferResponsejson = TestDataGenerator.GetFileContent("MOBInFlightMealsOfferResponse.json");
            var mOBInFlightMealsOfferResponse = JsonConvert.DeserializeObject<MOBInFlightMealsOfferResponse>(mOBInFlightMealsOfferResponsejson);
            return new object[] { mOBInFlightMealsOfferRequest[2], session[0], cMSContentMessage[0], reservationDetail[1], mOBPNR, dynamicOfferDetailResponse[0], checkOutResponse[0], mOBInFlightMealsOfferResponse };
        }
    }
}
