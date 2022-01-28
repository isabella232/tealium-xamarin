using System;
using Foundation;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Tests.Common;

namespace Tealium.Tests.IOS.CommonTests
{
    [TestFixture]
    public class RemoteCommandResponsePayloadTestsIOS : RemoteCommandPayloadTestsBase
    {
        protected override IRemoteCommandPayload GetPayload(TestPayloadData data)
        {
            NSMutableDictionary dict = new NSMutableDictionary();
            dict.Add(NSObject.FromObject(data.KeyForString), NSObject.FromObject(data.ValueForString));
            dict.Add(NSObject.FromObject(data.KeyForInt), NSObject.FromObject(data.ValueForInt));
            dict.Add(NSObject.FromObject(data.KeyForLong), NSObject.FromObject(data.ValueForLong));
            dict.Add(NSObject.FromObject(data.KeyForFloat), NSObject.FromObject(data.ValueForFloat));
            dict.Add(NSObject.FromObject(data.KeyForDouble), NSObject.FromObject(data.ValueForDouble));
            dict.Add(NSObject.FromObject(data.KeyForBool), NSObject.FromObject(data.ValueForBool));
            return new RemoteCommandPayloadIOS(dict);
        }
    }
}
