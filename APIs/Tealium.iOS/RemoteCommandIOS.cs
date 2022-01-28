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
                        new RemoteCommandTypeWrapper(command.Path, Foundation.NSBundle.MainBundle) : new RemoteCommandTypeWrapper()
                  )
        {

            this.command = command;
        }

        /**
         * You can call this method only if the class is inherited and you actually implement the HandleResponse method, otherwise this RemoteCommand will be useless
         */
        public RemoteCommandIOS(string commandId, string description, RemoteCommandTypeWrapper type)
            : base(commandId, description, type, response =>
           {

           })
        {
            Completion = response =>
            {
                HandleResponse(new RemoteCommandResponseIOS(response, commandId));
            };
        }

        public string Path => command?.Path;

        public string Url => command?.Url;

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


