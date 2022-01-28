using System;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TrackingAndRemoteCommandAcceptanceTestsBase
    {
        static readonly string REMOTE_COMMAND_TRIGGER_EVENT = "launch";
        static readonly string REMOTE_COMMAND_ID = "firebaseanalytics";

        const int TEST_TIMEOUT_SECONDS = 25;

        ITealiumInstanceManager tealiumInstanceMan;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            tealiumInstanceMan = new TealiumInstanceManager(GetInstanceFactory());
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            tealiumInstanceMan.DisposeAllInstances();
        }

        [SetUp]
        public void PrepareForTest()
        {
            tealiumInstanceMan.DisposeAllInstances();
        }

        string MappingsPath()
        {
#if __IOS__
            string path = "Assets/mappings";
#else
            string path = "mappings.json";
#endif
            return path;
        }

        /// <summary>
        /// Tests whether a remote command is invoked after a given TrackEvent.
        /// The command is defined in Tealium configuration.
        /// </summary>
        [Test]
        public void HandlesRemoteCommandAfterTrackEvent1()
        {
            bool remoteCommandReceived = false;

            DelegateRemoteCommand cmd = new DelegateRemoteCommand(REMOTE_COMMAND_ID, $"Test remoteCommand for {nameof(HandlesRemoteCommandAfterTrackEvent1)}", path: MappingsPath());
            cmd.HandleResponseDelegate += (DelegateRemoteCommand arg1, IRemoteCommandResponse arg2) =>
            {
                if (arg1.CommandId == REMOTE_COMMAND_ID)
                {
                    remoteCommandReceived = true;
                }
            };
            ITealium tealium = tealiumInstanceMan.CreateInstance(TealiumConfigHelper.GetTestConfigWithRemoteCommand(cmd));
            System.Threading.Tasks.Task.Delay(10000).Wait();
            tealium.Track(new TealiumEvent(REMOTE_COMMAND_TRIGGER_EVENT));

            int currentSecondsPassed = 0;
            while (currentSecondsPassed < TEST_TIMEOUT_SECONDS && remoteCommandReceived == false)
            {
                System.Threading.Tasks.Task.Delay(1000).Wait();
                currentSecondsPassed++;
            }

            Assert.True(remoteCommandReceived, $"Did not receive remote command from Tealium within {TEST_TIMEOUT_SECONDS}s period of time.");
        }

        /// <summary>
        /// Tests whether a remote command is invoked after a given TrackEvent.
        /// The command is added to existing Tealium instance.
        /// </summary>
        [Test]
        public void HandlesRemoteCommandAfterTrackEvent2()
        {
            bool remoteCommandReceived = false;
            DelegateRemoteCommand cmd = new DelegateRemoteCommand(REMOTE_COMMAND_ID, $"Test remoteCommand for {nameof(HandlesRemoteCommandAfterTrackEvent2)}", path: MappingsPath());
            cmd.HandleResponseDelegate += (DelegateRemoteCommand arg1, IRemoteCommandResponse arg2) =>
            {
                if (arg1.CommandId == REMOTE_COMMAND_ID)
                {
                    remoteCommandReceived = true;
                }
            };
            ITealium tealium = tealiumInstanceMan.CreateInstance(TealiumConfigHelper.GetTestConfigWithRemoteCommandEnabled(), (teal) =>
            {
                teal.AddRemoteCommand(cmd);
            });

            System.Threading.Tasks.Task.Delay(10000).Wait();

            tealium.Track(new TealiumEvent(REMOTE_COMMAND_TRIGGER_EVENT));
            int currentSecondsPassed = 0;
            while (currentSecondsPassed < TEST_TIMEOUT_SECONDS && remoteCommandReceived == false)
            {
                System.Threading.Tasks.Task.Delay(1000).Wait();
                currentSecondsPassed++;
            }

            Assert.True(remoteCommandReceived, $"Did not receive remote command from Tealium within {TEST_TIMEOUT_SECONDS}s period of time.");
        }
    }
}
