using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public enum MessageCode
    {
        None=0,
        SpaceAvailableBeforeDepartureGoldAndHigher=1,
        SpaceAvailableBeforeDepartureSilver=2,
        SpaceAvailableCpuToCabinOutsideWindowGoldAndHigher=3,
        SpaceAvailableCpuToCabinOutsideWindowSilver=4,
        InstantUpgrade=5,
        SpaceAvailableInsideWindowGoldAndHigher=6,
        SpaceAvailableInsideWindowSilverAndDayOfDeparture=7,
        WaitlistUpgradeRequested=8,
        ConfirmedUpgrade=9,
        WaitlistedSameFlightDifferentNonUpgradeClass=10,
        WaitlistedSegmentOnAnotherFlight=11,
        WaitlistClassConfirmed=12,
    }
}
