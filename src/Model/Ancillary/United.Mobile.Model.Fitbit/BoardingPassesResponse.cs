using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class BoardingPassesResponse : MOBResponse
    {
        public string RecordLocator { get; set; } = string.Empty;

        public List<EBP> EBPs { get; set; }

        public BoardingPassesResponse()
        {
            EBPs = new List<EBP>();
        }
    }
}
