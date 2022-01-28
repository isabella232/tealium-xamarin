using System;
using Tealium.Platform.iOS;
using UIKit;
using Tealium.iOS.NativeInterop.Extensions;

namespace Tealium.iOS
{
    public class TealiumInstanceFactoryIOS : ITealiumInstanceFactory
    {
        // TODO: Probably just want to use the setter to update the view value on all the tealium instances?
        public UIView View { get; set; }

        public ITealium CreateInstance(TealiumConfig config, Action<ITealium> ready)
        {

            TealiumConfigWrapper nativeConfig = config.ToNativeConfig();
            // TODO - this may need to be editable on the nativeConfig... which we don't currently maintain an accessible reference to.
            if (View != null)
            {
                nativeConfig.RootView = View;
            }

            TealiumIOSImpl tealium = null;

            tealium = new TealiumIOSImpl(new TealiumWrapper(nativeConfig, (success, error) =>
            {
                if (success)
                {
                    OnReady(tealium, config);
                    ready?.Invoke(tealium);
                }
                else
                {
                    ready?.Invoke(null);
                }
            }));
            return tealium;
        }

        private static void OnReady(ITealium tealium, TealiumConfig config)
        {
            // Set up remote commands
            if (config.Dispatchers.Contains(Dispatchers.RemoteCommands) && config.RemoteCommands != null)
            {
                foreach (IRemoteCommand cmd in config.RemoteCommands)
                {
                    tealium.AddRemoteCommand(cmd);
                }
            }
        }

        public ITealium CreateInstance(TealiumConfig config)
        {
            return CreateInstance(config, null);
        }
    }
}
