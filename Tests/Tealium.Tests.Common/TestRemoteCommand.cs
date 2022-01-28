using System;
namespace Tealium.Tests.Common
{
    public class TestRemoteCommand : IRemoteCommand
    {
        public TestRemoteCommand()
        {

        }

        public TestRemoteCommand(string cmdId, string desc)
        {
            CommandId = cmdId;
            Description = desc;
        }

        public string CommandId { get; set; }

        public string Description { get; set; }

        public bool Disposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool HandleResponseCalled { get; private set; }
        public IRemoteCommandResponse LastResponse { get; private set; }

        public string Path => null;

        public string Url => null;

        public void HandleResponse(IRemoteCommandResponse response)
        {
            HandleResponseCalled = true;
            LastResponse = response;
        }
    }
}
