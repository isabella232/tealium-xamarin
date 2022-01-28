using System;
namespace Tealium
{
    /// <summary>
    /// Allows to access data from a remote command response payload.
    /// </summary>
    public interface IRemoteCommandPayload
    {
        /// <summary>
        /// Gets a value for a given key from the payload.
        /// </summary>
        /// <returns>The value cast to desired type.</returns>
        /// <param name="key">Key accompanying the value.</param>
        /// <typeparam name="T">Target type parameter to cast the value to.</typeparam>
        T GetValueForKey<T>(string key);

        /// <summary>
        /// Tells whether this payload contains data with the given key.
        /// </summary>
        /// <returns><c>true</c>, if the key is present, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        bool ContainsKey(string key);
    }
}
