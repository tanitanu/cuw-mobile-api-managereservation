using System;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable]
    public enum MOBUpgradeType
    {
        None=0,
        ComplimentaryPremierUpgrade=1,
        MileagePlusUpgradeAwards=2,
        GlobalPremierUpgrade=3,
        RegionalPremierUpgrade=4,
        RevenueUpgradeStandby=5,
        PremierCabinUpgrade=6,
        PremierInstantUpgrade=7,
        Waitlisted=8,
        Unknown=9,
        PlusPointsUpgrade = 10,
    }
}
