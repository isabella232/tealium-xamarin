using System;
using System.Collections.Generic;
using System.Linq;
using Tealium.Droid.NativeInterop.Extensions;

namespace Tealium.Droid
{
    public class TealiumInstanceFactoryDroid : ITealiumInstanceFactory
    {
        readonly Android.App.Application application;

        public TealiumInstanceFactoryDroid(Android.App.Application application)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
        }

        public ITealium CreateInstance(TealiumConfig config)
        {
            return CreateInstance(config, null);
        }

        public ITealium CreateInstance(TealiumConfig config, Action<ITealium> ready)
        {
            Com.Tealium.Core.TealiumConfig nativeConfig = config.ToNativeConfig(application);

            OnTealiumReady onReady = new OnTealiumReady(config, ready);
            var tealium = Com.Tealium.Core.Tealium.CompanionInstance.Create(config.InstanceId, nativeConfig, onReady);
            var tealiumDroid = new TealiumDroidImpl(tealium, config.InstanceId);
            // Native and CLR implementations are made independantly
            // but the callback needs the CLR implementation.
            onReady.TealiumDroid = tealiumDroid;

            return tealiumDroid;
        }

        private static void OnReady(ITealium tealiumDroid, TealiumConfig config, Com.Tealium.Core.Tealium nativeTealium)
        {
            // suscribe event listeners
            var eventListeners = config.Listeners.Select(l => l.ToNativeEventListener()).Where(l => l != null).ToList();
            eventListeners.ForEach((listener) =>
            {
                nativeTealium.Events.Subscribe(listener);
            });

            // Set up remote commands
            if (config.Dispatchers.Contains(Dispatchers.RemoteCommands) && config.RemoteCommands != null)
            {
                foreach (IRemoteCommand cmd in config.RemoteCommands)
                {
                    var remoteCommandDroid = new RemoteCommandDroid(cmd);
                    var remoteCommandDispatcher = Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcherKt.GetRemoteCommands(nativeTealium);
                    remoteCommandDispatcher?.Add(remoteCommandDroid, cmd.Path, cmd.Url);
                }
            }
        }

        /// <summary>
        /// Native implementation to handle the on-ready callback for the native
        /// Tealium instance.
        /// </summary>
        private class OnTealiumReady : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        {
            readonly TealiumConfig config;
            readonly Action<ITealium> callback;

            public OnTealiumReady(TealiumConfig config, Action<ITealium> callback)
            {
                this.config = config;
                this.callback = callback;
            }

            public ITealium TealiumDroid { get; set; }

            public Java.Lang.Object Invoke(Java.Lang.Object p0)
            {
                Com.Tealium.Core.Tealium nativeTealium = p0 as Com.Tealium.Core.Tealium;
                if (nativeTealium == null)
                {
                    callback?.Invoke(null);
                }
                else
                {
                    if (config.LogLevel != null)
                    {
                        Com.Tealium.Core.Logger.CompanionInstance.LogLevel = config.LogLevel?.ToNativeLogLevel();
                    }

                    if (TealiumDroid != null)
                    {
                        OnReady(TealiumDroid, config, nativeTealium);
                        callback?.Invoke(TealiumDroid);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Native Tealium ready, but ITealium is not.");
                        callback?.Invoke(null);
                    }
                }

                return null;
            }
        }
    }
}
