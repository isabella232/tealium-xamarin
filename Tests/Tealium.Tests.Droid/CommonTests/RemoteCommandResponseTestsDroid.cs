using System;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;

namespace Tealium.Tests.Droid.CommonTests
{
    [TestFixture]
    public class RemoteCommandResponseTestsDroid : RemoteCommandResponseTestsBase
    {
        protected override IRemoteCommandResponse GetResponseForParams(string body, int status, string commandId, string responseId,
                                                                       string payloadKey = null, string payloadValue = null)
        {
            Org.Json.JSONObject json = new Org.Json.JSONObject();
            if (payloadKey != null && payloadValue != null)
            {
                json.Put(payloadKey, new Java.Lang.String(payloadValue));
            }
            var nativeResp = new Com.Tealium.Remotecommands.RemoteCommand.Response(null, commandId, responseId, json);
            nativeResp.SetBody(body);
            nativeResp.SetStatus(status);
            return new RemoteCommandResponseDroid(nativeResp);
        }
    }
}
