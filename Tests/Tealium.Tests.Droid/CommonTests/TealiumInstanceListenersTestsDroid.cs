using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;
using System;

namespace Tealium.Tests.Droid.CommonTests
{
    [TestFixture]
    public class TealiumInstanceListenersTestsDroid: TaliumInstanceListenersTestsBase
    {
        public TealiumInstanceListenersTestsDroid()
        {
        }

        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryDroid(MainActivity.CurrentApplication);
        }

        [Test]
        public void TestSpecificConsentExpiryListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddConsentExpiryListener(() =>
            {

            });
            Assert.DoesNotThrow(() => tealDroid.RemoveConsentExpiryListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealDroid.RemoveVisitorServiceListener((CollectionSpecificKey<NativeVisitorServiceListener>)(object)specificKey));
        }

        [Test]
        public void TestGenericConsentExpiryListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddConsentExpiryListener(() =>
            {

            });
            Assert.DoesNotThrow(() => tealium.RemoveConsentExpiryListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveVisitorServiceListener(specificKey));
        }

        [Test]
        public void TestSpecificVisitorServiceListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddVisitorServiceListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealDroid.RemoveVisitorServiceListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealDroid.RemoveConsentExpiryListener((CollectionSpecificKey<NativeConsentExpiryListener>)(object)specificKey));
        }

        [Test]
        public void TestGenericVisitorServiceListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddVisitorServiceListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealium.RemoveVisitorServiceListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveConsentExpiryListener(specificKey));
        }

        [Test]
        public void TestSpecificVisitorIdListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddVisitorIdListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealDroid.RemoveVisitorIdListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealDroid.RemoveConsentExpiryListener((CollectionSpecificKey<NativeConsentExpiryListener>)(object)specificKey));
        }

        [Test]
        public void TestGenericVisitorIdListener()
        {
            TealiumDroidImpl tealDroid = (TealiumDroidImpl)tealium;

            var specificKey = tealDroid.AddVisitorIdListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealium.RemoveVisitorIdListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveConsentExpiryListener(specificKey));
        }
    }
}
