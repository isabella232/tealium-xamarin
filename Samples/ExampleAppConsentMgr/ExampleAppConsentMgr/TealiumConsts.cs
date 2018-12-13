using System;
using Tealium;

namespace ExampleAppConsentMgr
{
    public static class TealiumConsts
    {
        public static readonly string InstanceId = "Tealium_Main";
        public static readonly string AccountName = "tealiummobile";
        public static readonly string ProfileName = "demo";
        public static readonly string Environment = "dev";
        public static readonly string RemoteCommandId = "verify";

        public static ITealiumInstanceManager InstanceManager
        {
            get; 
            set;
        }

        public static ITealium DefaultInstance
        {
            get => InstanceManager?.GetExistingInstance(InstanceId);
        }

        public static ConsentManagerModel ConsentManagerModel
        {
            get; 
            set;
        }

    }
}
