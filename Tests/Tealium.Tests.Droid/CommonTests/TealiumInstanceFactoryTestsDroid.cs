using System;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;

namespace Tealium.Tests.Droid.CommonTests
{
    [TestFixture]
    public class TealiumInstanceFactoryTestsDroid : TealiumInstanceFactoryTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryDroid(MainActivity.CurrentApplication);
        }
    }
}
