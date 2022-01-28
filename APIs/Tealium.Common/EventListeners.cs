using System;
namespace Tealium
{
    public interface ITealiumEventListener
    {

    }

    /// <summary>
    /// Listens for 'dispatch queued' events.
    /// </summary>
    public interface IDispatchQueuedEventListener : ITealiumEventListener
    {
        /// <summary>
        /// Handles 'dispatch queued' events. Do not manipulate any of its properties 
        /// or payload data here as they will not take effect or persist.
        /// </summary>
        /// <param name="dispatch">Dispatch object.</param>
        void OnDispatchQueued(Dispatch dispatch);
    }

    /// <summary>
    /// Listens for 'dispatch dropped' events.
    /// </summary>
    public interface IDispatchDroppedEventListener : ITealiumEventListener
    {
        /// <summary>
        /// Handles 'dispatch dropped' events. Do not manipulate any of its properties 
        /// or payload data here as they will not take effect or persist.
        /// </summary>
        /// <param name="dispatch">Dispatch object.</param>
        void OnDispatchDropped(Dispatch dispatch);
    }

    /// <summary>
    /// Listens for 'consent expired' events.
    /// </summary>
    public interface IConsentExpiredEventListener : ITealiumEventListener
    {
        /// <summary>
        /// Handles Consent Expiration events.
        /// </summary>
        void OnConsentExpired();
    }

    /// <summary>
    /// Listens for 'dispatch dropped' events.
    /// </summary>
    public interface IVisitorUpdatedEventListener : ITealiumEventListener
    {
        /// <summary>
        /// Handles updates to the VisitorProfile.
        /// </summary>
        /// <param name="visitorProfile">The visitor profile.</param>
        void OnVisitorUpdated(IVisitorProfile visitorProfile);
    }

    /// <summary>
    /// Listens for 'settings published' events.
    /// </summary>
    public interface ISettingsPublishedEventListener : ITealiumEventListener
    {
        /// <summary>
        /// Handles 'settings published' events.
        /// </summary>
        void OnSettingsPublished();
    }
}
