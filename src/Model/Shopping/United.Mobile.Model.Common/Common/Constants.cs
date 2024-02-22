using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    class Constants
    {
        internal sealed class Constant
        {
            /// <summary>
            /// This class is never instantiated, just provides a holding place for all 
            /// global constants for this assembly.
            /// </summary>
            private Constant() { }


            /// <summary>
            /// Configuration section name for DALFactory
            /// </summary>
            internal const string CONFIGURATION_SECTION_REWARDS = @"Rewards";


            /// <summary>
            /// Exception police name
            /// </summary>
            internal const string EXCEPTION_POLICY = @"Exception Policy";
        }
    }
}
