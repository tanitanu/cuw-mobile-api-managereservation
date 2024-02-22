using System;

namespace United.Mobile.Model.Shopping.Misc
{
    public class DevicePNRSegmet
    {
        private long id;
        private string deviceId;
        private string applicationId;
        private string applicationVersion;
        private string mileagePlusNumber;
        private string pushToken;
        private string recordLocator;
        private DateTime creationTime;
        private string lastName;
        private string flightNumber;
        private DateTime flightDate;
        private string origin;
        private string destination;
        private DateTime scheduledDepartureTime;
        private DateTime scheduledArrivalTime;
        private DateTime scheduledDepartureTimeUtc;
        private DateTime scheduledArrivalTimeUtc;
        
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public string ApplicationId
        {
            get { return applicationId; }
            set { applicationId = value; }
        }

        public string ApplicationVersion
        {
            get { return applicationVersion; }
            set { applicationVersion = value; }
        }

        public string MilagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }

        public string PushToken
        {
            get { return pushToken; }
            set { pushToken = value; }
        }

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        public DateTime CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        public DateTime FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public DateTime ScheduledDepartureTime
        {
            get { return scheduledDepartureTime; }
            set { scheduledDepartureTime = value; }
        }

        public DateTime ScheduledArrivalTime
        {
            get { return scheduledArrivalTime; }
            set { scheduledArrivalTime = value; }
        }

        public DateTime ScheduledDepartureTimeUtc
        {
            get { return scheduledDepartureTimeUtc; }
            set { scheduledDepartureTimeUtc = value; }
        }

        public DateTime ScheduledArrivalTimeUtc
        {
            get { return scheduledArrivalTimeUtc; }
            set { scheduledArrivalTimeUtc= value; }
        }
    }
}
