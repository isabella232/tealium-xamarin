using System;
using Native = Com.Tealium.Dispatcher;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;
using System.Collections.Generic;
using Tealium.Droid.NativeInterop;

namespace Tealium.Tests.Droid.CommonTests
{
    [TestFixture]
    public class DispatchTestsDroid : DispatchTestsBase
    {
        protected override Dispatch GetDispatchWithData(string stringKey, string stringValue)
        {
            var dispatch = new Native.TealiumEvent("eventName", new Dictionary<string, Java.Lang.Object>()
            {
                { stringKey, stringValue }
            });

            return new DispatchDroid(dispatch);
        }

        protected override Dispatch GetDispatchWithData(string arrayKey, string[] array)
        {
            var dispatch = new Native.TealiumEvent("eventName", new Dictionary<string, Java.Lang.Object>()
            {
                { arrayKey, array }
            });

            return new DispatchDroid(dispatch);
        }

        protected override Dispatch GetDispatchWithData(string dictionaryKey, IDictionary<string, object> dictionary)
        {
            var dispatch = new Native.TealiumEvent("eventName", new Dictionary<string, Java.Lang.Object>()
            {
                { dictionaryKey, new Java.Util.HashMap(JavaDictionaryToClrDictionaryConverter.ConvertBack(dictionary)) }
            });

            return new DispatchDroid(dispatch);
        }

        protected override Dispatch GetDispatchWithData(KeyValuePair<string, object> data)
        {
            var dispatch = new Native.TealiumEvent("eventName", new Dictionary<string, Java.Lang.Object>()
            {
                { data.Key, JavaToClrConverter.ConvertBack(data.Value) }
            });

            return new DispatchDroid(dispatch);
        }

        protected override Dispatch GetEmptyDispatch()
        {
            return new DispatchDroid(new Native.TealiumEvent("eventName"));
        }


    }
}
