using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Test.ViewResSeatMap.Tests
{
    public class TestDataGenerator
    {
        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }      
        public static IEnumerable<object[]> InputSelectSeats()
        {
            var response = GetFileContent("SeatChangeState.json");
            var seatChange = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 1, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 1, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[1] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "CODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "SAN", "SFO", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "100", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "UAWS-MOBILE-ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };
        }

        public static IEnumerable<object[]> InputSelectSeats1()
        {
            var response = GetFileContent("SeatChangeState.json");
            var seatChange = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

           // yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 1, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "SUN., MAR. 13, 2022", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 1, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[1] };

            yield return new object[] { "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "CODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "SAN", "SFO", seatChange[0] };

            yield return new object[] { "59AD27EB-9B93-47C2-B275-D45EC7DC524F", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 3, "100", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };

            yield return new object[] { "UAWS-MOBILE-ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0,0,0", "seat|9.0|Data|array|PSL,Assignment,Assign", "nextOrigin", "nextDestination", seatChange[0] };


        }

        public static IEnumerable<object[]> InputSeatChangeInitialize()
        {
            var data = GetFileContent("MOBSeatChangeInitializeRequest.json");
            var request = JsonConvert.DeserializeObject<MOBSeatChangeInitializeRequest[]>(data);
            var SeatEngine = JsonConvert.DeserializeObject<Model.MPRewards.SeatEngine[]>(GetFileContent("SeatEngineResponse.json"));
            yield return new object[] { request[1], SeatEngine[0] };
            yield return new object[] { request[1], SeatEngine[1] };
            yield return new object[] { request[1], SeatEngine[2] };
            yield return new object[] { request[0], SeatEngine[1] };
            yield return new object[] { request[2], SeatEngine[1] };
            yield return new object[] { request[3], SeatEngine[1] };
        }

        public static IEnumerable<object[]> RegisterSeats()
        {
            var response = GetFileContent("RegisterSeatsResponse.json");
            var responsePayload = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

            var request = GetFileContent("RegisterSeatsRequest.json");
            var requestPayload = JsonConvert.DeserializeObject<MOBRegisterSeatsRequest[]>(request);

            var CheckOutResponse = JsonConvert.DeserializeObject<CheckOutResponse[]>(GetFileContent("CheckOutResponse.json"));

            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[5], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[4], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[3], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[1], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[1], CheckOutResponse[0] };
            yield return new object[] { requestPayload[1], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[1] };

        }

        public static IEnumerable<object[]> RegisterSeats1()
        {
            var response = GetFileContent("RegisterSeatsResponse.json");
            var responsePayload = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

            var request = GetFileContent("RegisterSeatsRequest.json");
            var requestPayload = JsonConvert.DeserializeObject<MOBRegisterSeatsRequest[]>(request);

            var CheckOutResponse = JsonConvert.DeserializeObject<CheckOutResponse[]>(GetFileContent("CheckOutResponse.json"));

            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[5], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[4], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[3], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[1], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[2], CheckOutResponse[0] };
            yield return new object[] { requestPayload[1], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[1] };

        }

        public static IEnumerable<object[]> RegisterSeats1_1()
        {
            var response = GetFileContent("RegisterSeatsResponse.json");
            var responsePayload = JsonConvert.DeserializeObject<SeatChangeState[]>(response);

            var request = GetFileContent("RegisterSeatsRequest.json");
            var requestPayload = JsonConvert.DeserializeObject<MOBRegisterSeatsRequest[]>(request);

            var CheckOutResponse = JsonConvert.DeserializeObject<CheckOutResponse[]>(GetFileContent("CheckOutResponse.json"));

            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[5], responsePayload[0], CheckOutResponse[0] };
            //yield return new object[] { requestPayload[4], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[3], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[1], CheckOutResponse[0] };
            yield return new object[] { requestPayload[2], responsePayload[2], CheckOutResponse[0] };
            yield return new object[] { requestPayload[1], responsePayload[0], CheckOutResponse[0] };
            yield return new object[] { requestPayload[0], responsePayload[0], CheckOutResponse[1] };

        }
    }
}
