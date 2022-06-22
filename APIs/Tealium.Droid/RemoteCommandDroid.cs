using System;
using Com.Tealium.Remotecommands;


namespace Tealium.Droid
{
    /// <summary>
    /// Native RemoteCommand implementation, delegating to the provided CLR
    /// implementation.
    /// </summary>
    public class RemoteCommandDroid : RemoteCommand
    {
        readonly IRemoteCommand command;

        public RemoteCommandDroid(IRemoteCommand command)
            : base(command.CommandId, command.Description, command.Version)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override void OnInvoke(Response response)
        {
            command.HandleResponse(new RemoteCommandResponseDroid(response));
            response.Send();
        }
    }
}
