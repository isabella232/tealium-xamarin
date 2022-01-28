using System;
using Com.Tealium.Remotecommands;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;

namespace Tealium.Tests.Droid
{
    [TestFixture]
    public class RemoteCommandDroidTests
    {
        [Test]
        public void CallsCommandsHandleResponse()
        {
            TestRemoteCommand testCommand = new TestRemoteCommand("TestCommandId", "Test command.");
            TestableRemoteCommandDroid commandDroid = new TestableRemoteCommandDroid(testCommand);

            commandDroid.CallOnInvoke(new RemoteCommand.Response(null, testCommand.CommandId, "ResponseId", new Org.Json.JSONObject()));

            Assert.IsTrue(testCommand.HandleResponseCalled, $"Command's {nameof(IRemoteCommand.HandleResponse)} was not called.");
            Assert.IsNotNull(testCommand.LastResponse, $"Command's {nameof(IRemoteCommand.HandleResponse)} should be called with non null response.");

            //cleanup
            commandDroid.Dispose();
        }

        class TestableRemoteCommandDroid : RemoteCommandDroid
        {
            IRemoteCommand commandRefCopy;

            public TestableRemoteCommandDroid(IRemoteCommand command)
            : base(command)
            {
                commandRefCopy = command;
            }

            public void CallOnInvoke(RemoteCommand.Response response)
            {
                OnInvoke(response);
            }
        }
    }
}
