using System;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TealiumInstanceFactoryTestsBase
    {
        ITealiumInstanceFactory tealiumFactory;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            tealiumFactory = GetInstanceFactory();
        }

        [Test]
        public void CreatesNonNullTealiumInstance()
        {
            //NOTE: there must be no native Tealium instances alive at this moment!
            ITealium tealium = tealiumFactory.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());

            Assert.NotNull(tealium);

            tealium.Dispose();
        }
    }
}
