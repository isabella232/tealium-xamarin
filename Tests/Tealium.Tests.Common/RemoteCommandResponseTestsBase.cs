using System;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class RemoteCommandResponseTestsBase
    {
        //NOTE: this method assumes that payload won't be null!
        protected abstract IRemoteCommandResponse GetResponseForParams(string body, int status, string commandId,
                                                                       string responseId, string payloadKey = null, string payloadValue = null);

        [Test]
        public void GetsResponseBody()
        {
            string testBody = "InitialBody";
            IRemoteCommandResponse resp = GetResponseForParams(testBody, 0, "cmdId", "respId");

            Assert.AreEqual(testBody, resp.Body);
        }

        [Test]
        public void SetsResponseBody()
        {
            string testBody = "TestBody";
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", 0, "cmdId", "respId");

            resp.Body = testBody;

            Assert.AreEqual(testBody, resp.Body);
        }

        [Test]
        public void GetsResponseStatus()
        {
            int testStatus = 3;
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", testStatus, "cmdId", "respId");

            Assert.AreEqual(testStatus, resp.Status);
        }

        [Test]
        public void SetsResponseStatus()
        {
            int testStatus = 410;
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", 0, "cmdId", "respId");

            resp.Status = testStatus;

            Assert.AreEqual(testStatus, resp.Status);
        }

        [Test]
        public void GetsResponseCommandId()
        {
            string testCmdId = "testCmdId";
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", 0, testCmdId, "respId");

            Assert.AreEqual(testCmdId, resp.CommandId);
        }


        [Test]
        public void GetsResponseResponseId()
        {
            string testResposneId = "testResposneId";
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", 0, "cmdId", testResposneId);

            Assert.AreEqual(testResposneId, resp.ResponseId);
        }

        [Test]
        public void GetsResponsePayload()
        {
            IRemoteCommandResponse resp = GetResponseForParams("InitialBody", 0, "cmdId", "respId", "key", "value");

            Assert.NotNull(resp.Payload);
        }
    }
}
