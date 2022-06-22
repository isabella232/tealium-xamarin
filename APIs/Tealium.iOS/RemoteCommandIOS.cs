using System;
using Tealium;
using Tealium.Platform.iOS;
using Foundation;
namespace Tealium.iOS
{
    public class RemoteCommandIOS : RemoteCommandWrapper, IRemoteCommand
    {
        readonly IRemoteCommand command;

        /**
         * You can pass your own implementation of remote command here
         */
        public RemoteCommandIOS(IRemoteCommand command)
            : this(command.CommandId,
                  command.Description,
                  (command.Url != null) ? new RemoteCommandTypeWrapper(command.Url) : (command.Path != null) ?
                        new RemoteCommandTypeWrapper(command.Path, Foundation.NSBundle.MainBundle) : new RemoteCommandTypeWrapper(),
                  command.Name,
                  command.Version)
        {
            this.command = command;
        }

        /**
         * You can call this method only if the class is inherited and you actually implement the HandleResponse method, otherwise this RemoteCommand will be useless
         */
        public RemoteCommandIOS(string commandId, string description, RemoteCommandTypeWrapper type, string name, string version)
            : base(commandId, description, type, name, version, response =>
           {

           })
        {
            Completion = response =>
            {
                HandleResponse(new RemoteCommandResponseIOS(response, commandId));
            };
        }

        public string Path => Type.Path;

        public string Url => Type.Url;

        public void HandleResponse(IRemoteCommandResponse response)
        {
            command?.HandleResponse(response);
        }

        protected override void Dispose(bool disposing)
        {
            command?.Dispose();
            base.Dispose(disposing);
        }
    }

}


