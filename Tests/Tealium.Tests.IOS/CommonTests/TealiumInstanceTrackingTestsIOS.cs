using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Tests.Common;

namespace Tealium.Tests.IOS.CommonTests
{
    [TestFixture]
    public class TealiumInstanceTrackingTestsIOS : TealiumInstanceTrackingTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }
    }
}
