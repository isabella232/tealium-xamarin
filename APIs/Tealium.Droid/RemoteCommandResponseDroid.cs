using System;
using Com.Tealium.Remotecommands;

namespace Tealium.Droid
{
    public class RemoteCommandResponseDroid : IRemoteCommandResponse
    {
        RemoteCommand.Response nativeResponse;

        public RemoteCommandResponseDroid(RemoteCommand.Response nativeResponse)
        {
            this.nativeResponse = nativeResponse ?? throw new ArgumentNullException(nameof(nativeResponse));
            Payload = new RemoteCommandPayloadDroid(nativeResponse.RequestPayload);
        }

        public string Body
        {
            get
            {
                return nativeResponse.Body;
            }
            set
            {
                nativeResponse.SetBody(value);
            }
        }

        public int Status
        {
            get
            {
                return nativeResponse.Status;
            }
            set
            {
                nativeResponse.SetStatus(value);
            }
        }

        public string CommandId => nativeResponse.CommandId;

        public string ResponseId => nativeResponse.Id;

        public IRemoteCommandPayload Payload { get; }
    }
}
