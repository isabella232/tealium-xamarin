using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Tests.Common;

namespace Tealium.Tests.IOS.CommonTests
{
    [TestFixture]
    public class TealiumInstanceFactoryTestsIOS : TealiumInstanceFactoryTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }
    }
}
