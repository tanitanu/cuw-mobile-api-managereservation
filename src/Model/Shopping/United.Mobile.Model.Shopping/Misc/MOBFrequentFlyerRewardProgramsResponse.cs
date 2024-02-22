using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Shopping;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FrequentFlyerRewardProgramsResponse : MOBResponse
    {
        private List<RewardProgram> rewardProgramList;

        public List<RewardProgram> RewardProgramList
        {
            get
            {
                return this.rewardProgramList;
            }
            set
            {
                this.rewardProgramList = value;
            }
        }
    }
}
