using System;
using Tealium;
namespace TealiumXamarinExample.Teal
{
    /// <summary>
    /// Keeps Tealium configuration parameters.
    /// </summary>
    public static class TealiumConstants
    {
        public static readonly string ACCOUNT_NAME = "tealiummobile";
        public static readonly string PROFILE_NAME = "firebase-tag";
#if DEBUG
        public static readonly Tealium.Environment ENVIRONMENT = Tealium.Environment.Dev;
#else
        public static readonly Tealium.Environment ENVIRONMENT =  Tealium.Environment.Prod;
#endif
        public static readonly string REMOTE_COMMAND_TRIGGER_EVENT = "HelperReady_BG_Queue";
        public static readonly string REMOTE_COMMAND_ID = "hello-world";
    }
}
