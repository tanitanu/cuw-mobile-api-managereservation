using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.GooglePay;

namespace United.Mobile.Test.GooglePay.Tests
{
    public class Input
    {
        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }

        public static IEnumerable<object[]> InputGooglePay()
        {
            var googlePayRequestjson = GetFileContent("GooglePayRequest.json");
            var AccessTokenClassjson = GetFileContent("AccessTokenClass.json");
            var FlightClassjson = GetFileContent("MOBGooglePayFlightClassDef.json");

            var googlePayRequest = JsonConvert.DeserializeObject<List<MOBGooglePayFlightRequest>>(googlePayRequestjson);

            var AccessToken = JsonConvert.DeserializeObject<List<AccessTokenClass>>(AccessTokenClassjson);
            var FlightClass = JsonConvert.DeserializeObject<List<MOBGooglePayFlightClassDef>>(AccessTokenClassjson);
            yield return new object[] {googlePayRequest[0], JsonConvert.SerializeObject(AccessToken[0]), JsonConvert.SerializeObject(FlightClass[0])};
            
        }

        public static IEnumerable<object[]> InputGooglePay1()
        {
            var UpdateFlightFromRequestRequestjson = GetFileContent("UpdateFlightFromRequestRequest.json");
            var AccessTokenClassjson = GetFileContent("AccessTokenClass.json");
            var FlightClassjson = GetFileContent("MOBGooglePayFlightClassDef.json");
            var UpdateFlightFromRequestRequest = JsonConvert.DeserializeObject<List<MOBGooglePayFlightRequest>>(UpdateFlightFromRequestRequestjson);

           
            var AccessToken = JsonConvert.DeserializeObject<List<AccessTokenClass>>(AccessTokenClassjson);
            var FlightClass = JsonConvert.DeserializeObject<List<MOBGooglePayFlightClassDef>>(AccessTokenClassjson);
            yield return new object[] { UpdateFlightFromRequestRequest[0], JsonConvert.SerializeObject(AccessToken[0]), JsonConvert.SerializeObject(FlightClass[0]) };


        }
    }
}
