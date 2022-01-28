using System;
using Tealium.Platform.iOS;

namespace Tealium.iOS
{
    public class RemoteCommandResponseIOS : IRemoteCommandResponse
    {
        readonly RemoteCommandResponseWrapper response;
        readonly string commandId;
        readonly string responseId;
        public RemoteCommandResponseIOS(RemoteCommandResponseWrapper response, string commandId)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
            this.commandId = commandId;
            responseId = Guid.NewGuid().ToString();
            Payload = new RemoteCommandPayloadIOS(response.Payload);
        }

        public string Body 
        { 
            get => response.Data.ToString();
            set => response.Data = Foundation.NSData.FromString(value);
        }

        public int Status 
        {
            get => response.Status.Int32Value;
            set => response.Status = value;
        }

        public string CommandId => commandId;

        public string ResponseId => responseId;

        public IRemoteCommandPayload Payload { get; }
    }
}
