using System;
using Tealium.Droid;
using Tealium.Tests.Common;

namespace Tealium.Tests.Droid.CommonTests
{
    public class RemoteCommandResponsePayloadTestsDroid : RemoteCommandPayloadTestsBase
    {
        protected override IRemoteCommandPayload GetPayload(TestPayloadData data)
        {
            Org.Json.JSONObject json = new Org.Json.JSONObject();
            json.Put(data.KeyForString, data.ValueForString);
            json.Put(data.KeyForInt, data.ValueForInt);
            json.Put(data.KeyForLong, data.ValueForLong);
            json.Put(data.KeyForFloat, data.ValueForFloat);
            json.Put(data.KeyForDouble, data.ValueForDouble);
            json.Put(data.KeyForBool, data.ValueForBool);
            return new RemoteCommandPayloadDroid(json);
        }
    }

}
