using System;
using NUnit.Framework;
namespace Tealium.Tests.Common
{
    [TestFixture()]
    public abstract class TaliumInstanceListenersTestsBase
    {

        readonly ITealiumInstanceManager instanceManager;
        protected static ITealium tealium;

        public TaliumInstanceListenersTestsBase()
        {
            if (tealium == null)
            {
                instanceManager = new TealiumInstanceManager(GetInstanceFactory());
                tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetTestConfigWithConsentManagerEnabled());
            }
        }


        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            // Reset everything to start from scratch.
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
        }


        [Test]
        public void TestConsentExpiryListeners()
        {
            var key = tealium.AddConsentExpiryListener(() =>
            {

            });

            Assert.DoesNotThrow(() => tealium.RemoveConsentExpiryListener(key));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveVisitorServiceListener(key));
        }

        [Test]
        public void TestVisitorServiceListeners()
        {
            var key = tealium.AddVisitorServiceListener((v) =>
            {

            });

            Assert.DoesNotThrow(() => tealium.RemoveVisitorServiceListener(key));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveConsentExpiryListener(key));
        }

        [Test]
        public void TestVisitorIdListeners()
        {
            var key = tealium.AddVisitorIdListener((v) =>
            {

            });

            Assert.DoesNotThrow(() => tealium.RemoveVisitorIdListener(key));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveConsentExpiryListener(key));
        }
    }
}