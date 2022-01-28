using System;
namespace Tealium.Tests.Common
{
    public static class TealiumConfigHelper
    {
        static string INSTANCE_ID = "testinstance";
        static string ACCOUNT_NAME = "tealiummobile";
        static string PROFILE_NAME = "firebase-analytics";
        static Environment ENVIRONMENT = Environment.Dev;


        public static TealiumConfig GetTestConfigWithRemoteCommand(IRemoteCommand cmd)
        {
            return new TealiumConfig(ACCOUNT_NAME,
                PROFILE_NAME,
                ENVIRONMENT,
                new System.Collections.Generic.List<Dispatchers>() { Dispatchers.RemoteCommands },
                new System.Collections.Generic.List<Collectors>() { },
                remoteCommands: new System.Collections.Generic.List<IRemoteCommand>(1) { cmd });
        }

        public static TealiumConfig GetTestConfigWithRemoteCommandEnabled()
        {
            return new TealiumConfig(ACCOUNT_NAME,
                PROFILE_NAME,
                ENVIRONMENT,
                new System.Collections.Generic.List<Dispatchers>() { Dispatchers.RemoteCommands },
                new System.Collections.Generic.List<Collectors>() { });
        }

        public static TealiumConfig GetTestConfigWithConsentManagerEnabled(String instanceId = "default")
        {
            if (instanceId == "default") { instanceId = INSTANCE_ID; }//allows for a second instance name
            return new TealiumConfig(ACCOUNT_NAME,
                PROFILE_NAME,
                ENVIRONMENT,
                new System.Collections.Generic.List<Dispatchers>() { },
                new System.Collections.Generic.List<Collectors>() { },
                consentPolicy: ConsentManager.ConsentPolicy.CCPA);
        }

        public static TealiumConfig GetSimpleTestConfig(int configNumber = -1)
        {
            return GetSimpleTestConfigWithAutoTracking(true, configNumber);
        }

        public static TealiumConfig GetSimpleTestConfigWithAutoTracking(bool autoTracking, int configNumber = -1)
        {
            if (configNumber == -1)
            {
                return new TealiumConfig(ACCOUNT_NAME,
                                        PROFILE_NAME,
                                        ENVIRONMENT,
                                        new System.Collections.Generic.List<Dispatchers>() { },
                                        new System.Collections.Generic.List<Collectors>() { Collectors.LifeCycle },
                                        lifecycleAutotrackingEnabled: autoTracking);
            }
            else
            {
                return new TealiumConfig(ACCOUNT_NAME + configNumber,
                                        PROFILE_NAME + configNumber,
                                        ENVIRONMENT,
                                        new System.Collections.Generic.List<Dispatchers>() { },
                                        new System.Collections.Generic.List<Collectors>() { Collectors.LifeCycle },
                                        lifecycleAutotrackingEnabled: autoTracking);
            }
        }
    }
}
