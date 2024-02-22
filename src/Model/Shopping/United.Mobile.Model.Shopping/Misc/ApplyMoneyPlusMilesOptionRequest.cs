using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.Misc
{
	[Serializable]
	public class ApplyMoneyPlusMilesOptionRequest : ShoppingRequest
	{
		private string optionId;
		public string OptionId
		{
			get { return optionId; }
			set { optionId = value; }
		}

	}
}
