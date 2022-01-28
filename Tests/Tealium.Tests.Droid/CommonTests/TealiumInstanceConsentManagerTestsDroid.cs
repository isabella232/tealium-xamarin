using System;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;

namespace Tealium.Tests.Droid.Assets
{
    [TestFixture]
    public class TealiumInstanceConsentManagerTestsDroid : TealiumInstanceConsentManagerTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryDroid(MainActivity.CurrentApplication);
        }
    }
}
