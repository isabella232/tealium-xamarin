using System;
namespace Tealium
{
    /// <summary>
    /// Allows to handle a remote with a given ID.
    /// </summary>
    public interface IRemoteCommand : IDisposable
    {
        /// <summary>
        /// Remote command ID.
        /// </summary>
        string CommandId { get; }

        /// <summary>
        /// Remote command description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// An optional path to a local file containing this Remote Command's mappings
        /// </summary>
        string Path { get; }

        /// <summary>
        /// An optional path to a remote file containing this Remote Command's mappings
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Remote command handler. Contains all the business logic.
        /// Called internally by a platform-specific <see cref="ITealium"/> instance.
        /// </summary>
        /// <param name="response">Response object containing all necessary data to handle the command.</param>
        void HandleResponse(IRemoteCommandResponse response);

        /// <summary>
        /// Remote command name used for tracking purposes.
        /// </summary>
        string Name { get;  }

        /// <summary>
        /// Remote command version used for tracking purposes.
        /// </summary>
        string Version { get; }
    }
}
