using System;
using Com.Tealium.Dispatcher;
using Com.Tealium.Core.Messaging;
using Com.Tealium.Core.Settings;
using Com.Tealium.Core.Consent;
using Tealium.Droid.NativeInterop.Extensions;
using Com.Tealium.Visitorservice;

namespace Tealium.Droid
{
    #region Dispatch validators

    /// <summary>
    /// Native Dispatch Validator implementation - delegates to the provided CLR
    /// implementation.
    /// </summary>
    public class NativeDispatchValidator : Java.Lang.Object, Com.Tealium.Core.Validation.IDispatchValidator
    {
        private readonly IDispatchValidator validator;

        public bool Enabled { get; set; } = true;

        public string Name { get => validator.Name ?? $"CustomValidator_{Guid.NewGuid()}"; }

        public NativeDispatchValidator(IDispatchValidator validator)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        bool Com.Tealium.Core.Validation.IDispatchValidator.ShouldQueue(IDispatch dispatch)
        {
            if (validator != null && dispatch != null)
            {
                return validator.ShouldQueue(new DispatchDroid(dispatch));
            }
            else
            {
                return false;
            }
        }

        bool Com.Tealium.Core.Validation.IDispatchValidator.ShouldDrop(IDispatch dispatch)
        {
            if (validator != null && dispatch != null)
            {
                return validator.ShouldDrop(new DispatchDroid(dispatch));
            }
            else
            {
                return false;
            }
        }
    }

    #endregion Dispatch validators

    #region Event listeners

    /// <summary>
    /// Native consent expiry listener implementation.
    /// </summary>
    public class NativeConsentExpiryListener : Java.Lang.Object, IUserConsentPreferencesUpdatedListener
    {
        private readonly Action Action;

        public NativeConsentExpiryListener(Action action)
        {
            Action = action;
        }

        public void OnUserConsentPreferencesUpdated(UserConsentPreferences userConsentPreferences, IConsentManagementPolicy policy)
        {
            if (userConsentPreferences.ConsentStatus.ToStatus() == ConsentManager.ConsentStatus.Unknown)
            {
                Action.Invoke();
            }
        }
    }

    /// <summary>
    /// Native visitor service listener implementation.
    /// </summary>
    public class NativeVisitorServiceListener : Java.Lang.Object, IVisitorUpdatedListener
    {
        private readonly Action<IVisitorProfile> Action;

        public NativeVisitorServiceListener(Action<IVisitorProfile> action)
        {
            Action = action;
        }

        public void OnVisitorUpdated(VisitorProfile visitorProfile)
        {
            if (visitorProfile != null)
            {
                Action.Invoke(new VisitorProfileDroid(visitorProfile));
            }
        }
    }

    /// <summary>
    /// Native visitor id listener implementation.
    /// </summary>
    public class NativeVisitorIdListener : Java.Lang.Object, IVisitorIdUpdatedListener
    {
        private readonly Action<string> Action;

        public NativeVisitorIdListener(Action<string> action)
        {
            Action = action;
        }

        public void OnVisitorIdUpdated(string visitorId)
        {
            if (visitorId != null)
            {
                Action.Invoke(visitorId);
            }
        }
    }

    /// <summary>
    /// Native Event Listener implementation that implements all supported
    /// events to support a provided event listener implementing multiple
    /// different events.
    /// </summary>
    public class NativeEventListener : Java.Lang.Object, ILibrarySettingsUpdatedListener, IDispatchDroppedListener, IDispatchQueuedListener, IConsentExpiredEventListener, IVisitorUpdatedListener
    {
        private readonly ITealiumEventListener listener;

        public NativeEventListener(ITealiumEventListener listener)
        {
            this.listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }

        public void OnConsentExpired()
        {
            if (listener is IConsentExpiredEventListener)
            {
                ((IConsentExpiredEventListener)listener).OnConsentExpired();
            }
        }

        public void OnDispatchDropped(IDispatch dispatch)
        {
            if (listener is IDispatchDroppedEventListener)
            {
                ((IDispatchDroppedEventListener)listener).OnDispatchDropped(new DispatchDroid(dispatch));
            };
        }

        public void OnDispatchQueued(IDispatch dispatch)
        {
            if (listener is IDispatchQueuedEventListener)
            {
                ((IDispatchQueuedEventListener)listener).OnDispatchQueued(new DispatchDroid(dispatch));
            };
        }

        public void OnLibrarySettingsUpdated(LibrarySettings settings)
        {
            if (listener is ISettingsPublishedEventListener)
            {
                ((ISettingsPublishedEventListener)listener).OnSettingsPublished();
            }
        }

        public void OnVisitorUpdated(VisitorProfile visitorProfile)
        {
            if (listener is IVisitorUpdatedEventListener)
            {
                ((IVisitorUpdatedEventListener)listener).OnVisitorUpdated(new VisitorProfileDroid(visitorProfile));
            }
        }
    }

    #endregion Event listeners
}
