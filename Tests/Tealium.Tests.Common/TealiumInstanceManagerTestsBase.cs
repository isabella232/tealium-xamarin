using System;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TealiumInstanceManagerTestsBase
    {
        ITealiumInstanceManager instanceManager;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            instanceManager = new TealiumInstanceManager(GetInstanceFactory());
            instanceManager.DisposeAllInstances();
        }

        [SetUp]
        public void TestSetup()
        {
            //to make sure each test starts with no existing Tealium instances
            instanceManager.DisposeAllInstances();
        }

        [TearDown]
        public void TearDown()
        {
            instanceManager.DisposeAllInstances();
        }

        [Test]
        public void CreatesNonNullTealiumInstance()
        {
            ITealium tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());

            Assert.NotNull(tealium);
        }

        [Test]
        public void DisposesSingleTealiumInstance()
        {
            ITealium tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());
            var id = tealium.InstanceId;
            bool disposed = instanceManager.DisposeInstace(tealium.InstanceId);
            var disposedInstance = instanceManager.GetExistingInstance(tealium.InstanceId);
            var allInstances = instanceManager.GetAllInstances();
            
            Assert.True(disposed, "Tealium instance manager did not dispose the instance or returned false in error.");
            Assert.Null(disposedInstance);
            Assert.False(allInstances.ContainsKey(id), "Disposed Tealium instance is still kept by Tealium instance manager.");
        }

        [Test]
        public void DisposesAllTealiumInstances()
        {
            bool instanceAdded = false;
            instanceAdded = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig(1)) != null;
            instanceAdded = instanceAdded && instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig(2)) != null;
            if (!instanceAdded)
            {
                Assert.Inconclusive("Unable toperform test, creating test instances failed!");
            }

            bool disposed = instanceManager.DisposeAllInstances();
            var allInstances = instanceManager.GetAllInstances();

            Assert.True(disposed);
            Assert.AreEqual(0, allInstances.Count);
        }

        [Test]
        public void AddsAndRetrievesFirstTealiumInstance()
        {
            MakeAddTealiumInstanceTest(1);
        }

        [Test]
        public void AddsAndRetrievesSubsequentTealiumInstance()
        {
            bool instanceAdded = false;
            instanceAdded = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig(1)) != null;
            instanceAdded = instanceAdded && instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig(2)) != null;
            if (!instanceAdded)
            {
                Assert.Inconclusive("Unable toperform test, creating test instances failed!");
            }

            MakeAddTealiumInstanceTest(3);
        }

        void MakeAddTealiumInstanceTest(int expectedFinalInstanceCount)
        {
            ITealium createdTealium = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());

            ITealium retreivedTealium = instanceManager.GetExistingInstance(createdTealium.InstanceId);
            var allInstances = instanceManager.GetAllInstances();

            Assert.NotNull(createdTealium);
            Assert.NotNull(retreivedTealium);
            Assert.AreEqual(createdTealium, retreivedTealium);
            Assert.True(allInstances.ContainsKey(createdTealium.InstanceId));
            Assert.AreEqual(expectedFinalInstanceCount, allInstances.Count);
        }

        [Test]
        public void ReturnsNullForNotExistingInstanceId()
        {
            ITealium tealium = instanceManager.GetExistingInstance("NotExistingInstanceId");

            Assert.Null(tealium);
        }

        [Test]
        public void ThrowsIfAttemptToCreateInstanceWithExistingId()
        {
            var config = TealiumConfigHelper.GetSimpleTestConfig();
            bool instanceAdded = false;
            instanceAdded = instanceManager.CreateInstance(config) != null;
            if (!instanceAdded)
            {
                Assert.Inconclusive("Unable toperform test, creating test instances failed!");
            }

            Assert.Throws(typeof(InvalidOperationException),
                          () => instanceManager.CreateInstance(config));
        }

        [Test]
        public void DoesNotThrowWithLogLevel()
        {
            TealiumConfig config1 = TealiumConfigHelper.GetSimpleTestConfig(1);
            config1.LogLevel = null;
            TealiumConfig config2 = TealiumConfigHelper.GetSimpleTestConfig(2);
            config2.LogLevel = LogLevel.Dev;
            TealiumConfig config3 = TealiumConfigHelper.GetSimpleTestConfig(3);
            config3.LogLevel = LogLevel.Qa;
            TealiumConfig config4 = TealiumConfigHelper.GetSimpleTestConfig(4);
            config4.LogLevel = LogLevel.Prod;
            TealiumConfig config5 = TealiumConfigHelper.GetSimpleTestConfig(5);
            config5.LogLevel = LogLevel.Silent;

            instanceManager.CreateInstance(config1);
            instanceManager.CreateInstance(config2);
            instanceManager.CreateInstance(config3);
            instanceManager.CreateInstance(config4);
            instanceManager.CreateInstance(config5);
        }
    }
}
