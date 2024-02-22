using System;

namespace United.Mobile.Model.MicrositeServices
{
    [Serializable()]
    public class MOBBaggageDetail
    {
        private string tripInfo; // Chicago, IL(ORD) to Denver, CO(DEN) 
        private string flightDate; // Wed. Jan 31, 2018
        private string firstCheckedBagPrice; // $25
        private string firstPrePaidBagPrice = string.Empty; // $25
        private string secondCheckedBagPrice; // $35
        private string secondPrePaidBagPrice = string.Empty; // $25
        private string weightPerBag; // 50lbs (23kg)

        public string TripInfo
        {
            get { return tripInfo; }
            set { tripInfo = value; }
        }

        public string FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }
        public string FirstCheckedBagPrice
        {
            get { return firstCheckedBagPrice; }
            set { firstCheckedBagPrice = value; }
        }
        public string FirstPrePaidBagPrice
        {
            get { return firstPrePaidBagPrice; }
            set { firstPrePaidBagPrice = value; }
        }
        public string SecondCheckedBagPrice
        {
            get { return secondCheckedBagPrice; }
            set { secondCheckedBagPrice = value; }
        }
        public string SecondPrePaidBagPrice
        {
            get { return secondPrePaidBagPrice; }
            set { secondPrePaidBagPrice = value; }
        }
        public string WeightPerBag
        {
            get { return weightPerBag; }
            set { weightPerBag = value; }
        }


    }
}
