using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Test.CancelReservation.Tests.TestData;

namespace United.Mobile.Test.CancelReservation.Tests

{
    class TestDataGenerator
    {
        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }
        public static IEnumerable<object[]> CancelRefundInfo()
        {
            //var EligibilityResponse = JsonConvert.DeserializeObject<List<EligibilityResponse>>(GetFileContent("EligibilityResponse.json"));

            //var requestData = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoRequest>>(GetFileContent("MOBCancelRefundInfoRequest.json"));

            //yield return new object[] { EligibilityResponse[0],requestData[0] };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set1();


        }

        public static IEnumerable<object[]> CancelRefundInfo_test()
        {

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set1_1();


        }

        public static IEnumerable<object[]> CancelAndRefund()
        {
            //var requestData = JsonConvert.DeserializeObject<List<MOBCancelAndRefundReservationRequest>>(GetFileContent("MOBCancelAndRefundReservationRequest.json"));
            //var MOBCancelRefundInfoResponse = JsonConvert.DeserializeObject<List<MOBCancelRefundInfoResponse>>(GetFileContent("MOBCancelRefundInfoResponse.json"));
            //var MOBQuoteRefundResponse = JsonConvert.DeserializeObject<List<MOBQuoteRefundResponse>>(GetFileContent("MOBQuoteRefundResponse.json"));

            //yield return new object[] { requestData[0], MOBCancelRefundInfoResponse[0], MOBQuoteRefundResponse[0] };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2();
        }

        public static IEnumerable<object[]> CancelAndRefund_test()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2_1();
        }

        public static IEnumerable<object[]> CheckinCancelRefundInfo()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set3();
        }

        public static IEnumerable<object[]> CheckinCancelRefundInfo_test()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set3_1();
        }

        public static IEnumerable<object[]> CheckinCancelRefundInfo_code()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set3_2();
        }

        public static IEnumerable<object[]> CheckinCancelRefundInfo_codes()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set3_3();
        }
    }
}
