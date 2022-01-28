using System;
using Tealium.Droid.NativeInterop;

namespace Tealium.Droid
{
    public class RemoteCommandPayloadDroid : IRemoteCommandPayload
    {
        readonly Org.Json.JSONObject payload;

        public RemoteCommandPayloadDroid(Org.Json.JSONObject payload)
        {
            this.payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        public T GetValueForKey<T>(string key)
        {
            var jObject = payload.Opt(key);
            if (typeof(T) == typeof(float))
            {
                //this hack is needed due to JSONObject returns floats as doubles;
                //jObjConv is not aware of that and tries to cast Java.Lang.Double
                //to float - which obviously fails
                object floatObj = (float)(double)jObject;
                return (T)floatObj;
            }
            var obj = JavaToClrConverter.Convert<T>(jObject);
            return obj;
        }

        public bool ContainsKey(string key)
        {
            return payload.Opt(key) != null;
        }
    }
}
