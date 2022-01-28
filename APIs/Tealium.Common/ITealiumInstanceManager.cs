using System;
namespace Tealium
{
    /// <summary>
    /// Manages (creates, holds and disposes) Tealium instances.
    /// </summary>
    public interface ITealiumInstanceManager
    {

        /// <summary>
        /// Creates and configures a new Tealium instance.
        /// </summary>
        /// <returns>Platform-specific <see cref="ITealium"/> instance.</returns>
        /// <param name="config">Tealium configuration with all required and optional parameters.</param>
        ITealium CreateInstance(TealiumConfig config);

        /// <summary>
        /// Creates and configures a new Tealium instance.
        /// </summary>
        /// <returns>Platform-specific <see cref="ITealium"/> instance.</returns>
        /// <param name="config">Tealium configuration with all required and optional parameters.</param>
        ITealium CreateInstance(TealiumConfig config, Action<ITealium> ready);

        /// <summary>
        /// Gets existing Tealium instance.
        /// </summary>
        /// <returns>Existing instance with the given identifier.</returns>
        /// <param name="instanceId">Tealium instance identifier.</param>
        ITealium GetExistingInstance(string instanceId);

        /// <summary>
        /// Gets existing Tealium instance.
        /// </summary>
        /// <returns>Existing instance with the given identifier.</returns>
        /// <param name="account">Tealium instance account.</param>
        /// <param name="profile">Tealium instance profile.</param>
        /// <param name="environment">Tealium instance environment.</param>
        ITealium GetExistingInstance(string account, string profile, Environment environment);

        /// <summary>
        /// Gets all Tealium instances.
        /// </summary>
        /// <returns>All instances in a form of a dictionary.</returns>
        System.Collections.Generic.Dictionary<string, ITealium> GetAllInstances();

        /// <summary>
        /// Disposes a Tealium instace.
        /// </summary>
        /// <returns><c>true</c>, if Tealium instace was disposed, <c>false</c> otherwise.</returns>
        /// <param name="instanceId">Tealium instance identifier.</param>
        bool DisposeInstace(string instanceId);

        /// <summary>
        /// Disposes all Tealium instances.
        /// </summary>
        /// <returns><c>true</c>, if all instances were disposed, <c>false</c> otherwise.</returns>
        bool DisposeAllInstances();
    }
}
