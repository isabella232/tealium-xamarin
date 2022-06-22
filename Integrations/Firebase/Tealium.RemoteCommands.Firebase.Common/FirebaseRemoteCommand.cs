using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tealium.RemoteCommands.Firebase
{

    public abstract class FirebaseRemoteCommand : IRemoteCommand
    {

        

        protected static readonly Dictionary<string, string> eventsMap = new Dictionary<string, string>();
        protected static readonly Dictionary<string, string> parameters = new Dictionary<string, string>();

        public static readonly string DefaultCommandId = "firebaseAnalytics";
        public static readonly string DefaultCommandDesc = "Tealium Firebase Analytics integration";


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.RemoteCommands.Firebase.FirebaseRemoteCommand"/> class.
        /// Pre-built Remote Command supporting Firebase Analytics for both iOS
        /// and Android on Xamarin. Empty constructor will use the 
        /// <see langword="static"/> String value of firebaseAnalytics as the
        /// command id.
        /// </summary>
        public FirebaseRemoteCommand() : this(DefaultCommandId, DefaultCommandDesc)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.RemoteCommands.Firebase.FirebaseRemoteCommand"/> class.
        /// Pre-built Remote Command supporting Firebase Analytics for both iOS
        /// and Android on Xamarin. Supply a commmand id and description to 
        /// override the defaults.
        /// </summary>
        /// <param name="commandId">Command identifier.</param>
        /// <param name="description">Description.</param>
        public FirebaseRemoteCommand(string commandId, string description)
        {
            CommandId = commandId ?? DefaultCommandId;
            Description = description ?? DefaultCommandDesc;
        }

        public string CommandId { get; set; }

        public string Description { get; set; }

        public abstract string Path { get; }
        public abstract string Url { get; }

        public string Name => "xamarin-"+CommandId;

        public string Version => "1.0.0";

        /// <summary>
        /// Handles the Remote Command response - this contains all the logic 
        /// for executing the different functions Firebase Analytics.
        /// A comma separated list of command names as set out in the <see cref="Commands"/>
        /// class is required at the paylod key <see cref="KeyCommandName"/>
        /// </summary>
        /// <param name="response">Response.</param>
        public void HandleResponse(IRemoteCommandResponse response)
        {
            string command = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyCommandName);
            if (string.IsNullOrEmpty(command))
            {
                return;
            }

            string[] commandArray;
            // split the commands into an array
            commandArray = command.Split(',');

            for (int j = 0, commandlen = commandArray.Length; j < commandlen; j++)
            {
                try
                {
                    command = commandArray[j];
                    command = command.Trim().ToLower();
                    switch (command)
                    {
                        case FirebaseConstants.Commands.Config:
                            int? timeout = null;
                            try
                            {
                                timeout = response.Payload.ContainsKey(FirebaseConstants.KeySessionTimeout) == true ?
                                            (int?)int.Parse(response.Payload.GetValueForKey<string>(FirebaseConstants.KeySessionTimeout)) * 1000 : null;
                            }
                            catch { }

                            bool? analyticsEnabled = null;
                            try
                            {
                                analyticsEnabled = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyAnalyticsEnabled) != null && response.Payload.GetValueForKey<string>(FirebaseConstants.KeyAnalyticsEnabled) == "false" ?
                                            false : true;
                            }
                            catch { }
                            string loggerLevel = null;
                            try
                            {
                                loggerLevel = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyLogLevel);
                            }
                            catch { }
                            Configure(timeout, analyticsEnabled, loggerLevel);

                            break;
                        case FirebaseConstants.Commands.LogEvent:
                            string eventName = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyEventName);

                            Dictionary<string, object> eventParams = new Dictionary<string, object>();
                            string paramKey = response.Payload.ContainsKey(FirebaseConstants.JSONKeyEventParams) ? FirebaseConstants.JSONKeyEventParams : response.Payload.ContainsKey(FirebaseConstants.KeyEventParams) ? FirebaseConstants.KeyEventParams : null;
                            if (paramKey != null)
                            {
                                try
                                {
                                    eventParams = response.Payload.GetValueForKey<Dictionary<string, object>>(paramKey);
                                }
                                catch
                                {
                                }
                            }

                            if (response.Payload.ContainsKey(FirebaseConstants.JSONKeyItemsParams))
                            {
                                try
                                {
                                    Dictionary<string, object> items = response.Payload.GetValueForKey<Dictionary<string, object>>(FirebaseConstants.JSONKeyItemsParams);
                                    eventParams[FirebaseConstants.KeyItemsParams] = FormatItems(items);
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine("Error handling " + e.Message);
                                    System.Diagnostics.Debug.WriteLine("Stack Trace: " + e.StackTrace);
                                }
                            }
                            if (eventParams.ContainsKey(FirebaseConstants.KeyItemsParams)) // Both if we manually formatted or if they come from the webview already
                            {
                                Dictionary<string, object>[] items = (Dictionary<string,object>[])eventParams[FirebaseConstants.KeyItemsParams];

                                eventParams[FirebaseConstants.KeyItemsParams] = items.Select(item => MapParamKeys(item)).ToArray();
                            }
                            if (!IsNullOrNullString(eventName))
                            {
                                LogEvent(mapEventNames(eventName), MapParamKeys(eventParams));
                            }
                            break;
                        case FirebaseConstants.Commands.SetScreenName:
                            string screenName = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyScreenName);
                            string screenClass = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyScreenClass);
                            if (!IsNullOrNullString(screenName) && !IsNullOrNullString(screenClass))
                            {
                                SetScreenName(screenName, screenClass);
                            }
                            break;
                        case FirebaseConstants.Commands.SetUserProperty:
                            string propertyName = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyUserPropertyName);
                            string propertyValue = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyUserPropertyValue);
                            if (!IsNullOrNullString(propertyName) && !IsNullOrNullString(propertyValue))
                            {
                                SetUserProperty(propertyName, propertyValue);
                            }
                            break;
                        case FirebaseConstants.Commands.SetUserId:
                            string userId = response.Payload.GetValueForKey<string>(FirebaseConstants.KeyUserId);
                            if (!IsNullOrNullString(userId))
                            {
                                SetUserId(userId);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error handling " + this.CommandId + ":" + command + " - " + e.Message);
                    System.Diagnostics.Debug.WriteLine("Stack Trace: " + e.StackTrace);
                }

            }
        }

        Dictionary<string, object> MapParamKeys(Dictionary<string, object> dict)
        {
            return dict.Select(tuple => new KeyValuePair<string, object>(mapParams(tuple.Key), tuple.Value))
                                            .ToDictionary(tuple => tuple.Key, tuple => tuple.Value);
        }

        private Dictionary<string, object>[] FormatItems(Dictionary<string,object> items)
        {
            if (!items.ContainsKey(FirebaseConstants.KeyItemIdParam))
            {
                return new Dictionary<string, object>[] { items };
            }
            var paramId = items[FirebaseConstants.KeyItemIdParam];
            var newItems = new Dictionary<string, object>();
            var isList = paramId is IList;
            foreach (var key in items.Keys)
            {
                object[] list;
                if (isList)
                {
                    list = new object[((IList)paramId).Count];
                    ((IList)items[key]).CopyTo(list, 0);
                } else
                {
                    list = new object[] { items[key] };
                }
                newItems[key] = list;
            }
            object[] paramIds = (object[])newItems[FirebaseConstants.KeyItemIdParam];
            Dictionary<string, object>[] result = new Dictionary<string, object>[paramIds.Length];
            for (int i = 0; i < paramIds.Length; i++) 
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (var key in newItems.Keys)
                {
                    object[] list = (object[])newItems[key];
                    dict[key] = list[i];
                }
                result[i] = dict;
            }
            return result;
        }

        /// <summary>
        /// Helper method that returns a known event name or the <paramref name="eventName"/>
        /// provided.
        /// </summary>
        /// <returns>The known even name, or provided event name</returns>
        /// <param name="eventName">Event name.</param>
        protected static string mapEventNames(string eventName)
        {
            if (!eventsMap.ContainsKey(eventName))
            {
                return eventName;
            }
            return eventsMap[eventName] ?? eventName;
        }

        /// <summary>
        /// Helper method that returns a known param name or the <paramref name="param"/>
        /// provided.        
        /// </summary>
        /// <returns>The known parameter name, or provided param name.</returns>
        /// <param name="param">Parameter.</param>
        protected static string mapParams(string param)
        {
            if (!parameters.ContainsKey(param))
            {
                return param;
            }
            return parameters[param] ?? param;
        }

        /// <summary>
        /// Helper method to determine whether a string is null, or a null string.
        /// The conversion from a <see langword="null"/> entry in an iOS disctionary
        /// will end up casting the null to a string value "&lt;null&gt;"
        /// Although this is iOS specific, we need to keep the handling in this class
        /// so we can stop any unnecessary subclass method calls.
        /// </summary>
        /// <returns><c>true</c>, if null or null string was found, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        private bool IsNullOrNullString(string value)
        {
            //iOS null values in a NSDictionary come through as "<null>" string.
            return value == null || value == "<null>";
        }

        /// <summary>
        /// Configure Firebase Analytics with the specified sessionTimeoutDuration, 
        /// minSessionDuration and analyticsEnabled.
        /// </summary>
        /// <param name="sessionTimeoutDuration">Session timeout duration.</param>
        /// <param name="analyticsEnabled">Analytics enabled.</param>
        /// <param name="loggerLevel">Logger level.</param>
        protected abstract void Configure(int? sessionTimeoutDuration, bool? analyticsEnabled, string loggerLevel);

        /// <summary>
        /// Logs a Firebase event with the given <paramref name="eventName"/> and <paramref name="eventParams"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="eventParams">Event parameters.</param>
        protected abstract void LogEvent(string eventName, Dictionary<string, object> eventParams);

        /// <summary>
        /// Sets the <paramref name="screenName"/> and <paramref name="screenClass"/>.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        /// <param name="screenClass">Screen class.</param>
        protected abstract void SetScreenName(string screenName, string screenClass);

        /// <summary>
        /// Sets a property on the existing User, given by the supplied 
        /// <paramref name="propertyName"/> and <paramref name="propertyValue"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="propertyValue">Property value.</param>
        protected abstract void SetUserProperty(string propertyName, string propertyValue);

        /// <summary>
        /// Sets the current user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected abstract void SetUserId(string userId);

        public void Dispose()
        {
            //do nothing.
        }
    }
}
