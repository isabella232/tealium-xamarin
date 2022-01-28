using System;
using Tealium.iOS;
using Tealium.Tests.Common;
using NUnit.Framework;

namespace Tealium.Tests.IOS.CommonTests
{
    public class TealiumInstanceConsentManagerTestsIOS : TealiumInstanceConsentManagerTestsBase
    {
        protected override ITealiumInstanceFactory GetInstanceFactory()
        {
            return new TealiumInstanceFactoryIOS();
        }
    }
}
