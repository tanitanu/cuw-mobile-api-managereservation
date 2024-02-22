using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.UpgradeCabin;

namespace United.Mobile.Test.UpgradeCabin.Tests
{
   public class TestDataSet
    {

        public Object[] set1()
        {

            var mOBUpgradePlusPointWebMyTripRequestjson = TestDataGenerator.GetFileContent("MOBUpgradePlusPointWebMyTripRequest.json");
            var mOBUpgradePlusPointWebMyTripRequest = JsonConvert.DeserializeObject<List<MOBUpgradePlusPointWebMyTripRequest>>(mOBUpgradePlusPointWebMyTripRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { mOBUpgradePlusPointWebMyTripRequest[0], session[0], hashPinValidate };
        }

        public Object[] set1_1()
        {

            var mOBUpgradePlusPointWebMyTripRequestjson = TestDataGenerator.GetFileContent("MOBUpgradePlusPointWebMyTripRequest.json");
            var mOBUpgradePlusPointWebMyTripRequest = JsonConvert.DeserializeObject<List<MOBUpgradePlusPointWebMyTripRequest>>(mOBUpgradePlusPointWebMyTripRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { mOBUpgradePlusPointWebMyTripRequest[1], session[0], hashPinValidate };
        }

        public Object[] set1_2()
        {

            var mOBUpgradePlusPointWebMyTripRequestjson = TestDataGenerator.GetFileContent("MOBUpgradePlusPointWebMyTripRequest1.json");
            var mOBUpgradePlusPointWebMyTripRequest = JsonConvert.DeserializeObject<List<MOBUpgradePlusPointWebMyTripRequest>>(mOBUpgradePlusPointWebMyTripRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { mOBUpgradePlusPointWebMyTripRequest[0], session[0], hashPinValidate };
        }

        public Object[] set2()
        {

            var mOBUpgradeCabinEligibilityRequestjson = TestDataGenerator.GetFileContent("MOBUpgradeCabinEligibilityRequest.json");
            var mOBUpgradeCabinEligibilityRequest = JsonConvert.DeserializeObject<List<MOBUpgradeCabinEligibilityRequest>>(mOBUpgradeCabinEligibilityRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { mOBUpgradeCabinEligibilityRequest[0], session[0], hashPinValidate };
        }

        public Object[] set2_1()
        {

            var mOBUpgradeCabinEligibilityRequestjson = TestDataGenerator.GetFileContent("MOBUpgradeCabinEligibilityRequest.json");
            var mOBUpgradeCabinEligibilityRequest = JsonConvert.DeserializeObject<List<MOBUpgradeCabinEligibilityRequest>>(mOBUpgradeCabinEligibilityRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);




            return new object[] { mOBUpgradeCabinEligibilityRequest[1], session[0], hashPinValidate };
        }

        public Object[] set2_2()
        {

            var mOBUpgradeCabinEligibilityRequestjson = TestDataGenerator.GetFileContent("MOBUpgradeCabinEligibilityRequest.json");
            var mOBUpgradeCabinEligibilityRequest = JsonConvert.DeserializeObject<List<MOBUpgradeCabinEligibilityRequest>>(mOBUpgradeCabinEligibilityRequestjson);

            var sessionjson = TestDataGenerator.GetFileContent("Session.json");
            var session = JsonConvert.DeserializeObject<List<Session>>(sessionjson);

            var hashPinValidateJson = TestDataGenerator.GetFileContent("HashPinValidate.json");
            var hashPinValidate = JsonConvert.DeserializeObject<HashPinValidate>(hashPinValidateJson);


            return new object[] { mOBUpgradeCabinEligibilityRequest[0], session[0], hashPinValidate };
        }

    }
}
