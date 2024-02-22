using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public enum UpgradeType
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
        ScheduleChange = 11,
    }
}
