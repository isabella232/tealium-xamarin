using System;
namespace Tealium
{
    /// <summary>
    /// Platform-specific <see cref="ITealium"/> instance factory.
    /// </summary>
    public interface ITealiumInstanceFactory
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
        /// <param name="ready">Ready Callback for when the Tealium instance has finished initializing</param>
        ITealium CreateInstance(TealiumConfig config, Action<ITealium> ready);
    }
}
