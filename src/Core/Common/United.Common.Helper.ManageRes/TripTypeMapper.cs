using System.Collections.ObjectModel;
using UAWSFlightReservation;
//using United.Mobile.DAL.UAWSFlightReservation;
using United.Service.Presentation.SegmentModel;

namespace United.Common.Helper.ManageRes
{
    public class TripTypeMapper
    {
        private int _tripsLength;
        private string _firstTripOrigin;
        private string _finalTripDestination;

        public TripTypeMapper(Trip[] trips)
        {
            _tripsLength = trips.Length;
            if (_tripsLength == 2)
            {
                _firstTripOrigin = trips[0].Origin;
                _finalTripDestination = trips[1].Destination;
            }
        }

        public TripTypeMapper(Collection<ReservationFlightSegment> trips)
        {
            _tripsLength = trips.Count;

            if (_tripsLength == 2)
            {
                _firstTripOrigin = trips[0].FlightSegment.DepartureAirport.IATACode;
                _finalTripDestination = trips[1].FlightSegment.ArrivalAirport.IATACode;
            }
        }

        public string Map()
        {
            if (_tripsLength == 1)
                return "One-way";

            if (_tripsLength > 2)
                return "Multi-city";

            return _firstTripOrigin == _finalTripDestination ? "Roundtrip" : "Multi-city";
        }
    }
}