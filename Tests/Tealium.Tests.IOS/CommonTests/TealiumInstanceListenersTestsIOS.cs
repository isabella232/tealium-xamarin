using NUnit.Framework;
using Tealium.iOS;
using Tealium.Tests.Common;
using System;

namespace Tealium.Tests.Droid.CommonTests
{
    [TestFixture]
    public class TealiumInstanceListenersTestsIOS: TaliumInstanceListenersTestsBase
    {
        public TealiumInstanceListenersTestsIOS()
        {
        }

        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }

        [Test]
        public void TestSpecificConsentExpiryListener()
        {
            TealiumIOSImpl tealIOS = (TealiumIOSImpl)tealium;

            var specificKey = tealIOS.AddConsentExpiryListener(() =>
            {

            });
            Assert.DoesNotThrow(() => tealIOS.RemoveConsentExpiryListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealIOS.RemoveVisitorServiceListener((CollectionSpecificKey<Action<IVisitorProfile>>)(object)specificKey));
        }

        [Test]
        public void TestGenericConsentExpiryListener()
        {
            TealiumIOSImpl tealIOS = (TealiumIOSImpl)tealium;

            var specificKey = tealIOS.AddConsentExpiryListener(() =>
            {

            });
            Assert.DoesNotThrow(() => tealium.RemoveConsentExpiryListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveVisitorServiceListener(specificKey));
        }

        [Test]
        public void TestSpecificVisitorServiceListener()
        {
            TealiumIOSImpl tealIOS = (TealiumIOSImpl)tealium;

            var specificKey = tealIOS.AddVisitorServiceListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealIOS.RemoveVisitorServiceListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealIOS.RemoveConsentExpiryListener((CollectionSpecificKey<Action>)(object)specificKey));
        }

        [Test]
        public void TestGenericVisitorServiceListener()
        {
            TealiumIOSImpl tealIOS = (TealiumIOSImpl)tealium;

            var specificKey = tealIOS.AddVisitorServiceListener((v) =>
            {

            });
            Assert.DoesNotThrow(() => tealium.RemoveVisitorServiceListener(specificKey));
            Assert.Throws(typeof(InvalidCastException), () => tealium.RemoveConsentExpiryListener(specificKey));
        }
    }
}
