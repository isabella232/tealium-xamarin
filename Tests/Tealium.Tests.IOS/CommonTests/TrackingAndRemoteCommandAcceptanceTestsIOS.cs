using System;
using NUnit.Framework;
using Tealium.Tests.Common;
using Tealium.iOS;

namespace Tealium.Tests.IOS.CommonTests
{
    [TestFixture]
    public class TrackingAndRemoteCommandAcceptanceTestsIOS : TrackingAndRemoteCommandAcceptanceTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }
    }
}
