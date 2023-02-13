using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tealium.Droid.NativeInterop;
using Tealium.Droid.NativeInterop.Extensions;

namespace Tealium.Droid
{
    /// <summary>
    /// Wrapper for Tealium Android native object adapted to <see cref="ITealium"/>
    /// cross-platform interface.
    /// </summary>
    public class TealiumDroidImpl : ITealium
    {
        readonly Com.Tealium.Core.Tealium tealium;
        readonly ConsentManager consentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.Droid.TealiumDroidImpl"/> class.
        /// </summary>
        /// <param name="tealium">Tealium.</param>
        /// <param name="instanceId">Instance identifier.</param>
        public TealiumDroidImpl(Com.Tealium.Core.Tealium tealium, string instanceId)
        {
            this.tealium = tealium ?? throw new ArgumentNullException(nameof(tealium));
            InstanceId = instanceId ?? throw new ArgumentNullException(nameof(instanceId));

            consentManager = new ConsentManagerDroid(tealium);
        }

        bool disposed = false;

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;

            try
            {
                Com.Tealium.Core.Tealium.CompanionInstance.Destroy(InstanceId);
                tealium.Dispose();
                foreach (var cmd in commandsDroid.Keys)
                {
                    commandsDroid[cmd].Dispose();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error while disposing Tealium instance {InstanceId}!\n{e.Message}");
            }

            if (commands != null && commands.Count > 0)
            {
                lock (commandsSync)
                {
                    foreach (var key in commands.Keys)
                    {
                        try
                        {
                            commands[key].Dispose();
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error while disposing managed remote command with id {key}.\n{e.Message}");
                        }
                    }
                }
                commands = null;
            }

            if (commandsDroid != null && commandsDroid.Count > 0)
            {
                lock (commandsSync)
                {
                    foreach (var key in commandsDroid.Keys)
                    {
                        try
                        {
                            commandsDroid[key].Dispose();
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error while disposing native remote command with id {key}.\n{e.Message}");
                        }
                    }
                }
                commandsDroid = null;
            }

            lock (consentListenersSync)
            {
                foreach (var key in consentExpiryListeners.Keys)
                {
                    var listener = consentExpiryListeners.Get(key);
                    _ = consentExpiryListeners.Remove(key);
                    tealium.Events.Unsubscribe(listener);
                }
            }

            lock (visitorServiceListenersSync)
            {
                foreach (var key in visitorServiceListeners.Keys)
                {
                    var listener = visitorServiceListeners.Get(key);
                    _ = visitorServiceListeners.Remove(key);
                    tealium.Events.Unsubscribe(listener);
                }
            }
        }

        public string InstanceId { get; private set; }

        #region Tracking

        public void Track(Dispatch dispatch)
        {
            Com.Tealium.Dispatcher.IDispatch nativeDispatch = null;
            if (dispatch is TealiumView)
            {
                nativeDispatch = new Com.Tealium.Dispatcher.TealiumView(((TealiumView)dispatch).ViewName, JavaDictionaryToClrDictionaryConverter.ConvertBack(dispatch.DataLayer));
            }
            else if (dispatch is TealiumEvent)
            {
                nativeDispatch = new Com.Tealium.Dispatcher.TealiumEvent(((TealiumEvent)dispatch).EventName, JavaDictionaryToClrDictionaryConverter.ConvertBack(dispatch.DataLayer));
            }
            else
            {
                nativeDispatch = new Com.Tealium.Dispatcher.TealiumEvent(dispatch.Type, JavaDictionaryToClrDictionaryConverter.ConvertBack(dispatch.DataLayer));
            }

            if (nativeDispatch != null)
            {
                tealium.Track(nativeDispatch);
            }
        }

        #endregion Tracking

        #region Remote commands

        object commandsSync = new object();
        Dictionary<string, IRemoteCommand> commands = new Dictionary<string, IRemoteCommand>(2);
        Dictionary<string, RemoteCommandDroid> commandsDroid = new Dictionary<string, RemoteCommandDroid>(2);

        public void AddRemoteCommand(IRemoteCommand remoteCommand)
        {
            if (disposed)
            {
                return;
            }
            lock (commandsSync)
            {
                if (!commands.ContainsKey(remoteCommand.CommandId))
                {
                    commands.Add(remoteCommand.CommandId, remoteCommand);
                    var droidCommand = new RemoteCommandDroid(remoteCommand);
                    commandsDroid.Add(remoteCommand.CommandId, droidCommand);

                    var remoteCommandDispatcher = Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcherKt.GetRemoteCommands(tealium);
                    remoteCommandDispatcher?.Add(droidCommand, remoteCommand.Path, remoteCommand.Url);
                }
            }
        }

        void ITealium.RemoveRemoteCommand(string id)
        {
            if (disposed)
            {
                return;
            }
            lock (commandsSync)
            {
                if (commands.ContainsKey(id))
                {
                    var command = commands[id];
                    commands.Remove(id);
                    command.Dispose();
                    var droidCommand = commandsDroid[id];
                    commandsDroid.Remove(id);

                    var remoteCommandDispatcher = Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcherKt.GetRemoteCommands(tealium);
                    remoteCommandDispatcher?.Remove(id);

                    droidCommand.Dispose(); //just in case!
                }
            }
        }

        #endregion Remote commands


        #region Consent Manager

        public ConsentManager ConsentManager { get => this.consentManager; }

        public void SetConsentStatus(ConsentManager.ConsentStatus status)
        {
            if (disposed)
            {
                return;
            }

            consentManager.UserConsentStatus = status;
        }

        public ConsentManager.ConsentStatus GetConsentStatus()
        {
            if (disposed)
            {
                return ConsentManager.ConsentStatus.Unknown;
            }

            return consentManager.UserConsentStatus;
        }

        public void SetConsentCategories(List<ConsentManager.ConsentCategory> categories)
        {
            if (disposed)
            {
                return;
            }

            consentManager.UserConsentCategories = categories.ToArray();
        }

        public List<ConsentManager.ConsentCategory> GetConsentCategories()
        {
            if (disposed)
            {
                return ConsentManager.NoCategories.ToList();
            }

            return consentManager.UserConsentCategories.ToList();
        }

        #endregion Consent Manager


        #region Helper methods

        public void AddToDataLayer(IDictionary<string, object> data, Expiry expiry)
        {
            if (disposed)
            {
                return;
            }

            var dataLayer = tealium.DataLayer;
            if (dataLayer == null) return;

            var nativeExpiry = expiry.ToNativeExpiry();

            foreach (KeyValuePair<string, object> pair in data)
            {
                if (pair.Key == null || pair.Value == null) continue;

                if (pair.Value is string)
                {
                    dataLayer.PutString(pair.Key, (string)pair.Value, nativeExpiry);
                }
                else if (pair.Value is char)
                {
                    dataLayer.PutString(pair.Key, ((char)pair.Value).ToString(), nativeExpiry);
                }
                else if (pair.Value is bool)
                {
                    dataLayer.PutBoolean(pair.Key, (bool)pair.Value, nativeExpiry);
                }
                else if (pair.Value is int)
                {
                    dataLayer.PutInt(pair.Key, (int)pair.Value, nativeExpiry);
                }
                else if (pair.Value is long)
                {
                    dataLayer.PutLong(pair.Key, (long)pair.Value, nativeExpiry);
                }
                else if (pair.Value is double)
                {
                    dataLayer.PutDouble(pair.Key, (double)pair.Value, nativeExpiry);
                }
                else if (pair.Value is float)
                {
                    dataLayer.PutDouble(pair.Key, (float)pair.Value, nativeExpiry);
                }
                else if (pair.Value is ICollection<string>)
                {
                    dataLayer.PutStringArray(pair.Key, ((ICollection<string>)pair.Value).ToArray(), nativeExpiry);
                }
                else if (pair.Value is ICollection<int>)
                {
                    var collection = (ICollection<int>)pair.Value;
                    dataLayer.PutIntArray(pair.Key, collection.Select(i => new Java.Lang.Integer(i)).ToArray(), nativeExpiry);
                }
                else if (pair.Value is ICollection<long>)
                {
                    var collection = (ICollection<long>)pair.Value;
                    dataLayer.PutLongArray(pair.Key, collection.Select(i => new Java.Lang.Long(i)).ToArray(), nativeExpiry);
                }
                else if (pair.Value is ICollection<double>)
                {
                    var collection = (ICollection<double>)pair.Value;
                    dataLayer.PutDoubleArray(pair.Key, collection.Select(i => new Java.Lang.Double(i)).ToArray(), nativeExpiry);
                }
                else if (pair.Value is ICollection<bool>)
                {
                    var collection = (ICollection<bool>)pair.Value;
                    dataLayer.PutBooleanArray(pair.Key, collection.Select(i => new Java.Lang.Boolean(i)).ToArray(), nativeExpiry);
                }
                else if (pair.Value is IDictionary)
                {
                    Org.Json.JSONObject jsonObject = JavaToClrConverter.ConvertBack(pair.Value) as Org.Json.JSONObject;
                    if (jsonObject != null)
                    {
                        dataLayer.PutJsonObject(pair.Key, jsonObject, nativeExpiry);
                    }
                }
                else if (pair.Value is ICollection)
                {
                    var collection = (ICollection)pair.Value;
                    dataLayer.PutStringArray(pair.Key, collection.Cast<object>().Select(i => i.ToString()).ToArray(), nativeExpiry);
                }
            }
        }

        public object GetFromDataLayer(string id)
        {
            if (disposed)
            {
                return null;
            }

            return JavaToClrConverter.Convert(tealium.DataLayer.Get(id));
        }

        public void RemoveFromDataLayer(ICollection<string> keys)
        {
            if (disposed)
            {
                return;
            }

            foreach (string key in keys)
            {
                if (key != null)
                {
                    tealium.DataLayer.Remove(key);
                }
            }
        }

        public void JoinTrace(string id)
        {
            if (disposed)
            {
                return;
            }

            tealium.JoinTrace(id);
        }

        public void LeaveTrace()
        {
            if (disposed)
            {
                return;
            }

            tealium.LeaveTrace();
        }

        public string GetVisitorId()
        {
            if (disposed)
            {
                return null;
            }

            return tealium.VisitorId;
        }

        readonly object visitorServiceListenersSync = new object();
        readonly KeyedCollection<NativeVisitorServiceListener> visitorServiceListeners = new KeyedCollection<NativeVisitorServiceListener>();

        public CollectionSpecificKey<NativeVisitorServiceListener> AddVisitorServiceListener(Action<IVisitorProfile> callback)
        {
            if (disposed)
            {
                return null;
            }
            CollectionSpecificKey<NativeVisitorServiceListener> key;
            lock (visitorServiceListenersSync)
            {
                var listener = new NativeVisitorServiceListener(callback);
                key = visitorServiceListeners.Add(listener);
                tealium.Events.Subscribe(listener);
            }
            return key;
        }

        public void RemoveVisitorServiceListener(CollectionSpecificKey<NativeVisitorServiceListener> key)
        {
            if (disposed)
            {
                return;
            }
            lock (visitorServiceListenersSync)
            {
                var listener = visitorServiceListeners.Get(key);
                _ = visitorServiceListeners.Remove(key);
                tealium.Events.Unsubscribe(listener);
            }
        }

        readonly object visitorIdListenersSync = new object();
        readonly KeyedCollection<NativeVisitorIdListener> visitorIdListeners = new KeyedCollection<NativeVisitorIdListener>();

        public CollectionSpecificKey<NativeVisitorIdListener> AddVisitorIdListener(Action<string> callback)
        {
            if (disposed)
            {
                return null;
            }
            CollectionSpecificKey<NativeVisitorIdListener> key;
            lock (visitorIdListenersSync)
            {
                var listener = new NativeVisitorIdListener(callback);
                key = visitorIdListeners.Add(listener);
                tealium.Events.Subscribe(listener);
            }
            return key;
        }

        public void RemoveVisitorIdListener(CollectionSpecificKey<NativeVisitorIdListener> key)
        {
            if (disposed)
            {
                return;
            }
            lock (visitorIdListenersSync)
            {
                var listener = visitorIdListeners.Get(key);
                _ = visitorIdListeners.Remove(key);
                tealium.Events.Unsubscribe(listener);
            }
        }

        readonly object consentListenersSync = new object();
        readonly KeyedCollection<NativeConsentExpiryListener> consentExpiryListeners = new KeyedCollection<NativeConsentExpiryListener>();

        public CollectionSpecificKey<NativeConsentExpiryListener> AddConsentExpiryListener(Action callback)
        {
            if (disposed)
            {
                return null;
            }
            CollectionSpecificKey<NativeConsentExpiryListener> key;
            lock (consentListenersSync)
            {
                var listener = new NativeConsentExpiryListener(callback);
                key = consentExpiryListeners.Add(listener);
                tealium.Events.Subscribe(listener);
            }
            return key;
        }

        public void RemoveConsentExpiryListener(CollectionSpecificKey<NativeConsentExpiryListener> key)
        {
            if (disposed)
            {
                return;
            }
            lock (consentListenersSync)
            {
                var listener = consentExpiryListeners.Get(key);
                _ = consentExpiryListeners.Remove(key);
                tealium.Events.Unsubscribe(listener);
            }
        }

        AnyCollectionKey ITealium.AddVisitorServiceListener(Action<IVisitorProfile> callback)
        {
            return AddVisitorServiceListener(callback);
        }

        void ITealium.RemoveVisitorServiceListener(AnyCollectionKey key)
        {
            RemoveVisitorServiceListener((CollectionSpecificKey<NativeVisitorServiceListener>)key);
        }

        AnyCollectionKey ITealium.AddVisitorIdListener(Action<string> callback)
        {
            return AddVisitorIdListener(callback);
        }

        void ITealium.RemoveVisitorIdListener(AnyCollectionKey key)
        {
            RemoveVisitorIdListener((CollectionSpecificKey<NativeVisitorIdListener>)key);
        }

        AnyCollectionKey ITealium.AddConsentExpiryListener(Action callback)
        {
            return AddConsentExpiryListener(callback);
        }

        void ITealium.RemoveConsentExpiryListener(AnyCollectionKey key)
        {
            RemoveConsentExpiryListener((CollectionSpecificKey<NativeConsentExpiryListener>)key);
        }

        public void ClearStoredVisitorIds()
        {
            tealium.ClearStoredVisitorIds();
        }

        public void ResetVisitorId()
        {
            tealium.ResetVisitorId();
        }

        #endregion Helper methods
    }
}