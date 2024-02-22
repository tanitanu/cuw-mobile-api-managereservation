using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Mobile.Test.ManageReservation.Tests.TestData;

namespace United.Mobile.Test.ManageReservation.Tests
{
    class TestDataGenerator
    {
        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }


        public static IEnumerable<object[]> GetPNRByRecordLocator()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> GetPNRByRecordLocator1()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent(@"NegativeTestCases\pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> GetPNRByRecordLocator1_1()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[1], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> GetPNRByRecordLocator_Neg()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent(@"NegativeTestCases\pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> PerformInstantUpgrade()
        {
            var instantUpgradeRequest = JsonConvert.DeserializeObject<List<MOBInstantUpgradeRequest>>(GetFileContent("instantUpgradeRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { instantUpgradeRequest[0], pnrByRecordLocatorResponse[0] };


        }

        public static IEnumerable<object[]> PerformInstantUpgrade_neg()
        {
            var instantUpgradeRequest = JsonConvert.DeserializeObject<List<MOBInstantUpgradeRequest>>(GetFileContent(@"NegativeTestCases\instantUpgradeRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { instantUpgradeRequest[1], pnrByRecordLocatorResponse[0] };


        }

        public static IEnumerable<object[]> PerformInstantUpgrade_exception()
        {
            var instantUpgradeRequest = JsonConvert.DeserializeObject<List<MOBInstantUpgradeRequest>>(GetFileContent(@"NegativeTestCases\instantUpgradeRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { instantUpgradeRequest[1], pnrByRecordLocatorResponse[0] };


        }

        public static IEnumerable<object[]> GetOneClickEnrollmentDetailsForPNR()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> GetOneClickEnrollmentDetailsForPNR_codecov()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> GetOneClickEnrollmentDetailsForPNR_codecov1()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { pnrByRecordLocatorRequest[1], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> ConfirmScheduleChange_Test()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set1();
        }

        public static IEnumerable<object[]> ConfirmScheduleChange_excep()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set1_1();
        }

        public static IEnumerable<object[]> RequestReceiptByEmail()
        {
            var requestData = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(GetFileContent("MOBReceiptByEmailRequest.json"));
            var CommonDef = GetFileContent("CommonDef.json");
            var CommonDefData = JsonConvert.DeserializeObject<CommonDef>(CommonDef);

            yield return new object[] { requestData[0], CommonDefData };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2();

        }

        public static IEnumerable<object[]> RequestReceiptByEmail1()
        {
            var requestData = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(GetFileContent("MOBReceiptByEmailRequest.json"));
            var CommonDef = GetFileContent("CommonDef.json");
            var CommonDefData = JsonConvert.DeserializeObject<CommonDef>(CommonDef);
            yield return new object[] { requestData[0], CommonDefData };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2();

        }

        public static IEnumerable<object[]> RequestReceiptByEmail1_1()
        {
            var requestData = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(GetFileContent("MOBReceiptByEmailRequest.json"));

            var commonDefjson = TestDataGenerator.GetFileContent("CommonDef.json");
            var commonDef = JsonConvert.DeserializeObject<CommonDef>(commonDefjson);

            yield return new object[] { requestData[0], commonDef };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2();

        }

        public static IEnumerable<object[]> RequestReceiptByEmail_exception()
        {
            var requestData = JsonConvert.DeserializeObject<List<MOBReceiptByEmailRequest>>(GetFileContent("MOBReceiptByEmailRequest.json"));
            yield return new object[] { requestData[0] };

            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set2_1();

        }

        public static IEnumerable<object[]> GetMileageAndStatusOptions_Request()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set3();
        }

        public static IEnumerable<object[]> GetActionDetailsForOffers_Request()
        {
            TestDataSet testDataSet = new TestDataSet();
            yield return testDataSet.set4();
        }
        public static IEnumerable<object[]> GetPNRByRecordLocator_Flow()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest1.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse1.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }

        public static IEnumerable<object[]> PerformInstantUpgrade_Flow()
        {
            var instantUpgradeRequest = JsonConvert.DeserializeObject<List<MOBInstantUpgradeRequest>>(GetFileContent("instantUpgradeRequest.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse.json"));

            yield return new object[] { instantUpgradeRequest[0], pnrByRecordLocatorResponse[0] };


        }

        public static IEnumerable<object[]> GetOneClickEnrollmentDetailsForPNR_Flow()
        {
            var pnrByRecordLocatorRequest = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorRequest>>(GetFileContent("pnrByRecordLocatorRequest1.json"));
            var pnrByRecordLocatorResponse = JsonConvert.DeserializeObject<List<MOBPNRByRecordLocatorResponse>>(GetFileContent("pnrByRecordLocatorResponse1.json"));

            yield return new object[] { pnrByRecordLocatorRequest[0], pnrByRecordLocatorResponse[0] };
        }
    }
}
