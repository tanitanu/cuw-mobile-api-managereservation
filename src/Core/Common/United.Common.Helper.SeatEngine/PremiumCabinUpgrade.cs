using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Common.HelperSeatEngine;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping.Pcu;
using Reservation = United.Service.Presentation.ReservationModel.Reservation;

namespace United.Common.Helper.SeatEngine
{
    public class PremiumCabinUpgrade
    {
        private readonly ISessionHelperService _sessionHelperService;
        private readonly string _flightNumber;
        private readonly string _origin;
        private readonly string _destination;
        public string CartId;
        public bool IsValidOffer;
        public string RecordLocator;
        private List<PcuSegment> _pcuSegments;
        private List<string> _travelerNames;
        private readonly string _sessionId;

        public PremiumCabinUpgrade(ISessionHelperService sessionHelperService, string sessionId, string flightNumber, string origin, string destination)
        {
            _sessionHelperService = sessionHelperService;
            this._flightNumber = flightNumber;
            this._origin = origin;
            this._destination = destination;
            _sessionId = sessionId;
        }

        public async Task<PremiumCabinUpgrade> LoadOfferStateforSeatMap()
        {
            var pcuState = new PcuState();
            pcuState = await _sessionHelperService.GetSession<PcuState>(_sessionId, pcuState.ObjectName, new List<string> { _sessionId, pcuState.ObjectName }).ConfigureAwait(false);

            if (pcuState == null
                || pcuState.PremiumCabinUpgradeOfferDetail == null
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions == null
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleSegments == null
                || !pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleSegments.Any()
                || pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleTravelers == null
                || !pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleTravelers.Any())
                return this;

            _pcuSegments = pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleSegments;
            _travelerNames = pcuState.PremiumCabinUpgradeOfferDetail.PcuOptions.EligibleTravelers;
            RecordLocator = pcuState.RecordLocator;
            return this;
        }

        public List<PcuUpgradeOption> GetUpgradeOptionsForSeatMap()
        {
            if (_pcuSegments == null || !_pcuSegments.Any())
                return null;

            var segment = _pcuSegments.FirstOrDefault(s => s != null && s.Origin.Equals(_origin, StringComparison.OrdinalIgnoreCase) &&
                                                                        s.Destination.Equals(_destination, StringComparison.OrdinalIgnoreCase) &&
                                                                        s.FlightNumber.Equals(_flightNumber, StringComparison.OrdinalIgnoreCase));

            if (segment == null || segment.UpgradeOptions == null || !segment.UpgradeOptions.Any())
                return null;

            var upgradeOptions = segment.UpgradeOptions.Where(u => u != null && u.Price > 0);
            if (upgradeOptions == null || !upgradeOptions.Any())
                return null;

            return upgradeOptions.ToList();
        }

        public string GetTravelerNames()
        {
            if (_travelerNames == null || !_travelerNames.Any())
                return null;

            var travelerNames = string.Empty;
            _travelerNames.ForEach(t => travelerNames = string.IsNullOrEmpty(travelerNames) ? GetFormatedName(t) : travelerNames + ", " + GetFormatedName(t));
            return travelerNames;
        }

        private string GetFormatedName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return fullName;
            var name = fullName.Split(',');
            if (name.Length < 2)
                return name[0];

            var traveler = new { lastname = name[0], firstName = name[1] };
            return traveler.firstName + " " + traveler.lastname;
        }
    }

}

