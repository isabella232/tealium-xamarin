using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Platform.iOS;


namespace Tealium.Tests.iOS
{

    public class SomeRemoteCommandIOSSubclass: RemoteCommandIOS
    {
        public override string Version => "someVersion";
        public override string Name => "someName";
        public SomeRemoteCommandIOSSubclass(IRemoteCommand command)
            :base(command)
        {

        }
    }

    [TestFixture]
    public class RemoteCommandIOSTests
    {
        [Test]
        public void DefaultNameAndVersion()
        {
            var command = new RemoteCommandIOS("id", null, new RemoteCommandTypeWrapper(), null, null);
            Assert.AreEqual(command.Name, "id");
            Assert.NotNull(command.Version);
            var wrapper = new RemoteCommandWrapper("id", null, new RemoteCommandTypeWrapper(), response => { });
            Assert.AreEqual(command.Version, wrapper.Version);
        }

        [Test]
        public void NameAndVersion()
        {
            var command = new SomeRemoteCommandIOSSubclass(new RemoteCommandIOS("id", null, new RemoteCommandTypeWrapper(), null, null));
            Assert.AreEqual(command.Name, "someName");
            Assert.AreEqual(command.Version, "someVersion");
        }
    }
}
