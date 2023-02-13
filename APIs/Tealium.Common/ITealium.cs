using System;
using System.Collections.Generic;
using static Tealium.ConsentManager;

#nullable enable
namespace Tealium
{
    /// <summary>
    /// Allows for controlling a Tealium instance.
    /// Tealium instances created by <see cref="ITealiumInstanceManager"/> must also 
    /// be disposed by it to avoid memory leaks.
    /// </summary>
    public interface ITealium : IDisposable
    {
        /// <summary>
        /// Records an event (screen view/interaction) with the SDK, which will
        /// subsequently gather additional platform data and dispatch to the
        /// configured Dispatchers
        /// </summary>
        /// <param name="dispatch">The dispatch and associated context data for this event</param>
        void Track(Dispatch dispatch);

        /// <summary>
        /// Adds data to the Data Layer, where the Dictionary key will be the
        /// key in the Data Layer and the Dictionary value will be the value
        /// stored at that key.
        /// </summary>
        /// <param name="data">The key value pairs to store in the Data Layer</param>
        /// <param name="expiry">How long to store this data for.</param>
        void AddToDataLayer(IDictionary<string, object> data, Expiry expiry);

        /// <summary>
        /// Gets the value from the Data Layer given the Id.
        /// </summary>
        /// <param name="id">Id of the key to retrieve</param>
        /// <returns></returns>
        object? GetFromDataLayer(string id);

        /// <summary>
        /// Removes all the provided keys from the Data Layer
        /// </summary>
        /// <param name="keys">List of keys to remove</param>
        void RemoveFromDataLayer(ICollection<string> keys);

        /// <summary>
        /// Adds an IRemote command to handle the remote commands
        /// </summary>
        /// <param name="remoteCommand"></param>
        void AddRemoteCommand(IRemoteCommand remoteCommand);

        /// <summary>
        /// Removes a Remote Command with the given id, if there is one.
        /// </summary>
        /// <param name="id">Id of the Remote Command to remove</param>
        void RemoveRemoteCommand(string id);

        /// <summary>
        /// Joins the trace using the given Id
        /// </summary>
        /// <param name="id">Id of the trace to join</param>
        void JoinTrace(string id);

        /// <summary>
        /// Leaves the current trace, if already in one
        /// </summary>
        void LeaveTrace();

        /// <summary>
        /// Gets the cirrent Visitor Id
        /// </summary>
        /// <returns>Visitor Id</returns>
        string? GetVisitorId();

        /// <summary>
        /// Adds a callback to handle the updates to the Visitor Profile
        /// </summary>
        /// <param name="callback">Action to execute when the visitor is updated</param>
        AnyCollectionKey AddVisitorServiceListener(Action<IVisitorProfile> callback);

        /// <summary>
        /// Removes a previously added callback to handle the updates to the Visitor Profile
        /// </summary>
        /// <param name="key">The key returned when adding the listener</param>
        void RemoveVisitorServiceListener(AnyCollectionKey key);

        /// <summary>
        /// Adds a callback to handle the updates to the Visitor ID
        /// </summary>
        /// <param name="callback">Action to execute when the visitor is updated</param>
        public AnyCollectionKey AddVisitorIdListener(Action<string> callback);

        /// <summary>
        /// Removes a previously added callback to handle the updates to the Visitor ID
        /// </summary>
        /// <param name="key">The key returned when adding the listener</param>
        public void RemoveVisitorIdListener(AnyCollectionKey key);

        /// <summary>
        /// Adds a callback to handle expiration of consent.
        /// </summary>
        /// <param name="callback">Action to execute when consent has expired</param>
        AnyCollectionKey AddConsentExpiryListener(Action callback);

        /// <summary>
        /// Removes a previously added a callback to handle expiration of consent.
        /// </summary>
        /// <param name="key">The key returned when adding the listener</param>
        void RemoveConsentExpiryListener(AnyCollectionKey key);


        /// <summary>
        /// Gets the Tealium instance identifier.
        /// </summary>
        string InstanceId { get; }

        /// <summary>
        /// Gets the consent manager.
        /// </summary>
        /// <value>The consent manager.</value>
        ConsentManager ConsentManager { get; }

        /// <summary>
        /// Sets the Consent Status for this instance.
        /// </summary>
        /// <param name="status">Whether the user has consented or not.</param>
        void SetConsentStatus(ConsentStatus status);

        /// <summary>
        /// Gets the current Consent Status
        /// </summary>
        /// <returns></returns>
        ConsentStatus GetConsentStatus();

        /// <summary>
        /// Sets the list of consented categories
        /// </summary>
        /// <param name="categories">The list of categories the user consents to</param>
        void SetConsentCategories(List<ConsentCategory> categories);

        /// <summary>
        /// Gets the current list of consented categories
        /// </summary>
        /// <returns></returns>
        List<ConsentCategory>? GetConsentCategories();

        /// <summary>
        /// Clears the stored visitorIds and resets the current visitorId. Mainly for legal compliance reasons.
        ///
        /// This will also automatically reset the current visitorIds.
        /// Visitor Ids will still get stored in future, as long as the visitorIdentityKey is passed in the config and the dataLayer contains that key.
        ///
        /// - Warning: In order to avoid storing the newly reset visitorId with the current identity right after the storage is cleared, the identity key must be previously deleted from the data layer.
        /// </summary>
        public void ClearStoredVisitorIds();

        /// <summary>
        /// Resets the Tealium Visitor Id
        /// </summary>
        public void ResetVisitorId();
    }
}
