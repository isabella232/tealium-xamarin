using System;
namespace Tealium
{
    /// <summary>
    /// Allows access to remote command's response.
    /// </summary>
    public interface IRemoteCommandResponse
    {
        /// <summary>
        /// Gets or sets the remote command response body.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets the remote command response status.
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Parent command identifier.
        /// </summary>
        string CommandId { get; }

        /// <summary>
        /// Current response identifier.
        /// </summary>
        string ResponseId { get; }

        /// <summary>
        /// The payload data of the command.
        /// </summary>
        IRemoteCommandPayload Payload { get; }
    }
}
