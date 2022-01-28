using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TealiumInstanceRemoteCommandsTestsBase
    {
        ITealiumInstanceManager instanceManager;
        ITealium tealium;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            instanceManager = new TealiumInstanceManager(GetInstanceFactory());
            tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            instanceManager.DisposeAllInstances();
        }

        [Test]
        public void AddsRemoteCommand()
        {
            string cmdId = $"TestCommand_{nameof(AddsRemoteCommand)}";
            TestRemoteCommand cmd = new TestRemoteCommand(cmdId, $"This is a test command for {nameof(AddsRemoteCommand)}.");

            tealium.AddRemoteCommand(cmd);
        }

        [Test]
        public void RemovesAndDisposesRemoteCommand()
        {
            string cmdId = $"TestCommand_{nameof(RemovesAndDisposesRemoteCommand)}";
            TestRemoteCommand cmd = new TestRemoteCommand(cmdId, $"This is a test command for {nameof(RemovesAndDisposesRemoteCommand)}.");
            System.Threading.Tasks.Task.Delay(1500).Wait();
            tealium.AddRemoteCommand(cmd);

            tealium.RemoveRemoteCommand(cmdId);

            Assert.True(cmd.Disposed, "The removed remote command was not disposed after removal.");
        }

        [Test]
        public void DisposesCommandsWhenIsDisposed()
        {
            var config = TealiumConfigHelper.GetSimpleTestConfig(1);
            ITealium disposeTestTealium = instanceManager.CreateInstance(config);
            string cmdId = $"TestCommand_{nameof(DisposesCommandsWhenIsDisposed)}";
            TestRemoteCommand cmd = new TestRemoteCommand(cmdId, $"This is a test command for {nameof(DisposesCommandsWhenIsDisposed)}.");
            System.Threading.Tasks.Task.Delay(1500).Wait();

            disposeTestTealium.AddRemoteCommand(cmd);

            bool didDisposeTealium = instanceManager.DisposeInstace(config.InstanceId);
            if (!didDisposeTealium)
            {
                Assert.Inconclusive("Unable to dispose tealium instance, testing of disposing remote commands is impossible.");
            }

            Assert.True(cmd.Disposed, "Test command should be disposed while disposing Tealium instance.");
        }
    }
}
