using System;
using System.Collections.Generic;

using Tealium;
using Tealium.iOS;

namespace ExampleAppConsentMgr.iOS
{
    /// <summary>
    /// This class demonstrates how to configure the Tealium API for using
    /// <see cref="TealiumInstanceManager"/> and how to acquire a 
    /// Tealium instance configured to automatically track application
    /// lifecycle, how to add a remote command, how to configure data sources,
    /// how to create dispatch validator and event listeners.
    /// </summary>
    public class TealiumHelper
    {
        public static ITealiumInstanceManager instanceManager;

        static bool initialized = false;

        public static ITealium GetTealium()
        {
            if (!initialized)
            {
                initialized = true;

                //***********************************************************
                // 1. Mandatory configuration - this is platform specific
                //***********************************************************

                instanceManager = new TealiumInstanceManager(new TealiumInstanceFactoryIOS());
                TealiumConsts.InstanceManager = instanceManager;

                // provide the Tealium automatic lifecycle tracking delegate - without this TealiumInstanceFactoryIOS won't
                // be able to set automatic lifecycle tracking (not set will result in null and not being able to track lifecycle)
                //TealiumLifecycleManager.SetLifecycleAutoTracking = TealiumIOS.Lifecycle.TealiumLifecycleControlDelegation.SetLifecycleAutoTracking;


                //***********************************************************
                // 2. Optional configuration - this is cross-platform
                //***********************************************************

                // the below two steps are optional, but they demonstrate
                // how to use remote commands, dispatch validators and event listeners
                var commands = SetupRemoteCommands();
                TealiumAdvancedConfig advancedConfig = SetupAdvancedConfig();


                //***********************************************************
                // 3. Final configuration and Tealium instance creation - this is also cross-platform
                //***********************************************************

                TealiumConfig config = new TealiumConfig(TealiumConsts.InstanceId,
                                                         TealiumConsts.AccountName,
                                                         TealiumConsts.ProfileName,
                                                         TealiumConsts.Environment,
                                                         false,
                                                         commands,
                                                         advancedConfig
                                                         );
                // Enable Consent Management
                config.IsConsentManagerEnabled = true;
                // Optionally set the initial consent status/categories.
                //config.InitialUserConsentStatus = ConsentManager.ConsentStatus.Consented;
                //config.InitialUserConsentCategories = ConsentManager.AllCategories;

                var tealium = instanceManager.CreateInstance(config);


                //***********************************************************
                // 4. Optionally add data sources - this is cross-platform too
                //***********************************************************

                SetupDataSources(tealium);
            }

            return instanceManager.GetExistingInstance(TealiumConsts.InstanceId);
        }

        static void SetupDataSources(ITealium tealium)
        {
            tealium.AddVolatileDataSources(new Dictionary<string, object>(1)
            {
                { "VolatileDataTest", "Example volatile value." }
            });

            tealium.AddPersistentDataSources(new Dictionary<string, object>(1)
            {
                { "PersistentDataTest", "Example persistent value." }
            });
        }

        static TealiumAdvancedConfig SetupAdvancedConfig()
        {
            DelegateDispatchValidator validator = new DelegateDispatchValidator()
            {
                ShouldDropDispatchDelegate = (ITealium arg1, IDispatch arg2) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside ShouldDropDispatchDelegate!");
                    return false;
                },

                ShouldQueueDispatchDelegate = (ITealium arg1, IDispatch arg2, bool shouldQueue) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside ShouldQueueDispatchDelegate!");
                    return shouldQueue;
                }
            };

            DispatchSentDelegateEventListener sendingListener = new DispatchSentDelegateEventListener()
            {
                DispatchSent = (tealium, dispatch) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside DispatchSent!");
                    dispatch.PutString("KeyAddedBySendListener", "Value added by sending listener.");
                }
            };

            DispatchQueuedDelegateEventListener queuingListener = new DispatchQueuedDelegateEventListener()
            {
                DispatchQueued = (tealium, dispatch) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside DispatchQueued!");
                    dispatch.PutString("KeyAddedByQueuedListener", "Value added by queuing listener.");
                }
            };

            WebViewReadyDelegateEventListener webViewListener = new WebViewReadyDelegateEventListener()
            {
                WebViewReady = (tealium, webView) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside WebViewReady!");
                }
            };

            SettingsPublishedDelegateEventListener settingsListener = new SettingsPublishedDelegateEventListener()
            {
                SettingsPublished = (tealium) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside SettingsPublished!");
                }
            };

            TealiumAdvancedConfig advancedConfig = new TealiumAdvancedConfig(validator,
                                                                        sendingListener,
                                                                        queuingListener,
                                                                        webViewListener,
                                                                        settingsListener);
            return advancedConfig;
        }

        static List<IRemoteCommand> SetupRemoteCommands()
        {
            var command = new DelegateRemoteCommand(TealiumConsts.RemoteCommandId, "Test command " + TealiumConsts.RemoteCommandId)
            {
                HandleResponseDelegate = (DelegateRemoteCommand cmd, IRemoteCommandResponse resp) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Handling command {cmd.CommandId}...");
                    var test_id = resp.Payload.GetValueForKey<string>("test_id");
                    System.Diagnostics.Debug.WriteLine($"Handling command {test_id}...");
                    /*System.Threading.Tasks.Task.Run(() =>
                    {
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new RemoteCommandHandledEvent(DateTime.Now));
                    }
                    );
                    */
                }
            };

            return new List<IRemoteCommand>() { command };
        }
    }
}
