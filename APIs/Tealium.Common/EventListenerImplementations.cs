using System;

namespace Tealium
{
    /// <summary>
    /// Listens for 'dispatch queued' events. Provides a delegate method to
    /// handle the events.
    /// </summary>
    public class DispatchDroppedDelegateEventListener : IDispatchDroppedEventListener
    {
        public void OnDispatchDropped(Dispatch dispatch)
        {
            DispatchDropped?.Invoke(dispatch);
        }

        /// <summary>
        /// Delegate method for handling the event.
        /// </summary>
        public Action<Dispatch> DispatchDropped { get; set; }
    }

    /// <summary>
    /// Listens for 'dispatch queued' events. Provides a delegate method to
    /// handle the events.
    /// </summary>
    public class DispatchQueuedDelegateEventListener : IDispatchQueuedEventListener
    {
        public void OnDispatchQueued(Dispatch dispatch)
        {
            DispatchQueued?.Invoke(dispatch);
        }

        /// <summary>
        /// Delegate method for handling the event.
        /// </summary>
        public Action<Dispatch> DispatchQueued { get; set; }
    }


    /// <summary>
    /// Listens for 'settings published' events. Provides a delegate method to
    /// handle the events.
    /// </summary>
    public class SettingsPublishedDelegateEventListener : ISettingsPublishedEventListener
    {
        public void OnSettingsPublished()
        {
            SettingsPublished?.Invoke();
        }

        /// <summary>
        /// Delegate method for handling the event.
        /// </summary>
        public Action SettingsPublished { get; set; }
    }

    /// <summary>
    /// Listens for 'consent expired' events. Provides a delegate method to
    /// handle the events.
    /// </summary>
    public class ConsentExpiredDelegateEventListener : IConsentExpiredEventListener
    {
        public void OnConsentExpired()
        {
            ConsentExpired?.Invoke();
        }

        /// <summary>
        /// Delegate method for handling the event.
        /// </summary>
        public Action ConsentExpired { get; set; }
    }

    /// <summary>
    /// Listens for 'visitor updated' events. Provides a delegate method to
    /// handle the events.
    /// </summary>
    public class VisitorUpdatedDelegateEventListener : IVisitorUpdatedEventListener
    {
        public void OnVisitorUpdated(IVisitorProfile visitorProfile)
        {
            VisitorUpdated?.Invoke(visitorProfile);
        }

        /// <summary>
        /// Delegate method for handling the event.
        /// </summary>
        public Action<IVisitorProfile> VisitorUpdated { get; set; }
    }
}
