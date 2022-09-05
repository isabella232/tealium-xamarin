using System;
using System.Collections.Generic;
using System.Linq;
using CoreFoundation;
using Foundation;
using Tealium.iOS.NativeInterop;
using System.Threading.Tasks;
using Tealium.iOS.NativeInterop.Extensions;
using Tealium.Platform.iOS;

// TODO: add synchronization and dispose checks
namespace Tealium.iOS
{
    /// <summary>
    /// Wrapper for Tealium iOS native object adapted to <see cref="ITealium"/>
    /// cross-platform interface.
    /// </summary>
    public class TealiumIOSImpl : ITealium
    {
        private bool disposedValue;
        private readonly TealiumWrapper nativeTealium;

        public string InstanceId => nativeTealium.InstanceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.iOS.TealiumIOSImpl"/> class.
        /// </summary>
        /// <param name="tealium">Tealium.</param>
        public TealiumIOSImpl(TealiumWrapper tealium)
        {
            nativeTealium = tealium ?? throw new ArgumentNullException(nameof(tealium));
        }

        #region Consent

        private ConsentManager _ConsentManager;
        public ConsentManager ConsentManager
        {
            get
            {
                if (_ConsentManager != null) { return _ConsentManager; }
                if (nativeTealium.ConsentManager != null)
                {
                    _ConsentManager = new ConsentManagerIOS(nativeTealium.ConsentManager);
                    return _ConsentManager;
                }
                return null;
            }
        }

        public List<ConsentManager.ConsentCategory> GetConsentCategories()
        {
            if (disposedValue)
            {
                return ConsentManager.NoCategories.ToList();
            }
            return ConsentManager.UserConsentCategories.ToList();
        }

        public ConsentManager.ConsentStatus GetConsentStatus()
        {
            if (disposedValue)
            {
                return ConsentManager.ConsentStatus.Unknown;
            }

            return ConsentManager.UserConsentStatus;
        }

        public void SetConsentCategories(List<ConsentManager.ConsentCategory> categories)
        {
            if (disposedValue)
            {
                return;
            }

            ConsentManager.UserConsentCategories = categories.ToArray();
        }

        readonly object consentExpirySync = new object();

        KeyedCollection<Action> consentExpiryListeners;

        public CollectionSpecificKey<Action> AddConsentExpiryListener(Action callback)
        {
            if (disposedValue)
            {
                return null;
            }
            CollectionSpecificKey<Action> key;
            lock (consentExpirySync)
            {
                if (consentExpiryListeners == null)
                {
                    consentExpiryListeners = new KeyedCollection<Action>();
                    nativeTealium.OnConsentExpiration = () =>
                    {
                        lock(consentExpirySync)
                        {
                            foreach (var key in consentExpiryListeners.Keys)
                            {
                                Action listener = consentExpiryListeners.Get(key);
                                listener.Invoke();
                            }
                        }
                    };
                }
                key = consentExpiryListeners.Add(callback);
            }
            return key;
        }

        public void RemoveConsentExpiryListener(CollectionSpecificKey<Action> key)
        {
            if (disposedValue)
            {
                return;
            }
            lock (consentExpirySync)
            {
                _ = consentExpiryListeners.Remove(key);
            }
        }

        public void SetConsentStatus(ConsentManager.ConsentStatus status)
        {
            if (disposedValue)
            {
                return;
            }

            ConsentManager.UserConsentStatus = status;
        }

        #endregion Consent

        #region Track

        public void Track(Dispatch dispatch)
        {
            if (disposedValue)
            {
                return;
            }
            NSDictionary<NSString, NSObject> data = NSDictionaryConverter.ConvertBack<NSObject>(dispatch.DataLayer);
            if (dispatch.Type == "view")
            {
                nativeTealium.TrackViewWithTitle(((TealiumView)dispatch).ViewName, data);
            }
            else
            {
                nativeTealium.TrackWithTitle(((TealiumEvent)dispatch).EventName, data);
            }
            
            // TODO: we want to convert bools to string (!)
            
        }

        public string GetVisitorId()
        {
            return nativeTealium.VisitorId;
        }

        public void JoinTrace(string id)
        {
            nativeTealium.JoinTrace(id);
        }

        public void LeaveTrace()
        {
            nativeTealium.LeaveTrace();
        }

        #endregion Track

        #region Remote Commands

        readonly object commandsSync = new object();

        /// <summary>
        ///  Just added for allowing to dispose them in the future.
        ///
        /// Probably this is not even needed as removing them via the Destroy method on the native instance will do pretty much the same.
        /// </summary>
        readonly Dictionary<string, RemoteCommandIOS> commands = new Dictionary<string, RemoteCommandIOS>();

        public void AddRemoteCommand(IRemoteCommand remoteCommand)
        {
            if (disposedValue)
            {
                return;
            }

            lock (commandsSync)
            {
                if (commands.ContainsKey(remoteCommand.CommandId))
                {
                    return;
                }

                RemoteCommandIOS command;
                if (remoteCommand is RemoteCommandIOS wrapperCommand)
                {
                    command = wrapperCommand;
                }
                else
                {
                    command = new RemoteCommandIOS(remoteCommand);
                }
                commands.Add(command.CommandId, command);
                nativeTealium.AddRemoteCommand(command);
            }
        }

        public void RemoveRemoteCommand(string id)
        {
            if (disposedValue)
            {
                return;
            }
            lock (commandsSync)
            {
                if (commands.ContainsKey(id))
                {
                    var command = commands[id];
                    command.Dispose();
                    commands.Remove(id);
                    nativeTealium.RemoveRemoteCommandWithId(id);
                }
            }
        }

        #endregion Remote Commands

        #region Data Layer

        public void AddToDataLayer(IDictionary<string, object> data, Expiry expiry)
        {
            nativeTealium.AddToDataLayerWithData(NSDictionaryConverter.ConvertBack<NSObject>(data), expiry.ToNativeExpiry());
        }

        /// <summary>
        /// Returns Data from the Data Layer.
		///
		/// Due to platform limitations:
		/// In case of List or Dictionaries, they always return as List<object> and Dictionary<string, object>
		/// In case of numbers, int always return as long, float always return as double.
        /// </summary>
        public object GetFromDataLayer(string id)
        {
            var obj = nativeTealium.GetFromDataLayerWithKey(id);
            if (obj == null)
            {
                return null;
            }
            return NSObjectConverter.Convert(obj);
        }

        public void RemoveFromDataLayer(ICollection<string> keys)
        {
            nativeTealium.RemoveFromDataLayerWithKeys(keys.ToArray());
        }

        #endregion Data Layer

        #region Visitor Service

        readonly object visitorServiceSync = new object();

        KeyedCollection<Action<IVisitorProfile>> visitorServiceListeners;

        public CollectionSpecificKey<Action<IVisitorProfile>> AddVisitorServiceListener(Action<IVisitorProfile> callback)
        {
            if (disposedValue)
            {
                return null;
            }
            CollectionSpecificKey<Action<IVisitorProfile>> key;
            lock (visitorServiceSync)
            {
                if (visitorServiceListeners == null)
                {
                    visitorServiceListeners = new KeyedCollection<Action<IVisitorProfile>>();
                    nativeTealium.OnVisitorProfileUpdate = (profile) =>
                    {
                        lock(visitorServiceSync)
                        {
                            foreach (var key in visitorServiceListeners.Keys)
                            {
                                Action<IVisitorProfile> listener = visitorServiceListeners.Get(key);
                                listener.Invoke(new VisitorProfileIOS(profile));
                            }
                        }
                        
                    };
                }
                key = visitorServiceListeners.Add(callback);
            }
            return key;
        }

        public void RemoveVisitorServiceListener(CollectionSpecificKey<Action<IVisitorProfile>> key)
        {
            if (disposedValue)
            {
                return;
            }
            lock (visitorServiceSync)
            {
                _ = visitorServiceListeners.Remove(key);
            }
        }

        #endregion Visitor Service

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lock (visitorServiceSync)
                    {
                        nativeTealium.OnVisitorProfileUpdate = null;
                        visitorServiceListeners?.Clear();
                    }
                    lock (commandsSync)
                    {
                        foreach (var id in commands.Keys)
                        {
                            commands[id].Dispose();
                        }
                        commands.Clear();
                    }
                    lock (consentExpirySync)
                    {
                        nativeTealium.OnConsentExpiration = null;
                        consentExpiryListeners?.Clear();
                    }
                    nativeTealium.Destroy();
                    nativeTealium.Dispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TealiumIOSImpl()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        AnyCollectionKey ITealium.AddVisitorServiceListener(Action<IVisitorProfile> callback)
        {
            return AddVisitorServiceListener(callback);
        }

        void ITealium.RemoveVisitorServiceListener(AnyCollectionKey key)
        {
            RemoveVisitorServiceListener((CollectionSpecificKey<Action<IVisitorProfile>>)key);
        }

        AnyCollectionKey ITealium.AddConsentExpiryListener(Action callback)
        {
            return AddConsentExpiryListener(callback);
        }

        void ITealium.RemoveConsentExpiryListener(AnyCollectionKey key)
        {
            RemoveConsentExpiryListener((CollectionSpecificKey<Action>)key);
        }

        #endregion Dispose
    }
}
