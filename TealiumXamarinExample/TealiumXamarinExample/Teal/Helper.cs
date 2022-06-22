using System;
using System.Collections.Generic;
using System.Collections;
using Tealium;

namespace TealiumXamarinExample.Teal
{
    public class Helper
    {
        private static ITealiumInstanceManager InstanceManager { get; set; }

        public static void SetInstanceManager(ITealiumInstanceManager tealiumInstanceManager)
        {
            if (tealiumInstanceManager == null) throw new ArgumentNullException(nameof(tealiumInstanceManager));

            if (InstanceManager != null) return;

            InstanceManager = tealiumInstanceManager;
        }

        public static void Init()
        {
            if (InstanceManager == null) throw new NotSupportedException("An ITealiumInstanceManager is required to be set.");

            var commands = SetupRemoteCommands();

            TealiumConfig config = new TealiumConfig(TealiumConstants.ACCOUNT_NAME,
                                                         TealiumConstants.PROFILE_NAME,
                                                         TealiumConstants.ENVIRONMENT,
                                                         new List<Dispatchers> {
                                                             Dispatchers.Collect, Dispatchers.RemoteCommands//, Dispatchers.TagManagement
                                                         },
                                                         new List<Collectors> {
                                                             Collectors.LifeCycle, Collectors.AppData
                                                         },
                                                         remoteCommands: commands,
                                                         consentPolicy: ConsentManager.ConsentPolicy.CCPA,
                                                         visitorServiceEnabled: true
                                                         );
            config.LogLevel = LogLevel.Dev;
            config.Listeners.Add(new TealiumEventListeners());

            // Optional - DispatchValidator examples
            //DelegateDispatchValidator delegateValidator = new DelegateDispatchValidator("DelegateValidator");
            //delegateValidator.ShouldQueueDispatchDelegate = (dispatch) => { return false; };
            //delegateValidator.ShouldDropDispatchDelegate = (dispatch) => { return false; };
            //config.DispatchValidators.Add(delegateValidator);
            //config.DispatchValidators.Add(new MyDispatchValidator(queue: true));

            InstanceManager.CreateInstance(config, (tealium) =>
            {
                if (tealium != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Tealium initialized sucessfully ({config.Account}/{config.Profile})");

                    tealium.AddVisitorServiceListener((visitorProfile) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Visitor Updated");
                        System.Diagnostics.Debug.WriteLine($"Visitor: {visitorProfile}");
                    });

                    tealium.AddConsentExpiryListener(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("Consent Expired");
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Tealium failed to initialize.");
                }
            });

        }

        public static ITealium DefaultInstance
        {
            get => InstanceManager.GetExistingInstance(TealiumConstants.ACCOUNT_NAME, TealiumConstants.PROFILE_NAME, TealiumConstants.ENVIRONMENT);
        }

        public static List<IRemoteCommand> RemoteCommands = new List<IRemoteCommand>();

        static List<IRemoteCommand> SetupRemoteCommands()
        {
            var command = new DelegateRemoteCommand(TealiumConstants.REMOTE_COMMAND_ID, "Test command " + TealiumConstants.REMOTE_COMMAND_ID)
            {
                HandleResponseDelegate = (DelegateRemoteCommand cmd, IRemoteCommandResponse resp) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Handling command {cmd.CommandId}");
                    System.Diagnostics.Debug.WriteLine($"Command Payload: {resp.Payload}");
                }
            };
            var list = new List<IRemoteCommand>(RemoteCommands);
            list.Add(command);

            return list;
        }
    }

    class TealiumEventListeners : IVisitorUpdatedEventListener, IConsentExpiredEventListener, IDispatchDroppedEventListener, IDispatchQueuedEventListener, ISettingsPublishedEventListener
    {
        public void OnConsentExpired()
        {
            System.Diagnostics.Debug.WriteLine("Consent Expired.");
        }

        public void OnDispatchDropped(Dispatch dispatch)
        {
            System.Diagnostics.Debug.WriteLine("DispatchDropped: ");
            System.Diagnostics.Debug.WriteLine($"Type - {dispatch.Type}");
            System.Diagnostics.Debug.WriteLine($"DataLayer - {dispatch.DataLayer}");
        }

        public void OnDispatchQueued(Dispatch dispatch)
        {
            System.Diagnostics.Debug.WriteLine("DispatchQueued: ");
            System.Diagnostics.Debug.WriteLine($"Type - {dispatch.Type}");
            System.Diagnostics.Debug.WriteLine($"DataLayer - {dispatch.DataLayer}");
        }

        public void OnSettingsPublished()
        {
            System.Diagnostics.Debug.WriteLine("Settings Published");
        }

        public void OnVisitorUpdated(IVisitorProfile visitorProfile)
        {
            System.Diagnostics.Debug.WriteLine("VisitorProfile Updated");
        }
    }

    public class MyDispatchValidator : IDispatchValidator
    {
        public string Name => "MyDispatchValidator";
        private readonly bool drop;
        private readonly bool queue;

        public MyDispatchValidator(bool drop = false, bool queue = false)
        {
            this.drop = drop;
            this.queue = queue;
        }

        public bool ShouldDrop(Dispatch dispatch)
        {
            return drop;
        }

        public bool ShouldQueue(Dispatch dispatch)
        {
            return queue;
        }
    }
}
