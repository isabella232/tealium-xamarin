using System;
using Foundation;
using Tealium.iOS.NativeInterop;

namespace Tealium.iOS
{
    public class RemoteCommandPayloadIOS : IRemoteCommandPayload
    {
        readonly NSDictionary payload;

        public RemoteCommandPayloadIOS(NSDictionary payload)
        {
            this.payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        public T GetValueForKey<T>(string key)
        {
            var nsObj = payload[key];
            T result = NSObjectConverter.ConvertToTargetType<T>(nsObj);

            return result;
        }

        public bool ContainsKey(string key)
        {
            return payload.ContainsKey(NSObjectConverter.ConvertBack(key));
        }
    }
}
