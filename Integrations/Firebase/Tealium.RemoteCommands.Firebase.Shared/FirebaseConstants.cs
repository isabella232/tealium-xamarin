using System;
namespace Tealium.RemoteCommands.Firebase
{
    static class FirebaseConstants
    {
        public static readonly string KeySessionTimeout = "firebase_session_timeout_seconds";
        public static readonly string KeyAnalyticsEnabled = "firebase_analytics_enabled";
        // reserved for future use. log level can only be set
        public static readonly string KeyLogLevel = "firebase_log_level";
        public static readonly string KeyEventName = "firebase_event_name";
        public static readonly string KeyEventParams = "firebase_event_params";
        public static readonly string JSONKeyEventParams = "event";
        public static readonly string KeyItemsParams = "param_items";
        public static readonly string JSONKeyItemsParams = "items";
        public static readonly string KeyItemIdParam = "param_item_id";

        // deprecated
        public static readonly string KeyScreenName = "firebase_screen_name";
        // deprecated
        public static readonly string KeyScreenClass = "firebase_screen_class";

        public static readonly string KeyUserPropertyName = "firebase_property_name";
        public static readonly string KeyUserPropertyValue = "firebase_property_value";
        public static readonly string KeyUserId = "firebase_user_id";
        public static readonly string KeyCommandName = "command_name";

        /// <summary>
        /// Available Command names, this will be looked for in any Remote
        /// Command payload at the key of <see cref="KeyCommandName"/>
        /// </summary>
        public static class Commands
        {
            public const string Config = "config";
            public const string SetUserId = "setuserid";
            public const string SetUserProperty = "setuserproperty";
            public const string SetScreenName = "setscreenname";
            public const string LogEvent = "logevent";
        }
    }
}
