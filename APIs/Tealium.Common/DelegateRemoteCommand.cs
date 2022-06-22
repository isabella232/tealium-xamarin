using System;
namespace Tealium
{
    /// <summary>
    /// A generic, cross-platform remote command that allows to handle any 
    /// remote command using a custom delegate.
    /// </summary>
    public class DelegateRemoteCommand : IRemoteCommand
    {
        private readonly string path;
        private readonly string url;

        public string Name { get; }
        public string Version { get; }

        public DelegateRemoteCommand(string commandId, string description, string path = null, string url = null)
        {
            CommandId = commandId ?? throw new ArgumentNullException(nameof(commandId));
            Description = description ?? throw new ArgumentNullException(nameof(description));

            this.path = path;
            this.url = url;
        }

        public string CommandId { get; }

        public string Description { get; }

        public string Path => path;

        public string Url => url;

        /// <summary>
        /// Custom remote command handler delegate. Called internally by <see cref="IRemoteCommand.HandleResponse"/>.
        /// Takes the <see cref="DelegateRemoteCommand"/> parameter to enable the delegate to access some
        /// specific data contained by the instance (it can be an inheritor class).
        /// </summary>
        public Action<DelegateRemoteCommand, IRemoteCommandResponse> HandleResponseDelegate { get; set; }

        public void HandleResponse(IRemoteCommandResponse response)
        {
            HandleResponseDelegate?.Invoke(this, response);
        }

        public void Dispose()
        {
            HandleResponseDelegate = null;
        }
    }
}
