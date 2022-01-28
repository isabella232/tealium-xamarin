using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Tests.Common;

namespace Tealium.Tests.iOS.CommonTests
{
    [TestFixture]
    public class TealiumInstanceManagerTestsIOS : TealiumInstanceManagerTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }
    }
}
