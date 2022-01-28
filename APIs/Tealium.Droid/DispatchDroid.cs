using System;
using System.Collections.Generic;
using Tealium.Droid.NativeInterop;

namespace Tealium.Droid
{
    public class DispatchDroid : Dispatch
    {
        readonly Com.Tealium.Dispatcher.IDispatch dispatch;

        public DispatchDroid(Com.Tealium.Dispatcher.IDispatch dispatch)
        {
            this.dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
            DataLayer = JavaDictionaryToClrDictionaryConverter.Convert(dispatch.Payload());
            Type = DataLayer["tealium_event_type"] as string ?? "";
        }

        public bool ContainsKey(string key)
        {
            return dispatch.Payload().ContainsKey(key);
        }

        public List<string> GetAllKeys()
        {
            return new List<string>(dispatch.Payload().Keys);
        }
    }
}
