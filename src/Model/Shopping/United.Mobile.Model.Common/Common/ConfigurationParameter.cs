using System;
using System.Configuration;

namespace United.Mobile.Model.Shopping
{
    public class ConfigurationParameter
    {
        /// <summary>
        /// Encapsulates Type configuration settings
        /// </summary>
        public sealed class ConfigParameter
        {
            private RewardTypes _rewardTypes = null;
            private static ConfigParameter _instance = null;


            /// <summary>
            /// Default static cctor.
            /// </summary>
            static ConfigParameter() { }


            /// <summary>
            /// Internal instance ctor ensures that clients can access 
            /// only singleton instance of config object, 
            /// but our unit tests can create instances at will.
            /// </summary>
            internal ConfigParameter(RewardTypes rewardTypes)
            {
                _rewardTypes = rewardTypes;
            }


            /// <summary>
            /// Static singleton accessor to ConfigParameters
            /// </summary>
            public static ConfigParameter Configuration
            {
                get
                {
                    //  if null, get it now
                    if (null == _instance)
                    {
                        RewardSection config = null;
                        try
                        {
                            config = ConfigurationManager.GetSection(Constants.Constant.CONFIGURATION_SECTION_REWARDS) as RewardSection;
                        }
                        catch (Exception ex)
                        {
                            //bool rethrow = ExceptionPolicy.HandleException(ex, Constant.EXCEPTION_POLICY);
                            //if (rethrow)
                            throw ex;
                        }

                        RewardTypes rewardTypes = null;
                        if (null != config && config.RewardTypes.Count != 0)
                        {
                            rewardTypes = new RewardTypes();

                            RewardType rewardType = null;
                            for (int i = 0; i < config.RewardTypes.Count; ++i)
                            {
                                int key = Convert.ToInt32(config.RewardTypes[i].Key);
                                rewardType = new RewardType(key, config.RewardTypes[i].Type, config.RewardTypes[i].Description, config.RewardTypes[i].ProductID);
                                rewardTypes.Add(rewardType);
                            }
                        }

                        _instance = new ConfigParameter(rewardTypes);
                    }

                    return _instance;
                }
            }


            /// <summary>
            /// Exposes a strongly typed collection of DALType objects 
            /// which can be looked up by their interface name string.
            /// </summary>
            public RewardTypes RewardTypes
            {
                get
                {
                    return _rewardTypes;
                }
            }
        }
    }
}
