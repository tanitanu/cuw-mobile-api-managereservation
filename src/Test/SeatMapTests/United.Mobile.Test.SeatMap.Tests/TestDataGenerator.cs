using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Test.SeatMap.Tests
{
    public class TestDataGenerator
    {
        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }

        public static IEnumerable<object[]> GetSeatMap()
        {
            var seatMapResponse = JsonConvert.DeserializeObject<List<Definition.SeatCSL30.SeatMap>>(GetFileContent("SeatMapResponse.json"));
            yield return new object[] { "ACCESSCODE", 2, seatMapResponse[0] };
            yield return new object[] { "ACCESSCODE", 2, seatMapResponse[1] };
            yield return new object[] { "ACCESSCODE", 2, seatMapResponse[2] };
            yield return new object[] { "ACCESSCODE", 1, seatMapResponse[0] };
            yield return new object[] { "ACCESSCODE", 6, seatMapResponse[0] };
            yield return new object[] { "ACCESSCODE", 16, seatMapResponse[0] };
            yield return new object[] { "ACCESSCODE", -1, seatMapResponse[0] };
            yield return new object[] { "0", 16, seatMapResponse[0] };
            yield return new object[] { "0", 2, seatMapResponse[0] };
            yield return new object[] { "0", 6, seatMapResponse[0] };
            yield return new object[] { "0", 1, seatMapResponse[0] };
            yield return new object[] { "0", -1, seatMapResponse[0] };
            yield return new object[] { "CODE", 6, seatMapResponse[0] };

        }

        public static IEnumerable<object[]> RegisterSeats()
        {
            var response = GetFileContent("RegisterSeatsResponse.json");
            var responsePayload = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

            var request = GetFileContent("RegisterSeatsRequest.json");
            var requestPayload = JsonConvert.DeserializeObject<MOBRegisterSeatsRequest[]>(request);

            var CheckOutResponse = JsonConvert.DeserializeObject<CheckOutResponse[]>(GetFileContent("CheckOutResponse.json"));

            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[5], responsePayload[0], CheckOutResponse[0] };
            ////yield return new object[] { requestPayload[4], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[3], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[2], responsePayload[1], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[2], responsePayload[2], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[1], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[1] };

        }

    
    }
}
